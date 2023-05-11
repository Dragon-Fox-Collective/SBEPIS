//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	[System.Serializable]
	public sealed class GraphArgument
	{
		[SerializeField]
		private int _ParameterID = 0;

		[SerializeField]
		private string _ParameterName = "";

		[SerializeField]
		private Parameter.Type _ParameterType = Parameter.Type.Int;

		[SerializeField]
		private ClassTypeReference _Type = new ClassTypeReference();

		[SerializeField]
		private int _ParameterIndex = 0;

		[SerializeField]
		private int _OutputSlotIndex = 0;

		[SerializeField]
		private GraphArgumentUpdateTiming _UpdateTiming = GraphArgumentUpdateTiming.Enter;

		[SerializeField]
		private bool _IsPublicSet = true;

		[SerializeField]
		private bool _IsPublicGet = true;

		public int parameterID
		{
			get
			{
				return _ParameterID;
			}
		}

		public string parameterName
		{
			get
			{
				return _ParameterName;
			}
		}

		public Parameter.Type parameterType
		{
			get
			{
				return _ParameterType;
			}
		}

		public System.Type type
		{
			get
			{
				return _Type.type;
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

		public GraphArgumentUpdateTiming updateTiming
		{
			get
			{
				return _UpdateTiming;
			}
		}

		public bool isPublicSet
		{
			get
			{
				return _IsPublicSet;
			}
		}

		public bool isPublicGet
		{
			get
			{
				return _IsPublicGet;
			}
		}
	}
}