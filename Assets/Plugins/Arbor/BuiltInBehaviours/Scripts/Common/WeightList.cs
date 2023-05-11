//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

namespace Arbor
{
	using Internal;

	public abstract class WeightListBase
	{
		public abstract int count
		{
			get;
		}

		public abstract void Add(object value, float weight);
		public abstract void Insert(int index, object value, float weight);

		public abstract object GetValueObject(int index);
		public abstract void SetValueObject(int index, object value);

		public abstract float GetWeight(int index);
		public abstract void SetWeight(int index, float weight);

		public abstract float GetTotalWeight();
	}

#if ARBOR_DOC_JA
	/// <param name="Value">値</param>
	/// <param name="Weight">重み。 大きいほど確率が高くなる。</param>
	/// <param name="Probability">実際の確率</param>
#else
	/// <param name="Value">value</param>
	/// <param name="Weight">Weight. The larger the probability increases.</param>
	/// <param name="Probability">Actual probability</param>
#endif
	[System.Serializable]
	[Documentable]
	public class WeightList<T> : WeightListBase, ISerializationCallbackReceiver, ISerializeVersionCallbackReceiver
	{
		#region Serialize fields

		[SerializeField]
		[HideInDocument]
		private List<T> _Values = new List<T>();

		[SerializeField]
		[ConstantRange(0, 100)]
		[HideInDocument]
		private List<FlexibleFloat> _Weights = new List<FlexibleFloat>();

		[SerializeField]
		[HideInInspector]
		private SerializeVersion _SerializeVersion = new SerializeVersion();

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_Weights")]
		private List<float> _OldWeights = new List<float>();

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

#if ARBOR_DOC_JA
		/// <summary>
		/// WeightListコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// WeightList constructor
		/// </summary>
#endif
		public WeightList()
		{
			// Initialize when calling from script.
			_SerializeVersion.Initialize(this);
		}

		public override int count
		{
			get
			{
				return _Values.Count;
			}
		}

		public override void Add(object value, float weight)
		{
			_Values.Add((T)value);
			_Weights.Add(new FlexibleFloat(weight));
		}

		public override void Insert(int index, object value, float weight)
		{
			_Values.Insert(index, (T)value);
			_Weights.Insert(index, new FlexibleFloat(weight));
		}

		public override object GetValueObject(int index)
		{
			return GetValue(index);
		}

		public T GetValue(int index)
		{
			return _Values[index];
		}

		public override void SetValueObject(int index, object value)
		{
			SetValue(index, (T)value);
		}

		public void SetValue(int index, T value)
		{
			_Values[index] = value;
		}

		public override float GetWeight(int index)
		{
			return _Weights[index].value;
		}

		public override void SetWeight(int index, float weight)
		{
			_Weights[index] = new FlexibleFloat(weight);
		}

		public override float GetTotalWeight()
		{
			if (_Values.Count == 0 || _Values.Count != _Weights.Count)
			{
				return 0;
			}

			float totalWeight = 0.0f;

			for (int index = 0, count = _Weights.Count; index < count; index++)
			{
				totalWeight += _Weights[index].value;
			}

			return totalWeight;
		}

		public T GetRandomItem()
		{
			float totalWeight = GetTotalWeight();

			if (totalWeight == 0.0f)
			{
				return default(T);
			}

			float r = Random.Range(0, totalWeight);

			totalWeight = 0.0f;

			int findIndex = 0;

			for (int index = 0, count = _Values.Count; index < count; index++)
			{
				float weight = _Weights[index].value;

				if (totalWeight <= r && r < totalWeight + weight)
				{
					findIndex = index;
					break;
				}

				totalWeight += weight;
			}

			return _Values[findIndex];
		}

		#region ISerializeVersionCallbackReceiver

		int ISerializeVersionCallbackReceiver.newestVersion
		{
			get
			{
				return kCurrentSerializeVersion;
			}
		}

		void ISerializeVersionCallbackReceiver.OnInitialize()
		{
			_SerializeVersion.version = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Weights.Clear();

			for (int index = 0; index < _OldWeights.Count; index++)
			{
				float weight = _OldWeights[index];
				_Weights.Add(new FlexibleFloat(weight));
			}

			//_OldWeights.Clear();
		}

		void ISerializeVersionCallbackReceiver.OnSerialize(int version)
		{
			switch (version)
			{
				case 0:
					SerializeVer1();
					break;
			}
		}

		void ISerializeVersionCallbackReceiver.OnVersioning()
		{
		}

		#endregion // ISerializeVersionCallbackReceiver

		#region ISerializationCallbackReceiver

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			_SerializeVersion.AfterDeserialize();
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			_SerializeVersion.BeforeDeserialize();
		}

		#endregion // ISerializationCallbackReceiver
	}
}