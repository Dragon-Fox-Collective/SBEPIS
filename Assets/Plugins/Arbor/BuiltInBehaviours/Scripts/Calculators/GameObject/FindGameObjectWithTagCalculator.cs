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
	/// 指定タグがついた一つのアクティブGameObjectを探して返す。
	/// </summary>
	/// <remarks>
	/// 詳しくは、<see cref="GameObject.FindWithTag(string)"/>を参照してください。
	/// </remarks>
#else
	/// <summary>
	/// Finds and returns one active GameObject with the specified tag.
	/// </summary>
	/// <remarks>
	/// For more information, see <see cref="GameObject.FindWithTag(string)"/>.
	/// </remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("GameObject/FindGameObjectWithTag")]
	[BehaviourTitle("FindGameObjectWithTag")]
	[BuiltInBehaviour]
	public sealed class FindGameObjectWithTagCalculator : Calculator
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// タグ
		/// </summary>
#else
		/// <summary>
		/// Tag
		/// </summary>
#endif
		[SerializeField]
		[TagSelector]
		private FlexibleString _Tag = new FlexibleString("Untagged");

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
			var gameObject = GameObject.FindWithTag(_Tag.value);
			_Output.SetValue(gameObject);
		}
	}
}