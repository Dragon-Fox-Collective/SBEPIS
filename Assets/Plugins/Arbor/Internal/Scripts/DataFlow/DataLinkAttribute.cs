//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 通常のフィールドをDataSlot化する属性。
	/// </summary>
#else
	/// <summary>
	/// Attribute that makes DataSlot of normal field.
	/// </summary>
#endif
	[System.AttributeUsage(System.AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class DataLinkAttribute : PropertyAttribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// UpdateTimingが指定されているかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether UpdateTiming is specified.
		/// </summary>
#endif
		public bool hasUpdateTiming
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定されているUpdateTiming
		/// </summary>
#else
		/// <summary>
		/// Specified UpdateTiming
		/// </summary>
#endif
		public DataLinkUpdateTiming updateTiming
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataLinkAttributeコンストラクタ<br/>
		/// このコンストラクタを使用してエディタからUpdateTimingを指定します。
		/// </summary>
#else
		/// <summary>
		/// DataLinkAttribute constructor<br/>
		/// Use this constructor to specify UpdateTiming from the field.
		/// </summary>
#endif
		public DataLinkAttribute()
		{
			hasUpdateTiming = false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataLinkAttributeコンストラクタ<br/>
		/// UpdateTimingを指定しエディタで変更させないようにする場合はこのコンストラクタを使用する。
		/// </summary>
		/// <param name="updateTiming">指定するUpdateTiming</param>
#else
		/// <summary>
		/// DataLinkAttribute constructor<br/>
		/// Use this constructor to specify UpdateTiming and not to change it in the editor.
		/// </summary>
		/// <param name="updateTiming">Specify UpdateTiming</param>
#endif
		public DataLinkAttribute(DataLinkUpdateTiming updateTiming)
		{
			hasUpdateTiming = true;
			this.updateTiming = updateTiming;
		}
	}
}