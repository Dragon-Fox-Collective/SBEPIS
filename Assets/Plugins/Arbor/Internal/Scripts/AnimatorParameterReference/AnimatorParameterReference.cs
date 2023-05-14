//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Animatorパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Animator parameters.
	/// </summary>
#endif
	[System.Serializable]
	public class AnimatorParameterReference : ISerializationCallbackReceiver, ISerializeVersionCallbackReceiver
	{
		#region Serialize Fields

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータが格納されているAnimator
		/// </summary>
#else
		/// <summary>
		/// Animator parameters are stored.
		/// </summary>
#endif
		[SerializeField, SlotType(typeof(Animator))]
		private FlexibleComponent _Animator = new FlexibleComponent(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの名前
		/// </summary>
#else
		/// <summary>
		/// Parameter name.
		/// </summary>
#endif
		[SerializeField]
		private AnimatorName _Name = new AnimatorName();

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータのタイプ
		/// </summary>
#else
		/// <summary>
		/// Parameter type.
		/// </summary>
#endif
		public AnimatorControllerParameterType type = AnimatorControllerParameterType.Float;

		[SerializeField, HideInInspector]
		private SerializeVersion _SerializeVersion = new SerializeVersion();

		#region old

		// version 0
		[SerializeField, HideInInspector, FormerlySerializedAs("animator")]
		private Animator _OldAnimator = null;

		// version 1
		[SerializeField, HideInInspector, FormerlySerializedAs("name")]
		private string _OldName = "";

		#endregion // old

		#endregion // Serialize Fields

		private const int kCurrentSerializeVersion = 2;

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータが格納されているAnimator
		/// </summary>
#else
		/// <summary>
		/// Animator parameters are stored.
		/// </summary>
#endif
		public Animator animator
		{
			get
			{
				return _Animator.value as Animator;
			}
			set
			{
				_Animator.SetConstant(animator);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの名前
		/// </summary>
#else
		/// <summary>
		/// Parameter name.
		/// </summary>
#endif
		public string name
		{
			get
			{
				return _Name.name;
			}
			set
			{
				_Name.name = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータ名のハッシュ値を返す。パラメータ名がnullか空の場合はnullとなる。
		/// </summary>
#else
		/// <summary>
		/// Returns the hash value of the parameter name. If the parameter name is null or empty, it will be null.
		/// </summary>
#endif
		public int? nameHash
		{
			get
			{
				return _Name.hash;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AnimatorParameterReferenceのコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// AnimatorParameterReference constructor
		/// </summary>
#endif
		public AnimatorParameterReference()
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
			_Animator.SetConstant(_OldAnimator);
		}

		void SerializeVer2()
		{
			_Name.name = _OldName;
		}

		void ISerializeVersionCallbackReceiver.OnSerialize(int version)
		{
			switch (version)
			{
				case 0:
					SerializeVer1();
					break;
				case 1:
					SerializeVer2();
					break;
			}
		}

		void ISerializeVersionCallbackReceiver.OnVersioning()
		{
		}

		#endregion // ISerializeVersionCallbackReceiver

		#region ISerializationCallbackReceiver

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			_SerializeVersion.BeforeDeserialize();
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			_SerializeVersion.AfterDeserialize();
		}

		#endregion // ISerializationCallbackReceiver
	}
}
