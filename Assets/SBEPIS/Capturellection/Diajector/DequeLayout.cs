using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public interface DequeLayout
	{
		public void Tick(IList<Storable> inventory, object state, float deltaTime);
		public void Layout(IList<Storable> inventory, object state);
		public Vector3 GetMaxPossibleSizeOf(IList<Storable> inventory, object state);
		
		protected static Vector3 GetSizeFromExistingLayout(IEnumerable<Storable> inventory) =>
			inventory.Select(storable => new Bounds(storable.Position, storable.MaxPossibleSize)).Aggregate(new Bounds(), (current, bounds) => current.Containing(bounds)).size;
		
		protected static Quaternion GetOffsetRotation(Vector3 direction) => Quaternion.AngleAxis(-5f, Vector3.Cross(direction, Vector3.forward));
	}
	
	public abstract class DequeLayoutBase : DequeLayout
	{
		public void Tick(IList<Storable> inventory, object state, float deltaTime)
		{
			if (!PerformMagic((Action<IList<Storable>, object, float>)Tick, state, new[] { inventory, state, deltaTime }, out object _))
				inventory.ForEach(storable => storable.Tick(deltaTime));
		}
		
		public void Layout(IList<Storable> inventory, object state)
		{
			if (!PerformMagic((Action<IList<Storable>, object>)Layout, state, new[] { inventory, state }, out object _))
				throw new NotImplementedException();
		}
		
		public Vector3 GetMaxPossibleSizeOf(IList<Storable> inventory, object state) =>
			PerformMagic((Func<IList<Storable>, object, Vector3>)GetMaxPossibleSizeOf, state, new[] { inventory, state }, out Vector3 size)
				? size
				: DequeLayout.GetSizeFromExistingLayout(inventory);
		
		private bool PerformMagic<T>(Delegate baseMethod, object state, object[] parameters, out T methodReturn)
		{
			MethodInfo method = GetType().GetMethods().FirstOrDefault(method => method.Name == baseMethod.Method.Name && method.IsGenericMethod);
			if (method != null)
			{
				methodReturn = (T)method.MakeGenericMethod(state.GetType()).Invoke(this, parameters);
				return true;
			}
			else
			{
				methodReturn = default;
				return false;
			}
		}
	}
}
