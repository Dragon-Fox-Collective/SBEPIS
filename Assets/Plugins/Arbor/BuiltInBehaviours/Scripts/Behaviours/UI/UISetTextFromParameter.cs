//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ARBOR_SUPPORT_UGUI
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// TextをParameterから設定します。
	/// </summary>
#else
	/// <summary>
	/// Set the Text from Parameter.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("UI/UISetTextFromParameter")]
	[BuiltInBehaviour]
	public sealed class UISetTextFromParameter : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象となるText。
		/// </summary>
#else
		/// <summary>
		/// Text of interest.
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Text))]
		private FlexibleComponent _Text = new FlexibleComponent(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 参照するParameter
		/// </summary>
#else
		/// <summary>
		/// Reference Parameter
		/// </summary>
#endif
		[SerializeField]
		private ParameterReference _Parameter = new ParameterReference();

#if ARBOR_DOC_JA
		/// <summary>
		/// 出力するフォーマット(Int、Floatのみ)<br/>
		/// フォーマットの詳細については、次を参照してください。<a href="https://msdn.microsoft.com/ja-jp/library/dwhawy9k(v=vs.110).aspx" target="_blank">標準の数値書式指定文字列</a>、<a href="https://msdn.microsoft.com/ja-jp/library/0c899ak8(v=vs.110).aspx" target="_blank">カスタム数値書式指定文字列</a>
		/// </summary>
#else
		/// <summary>
		/// Format to output (Int, Float only)<br/>
		/// For more information about numeric format specifiers, see <a href="https://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx" target="_blank">Standard Numeric Format Strings</a> and <a href="https://msdn.microsoft.com/en-us/library/0c899ak8(v=vs.110).aspx" target="_blank">Custom Numeric Format Strings</a>.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _Format = new FlexibleString(string.Empty);

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータが変更したときに更新するかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether to update when parameters change.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _ChangeTimingUpdate = new FlexibleBool(false);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_Text")]
		[HideInInspector]
		private Text _OldText = null;

		[SerializeField]
		[FormerlySerializedAs("_Format")]
		[HideInInspector]
		private string _OldFormat = string.Empty;

		[SerializeField]
		[FormerlySerializedAs("_ChangeTimingUpdate")]
		[HideInInspector]
		private bool _OldChangeTimingUpdate = false;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 3;

		public Text cachedText
		{
			get
			{
				return _Text.value as Text;
			}
		}

		void UpdateText()
		{
			Text text = cachedText;
			if (text != null)
			{
				if (_Parameter.parameter != null)
				{
					text.text = _Parameter.parameter.ToString(_Format.value);
				}
				else
				{
					text.text = string.Empty;
				}
			}
		}

		void OnChangedParameter(Parameter parameter)
		{
			UpdateText();
		}

		private Parameter _CachedParameter;
		private Parameter cachedParameter
		{
			get
			{
				if (_CachedParameter == null)
				{
					_CachedParameter = _Parameter.parameter;
				}
				return _CachedParameter;
			}
		}

		private bool _IsSettedOnChanged;

		void SetOnChanged()
		{
			if (_IsSettedOnChanged)
			{
				ReleaseOnChanged();
				_CachedParameter = null;
			}

			Parameter parameter = cachedParameter;
			if (parameter != null && _ChangeTimingUpdate.value)
			{
				parameter.onChanged += OnChangedParameter;
				_IsSettedOnChanged = true;
			}
		}

		void ReleaseOnChanged()
		{
			Parameter parameter = cachedParameter;
			if (parameter != null && _IsSettedOnChanged)
			{
				parameter.onChanged -= OnChangedParameter;
				_IsSettedOnChanged = false;
			}
		}

		void OnEnable()
		{
			SetOnChanged();
		}

		void OnDisable()
		{
			ReleaseOnChanged();
		}

		protected override void OnValidate()
		{
			base.OnValidate();

			if (Application.isPlaying && isActiveAndEnabled && _IsSettedOnChanged)
			{
				SetOnChanged();
			}
		}

		// Update is called once per frame
		public override void OnStateBegin()
		{
			UpdateText();
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Text = (FlexibleComponent)_OldText;
		}

		void SerializeVer2()
		{
			_Text.SetHierarchyIfConstantNull();
		}

		void SerializeVer3()
		{
			_Format = (FlexibleString)_OldFormat;
			_ChangeTimingUpdate = (FlexibleBool)_OldChangeTimingUpdate;
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
					case 2:
						SerializeVer3();
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
	}
}
#endif
