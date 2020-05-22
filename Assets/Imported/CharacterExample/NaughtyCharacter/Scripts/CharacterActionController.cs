using UnityEngine;
using UnityEngine.Events;

public class CharacterActionController : MonoBehaviour
{
    public UnityEvent OnClick;
    public UnityEvent OnRightClick;
    public UnityEvent<KeyCode> OnKey;
}