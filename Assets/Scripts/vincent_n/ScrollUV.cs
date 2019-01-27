using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUV : MonoBehaviour
{
    public float m_speedScroll = 0.1f;

    private void Update()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        Material mat = mr.material;

        Vector2 offset = mat.mainTextureOffset;
        offset.x = transform.position.x * m_speedScroll;
        offset.y = transform.position.y * m_speedScroll;
        mat.mainTextureOffset = offset;

    }
}
