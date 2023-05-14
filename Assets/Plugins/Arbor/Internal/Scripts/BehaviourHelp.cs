//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// StateBehaviourのヘルプボタンから表示するURLを指定する属性。
	/// </summary>
#else
	/// <summary>
	/// Attribute that specifies the URL to be displayed from the Help button of StateBehaviour.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	[Internal.DocumentManual("/manual/scripting/behaviourattribute/BehaviourHelp.md")]
	public sealed class BehaviourHelp : Attribute
	{
		private string _Url;

#if ARBOR_DOC_JA
		/// <summary>
		/// ヘルプボタンを押した際にブラウザに表示するurl
		/// </summary>
#else
		/// <summary>
		/// Url to display in browser when pressing help button
		/// </summary>
#endif
		public string url
		{
			get
			{
				return _Url;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BehaviourHelpコンストラクタ
		/// </summary>
		/// <param name="url">ヘルプページのurl</param>
#else
		/// <summary>
		/// BehaviourHelp constructor
		/// </summary>
		/// <param name="url">Help page url</param>
#endif
		public BehaviourHelp(string url)
		{
			_Url = url;
		}
	}
}