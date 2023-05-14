//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
	using Arbor.Utilities;

#if ARBOR_DOC_JA
	/// <summary>
	/// AnimatorのParameterを演算して変更する。
	/// </summary>
#else
	/// <summary>
	/// Calculate and change the parameter of Animator.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Animator/CalcAnimatorParameter")]
	[BuiltInBehaviour]
	public sealed class CalcAnimatorParameter : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region enum

		[Internal.Documentable]
		public enum Function
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// 値を代入する。
			/// </summary>
#else
			/// <summary>
			/// Substitute values.
			/// </summary>
#endif
			Assign,

#if ARBOR_DOC_JA
			/// <summary>
			/// 値を加算する。<br/>
			/// 減算したい場合は負値を指定する。
			/// </summary>
#else
			/// <summary>
			/// Add values.<br/>
			/// To subtract it, specify a negative value.
			/// </summary>
#endif
			Add,
		}

		[System.Serializable]
		public sealed class FlexibleFunction : FlexibleField<Function>
		{
			public FlexibleFunction()
			{
			}

			public FlexibleFunction(Function value) : base(value)
			{
			}

			public FlexibleFunction(AnyParameterReference parameter) : base(parameter)
			{
			}

			public FlexibleFunction(InputSlotAny slot) : base(slot)
			{
			}

			public static explicit operator Function(FlexibleFunction flexible)
			{
				return flexible.value;
			}

			public static explicit operator FlexibleFunction(Function value)
			{
				return new FlexibleFunction(value);
			}
		}

		#endregion // enum

		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 実行するメソッド
		/// </summary>
#else
		/// <summary>
		/// The method to execute
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(ExecuteMethodFlags))]
		private FlexibleExecuteMethodFlags _ExecuteMethodFlags = new FlexibleExecuteMethodFlags(ExecuteMethodFlags.Enter);

#if ARBOR_DOC_JA
		/// <summary>
		/// 参照するAnimatorとParameter名。
		/// </summary>
#else
		/// <summary>
		/// Reference Animator and Parameter name.
		/// </summary>
#endif
		[FormerlySerializedAs("reference")]
		[SerializeField]
		private AnimatorParameterReference _Reference = new AnimatorParameterReference();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するタイプ(Int、Floatのみ)。
		/// </summary>
#else
		/// <summary>
		/// Type to calculate (Int, Float only).
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(Function))]
		private FlexibleFunction _Function = new FlexibleFunction(Function.Assign);

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するfloat値
		/// </summary>
#else
		/// <summary>
		/// float value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _FloatValue = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するint値
		/// </summary>
#else
		/// <summary>
		/// int value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleInt _IntValue = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するbool値。<br/>
		/// bool値の場合はこの値をそのまま代入する。
		/// </summary>
#else
		/// <summary>
		/// bool value to compute.<br/>
		/// In case of bool value, substitute this value as it is.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _BoolValue = new FlexibleBool();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion;

		#region old

		[FormerlySerializedAs("floatValue")]
		[SerializeField]
		[HideInInspector]
		private float _OldFloatValue = 0f;

		[FormerlySerializedAs("intValue")]
		[SerializeField]
		[HideInInspector]
		private int _OldIntValue = 0;

		[FormerlySerializedAs("boolValue")]
		[SerializeField]
		[HideInInspector]
		private bool _OldBoolValue = false;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("function")]
		[FormerlySerializedAs("_Function")]
		private Function _OldFunction = Function.Assign;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 2;

		public float floatValue
		{
			get
			{
				return _FloatValue.value;
			}
		}

		public int intValue
		{
			get
			{
				return _IntValue.value;
			}
		}

		public bool boolValue
		{
			get
			{
				return _BoolValue.value;
			}
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_FloatValue = (FlexibleFloat)_OldFloatValue;
			_IntValue = (FlexibleInt)_OldIntValue;
			_BoolValue = (FlexibleBool)_OldBoolValue;
		}

		void SerializeVer2()
		{
			_Function = (FlexibleFunction)_OldFunction;
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
					case 1:
						SerializeVer2();
						_SerializeVersion++;
						break;
					default:
						_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
			Serialize();
		}

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			Serialize();
		}

		void DoCalc(ExecuteMethodFlags executeMethodFlags)
		{
			if ((_ExecuteMethodFlags.value & executeMethodFlags) != executeMethodFlags)
			{
				return;
			}

			Animator animator = _Reference.animator;
			if (animator == null)
			{
				return;
			}

			if (!_Reference.nameHash.HasValue)
			{
				return;
			}

			if (!AnimatorUtility.CheckExistsParameter(animator, _Reference.name))
			{
				return;
			}

			int nameHash = _Reference.nameHash.Value;

			Function function = _Function.value;

			switch (_Reference.type)
			{
				case AnimatorControllerParameterType.Float:
					{
						float value = floatValue;
						switch (function)
						{
							case Function.Add:
								value += animator.GetFloat(nameHash);
								break;
						}
						animator.SetFloat(nameHash, value);
					}
					break;
				case AnimatorControllerParameterType.Int:
					{
						int value = intValue;
						switch (function)
						{
							case Function.Add:
								value += animator.GetInteger(nameHash);
								break;
						}
						animator.SetInteger(nameHash, value);
					}
					break;
				case AnimatorControllerParameterType.Bool:
					{
						animator.SetBool(nameHash, boolValue);
					}
					break;
				case AnimatorControllerParameterType.Trigger:
					{
						animator.SetTrigger(nameHash);
					}
					break;
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			DoCalc(ExecuteMethodFlags.Enter);
		}

		public override void OnStateUpdate()
		{
			DoCalc(ExecuteMethodFlags.Update);
		}

		public override void OnStateLateUpdate()
		{
			DoCalc(ExecuteMethodFlags.LateUpdate);
		}

		public override void OnStateEnd()
		{
			DoCalc(ExecuteMethodFlags.Leave);
		}

		public override void OnStateFixedUpdate()
		{
			DoCalc(ExecuteMethodFlags.FixedUpdate);
		}
	}
}
