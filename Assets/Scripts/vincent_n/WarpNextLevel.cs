using System;
using Managers;
using UnityEngine;

public class WarpNextLevel : MonoBehaviour
{
    [SerializeField] private String     m_playerTagName = "Player";
    [SerializeField] private GameObject m_spawnSpeedOfLightPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject go = other.gameObject;
        if (go.CompareTag(m_playerTagName))
        {
            if (m_spawnSpeedOfLightPrefab != null)
            {
                Quaternion rotation = go.transform.GetChild(3).rotation;
                rotation = Quaternion.Slerp(rotation,
                                                         Quaternion.Euler(new Vector3(-90, rotation.z, 0)),
                                                         Time.deltaTime * 100);
                Instantiate(m_spawnSpeedOfLightPrefab, go.transform.position, rotation);
            }

            if (GameManager.instance != null)
                GameManager.instance.NextLevel();
        }
    }
}