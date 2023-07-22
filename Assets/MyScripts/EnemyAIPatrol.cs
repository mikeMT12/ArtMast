using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIPatrol : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    [SerializeField] LayerMask groundLayer, playerLayer;
    Animator animator;
    BoxCollider boxCollider;
    public Health health;

    //patrol
    Vector3 destPoint;
    bool walkpointSet;
    [SerializeField] float range;

    //state change
    [SerializeField] float sightRange, attackRange;
    bool playerInSight, playerInAttckRange;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        boxCollider = GetComponentInChildren<BoxCollider>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
