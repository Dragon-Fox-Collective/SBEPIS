using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public interface DequeLayout
	{
		public void Tick(List<Storable> inventory, object state, float deltaTime);
		public Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, object state);
		
		protected static Vector3 GetSizeFromExistingLayout(IEnumerable<Storable> inventory) =>
			inventory.Select(storable => new Bounds(storable.Position, storable.MaxPossibleSize)).Aggregate(new Bounds(), (current, bounds) => current.Containing(bounds)).size;
		
		protected static Quaternion GetOffsetRotation(Vector3 direction) => Quaternion.AngleAxis(-5f, Vector3.Cross(direction, Vector3.forward));
	}

	public abstract class DequeLayoutBase : DequeLayout
	{
		public void Tick(List<Storable> inventory, object state, float deltaTime)
		{
			MethodInfo method = GetType().GetMethods().FirstOrDefault(method => method.Name == "Tick" && method.IsGenericMethod);
			if (method != null)
				method.MakeGenericMethod(state.GetType()).Invoke(this, new[] { inventory, state, deltaTime });
			else
				throw new NotImplementedException();
		}
		
		public Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, object state)
		{
			MethodInfo method = GetType().GetMethods().FirstOrDefault(method => method.Name == "GetMaxPossibleSizeOf" && method.IsGenericMethod);
			if (method != null)
				return (Vector3)method.MakeGenericMethod(state.GetType()).Invoke(this, new[] { inventory, state });
			else
				return DequeLayout.GetSizeFromExistingLayout(inventory);
		}
	}
}
