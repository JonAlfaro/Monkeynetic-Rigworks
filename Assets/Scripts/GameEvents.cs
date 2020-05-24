using UnityEngine;
using UnityEngine.Events;


public class GameEvents : MonoBehaviour
{
    public UnityEvent<bool> OnToggleBuildMode;
    public UnityEvent<int> OnNumberedAction;


    public void HandleInput(KeyCode key)
    {
        switch (key) {
            case KeyCode.B:
                GameVariables.IsBuildModeEnabled = !GameVariables.IsBuildModeEnabled;
                OnToggleBuildMode.Invoke(GameVariables.IsBuildModeEnabled);
                // Switch back to Action Mode
                GameVariables.IsActionModeEnabled = !GameVariables.IsBuildModeEnabled ? true : false;
                break;
            case KeyCode.Escape:
                GameVariables.IsMenuMode = !GameVariables.IsMenuMode;
                GameVariables.IsActionModeEnabled = !GameVariables.IsMenuMode ? true : false;
                break;
            case KeyCode.Alpha0:
            case KeyCode.Alpha1:
            case KeyCode.Alpha2:
            case KeyCode.Alpha3:
            case KeyCode.Alpha4:
            case KeyCode.Alpha5:
            case KeyCode.Alpha6:
            case KeyCode.Alpha7:
            case KeyCode.Alpha8:
            case KeyCode.Alpha9:
                OnNumberedAction.Invoke(UtilityMethods.KeyCodeToInt(key));
                break;
        }
    }
}
