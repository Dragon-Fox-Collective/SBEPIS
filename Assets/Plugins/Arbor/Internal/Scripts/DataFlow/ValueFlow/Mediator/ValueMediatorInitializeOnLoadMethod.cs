//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor.ValueFlow
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ValueMeditorの初期化時に呼ばれるコールバックメソッドを指定する属性。
	/// </summary>
	/// <remarks>この属性を指定したメソッドはValueMeditorの初期化の際に呼ばれるため、その時にValueMeditor.Register{T}メソッドを呼び出して登録ができる。</remarks>
#else
	/// <summary>
	/// Attribute that specifies the callback method to be called when ValueMeditor is initialized.
	/// </summary>
	/// <remarks>The method that specifies this attribute is called during the initialization of ValueMeditor, so the ValueMeditor.Register{T} method can be called at that time for registration.</remarks>
#endif
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class ValueMediatorInitializeOnLoadMethod : Attribute
	{
	}
}