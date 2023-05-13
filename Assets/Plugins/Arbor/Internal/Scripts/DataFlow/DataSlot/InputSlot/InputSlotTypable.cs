//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 型を指定する入力スロットクラス
	/// </summary>
	/// <remarks>
	/// 使用可能な属性 : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="HideSlotFields" /></description></item>
	/// </list>
	/// </remarks>
#else
	/// <summary>
	/// Input slot class specifying type
	/// </summary>
	/// <remarks>
	/// Available Attributes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="HideSlotFields" /></description></item>
	/// </list>
	/// </remarks>
#endif
	[System.Serializable]
	public sealed class InputSlotTypable : InputSlotBase, ISerializationCallbackReceiver
	{
		[SerializeField]
		[HideSlotFields]
		private ClassTypeReference _Type = new ClassTypeReference();

#if ARBOR_DOC_JA
		/// <summary>
		/// スロットに格納されるデータの型
		/// </summary>
#else
		/// <summary>
		/// The type of data stored in the slot
		/// </summary>
#endif
		public override System.Type dataType
		{
			get
			{
				return _Type.type;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// InputSlotTypableのコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// InputSlotTypable constructor
		/// </summary>
#endif
		public InputSlotTypable()
		{
			RegisterCallbackTypeChanged();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// InputSlotTypableのコンストラクタ
		/// </summary>
		/// <param name="type">スロットに格納するデータ型</param>
#else
		/// <summary>
		/// InputSlotTypable constructor
		/// </summary>
		/// <param name="type">Data type to be stored in the slot</param>
#endif
		public InputSlotTypable(System.Type type) : this()
		{
			SetType(type);
		}

		internal void SetType(System.Type type)
		{
			_Type.type = type;
		}

		void OnTypeChanged()
		{
			ConnectableTypeChanged();
		}

		void RegisterCallbackTypeChanged()
		{
			_Type.onTypeChanged -= OnTypeChanged;
			_Type.onTypeChanged += OnTypeChanged;
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			RegisterCallbackTypeChanged();
		}
	}
}