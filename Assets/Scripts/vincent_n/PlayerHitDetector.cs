using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitDetector : MonoBehaviour
{
    [SerializeField] private PlayerDataSO m_playerData;
    [SerializeField] private float m_velocityMagnitudeForDamage = 1.0f;
    [SerializeField] private List<string> m_tagListHit = new List<string>();
    [SerializeField] private float m_invulnerabilityTime = 2.0f;
    [SerializeField] private bool m_isInvulnerable = false;
    [SerializeField] private bool m_spark = false;
    [SerializeField] private float m_sparksIntervale = 3.0f;

    //public GameObject m_sparksPrefab1;
    //public GameObject m_sparksPrefab2;
    //public GameObject m_sparksPrefab3;
    //public GameObject m_sparksPrefab4;
    [SerializeField] private List<GameObject> m_sparksPrefabs;
    private GameObject m_temporarySparks;


    //public GameObject m_lowHealthSparksPrefab1;
    //public GameObject m_lowHealthSparksPrefab2;
    //public GameObject m_lowHealthSparksPrefab3;
    //public GameObject m_lowHealthSparksPrefab4;
    [SerializeField] private List<GameObject> m_lowHealthPrefabs;
    private GameObject m_lowHealthSparks;

    private void Update()
    {
        LowHealt();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject go = other.gameObject;
        Debug.Log("asddasasdadsdasadsadsasdasddasasdasd"+ go.tag);
        foreach (string tagName in m_tagListHit)
        {
            if (go.CompareTag(tagName))
            {
                Rigidbody2D rigidbody2D = go.GetComponent<Rigidbody2D>();
                WasteBehaviour wasteBehaviourScript = go.GetComponent<WasteBehaviour>();

                if (rigidbody2D != null && wasteBehaviourScript != null)
                {
                    Debug.Log("Velocity Magnitude of " + go.name + " : " + rigidbody2D.velocity.magnitude);
                    if (AudioManager.instance != null)
                    {
                        AudioManager.instance.SetIsCollision(true);
                        AudioManager.instance.SetVolume(AudioManager.instance.m_OneShotAudioSource, (Mathf.Abs(rigidbody2D.velocity.normalized.x) + Mathf.Abs(rigidbody2D.velocity.normalized.y)) / 2);
                    }

                    if (rigidbody2D.velocity.magnitude > m_velocityMagnitudeForDamage &&
                        wasteBehaviourScript.m_state == WasteBehaviour.WasteState.MORTAL && !m_isInvulnerable)
                    {
                        Debug.Log("HIT");

                        wasteBehaviourScript.m_life--;
                        m_playerData.m_life -= wasteBehaviourScript.m_damageValue;

                        if (m_temporarySparks == null)
                            m_temporarySparks = Instantiate(m_sparksPrefabs[Random.Range(0, m_sparksPrefabs.Count - 1)], transform.position, Quaternion.identity);

                        StartCoroutine("ResetToFreeState", m_invulnerabilityTime);
                    }
                }
                else
                {
                    Debug.Log("Maybe it's a Laser Enemy Shot");
                    m_playerData.m_life -= 5;
                    Instantiate(m_sparksPrefabs[Random.Range(0, m_sparksPrefabs.Count - 1)], transform.position, Quaternion.identity);
                    Destroy(go);
                }

                break;
            }
        }
    }

    private void LowHealt()
    {
        if (m_playerData.m_life < 25.0f && !m_spark)
        {
            m_lowHealthSparks = Instantiate(m_lowHealthPrefabs[Random.Range(0, m_lowHealthPrefabs.Count - 1)], transform.position, Quaternion.identity);

            StartCoroutine("SparksIntervale", m_sparksIntervale);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
    }

    private IEnumerator ResetToFreeState()
    {
        //Debug.Log("invulnarable");
        m_isInvulnerable = true;
        yield return new WaitForSeconds(m_invulnerabilityTime);
        Destroy(m_temporarySparks);
        m_isInvulnerable = false;
        if (AudioManager.instance != null)
            AudioManager.instance.SetIsCollision(false);
    }

    private IEnumerator SparksIntervale()
    {
        m_spark = true;
        yield return new WaitForSeconds(m_sparksIntervale);
        Destroy(m_lowHealthSparks);
        m_spark = false;
    }
}