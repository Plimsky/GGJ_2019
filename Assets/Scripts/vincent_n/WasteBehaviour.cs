﻿using System.Collections;
using UnityEngine;

public class WasteBehaviour : MonoBehaviour
{
    public enum WasteState
    {
        FREE,
        TRACKED,
        MORTAL
    }

    public WasteState m_state;
    public int m_life = 5;
    public int m_damageValue = 10;
    public float m_dragForce = 0.5f;
    [SerializeField] private float m_mortalTime = 3.0f;
    [SerializeField] private float m_trackedTime = 1.0f;
    [SerializeField] private float m_velocityLimitForBeingMortal = 1f;
    [SerializeField] private GameObject m_particles;
    [SerializeField] private GameObject m_particles2;


    private Rigidbody2D m_rigidbody;
    private bool m_timerStarted;
    private WasteState m_oldState;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_particles.SetActive(false);
        m_particles2.SetActive(false);
    }

    private void Update()
    {
        if ((m_oldState == WasteState.FREE || m_oldState == WasteState.MORTAL) && m_state == WasteState.TRACKED)
        {
            Debug.Log("Reducing velocity");
            m_rigidbody.velocity /= 3.0f;
        }

        if (m_rigidbody.velocity.magnitude >= m_velocityLimitForBeingMortal && m_state == WasteState.TRACKED)
        {
            m_state = WasteState.MORTAL;
            StopCoroutine("ResetToFreeState");
            m_timerStarted = false;
        }
        if (m_rigidbody.velocity.magnitude < m_velocityLimitForBeingMortal && m_state == WasteState.MORTAL)
            m_state = WasteState.FREE;

        if (m_state == WasteState.MORTAL && !m_timerStarted)
            StartCoroutine("ResetToFreeState", m_mortalTime);
//
//        if (m_state == WasteState.TRACKED && !m_timerStarted)
//            StartCoroutine("ResetToFreeState", m_trackedTime);

        if (m_life <= 0)
            Destroy(gameObject);

        if (m_state == WasteState.MORTAL)
        {
            m_particles.SetActive(true);
            m_particles2.SetActive(true);
        }
        else
        {
            m_particles.SetActive(false);
            m_particles2.SetActive(false);
        }

        m_oldState = m_state;
    }

    private IEnumerator ResetToFreeState()
    {
        m_timerStarted = true;

        yield return new WaitForSeconds(m_mortalTime);

        m_state = WasteState.FREE;
        m_timerStarted = false;
    }
}