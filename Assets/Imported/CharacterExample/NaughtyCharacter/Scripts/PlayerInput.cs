using System.Collections.Generic;
using UnityEngine;

namespace NaughtyCharacter
{
	public class PlayerInput : MonoBehaviour
	{
		public float MoveAxisDeadZone = 0.2f;
        public KeyCode[] keysToDetect = new[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.B };

        public Vector2 MoveInput { get; private set; }
		public Vector2 LastMoveInput { get; private set; }
		public Vector2 CameraInput { get; private set; }
		public bool JumpInput { get; private set; }
        public bool ClickInput { get; private set; }
        public bool RightClickInput { get; private set; }
        public List<KeyCode> KeyInput { get; private set; }

        public bool HasMoveInput { get; private set; }

		public void UpdateInput()
		{
			// Update MoveInput
			Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			if (Mathf.Abs(moveInput.x) < MoveAxisDeadZone)
			{
				moveInput.x = 0.0f;
			}

			if (Mathf.Abs(moveInput.y) < MoveAxisDeadZone)
			{
				moveInput.y = 0.0f;
			}

			bool hasMoveInput = moveInput.sqrMagnitude > 0.0f;

			if (HasMoveInput && !hasMoveInput)
			{
				LastMoveInput = MoveInput;
			}

			MoveInput = moveInput;
			HasMoveInput = hasMoveInput;

			// Update other inputs
			CameraInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			JumpInput = Input.GetButton("Jump");
            ClickInput = Input.GetMouseButtonDown(0);
            RightClickInput = Input.GetMouseButtonDown(1);
            KeyInput = new List<KeyCode>();
            foreach (KeyCode keyCode in keysToDetect)
            {
                if (Input.GetKeyDown(keyCode))
                {
                    KeyInput.Add(keyCode);
                }
            }
        }
	}
}
