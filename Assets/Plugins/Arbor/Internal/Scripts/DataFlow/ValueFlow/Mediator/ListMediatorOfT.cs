//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor.ValueFlow
{
	internal sealed class ListMediator<T> : ListMediator
	{
		internal ListMediator() : base(typeof(T))
		{
		}

		public override void GetElement(IList instance, int index, IValueSetter container)
		{
			IList<T> list = instance as IList<T>;

			if (list != null)
			{
				var value = list[index];

				IValueSetter<T> c = container as IValueSetter<T>;
				if (c != null)
				{
					c.SetValue(value);
					return;
				}

				IOutputSlotAny slotAny = container as IOutputSlotAny;
				if (slotAny != null)
				{
					slotAny.SetValue(value);
					return;
				}

				container.SetValueObject(value);
			}
			else
			{
				base.GetElement(instance, index, container);
			}
		}

		public override string GetElementString(IList instance, int index)
		{
			IList<T> list = instance as IList<T>;

			if (list != null)
			{
				return list[index].ToString();
			}
			else
			{
				return base.GetElementString(instance, index);
			}
		}

		public override bool Contains(IList instance, IValueGetter container)
		{
			IList<T> list = instance as IList<T>;

			if (list != null && container.TryGetValue(out T value))
			{
				return list.Contains(value);
			}

			return base.Contains(instance, container);
		}

		public override int IndexOf(IList instance, IValueGetter container)
		{
			IList<T> list = instance as IList<T>;

			if (list != null && container.TryGetValue(out T value))
			{
				return list.IndexOf(value);
			}

			return base.IndexOf(instance, container);
		}

		public override int LastIndexOf(IList instance, IValueGetter container)
		{
			IList<T> list = instance as IList<T>;

			if (list != null && container.TryGetValue(out T value))
			{
				return ListUtility.LastIndexOf(list, value);
			}

			return base.LastIndexOf(instance, container);
		}

		public override IList SetElement(IList instance, int index, IValueGetter container, ListInstanceType instanceType)
		{
			IList<T> list = instance as IList<T>;

			if (list != null && container.TryGetValue(out T value))
			{
				return ListUtility.SetElement(list, index, value, instanceType) as IList;
			}

			return base.SetElement(instance, index, container, instanceType);
		}

		public override IList AddElement(IList instance, IValueGetter container, ListInstanceType instanceType)
		{
			IList<T> list = instance as IList<T>;

			if (list != null && container.TryGetValue(out T value))
			{
				return ListUtility.AddElement(list, value, instanceType) as IList;
			}

			return base.AddElement(instance, container, instanceType);
		}

		public override IList InsertElement(IList instance, int index, IValueGetter container, ListInstanceType instanceType)
		{
			IList<T> list = instance as IList<T>;

			if (list != null && container.TryGetValue(out T value))
			{
				return ListUtility.InsertElement(list, index, value, instanceType) as IList;
			}
			
			return base.InsertElement(instance, index, container, instanceType);
		}

		public override IList RemoveElement(IList instance, IValueGetter container, ListInstanceType instanceType)
		{
			IList<T> list = instance as IList<T>;

			if (list != null && container.TryGetValue(out T value))
			{
				return ListUtility.RemoveElement(list, value, instanceType) as IList;
			}

			return base.RemoveElement(instance, container, instanceType);
		}

		public override IList NewArray(int count)
		{
			return new T[count];
		}

		public override IList NewList()
		{
			return new List<T>();
		}

		public override IList ToList(IList instance)
		{
			IList<T> list = instance as IList<T>;
			if (list != null)
			{
				return ListUtility.ToList(list);
			}
			else
			{
				return base.ToList(instance);
			}
		}

		public override IList ToArray(IList instance)
		{
			IList<T> list = instance as IList<T>;
			if (list != null)
			{
				return ListUtility.ToArray(list);
			}
			else
			{
				return base.ToArray(instance);
			}
		}

		public override IList RemoveAtIndex(IList instance, int index, ListInstanceType instanceType)
		{
			IList<T> list = instance as IList<T>;
			if (list != null)
			{
				return ListUtility.RemoveAt(list, index, instanceType) as IList;
			}
			else
			{
				return base.RemoveAtIndex(instance, index, instanceType);
			}
		}

		public override IList Clear(IList instance, ListInstanceType instanceType)
		{
			IList<T> list = instance as IList<T>;
			if (list != null)
			{
				return ListUtility.Clear(list, instanceType) as IList;
			}
			else
			{
				return base.Clear(instance, instanceType);
			}
		}

		public override bool EqualsList(IList a, IList b)
		{
			IList<T> listA = a as IList<T>;
			IList<T> listB = b as IList<T>;
			if (listA != null && listB != null)
			{
				return ListUtility.Equals(listA, listB);
			}
			else
			{
				return base.EqualsList(a, b);
			}
		}

		public override void AddRange(IList instance, IList addToList)
		{
			IList<T> list = instance as IList<T>;
			IList<T> listAddToList = addToList as IList<T>;

			if (list != null && listAddToList != null)
			{
				ListUtility.AddRange(list, listAddToList);
			}
			else
			{
				base.AddRange(instance, addToList);
			}
		}
	}
}