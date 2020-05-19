using NaughtyCharacter;
using UnityEngine;
using UnityEngine.Events;

public enum CharacterActionType { Build, DebugLog }

// This class will have shit like building and shooting ak-47 etc.
public class CharacterActionController : MonoBehaviour
{
    public CharacterActionType ActionType;
    public UnityEvent OnBuild;
    public UnityEvent OnDebugLog;

    public void CharacterClicked(Character character)
    {
        switch (ActionType)
        {
            case CharacterActionType.Build:
                OnBuild.Invoke();
                break;
            case CharacterActionType.DebugLog:
                OnDebugLog.Invoke();
                break;
        }
    }
}