﻿using Managers;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_speed = 50.0f;
    private Rigidbody2D m_rigidbody;
    private Vector2 m_velocity;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 playerDirection;

        if (AudioManager.instance != null && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
            AudioManager.instance.SetIsPropulsion(true);
        else if (AudioManager.instance != null && (Input.GetAxis("Horizontal") == 0 || Input.GetAxis("Vertical") == 0))
            AudioManager.instance.SetIsPropulsion(false);

        playerDirection.x = Input.GetAxis("Horizontal");
        playerDirection.y = Input.GetAxis("Vertical");
        playerDirection *= m_speed;

        m_rigidbody.AddForce(playerDirection, ForceMode2D.Force);
    }
}