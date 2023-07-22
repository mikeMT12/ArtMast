using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class NPCController : MonoBehaviour
{

    [Header("Для игрока")]
    [Space]
    [Tooltip("Позиция игрока, перетащите игрока")]
    public Transform playerPosition; // Позиция игрока
    [Space(2)]
    [Header("Для NPC")]
    [Space]
    [Tooltip("Дистанция, с которой NPC открывает огонь")]
    [Range(0, 10)]
    public float shootingRange = 10f; // Дистанция для стрельбы
    [Tooltip("Дистанция обхода препятствий")]
    public Transform[] PointPositions;
    public float DistanceToChangePoint;
    private int NowPositionNumber = 0;
    private bool IsWalk;
    [Space(2)]
    [Header("Для стрельбы")]
    [Space]
    [Tooltip("Время стрельбы, сколько ждать, в 0.2с")]
    [Range(0, 1000)]
    public float timeattack;
    [Tooltip("Дальность стрельбы")]
    [Range(0, 100)]
    public float power;
    [Tooltip("Радиус разброса пуль")]
    [Range(0, 3)]
    public float bulletSpreadRadius; // Разброс пуль
    [Tooltip("Префаб пули")]
    public GameObject bulletPrefab; // Префаб пули
    [Tooltip("Точка спавна пули")]
    public Transform bulletSpawnPoint; // Точка спавна пуль

    private NavMeshAgent navMeshAgent;
    private int time;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = shootingRange; // Устанавливаем дистанцию остановки равной дистанции для стрельбы
    }

    private void Update()
    {
        // Следование за игроком
       
     
        time++;
    }

    IEnumerator Shoot()
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
            if (hit.transform.CompareTag("Player"))
            {
                // Вызываем функцию попадания
                hit.transform.GetComponent<PlayerController>().Hit();
            }
            // Создаем пулю на месте спавна пуль и направляем ее по линии стрельбы
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(shootingDirection));

            // Наносим пуле начальную скорость или другие эффекты
            bullet.GetComponent<Rigidbody>().AddForce(power * bullet.transform.forward);
            // Уничтожаем пулю через некоторое время или при достижении максимального расстояния
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

