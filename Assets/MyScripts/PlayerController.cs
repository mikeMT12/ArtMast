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
    [Space]
    [Header("Movement")]
    public float moveSpeed = 5f; // �������� ����������� ������
    public float sprintSpeed = 10f; // �������� ����
    public float rotationSpeed = 10f; // �������� �������� ������
    public Transform Camera;
    public Vector3 CameraPoint;
    public Transform PlayerVisiable;
    private bool isSprinting = false; // ����, ������������, ����� �� �����
    [Header("Audio")]
    public AudioClip _AudioClip;
    private AudioSource audioSource;
    public float[] timeings;
    public float SoundLenth;
    private int nowTimeItem = -1;
    public float pastTime;
    [Space(2)]
    [Header("NPC")]
    public int AttacsNpc;
    [Header("Player Ataach Settngs")]
    public Transform ShootingDirectionPoint;
    public float ShootingSpredRadius;
    public Animator PlayerAnimator;
    public string[] NameOfComboAttack;
    public float AttackDistance;

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
    }
    public void Hit(float damage)
    {
        PlayerHealth -= damage;
        if (DamageEffect != null) { Instantiate(DamageEffect, transform); Destroy(DamageEffect, 1); }
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
            //��� ��� ��� ����
            SceneManager.LoadScene(0);
            Destroy(gameObject, 1);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bool IsCombo = false;
            for (int i = nowTimeItem + 1; i < timeings.Length; i++)
            {
                if (timeings[i] - 0.2f <= pastTime && pastTime <= timeings[i] + 0.2f)
                {
                    IsCombo = true;
                    nowTimeItem = i;
                    ComboAttack();
                    break;
                }

            }
            if (!IsCombo) { Attack(); }

        }
        // �������� ������� �������� ���� ��������������� � ������������� ����� (WASD ��� �������)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if (horizontalInput != 0 || verticalInput != 0) Walk(horizontalInput, verticalInput);
        else { PlayerAnimator.Play("idle"); PlayerAnimator.SetInteger("speed", 0); }
    }

    void Walk(float horizontalInput, float verticalInput)
    {
            // ���������, ������ �� ������ Shift ��� ����
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
        // ��������� ������� �������� ������ � ����������� �� ����
        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

            // ������� ������ ����������� �������� �� ������ ������� ��������
            Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

            // ���� ����� ��������, �� ������������ ��� � ������� ��������
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                PlayerVisiable.transform.rotation = Quaternion.Lerp(PlayerVisiable.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // ���������� ������ � ��������� ����������� � ������ ��������
            transform.Translate(moveDirection * currentSpeed * Time.deltaTime);
            Camera.position = transform.position + CameraPoint;
    }

    void Attack()
    {
        // ��������� ����������� ��������
        Vector3 shootingDirection = ShootingDirectionPoint.position - transform.position;

        // ��������� ������� ����
        Vector3 bulletSpread = UnityEngine.Random.insideUnitCircle * ShootingSpredRadius;
        shootingDirection.x += bulletSpread.x;
        shootingDirection.y += bulletSpread.y;

        // ��������� ��� � ����������� ������
        RaycastHit hit;
        if (Physics.Raycast(transform.position, shootingDirection, out hit))
        {
            // ��������� ��������� � ������
            if (hit.transform.GetComponent<NPCController>() != null )
            {
                float distanceToNPC = Vector3.Distance(transform.position, hit.transform.position);
                if(distanceToNPC <= AttackDistance)
                // �������� ������� ���������
                hit.transform.GetComponent<NPCController>().OnHit(1);
            }
            if (PlayerAnimator != null) { PlayerAnimator.Play("Attack"); }
        }
    }

    void ComboAttack()
    {
        // ��������� ����������� ��������
        Vector3 shootingDirection = transform.position - transform.position;

        // ��������� ������� ����
        Vector3 bulletSpread = UnityEngine.Random.insideUnitCircle * ShootingSpredRadius;
        shootingDirection.x += bulletSpread.x;
        shootingDirection.y += bulletSpread.y;

        // ��������� ��� � ����������� ������
        RaycastHit hit;
        if (Physics.Raycast(transform.position, shootingDirection, out hit))
        {
            // ��������� ��������� � ������
            if (hit.transform.GetComponent<NPCController>() != null)
            {
                // �������� ������� ���������
                hit.transform.GetComponent<NPCController>().OnHit(10);
            }
            if (PlayerAnimator != null) { PlayerAnimator.Play(NameOfComboAttack[UnityEngine.Random.Range(0, NameOfComboAttack.Length)]); }
        }
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

}