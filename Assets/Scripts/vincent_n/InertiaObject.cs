using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InertiaObject : MonoBehaviour
{
    [SerializeField] private float m_speedRotation = 4.0f;
    private Rigidbody2D m_rigidbody;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        if (m_rigidbody != null)
        {
            m_rigidbody.AddTorque(m_speedRotation, ForceMode2D.Force);
        }
    }
}