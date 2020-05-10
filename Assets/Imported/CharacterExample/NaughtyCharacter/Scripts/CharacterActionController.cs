using NaughtyCharacter;
using UnityEngine;

public enum CharacterActionType { SpawnThingy, DebugLog }

// This class will have shit like building and shooting ak-47 etc.
public class CharacterActionController : MonoBehaviour
{
    public CharacterActionType ActionType;
    public GameObject ThingyToSpawn;

    public void CharacterClicked(Character character)
    {
        switch (ActionType)
        {
            case CharacterActionType.DebugLog:
                DebugLog();
                break;
            case CharacterActionType.SpawnThingy:
                SpawnThingy();
                break;
        }
    }

    private void DebugLog()
    {
        Debug.Log("Action Fired");
    }

    private void SpawnThingy()
    {
        Vector3 forwardFromRotation = transform.position + (transform.rotation * Vector3.forward);
        Instantiate(ThingyToSpawn, forwardFromRotation, Quaternion.identity);
    }
}