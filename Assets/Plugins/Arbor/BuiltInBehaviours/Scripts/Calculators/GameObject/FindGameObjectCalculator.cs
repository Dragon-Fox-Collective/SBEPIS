//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 名前でGameObjectを探して返す。
	/// </summary>
	/// <remarks>
	/// 詳しくは、<see cref="GameObject.Find(string)"/>を参照してください。
	/// </remarks>
#else
	/// <summary>
	/// Finds and returns a GameObject by name.
	/// </summary>
	/// <remarks>
	/// For more information, see <see cref="GameObject.Find(string)"/>.
	/// </remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("GameObject/FindGameObject")]
	[BehaviourTitle("FindGameObject")]
	[BuiltInBehaviour]
	public sealed class FindGameObjectCalculator : Calculator
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// GameObjectの名前
		/// </summary>
#else
		/// <summary>
		/// The name of the GameObject.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _Name = new FlexibleString();

#if ARBOR_DOC_JA
		/// <summary>
		/// 見つかったGameObjectを出力する。見つからなかった場合はnullを返す。
		/// </summary>
#else
		/// <summary>
		/// Output the found GameObject. If not found, it returns null.
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotGameObject _Output = new OutputSlotGameObject();

		// Use this for calculate
		public override void OnCalculate()
		{
			GameObject gameObject = GameObject.Find(_Name.value);
			_Output.SetValue(gameObject);
		}
	}
}