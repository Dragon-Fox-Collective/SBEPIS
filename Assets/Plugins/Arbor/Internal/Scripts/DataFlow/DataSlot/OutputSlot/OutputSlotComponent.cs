//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Component型の出力スロット
	/// </summary>
	/// <remarks>
	/// 使用可能な属性 : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="HideSlotFields" /></description></item>
	/// </list>
	/// </remarks>
#else
	/// <summary>
	/// Component type of output slot
	/// </summary>
	/// <remarks>
	/// Available Attributes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="HideSlotFields" /></description></item>
	/// </list>
	/// </remarks>
#endif
	[System.Serializable]
	public sealed class OutputSlotComponent : OutputSlot<Component>
	{
		[SerializeField]
		[ClassComponent]
		[HideSlotFields]
		private ClassTypeReference _Type = new ClassTypeReference(typeof(Component));

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
				return _Type.type ?? typeof(Component);
			}
		}

		void OnTypeChanged()
		{
			ConnectableTypeChanged();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ISerializationCallbackReceiver.OnAfterDeserializeから呼び出される。
		/// </summary>
#else
		/// <summary>
		/// Called from ISerializationCallbackReceiver.OnAfterDeserialize.
		/// </summary>
#endif
		protected override void OnAfterDeserialize()
		{
			base.OnAfterDeserialize();

			_Type.onTypeChanged -= OnTypeChanged;
			_Type.onTypeChanged += OnTypeChanged;
		}
	}
}