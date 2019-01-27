using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerZone : MonoBehaviour
{

    public GameObject[] Ennemies;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject ennemy in Ennemies)
        {
            ennemy.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            ActivateEnnemy();
        }
    }


    void ActivateEnnemy()
    {
        foreach(GameObject ennemy in Ennemies)
        {
            ennemy.SetActive(true);
        }
    }
}
