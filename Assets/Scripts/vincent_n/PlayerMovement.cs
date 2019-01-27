using Managers;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_speed = 50.0f;
    [SerializeField] private Transform m_bodyPlayer;
    [SerializeField] private float m_smoothBodyPlayerRotation = 1.0f;

    private Rigidbody2D m_rigidbody;
    private Vector2 m_velocity;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 playerDirection = Vector2.zero;

        if (AudioManager.instance != null && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
            AudioManager.instance.SetIsPropulsion(true);
        else if (AudioManager.instance != null && (Input.GetAxis("Horizontal") == 0 || Input.GetAxis("Vertical") == 0))
            AudioManager.instance.SetIsPropulsion(false);

        playerDirection.x = Input.GetAxis("Horizontal");
        playerDirection.y = Input.GetAxis("Vertical");


        if (playerDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
            m_bodyPlayer.rotation = Quaternion.Slerp(m_bodyPlayer.rotation, Quaternion.Euler(new Vector3(0, 0, angle + 90)), Time.deltaTime * m_smoothBodyPlayerRotation);
        }

        playerDirection *= m_speed;

        m_rigidbody.AddForce(playerDirection, ForceMode2D.Force);
    }
}