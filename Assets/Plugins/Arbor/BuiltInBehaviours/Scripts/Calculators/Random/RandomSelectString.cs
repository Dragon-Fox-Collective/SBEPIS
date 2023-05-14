//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
	[System.Serializable]
	[Arbor.Internal.Documentable]
	public sealed class WeightStringList : WeightList<FlexibleString>
	{
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// リストからランダムに選択したstringを出力する。
	/// </summary>
#else
	/// <summary>
	/// Output string selected randomly from the list.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Random/Random.SelectString")]
	[BehaviourTitle("Random.SelectString")]
	[BuiltInBehaviour]
	public sealed class RandomSelectString : Calculator
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// stringの重みリスト
		/// </summary>
#else
		/// <summary>
		/// string weight List
		/// </summary>
#endif
		[SerializeField]
		private WeightStringList _Weights = new WeightStringList();

#if ARBOR_DOC_JA
		/// <summary>
		/// 出力スロット
		/// </summary>
#else
		/// <summary>
		/// Output slot
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotString _Output = new OutputSlotString();

		public override bool OnCheckDirty()
		{
			return true;
		}

		public override void OnCalculate()
		{
			FlexibleString item = _Weights.GetRandomItem();

			string value = (item != null) ? item.value : string.Empty;
			_Output.SetValue(value);
		}
	}
}