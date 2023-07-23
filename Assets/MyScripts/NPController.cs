using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;
public class NPCController : MonoBehaviour
{

    [Header("��� ������")]
    [Space]
    [Tooltip("������� ������, ���������� ������")]
    public Transform playerPosition; // ������� ������
    [Space(2)]
    [Header("��� NPC")]
    [Space]
    [Tooltip("����� �� ������� ����� npc")]
    public Transform[] PointPositions;
    [Tooltip("���������, � ������� NPC ��������� �����")]
    [Range(0, 10)]
    public float shootingRange = 10f; // ��������� ��� ��������
    public float AttackTime;
    public float DistanceToChangePoint;
    public float PersecutionDistance;
    public Animator NpcAnimator;
    public float NpcHealth;
    public Scrollbar HealthScrollbar;
    private int NowPositionNumber = 0;
    private bool IsWalk = true;
    [Space(2)]
    [Header("��� ��������")]
    [Space]
    public float DamageStrong;
    [Tooltip("������ �������� ����")]
    [Range(0, 3)]
    public float bulletSpreadRadius; // ������� ����
    [Tooltip("����� ������ ����")]
    public Transform bulletSpawnPoint; // ����� ������ ����


    private float StartHealth;
    private NavMeshAgent navMeshAgent;
    private int time;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = shootingRange;
         StartHealth = NpcHealth;
    }

    public void StartWalk()
    {
        StartCoroutine(WalkAround());
    }

    void Shoot()
    {
        // ��������� ����������� ��������
        Vector3 shootingDirection = playerPosition.position - bulletSpawnPoint.position;

        // ��������� ������� ����
        Vector3 bulletSpread = Random.insideUnitCircle * bulletSpreadRadius;
        shootingDirection.x += bulletSpread.x;
        shootingDirection.y += bulletSpread.y;

        // ��������� ��� � ����������� ������
        RaycastHit hit;
        if (Physics.Raycast(bulletSpawnPoint.position, shootingDirection, out hit))
        {
            // ��������� ��������� � ������
            if (hit.transform.GetComponent<PlayerController>() != null)
            {
                // �������� ������� ���������
                hit.transform.GetComponent<PlayerController>().Hit(DamageStrong);
            }
            if(NpcAnimator!=null) {  NpcAnimator.Play("attack");}
        }
    }

    IEnumerator Persecution()
    {
        while (!IsWalk)
        {         
            float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.position);
            if (distanceToPlayer <= shootingRange)
                    {
                        Shoot();   
                    }
            else 
                {  
                    navMeshAgent.SetDestination(playerPosition.position);
                }

            //���� ����� ������ ������ �� ��������� �������������
            if (distanceToPlayer >= PersecutionDistance * 4)
            {
                IsWalk = true;
                PlayerController.instance.AttacsNpc--;
                StartCoroutine(WalkAround());
            }
            yield return new WaitForSeconds(AttackTime);
        }
         
    }

    IEnumerator WalkAround()
    {
        navMeshAgent.destination = PointPositions[NowPositionNumber].position;
        while (IsWalk)
        {
            //walk to points
            if(navMeshAgent.remainingDistance <= DistanceToChangePoint)
            {
                NowPositionNumber++;
                if(NowPositionNumber == PointPositions.Length) NowPositionNumber = 0;
                navMeshAgent.SetDestination(PointPositions[NowPositionNumber].position);
            }
            //chek attack player
            float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.position);
            if (distanceToPlayer <= PersecutionDistance && PlayerController.instance.AttacsNpc <= 1)// 1 ������ �������� �� 2
            {
                IsWalk = false;
                PlayerController.instance.AttacsNpc++;
                StartCoroutine(Persecution());
            }

            yield return new WaitForSeconds(0.1f);
        }      
    }

    public void OnHit(float damage)
    {
        NpcHealth -= damage;
        HealthScrollbar.size =  NpcHealth / StartHealth;
        if(NpcHealth <= 0)
        {
            StopAllCoroutines();
            if(NpcAnimator != null) {NpcAnimator.Play("death"); }
            
            Destroy(gameObject, 1);
        }
        
    }
}

