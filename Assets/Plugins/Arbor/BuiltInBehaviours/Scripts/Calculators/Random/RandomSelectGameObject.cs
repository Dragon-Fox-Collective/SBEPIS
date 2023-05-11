//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
	[System.Serializable]
	[Arbor.Internal.Documentable]
	public sealed class WeightGameObjectList : WeightList<FlexibleGameObject>
	{
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// リストからランダムに選択したGameObjectを出力する。
	/// </summary>
#else
	/// <summary>
	/// Output GameObject selected randomly from the list.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Random/Random.SelectGameObject")]
	[BehaviourTitle("Random.SelectGameObject")]
	[BuiltInBehaviour]
	public sealed class RandomSelectGameObject : Calculator
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// GameObjectの重みリスト
		/// </summary>
#else
		/// <summary>
		/// GameObject weight list
		/// </summary>
#endif
		[SerializeField]
		private WeightGameObjectList _Weights = new WeightGameObjectList();

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
		private OutputSlotGameObject _Output = new OutputSlotGameObject();

		public override bool OnCheckDirty()
		{
			return true;
		}

		public override void OnCalculate()
		{
			FlexibleGameObject item = _Weights.GetRandomItem();

			GameObject value = (item != null) ? item.value : null;
			_Output.SetValue(value);
		}
	}
}