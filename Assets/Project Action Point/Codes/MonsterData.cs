using UnityEngine;


[CreateAssetMenu(fileName = "Monster", menuName = "Scriptble Object/MonsterData")]
public class MonsterData : ScriptableObject
{
    [Header("# Main Info")]
    public string monsterName;
    public bool isBoss = false;

    [Header("# Display")]
    public GameObject monsterPrefab;
    public AnimatorOverrideController animatorOverride;


    [Header("# Stats")]
    public float maxHealth;
    public float attackPower;
    public float defensePower;

    [Header("# Logic")]
    public int floorToAppear;
}
        