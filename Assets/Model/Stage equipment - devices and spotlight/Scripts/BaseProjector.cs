using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class BaseProjector : MonoBehaviour
{
    protected Transform bracket, body;

    protected void ObjectRotation(Transform objectToRotate, float speed = 2f, bool x = false, bool y = false, bool z = false)
    {

        float xAxis = 0f, yAxis = 0f, zAxis = 0f;
        if (x != false)
        {
            xAxis = speed * Time.deltaTime;
        }

        if (y != false)
        {
            yAxis = speed * Time.deltaTime;
        }

        if (z != false)
        {
            zAxis = speed * Time.deltaTime;
        }
        objectToRotate.Rotate(xAxis, yAxis, zAxis);


    }

}
