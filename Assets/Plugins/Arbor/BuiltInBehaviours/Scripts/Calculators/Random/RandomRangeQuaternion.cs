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
	/// 2値間のランダムなQuaternion値を出力する。
	/// </summary>
#else
	/// <summary>
	/// Outputs a random Quaternion value between two values.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Random/Random.RangeQuaternion")]
	[BehaviourTitle("Random.RangeQuaternion")]
	[BuiltInBehaviour]
	public sealed class RandomRangeQuaternion : Calculator, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 1つ目のQuaternion
		/// </summary>
#else
		/// <summary>
		/// 1st Quaternion
		/// </summary>
#endif
		[SerializeField]
		private FlexibleQuaternion _QuatenionA = new FlexibleQuaternion(Quaternion.identity);

#if ARBOR_DOC_JA
		/// <summary>
		/// 2つ目のQuaternion
		/// </summary>
#else
		/// <summary>
		/// 2nd Quaternion
		/// </summary>
#endif
		[SerializeField]
		private FlexibleQuaternion _QuaternionB = new FlexibleQuaternion(Quaternion.identity);

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
		private FlexibleInterpolateType _InterpolateType = new FlexibleInterpolateType(InterpolateType.Slerp);

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
		private OutputSlotQuaternion _Output = new OutputSlotQuaternion();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_InterpolateType")]
		private InterpolateType _OldInterpolateType = InterpolateType.Slerp;

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
					_Output.SetValue(Quaternion.Lerp(_QuatenionA.value, _QuaternionB.value, Random.value));
					break;
				case InterpolateType.Slerp:
					_Output.SetValue(Quaternion.Slerp(_QuatenionA.value, _QuaternionB.value, Random.value));
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