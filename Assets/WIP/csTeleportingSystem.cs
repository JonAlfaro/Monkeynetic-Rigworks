using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class csTeleportingSystem : MonoBehaviour
{
    [FormerlySerializedAs("_freezePlayer1")]
    public UnityEvent<float> freezePlayer1;

    // Start is called before the first frame update
    void Start()
    {
        freezePlayer1.Invoke(20000f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
