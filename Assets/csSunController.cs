using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class csSunController : MonoBehaviour
{
    [FormerlySerializedAs("SkyboxSpeed")] public float skyboxSpeed;
    private static readonly int Rotation = Shader.PropertyToID("_Rotation");
    private static readonly int SunDirection = Shader.PropertyToID("_SunDirection");

    // Update is called once per frame
    void FixedUpdate()
    {
        // transform.localRotation *= Quaternion.AngleAxis(skyboxSpeed * Time.deltaTime, Vector3.right);
        // Shader.SetGlobalVector(SunDirection, transform.forward);
        Vector3 angle = transform.eulerAngles;
        float x = angle.x;
        float y = angle.y;
        float z = angle.z;

        if (Vector3.Dot(transform.up, Vector3.up) >= 0f)
        {
            if (angle.x >= 0f && angle.x <= 90f)
            {
                x = angle.x;
            }
            if (angle.x >= 270f && angle.x <= 360f)
            {
                x = angle.x - 360f;
            }
        }
        if (Vector3.Dot(transform.up, Vector3.up) < 0f)
        {
            if (angle.x >= 0f && angle.x <= 90f)
            {
                x = 180 - angle.x;
            }
            if (angle.x >= 270f && angle.x <= 360f)
            {
                x = 180 - angle.x;
            }
        }

        if (angle.y > 180)
        {
            y = angle.y - 360f;
        }

        if (angle.z > 180)
        {
            z = angle.z - 360f;
        }

        // Debug.Log(angle + " :::: " + Mathf.Round(x) + " , " + Mathf.Round(y) + " , " + Mathf.Round(z));
        // if (Mathf.Round(x) >= 0)
        // {
        //     Debug.Log(x);
        //     Shader.SetGlobalVector("_SkyAngle", new Vector4(x,0));
        // }
        // else
        // {
        //     Debug.Log(360+x);
        //     Shader.SetGlobalVector("_SkyAngle", new Vector4(360+x,0));
        // }
    }
}
