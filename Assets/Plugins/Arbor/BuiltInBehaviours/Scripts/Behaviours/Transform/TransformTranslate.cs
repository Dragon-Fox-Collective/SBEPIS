//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Transformを移動する。
	/// </summary>
#else
	/// <summary>
	/// Moves the transform.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transform/TransformTranslate")]
	[BuiltInBehaviour]
	public sealed class TransformTranslate : TransformBehaviourBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 位置の座標空間
		/// </summary>
#else
		/// <summary>
		/// Coordinate space of position
		/// </summary>
#endif
		[SerializeField]
		private FlexibleSpace _Space = new FlexibleSpace(Space.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 更新メソッドの種類
		/// </summary>
#else
		/// <summary>
		/// Type of update method
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(UpdateMethodType))]
		private FlexibleUpdateMethodType _UpdateMethodType = new FlexibleUpdateMethodType(UpdateMethodType.StateUpdate);

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動速度
		/// </summary>
#else
		/// <summary>
		/// Movement velocity
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _Velocity = new FlexibleVector3();

		[SerializeField]
		[HideInInspector]
		private int _TransformTransate_SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_Space")]
		private Space _OldSpace = Space.Self;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_UpdateMethodType")]
		private UpdateMethodType _OldUpdateMethodType = UpdateMethodType.StateUpdate;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		void UpdateTranslate(UpdateMethodType updateMethodType)
		{
			if (_UpdateMethodType.value != updateMethodType)
			{
				return;
			}

			Transform target = cachedTransform;
			if (target != null)
			{
				Vector3 translation = _Velocity.value * Time.deltaTime;
				target.Translate(translation, _Space.value);
			}
		}

		[Internal.ExcludeTest]
		private void Update()
		{
			using (CalculateScope.OpenScope())
			{
				UpdateTranslate(UpdateMethodType.Update);
			}
		}

		// OnStateUpdate is called once per frame
		public override void OnStateUpdate()
		{
			UpdateTranslate(UpdateMethodType.StateUpdate);
		}

		protected override void Reset()
		{
			base.Reset();

			_TransformTransate_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Space = (FlexibleSpace)_OldSpace;
			_UpdateMethodType = (FlexibleUpdateMethodType)_OldUpdateMethodType;
		}

		void Serialize()
		{
			while (_TransformTransate_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_TransformTransate_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_TransformTransate_SerializeVersion++;
						break;
					default:
						_TransformTransate_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}


		public override void OnAfterDeserialize()
		{
			base.OnAfterDeserialize();

			Serialize();
		}

		public override void OnBeforeSerialize()
		{
			base.OnBeforeSerialize();

			Serialize();
		}
	}
}