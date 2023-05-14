//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	internal interface IParameterAccessor
	{
		bool TryGetValue(Parameter parameter, out object outValue);
		bool SetValue(Parameter parameter, object value);
	}
}