using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class csTeleportingSystem : MonoBehaviour
{
    [FormerlySerializedAs("_freezePlayer1")]
    public UnityEvent<float> freezePlayer1;

    public GameObject Player1Object;
    public GameObject Player2Object;
    public string Player1Tag = "";
    public string Player2Tag = "";
    public Canvas Player1Canvas;
    public Canvas Player2Canvas;
    private float _player1CanvasColorStep = 0f;
    private float _player2CanvasColorStep = 0f;
    private float _canvasRate = 1f;
    private bool _isPlayer1Teleporting = false;
    private bool _isPlayer2Teleporting = false;
    public GameObject TeleportDestination;
    public Color teleportColor = Color.green;


    private void Start()
    {
        _canvasRate = Time.fixedDeltaTime / 2f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isPlayer1Teleporting)
        {
            Player1Canvas.GetComponent<Image>().color = Color.Lerp(Color.clear, teleportColor, _player1CanvasColorStep);
            if (_player1CanvasColorStep < 1)
            {
                _player1CanvasColorStep += _canvasRate;
            }
            else
            {
                // Invoke Teleport
                freezePlayer1.Invoke(10);
                Player1Object.transform.SetPositionAndRotation(TeleportDestination.transform.position,
                    Player1Object.transform.rotation);
                _player1CanvasColorStep = 0;
            }

            Debug.Log(_player1CanvasColorStep);
        }
        else
        {
            Player1Canvas.GetComponent<Image>().color = Color.Lerp(Color.clear, teleportColor, _player1CanvasColorStep);
            if (_player1CanvasColorStep > 0)
            {
                _player1CanvasColorStep -= _canvasRate;
            }
        }
        
        
    }
    

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag(Player1Tag))
        {
            _isPlayer1Teleporting = true;
        }
        
        if (other.gameObject.CompareTag(Player2Tag))
        {
            Debug.Log("Player2Object TELEPORT");
        }
    }
    void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.CompareTag(Player1Tag))
        {
            _isPlayer1Teleporting = false;
        }
        
        if (other.gameObject.CompareTag(Player2Tag))
        {
            Debug.Log("Player2Object TELEPORT");
        }
    }
}
