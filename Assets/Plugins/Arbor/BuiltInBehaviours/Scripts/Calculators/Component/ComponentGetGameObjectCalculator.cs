//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ComponentのGameObjectを出力する。
	/// </summary>
#else
	/// <summary>
	/// Output Component's GameObject.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Component/Component.GetGameObject")]
	[BehaviourTitle("Component.GetGameObject")]
	[BuiltInBehaviour]
	public sealed class ComponentGetGameObjectCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Component
		/// </summary>
		[SerializeField] private InputSlotComponent _Component = new InputSlotComponent();

		/// <summary>
		/// GameObject
		/// </summary>
		[SerializeField] private OutputSlotGameObject _GameObject = new OutputSlotGameObject();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			GameObject gameObject = null;
			Component component = null;
			if (_Component.GetValue(ref component) && component != null)
			{
				gameObject = component.gameObject;
			}

			_GameObject.SetValue(gameObject);
		}
	}
}
