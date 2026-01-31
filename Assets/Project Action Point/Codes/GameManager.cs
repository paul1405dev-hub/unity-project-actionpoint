using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ActionManager actionManager;
    public PlayerStats playerStats;
    public Equipment equipment;
    public CraftingManager craftingManager;

    public BattleController battleController;
    public GameViewManager gameViewManager;

    [Header("#Day Control")]
    public int day = 1;
    public int floor = 0;

    void Awake()
    {
        // ⭐ 싱글톤 패턴: 단일 인스턴스 보장
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 이미 존재하면 자신을 파괴
        }

        AudioManager.instance.PlayBgm(true);
    }

    public void NextDay()
    {
        day++;

        if (day > 20)
        {
            GameOver(false);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
            AudioManager.instance.PlayBgm(false);
        }
    }
    public void WinGame()
    {
        GameOver(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
        AudioManager.instance.PlayBgm(false);
    }
    public void GameOver(bool win) // bool win은 win만 true 값을 가지게 하는 건가?
    {
        if (win)
        {
            gameViewManager.ShowWin();
        }
        else
        {
            gameViewManager.ShowLose();  
        }

    }


}
