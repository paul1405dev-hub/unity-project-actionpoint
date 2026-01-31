using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float MaxHealth = 100;
    public float CurHealth;
    public float AttackPow = 10;
    public float DefensePow = 10;

    [Header("# Combat Data")]
    public MonsterData[] allMonsters;

    // 전투 동시 실행 방지 플래그
    private bool isBattleRunning = false;

    // 전투 화면 최소 노출 시간(초) – 한 방 처치여도 이 시간만큼은 보이게
    private const float MinBattleShowSeconds = 0.30f;

    // 전투 결과(승/패) 화면 유지 시간(초)
    private const float ResultHoldSeconds = 0.50f;

    void Awake()
    {
        CurHealth = MaxHealth;
    }

    public void ApplyActionEffect(ActionManager.ActionType type)
    {
        switch (type)
        {
            case ActionManager.ActionType.BuildItem:
                GameManager.instance.craftingManager.CraftItem();
                break;

            case ActionManager.ActionType.RegenHealth:
                CurHealth = Mathf.Min(MaxHealth, CurHealth + 10);
                break;

            case ActionManager.ActionType.Battle:
                if (isBattleRunning)
                {
                    Debug.LogWarning("[전투] 이미 전투가 진행 중입니다.");
                    return;
                }
                StartCoroutine(SimulateBattleRoutine());
                break;
        }
    }

    private IEnumerator SimulateBattleRoutine()
    {
        isBattleRunning = true;

        GameManager.instance.floor++;
        int currentFloor = GameManager.instance.floor;

        // 몬스터 필터링
        MonsterData[] availableMonsters;
        if (currentFloor == 10)
            availableMonsters = allMonsters.Where(m => m.isBoss).ToArray();
        else
            availableMonsters = allMonsters.Where(m => m.floorToAppear == currentFloor).ToArray();

        // 데이터 누락
        if (availableMonsters.Length == 0)
        {
            GameManager.instance.floor--;
            Debug.LogError($"⚠️ {currentFloor}층에 몬스터 데이터가 없습니다.");
            isBattleRunning = false;
            yield break;
        }

        MonsterData selectedMonster = availableMonsters[Random.Range(0, availableMonsters.Length)];
        if (selectedMonster == null || selectedMonster.monsterPrefab == null)
        {
            GameManager.instance.floor--;
            Debug.LogError($"⚠️ 몬스터 프리팹이 비어있습니다. ({selectedMonster?.monsterName ?? "Unknown"})");
            isBattleRunning = false;
            yield break;
        }

        // 전투 화면 전환 + 몬스터 표시
        if (GameManager.instance.gameViewManager != null)
        {
            GameManager.instance.gameViewManager.EnterBattleView();
        }
        else
        {
            Debug.LogWarning("[전투] GameViewController가 연결되어 있지 않습니다. 화면 전환 없이 진행합니다.");
        }

        if (GameManager.instance.battleController != null)
        {
            GameManager.instance.battleController.Show(selectedMonster);
        }
        else
        {
            Debug.LogError("[전투 실패] BattleController가 GameManager에 연결되어 있지 않습니다.");
            if (GameManager.instance.gameViewManager != null)
            {
                GameManager.instance.gameViewManager.ExitBattleView();
            }
            GameManager.instance.floor--;
            isBattleRunning = false;
            yield break;
        }

        // —— 렌더 안정화 + 최소 노출 시간 확보 ——
        yield return null; // 1 프레임 대기: Instantiate/카메라 토글 반영
        yield return new WaitForSeconds(MinBattleShowSeconds);

        // 5) 전투 시뮬레이션(턴 연출 포함)
        float playerDamagePerHit = Mathf.Max(1, AttackPow - selectedMonster.defensePower);
        float monsterDamagePerHit = Mathf.Max(1, selectedMonster.attackPower - DefensePow);
        float monsterCurHealth = selectedMonster.maxHealth;

        while (monsterCurHealth > 0 && CurHealth > 0)
        {
            // 플레이어 공격
            monsterCurHealth -= playerDamagePerHit;
            GameManager.instance.battleController.PlayHit();
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);

            // 즉시 처치되어도 최소 한 번은 화면을 봤으므로 바로 정리 가능
            if (monsterCurHealth <= 0)
                break;

            // 몬스터 반격
            CurHealth -= monsterDamagePerHit;

            // 한 스텝 연출 시간
            yield return new WaitForSeconds(1.0f);
        }

        // 승패 처리
        bool isPlayerWin = monsterCurHealth <= 0;
        if (isPlayerWin)
        {
            // Dead 트리거 → 죽음 연출 동안 대기
            GameManager.instance.battleController.PlayDead();
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);

            yield return null;

            float deadClipSeconds = GameManager.instance.battleController.GetCurrentClipLengthSeconds(0.6f);
            Debug.Log($"Dead clip seconds = {deadClipSeconds}");
            yield return new WaitForSeconds(deadClipSeconds);

            if (selectedMonster.isBoss && currentFloor == 10)
            {
                GameManager.instance.WinGame();
            }
            else
            {
                Debug.Log($"🗡 {selectedMonster.monsterName} 처치! {currentFloor}층 클리어");
            }
        }
        else // 패배
        {
            CurHealth = 0;
            yield return new WaitForSeconds(ResultHoldSeconds);
            GameManager.instance.battleController.Hide();
            if (GameManager.instance.gameViewManager != null)
                GameManager.instance.gameViewManager.ExitBattleView();
            GameManager.instance.GameOver(false);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
            AudioManager.instance.PlayBgm(false);
        }

        // 7) 결과 화면 잠깐 유지 후 정리
        yield return new WaitForSeconds(ResultHoldSeconds);

        // 몬스터 오브젝트 제거
        GameManager.instance.battleController.Hide();

        // 전투 화면 종료(카메라/캔버스 복구)
        if (GameManager.instance.gameViewManager != null)
        {
            GameManager.instance.gameViewManager.ExitBattleView();
        }

        isBattleRunning = false;
    }
}



