using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    public Image[] actionIcon;

    [Header("Floating Text Setup")]
    public GameObject floatingTextPrefab;
    public Transform actionPointLackTextSpawnPoint;

    [Header("WarningPanels")]
    public GameObject SleepWarningPanel;
    public GameObject ClimbWarningPanel;

    public int CountBuildAction = 0;
    public int CountRegenAction = 0;

    public int MaxActionPoint = 3;
    public int CurActionPoint;


    public enum ActionType {Battle, BuildItem, RegenHealth, Sleep}


    void Awake()
    {
        CurActionPoint = MaxActionPoint;
        
        UpdateActionUI();                                                                                                   
    }

 
    void UpdateActionUI()
    {
        int inactiveCount = MaxActionPoint - CurActionPoint;

        for (int index = 0; index < actionIcon.Length; index++)
        {
            actionIcon[index].gameObject.SetActive(index >= inactiveCount);
        }
    }
    private void TakeAction(ActionType type)
    {
        if (CurActionPoint <= 0)
        {
            ShowFloatingText("행동 포인트 부족!", actionPointLackTextSpawnPoint);
            return;
        }
        CurActionPoint--;
        UpdateActionUI();
        if (GameManager.instance.playerStats != null)
        {
            GameManager.instance.playerStats.ApplyActionEffect(type);
        }
    }

    private void ShowFloatingText(string message, Transform targetTransform)
    {
        if (floatingTextPrefab == null || targetTransform == null)
        {
            Debug.LogError("FloatingText 프리팹 또는 Root가 연결되지 않았습니다.");
            return;
        }

        GameObject newTextGO = Instantiate(floatingTextPrefab, targetTransform, false);

        Text textComp = newTextGO.GetComponent<Text>();
        if (textComp != null)
        {
            textComp.text = message;
        }

        RectTransform rt = newTextGO.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchoredPosition = Vector2.zero;
        }
    }
    public void BuildItemAction()
    {
        TakeAction(ActionType.BuildItem);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Craft);
        CountBuildAction++;
    }
    public void RegenHealthAction()
    {
        TakeAction(ActionType.RegenHealth);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Health);
        CountRegenAction++;
    }
    public void TowerClimbAction()
    {
        PlayerStats stats = GameManager.instance.playerStats;

        if (stats.CurHealth < (stats.MaxHealth * 0.5f))
        {
            ClimbWarningPanel.SetActive(true);
        }
        else
        {
            TakeAction(ActionType.Battle);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Climb);
        }
            
    }
    public void SleepAction()
    {
        if (CurActionPoint >= 1)
        {
            SleepWarningPanel.SetActive(true);
        }
        else
        {
            CurActionPoint = MaxActionPoint;
            UpdateActionUI();
            GameManager.instance.NextDay();
        }
    }
    public void Yes_Sleep()
    {
        CurActionPoint = MaxActionPoint;
        UpdateActionUI();
        GameManager.instance.NextDay();
        SleepWarningPanel.SetActive(false);
    }
    public void Return_Sleep()
    {
        SleepWarningPanel.SetActive(false);
    }


    public void Yes_Climb()
    {
        TakeAction(ActionType.Battle);
        ClimbWarningPanel.SetActive(false);
    }
    public void Return_Climb()
    {
        ClimbWarningPanel.SetActive(false);
    }

}
