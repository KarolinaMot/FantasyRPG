using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif


	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool dash;
		public bool attack;
		public bool lockOn;
		public bool disabledlockOn = true;

		public bool sprintDisabled;
		public bool interactable = false;
		public bool interact;
		private int timer = 0;

		[Header("Movement Settings")]
		public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnDash(InputValue value)
		{
			DashInput(value.isPressed);
		}
		
		public void OnAttack(InputValue value)
		{
			AttackInput(value.isPressed);
		}

		public void OnLockOn(InputValue value){
			LockOnInput(value.isPressed);
		}

		public void OnInteract(InputValue value){
			InteractInput(value.isPressed);
		}
#else
	// old input sys if we do decide to have it (most likely wont)...
#endif
		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void DashInput(bool newDashState)
		{
			if(!sprintDisabled)
			dash = newDashState;
		}

		public void AttackInput (bool newAttackState)
		{
		  attack = newAttackState;
		  Debug.Log("Attack input");
		}

		public void LockOnInput(bool newLockOnState){
			if(!disabledlockOn){
				if(!lockOn && newLockOnState)
					lockOn = true;
				else if(lockOn && newLockOnState){
					lockOn = false;
				}
				return;
			}
			lockOn = false;
		}

		public void InteractInput(bool newInteractState){
			if(interactable)
			interact = true;
		}
#if !UNITY_IOS || !UNITY_ANDROID

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

#endif
	}
	
