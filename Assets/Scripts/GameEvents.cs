using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public UnityEvent<bool> OnToggleBuildMode;

    public void HandleInput(KeyCode key)
    {
        switch (key) {
            case KeyCode.B:
                GameVariables.IsBuildModeEnabled = !GameVariables.IsBuildModeEnabled;
                OnToggleBuildMode.Invoke(GameVariables.IsBuildModeEnabled);
                break;
        }
    }
}
