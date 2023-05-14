//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor.ValueFlow
{
	internal class ValueMediator<T> : ValueMediator where T : struct
	{
		internal ValueMediator() : base(typeof(T), new ListMediator<T>())
		{
		}

		public override IValueContainer CreateContainer()
		{
			return new ValueContainer<T>();
		}

		public override void ExportParameter(Parameter parameter, IValueSetter setter)
		{
			var value = parameter.GetValue<T>();

			IValueSetter<T> s = setter as IValueSetter<T>;
			if (s != null)
			{
				s.SetValue(value);
				return;
			}

			IOutputSlotAny slotAny = setter as IOutputSlotAny;
			if (slotAny != null)
			{
				slotAny.SetValue<T>(value);
				return;
			}

			setter.SetValueObject(value);
		}
	}
}