using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    NavMeshAgent2D Agent;

    private Transform Player;
    private Vector3 FallBackVector;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        Agent = GetComponent<NavMeshAgent2D>();  
        Agent.SetDestination(Player.position);
    }

    // Update is called once per frame
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
    }
}
