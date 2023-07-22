using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public AudioClip _AudioClip;
    public float[] timeings;

    public int nowTimeItem = -1;
    private AudioSource audioSource;
    public float pastTime;
   /* [Range(0.1f, 1f)]
    public float volumeScale = 0.5f;*/
   public PlayerCombat playerCombat;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
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
        print("attack");
    }

    void ComboAttack()
    {
        print("combo");
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