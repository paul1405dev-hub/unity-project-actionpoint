using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType {Day, Health, Attack, Defense, Floor}
    public InfoType type;

    public Text myText;
    Slider mySlider;

    void Awake()
    {
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        // ⭐ Null 체크 추가: GameManager가 준비되었을 때만 UI 업데이트
        if (GameManager.instance == null) return;

        switch (type)
        {
            case InfoType.Day:
                myText.text = string.Format("Day{0}", GameManager.instance.day);
                break;
            case InfoType.Floor:
                myText.text = string.Format("{0:F0}층", GameManager.instance.floor);
                break;
            case InfoType.Health:
                float curHealth = GameManager.instance.playerStats.CurHealth;
                float maxHealth = GameManager.instance.playerStats.MaxHealth;

                mySlider.value = curHealth/maxHealth;
                myText.text = string.Format("{0:F0}/{1:F0}", curHealth, maxHealth);
                break;
            case InfoType.Attack:
                float attack = GameManager.instance.playerStats.AttackPow;
                myText.text = string.Format("공격력:{0:F0}", attack);
                break;
            case InfoType.Defense:
                float defense = GameManager.instance.playerStats.DefensePow;
                myText.text = string.Format("방어력:{0:F0}", defense);
                break;
        }
    }



}
