using UnityEngine;
using UnityEngine.UI;

public class GameViewManager : MonoBehaviour
{
    [Header("Cameras")]
    public Camera mainCamera;
    public Camera battleCamera;

    [Header("Canvases")]
    public GameObject mainHudCanvas;
    public GameObject overlayCanvas;

    [Header("Overlay Panel")]
    public GameObject battleResultRoot;
    public GameObject winPanel;
    public GameObject losePanel;


    [Header("Battle Result")]
    public Text DayText;
    public Text EquipCraftText;
    public Text RegenText;

    public Text DayTextL;
    public Text EquipCraftTextL;
    public Text RegenTextL;

    public void EnterBattleView()
    {
        if (mainCamera != null) mainCamera.gameObject.SetActive(false);
        if (battleCamera != null) battleCamera.gameObject.SetActive(true);

        if (mainHudCanvas != null) mainHudCanvas.SetActive(false);
        if (winPanel != null) winPanel.gameObject.SetActive(false);
        if (losePanel != null) losePanel.gameObject.SetActive(false);

    }

    public void ExitBattleView()
    {
        if (battleCamera != null) battleCamera.gameObject.SetActive(false);
        if (mainCamera != null) mainCamera.gameObject.SetActive(true);

        //if (battleUiCanvas != null) battleUiCanvas.SetActive(false);
        if (mainHudCanvas != null) mainHudCanvas.SetActive(true);

    }

    public void ShowWin()
    {
        if (battleResultRoot != null) battleResultRoot.SetActive(true);

        if (winPanel != null) winPanel.SetActive(true);
        if (losePanel != null) losePanel.SetActive(false);

        DayText.text = string.Format("{0}일 만의 승리", GameManager.instance.day);
        EquipCraftText.text = string.Format("장비 제작 횟수 : {0} 번", GameManager.instance.actionManager.CountBuildAction);
        RegenText.text = string.Format("체력 회복 횟수 : {0}번", GameManager.instance.actionManager.CountRegenAction);


        
    }

    public void ShowLose()
    {
        if (battleResultRoot != null) battleResultRoot.SetActive(true);

        if (losePanel != null) losePanel.SetActive(true);
        if (winPanel != null) winPanel.SetActive(false);

        DayTextL.text = string.Format("Day{0} 에서 패배..", GameManager.instance.day);
        EquipCraftTextL.text = string.Format("장비 제작 횟수 : {0} 번", GameManager.instance.actionManager.CountBuildAction);
        RegenTextL.text = string.Format("체력 회복 횟수 : {0}번", GameManager.instance.actionManager.CountRegenAction);
    }

    public void Return()
    {
        battleResultRoot.SetActive(false);
        if (battleResultRoot != null) battleResultRoot.SetActive(false);

    }
}
