using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public List<AttackSO> combo;
    float lastClickedTime;
    float lastComboEnd;
    int comboCounter;
    [Range(0.1f, 2f)]
    public float timeBetweenCombos = 0.2f;
    Animator animator;
    [SerializeField] Weapon weapon;
    public float timeBetweenDraw = 0.6f;

    public AudioClip _AudioClip;
    public float[] timeings;

    public int nowTimeItem = -1;
    private AudioSource audioSource;
    public float pastTime;
    public GameObject ParentForWeapon;
    public Vector3 WeaponInHandP;
    public Quaternion WeaponInHandR;
    //Vector3(2.43099999,1.19400001,-0.953000009)
    //Quaternion(0.102721341,0.146560788,0.82362324,0.538157105)

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (weapon.attackState == false)
            {
                weapon.DrawWeapon();
                PlaySound();    

                weapon.transform.position = new Vector3(2.4f, 1.19400001f, -0.953000009f);
                weapon.transform.rotation = new Quaternion(0.102721341f, 0.146560788f, 0.82362324f, 0.538157105f);
                weapon.transform.parent = ParentForWeapon.transform;
                Debug.Log("1");
                weapon.attackState = true;

            }
                
            for (int i = nowTimeItem + 1; i < timeings.Length; i++)
            {
                if (timeings[i] - 0.5f <= pastTime && pastTime <= timeings[i] + 0.5f)
                {
                    CancelInvoke("EndCombo");
                    animator.runtimeAnimatorController = combo[comboCounter].animatorOV;
                    animator.Play("Attack", 0, 0);
                    weapon.damage = combo[comboCounter].damage;
                    comboCounter++;
                    lastClickedTime = Time.time;

                    if (comboCounter + 1 > combo.Count)
                    {
                        comboCounter = 0;
                    }
                    nowTimeItem = i;
                    break;
                }

            }
        }
        ExitAttack();

    }

    public void Attack()
    {
        /*if (Time.time - lastComboEnd > timeBetweenCombos && comboCounter <= combo.Count)
        {
            CancelInvoke("EndCombo");

            if (Time.time - lastClickedTime >= 0.2f)
            {
                animator.runtimeAnimatorController = combo[comboCounter].animatorOV;
                animator.Play("Attack", 0, 0);
                weapon.damage = combo[comboCounter].damage;
                comboCounter++;
                lastClickedTime = Time.time;

                if (comboCounter + 1 > combo.Count)
                {
                    comboCounter = 0;
                }
            }
        }
                CancelInvoke("EndCombo");

                animator.runtimeAnimatorController = combo[comboCounter].animatorOV;
                animator.Play("Attack", 0, 0);
                weapon.damage = combo[comboCounter].damage;
                comboCounter++;
                lastClickedTime = Time.time;

                if (comboCounter + 1 > combo.Count)
                {
                    comboCounter = 0;
                }*/


    }

    public void ExitAttack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke("EndCombo", 1);
        }
    }

    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(_AudioClip);
        Debug.Log(audioSource.isPlaying);
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        while (true)
        {
            pastTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}


