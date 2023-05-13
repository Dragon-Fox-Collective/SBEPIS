//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 間違ったタイプのパラメータの値にアクセスしたときにスローされる例外。
	/// </summary>
#else
	/// <summary>
	/// The exception that is thrown when accessing the value of the wrong type of parameter.
	/// </summary>
#endif
	public sealed class ParameterTypeMismatchException : System.Exception
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="ParameterTypeMismatchException"/>クラスの新しいインスタンスを初期化。
		/// </summary>
#else
		/// <summary>
		/// Initializes a new instance of the <see cref="ParameterTypeMismatchException"/> class.
		/// </summary>
#endif
		public ParameterTypeMismatchException() : base("It can not be assigned because the parameter type is different.")
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定したエラーメッセージを使用して、<see cref="ParameterTypeMismatchException"/>クラスの新しいインスタンスを初期化。
		/// </summary>
		/// <param name="message">エラーメッセージ文字列。</param>
#else
		/// <summary>
		/// Initializes a new instance of the <see cref="ParameterTypeMismatchException"/> class with a specified error message.
		/// </summary>
		/// <param name="message">Error message string.</param>
#endif
		public ParameterTypeMismatchException(string message) : base(message)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定したエラーメッセージおよびこの例外の原因となった内部例外への参照を使用して、<see cref="ParameterTypeMismatchException"/>クラスの新しいインスタンスを初期化。
		/// </summary>
		/// <param name="message">エラーメッセージ文字列。</param>
		/// <param name="inner">現在の例外の原因となった例外。</param>
#else
		/// <summary>
		/// Initializes a new instance of the <see cref="ParameterTypeMismatchException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">Error message string.</param>
		/// <param name="inner">The exception that is the cause of the current exception.</param>
#endif
		public ParameterTypeMismatchException(string message, System.Exception inner) : base(message, inner)
		{
		}
	}
}