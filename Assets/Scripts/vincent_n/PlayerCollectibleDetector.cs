using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class PlayerCollectibleDetector : MonoBehaviour
{
    [SerializeField] private PlayerDataSO     m_playerData;
    [SerializeField] private String           m_fragmentTag = "Collectibles/Fragment";
    [SerializeField] private String           m_batteryTag  = "Collectibles/BatteryPack";
    [SerializeField] private List<GameObject> m_powerUpPrefabs;

    [SerializeField] private float m_timeBeforeDestroyingPrefabParticle = 2.0f;
    //private GameObject m_temporaryPowerUp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(m_fragmentTag))
        {
            m_playerData.m_fragments++;
            GameManager.instance.OnUpdateStats();
            GameObject powerUp = Instantiate(m_powerUpPrefabs[1], transform.position, Quaternion.identity,
                                             gameObject.transform);
            powerUp.transform.localPosition -= Vector3.forward;
            if(AudioManager.instance != null)
               AudioManager.instance.SetIsPowerUp(true);
            Destroy(other.gameObject);
            Destroy(powerUp, m_timeBeforeDestroyingPrefabParticle);
        }
        else if (other.gameObject.CompareTag(m_batteryTag))
        {
            if (m_playerData.m_battery < m_playerData.m_maxBattery)
            {
                m_playerData.m_battery += 10;
                GameManager.instance.OnUpdateStats();
                if (AudioManager.instance != null)
                    AudioManager.instance.SetIsPowerUp(true);
                GameObject powerUp = Instantiate(m_powerUpPrefabs[0], transform.position, Quaternion.identity,
                                                 gameObject.transform);
                powerUp.transform.localPosition -= Vector3.forward;
                Debug.Log("hit");
                Destroy(other.gameObject);
                Destroy(powerUp, m_timeBeforeDestroyingPrefabParticle);
            }

            if (m_playerData.m_battery > m_playerData.m_maxBattery)
            {
                m_playerData.m_battery = m_playerData.m_maxBattery;
                //Instantiate(m_powerUpPrefabs[0], transform.position, Quaternion.identity);
            }
        }
    }
}