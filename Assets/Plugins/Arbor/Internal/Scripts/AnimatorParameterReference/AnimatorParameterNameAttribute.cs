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
	/// <see cref="AnimatorName"/>にパラメータ名の設定を指定する属性。
	/// </summary>
#else
	/// <summary>
	/// An attribute that specifies the parameter name setting for <see cref="AnimatorName"/>.
	/// </summary>
#endif
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public sealed class AnimatorParameterNameAttribute : PropertyAttribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// タイプ持つフラグ。
		/// </summary>
#else
		/// <summary>
		/// A flag that has a type.
		/// </summary>
#endif
		public readonly bool hasType;

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータのタイプ
		/// </summary>
#else
		/// <summary>
		/// Parameter type
		/// </summary>
#endif
		public readonly AnimatorControllerParameterType type;

#if ARBOR_DOC_JA
		/// <summary>
		/// AnimatorParameterNameAttributeコンストラクタ<br/>
		/// パラメータタイプに制限なくパラメータ名を設定するのに使用します。
		/// </summary>
#else
		/// <summary>
		/// AnimatorParameterNameAttribute constructor<br/>
		/// Used to set parameter names without restrictions on the parameter type.
		/// </summary>
#endif
		public AnimatorParameterNameAttribute()
		{
			hasType = false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AnimatorParameterNameAttributeコンストラクタ<br/>
		/// 指定したパラメータタイプのみ設定できるようになります。
		/// </summary>
		/// <param name="type">パラメータのタイプ</param>
#else
		/// <summary>
		/// AnimatorParameterNameAttribute constructor<br/>
		/// Only the specified parameter type can be set.
		/// </summary>
		/// <param name="type">parameter type</param>
#endif
		public AnimatorParameterNameAttribute(AnimatorControllerParameterType type)
		{
			hasType = true;
			this.type = type;
		}
	}
}