using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class PlayerCollectibleDetector : MonoBehaviour
{
    [SerializeField] private PlayerDataSO m_playerData;
    [SerializeField] private String m_fragmentTag = "Collectibles/Fragment";
    [SerializeField] private String m_batteryTag = "Collectibles/BatteryPack";
    [SerializeField] private List<GameObject> m_powerUpPrefabs;
    //private GameObject m_temporaryPowerUp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(m_fragmentTag))
        {
            m_playerData.m_fragments++;
            GameManager.instance.OnUpdateStats();
            Instantiate(m_powerUpPrefabs[1], transform.position, Quaternion.identity, gameObject.transform);
            AudioManager.instance.SetIsPowerUp(true);
            Destroy(other.gameObject);
        }
        else if(other.gameObject.CompareTag(m_batteryTag))
        {
            if (m_playerData.m_battery < m_playerData.m_maxBattery)
            {
                m_playerData.m_battery += 10;
                GameManager.instance.OnUpdateStats();
                AudioManager.instance.SetIsPowerUp(true);
                Instantiate(m_powerUpPrefabs[0], transform.position, Quaternion.identity, gameObject.transform);
                Debug.Log("hit");
                Destroy(other.gameObject);
            }

            if (m_playerData.m_battery > m_playerData.m_maxBattery)
            {
                m_playerData.m_battery = m_playerData.m_maxBattery;
                //Instantiate(m_powerUpPrefabs[0], transform.position, Quaternion.identity);
            }
        }
    }
}