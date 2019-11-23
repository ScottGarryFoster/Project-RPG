//--------------------------------------------------------------------------
// @name MovementController.cs
//
// @author SoulFission
//
// @brief
//  en:  Voruko locomotion script
//  chs: Voruko的locomotion脚本
//  ja:  ボルコのlocomotionスクリプト
//--------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voruko
{
	[DefaultExecutionOrder(Definations.ExeOrder_moveCtrl)]
	public class MovementController : MonoBehaviour
	{
        #region fields
        [Header("Links")]
        [Tooltip("Drag the Input Control on here for Pause")]
        public InputControl _InputContol;

        [Header("Other")]
		[Tooltip("whether keyboard/controller controls this gameobject.")]
		public bool _IsPlayer = false;

		[Tooltip("whether smooth move input.")]
		public bool _SmoothControl = true;

		[Tooltip("smooth factor when idle/walk/run.")]
		[Range(0f,1f)]
		public float _MoveSmoothFactor = 0.1f;

		[Tooltip("smooth factor when rotate.")]
		[Range(0f,1f)]
		public float _RotateSmoothFactor = 0.1f;



		[Tooltip("move speed.")]
		public float _MoveSpeed = 10;
		[Tooltip("gravity.")]
		public float _Gravity = 50f;
		[Tooltip("controller deadzone.")]
		[Range(0f,1f)]
		public float _DeadZone = 0.1f;

		[Tooltip("addictive gravity when character is ground,to avoid transition to air animation when down slope.")]
		public float _FalloffBias = 10f;
		[Tooltip("speed of jump.")]
		public float _JumpSpeed = 20f;


		public enum AirMode
		{
			Add,
			Lerp,
		};
		[Tooltip("the way of smooth movement when in the air.")]
		public AirMode _AirMode = AirMode.Lerp;

		[Tooltip("how much move input works when in the air.")]
		[Range(0f,1f)]
		public float _AirMoveBlend = 0.5f;


		[Tooltip("knockback speed when damaged.")]
		public float _KnockBackSpeed = 40f;
		[Tooltip("knockback speed reduction.")]
		public float _KnockBackReduction = 15f;
		[Tooltip("knockback direction rotate factor.")]
		public float _KnockBackRotationBlend = 0.5f;

		[Tooltip("the speed percentage of dodge move when no move input.")]
		public float _MinRollDodgeSpeed = 0.5f;
		[Tooltip("the speed percentage of dodge move when max move input.")]
		public float _RollDodgeFactor = 1f;


		private float gravitySpeed      = 0f;
		private Vector3 moveVecPrev     = Vector3.zero;
		private float moveLengthPrev    = 0f;
		private CharacterController charaCtrl = null;
		private Animator animator       = null;
		public Animator Anim
		{
			get { return animator; }
		}
		private DamageController damageCtrl = null;
		public DamageController DamageCtrl
		{
			get { return damageCtrl; }
		}

		private WeaponController weapon = null;

		private Vector3 jumpStartVec			= Vector3.zero;
		private float	jumpStartLength			= 0f;
		private Vector3 jumpVec					= Vector3.zero;
		private Vector3 rotationLockVec			= Vector3.zero;
		private float	rotationLock			= 0f;
		private Vector3 knockBackVec			= Vector3.zero;
		private float	knockBackSpeedCounter	= 0f;

		private enum State
		{
			Null,
			Ground,
			Jump,
			Air,
			Attack,
			Attacking,
			AirAttack,
			RollDodge,
			RollDodging,
			Damage,
			Dead,
		};
		private State StateCurrent = State.Ground;
		private State StateNext = State.Null;

		public enum Action {
			Null		=	0x00000000,
			Jump		=	0x00000001,
			Attack		=	0x00000002,
			RollDodge	=	0x00000004,
		};
		private Action action;

		private Vector3 actionInputVec;
		private Action actionInput;

		public static int baseMoveLayer = 0;

#endregion
#region action variable
		Vector3 inputVec = Vector3.zero;
		float inputLength = 0f;

		// variable for move
		Vector3 moveVec = Vector3.zero;
		float moveLength = 0f;
		Vector3 gravityBias = Vector3.zero;// push down when walk downhill
		float moveBlend = 1f;

		bool startDamage = false;
		#endregion
#region public methods

		// for other scripts
		public void setAction(Vector3 move,Action a) {
			actionInputVec	= move;
			actionInput		= a;
		}
		public bool isIdle() 
		{
			return StateCurrent == State.Ground && moveLength == 0f;
		}

		public bool isMoving()
		{
			return StateCurrent == State.Ground && moveLength > 0f;
		}
		public bool isAir()
		{
			return (StateCurrent == State.Jump || StateCurrent == State.Air || StateCurrent == State.AirAttack);
		}
		public bool isAttack()
		{
			return (StateCurrent == State.Attack || StateCurrent == State.Attacking || StateCurrent == State.AirAttack);
		}
		public bool isRollDodging()
		{
			return StateCurrent == State.RollDodging || StateCurrent == State.RollDodge;
		}
		public bool isDamaging()
		{
			return StateCurrent == State.Damage;
		}
		public bool isDead()
		{
			return StateCurrent == State.Dead;
		}

#endregion
#region cycle Methods
		// Use this for initialization
		private void Start()
		{
			charaCtrl	= GetComponent<CharacterController>();
			animator	= GetComponent<Animator>();
			damageCtrl	= GetComponent<DamageController>();

			moveVecPrev     = transform.rotation * new Vector3(0f, 0f, 1f);
			moveLengthPrev  = 0f;
			weapon = GetComponentInChildren<WeaponController>();
		}

		// Update is called once per frame
		private void Update()
		{
			// based on character controller
			if (charaCtrl == null)
				return;

			setupFields();
			updateInput();

			updateSmooth();
			updateRoutine();
			updateAnimatorParam();

			backupFields();                
		}

		#endregion
		#region updaters
		private void setupFields()
		{
			// setup every frame
			moveVec = Vector3.zero;
			moveLength = 0f;
			gravityBias = Vector3.zero;// push down when walk downhill
			jumpVec = Vector3.zero;
			rotationLock = 0f;

			action = Action.Null;

			// setup animator curve varible
			moveBlend = 1f;
			if (animator != null)
			{
				moveBlend = animator.GetFloat("moveBlend");
				rotationLock = animator.GetFloat("rotationLock");
			}

		}

		private void updateInput()
		{
			if(_IsPlayer)
			{
				inputVec.x = Input.GetAxisRaw("Horizontal");
				inputVec.z = Input.GetAxisRaw("Vertical");
				inputLength = Vector3.Magnitude(inputVec);

				action |= Input.GetButtonDown("Jump") ? Action.Jump : Action.Null;
				action |= Input.GetButtonDown("Fire1") ? Action.Attack : Action.Null;
				action |= Input.GetButtonDown("Fire2") ? Action.RollDodge : Action.Null;

				// input deadzone
				if( inputLength < _DeadZone )
				{
					inputLength = 0f;
				}
			}
			else 
			{
				inputVec = actionInputVec;
				inputLength = Vector3.Magnitude(inputVec);
				action = actionInput;

				actionInputVec = Vector3.zero;
				actionInput = Action.Null;
			}

			// calculate moveVec
			{
				if(inputLength == 0f)
				{
					moveVec = moveVecPrev;
				}
				else
				{
					if( _IsPlayer )
					{
						//moveVec = Camera.main.transform.TransformDirection(inputVec);
                        if(_InputContol != null)
                            if(_InputContol.CurrentCamera != null)
                                moveVec = _InputContol.CurrentCamera.transform.TransformDirection(inputVec);
                            else
                                moveVec = Camera.main.transform.TransformDirection(inputVec);
                        else
                            moveVec = Camera.main.transform.TransformDirection(inputVec);

                    }
                    else 
					{
						moveVec = inputVec;
					}

					moveVec = moveVec.normalized;
				}

				moveLength = inputLength;

			}
			
		}

		private void updateSmooth()
		{
			if (_SmoothControl)
			{
				var currentDir = transform.rotation * new Vector3(0f, 0f, 1f);
				// Smooth

				// secceeded
				float adjustFactorRange = 12f;
				float rotateBlend = Mathf.Clamp((1f - _RotateSmoothFactor) * Time.deltaTime * adjustFactorRange, 0f, 1f);
				float speedBlend = Mathf.Clamp((1f - _MoveSmoothFactor) * Time.deltaTime * adjustFactorRange, 0f, 1f);

				if (_RotateSmoothFactor < Mathf.Epsilon)
					rotateBlend = 1f;
				if (_RotateSmoothFactor < Mathf.Epsilon)
					speedBlend = 1f;

				moveVec = Vector3.Slerp(currentDir, moveVec, rotateBlend);
				moveLength = Mathf.Lerp(moveLengthPrev, moveLength, speedBlend);

				moveVec.y = 0f;
				moveVec = moveVec.normalized;
			}

		}

		private void updateRoutine()
		{
			bool result = false;
			while (!result)
			{
				if(StateNext != State.Null)
				{
					StateCurrent = StateNext;
					StateNext = State.Null;
				}
				switch (StateCurrent)
				{
				case State.Null         : result = updateNull();        break;
				case State.Ground       : result = updateGround();      break;
				case State.Jump         : result = updateJump();        break;
				case State.Air          : result = updateAir();         break;
				case State.Attack       : result = updateAttack();      break;
				case State.Attacking    : result = updateAttacking();   break;
				case State.AirAttack    : result = updateAirAttack();   break;
				case State.RollDodge    : result = updateRollDodge();   break;
				case State.RollDodging: result = updateRollDodging(); break;
				case State.Damage       : result = updateDamage();      break;
				case State.Dead			: result = updateDead();		break;
				}
			}

		}

#region routines
		private bool updateDamage()
		{
			if (!startDamage)
			{
				if (!animator.GetCurrentAnimatorStateInfo(baseMoveLayer).IsTag("damage") &&
					!animator.GetNextAnimatorStateInfo(baseMoveLayer).IsTag("damage"))
				{
					jumpState(State.Ground);
					return false;
				}
			}
			else
			{
				startDamage = false;
			}

			if(animator.GetCurrentAnimatorStateInfo(baseMoveLayer).IsTag("dead"))
			{
				jumpState(State.Dead);

			}
			gravityBias.y -= _FalloffBias * Time.deltaTime;

			transform.rotation = Quaternion.LookRotation(Vector3.Slerp(transform.forward, -knockBackVec, _KnockBackRotationBlend));
			charaCtrl.Move(knockBackVec * knockBackSpeedCounter * Time.deltaTime + gravityBias);

			knockBackSpeedCounter -= knockBackSpeedCounter * _KnockBackReduction * Time.deltaTime;

			return true;
		}

		private bool updateDead() {
			gravityBias.y -= _FalloffBias * Time.deltaTime;

			transform.rotation = Quaternion.LookRotation(Vector3.Slerp(transform.forward,-knockBackVec,_KnockBackRotationBlend));
			charaCtrl.Move(knockBackVec * knockBackSpeedCounter * Time.deltaTime + gravityBias);

			knockBackSpeedCounter -= knockBackSpeedCounter * _KnockBackReduction * Time.deltaTime;

			return true;
		}

		private bool updateNull()
		{
			rotateController();
			return true;
		}
		private bool updateGround()
		{
			jumpStartVec = moveVec;
			jumpStartLength = moveLength;

			if (!charaCtrl.isGrounded)
			{
				jumpState(State.Air);
				return false;
			}
			gravitySpeed = 0f;

			if ((action & Action.Jump) != Action.Null && isJumpable())
			{
				jumpState(State.Jump);
				return false;
			}
			else if ((action & Action.Attack) != Action.Null)
			{
				// Attack
				jumpState(State.Attack);
				return false;
			}
			else if ((action & Action.RollDodge) != Action.Null)
			{
				jumpState(State.RollDodge);
				return false;
			}

			gravityBias.y -= _FalloffBias * Time.deltaTime;

			rotateController();
			moveControllerGround();

			return true;
		}

		private bool updateRollDodge() {

			gravityBias.y -= _FalloffBias * Time.deltaTime;


			rotateController();
			moveControllerGround();

			jumpState(State.RollDodging);
			return true;
		}
		private bool updateRollDodging()
		{
			if(!animator.GetCurrentAnimatorStateInfo(baseMoveLayer).IsTag("dodge") &&
			!animator.GetNextAnimatorStateInfo(baseMoveLayer).IsTag("dodge"))
			{
				jumpState(State.Ground);
				return true;
			}

			gravityBias.y -= _FalloffBias * Time.deltaTime;
			moveLength = Mathf.Clamp(moveLength,_MinRollDodgeSpeed,1f);
			moveBlend *= _RollDodgeFactor;

			rotateController();
			moveControllerGround();

			return true;
		}
		private bool updateAttack()
		{
    
			gravityBias.y -= _FalloffBias * Time.deltaTime;

			jumpState(State.Attacking);

			rotateController();
			moveControllerGround();

			return true;
		}
		private bool updateAttacking()
		{
			if ((action & Action.Attack) != Action.Null)
			{
				// Attack
				jumpState(State.Attack);
				return false;
			}

			if (!animator.GetNextAnimatorStateInfo(baseMoveLayer).IsTag("attack"))
			{
				jumpState(State.Ground);
				return true;
			}

			gravityBias.y -= _FalloffBias * Time.deltaTime;

			rotateController();
			moveControllerGround();

			return true;
		}

	private bool updateJump()
		{
			// Jump
			gravityBias.y += _JumpSpeed * Time.deltaTime;
			gravitySpeed += _JumpSpeed;
			jumpStartVec = moveVecPrev;
			jumpStartLength = moveLengthPrev;

			jumpState(State.Air);

			rotateController();
			moveControllerAir();

			return true;
		}

		private bool updateAir()
		{
			if (charaCtrl.isGrounded)
			{
				jumpState(State.Ground);
				return false;
			}

			airMove();

			if ((action & Action.Attack) != Action.Null)
			{
				jumpState(State.AirAttack);
				return false;
			}

			float y = transform.position.y;
			

			rotateController();
			moveControllerAir();

			if( gravitySpeed > 0f )
			{
				if( transform.position.y - y < Mathf.Epsilon )
				{
					gravitySpeed = 0f;
				}
			}

			return true;
		}

		private bool updateAirAttack()
		{
			if (charaCtrl.isGrounded)
			{
				jumpState(State.Ground);
				return false;
			}
			airMove();

			rotateController();
			moveControllerAir();

			return true;
		}
#endregion
#region routine utils
		private void jumpState(State s) 
		{
			StateNext = s;
		}
		private bool isJumpable()
		{
			return animator.GetCurrentAnimatorStateInfo(baseMoveLayer).IsTag("move") ||
				 animator.GetNextAnimatorStateInfo(baseMoveLayer).IsTag("move"); ;
		}

		private bool isRollDodgable()
		{
			return !animator.GetCurrentAnimatorStateInfo(baseMoveLayer).IsTag("air") &&
				!animator.GetCurrentAnimatorStateInfo(baseMoveLayer).IsTag("attack") &&
				!animator.GetCurrentAnimatorStateInfo(baseMoveLayer).IsTag("airAttack");
		}

		private void airMove()
		{
			// in the air
			if (_AirMode == AirMode.Lerp)
			{
				moveVec = Vector3.Slerp(moveVecPrev, moveVec, _AirMoveBlend);
				moveLength = Mathf.Lerp(jumpStartLength, moveLength, _AirMoveBlend);
			}
			else if (_AirMode == AirMode.Add)
			{
				jumpVec = jumpStartVec * jumpStartLength * _MoveSpeed;
				moveLength *= _AirMoveBlend;
			}

			gravitySpeed -= _Gravity * Time.deltaTime;
			gravityBias.y += gravitySpeed * Time.deltaTime;
			
		}

		private void rotateController()
		{
			if (moveLength > Mathf.Epsilon)
			{
				if (rotationLock != 0f)
					transform.rotation = Quaternion.LookRotation(rotationLockVec);
				else
					transform.rotation = Quaternion.LookRotation(moveVec);

			}
		}

		private void moveControllerGround()
		{
			charaCtrl.Move(moveVec * moveBlend * moveLength * _MoveSpeed * Time.deltaTime + gravityBias);

		}
		private void moveControllerAir()
		{
			charaCtrl.Move(moveVec * moveBlend * moveLength * _MoveSpeed * Time.deltaTime + gravityBias + jumpVec * Time.deltaTime);

		}
#endregion
		private void updateAnimatorParam()
		{
			// mechanim control
			if (animator != null)
			{
				float speedParam = moveLength; //(moveLength - _DeadZone) / (1f - _DeadZone);
				animator.SetFloat("speed", speedParam);

				// jump
				animator.SetBool("isJump", StateCurrent == State.Jump);
				// fall ground
				animator.SetBool("isGround", charaCtrl.isGrounded);
				//    StateCurrent == State.Ground && 
				//    (StatePrev == State.Air || StatePrev == State.AirAttack));

				// in the air
				animator.SetBool("isAir",
					StateCurrent == State.Air);
				// attack
				animator.SetBool("isAttack",
					StateCurrent == State.Attack || StateCurrent == State.AirAttack);
				//dodge
				animator.SetBool("isDodge",
					StateCurrent == State.RollDodge);

			}
		}

		private void backupFields()
		{
			moveVecPrev     = moveVec;
			moveLengthPrev  = moveLength;
			if (rotationLock == 0f)
			{
				rotationLockVec = moveVec;
			}
		}

#endregion
#region colliders     
		private void OnTriggerEnter(Collider col)
		{
			if (col.tag == "Weapon") {
				var ctrl = col.GetComponent<WeaponController>();
				if (ctrl == null)
					return;

				if( ctrl.ParentController == this )
					return;
				if( ctrl.ParentController.tag == tag )
					return;

				float damage = ctrl.getDamage();
				if (damage <= 0f)
					return;

				// calculate knockBack from character transform
				if(StateCurrent != State.Damage && !startDamage)
				{
					// setup knockback
					jumpState(State.Damage);
					{
						knockBackVec = transform.position - ctrl.getParentPosition();
						knockBackVec.y = 0f;
						knockBackVec.Normalize();

						knockBackSpeedCounter = _KnockBackSpeed;
					}

					// animation crossfade
					string damageAnimation = damageCtrl.doDamage((int)damage);
					animator.CrossFade(damageAnimation, 0.1f, baseMoveLayer, 0.15f);

					startDamage = true;
				}
			}
		}

		public void revive()
		{
			animator.CrossFade("Move",0f);
		}
		#endregion

		#region Public Methods

		public void setWeapon(int value)
		{
			weapon.gameObject.SetActive(value != 0);
		}

        public GameObject getWeapon()
        {
            return weapon.gameObject;
        }
		#endregion
	}
}