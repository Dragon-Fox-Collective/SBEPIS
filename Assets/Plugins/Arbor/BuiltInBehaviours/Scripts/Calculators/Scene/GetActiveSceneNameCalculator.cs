//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// アクティブシーン名を取得する。
	/// </summary>
#else
	/// <summary>
	/// Get active scenbe name.
	/// </summary>
#endif
	[AddBehaviourMenu("Scene/GetActiveSceneName")]
	[BehaviourTitle("GetActiveSceneName")]
	[BuiltInBehaviour]
	[AddComponentMenu("")]
	public sealed class GetActiveSceneNameCalculator : Calculator
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// アクティブシーン名の出力。
		/// </summary>
#else
		/// <summary>
		/// Output active scene name.
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotString _Output = new OutputSlotString();

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			Scene scene = SceneManager.GetActiveScene();
			_Output.SetValue(scene.name);
		}
	}
}