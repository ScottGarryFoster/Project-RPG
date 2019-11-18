//--------------------------------------------------------------------------
// @name DamageController.cs
//
// @author SoulFission
//
// @brief
//  en:  controls Damage
//  chs: 打击脚本
//  ja:  ダメージ管理用スクリプト
//--------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voruko
{
    public class DamageController : MonoBehaviour
    {
		public const string SmallDamageMotion = "Damage1";
		public const string BigDamageMotion = "Damage2";
		public const string DeadMotion		= "Dead";

		[Tooltip("Max hit point.")]
		public int MaxHP = 5;
		[Tooltip("Current hit point.initialize to max hit point when start.")]
		public int HP = 0;
		[Tooltip("The num of taking Big Damage.")]
		public int BigDamageCounter = 2;

		private MovementController moveCtrl = null;

		private void Start() 
		{
			moveCtrl = GetComponent<MovementController>();
			HP = MaxHP;
		}

		[ContextMenu("revive")]
		public void revive()
		{
			HP = MaxHP;
			moveCtrl.revive();
		}

		public string doDamage(int damage) 
		{
			HP -= damage;
			if( HP <= 0 )
				return DeadMotion;
			if( (MaxHP - HP) % BigDamageCounter == 0 )
				return BigDamageMotion;
			return SmallDamageMotion;
		}
	}
}