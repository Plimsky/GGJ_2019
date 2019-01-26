using System;
using UnityEngine;

public class PlayerCollectibleDetector : MonoBehaviour
{
    [SerializeField] private String m_fragmentTag = "Collectibles/Fragment";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(m_fragmentTag))
        {
            Destroy(other.gameObject);
        }
    }
}