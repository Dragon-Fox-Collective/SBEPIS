//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor.BehaviourTree.Decorators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 距離のチェック
	/// </summary>
#else
	/// <summary>
	/// Checking the distance
	/// </summary>
#endif

	[AddComponentMenu("")]
	[AddBehaviourMenu("DistanceCheck")]
	[BuiltInBehaviour]
	public sealed class DistanceCheck : Decorator
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 比較元のTransform。
		/// </summary>
#else
		/// <summary>
		/// Transform to compare from.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleTransform _Transform = new FlexibleTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のTransform。
		/// </summary>
#else
		/// <summary>
		/// Transform to be compared.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleTransform _Target = new FlexibleTransform();

#if ARBOR_DOC_JA
		/// <summary>
		/// 距離。Targetとの距離がこの値よりも少なければtrueを返す。
		/// </summary>
#else
		/// <summary>
		/// Distance. Returns true if the distance to Target is less than this value.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Distance = new FlexibleFloat();

		protected override bool OnConditionCheck()
		{
			Transform target = _Target.value;
			if (target == null)
			{
				return false;
			}

			Transform transform = _Transform.value;
			if (transform == null)
			{
				return false;
			}

			float distanceValue = _Distance.value;
			float distanceSqr = distanceValue * distanceValue;

			float distanceSqrToTarget = (transform.position - target.position).sqrMagnitude;
			return distanceSqrToTarget < distanceSqr;
		}
	}
}