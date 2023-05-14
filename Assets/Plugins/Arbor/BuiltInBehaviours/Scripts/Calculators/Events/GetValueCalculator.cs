//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
	using Arbor.Events;

#pragma warning disable 1572
#if ARBOR_DOC_JA
	/// <summary>
	/// フィールドやプロパティの値を取得し、データフローに出力する。
	/// </summary>
	/// <param name="Type">値を取得するメンバーがある型</param>
	/// <param name="Member">値を取得するメンバー</param>
	/// <param label="&lt;Target&gt;">インスタンス</param>
	/// <param name="OutputValue">値の出力スロット</param>
	/// <remarks>
	/// 呼び出しはリフレクションを介して行われるため、ストリッピングにより呼び出し先メンバーがビルドから削除される可能性があります。<br />
	/// 詳しくは<a href="https://docs.unity3d.com/ja/current/Manual/ManagedCodeStripping.html">マネージコードストリッピング - Unity マニュアル</a>を参照してください。
	/// </remarks>
#else
	/// <summary>
	/// Get the values of fields and properties and output them to the data flow.
	/// </summary>
	/// <param name="Type">The type whose members get the values</param>
	/// <param name="Member">Member to get the value from</param>
	/// <param label="&lt;Target&gt;">Instance</param>
	/// <param name="OutputValue">Value output slot</param>
	/// <remarks>
	/// Since the call is made via reflection, stripping can cause the called member to be removed from the build.<br />
	/// See <a href="https://docs.unity3d.com/Manual/ManagedCodeStripping.html">Unity - Manual:  Managed code stripping</a> for more information.
	/// </remarks>
#endif
#pragma warning restore 1572
	[AddComponentMenu("")]
	[AddBehaviourMenu("Events/GetValue")]
	[BehaviourTitle("GetValue")]
	[BuiltInBehaviour]
	[Internal.DocumentManual("/manual/dataflow/invoke.md")]
	public sealed class GetValueCalculator : Calculator
	{
		[SerializeField]
		[Internal.HideInDocument]
		private PersistentGetValue _Persistent = new PersistentGetValue();

		protected override void Awake()
		{
			LogWarning();
		}

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			_Persistent.Invoke();
		}

		[System.Diagnostics.Conditional("UNITY_EDITOR")]
		void LogWarning()
		{
			string warningMessage = _Persistent.GetWarningMessage();
			if (!string.IsNullOrEmpty(warningMessage))
			{
				Debug.LogWarningFormat(nodeGraph, "[{0} Event]\n{1}", this, warningMessage);
			}
		}
	}
}