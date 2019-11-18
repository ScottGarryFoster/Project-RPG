//--------------------------------------------------------------------------
// @name DemoBotController.cs
//
// @author SoulFission
//
// @brief
//  en:  Controls Demo Bots
//  chs: Demo机器人脚本
//  ja:  デモ用ボットスクリプト
//--------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Voruko
{
	[DefaultExecutionOrder(1)]
	public class DemoBotController: MonoBehaviour
    {
		public enum Style
		{
			Null,
			Idle,
			MoveOnly,
			MoveWithJumpAndDodge,
			All,
		};
		[Tooltip("Bot Style.")]
		public Style _Style = Style.Idle;

		[Tooltip("Corotine update time.")]
		public float _CorotineWaitTime = 0.5f;
		[Tooltip("Controls move speed factor upon distance to player.")]
		public AnimationCurve _MoveCurve = new AnimationCurve();
		[Tooltip("Bot jumps when vertical distance to player is longer than this param.")]
		public float _JumpVerticalDistance = 1f;
		[Tooltip("Bot jumps when Horizontal distance to player is shorter than this param.")]
		public float _JumpHorizontalDistance = 4f;

		[Tooltip("Bot attacks when distance to player is shorter than this param.")]
		public float _AttackDistance = 2f;
		[Tooltip("Attack cooldown time.")]
		public float _AttackCoolDownTime = 1f;
		[Tooltip("Bot dodges to opposite direction when vertical distance to player is shorter than this param and player is attacking.")]
		public float _DodgeDistance = 4f;

		private enum Option
		{
			Null = 0x00000000,
			Idle = 0x00000001,
			TracePlayer = 0x00000002,
			TracePlayerFast = 0x00000004,
			WithJump = 0x00000008,
			WithDodge = 0x00000010,
			WithAttack = 0x00000020,
		}
		private Option option = Option.Null;

		private MovementController.Action action;

		private MovementController mmCtrl;
		private Transform player = null;
		MovementController playerMoveCtrl = null;
		private Vector3 TargetPosition;

		private float attackCoolDownCounter;

		private void Start()
		{
			mmCtrl = GetComponent<MovementController>();
			StartCoroutine("coroutine");
			TargetPosition = transform.position;
		}

		private void Update()
		{
			switch( _Style )
			{
			case Style.Null: option = Option.Null; break;
			case Style.Idle: option = Option.Null; break;
			case Style.MoveOnly:option = Option.TracePlayer; break;
			case Style.MoveWithJumpAndDodge: option = Option.TracePlayer | Option.WithJump | Option.WithDodge;break;
			case Style.All:option = Option.TracePlayer | Option.WithJump | Option.WithDodge | Option.WithAttack;break;
			}

			if( player == null )
				findPlayerTransform();
			if(player != null && playerMoveCtrl == null)
				playerMoveCtrl = player.GetComponent<MovementController>();
			updateMove();
		}

		 private IEnumerator coroutine()
		 {
			while( true )
			{
				action = MovementController.Action.Null;
				if( player == null )
					findPlayerTransform();
				if( player == null )
					break;

				var horizontalDist = TargetPosition - transform.position;
				horizontalDist.y = 0f;
				float horizontalDistLength = horizontalDist.magnitude;

				// walk
				if( (option & Option.TracePlayer) != Option.Null )
				{
					if( !mmCtrl.isRollDodging() )
						TargetPosition = player.position;
				}



				//jump
				if( (option & Option.WithJump) != Option.Null )
				{
					if( TargetPosition.y - transform.position.y > _JumpVerticalDistance &&
						horizontalDistLength < _JumpHorizontalDistance )
						action |= MovementController.Action.Jump;
				}

				//dodge
				if( (option & Option.WithDodge) != Option.Null )
				{
					if( playerMoveCtrl != null )
					{
						if( playerMoveCtrl.isAttack() && horizontalDistLength < _DodgeDistance )
						{
							TargetPosition = transform.position + (transform.position - player.position) * 10f;
							action = MovementController.Action.RollDodge;
						}
					}
				}

				//attack
				if( (option & Option.WithAttack) != Option.Null )
				{
					if( horizontalDistLength < _AttackDistance )
					{
						bool result = false;

						if( !mmCtrl.isAttack() )
						{
							if( attackCoolDownCounter <= 0f )
							{
								attackCoolDownCounter = _AttackCoolDownTime;
								result = true;
							}
						}
						else
						{
							result = true;
						}

						if( result )
						{
							if( (action & MovementController.Action.RollDodge) != MovementController.Action.Null )
							{
								var weapon = playerMoveCtrl.GetComponent<WeaponController>();
								if( weapon != null )
								{
									if( weapon.getDamage() == 0f )
										action |= MovementController.Action.Attack;
								}
								else
								{
									action |= MovementController.Action.Attack;
								}
							}
							else
							{
								action |= MovementController.Action.Attack;
							}
						}
					}
					attackCoolDownCounter -= _CorotineWaitTime;
				}
				

				yield return new WaitForSeconds(_CorotineWaitTime);
			}
			yield return new WaitForSeconds(_CorotineWaitTime); 
		}

		private void findPlayerTransform()
		{
			var ctrls = GameObject.FindObjectsOfType<MovementController>();
			for(int i = 0; i < ctrls.Length; i++ ) 
			{
				var ctrl = ((MovementController)ctrls[i]);
				if(ctrl._IsPlayer) 
				{
					player = ctrl.transform;
					break;
				}
			}
		}

		private void updateMove() 
		{
			if(mmCtrl != null && player != null && playerMoveCtrl != null) 
			{
				if( option == Option.Null || (option & Option.Idle) != Option.Null )
					return;
				Vector3 distance = TargetPosition - transform.position;
				float speed = 1f;
				
				if((option & Option.TracePlayerFast) == Option.Null && 
				!playerMoveCtrl.isAttack())
				{
					speed = _MoveCurve.Evaluate(distance.magnitude);
				}
				Vector3 move = distance.normalized * speed;

				mmCtrl.setAction(move,action);
				action = MovementController.Action.Null;
			}
		}

	}
}