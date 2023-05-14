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
	/// Rayをフォーマットした文字列を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns a formatted string of Ray.
	/// </summary>
#endif
	[AddBehaviourMenu("Ray/Ray.ToString")]
	[BehaviourTitle("Ray.ToString")]
	[BuiltInBehaviour]
	[AddComponentMenu("")]
	public sealed class RayToStringCalculator : Calculator
	{
		/// <summary>
		/// Ray
		/// </summary>
		[SerializeField]
		private InputSlotRay _Ray = new InputSlotRay();

#if ARBOR_DOC_JA
		/// <summary>
		/// フォーマット<br/>
		/// フォーマットの詳細については、次を参照してください。<a href="https://docs.microsoft.com/ja-jp/dotnet/standard/base-types/enumeration-format-strings" target="_blank">列挙型形式文字列</a>
		/// </summary>
#else
		/// <summary>
		/// Format<br/>
		/// For more information about numeric format specifiers, see <a href="https://docs.microsoft.com/en-us/dotnet/standard/base-types/enumeration-format-strings" target="_blank">Enumeration format strings</a>.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _Format = new FlexibleString();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotString _Result = new OutputSlotString();

		// Use this for calculate
		public override void OnCalculate()
		{
			Ray ray = default;
			if (_Ray.GetValue(ref ray))
			{
				string format = _Format.value;
				string str = !string.IsNullOrEmpty(format) ? ray.ToString(format) : ray.ToString();
				_Result.SetValue(str);
			}
		}
	}
}