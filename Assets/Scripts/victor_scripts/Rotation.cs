using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ROTATION
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 objectPos = transform.position;

        pos.x = pos.x - objectPos.x;
        pos.y = pos.y - objectPos.y;
        pos.z = 0f;
        
        float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

    }
}
