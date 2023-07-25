using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBalls : BaseProjector
{
    [Tooltip("Control Body rotation speed. Not moving if 0")]
    [SerializeField] [Range(-70f, 70f)] private float spinSpeed = 50f;
    [Tooltip("Control Ball rotation speed. Not moving if 0")]
    [SerializeField] [Range(-70f, 70f)] private float ballSpeed = 50f;
    private Transform left, right;
    void Start()
    {
        body = this.transform.Find("Body");
            left = body.Find("Left_Ball");
        right = body.Find("Right_Ball");

    }
    void Update()
    {
        ObjectRotation(body, spinSpeed, false, true);
        ObjectRotation(left, ballSpeed, true);
        ObjectRotation(right, -ballSpeed, true);
    }
}
