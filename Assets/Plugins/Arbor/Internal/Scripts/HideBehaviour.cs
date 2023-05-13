//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// AddBehaviourメニューに表示しないようにする属性。
	/// </summary>
#else
	/// <summary>
	/// The attributes you do not want to display to AddBehaviour menu.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	[Internal.DocumentManual("/manual/scripting/behaviourattribute/HideBehaviour.md")]
	public sealed class HideBehaviour : System.Attribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// HideBehaviourコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// HideBehaviour constructor
		/// </summary>
#endif
		public HideBehaviour()
		{
		}
	}
}
