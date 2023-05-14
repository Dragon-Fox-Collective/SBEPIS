//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 演算ノードを表すクラス
	/// </summary>
#else
	/// <summary>
	/// Class that represents a calculator
	/// </summary>
#endif
	[System.Serializable]
	public sealed class CalculatorNode : Node, INodeBehaviourContainer
	{
		[SerializeField]
		[FormerlySerializedAs("_Calculator")]
		private Object _Object;

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算ノードIDを取得。
		/// </summary>
#else
		/// <summary>
		/// Gets the calculator node identifier.
		/// </summary>
#endif
		[System.Obsolete("use Node.nodeID")]
		public int calculatorID
		{
			get
			{
				return nodeID;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 割り当てているCalculatorを取得。
		/// </summary>
#else
		/// <summary>
		/// Get the attached Calculator.
		/// </summary>
#endif
		public Calculator calculator
		{
			get
			{
				return _Object as Calculator;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 割り当てているCalculatorのObjectを取得。
		/// </summary>
		/// <returns>割り当てているCalculatorのObject</returns>
#else
		/// <summary>
		/// Get the Object of the attached Calculator
		/// </summary>
		/// <returns>The Object of the attached Calculator</returns>
#endif
		public Object GetObject()
		{
			return _Object;
		}

		internal CalculatorNode(NodeGraph nodeGraph, int nodeID, System.Type calculatorType) : base(nodeGraph, nodeID)
		{
			name = "Calculator";

			var calculator = Calculator.CreateCalculator(this, calculatorType);
			SetBehaviour(calculator);
		}

		void SetBehaviour(Calculator calculator)
		{
			_Object = calculator;
			calculator.OnAttachToNode();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Calculatorを作成する。エディタで使用する。
		/// </summary>
		/// <param name="calculatorType">Calculatorの型</param>
		/// <returns>生成した演算ノード。</returns>
#else
		/// <summary>
		/// Create a Calculator. Use it in the editor.
		/// </summary>
		/// <param name="calculatorType">Calculator type</param>
		/// <returns>The created calculator.</returns>
#endif
		public Calculator CreateCalculator(System.Type calculatorType)
		{
			var calculator = Calculator.CreateCalculator(this, calculatorType);
			SetBehaviour(calculator);
			return calculator;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Nodeが所属するNodeGraphが変わった際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when the NodeGraph to which the Node belongs has changed.
		/// </summary>
#endif
		protected override void OnGraphChanged()
		{
			Calculator sorceCalculator = _Object as Calculator;
			_Object = null;

			ComponentUtility.MoveBehaviour(this, sorceCalculator);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBehaviourを含んでいるかをチェックする。
		/// </summary>
		/// <param name="behaviour">チェックするNodeBehaviour</param>
		/// <returns>NodeBehaviourを含んでいる場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Check if it contains NodeBehaviour.
		/// </summary>
		/// <param name="behaviour">Check NodeBehaviour</param>
		/// <returns>Returns true if it contains NodeBehaviour.</returns>
#endif
		public override bool IsContainsBehaviour(NodeBehaviour behaviour)
		{
			Calculator calculator = behaviour as Calculator;
			return this.calculator == calculator;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Arbor3.9.0より前のノード名を取得する。
		/// </summary>
		/// <returns>旧ノード名</returns>
#else
		/// <summary>
		/// Get the node name before Arbor 3.9.0.
		/// </summary>
		/// <returns>Old node name</returns>
#endif
		protected override string GetOldName()
		{
			if (_Object is Calculator)
			{
				var type = _Object.GetType();
				string titleName = type.Name;

				bool useNicifyName = true;

				var behaviourTitle = AttributeHelper.GetAttribute<BehaviourTitle>(type);
				if (behaviourTitle != null)
				{
					titleName = behaviourTitle.titleName;
					useNicifyName = behaviourTitle.useNicifyName;
				}
				else
				{
#pragma warning disable 0618
					CalculatorTitle calculatorTitle = AttributeHelper.GetAttribute<CalculatorTitle>(type);
					if (calculatorTitle != null)
					{
						titleName = calculatorTitle.titleName;
					}
#pragma warning restore 0618
				}

				return useNicifyName ? ObjectNamesUtility.NicifyVariableName(titleName) : titleName;
			}
			else
			{
				return "Missing";
			}
		}

		int INodeBehaviourContainer.GetNodeBehaviourCount()
		{
			return 1;
		}

		T INodeBehaviourContainer.GetNodeBehaviour<T>(int index)
		{
			if (index == 0)
			{
				return _Object as T;
			}
			return null;
		}

		void INodeBehaviourContainer.SetNodeBehaviour(int index, NodeBehaviour behaviour)
		{
			if (index == 0)
			{
				SetBehaviour(behaviour as Calculator);
			}
		}
	}
}
