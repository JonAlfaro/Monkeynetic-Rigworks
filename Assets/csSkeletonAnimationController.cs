using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csSkeletonAnimationController : MonoBehaviour
{
    private Animation _anim;
    private Vector3 _lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        _lastPosition = transform.position;
        _anim = GetComponent<Animation>(); 
        _anim.Play("skeleton-skeleton|run");
    }

    // Update is called once per frame
    void Update()
    { 
        var pos = transform.position;
        if (pos != _lastPosition )
        {
            if (_anim.IsPlaying("skeleton-skeleton|idle") || !_anim.isPlaying)
            {
                _anim.Play("skeleton-skeleton|run");
            }
        }
        else
        {
            if (!_anim.isPlaying || _anim.IsPlaying("skeleton-skeleton|run"))
            {
                _anim.Play("skeleton-skeleton|idle");
            }
        }
        
        _lastPosition = pos;
    }
}
