//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Serializableの型がNodeBehaviour下のフィールドに割り当てられた際にフィールド情報を受け取るためのインターフェイス
	/// </summary>
#else
	/// <summary>
	/// Interface for receiving field information when a Serializable type is assigned to a field under NodeBehaviour
	/// </summary>
#endif
	public interface IAssignFieldReceiver
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 割り当てられたフィールド情報を受け取るコールバックメソッド。
		/// </summary>
		/// <param name="ownerObject">このフィールドを所有するオブジェクト</param>
		/// <param name="fieldInfo">フィールド情報</param>
#else
		/// <summary>
		/// A callback method that receives the assigned field information.
		/// </summary>
		/// <param name="ownerObject">The object that owns this field</param>
		/// <param name="fieldInfo">Field information</param>
#endif
		void OnAssignField(Object ownerObject, System.Reflection.FieldInfo fieldInfo);
	}
}