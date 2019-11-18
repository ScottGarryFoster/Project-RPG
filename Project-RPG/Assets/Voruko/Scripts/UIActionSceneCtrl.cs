//--------------------------------------------------------------------------
// @name UIActionSceneCtrl.cs
//
// @author SoulFission
//
// @brief
//  en:  Action check scene UI script
//  chs: 动作检查场景的UI脚本
//  ja:  アクションチェックシーン用のUIスクリプト
//--------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Voruko
{
    public class UIActionSceneCtrl: MonoBehaviour
    {
		private DemoBotController[] bots = null;
		private MovementController player = null;

		public void revivePlayer()
		{
			player.DamageCtrl.revive();
		}

		public void reviveBots()
		{
			for( int i = 0;i < bots.Length;i++ )
			{
				var mc = bots[i].GetComponent<DamageController>();
				if( mc != null )
					mc.revive();
			}
		}

		public void setBotStyleIdle()
		{
			for(int i = 0;i < bots.Length;i++ )
			{
				bots[i]._Style = DemoBotController.Style.Idle;
			}
		}

		public void setBotStyleMoveOnly()
		{
			for( int i = 0;i < bots.Length;i++ )
			{
				bots[i]._Style = DemoBotController.Style.MoveOnly;
			}
		}
		public void setBotStyleMoveWithJumpAndDodge()
		{
			for( int i = 0;i < bots.Length;i++ )
			{
				bots[i]._Style = DemoBotController.Style.MoveWithJumpAndDodge;
			}
		}

		public void setBotStyleAll()
		{
			for( int i = 0;i < bots.Length;i++ )
			{
				bots[i]._Style = DemoBotController.Style.All;
			}
		}
		private void Start()
		{
			bots = GameObject.FindObjectsOfType<DemoBotController>();
			findPlayerTransform();
		}

		private void findPlayerTransform()
		{
			var ctrls = GameObject.FindObjectsOfType<MovementController>();
			for( int i = 0;i < ctrls.Length;i++ )
			{
				var ctrl = ((MovementController)ctrls[i]);
				if( ctrl._IsPlayer )
				{
					player = ctrl;
					break;
				}
			}
		}
	}
}