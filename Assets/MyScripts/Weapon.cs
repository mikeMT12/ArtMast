using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public float damage;
    public int comboLength;

    BoxCollider triggerBox;
    public bool attackState = false;
    public GameObject player;
    


    private void Start()
    {
        triggerBox = GetComponent<BoxCollider>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<EnemyAIPatrol>();
        if(enemy != null)
        {
            enemy.health.HP -= damage;
            if(enemy.health.HP <= 0)
            {
                Destroy(enemy.gameObject);
            }
        }
    }

    public void EnableTriggerBox()
    {
        triggerBox.enabled = true;
    }

    public void DisableTriggerBox()
    {
        triggerBox.enabled = false;
    }

    public void DrawWeapon()
    {
        attackState = true;
        var animator = player.GetComponent<Animator>();
        animator.SetBool("DrawGitar", true);

    }

    public void SheathWeapon()
    {
        attackState = false;
        var animator = player.GetComponent<Animator>();
        animator.SetBool("DrawGitar", false);

    }
}
