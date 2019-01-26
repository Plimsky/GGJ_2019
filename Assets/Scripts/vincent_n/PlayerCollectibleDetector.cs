using System;
using Managers;
using UnityEngine;

public class PlayerCollectibleDetector : MonoBehaviour
{
    [SerializeField] private PlayerDataSO m_playerData;
    [SerializeField] private String m_fragmentTag = "Collectibles/Fragment";
    [SerializeField] private String m_batteryTag = "Collectibles/BatteryPack";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(m_fragmentTag))
        {
            m_playerData.m_fragments++;
            GameManager.instance.OnUpdateStats();
            Destroy(other.gameObject);
        }
        else if(other.gameObject.CompareTag(m_batteryTag))
        {
            if (m_playerData.m_battery < m_playerData.m_maxBattery)
            {
                m_playerData.m_battery += 10;
                GameManager.instance.OnUpdateStats();
                Destroy(other.gameObject);
            }

            if (m_playerData.m_battery > m_playerData.m_maxBattery)
                m_playerData.m_battery = m_playerData.m_maxBattery;
        }
    }
}