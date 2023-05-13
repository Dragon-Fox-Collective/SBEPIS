//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Quaternionをオイラー角で編集する属性。
	/// </summary>
#else
	/// <summary>
	/// Attribute to edit Quaternion at Euler angles.
	/// </summary>
#endif
	[System.AttributeUsage(System.AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class EulerAnglesAttribute : PropertyAttribute
	{
	}
}