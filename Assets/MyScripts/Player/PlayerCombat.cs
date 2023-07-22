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
    //Vector3(-2.69199991,-0.563000023,-0.204999998)
    //Quaternion(-0.395024359,-0.56962961,-0.445218742,0.566796422)
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
                
/*                var animator = weapon.player.GetComponent<Animator>();
                animator.SetBool("DrawGitar", true);*/
                PlaySound();

                WeaponTranslate();
                
              
                weapon.attackState = true;


            }
            if(weapon.attackState == true)
            {
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

    void WeaponTranslate()
    {
        weapon.transform.position = new Vector3(1.74f, 1.67f, -1.37f);
        weapon.transform.Rotate(-55.854f, -60.17f, -176.169f, Space.Self);
        weapon.transform.parent = ParentForWeapon.transform;
    }
}


