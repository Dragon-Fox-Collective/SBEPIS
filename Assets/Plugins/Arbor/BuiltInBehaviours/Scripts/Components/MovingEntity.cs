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
	/// 移動する存在の基本クラス。
	/// </summary>
#else
	/// <summary>
	/// The base class of a moving entity.
	/// </summary>
#endif
	[BuiltInComponent]
	public abstract class MovingEntity : MonoBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 移動速度
		/// </summary>
#else
		/// <summary>
		/// Movement velocity
		/// </summary>
#endif
		public abstract Vector3 velocity
		{
			get;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 自身のTransform
		/// </summary>
#else
		/// <summary>
		/// Own Transform
		/// </summary>
#endif
		public virtual Transform selfTransform
		{
			get
			{
				return _SelfTransform;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 前方ベクトル
		/// </summary>
#else
		/// <summary>
		/// Forward vector
		/// </summary>
#endif
		public virtual Vector3 forward
		{
			get
			{
				return selfTransform.forward;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 位置
		/// </summary>
#else
		/// <summary>
		/// position
		/// </summary>
#endif
		public virtual Vector3 position
		{
			get
			{
				return selfTransform.position;
			}
		}

		private Transform _SelfTransform;

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数はスクリプトのインスタンスがロードされたときに呼び出される。
		/// </summary>
#else
		/// <summary>
		/// This function is called when the script instance is being loaded.
		/// </summary>
#endif
		protected virtual void Awake()
		{
			_SelfTransform = GetComponent<Transform>();
		}
	}
}