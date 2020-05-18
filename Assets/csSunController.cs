using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csSunController : MonoBehaviour
{

    // Update is called once per frame
    void FixedUpdate()
    {
        Shader.SetGlobalVector("_SunDirection", transform.forward);
    }
}
