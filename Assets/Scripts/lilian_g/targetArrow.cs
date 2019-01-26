using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetArrow : MonoBehaviour
{
    public Transform m_target;
    public Transform m_player;

    private void Update()
    {
        transform.position = m_player.position;

        Vector3 pos = m_target.position;
        Vector3 objectPos = transform.position;

        pos.x = pos.x - objectPos.x;
        pos.y = pos.y - objectPos.y;
        pos.z = 0f;

        float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
    }
}
