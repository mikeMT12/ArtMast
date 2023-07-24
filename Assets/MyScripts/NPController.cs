using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;
public class NPCController : MonoBehaviour
{

    [Header("Для игрока")]
    [Space]
    [Tooltip("Позиция игрока, перетащите игрока")]
    public Transform playerPosition; // Позиция игрока
    [Space(2)]
    [Header("Для NPC")]
    [Space]
    [Tooltip("Точки по которым ходит npc")]
    public Transform[] PointPositions;
    [Tooltip("Дистанция, с которой NPC открывает огонь")]
    [Range(0, 10)]
    public float shootingRange = 10f; // Дистанция для стрельбы
    public float AttackTime;
    public float DistanceToChangePoint;
    public float PersecutionDistance;
    public Animator NpcAnimator;
    public float NpcHealth;
    public Scrollbar HealthScrollbar;
    private int NowPositionNumber = 0;
    private bool IsWalk = true;
    [Space(2)]
    [Header("Для стрельбы")]
    [Space]
    public float DamageStrong;
    [Tooltip("Радиус разброса пуль")]
    [Range(0, 3)]
    public float bulletSpreadRadius; // Разброс пуль
    [Tooltip("Точка спавна пули")]
    public Transform bulletSpawnPoint; // Точка спавна пуль


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
        // Вычисляем направление стрельбы
        Vector3 shootingDirection = playerPosition.position - bulletSpawnPoint.position;

        // Применяем разброс пуль
        Vector3 bulletSpread = Random.insideUnitCircle * bulletSpreadRadius;
        shootingDirection.x += bulletSpread.x;
        shootingDirection.y += bulletSpread.y;

        // Выпускаем луч в направлении игрока
        RaycastHit hit;
        if (Physics.Raycast(bulletSpawnPoint.position, shootingDirection, out hit))
        {
            // Проверяем попадание в игрока
            if (hit.transform.GetComponent<PlayerController>() != null)
            {
                // Вызываем функцию попадания
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

            //если игрок сишком далеко то закончить преследование
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
            if (distanceToPlayer <= PersecutionDistance && PlayerController.instance.AttacsNpc <= 1)// 1 можешь поменять на 2
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

