using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { set; get; }


    public int AttacsNpc;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    public void Hit()
    {
        print("HIT");
    }
}
