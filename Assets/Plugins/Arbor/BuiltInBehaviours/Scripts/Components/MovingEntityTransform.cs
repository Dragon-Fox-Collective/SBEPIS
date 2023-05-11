//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Transformによって移動するMovingEntity
	/// </summary>
	/// <remarks>速度はUpdate間での移動量をTime.deltaTimeで微分して求められる</remarks>
#else
	/// <summary>
	/// MovingEntity moved by Transform
	/// </summary>
	/// <remarks>velocity is calculated by differentiating the amount of movement between Updates with Time.deltaTime.</remarks>
#endif
	[AddComponentMenu("Arbor/Navigation/MovingEntityTransform")]
	[BuiltInComponent]
	[Internal.DocumentManual("/manual/builtin/movingentity/movingentitytransform.md")]
	public sealed class MovingEntityTransform : MovingEntity
	{
		private Vector3 _Velocity;
		private Vector3 _PrevPosition;

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動速度
		/// </summary>
#else
		/// <summary>
		/// Movement velocity
		/// </summary>
#endif
		public override Vector3 velocity
		{
			get
			{
				return _Velocity;
			}
		}

		private void Start()
		{
			_PrevPosition = position;
		}

		private void Update()
		{
			float t = Time.deltaTime;
			if (Mathf.Approximately(t, 0f))
			{
				return;
			}

			Vector3 currentPosition = position;

			_Velocity = (currentPosition - _PrevPosition) / t;

			_PrevPosition = currentPosition;
		}
	}
}