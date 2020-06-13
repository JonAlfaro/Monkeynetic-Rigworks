using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class csResourceDetails : MonoBehaviour
{
    [FormerlySerializedAs("FruitType")] public FruitResourceType fruitType;
    [FormerlySerializedAs("FruitAmount")] public int fruitAmount = 1;
    [FormerlySerializedAs("IncreaseResource")] public UnityEvent<FruitResourceType,int> increaseResource;

    [FormerlySerializedAs("MilliSecondsUntilFreeze")] public float milliSecondsUntilFreeze = 5000f;
    [FormerlySerializedAs("MilliSecondsUntilDespawn")] public float milliSecondsUntilDespawn = 180000f;
    private bool _isFrozen = false;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        milliSecondsUntilDespawn -= Time.fixedDeltaTime * 1000;
        if (milliSecondsUntilDespawn <= 0)
        {
            Destroy(gameObject);
        }
        
        if (_isFrozen) return;
        milliSecondsUntilFreeze -= Time.fixedDeltaTime * 1000;
        if (milliSecondsUntilFreeze <= 0)
        {
            _isFrozen = true;
            _rb.isKinematic = true;
        }

    }
    private void OnTriggerEnter(Collider colider)
    {
        switch (colider.gameObject.name)
        {
            case "pfPlayerWithControlles":
                increaseResource.Invoke(fruitType,fruitAmount);
                Destroy(gameObject);
                break;
            case "PlayerTwoWithCamera":
                break;
            default:
                return;
        }
    }
}
