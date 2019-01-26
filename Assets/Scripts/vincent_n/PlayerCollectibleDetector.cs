using System;
using Managers;
using UnityEngine;

public class PlayerCollectibleDetector : MonoBehaviour
{
    [SerializeField] private PlayerDataSO m_playerData;
    [SerializeField] private String m_fragmentTag = "Collectibles/Fragment";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(m_fragmentTag))
        {
            m_playerData.m_fragments++;
            GameManager.instance.OnUpdateStats();
            Destroy(other.gameObject);
        }
    }
}