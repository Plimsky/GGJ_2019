using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scritpable Objects/Player Data", order = 1)]
public class PlayerDataSO : ScriptableObject
{
    public int m_maxLife = 100;
    public int m_maxBattery = 100;
    public int m_life;
    public int m_battery;
    public int m_fragments;
    public int m_enemyDestroyed;

    public void Reset()
    {
        m_life = m_maxLife;
        m_battery = m_maxBattery;
        m_fragments = 0;
        m_enemyDestroyed = 0;
    }
}