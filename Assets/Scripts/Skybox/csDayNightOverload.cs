using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class DayNightOverload : MonoBehaviour
{
    private float _incrementAmount = 6f;

    private bool _manualRotation = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            if (!_manualRotation)
            {
                _incrementAmount += 6f;
            }
            else
            {
                transform.Rotate(3f, 0, 0);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f ) // backwards
        {
            if (!_manualRotation&& _incrementAmount > 0f)
            {
                _incrementAmount -= 6f;
            }
            else
            {
                transform.Rotate(-3f, 0, 0);
            }
        }
        
        if (Input.GetMouseButtonDown(2))
        {
            _manualRotation = !_manualRotation;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_manualRotation)
        {
            transform.Rotate(_incrementAmount / 60, 0, 0);
        }
    }
}