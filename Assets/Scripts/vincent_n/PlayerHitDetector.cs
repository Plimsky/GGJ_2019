using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitDetector : MonoBehaviour
{
    [SerializeField] private PlayerDataSO m_playerData;
    [SerializeField] private float m_velocityMagnitudeForDamage = 1.0f;
    [SerializeField] private List<String> m_tagListHit = new List<string>();

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject go = other.gameObject;

        foreach (String tagName in m_tagListHit)
        {
            if (go.CompareTag(tagName))
            {
                Rigidbody2D rigidbody2D = go.GetComponent<Rigidbody2D>();
                WasteBehaviour wasteBehaviourScript = go.GetComponent<WasteBehaviour>();

                if (rigidbody2D != null && wasteBehaviourScript != null)
                {
                    Debug.Log("Velocity Magnitude of " + go.name + " : " + rigidbody2D.velocity.magnitude);

                    if (rigidbody2D.velocity.magnitude > m_velocityMagnitudeForDamage &&
                        wasteBehaviourScript.m_state == WasteBehaviour.WasteState.MORTAL)
                    {
                        Debug.Log("HIT");
                        wasteBehaviourScript.m_life--;
                        m_playerData.m_life -= wasteBehaviourScript.m_damageValue;
                    }
                }
                else
                {
                    Debug.Log("Maybe it's a Laser Enemy Shot");
                }

                break;
            }
        }
    }
}