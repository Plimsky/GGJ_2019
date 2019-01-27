using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    NavMeshAgent2D Agent;

    private Transform Player;
    public Transform RayStart;

    public LayerMask Mask;
    private Vector3 FallBackVector;

    public Rigidbody2D LaserBullet;

    public float RateOfFire;
    public float TimeBetweenEachBurst;
    private float LastBulletTimer;
    private float TimeUntilNextBurst;

    public int BulletsPerBurst;
    private int BulletsShot;


    private bool isOnBurstCooldown;
        
    void Start()
    {
        RateOfFire = 1.0f / RateOfFire;
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        Agent = GetComponent<NavMeshAgent2D>();  
        Agent.SetDestination(Player.position);

        LastBulletTimer = 0.0f;
    }

    void Update()
    {
        FallBackVector = transform.position - Player.position;
        if(Agent.hasPath == false)
            Agent.SetDestination(Player.position);

        if(Vector3.Distance(Player.position, transform.position) < 5.0f)
            Agent.SetDestination(FallBackVector);
        else
            Agent.SetDestination(Player.position);

            
        Vector3 pos = Player.position;
        Vector3 objectPos = transform.position;

        pos.x = pos.x - objectPos.x;
        pos.y = pos.y - objectPos.y;
        pos.z = 0f;
        
        float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


        //Shooting
        Ray ray = new Ray(RayStart.position, transform.right * 100.0f);
        RaycastHit2D hit = Physics2D.Raycast(RayStart.position, transform.right * 100.0f);

        if(BulletsShot >= BulletsPerBurst)
        {
            isOnBurstCooldown = true;
            TimeUntilNextBurst = Time.time + TimeBetweenEachBurst;
            BulletsShot = 0;
        }


        if(isOnBurstCooldown)
        {
            if(Time.time > TimeUntilNextBurst)
                isOnBurstCooldown = false;
        }
        else
        {
            if(Time.time > LastBulletTimer)
            {
                if(hit.collider.gameObject.CompareTag("Player"))
                {
                    Rigidbody2D LaserBulletBis = Instantiate(LaserBullet, RayStart.position, RayStart.rotation);
                    LaserBulletBis.AddForce(transform.right * 600.0f);
                }
                BulletsShot++;

                LastBulletTimer = Time.time + RateOfFire;
            }
        }

        //Debug.DrawRay(RayStart.position, transform.right * 100.0f);

        Debug.Log(hit.collider.gameObject);
    }

    public void SetBulletsPerBurst(int Count)
    {
        BulletsPerBurst = Count;
    }

    public void SetTimeBetweenEachBurst(float Timer)
    {
        TimeBetweenEachBurst = Timer;
    }
}
