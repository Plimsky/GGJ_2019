using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    [Header("Player Config")]
    public PlayerDataSO m_playerData;

    public int   m_decrementBatteryValue  = 5;
    public float m_timerDecrementInterval = 1.0f;

    [Header("Ray Beam PHYSICS Config")]
    public float m_centerRaySmoothness = 0.5f;

    public LayerMask m_mask;
    public float     m_maxLengthRay = 8.0f;
    public float     m_dragForce    = 1;

    [Header("Ray Beam DISPLAY Config")]
    public GameObject m_blackHolePrefab;

    public float m_beamEndOffset      = 1f; //How far from the raycast hit point the end effect is positioned
    public float m_textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
    public float m_textureLengthScale = 3;  //Length of the beam texture


    [Header("Optional")]
    public Transform m_crosshair;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject[] m_beamLineRendererPrefab;

    [SerializeField] private GameObject[] m_beamStartPrefab;
    [SerializeField] private GameObject[] m_beamEndPrefab;
    [SerializeField] private GameObject   m_crossHairPrefab;

    private float _lastDirTimer;
    private int   _currentBeam = 0;

    private GameObject   _target;
    private Vector3      _mouseLastPos;
    private GameObject   _beamStart;
    private GameObject   _beamEnd;
    private GameObject   _beam;
    private LineRenderer _line;
    private Vector2      _centerRay;
    private Vector2      _centerRayVelocity = Vector2.zero;
    private GameObject   _temporaryBlackHole;
    private float        _distanceTarget;
    private float        _actualDecrementTime;

    void Start()
    {
        _mouseLastPos = Vector3.zero;
        if (m_crosshair != null)
            m_crosshair = Instantiate(m_crossHairPrefab, transform.position, Quaternion.identity).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_target != null)
        {
            _distanceTarget = Vector3.Distance(transform.position, _target.transform.position);
        }

        Cursor.visible = false;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        m_crosshair.position = new Vector3(mousePosition.x, mousePosition.y, 0);

        Vector3      rayDir = mousePosition - transform.position;
        RaycastHit2D hit    = Physics2D.Raycast(transform.position, rayDir, m_maxLengthRay, m_mask);

        //TRACTOR
        Debug.DrawRay(transform.position, mousePosition - transform.position, Color.blue);

        if (Time.time > _lastDirTimer)
        {
            _mouseLastPos = Input.mousePosition;
            _lastDirTimer = Time.time + 0.05f;
        }

        Vector3 mouseDir = Input.mousePosition - _mouseLastPos;

        if (m_playerData.m_battery >= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _beamStart = Instantiate(m_beamStartPrefab[_currentBeam], new Vector3(0, 0, 0), Quaternion.identity);
                _beamEnd   = Instantiate(m_beamEndPrefab[_currentBeam], new Vector3(0, 0, 0), Quaternion.identity);
                _beam = Instantiate(m_beamLineRendererPrefab[_currentBeam], new Vector3(0, 0, 0),
                                         Quaternion.identity);
                _line = _beam.GetComponent<LineRenderer>();

                if (AudioManager.instance != null)
                    AudioManager.instance.SetIsBeamActivated(true);

                if (hit)
                {
                    _target = hit.collider.gameObject;
                    if (_target.GetComponent<WasteBehaviour>() != null)
                    {
                        m_dragForce =
                            _target.GetComponent<WasteBehaviour>().m_dragForce;
                        _target.GetComponent<WasteBehaviour>().m_state = WasteBehaviour.WasteState.TRACKED;
                    }

                    m_playerData.m_battery -= m_decrementBatteryValue;
                }
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 tdir;
                if (_target)
                {
                    Vector3 target = new Vector2((transform.position.x + _target.transform.position.x) / 2,
                                                 (transform.position.y + _target.transform.position.y) / 2);
                    target     += (mouseDir.normalized * 5);
                    _centerRay =  Vector2.SmoothDamp(_centerRay, target, ref _centerRayVelocity, m_centerRaySmoothness);
                    tdir       =  _target.transform.position - transform.position;

                    if (_temporaryBlackHole == null)
                    {
                        _temporaryBlackHole =
                            Instantiate(m_blackHolePrefab, _target.transform.position, Quaternion.identity);
                        _temporaryBlackHole.transform.localScale = transform.localScale * 2;
                    }
                }
                else
                {
                    _centerRay = new Vector2((transform.position.x + mousePosition.x) / 2,
                                             (transform.position.y + mousePosition.y) / 2);
                    tdir = mousePosition - transform.position;
                }

                if (_line != null &&
                    _beamStart != null &&
                    _beamEnd != null &&
                    _beam != null)
                {
                    ShootBeamInDir(transform.position, tdir);
                }

                if (_target)
                {
                    _target.GetComponent<Rigidbody2D>().AddForce(mouseDir * m_dragForce);
                    _target.GetComponent<Rigidbody2D>().AddTorque(0.01f);
                    _actualDecrementTime += Time.deltaTime;

                    if (_actualDecrementTime > m_timerDecrementInterval)
                    {
                        _actualDecrementTime   =  0.0f;
                        m_playerData.m_battery -= m_decrementBatteryValue;
                    }
                }
            }
        }

        if (!Input.GetMouseButton(0))
        {
            _target = null;
            _centerRay = new Vector2((transform.position.x + mousePosition.x) / 2,
                                     (transform.position.y + mousePosition.y) / 2);
            _distanceTarget = 0.0f;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (AudioManager.instance != null)
                AudioManager.instance.SetIsBeamActivated(false);

            Destroy(_beamStart);
            Destroy(_beamEnd);
            Destroy(_beam);
            Destroy(_temporaryBlackHole);
            _distanceTarget      = 0.0f;
            _actualDecrementTime = 0.0f;
        }

        if (_temporaryBlackHole != null && _target != null)
        {
            _temporaryBlackHole.transform.position =
                new Vector3(_target.transform.position.x, _target.transform.position.y, -1);
        }

        if (_distanceTarget > m_maxLengthRay)
        {
            if (AudioManager.instance != null)
                AudioManager.instance.SetIsBeamActivated(false);
            _target = null;
            Destroy(_beamStart);
            Destroy(_beamEnd);
            Destroy(_beam);
            Destroy(_temporaryBlackHole);
        }
    }

    public void SetForceAppliedToWaste(float p_force)
    {
        m_dragForce = p_force;
    }

    void ShootBeamInDir(Vector2 p_start, Vector2 p_dir)
    {
        _line.positionCount = 3;
        _line.SetPosition(0, p_start);
        _beamStart.transform.position = p_start;

        Vector3      end      = Vector3.zero;
        Vector3      dir3D    = new Vector3(p_dir.x, p_dir.y, 0);
        Vector2      position = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hit      = Physics2D.Raycast(p_start, p_dir, m_maxLengthRay, m_mask);

        if (hit)
            end = hit.point - (p_dir.normalized * m_beamEndOffset);
        else
            end = position + p_dir.normalized * m_maxLengthRay;

        Debug.DrawRay(p_start, p_dir, Color.red);

        _line.SetPosition(1, _centerRay);

        _beamEnd.transform.position = end;
        _line.SetPosition(2, end);

        _beamStart.transform.LookAt(_centerRay);
        _beamEnd.transform.LookAt(_beamStart.transform.position);

        float distance = Vector3.Distance(p_start, end);
        _line.sharedMaterial.mainTextureScale  =  new Vector2(distance / m_textureLengthScale, 1);
        _line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * m_textureScrollSpeed, 0);

        AnimationCurve curve = new AnimationCurve();

        curve.AddKey(0.0f, 0.0f);
        curve.AddKey(1.0f, 0.5f);

        _line.widthCurve      = curve;
        _line.widthMultiplier = 1.0f;
    }
}