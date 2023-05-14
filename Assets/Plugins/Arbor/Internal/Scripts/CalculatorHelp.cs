//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Calculatorのヘルプボタンから表示するURLを指定する属性。
	/// </summary>
#else
	/// <summary>
	/// Attribute that specifies the URL to be displayed from the Help button of Calculator.
	/// </summary>
#endif
	[Obsolete("use BehaviourHelp")]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class CalculatorHelp : Attribute
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
		/// CalculatorHelpコンストラクタ
		/// </summary>
		/// <param name="url">ヘルプページのurl</param>
#else
		/// <summary>
		/// CalculatorHelp constructor
		/// </summary>
		/// <param name="url">Help page url</param>
#endif
		public CalculatorHelp(string url)
		{
			_Url = url;
		}
	}
}
