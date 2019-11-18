//--------------------------------------------------------------------------
// @name AnimationCheckController.cs
//
// @author SoulFission
//
// @brief
//  en:  controls Animation check
//  chs: 动画检查脚本
//  ja:  アニメーションチェック用スクリプト
//--------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Voruko
{
    public class AnimationCheckController : MonoBehaviour
    {

		private Animator[] animator;
		private GameObject[] weapon;

		void Start()
		{
			animator = GameObject.FindObjectsOfType<Animator>();
			weapon = GameObject.FindGameObjectsWithTag("Weapon");
		}

		public void switchAnimation(string name) {
			if(animator != null) {
				for( int i = 0;i < animator.Length;i++ )
				{
					animator[i].CrossFade(name,0f,0,0f);
				}
			}
		}

		public void switchWeapon()
		{
			if(weapon != null )
			{
				for( int i = 0;i < weapon.Length;i++ )
				{
					weapon[i].SetActive(!weapon[i].activeSelf);
				}
			}
		}
    }
}