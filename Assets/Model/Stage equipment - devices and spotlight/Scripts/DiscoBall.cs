using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBall : BaseProjector
{
    [SerializeField] private float rotationSpeed = 9f;
    private void Start()
    {
        body = this.transform.Find("Ball");
    }
    private void Update()
    {
        ObjectRotation(body, rotationSpeed, false, true) ;
    }
}
