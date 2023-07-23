using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { set; get; }
    [Header("Player Settings")]
    public float PlayerHealth;
    public Scrollbar HealthScrollbar;
    public GameObject DamageEffect;
    [Header("Audio")]
    public AudioClip _AudioClip;
    private AudioSource audioSource;
    public float[] timeings;
    public float SoundLenth;
    private int nowTimeItem = -1;
    private float pastTime;
    [Space(2)]
    [Header("NPC")]
    public int AttacsNpc;
    [Header("Player Ataach Settngs")]
    public Transform ShootingDirectionPoint;
    public float ShootingSpredRadius;
    public Animator PlayerAnimator;
    public string[] NameOfComboAttack;

    private float StartHealth;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayerAnimator = GetComponent<Animator>();
        StartHealth = PlayerHealth;
    }
    public void Hit(float damage)
    {
        PlayerHealth -= damage;
        if (DamageEffect != null) { Instantiate(DamageEffect, transform); Destroy(DamageEffect, 1); }
        HealthScrollbar.size = PlayerHealth / StartHealth;
        if(PlayerHealth <= 0)
        {
            print("gg im death");
            //тут сам что надо
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
    }

    void Attack()
    {
        // Вычисляем направление стрельбы
        Vector3 shootingDirection = ShootingDirectionPoint.position - transform.position;

        // Применяем разброс пуль
        Vector3 bulletSpread = Random.insideUnitCircle * ShootingSpredRadius;
        shootingDirection.x += bulletSpread.x;
        shootingDirection.y += bulletSpread.y;

        // Выпускаем луч в направлении игрока
        RaycastHit hit;
        if (Physics.Raycast(ShootingDirectionPoint.position, shootingDirection, out hit))
        {
            // Проверяем попадание в игрока
            if (hit.transform.GetComponent<NPCController>() != null)
            {
                // Вызываем функцию попадания
                hit.transform.GetComponent<NPCController>().OnHit(1);
            }
            if (PlayerAnimator != null) { PlayerAnimator.Play("Attack"); }
        }
    }

    void ComboAttack()
    {
        // Вычисляем направление стрельбы
        Vector3 shootingDirection = ShootingDirectionPoint.position - transform.position;

        // Применяем разброс пуль
        Vector3 bulletSpread = Random.insideUnitCircle * ShootingSpredRadius;
        shootingDirection.x += bulletSpread.x;
        shootingDirection.y += bulletSpread.y;

        // Выпускаем луч в направлении игрока
        RaycastHit hit;
        if (Physics.Raycast(ShootingDirectionPoint.position, shootingDirection, out hit))
        {
            // Проверяем попадание в игрока
            if (hit.transform.GetComponent<NPCController>() != null)
            {
                // Вызываем функцию попадания
                hit.transform.GetComponent<NPCController>().OnHit(2);
            }
            if (PlayerAnimator != null) { PlayerAnimator.Play(NameOfComboAttack[Random.Range(0, NameOfComboAttack.Length)]); }
        }
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(_AudioClip);
        StartCoroutine(Timer());
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
}