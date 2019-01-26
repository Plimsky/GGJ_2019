using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Camera m_camera;

    void Update()
    {
        Vector3 pos       = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 objectPos = transform.position;

        pos.x = pos.x - objectPos.x;
        pos.y = pos.y - objectPos.y;
        pos.z = 0f;

        float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
    }
}