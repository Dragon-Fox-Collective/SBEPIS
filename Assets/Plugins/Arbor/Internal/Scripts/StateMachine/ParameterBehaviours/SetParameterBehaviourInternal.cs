//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Arbor.ParameterBehaviours
{
	using Arbor.Events;
	using Arbor.ValueFlow;

#pragma warning disable 1574
#if ARBOR_DOC_JA
	/// <summary>
	/// <see cref="Arbor.ParameterBehaviours.SetParameterBehaviour" />の内部クラス。
	/// </summary>
#else
	/// <summary>
	/// Internal class of <see cref="Arbor.ParameterBehaviours.SetParameterBehaviour" />.
	/// </summary>
#endif
#pragma warning restore 1574
	[AddComponentMenu("")]
	[HideBehaviour]
	public class SetParameterBehaviourInternal : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize Fields

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
		/// 設定先パラメータ。
		/// </summary>
#else
		/// <summary>
		/// Setting destination parameter.
		/// </summary>
#endif
		[SerializeField]
		private ParameterReference _ParameterReference = new ParameterReference();

		[SerializeField]
		[Internal.HideInDocument]
		private ClassTypeReference _ValueType = new ClassTypeReference();

		[SerializeField]
		[Internal.HideInDocument]
		private ParameterType _ParameterType = ParameterType.Unknown;

#if ARBOR_DOC_JA
		/// <summary>
		/// 設定する値
		/// </summary>
#else
		/// <summary>
		/// Value to set
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentLabel("Value")]
		private ParameterList _ParameterList = new ParameterList();

		[SerializeField]
		[HideInInspector]
		private bool _IsInGraphParameter = false;

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideSlotFields]
		[FormerlySerializedAs("_Input")]
		[HideInInspector]
		private InputSlotTypable _OldInput = new InputSlotTypable();

		#endregion // old

		#endregion // Serialize Fields

#if ARBOR_DOC_JA
		/// <summary>
		/// グラフ内パラメータを参照しているかどうか。
		/// </summary>
#else
		/// <summary>
		///Whether this refers to a parameter in the graph.
		/// </summary>
#endif
		public bool isInGraphParameter
		{
			get
			{
				return _IsInGraphParameter;
			}
		}

		private const int kCurrentSerializeVersion = 2;

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータを設定する。
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// Set parameter.
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public void SetParameter(Parameter parameter)
		{
			_ParameterReference.container = parameter.container;
			_ParameterReference.id = parameter.id;
			_ParameterReference.name = parameter.name;

			System.Type valueType = parameter.valueType;

			_ValueType = new ClassTypeReference(valueType);
			_ParameterType = _ParameterList.AddElement(valueType, null);

			SetupIsInGraphParameter();

			RebuildFields();
		}

		void DoSetValue(ExecuteMethodFlags executeMethodFlags)
		{
			if ((_ExecuteMethodFlags.value & executeMethodFlags) != executeMethodFlags)
			{
				return;
			}

			Parameter parameter = _ParameterReference.parameter;
			if (parameter == null)
			{
				return;
			}

			System.Type valueType = _ValueType.type;

			if (valueType == null)
			{
				if (!string.IsNullOrEmpty(_ValueType.assemblyTypeName))
				{
					Debug.LogError("Type not found. It may have been deleted or renamed : " + this, this);
				}
				else
				{
					Debug.LogError("The element type is not specified : " + this, this);
				}
				return;
			}

			System.Type parameterValueType = parameter.valueType;

			if (parameterValueType != valueType)
			{
				Debug.LogError("The value type has changed : " + this, this);
				return;
			}

			ParameterType parameterType = ArborEventUtility.GetParameterType(parameterValueType, true);

			if (_ParameterType != parameterType)
			{
				Debug.LogError("The parameter type has changed : " + this, this);
				return;
			}

			IList list = _ParameterList.GetParameterList(_ParameterType);
			if (list == null)
			{
				return;
			}

			int count = list.Count;
			if (count == 0)
			{
				return;
			}

			IValueGetter valueContainer = list[0] as IValueGetter;

			if (valueContainer == null)
			{
				return;
			}

			parameter.SetValue(valueContainer);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Stateに入った際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when [state enter].
		/// </summary>
#endif
		public override void OnStateBegin()
		{
			DoSetValue(ExecuteMethodFlags.Enter);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Stateの更新。毎フレーム呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Update of State. It is called every frame.
		/// </summary>
#endif
		public override void OnStateUpdate()
		{
			DoSetValue(ExecuteMethodFlags.Update);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// State用のLateUpdate。毎フレーム、全てのUpdate後に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// LateUpdate for State. Every frame, called after all updates.
		/// </summary>
#endif
		public override void OnStateLateUpdate()
		{
			DoSetValue(ExecuteMethodFlags.LateUpdate);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Stateから出る際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when [state exit].
		/// </summary>
#endif
		public override void OnStateEnd()
		{
			DoSetValue(ExecuteMethodFlags.Leave);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// State用のFixedUpdate。物理演算のためのフレームレートに依存しないUpdate。
		/// </summary>
#else
		/// <summary>
		/// FixedUpdate for State. Frame-rate-independent Update for physics operations.
		/// </summary>
#endif
		public override void OnStateFixedUpdate()
		{
			DoSetValue(ExecuteMethodFlags.FixedUpdate);
		}

		void SetUpIsInGraphParameterInternal()
		{
			ParameterContainerBase container = _ParameterReference.container;
			_IsInGraphParameter = (nodeGraph.parameterContainer != null && container != null && nodeGraph.parameterContainer == container);
		}

		void SetupIsInGraphParameter()
		{
#if !ARBOR_DEBUG_IN_GRAPH_PARAMETER
			if (_ParameterReference.type != ParameterReferenceType.Constant)
			{
				_IsInGraphParameter = false;
				return;
			}

			NodeGraph nodeGraph = this.nodeGraph;

			if (nodeGraph == null)
			{
				_IsInGraphParameter = false;
				return;
			}

			if (!nodeGraph.isDeserialized)
			{
				nodeGraph.onAfterDeserialize += SetUpIsInGraphParameterInternal;
			}
			else
			{
				SetUpIsInGraphParameterInternal();
			}
#endif
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// デフォルト値にリセットする。
		/// </summary>
#else
		/// <summary>
		/// Reset to default values.
		/// </summary>
#endif
		protected virtual void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			SetupIsInGraphParameter();
		}

		void SerializeVer2()
		{
			_ValueType = _OldInput.dataType;
			_ParameterType = _ParameterList.AddElement(_ValueType, _OldInput);

			_OldInput.ClearBranch();
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

			_ParameterList.SetOverrideConstraint(_ParameterType, _ValueType.type);
		}
	}
}