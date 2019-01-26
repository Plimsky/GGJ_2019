using System.Collections;
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
    [SerializeField] private float m_mortalTime = 3.0f;
    [SerializeField] private float m_trackedTime = 1.0f;
    [SerializeField] private float m_velocityLimitForBeingMortal = 1f;

    private Rigidbody2D m_rigidbody;
    private bool m_timerStarted;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
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

        if (m_state == WasteState.TRACKED && !m_timerStarted)
            StartCoroutine("ResetToFreeState", m_trackedTime);

        if (m_life <= 0)
            Destroy(gameObject);
    }

    private IEnumerator ResetToFreeState()
    {
        m_timerStarted = true;

        yield return new WaitForSeconds(m_mortalTime);

        m_state = WasteState.FREE;
        m_timerStarted = false;
    }
}