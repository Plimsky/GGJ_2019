using System;
using Managers;
using UnityEngine;

public class WarpNextLevel : MonoBehaviour
{
    [SerializeField] private String m_playerTagName = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(m_playerTagName))
            GameManager.instance.NextLevel();
    }
}
