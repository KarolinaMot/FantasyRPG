using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */


	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class ThirdPersonController : MonoBehaviour
	{
		[Header("Player")]
		public float dashSpeed = 90;
		[Tooltip("Move speed of the character in m/s")]

		public float walkSpeed = 4.5f;
		public float moveSpeed = 2.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float sprintSpeed = 5.335f;
		[Tooltip("How fast the character turns to face movement direction")]
		[Range(0.0f, 0.3f)]
		public float rotationSmoothTime = 0.12f;
		[Tooltip("Acceleration and deceleration")]
		public float speedChangeRate = 10.0f;
		[HideInInspector]
		public float targetSpeed;
		public bool moving = false;
		public float dashTime =  2;


	    [Space(10)]
		[Tooltip("The height the player can jump")]
		public float jumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float gravity = -15.0f;
		public bool isJumping = false;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float jumpTimeout = 0.50f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float fallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool grounded = true;
		[Tooltip("Useful for rough ground")]
		public float groundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float groundedRadius = 0.28f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask groundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject cinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float topClamp = 70.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float bottomClamp = -30.0f;
		[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
		public float cameraAngleOverride = 0.0f;
		[Tooltip("For locking the camera position on all axis")]
		public bool lockCameraPosition = false;



		// cinemachine
		private float cinemachineTargetYaw;
		private float cinemachineTargetPitch;

		// player
		private float speed;
		private float animationBlend;
		private float targetRotation = 0.0f;
		private float rotationVelocity;
		private float verticalVelocity;
		private float terminalVelocity = 53.0f;
		public  bool isDashing = false;

		public bool isSprinting = true;	
		private bool canDash = true;
		public float timer = 0;

		// timeout deltatime
		private float jumpTimeoutDelta;
		private float fallTimeoutDelta;

		// animation IDs
		private int animIDSpeed;
		private int animIDGrounded;
		private int animIDJump;
		private int animIDFreeFall;
		private int animIDMotionSpeed;

		private Animator animator;
		private CharacterController controller;
		private CombatManager combatManager;
		private StarterAssetsInputs input;
		private GameObject mainCamera;
	

		private const float threshold = 0.01f;

		private bool hasAnimator;

		private CharacterStats playerStats;

		private void Awake()
		{
			// get a reference to our main camera
			if (mainCamera == null)
			{
				mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
			playerStats = GetComponent<CharacterStats>();
		}

		private void Start()
		{
			hasAnimator = TryGetComponent(out animator);
			controller = GetComponent<CharacterController>();
			input = GetComponent<StarterAssetsInputs>();
			combatManager = GetComponent<CombatManager>();

			AssignAnimationIDs();

			// reset our timeouts on start
			jumpTimeoutDelta = jumpTimeout;
			fallTimeoutDelta = fallTimeout;
		}

		private void Update()
		{
			hasAnimator = TryGetComponent(out animator);
			JumpAndGravity();
			GroundedCheck();
			Move();
		}

		private void LateUpdate()
		{
			CameraRotation();
		}

		private void AssignAnimationIDs()
		{
			animIDSpeed = Animator.StringToHash("Speed");
			animIDGrounded = Animator.StringToHash("Grounded");
			animIDJump = Animator.StringToHash("Jump");
			animIDFreeFall = Animator.StringToHash("FreeFall");
			animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
			grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);

			// update animator if using character
			if (hasAnimator)
			{
				animator.SetBool(animIDGrounded, grounded);
			}
		}

		private void CameraRotation()
		{
			// if there is an input and camera position is not fixed
			if (input.look.sqrMagnitude >= threshold && !lockCameraPosition)
			{
				cinemachineTargetYaw += input.look.x * Time.deltaTime;
				cinemachineTargetPitch += input.look.y * Time.deltaTime;
			}

			// clamp our rotations so our values are limited 360 degrees
			cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
			cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

			// Cinemachine will follow this target
			cinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + cameraAngleOverride, cinemachineTargetYaw, 0.0f);
		}
		
		private void Move()
		{

			SetSpeeed();

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);

				// round speed to 3 decimal places
				speed = Mathf.Round(speed * 1000f) / 1000f;
			}
			else
			{
				speed = targetSpeed;
			}
			animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);

			// normalise input direction
			Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if ((input.move != Vector2.zero && !combatManager.isAttacking || input.move != Vector2.zero && isDashing) && !input.lockOn)
			{
				targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
				float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);
				// rotate to face input direction relative to camera position
				transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
			}

			Vector3 targetDirection = Vector3.zero;
			if(!input.lockOn){
				targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
				// move the player
				controller.Move(targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
			}
			else{

				Vector2 input = new Vector2(inputDirection.x, inputDirection.z);
				input = Vector2.ClampMagnitude(input, 1);

				Vector3 camF = mainCamera.transform.forward;
				Vector3 camR = mainCamera.transform.right;

				camF.y = 0;
				camR.y = 0;
				camF = camF.normalized;
				camR = camR.normalized;
				transform.position += (camF*input.y + camR*input.x)*Time.deltaTime * targetSpeed;
				animator.SetFloat("StrafingX", inputDirection.x*-1);
				animator.SetFloat("StrafingY", inputDirection.z*-1);
			}
				

			

			// update animator if using character
			if (hasAnimator)
			{
				animator.SetFloat(animIDSpeed, animationBlend);
				animator.SetFloat(animIDMotionSpeed, inputMagnitude);
				if(input.lockOn && !isDashing && !isSprinting)
					animator.SetInteger("Strafing", 1);
				else
					animator.SetInteger("Strafing", 0);
			}
		}

		private void JumpAndGravity()
		{
			if (grounded)
			{
				isJumping = false;
				// reset the fall timeout timer
				fallTimeoutDelta = fallTimeout;

				// update animator if using character
				if (hasAnimator)
				{
					animator.SetBool(animIDJump, false);
					animator.SetBool(animIDFreeFall, false);
				}

				// stop our velocity dropping infinitely when grounded
				if (verticalVelocity < 0.0f)
				{
					verticalVelocity = -2f;
				}

				// Jump
				if (input.jump && jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

					// update animator if using character
					if (hasAnimator)
					{
						animator.SetBool(animIDJump, true);
					}
					isJumping = true;
				}

				// jump timeout
				if (jumpTimeoutDelta >= 0.0f)
				{
					jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				jumpTimeoutDelta = jumpTimeout;

				// fall timeout
				if (fallTimeoutDelta >= 0.0f)
				{
					fallTimeoutDelta -= Time.deltaTime;
				}
				else
				{
					// update animator if using character
					if (hasAnimator)
					{
						animator.SetBool(animIDFreeFall, true);
					}
				}

				// if we are not grounded, do not jump
				input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (verticalVelocity < terminalVelocity)
			{
				verticalVelocity += gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;
			
			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z), groundedRadius);
		}

		private void SetSpeeed()
		{
			if (input.move == Vector2.zero || combatManager.isAttacking)
			{
				targetSpeed = 0.0f;
				moving = false;
			}
			else
			{
				moving = true;
				if (input.sprintDisabled)
					targetSpeed = walkSpeed;
				else
				{
					if (input.dash && playerStats.stamina > 0 && targetSpeed != 0){
							Tick();
							if(timer>dashTime){
								targetSpeed=sprintSpeed;
								isDashing = false;
								isSprinting = false;
							}
							else{
								targetSpeed = dashSpeed;
								isDashing = true;
								isSprinting = false;
							}
					}
					else{
						if(input.lockOn && !isDashing && !isSprinting)
							targetSpeed = walkSpeed;
						else
							targetSpeed = moveSpeed;
						canDash = true;
						isDashing = false;
						isSprinting = false;
						timer=0;
					}
				}

			}
			if(input.dash && input.move != Vector2.zero && playerStats.stamina > 0){
					Tick();
					if(timer>dashTime){
						targetSpeed=sprintSpeed;
						isDashing = false;
						isSprinting = true;
						input.lockOn = false;
					}
					else{
						targetSpeed = dashSpeed;
						isDashing = true;
					}
			}
			else{
				canDash = true;
				isDashing = false;
				isSprinting = false;
				timer=0;
			}


		}

		public void Tick(){
			timer+= Time.deltaTime;
		}
	}
