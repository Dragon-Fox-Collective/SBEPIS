//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Listを変更する挙動の基本クラス
	/// </summary>
#else
	/// <summary>
	/// Base class for the behavior of changing List
	/// </summary>
#endif
	public abstract class ListBehaviourBase : StateBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 要素の型
		/// </summary>
#else
		/// <summary>
		/// Element type
		/// </summary>
#endif
		[SerializeField]
		[ClassNotStaticConstraint]
		[TypeFilter((TypeFilterFlags)(-1) & ~(TypeFilterFlags.Static))]
		private ClassTypeReference _ElementType = new ClassTypeReference();

#if ARBOR_DOC_JA
		/// <summary>
		/// 変更結果のListの出力スロット
		/// </summary>
#else
		/// <summary>
		/// Output slot of the list of changes
		/// </summary>
#endif
		[SerializeField]
		[HideSlotFields]
		private OutputSlotTypable _Output = new OutputSlotTypable();

#if ARBOR_DOC_JA
		/// <summary>
		/// Listのインスタンスを変更するタイプ
		/// </summary>
#else
		/// <summary>
		/// The type that modifies the List instance
		/// </summary>
#endif
		[SerializeField]
		private ListInstanceType _OutputType = ListInstanceType.Keep;

#if ARBOR_DOC_JA
		/// <summary>
		/// Listの入力スロット
		/// </summary>
#else
		/// <summary>
		/// List input slot
		/// </summary>
#endif
		[SerializeField]
		[HideSlotFields]
		private InputSlotTypable _Input = new InputSlotTypable();

		protected System.Type elementType
		{
			get
			{
				return _ElementType;
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			CallOnExecute();
		}

		void CallOnExecute()
		{
			System.Type elementType = _ElementType.type;

			if (elementType == null)
			{
				if (!string.IsNullOrEmpty(_ElementType.assemblyTypeName))
				{
					Debug.LogError("Type not found. It may have been deleted or renamed : " + this, this);
				}
				else
				{
					Debug.LogError("The element type is not specified : " + this, this);
				}
				return;
			}

			object array = null;
			if (!_Input.GetValue(ref array))
			{
				return;
			}

			IList list = array as IList;
			if (list == null)
			{
				return;
			}

			object returnArray = OnExecute(elementType, list, _OutputType);
			if (returnArray != null)
			{
				_Output.SetValue(returnArray);
			}
		}

		protected abstract object OnExecute(System.Type elementType, IList list, ListInstanceType outputType);
	}
}