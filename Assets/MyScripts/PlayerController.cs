using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { set; get; }
    public event Action StartNPC;

    [Header("Player Settings")]
    public float PlayerHealth;
    public Scrollbar HealthScrollbar;
    public GameObject DamageEffect;
    public Transform PlayerTransform;
    [Space]
    [Header("Movement")]
    public float moveSpeed = 5f; // Скорость перемещения игрока
    public float sprintSpeed = 10f; // Скорость бега
    public float rotationSpeed = 10f; // Скорость поворота игрока
    public Transform Camera;
    public Vector3 CameraPoint;
    public Transform PlayerVisiable;
    private bool isSprinting = false; // Флаг, определяющий, бежит ли игрок
    [Header("Audio")]
    public AudioClip _AudioClip;
    private AudioSource audioSource;
    public float[] timeings;
    public float SoundLenth;
    public int nowTimeItem = -1;
    public float pastTime;
    [Space(2)]
    [Header("NPC")]
    public int AttacsNpc;
    [Header("Player Ataach Settngs")]
    public Transform ShootingDirectionPoint;
    public float ShootingSpredRadius;
    public Animator PlayerAnimator;
    public AnimationClip[] DefaultAttack;
    public float AttackDistance;
    public float AttackStrenghForBaf;
    [Space(2)]
    [Header("Bafs")]
    public GameObject UiStrenthBaf;
    private bool IsIHaveStrenth;
    [Header("Rooms")]
    public int[] CountNPCInRoom;

    private CharacterController CharController;
    private bool IsAttack = false;
    private float StartHealth;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartHealth = PlayerHealth;
        CharController = GetComponent<CharacterController>();
    }
    public void Hit(float damage)
    {
        PlayerHealth -= damage;
        if (DamageEffect != null)
        {
            var deffect = Instantiate(DamageEffect, PlayerTransform.position, Quaternion.EulerAngles(0, 0, 0));
            deffect.transform.parent  = null; 
            Destroy(deffect, 1); }
        HealthScrollbar.size = PlayerHealth / StartHealth;
        var cblock = HealthScrollbar.colors;
        if (HealthScrollbar.size < 0.5f)
            cblock.normalColor = Color.Lerp(Color.red, Color.yellow, HealthScrollbar.size * 2);
        else
            cblock.normalColor = Color.Lerp(Color.green, Color.yellow, HealthScrollbar.size * 2 - 1.0f);
        HealthScrollbar.colors = cblock; // <--
        if (PlayerHealth <= 0)
        {
            print("gg im death");
            //тут сам что надо
            SceneManager.LoadScene(0);
            Destroy(gameObject, 1);
        }
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && !IsAttack)
        {
            bool IsCombo = false;
            for (int i = nowTimeItem + 1; i < timeings.Length; i++)
            {
                if (timeings[i] - 0.3f <= pastTime && pastTime <= timeings[i] + 0.3f)
                {
                    IsCombo = true;
                    nowTimeItem = i;
                    ComboAttack();
                    break;
                }

            }
            if (!IsCombo) { Attack(); }

        }
        // Получаем входные значения осей горизонтального и вертикального ввода (WASD или стрелки)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if ((horizontalInput != 0 || verticalInput != 0) && !IsAttack) Walk(horizontalInput, verticalInput);
        else if (!IsAttack){ PlayerAnimator.Play("idle");
               PlayerAnimator.SetInteger("speed", 0); }
    }

    void Walk(float horizontalInput, float verticalInput)
    {
            // Проверяем, нажата ли кнопка Shift для бега
            isSprinting = Input.GetKey(KeyCode.LeftShift);
        if (isSprinting)
        {
            PlayerAnimator.Play("run");
            PlayerAnimator.SetInteger("speed", 2);
        }
        else 
        {
            PlayerAnimator.Play("walk");
            PlayerAnimator.SetInteger("speed", 1); }
        // Вычисляем текущую скорость игрока в зависимости от бега
        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

            // Создаем вектор направления движения на основе входных значений
            Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

            // Если игрок движется, то поворачиваем его в сторону движения
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                PlayerVisiable.transform.rotation = Quaternion.Lerp(PlayerVisiable.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Перемещаем игрока в указанном направлении с учетом скорости
            //transform.Translate(moveDirection * currentSpeed * Time.deltaTime);
        CharController.Move(moveDirection * currentSpeed * Time.deltaTime);
            Camera.position = transform.position + CameraPoint;
    }

    void Attack()
    {
        StartCoroutine(IsAttacktimer(0));
        // Вычисляем направление стрельбы
        Vector3 shootingDirection = ShootingDirectionPoint.position - transform.position;

        // Применяем разброс пуль
        Vector3 bulletSpread = UnityEngine.Random.insideUnitCircle * ShootingSpredRadius;
        shootingDirection.x += bulletSpread.x;
        shootingDirection.y += bulletSpread.y;
        if (PlayerAnimator != null) { PlayerAnimator.Play("attack"); }
        // Выпускаем луч в направлении игрока
        RaycastHit hit;
        if (Physics.Raycast(transform.position, shootingDirection, out hit))
        {
            // Проверяем попадание в игрока
            if (hit.transform.GetComponent<NPCController>() != null )
            {
                float distanceToNPC = Vector3.Distance(transform.position, hit.transform.position);
                if(distanceToNPC <= AttackDistance)
                // Вызываем функцию попадания
                hit.transform.GetComponent<NPCController>().OnHit(AttackStrenghForBaf);
            }
            
        }
    }

    void ComboAttack()
    {
        int attackNum = UnityEngine.Random.Range(1, DefaultAttack.Length);
        StartCoroutine(IsAttacktimer(attackNum));
        // Вычисляем направление стрельбы
        Vector3 shootingDirection = ShootingDirectionPoint.position - transform.position;

        // Применяем разброс пуль
        Vector3 bulletSpread = UnityEngine.Random.insideUnitCircle * ShootingSpredRadius;
        shootingDirection.x += bulletSpread.x;
        shootingDirection.y += bulletSpread.y;
        if (PlayerAnimator != null) { PlayerAnimator.Play(DefaultAttack[attackNum].name); }
        // Выпускаем луч в направлении игрока
        RaycastHit hit;
        if (Physics.Raycast(transform.position, shootingDirection, out hit))
        {
            // Проверяем попадание в игрока
            if (hit.transform.GetComponent<NPCController>() != null)
            {
                float distanceToNPC = Vector3.Distance(transform.position, hit.transform.position);
                if (distanceToNPC <= AttackDistance)
                    // Вызываем функцию попадания
                    hit.transform.GetComponent<NPCController>().OnHit(5);
            }
            
        }
    }

    IEnumerator IsAttacktimer(int attack)
    {
        IsAttack = true;
        yield return new WaitForSeconds(DefaultAttack[attack].length);
        IsAttack = false;
    }
    IEnumerator Timer()
    {
        while (true)
        {
            if(pastTime == SoundLenth)
            {

                pastTime = 0;
                nowTimeItem = 0;
            }
            pastTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void StartNpc()
    {
        StartNPC?.Invoke();
        audioSource.PlayOneShot(_AudioClip);
        StartCoroutine(Timer());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.GetComponent<Hilka>() != null)
        {
            PlayerHealth += collision.transform.GetComponent<Hilka>().HillCount;
            Destroy(collision.transform.gameObject);
        }
        else if(collision.transform.GetComponent<Strenth>() != null)
        {
            UiStrenthBaf.SetActive(true);
            IsIHaveStrenth = true;
            Destroy(collision.transform.gameObject);
        }
        else if(collision.transform.GetComponent<AplayBafs>() != null && IsIHaveStrenth)
        {
            UiStrenthBaf.SetActive(false);
            IsIHaveStrenth = false;
            AttackStrenghForBaf += collision.transform.GetComponent<AplayBafs>().StrenthCount;
        }
    }
}