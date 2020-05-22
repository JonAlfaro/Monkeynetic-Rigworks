using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class csDissolveController : MonoBehaviour
{
    public Color MaterialColor;
    private MaterialPropertyBlock propertyBlock;
    private Renderer rend;
    private bool dissolveRunning = true;
    private float secondCounter = 0;
    private float dissovleStep;
    private float dissolvePosition = -5.2f;
    
    // Start is called before the first frame update

    private void Start()
    {
        propertyBlock = new MaterialPropertyBlock();
        rend = GetComponent<Renderer>();
        dissovleStep = Time.fixedDeltaTime / 3;
    }

    void FixedUpdate(){
        if (dissolveRunning)
        {
            secondCounter += Time.fixedDeltaTime;
            //Get a renderer component either of the own gameobject or of a child
            dissolvePosition += Time.fixedDeltaTime;
            var ree = new Vector4(dissolvePosition, 0);
            propertyBlock.SetVector("_Dissolve", ree);
            rend.SetPropertyBlock(propertyBlock);
    
            if (secondCounter >= 3)
            {
                dissolveRunning = false;
            }
        }
    }
}
