using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public GameObject weapon0;
    [SerializeField] private PlayerInput playerInput;

    public GameObject[] animTargets;
    Transform position;


    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerInput = GetComponent<PlayerInput>();
        position = GetComponent<Transform>();


    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButtonDown("Fire1"))
        {
           

            if (weapon.attackState == false)
            {
/*                var animator = weapon.player.GetComponent<Animator>();
                animator.SetBool("DrawGitar", true);*/
                PlaySound();
                weapon.attackState = true;


            }

            if(Time.time - lastComboEnd > timeBetweenCombos && comboCounter <= combo.Count)
            {
                if (Time.time - lastClickedTime >= 0.9f)
                {


                    //if (Time.time - lastClickedTime >= 0.2f)

                    for (int i = nowTimeItem + 1; i < timeings.Length; i++)
                    {
                        if (timeings[i] - 0.5f <= pastTime && pastTime <= timeings[i] + 0.5f)
                        {
                            weapon.gameObject.SetActive(true);
                            weapon0.gameObject.SetActive(false);
                            CancelInvoke("EndCombo");
                            animator.runtimeAnimatorController = combo[comboCounter].animatorOV;
                            playerInput.enabled = false;
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
        }*/

    }

    public void ExitAttack()
    {
        
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke("EndCombo", 1);
            weapon0.gameObject.SetActive(true);
            weapon.gameObject.SetActive(false);
            playerInput.enabled = true;

            
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


