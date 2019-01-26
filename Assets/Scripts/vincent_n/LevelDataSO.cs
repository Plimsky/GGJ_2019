using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Levels", order = 1)]
public class LevelDataSO : ScriptableObject
{
    public int m_totalFragments;
    public int m_minimalFragments;

    public int m_totalBatteryPacks;

    public int m_totalEnemies;
}