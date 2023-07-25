using System.Collections;
using UnityEngine;

public class ProjectorSimpleRotation : BaseProjector
{
    [Header("Body Rotation")]

    [Tooltip("Control Body rotation speed. Not moving if 0")]
    [SerializeField] [Range(0f, 2f)] protected float bodyRotationSpeed = 2f;

    [Tooltip("Control Body rotation angle. Not moving if 0")]
    [SerializeField] [Range(0f, 100f)] protected float bodyRotationAngle = 100f;

    [Header("Bracket Rotation")]

    [Tooltip("Control Bracket rotation speed. Not moving if 0")]
    [SerializeField] [Range(-30f, 30f)] private float spinSpeed = 3f;
    void Start()
    {
        bracket = this.transform.Find("Bracket");
        body = bracket.transform.Find("Body");
    }



    void Update()
    {
            SimpleBehavior();
    }
    void SimpleBehavior()
    {
        ObjectRotation(bracket, spinSpeed, false, true);//y rotation
        BodyRotation();
    }
   

    void BodyRotation() // sin rotation
    {
        body.transform.localRotation = Quaternion.Euler(bodyRotationAngle * Mathf.Sin(Time.time * bodyRotationSpeed), 0f, 0f);

    }

}
