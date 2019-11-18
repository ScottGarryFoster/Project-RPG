//--------------------------------------------------------------------------
// @name WeaponController.cs
//
// @author SoulFission
//
// @brief
//  en:  controls weapon behavior
//  chs: 武器脚本
//  ja:  武器用スクリプト
//--------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Voruko
{
    [DefaultExecutionOrder(Definations.ExeOrder_weaponJointCopy)]
    public class WeaponController : MonoBehaviour
    {
		private MovementController _ParentController = null;
        public MovementController ParentController {
            get { return _ParentController; }
        }

		public enum AttachJoint {
			Null,
			RHand,
		};
		[Tooltip("select which joint to attach.")]
		public AttachJoint _AttachJoint = AttachJoint.RHand;
		public Hashtable map;

		// JointAttach
		[Tooltip("target of attach.")]
		public Transform _Target = null;
		[Tooltip("whether copy target position to this transform.")]
		public bool _CopyPosition = true;
		[Tooltip("whether copy target rotation to this transform.")]
		public bool _CopyRotation = true;

		[Tooltip("whether keeps default transform to offset.")]
		public bool _KeepOffset = true;

		private Vector3 ofsPosition = new Vector3();
		private Quaternion ofsRotation = new Quaternion();

		
		// constructora
		public WeaponController()
        {
			// map init
			map = new Hashtable();
			map.Add(AttachJoint.Null,"");
			map.Add(AttachJoint.RHand,"hand.R");

			
		}
        // Use this for initialization
        void Start()
        {
            _ParentController = GetComponentInParent<MovementController>();
			if( _ParentController == null )
				return;
			var transformList = ParentController.GetComponentsInChildren<Transform>();
			for (int i = 0;i < transformList.Length;i++ )
			{
				if( transformList[i].name.Equals(map[_AttachJoint]) )
				{
					_Target = transformList[i];
					break;
				}
			}

			if( _KeepOffset )
			{
				ofsPosition = transform.localPosition;
				ofsRotation = transform.localRotation;
			}
		}

        // Update is called once per frame
        void Update()
        { }

		private void LateUpdate()
		{
			var targetTransform = _Target;
			if( _Target )
			{
				if( _CopyPosition )
					transform.position = targetTransform.position + ofsPosition;
				if( _CopyRotation )
					transform.rotation = targetTransform.rotation * ofsRotation;
			}
		}
		public float getDamage()
        {
            if (ParentController == null)
                return 0f;
            var animator = ParentController.Anim;
            if (animator == null)
                return 0f;
            return animator.GetFloat("attack") > 0 ? 1f:0f;
        }

        public Vector3 getParentPosition()
        {
            return ParentController == null ? transform.position : ParentController.transform.position;
        }
        /*
        void OnTriggerStay(Collider col)
        {
            var animator = col.gameObject.GetComponent<Animator>();
            var movement = col.gameObject.GetComponent<MovementController>();
            if (animator != null && movement != null)
            {
                if (!animator.GetCurrentAnimatorStateInfo(MovementController.baseMoveLayer).IsTag("damage") &&
                    !animator.GetNextAnimatorStateInfo(MovementController.baseMoveLayer).IsTag("damage"))
                    animator.CrossFade(movement._DamageAnimation, 0.1f, MovementController.baseMoveLayer, 0.15f);
            }

        }
        */
    }
}