using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class NPCController : MonoBehaviour
{

    [Header("��� ������")]
    [Space]
    [Tooltip("������� ������, ���������� ������")]
    public Transform playerPosition; // ������� ������
    [Space(2)]
    [Header("��� NPC")]
    [Space]
    [Tooltip("���������, � ������� NPC ��������� �����")]
    [Range(0, 10)]
    public float shootingRange = 10f; // ��������� ��� ��������
    [Tooltip("��������� ������ �����������")]
    public Transform[] PointPositions;
    public float DistanceToChangePoint;
    private int NowPositionNumber = 0;
    private bool IsWalk;
    [Space(2)]
    [Header("��� ��������")]
    [Space]
    [Tooltip("����� ��������, ������� �����, � 0.2�")]
    [Range(0, 1000)]
    public float timeattack;
    [Tooltip("��������� ��������")]
    [Range(0, 100)]
    public float power;
    [Tooltip("������ �������� ����")]
    [Range(0, 3)]
    public float bulletSpreadRadius; // ������� ����
    [Tooltip("������ ����")]
    public GameObject bulletPrefab; // ������ ����
    [Tooltip("����� ������ ����")]
    public Transform bulletSpawnPoint; // ����� ������ ����

    private NavMeshAgent navMeshAgent;
    private int time;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = shootingRange; // ������������� ��������� ��������� ������ ��������� ��� ��������
    }

    private void Update()
    {
        // ���������� �� �������
       
     
        time++;
    }

    IEnumerator Shoot()
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
            if (hit.transform.CompareTag("Player"))
            {
                // �������� ������� ���������
                hit.transform.GetComponent<PlayerController>().Hit();
            }
            // ������� ���� �� ����� ������ ���� � ���������� �� �� ����� ��������
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(shootingDirection));

            // ������� ���� ��������� �������� ��� ������ �������
            bullet.GetComponent<Rigidbody>().AddForce(power * bullet.transform.forward);
            // ���������� ���� ����� ��������� ����� ��� ��� ���������� ������������� ����������
            Destroy(bullet, 2f);
        }



        yield return new WaitForSeconds(1);

    }

    IEnumerator Persecution()
    {
        while (!IsWalk)
        { 
            navMeshAgent.SetDestination(playerPosition.position);
            float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.position);
            if (distanceToPlayer <= shootingRange && time > timeattack)
                    {
                        StartCoroutine(Shoot());
                        time = 0;
                    }
            else { StopCoroutine(Shoot()); }
            yield return new WaitForSeconds(0.1f);
        }
         
    }

    IEnumerator WalkAround()
    {
        navMeshAgent.destination = PointPositions[NowPositionNumber].position;
        while (IsWalk)
        {
            
            if(navMeshAgent.remainingDistance < DistanceToChangePoint)
            {
                NowPositionNumber++;
                if(NowPositionNumber == PointPositions.Length) NowPositionNumber = 0;
                navMeshAgent.destination = PointPositions[NowPositionNumber].position;
            }
           
            
            yield return new WaitForSeconds(0.1f);
        }      
    }

   
}

