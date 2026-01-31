using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UnityEngine.UI.Text를 사용하기 위해 필요

public class CraftingManager : MonoBehaviour
{
    // 관리자 연결
    public Equipment equipmentManager;

    // 아이템 템플릿
    public ItemData[] itemTemplates;
    private ItemData newlyCraftedItem; // 제작된 아이템을 임시 저장 (ScriptableObject 복제본)


    [Header("UI Elements")]
    public GameObject comparisonPanel; // 장비 비교 UI 전체 패널
    public GameObject warningPanel;    // 등급 하락 시 경고 UI 패널
    // 새 아이템 UI
    public Text newItemNameText;
    public Text newItemStatsText;
    public Text newItemQualityText;
    public Image newItemIconImage;
    // 현재 장착 아이템 UI
    public Text currentItemNameText;
    public Text currentItemStatsText;
    public Text currentItemQualityText;
    public Image currentItemIconImage;

    [Header("Quality Colors")]
    public Color normalColor = Color.gray; // 기본 회색
    public Color rareColor = Color.blue;   // 파랑색
    public Color epicColor = new Color(0.5f, 0f, 0.5f); // 보라색 (RGB: 128, 0, 128)

    [System.Serializable]
    public struct QualityProbability
    {
        public int floorThreshold; // 이 층수 이상에서 적용
        [Range(0, 100)] public int normalChance; // 일반 등급 확률 (0~100)
        [Range(0, 100)] public int rareChance;   // 희귀 등급 확률 (0~100)
    }
    // 에디터에서 설정할 확률 테이블
    public QualityProbability[] probabilityTable;


    public void CraftItem()
    {
        // A. 현재 층수에 맞는 확률 테이블 찾기
        QualityProbability currentProb = probabilityTable[0];
        int currentFloor = GameManager.instance.floor;

        foreach (QualityProbability prob in probabilityTable)
        {
            if (currentFloor >= prob.floorThreshold)
            {
                currentProb = prob;
            }
            else
            {
                break;
            }
        }

        // B. 등급 무작위 결정 (가중치 적용)
        ItemData.ItemQuality decidedQuality;
        int roll = Random.Range(0, 100);

        if (roll < currentProb.normalChance)
        {
            decidedQuality = ItemData.ItemQuality.Normal;
        }
        else if (roll < currentProb.normalChance + currentProb.rareChance)
        {
            decidedQuality = ItemData.ItemQuality.Rare;
        }
        else
        {
            decidedQuality = ItemData.ItemQuality.Epic;
        }

        // C. 아이템 필터링 및 최종 선택
        List<ItemData> filteredByQuality = new List<ItemData>();
        foreach (var item in itemTemplates)
        {
            if (item.quality == decidedQuality)
            {
                filteredByQuality.Add(item);
            }
        }

        if (filteredByQuality.Count == 0)
        {
            return;
        }

        // 무기(0) 또는 방패(1) 슬롯 무작위 결정
        ItemData.SlotType decidedSlot = (ItemData.SlotType)Random.Range(0, 2);

        List<ItemData> finalCandidates = new List<ItemData>();
        foreach (var item in filteredByQuality)
        {
            if (item.slotType == decidedSlot)
            {
                finalCandidates.Add(item);
            }
        }

        if (finalCandidates.Count == 0)
        {
            return;
        }

        // 최종 아이템 선택 및 복제 (Instantiate)
        ItemData selectedTemplate = finalCandidates[Random.Range(0, finalCandidates.Count)];

        newlyCraftedItem = Instantiate(selectedTemplate);
        newlyCraftedItem.name = newlyCraftedItem.itemName; // 이름에서 (Clone) 제거

        // D. UI 표시
        ShowComparisonUI(newlyCraftedItem);
    }

    private void ShowComparisonUI(ItemData newItem)
    {
        comparisonPanel.SetActive(true);

        // 1. 새 아이템 정보 표시
        newItemIconImage.sprite = newItem.itemIcon;
        newItemNameText.text = newItem.itemName;
        newItemStatsText.text = $"공격력: {newItem.baseAttack}\n방어력: {newItem.baseDefense}";
        newItemQualityText.text = $"{newItem.quality}";
        newItemQualityText.color = GetQualityColord(newItem.quality);
        

        // 2. 현재 장비 정보 찾기
        ItemData currentItem;
        if (newItem.slotType == ItemData.SlotType.Weapon)
        {
            currentItem = equipmentManager.currentWeapon;
        }
        else
        {
            currentItem = equipmentManager.currentShield;
        }

        // 3. 현재 아이템 정보 표시
        if (currentItem != null)
        {
            currentItemIconImage.sprite = currentItem.itemIcon;
            currentItemNameText.text = currentItem.itemName;
            currentItemQualityText.text = $"{currentItem.quality}";
            currentItemQualityText.color = GetQualityColord(currentItem.quality);
            currentItemStatsText.text = $"공격력: {currentItem.baseAttack}\n방어력: {currentItem.baseDefense}";
        }
        else
        {
            currentItemNameText.text = "현재 장비 없음";
            currentItemQualityText.text = "희귀도: 없음";
            currentItemStatsText.text = "공격력: 0\n방어력: 0";
        }
    }

    // '장착' 버튼 클릭 시 호출
    public void EquipItem()
    {
        // 1. 현재 장착 아이템 가져오기
        ItemData currentItem;
        if (newlyCraftedItem.slotType == ItemData.SlotType.Weapon)
        {
            currentItem = equipmentManager.currentWeapon;
        }
        else
        {
            currentItem = equipmentManager.currentShield;
        }

        // 2. 등급 비교 (현재 등급 > 새 등급이면 경고)
        // (int) 변환을 통해 enum의 순서(0, 1, 2...)를 비교
        if (currentItem != null && (int)currentItem.quality > (int)newlyCraftedItem.quality)
        {
            warningPanel.SetActive(true);
        }
        else
        {
            // 바로 장착
            equipmentManager.Equip(newlyCraftedItem);
            comparisonPanel.SetActive(false);
        }
    }

    // '폐기' 버튼 클릭 시 호출
    public void DiscardItem()
    {
        Destroy(newlyCraftedItem); // 복제된 ScriptableObject는 Destroy로 메모리 해제
        comparisonPanel.SetActive(false);
    }

    // 경고 패널의 '네' (장착 강행) 버튼 클릭 시 호출
    public void yesItem()
    {
        equipmentManager.Equip(newlyCraftedItem);
        warningPanel.SetActive(false);
        comparisonPanel.SetActive(false); // 경고와 비교 패널 모두 닫음
    }

    // 경고 패널의 '아니오' (장착 취소) 버튼 클릭 시 호출
    public void returnItem()
    {
        warningPanel.SetActive(false); // 경고만 닫고, 사용자가 폐기 또는 장착을 다시 선택하도록 비교 패널은 유지
    }

    private Color GetQualityColord(ItemData.ItemQuality quality)
    {
        switch (quality)
        {
            case ItemData.ItemQuality.Normal:
                return normalColor;
            case ItemData.ItemQuality.Rare:
                return rareColor;
            case ItemData.ItemQuality.Epic:
                return epicColor;
            default:
                return Color.white; // 정의되지 않은 경우 기본 색상
        }
    }

}
