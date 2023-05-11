//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ステートがアクティブになった際に、指定したTransformの親を設定する。
	/// </summary>
	/// <remarks>
	/// 詳しくは、<see cref="Transform.SetParent(Transform)"/>を参照してください。
	/// </remarks>
#else
	/// <summary>
	/// Sets the parent of the specified Transform when the state becomes active.
	/// </summary>
	/// <remarks>
	/// For more information, see <see cref="Transform.SetParent(Transform)"/>.
	/// </remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transform/TransformSetParent")]
	[BuiltInBehaviour]
	public sealed class TransformSetParent : StateBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 親を設定するTrasnform
		/// </summary>
#else
		/// <summary>
		/// Transform to set the parent
		/// </summary>
#endif
		[SerializeField]
		private FlexibleTransform _Transform = new FlexibleTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 設定する親Transform
		/// </summary>
#else
		/// <summary>
		/// Parent Transform to set
		/// </summary>
#endif
		[SerializeField]
		private FlexibleTransform _Parent = new FlexibleTransform();

#if ARBOR_DOC_JA
		/// <summary>
		/// 親変更前のワールド空間における姿勢を維持するフラグ。
		/// </summary>
#else
		/// <summary>
		/// A flag that maintains the posture in the world space before the parent change.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _WorldPositionStays = new FlexibleBool(true);

		// Use this for enter state
		public override void OnStateBegin()
		{
			Transform transform = _Transform.value;
			if (transform == null)
			{
				return;
			}

			transform.SetParent(_Parent.value, _WorldPositionStays.value);
		}
	}
}