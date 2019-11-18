//--------------------------------------------------------------------------
// @name UISliderHandleHelper.cs
//
// @author SoulFission
//
// @brief
//  en:  controls slider execution
//  chs: 滚动条脚本
//  ja:  スライダ用スクリプト
//--------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Voruko
{
    public class UISliderHandleHelper: MonoBehaviour
    {
		public MovementController _TargetCtrl	= null;
		public string _Var = null;

		private System.Reflection.FieldInfo fieldInfo = null;
		private UnityEngine.UI.Text valueText = null;
		private UnityEngine.UI.Slider valueSlider = null;

		private void Start()
		{
			if(_TargetCtrl == null )
			{
				var ctrls = GameObject.FindObjectsOfType<MovementController>();
				for( int i = 0;i < ctrls.Length;i++ )
				{
					var ctrl = ((MovementController)ctrls[i]);
					if( ctrl._IsPlayer )
					{
						_TargetCtrl = ctrl;
						break;
					}
				}

			}
			valueText	= GetComponentInChildren<UnityEngine.UI.Text>();
			valueSlider = GetComponent<UnityEngine.UI.Slider>();
			fieldInfo = typeof(MovementController).GetField(_Var);

			syncSlider();
		}

		private void OnEnable()
		{
			syncSlider();
		}

		private void Update()
		{
		}

		public void syncSlider() 
		{
			if( valueSlider == null || fieldInfo == null)
				return;
			valueSlider.value = (float)fieldInfo.GetValue(_TargetCtrl);

			if( valueText != null )
				valueText.text = valueSlider.value.ToString();
		}
		public void syncField() {
			if( _TargetCtrl == null || valueSlider == null)
				return;
			float value = valueSlider.value;
			fieldInfo.SetValue(_TargetCtrl,value);
			if(valueText != null)
				valueText.text = value.ToString();
		}
	}
}