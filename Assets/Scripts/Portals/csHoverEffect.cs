using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Serialization;

public class csHoverEffect : MonoBehaviour
{
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float _stepTracker = 0f;
    [FormerlySerializedAs("SpeedMultiplier")] public float speedMultiplier = 0.1f;
    [FormerlySerializedAs("RangeMultiplier")] public float rangeMultiplier = 0.1f;
    public float delayStartupMilliSeconds = 0;
    private bool _directionUp = true;
    
    // Start is called before the first frame update
    void Start()
    {
        var transform1 = transform;
        _startPosition = transform1.position;
        _endPosition = _startPosition + (transform1.up*rangeMultiplier);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (delayStartupMilliSeconds <= 0)
        {

            transform.SetPositionAndRotation(Vector3.Lerp(_startPosition, _endPosition, _stepTracker), transform.rotation);
            if (_directionUp)
            {
                _stepTracker += Time.fixedDeltaTime*speedMultiplier;
            }
            else
            {
                _stepTracker -= Time.fixedDeltaTime*speedMultiplier;
            }
        
            if (_stepTracker >= 1 || _stepTracker <= 0)
            {
                _directionUp = !_directionUp;
            }
        }
        else
        {
            delayStartupMilliSeconds -= Time.fixedDeltaTime * 1000;
        }
    }
}
