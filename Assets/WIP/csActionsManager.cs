using UnityEngine;
using UnityEngine.Serialization;

public enum ActionType { Forage, Sell, Eat_Apple};

public class csActionsManager : MonoBehaviour
{
    private ActionType _selectedAction = ActionType.Forage;
    [FormerlySerializedAs("Player")] public GameObject player;
    [FormerlySerializedAs("ResourceManager")] public GameObject resourceManager;
    public float shootUpSpeed = 2;
    
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
                case ActionType.Forage:
                    var rot = player.transform.rotation;
                    var playerForward = player.transform.forward;
                    Debug.Log(playerForward);
                    var pos = player.transform.position + playerForward;
                    RaycastHit[] raycastHits = Physics.BoxCastAll(new Vector3(pos.x, pos.y+1f, pos.z), 
                        new Vector3(0.5f, 0.5f, 0.5f), playerForward, Quaternion.identity, 1f);
                    for (var i = raycastHits.Length-1; i >= 0; i--)
                    {
                        if (raycastHits[i].transform.CompareTag("OrangeChest"))
                        {
                            raycastHits[i].transform.GetComponent<csOrangeChest>().OpenChest();
                            break;
                        }

                        if (raycastHits[i].transform.CompareTag("CanBeThwackdable"))
                        {
                            var thwackDetails = raycastHits[i].transform.GetComponent<csThwackDetails>();
                            var gb = Instantiate(thwackDetails.resourcePrefab, raycastHits[i].transform.position, Quaternion.identity);
                            gb.GetComponent<Rigidbody>().velocity = (Random.onUnitSphere * 1 + (gb.transform.up*shootUpSpeed));
                            gb.GetComponent<csResourceDetails>().fruitAmount = thwackDetails.dropCount;
                            gb.GetComponent<csResourceDetails>().increaseResource.AddListener(resourceManager.GetComponent<csResourceManager>().IncreaseFruit);
                            break;
                        }
                    }
                    break;
                case ActionType.Sell:
                    resourceManager.GetComponent<csResourceManager>().SellAll();
                    break;
                case ActionType.Eat_Apple:
                    resourceManager.GetComponent<csResourceManager>().EatApples();
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
