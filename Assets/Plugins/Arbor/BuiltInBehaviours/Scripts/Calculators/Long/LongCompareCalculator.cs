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
	/// longを比較する。
	/// </summary>
#else
	/// <summary>
	/// Compare long.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Long/Long.Compare")]
	[BehaviourTitle("Long.Compare")]
	[BuiltInBehaviour]
	public sealed class LongCompareCalculator : Calculator, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較タイプ。
		/// </summary>
#else
		/// <summary>
		/// Comparison type.
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(CompareType))]
		private FlexibleCompareType _CompareType = new FlexibleCompareType(CompareType.Equals);

#if ARBOR_DOC_JA
		/// <summary>
		/// 値1
		/// </summary>
#else
		/// <summary>
		/// Value 1
		/// </summary>
#endif
		[SerializeField] private FlexibleLong _Value1 = new FlexibleLong();

#if ARBOR_DOC_JA
		/// <summary>
		/// 値2
		/// </summary>
#else
		/// <summary>
		/// Value 2
		/// </summary>
#endif
		[SerializeField] private FlexibleLong _Value2 = new FlexibleLong();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotBool _Result = new OutputSlotBool();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_CompareType")]
		private CompareType _OldCompareType = CompareType.Equals;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		// Use this for calculate
		public override void OnCalculate()
		{
			bool result = CompareUtility.Compare(_Value1.value, _Value2.value, _CompareType.value);
			_Result.SetValue(result);
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_CompareType = (FlexibleCompareType)_OldCompareType;
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
