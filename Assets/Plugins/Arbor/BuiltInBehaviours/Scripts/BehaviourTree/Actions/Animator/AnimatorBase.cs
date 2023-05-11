//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// AnimatorにアクセスするためのActionBehaviour基本クラス
	/// </summary>
#else
	/// <summary>
	/// ActionBehaviour base class to access Animator
	/// </summary>
#endif
	[AddComponentMenu("")]
	[HideBehaviour()]
	public class AnimatorBase : ActionBehaviour
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象のAnimator
		/// </summary>
#else
		/// <summary>
		/// Animator of target
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Animator))]
		private FlexibleComponent _Animator = new FlexibleComponent(FlexibleHierarchyType.Self);

		#endregion // Serialize fields

		public Animator cachedAnimator
		{
			get
			{
				return _Animator.value as Animator;
			}
		}
	}
}