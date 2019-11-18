//--------------------------------------------------------------------------
// @name SimpleFollowCamera.cs
//
// @author SoulFission
//
// @brief
//  en:  controls Camera
//  chs: 跟随摄像机脚本
//  ja:  フォローカメラスクリプト
//--------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Voruko
{
	[DefaultExecutionOrder(Definations.ExeOrder_Camera)]
	public class SimpleFollowCamera:MonoBehaviour
	{
		[Tooltip("Camera target.")]
		public Transform _Target = null;
		[Tooltip("Camera distance to target.")]
		public float _Distance = 10f;
		[Tooltip("Camera target offset.")]
		public Vector3 _TowardsOfs = new Vector3(0f,2f,0f);
		[Tooltip("Rotation sensitivity.")]
		public float _RotateSensitivity = 100f;
		[Tooltip("Zoom sensitivity.")]
		public float _WheelSensitivity = 100f;

		private Vector3 mousePos;
		private Vector3 mousePosPrev;

		private Quaternion cameraDirection;

		private static Quaternion XRotate = Quaternion.AngleAxis(90f,new Vector3(1f,0f,0f));
		private static Quaternion YRotate = Quaternion.AngleAxis(90f,new Vector3(0f,1f,0f));

		public SimpleFollowCamera()
		{ }

		// Use this for initialization
		void Start()
		{

			if( _Target )
			{
				cameraDirection = Quaternion.LookRotation(transform.position - _Target.position,new Vector3(0f,1f,0f));
			}

			mousePos = Input.mousePosition;
			mousePosPrev = mousePos;
		}

		// Update is called once per frame
		void Update()
		{
			if( _Target )
			{
				Vector3 pos = _Target.position + _Distance * (cameraDirection * new Vector3(0f,0f,1f));
				transform.position = pos;
				transform.LookAt(_Target.position + _TowardsOfs);

				mousePos = Input.mousePosition;
				if( Input.GetMouseButton(2) )
				{
					var ofs = mousePos - mousePosPrev;
					var x = ofs.x * _RotateSensitivity * Time.deltaTime / (float)Screen.width;
					var y = ofs.y * _RotateSensitivity * Time.deltaTime / (float)Screen.height;

					var xrot = y > 0 ? XRotate : Quaternion.Inverse(XRotate);
					var yrot = x > 0 ? YRotate : Quaternion.Inverse(YRotate);
					cameraDirection = Quaternion.Slerp(cameraDirection,yrot * cameraDirection,Mathf.Abs(x));
					cameraDirection = Quaternion.Slerp(cameraDirection,cameraDirection * xrot,Mathf.Abs(y));


				}

				{
					float scroll = Input.GetAxis("Mouse ScrollWheel");
					_Distance -= scroll * _WheelSensitivity * Time.deltaTime;

				}
				// Controller Inputs
				try
				{
					{
						// Controller Right Scroll
						var x = Input.GetAxis("RScrollX") * Time.deltaTime;
						var y = -Input.GetAxis("RScrollY") * Time.deltaTime;

						var xrot = y > 0 ? XRotate : Quaternion.Inverse(XRotate);
						var yrot = x > 0 ? YRotate : Quaternion.Inverse(YRotate);
						cameraDirection = Quaternion.Slerp(cameraDirection,yrot * cameraDirection,Mathf.Abs(x));
						cameraDirection = Quaternion.Slerp(cameraDirection,cameraDirection * xrot,Mathf.Abs(y));


					}

					{
						// Controller L/R Trigger
						float scroll = Input.GetAxis("Trigger");
						_Distance += scroll * _WheelSensitivity * Time.deltaTime;

					}
				}
				catch
				{
					Debug.LogWarning(" input manager didn't setup RScrollX / RScrollY / Trigger.\n readme.txt for setup details.");
				}
				mousePosPrev = mousePos;


			}

		}

	}
}