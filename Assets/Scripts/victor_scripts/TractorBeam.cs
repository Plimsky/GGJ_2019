using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    public float     MaxLengthRay = 8.0f;
    public Transform Crosshair;

    public LayerMask Mask;

    private Vector3 MouseLastPos;

    public GameObject Target;
    public GameObject m_blackHolePrefab;

    private float LastDirTimer;
    public  float DragForce = 1;

    public float beamEndOffset      = 1f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
    public float textureLengthScale = 3;  //Length of the beam texture

    [Header("Prefabs")]
    public GameObject[] beamLineRendererPrefab;

    public GameObject[] beamStartPrefab;
    public GameObject[] beamEndPrefab;

    private int currentBeam = 0;

    private GameObject   beamStart;
    private GameObject   beamEnd;
    private GameObject   beam;
    private LineRenderer line;
    private Vector2      m_centerRay;
    private Vector2      m_centerRayVelocity   = Vector2.zero;
    public  float        m_centerRaySmoothness = 0.5f;

    private GameObject m_temporaryBlackHole;
    private float      m_distanceTarget;

    void Start()
    {
        MouseLastPos = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            m_distanceTarget = Vector3.Distance(transform.position, Target.transform.position);
        }

        Cursor.visible = false;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Crosshair.position = new Vector3(mousePosition.x, mousePosition.y, 0);

        Vector3      RayDir = mousePosition - transform.position;
        RaycastHit2D hit    = Physics2D.Raycast(transform.position, RayDir, MaxLengthRay, Mask);

        //TRACTOR
        Debug.DrawRay(transform.position, mousePosition - transform.position, Color.blue);

        if (Time.time > LastDirTimer)
        {
            MouseLastPos = Input.mousePosition;

            LastDirTimer = Time.time + 0.05f;
        }

        Vector3 MouseDir = Input.mousePosition - MouseLastPos;
        if (Input.GetMouseButtonDown(0))
        {
            beamStart = Instantiate(beamStartPrefab[currentBeam], new Vector3(0, 0, 0), Quaternion.identity);
            beamEnd   = Instantiate(beamEndPrefab[currentBeam], new Vector3(0, 0, 0), Quaternion.identity);
            beam      = Instantiate(beamLineRendererPrefab[currentBeam], new Vector3(0, 0, 0), Quaternion.identity);
            line      = beam.GetComponent<LineRenderer>();

            if (AudioManager.instance != null)
                AudioManager.instance.SetIsBeamActivated(true);

            if (hit)
            {

                Target = hit.collider.gameObject;
                if (Target.GetComponent<WasteBehaviour>() != null)
                {
                    DragForce                                     = Target.GetComponent<WasteBehaviour>().m_dragForce;
                    Target.GetComponent<WasteBehaviour>().m_state = WasteBehaviour.WasteState.TRACKED;
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 tdir;
            if (Target)
            {
                Vector3 target = new Vector2((transform.position.x + Target.transform.position.x) / 2,
                                             (transform.position.y + Target.transform.position.y) / 2);
                target += (MouseDir.normalized * 5);
                m_centerRay = Vector2.SmoothDamp(m_centerRay, target, ref m_centerRayVelocity, m_centerRaySmoothness);
                tdir        = Target.transform.position - transform.position;

                if (m_temporaryBlackHole == null)
                    m_temporaryBlackHole =
                        Instantiate(m_blackHolePrefab, Target.transform.position, Quaternion.identity);
            }
            else
            {
                m_centerRay = new Vector2((transform.position.x + mousePosition.x) / 2,
                                          (transform.position.y + mousePosition.y) / 2);
                tdir = mousePosition - transform.position;
            }

            if (line != null &&
                beamStart != null &&
                beamEnd != null &&
                beam != null)
            {
                ShootBeamInDir(transform.position, tdir);
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
            m_centerRay = new Vector2((transform.position.x + mousePosition.x) / 2,
                                      (transform.position.y + mousePosition.y) / 2);
            m_distanceTarget = 0.0f;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (AudioManager.instance != null)
                AudioManager.instance.SetIsBeamActivated(false);

            Destroy(beamStart);
            Destroy(beamEnd);
            Destroy(beam);
            Destroy(m_temporaryBlackHole);
            m_distanceTarget = 0.0f;
        }

        if (m_temporaryBlackHole != null && Target != null)
        {
            m_temporaryBlackHole.transform.position =
                new Vector3(Target.transform.position.x, Target.transform.position.y, 2);
        }

        if (m_distanceTarget > MaxLengthRay)
        {
            if (AudioManager.instance != null)
                AudioManager.instance.SetIsBeamActivated(false);
            Target = null;
            Destroy(beamStart);
            Destroy(beamEnd);
            Destroy(beam);
            Destroy(m_temporaryBlackHole);
        }
    }

    public void SetForceAppliedToWaste(float force)
    {
        DragForce = force;
    }

    void ShootBeamInDir(Vector2 start, Vector2 dir)
    {
        line.positionCount = 3;
        line.SetPosition(0, start);
        beamStart.transform.position = start;

        Vector3      end      = Vector3.zero;
        Vector3      dir3D    = new Vector3(dir.x, dir.y, 0);
        Vector2      position = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hit      = Physics2D.Raycast(start, dir, MaxLengthRay, Mask);

        if (hit)
            end = hit.point - (dir.normalized * beamEndOffset);
        else
            end = position + dir.normalized * MaxLengthRay;

        Debug.DrawRay(start, dir, Color.red);

        line.SetPosition(1, m_centerRay);

        beamEnd.transform.position = end;
        line.SetPosition(2, end);

        beamStart.transform.LookAt(m_centerRay);
        beamEnd.transform.LookAt(beamStart.transform.position);

        float distance = Vector3.Distance(start, end);
        line.sharedMaterial.mainTextureScale  =  new Vector2(distance / textureLengthScale, 1);
        line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);

        AnimationCurve curve = new AnimationCurve();

        curve.AddKey(0.0f, 0.0f);
        curve.AddKey(1.0f, 0.5f);

        line.widthCurve      = curve;
        line.widthMultiplier = 1.0f;
    }
}