//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Events
{
	using Arbor.ValueFlow;

	[System.Serializable]
	internal sealed class Argument
	{
		[SerializeField]
		private ClassTypeReference _Type = new ClassTypeReference();

		[SerializeField]
		private string _Name = "";

		[SerializeField]
		private ArgumentAttributes _Attributes = ArgumentAttributes.None;

		[SerializeField]
		private ParameterType _ParameterType = ParameterType.Unknown;

		[SerializeField]
		private int _ParameterIndex = 0;

		[SerializeField]
		private int _OutputSlotIndex = 0;

		public System.Type type
		{
			get
			{
				return _Type.type;
			}
		}

		public ParameterType parameterType
		{
			get
			{
				return _ParameterType;
			}
		}

		public string name
		{
			get
			{
				return _Name;
			}
			internal set
			{
				_Name = value;
			}
		}

		public ArgumentAttributes attributes
		{
			get
			{
				return _Attributes;
			}
		}

		public bool isOut
		{
			get
			{
				return (_Attributes & ArgumentAttributes.Out) == ArgumentAttributes.Out;
			}
		}

		public int parameterIndex
		{
			get
			{
				return _ParameterIndex;
			}
		}

		public int outputSlotIndex
		{
			get
			{
				return _OutputSlotIndex;
			}
		}

		internal IValueGetter valueContainer
		{
			get;
			set;
		}

		internal OutputSlotTypable outputSlot
		{
			get;
			set;
		}
	}
}