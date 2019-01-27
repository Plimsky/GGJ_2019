using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InertiaObject : MonoBehaviour
{

    [SerializeField, Range(-5.0f, 5.0f)] private float m_minSpeedRotation = -1.0f;
    [SerializeField, Range(-5.0f, 5.0f)] private float m_maxSpeedRotation = 1.0f;
    private Rigidbody2D m_rigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        if (m_rigidbody != null)
        {
            m_maxSpeedRotation = Mathf.Clamp(m_maxSpeedRotation, m_minSpeedRotation, 5.0f);
            m_rigidbody.AddTorque(Random.Range(m_minSpeedRotation, m_maxSpeedRotation), ForceMode2D.Force);
        }
    }
}