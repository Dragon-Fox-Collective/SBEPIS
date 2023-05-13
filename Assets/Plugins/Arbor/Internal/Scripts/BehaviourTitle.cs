//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// StateBehaviourの表示するタイトルを指定する属性。
	/// </summary>
#else
	/// <summary>
	/// Attribute that specifies the title to display the StateBehaviour.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	[Internal.DocumentManual("/manual/scripting/behaviourattribute/BehaviourTitle.md")]
	public sealed class BehaviourTitle : Attribute
	{
		private string _TitleName;

#if ARBOR_DOC_JA
		/// <summary>
		/// ローカライズを行うフラグ。trueの場合、titleNameをキーとして各言語のワードに変換する。
		/// </summary>
#else
		/// <summary>
		/// Flag for localization. If true, convert to word of each language with titleName as key.
		/// </summary>
#endif
		public bool localization;

#if ARBOR_DOC_JA
		/// <summary>
		/// Editor上で表示を行うための名前を使用するフラグ。デフォルトtrue。
		/// </summary>
#else
		/// <summary>
		/// Flag that uses a name to display on Editor. Default true.
		/// </summary>
#endif
		public bool useNicifyName = true;

#if ARBOR_DOC_JA
		/// <summary>
		/// タイトル名
		/// </summary>
#else
		/// <summary>
		/// Title name
		/// </summary>
#endif
		public string titleName
		{
			get
			{
				return _TitleName;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BehaviourTitleのコンストラクタ
		/// </summary>
		/// <param name="titleName">タイトル名</param>
#else
		/// <summary>
		/// BehaviourTitle constructor
		/// </summary>
		/// <param name="titleName">Title name</param>
#endif
		public BehaviourTitle(string titleName)
		{
			_TitleName = titleName;
		}
	}
}