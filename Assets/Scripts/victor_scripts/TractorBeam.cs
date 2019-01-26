using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    public Transform Crosshair;

    public LayerMask Mask;

    private Vector3 MouseLastPos;

    public GameObject Target;

    private float LastDirTimer;
    public  float DragForce = 1;

    void Start()
    {
        MouseLastPos = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Crosshair.position = new Vector3(position.x, position.y, 0);

        Vector3      RayDir = position - transform.position;
        RaycastHit2D hit    = Physics2D.Raycast(transform.position, RayDir, Mask);

        //TRACTOR
        Debug.DrawRay(transform.position, position - transform.position, Color.blue);

        if (Time.time > LastDirTimer)
        {
            MouseLastPos = Input.mousePosition;

            LastDirTimer = Time.time + 0.05f;
        }

        Vector3 MouseDir = Input.mousePosition - MouseLastPos;
        if (Input.GetMouseButton(0))
        {
            if (hit)
            {
                Target = hit.collider.gameObject;
                if (Target.GetComponent<WasteBehaviour>() != null)
                    Target.GetComponent<WasteBehaviour>().m_state = WasteBehaviour.WasteState.TRACKED;
            }

            if (Target)
            {
                Target.GetComponent<Rigidbody2D>().AddForce(MouseDir * DragForce);
                Target.GetComponent<Rigidbody2D>().AddTorque(0.01f);
            }
        }
        else if (!Input.GetMouseButton(0))
        {
            Target = null;
        }
    }

    public void SetForceAppliedToWaste(float force)
    {
        DragForce = force;
    }
}