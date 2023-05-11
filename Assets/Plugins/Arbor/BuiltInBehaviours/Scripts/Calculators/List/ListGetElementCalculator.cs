//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor.Calculators
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Listの要素を取得する。
	/// </summary>
	/// <remarks>
	/// IL2CPPなどのAOT環境では、List&lt;指定した型&gt;がコード上で使用していないと正常動作しない可能性があります。<br />
	/// 詳しくは、<a href="https://arbor-docs.caitsithware.com/ja/manual/dataflow/list.html#AOTRestrictions">事前コンパイル(AOT)での制限</a>を参照してください。
	/// </remarks>
#else
	/// <summary>
	/// Get List element.
	/// </summary>
	/// <remarks>
	/// In an AOT environment such as IL2CPP, List&lt;specified type&gt; may not work properly unless it is used in the code.<br />
	/// See <a href="https://arbor-docs.caitsithware.com/en/manual/dataflow/list.html#AOTRestrictions">Ahead-of-Time (AOT) Restrictions</a> for more information.
	/// </remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("List/List.GetElement")]
	[BehaviourTitle("List.GetElement", useNicifyName = false)]
	[BuiltInBehaviour]
	public sealed class ListGetElementCalculator : Calculator
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

#if ARBOR_DOC_JA
		/// <summary>
		/// 取得するインデックス
		/// </summary>
#else
		/// <summary>
		/// The index to get
		/// </summary>
#endif
		[SerializeField]
		private FlexibleInt _Index = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果の出力スロット
		/// </summary>
#else
		/// <summary>
		/// Resulting output slot
		/// </summary>
#endif
		[SerializeField]
		[HideSlotFields]
		private OutputSlotTypable _Output = new OutputSlotTypable();

		// Use this for calculate
		public override void OnCalculate()
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

			int index = _Index.value;

			var listElementType = ListMediator.GetElementType(list, _Output);
			if (listElementType != null)
			{
				elementType = listElementType;
			}

			ValueMediator.Get(elementType).listMediator.GetElement(list, index, _Output);
		}
	}
}