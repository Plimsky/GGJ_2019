using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_speed = 5.0f;
    [SerializeField] private float m_smoothing = 0.5f;
    private Rigidbody2D m_rigidbody;
    private Vector2 m_velocity;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 playerDirection;

        playerDirection.x = Input.GetAxis("Horizontal");
        playerDirection.y = Input.GetAxis("Vertical");

        playerDirection.Normalize();
        playerDirection *= m_speed;

        Vector3 smoothMovement = Vector2.SmoothDamp(m_rigidbody.velocity, playerDirection, ref m_velocity, m_smoothing);

        m_rigidbody.AddForce(smoothMovement);
    }
}