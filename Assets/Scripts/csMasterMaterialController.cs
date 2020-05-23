using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class csMasterMaterialController : MonoBehaviour
{
    [FormerlySerializedAs("MaterialColor")] public Color materialColor;
    [FormerlySerializedAs("DissolveColor")] public Color dissolveColor;
    private MaterialPropertyBlock _propertyBlock;
    private Renderer _rend;
    public float secondsToDissolve = 3;
    private DissolveController _dissolveController;


    private class DissolveController
    {
        private const float StartDissolveX = -5.2f;
        private const float TimeForFullDissolveCycle = 2f;
        private bool _running;
        private float _secondCounter = 0;
        private readonly MaterialPropertyBlock _matPropBlock;
        private readonly Renderer _matRend;
        private readonly float _dissolveStep = 0f;
        private float _timeToDissolve;
        private float _dissolvePosition = StartDissolveX;
        private static readonly int Dissolve = Shader.PropertyToID("_Dissolve");
        private static readonly int DissolveColor = Shader.PropertyToID("_DissolveColor");


        public DissolveController(Boolean running, float fixedDeltaTime, float timeToDissolve, Renderer matRend, MaterialPropertyBlock matPropBlock, Color dissolveColor)
        {
            // Set Dissolve Color
            _matPropBlock = matPropBlock;
            _matRend = matRend;
            _matPropBlock.SetColor(DissolveColor, dissolveColor);
            _matRend.SetPropertyBlock(matPropBlock);
            
            // Construct Controller Properities
            _dissolveStep = (TimeForFullDissolveCycle*fixedDeltaTime)/timeToDissolve;
            _timeToDissolve = timeToDissolve;
            _running = running;
        }

        public void Reset()
        {
            _secondCounter = 0;
            _running = true;
        }
        
        public void Reset(int newTimeToDissolve)
        {
            _secondCounter = 0;
            _timeToDissolve = newTimeToDissolve;
            _running = true;
        }

        public bool Next(Renderer matRend, MaterialPropertyBlock matPropBlock)
        {
            if (!_running)
            {
                return _running;
            }
            
            // Resolve Next Dissolve Step
            _secondCounter += Time.fixedDeltaTime;
            _dissolvePosition += _dissolveStep;
            _matPropBlock.SetVector(Dissolve, new Vector4(_dissolvePosition, 0));
            _matRend.SetPropertyBlock(matPropBlock);
            
            if (_secondCounter >= _timeToDissolve)
            {
                _running = false;
            }
            return true;
        }
    }
    
    // Start is called before the first frame update

    private void Start()
    {
        _propertyBlock = new MaterialPropertyBlock();
        _rend = GetComponent<Renderer>();
        _dissolveController = new DissolveController(true, Time.fixedDeltaTime, secondsToDissolve, _rend, _propertyBlock, dissolveColor);
    }

    void FixedUpdate(){
        _dissolveController.Next(_rend, _propertyBlock);
    }


}
