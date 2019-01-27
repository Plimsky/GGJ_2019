using Managers;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float      m_speed = 50.0f;
    [SerializeField] private Transform  m_bodyPlayer;
    [SerializeField] private float      m_smoothBodyPlayerRotation = 1.0f;
    [SerializeField] private GameObject m_movingBeam;


    private Rigidbody2D m_rigidbody;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        //m_movingBeam = transform.GetChild(4).transform.gameObject;
    }

    private void FixedUpdate()
    {
        Vector2 playerDirection = Vector2.zero;
        playerDirection.x = Input.GetAxis("Horizontal");
        playerDirection.y = Input.GetAxis("Vertical");

        if (AudioManager.instance != null && playerDirection != Vector2.zero)
        {
            AudioManager.instance.SetIsPropulsion(true);
            AudioManager.instance.SetVolume(AudioManager.instance.m_AmbientMusicAudioSource,
                                            (Mathf.Abs(m_rigidbody.velocity.normalized.x) +
                                             Mathf.Abs(m_rigidbody.velocity.normalized.y)) / 2);
            if (m_movingBeam != null)
                m_movingBeam.SetActive(true);
        }

        if (AudioManager.instance != null && playerDirection == Vector2.zero)
        {
            AudioManager.instance.SetIsPropulsion(false);
            if (m_movingBeam != null)
                m_movingBeam.SetActive(true);
        }

        if (playerDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
            m_bodyPlayer.rotation = Quaternion.Slerp(m_bodyPlayer.rotation,
                                                     Quaternion.Euler(new Vector3(0, 0, angle + 90)),
                                                     Time.deltaTime * m_smoothBodyPlayerRotation);
        }


        playerDirection *= m_speed;

        m_rigidbody.AddForce(playerDirection, ForceMode2D.Force);
    }
}