﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Serialization;

public enum ActionType { Attack, Axe, Mine, Map };

public class csActionsManager : MonoBehaviour
{
    private ActionType _selectedAction = ActionType.Attack;
    [FormerlySerializedAs("Player")] public GameObject player;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void DoAction()
    {
        if (GameVariables.IsActionModeEnabled)
        {
            switch (_selectedAction)
            {
                case ActionType.Attack:
                    Debug.Log("Attack");
                    break;
                case ActionType.Axe:
                    var rot = player.transform.rotation;
                    var playerForward = player.transform.forward;
                    Debug.Log(playerForward);
                    var pos = player.transform.position + playerForward;
                    RaycastHit[] raycastHits = Physics.BoxCastAll(new Vector3(pos.x, pos.y+1f, pos.z), 
                        new Vector3(0.5f, 0.5f, 0.5f), playerForward, Quaternion.identity, 1f);
                    for (var i = raycastHits.Length-1; i >= 0; i--)
                    {
                        if (raycastHits[i].transform.CompareTag("CanBeThwackdable"))
                        {
                            var thwackDetails = raycastHits[i].transform.GetComponent<csThwackDetails>();
                            var gb = Instantiate(thwackDetails.resourcePrefab, raycastHits[i].transform.position, Quaternion.identity);
                            gb.GetComponent<Rigidbody>().velocity = Random.onUnitSphere * 1;
                            Debug.Log(thwackDetails.dropCount);
                            Debug.Log(thwackDetails.dropResouceType);
                            break;
                        }
                    }
                    Debug.Log("Axe");
                    break;
                case ActionType.Mine:
                    Debug.Log("Mine");
                    break;
                case ActionType.Map:
                    Debug.Log("Map");
                    break;
                
            }
        }
    }

    public void SelectActionInt(int actionIndex)
    {
        SelectAction((ActionType)actionIndex-1);
    }

    public void SelectAction(ActionType actionType)
    {
        _selectedAction = actionType;
    }
}
