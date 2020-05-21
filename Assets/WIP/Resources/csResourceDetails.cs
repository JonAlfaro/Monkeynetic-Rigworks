using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class csResourceDetails : MonoBehaviour
{
    [FormerlySerializedAs("FruitType")] public FruitResourceType fruitType;
    [FormerlySerializedAs("FruitAmount")] public int fruitAmount = 1;
    [FormerlySerializedAs("IncreaseResource")] public UnityEvent<FruitResourceType,int> increaseResource;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter(Collider colider)
    {
        Debug.Log("zzzzzzzzzzzzzzzzzzzzzzzzzzzzz");
        switch (colider.gameObject.name)
        {
            case "pfPlayerWithControlles":
                Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                increaseResource.Invoke(fruitType,fruitAmount);
                break;
            case "PlayerTwoWithCamera":
                break;
            default:
                return;
        }
    }
}
