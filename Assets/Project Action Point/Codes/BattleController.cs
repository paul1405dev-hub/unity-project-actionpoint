using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    [Header("Scene refs")]
    public Transform battleRoot;
    public Transform spawnPoint;
    public Camera battleCamera;

    [Header("CurMons")]
    private GameObject currentMonsterInstance;
    private Animator currentMonsterAnimator;

    public GameObject Show(MonsterData monsterData)
    {
        if (monsterData == null) 
        {
            Debug.LogError("[BattleController] MonsterData가 null입니다.");
            return null;
        }
        
        if (monsterData.monsterPrefab == null)
        {
            Debug.LogError($"[BattleController] '{monsterData.monsterName}'의 monsterPrefab이 비어있습니다.");
            return null;
        }

        if (currentMonsterInstance != null)
        {
            Destroy(currentMonsterInstance);
            currentMonsterInstance = null;
        }

        Vector3 spawnPosition = (spawnPoint != null) ? spawnPoint.position : Vector3.zero;

        currentMonsterInstance = Instantiate(monsterData.monsterPrefab, spawnPosition, Quaternion.identity, battleRoot);
          
        //(Layer,SortingOrder 보정)
        currentMonsterInstance.layer = LayerMask.NameToLayer("Battle");
        SpriteRenderer spriteRenderer = currentMonsterInstance.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null )
        {
            spriteRenderer.sortingLayerName = "Battle";
            if (spriteRenderer.sortingOrder < 1) spriteRenderer.sortingOrder = 1;
        }

        currentMonsterAnimator = currentMonsterInstance.GetComponent<Animator>();
        if (currentMonsterAnimator != null )
        {
            if (monsterData.animatorOverride != null)
            {
                currentMonsterAnimator.runtimeAnimatorController = monsterData.animatorOverride;

                // 상태 초기화(갱신)  바로 첫 프레임부터 올바른 상태가 나오도록
                currentMonsterAnimator.Rebind();
                currentMonsterAnimator.Update(0f);
                currentMonsterAnimator.Play("Idle", 0 ,0f);
            }

            else
            {
                Debug.LogWarning($"[BattleController] '{monsterData.monsterName}'의 animatorOverride가 비어있습니다. 프리팹 기본 컨트롤러를 사용합니다.");
            }
        }
        else
        {
            Debug.LogWarning($"[BattleController] '{monsterData.monsterName}' 프리팹에 Animator가 없습니다. 애니메이션이 재생되지 않습니다.");
        }
        
        return currentMonsterInstance;
    }

    public void Hide()
    {
        if (currentMonsterInstance != null)
        {
            Destroy (currentMonsterInstance);

            currentMonsterInstance = null;
        }
    }

    public void PlayIdle()
    {
        if (currentMonsterAnimator == null) return;
        currentMonsterAnimator.ResetTrigger("Hit");
        currentMonsterAnimator.ResetTrigger("Dead");
        currentMonsterAnimator.SetTrigger("Idle");
    }

    public void PlayHit()
    {
        if (currentMonsterAnimator == null) return;
        currentMonsterAnimator.ResetTrigger("Idle");
        currentMonsterAnimator.SetTrigger("Hit");
    }

    public void PlayDead()
    {
        if (currentMonsterAnimator == null) return;

        currentMonsterAnimator.ResetTrigger("Hit");
        currentMonsterAnimator.ResetTrigger("Dead");

        currentMonsterAnimator.Play("Dead", 0, 0f);
    }
    public float GetCurrentClipLengthSeconds(float fallback = 0.5f)
    {
        if (currentMonsterAnimator == null)
        {
            return fallback;
        }


        AnimatorClipInfo[] clipInfos = currentMonsterAnimator.GetCurrentAnimatorClipInfo(0);
        if (clipInfos != null && clipInfos.Length > 0 && clipInfos[0].clip != null)
        {
            float speed = currentMonsterAnimator.GetCurrentAnimatorStateInfo(0).speed;
            if (speed <= 0f) speed = 1f;
            return clipInfos[0].clip.length / speed;
        }
        return fallback;

    }


}
