//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	internal interface IParameterAccessor<T>
	{
		bool TryGetValue(Parameter parameter, out T outValue);
		bool SetValue(Parameter parameter, T value);
	}
}