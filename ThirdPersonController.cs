using System.Collections;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class ThirdPersonController : MonoBehaviour
	{

		[Header("Player")]
		[Tooltip("Reset Bool isAttack")]
		public float TimeResetAttack = 0.5f;

		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 2.0f;

		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 5.335f;

		[Tooltip("How fast the character turns to face movement direction")]
		[Range(0.0f, 0.3f)]
		public float RotationSmoothTime = 0.12f;

		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;

		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.50f;

		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;

		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.28f;

		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 70.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -30.0f;
		[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
		public float CameraAngleOverride = 0.0f;
		[Tooltip("For locking the camera position on all axis")]
		public bool LockCameraPosition = false;

	

		// cinemachine
		private float _cinemachineTargetYaw;
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _animationBlend;
		private float _targetRotation = 0.0f;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		// animation IDs
		private int _animIDSpeed;
		private int _animIDGrounded;
		private int _animIDJump;
		private int _animIDFreeFall;
		private int _animIDMotionSpeed;
		private int _animIDClicGauche;
		private int _animIDClicDroit;
		private int _animIDAkey;private int _animIDEkey;private int _animIDRkey;private int _animIDFkey;
		
		public bool ClicGaucheActive;
		public bool ClicDroitActive;
		

		[SerializeField]private GameObject weaponSlot;
		[SerializeField]private GameObject _weaponSlot;
		Transform _weaponCurrently;
		Transform _weaponCurrentlyA;

		private Animator _animator;
		private CharacterController _controller;
		private StarterAssets.StarterAssetsInputs _input;
		private GameObject _mainCamera;
		private InventaireKevin Ik;
		private ApplyDamage _ad;
		private Player _player;
		private const float _threshold = 0.01f;

		private bool _hasAnimator;

		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			_player = GetComponent<Player>();
			_hasAnimator = TryGetComponent(out _animator);
			_animator.speed = 1.5f;
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssets.StarterAssetsInputs>();
			_ad = GetComponent<ApplyDamage>();
			AssignAnimationIDs();
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;
		}
		private void FixedUpdate()
		{
			
		}

		private void Update()
		{
			GroundedCheck();
			_weaponCurrently = weaponSlot.transform;
			_weaponCurrentlyA = _weaponSlot.transform;
			_hasAnimator = TryGetComponent(out _animator);
			JumpAndGravity();
			Move();
			ClicGauche();
			ClicDroit();
			Ekey();
			Rkey();
			Fkey();
			CameraRotation();
		}
		private void LateUpdate()
		{
			
		}
		private void AssignAnimationIDs()
		{
			_animIDSpeed = Animator.StringToHash("Speed");
			_animIDGrounded = Animator.StringToHash("Grounded");
			_animIDJump = Animator.StringToHash("Jump");
			_animIDFreeFall = Animator.StringToHash("FreeFall");
			_animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
			_animIDClicGauche = Animator.StringToHash("ClicGauche");
			_animIDClicDroit = Animator.StringToHash("ClicDroit");
			_animIDEkey = Animator.StringToHash("Ekey");
			_animIDRkey = Animator.StringToHash("Rkey");
			_animIDFkey = Animator.StringToHash("Fkey");
		}
		
		IEnumerator ResetAttackCooldown()
		{
			yield return new WaitForSeconds(TimeResetAttack);
			_player._isAttack = false;
			_player._isParade = false;
			_player._States = null;
		
		}
		private void IsAttackState(bool statue)
		{
			_player._isAttack = statue;
		}
		private void WeaponTag(string animaNet)
		{

				if(_weaponCurrently.tag == "WeaponMelee")
				{   
					_animator.SetBool("WeaponRange",false);
					_animator.SetBool("WeaponMelee",true);
				}
				else if (_weaponCurrently.tag == "WeaponRange")
				{	
					_animator.SetBool("WeaponMelee",false);
					_animator.SetBool("WeaponRange",true);
				}
				else
				{
					_animator.SetBool("WeaponRange",false);
					_animator.SetBool("WeaponMelee",false);
				}
				_ad.MyTriggerOn(animaNet);
				ResetInput();
		}
private void ResetInput()
{
	_input.Fire=false;_input.Aiming=false;_input.Ekey=false;_input.Rkey=false;_input.Fkey=false;
}
		private void ClicGauche()
		{
			if(_input.Fire)
			{
				if(_weaponCurrently.tag == "WeaponMelee")
				{
					_player._isParade = false;
					_player._isAttack = true;
					_player._States = "Gauche";
				}

				if(_weaponCurrently.tag == "WeaponRange")
				{
				_player._isAttack = true;
				_player._States ="Shoot";
				}
				WeaponTag("ClicGauche");
				StartCoroutine(ResetAttackCooldown());
			}

		}
		private void ClicDroit()
		{
			if(_input.Aiming)
			{	
				
				if(_weaponCurrently.tag == "WeaponMelee")
				{
				  _player._isParade = false;
				  _player._isAttack = true;
				  _player._States = "Droite";  
				}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
				if(_weaponCurrently.tag == "WeaponRange")
				{
					Debug.Log("TaffBordel");
				}

				WeaponTag("ClicDroit");
				StartCoroutine(ResetAttackCooldown());
				
			}
		}
		private void Ekey()
		{
			if(_input.Ekey)
			{
			//Systeme Loot
			}
			
		}
		private void Rkey()
		{
			if(_input.Rkey)
			{
				if(_weaponCurrently.tag=="WeaponMelee")
				{
					_player._isAttack = false;
					_player._isParade = true;
				}
				if(_weaponCurrently.tag=="WeaponRange")
				{

				}
				WeaponTag("Rkey");
				StartCoroutine(ResetAttackCooldown());
			}
		}
		private void Fkey()
		{
			if(_input.Fkey)
			{
				_input.Fkey=false;
				WeaponTag("Rkey");
			}
		}


		private void GroundedCheck()
		{
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
			if (_hasAnimator)
			{
				_animator.SetBool(_animIDGrounded, Grounded);
			}
		}

		private void CameraRotation()
		{
			if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
			{
				_cinemachineTargetYaw += _input.look.x * Time.deltaTime;
				_cinemachineTargetPitch += _input.look.y * Time.deltaTime;
			}
			_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
			CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
		}

		private void Move()
		{
			float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
			if (_input.move == Vector2.zero) targetSpeed = 0.0f;
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
			_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
			_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}
			_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
			if (_input.move != Vector2.zero)
			{
				_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
				float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
				transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
			}


			Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
			_controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
			if (_hasAnimator)
			{
				_animator.SetFloat(_animIDSpeed, _animationBlend);
				_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
			}
		}
		private void JumpAndGravity()
		{
			if (Grounded)
			{
				_fallTimeoutDelta = FallTimeout;
				if (_hasAnimator)
				{
					_animator.SetBool(_animIDJump, false);
					_animator.SetBool(_animIDFreeFall, false);
				}
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDJump, true);
					}
				}

				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				_jumpTimeoutDelta = JumpTimeout;
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}
				else
				{
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDFreeFall, true);
					}
				}
				_input.jump = false;
			}
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
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

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;
			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}

		


	}
