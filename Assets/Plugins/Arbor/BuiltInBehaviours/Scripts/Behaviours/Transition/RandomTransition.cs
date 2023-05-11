//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ランダムに遷移する。
	/// </summary>
#else
	/// <summary>
	/// Transit randomly.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/RandomTransition")]
	[BuiltInBehaviour]
	public sealed class RandomTransition : StateBehaviour
	{
		#region inner class

#if ARBOR_DOC_JA
		/// <param name="Probability" order="10">実際の確率</param>
#else
		/// <param name="Probability" order="10">Actual probability</param>
#endif
		[System.Serializable]
		[Arbor.Internal.Documentable]
		private sealed class LinkWeight : ISerializationCallbackReceiver, ISerializeVersionCallbackReceiver
		{
			#region Serialize fields

#if ARBOR_DOC_JA
			/// <summary>
			/// 遷移しやすさ。個別のWeight/全体のWeightによって確率が決まる。
			/// </summary>
#else
			/// <summary>
			/// Ease of transition. Probability depends on individual weight / overall weight.
			/// </summary>
#endif
			[SerializeField]
			[ConstantRange(0, 100)]
			private FlexibleFloat _Weight = new FlexibleFloat(0f);

#if ARBOR_DOC_JA
			/// <summary>
			/// 遷移先ステート。<br />
			/// 遷移メソッド : OnStateBegin
			/// </summary>
#else
			/// <summary>
			/// Transition destination state.<br />
			/// Transition Method : OnStateBegin
			/// </summary>
#endif
			public StateLink link = new StateLink();

			[SerializeField]
			[HideInInspector]
			private SerializeVersion _SerializeVersion = new SerializeVersion();

			#region old

			[SerializeField]
			[HideInInspector]
			[FormerlySerializedAs("weight")]
			private float _OldWeight = 0f;

			#endregion // old

			#endregion // Serialize fields

			private const int kCurrentSerializeVersion = 1;

			public float weight
			{
				get
				{
					return _Weight.value;
				}
			}

#if ARBOR_DOC_JA
			/// <summary>
			/// LinkWeightコンストラクタ
			/// </summary>
#else
			/// <summary>
			/// LinkWeight constructor
			/// </summary>
#endif
			public LinkWeight()
			{
				// Initialize when calling from script.
				_SerializeVersion.Initialize(this);
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
				_Weight = (FlexibleFloat)_OldWeight;
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

		#endregion // inner class

		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先リスト。
		/// </summary>
#else
		/// <summary>
		/// Transition destination list.
		/// </summary>
#endif
		[SerializeField]
		private List<LinkWeight> _Links = new List<LinkWeight>();

		#endregion // Serialize fields

		public float GetTotalWeight()
		{
			if (_Links.Count == 0)
			{
				return 0;
			}

			float totalWeight = 0.0f;

			int linkCount = _Links.Count;
			for (int linkIndex = 0; linkIndex < linkCount; linkIndex++)
			{
				LinkWeight link = _Links[linkIndex];
				totalWeight += link.weight;
			}

			return totalWeight;
		}

		StateLink GetRandomLink()
		{
			float totalWeight = GetTotalWeight();

			if (totalWeight == 0.0f)
			{
				return null;
			}

			float r = Random.Range(0, totalWeight);

			totalWeight = 0.0f;

			int index = 0;

			int linkCount = _Links.Count;
			for (int linkIndex = 0; linkIndex < linkCount; linkIndex++)
			{
				LinkWeight link = _Links[linkIndex];

				if (totalWeight <= r && r < totalWeight + link.weight)
				{
					index = linkIndex;
					break;
				}

				totalWeight += link.weight;
			}

			return _Links[index].link;
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			StateLink link = GetRandomLink();
			if (link != null)
			{
				Transition(link);
			}
		}
	}
}
