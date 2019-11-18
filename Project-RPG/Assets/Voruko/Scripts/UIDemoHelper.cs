//--------------------------------------------------------------------------
// @name UIDemoHelper.cs
//
// @author SoulFission
//
// @brief
//  en:  UI helper script
//  chs: UI工具脚本
//  ja:  UI補助スクリプト
//--------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Voruko
{
    public class UIDemoHelper : MonoBehaviour
    {
		public void flipActive()
		{
			gameObject.SetActive(!gameObject.activeSelf);
		}

		public void switchScene(string name)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(name,UnityEngine.SceneManagement.LoadSceneMode.Single);
		}
	}
}