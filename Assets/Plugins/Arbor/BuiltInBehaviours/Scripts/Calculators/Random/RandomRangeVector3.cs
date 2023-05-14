//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 2値間のランダムなVector3値を出力する。
	/// </summary>
#else
	/// <summary>
	/// Outputs a random Vector3 value between two values.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Random/Random.RangeVector3")]
	[BehaviourTitle("Random.RangeVector3")]
	[BuiltInBehaviour]
	public sealed class RandomRangeVector3 : Calculator, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 1つ目のVector3
		/// </summary>
#else
		/// <summary>
		/// 1st Vector3
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _VectorA = new FlexibleVector3(Vector3.zero);

#if ARBOR_DOC_JA
		/// <summary>
		/// 2つ目のVector3
		/// </summary>
#else
		/// <summary>
		/// 2nd Vector3
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _VectorB = new FlexibleVector3(Vector3.zero);

#if ARBOR_DOC_JA
		/// <summary>
		/// 補間タイプ
		/// </summary>
#else
		/// <summary>
		/// Interpolate type
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(InterpolateType))]
		private FlexibleInterpolateType _InterpolateType = new FlexibleInterpolateType(InterpolateType.Lerp);

#if ARBOR_DOC_JA
		/// <summary>
		/// 出力スロット
		/// </summary>
#else
		/// <summary>
		/// Output slot
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotVector3 _Output = new OutputSlotVector3();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_InterpolateType")]
		private InterpolateType _OldInterpolateType = InterpolateType.Lerp;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			switch (_InterpolateType.value)
			{
				case InterpolateType.Lerp:
					_Output.SetValue(Vector3.Lerp(_VectorA.value, _VectorB.value, Random.value));
					break;
				case InterpolateType.Slerp:
					_Output.SetValue(Vector3.Slerp(_VectorA.value, _VectorB.value, Random.value));
					break;
			}
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_InterpolateType = (FlexibleInterpolateType)_OldInterpolateType;
		}

		void Serialize()
		{
			while (_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_SerializeVersion++;
						break;
					default:
						_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			Serialize();
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
			Serialize();
		}
	}
}