using System.Linq;
using SBEPIS.Bits.Tags;
using SBEPIS.Items;
using UnityEngine;

namespace SBEPIS.Bits.ThaumergeRules
{
	public abstract class ThaumergeRule
	{
		public virtual void Init() { }
		public abstract bool Apply(TaggedBitSet bits, ItemModule item, ItemModuleManager modules);
	}
	
	public abstract class DoOnceThaumaturgeRule : ThaumergeRule
	{
		private bool applied;

		public override void Init() => applied = false;

		public override bool Apply(TaggedBitSet bits, ItemModule item, ItemModuleManager modules)
		{
			if (applied)
				return false;
			applied = true;
			return ApplyOnce(bits, item, modules);
		}

		public abstract bool ApplyOnce(TaggedBitSet bits, ItemModule item, ItemModuleManager modules);
	}
	
	public class BaseModelReplaceThaumergeRule : DoOnceThaumaturgeRule
	{
		public override bool ApplyOnce(TaggedBitSet bits, ItemModule item, ItemModuleManager modules)
		{
			if (item.Bits.Tags.Any(member => member is BaseModelTag))
				return false;

			if (bits.Tags.FirstOrDefault(member => member is BaseModelTag) is not BaseModelTag tag)
				return false;

			ItemModule module = Object.Instantiate(tag.ItemModule);

			module.transform.Replace(item.replaceObject);
			item.replaceObject = module.replaceObject;

			item.Bits |= module.Bits.Bits;
			item.Bits += tag;

			return true;
		}
	}
	
	public class AeratedAttachThaumergeRule : DoOnceThaumaturgeRule
	{
		private readonly Bit aerated;
		
		public AeratedAttachThaumergeRule()
		{
			aerated = BitManager.Instance.Bits.First(bit => bit.BitName == "Aerated");
		}
		
		public override bool ApplyOnce(TaggedBitSet bits, ItemModule item, ItemModuleManager modules)
		{
			if (!bits.Has(aerated) || !item.aeratedAttachmentPoint)
				return false;
			
			return true;
		}
	}
	
	public class DefaultReplaceThaumergeRule : ThaumergeRule
	{
		public override bool Apply(TaggedBitSet bits, ItemModule item, ItemModuleManager modules)
		{
			if (bits.Bits == item.Bits.Bits)
				return false;
			
			ItemModule modulePrefab = GetModulePrefabFromScore(bits, item, modules);
			
			if (modulePrefab is null)
				return false;
			
			ItemModule module = Object.Instantiate(modulePrefab);
			PlaceModuleUnderItem(item, module);
			
			return true;
		}
		
		private static ItemModule GetModulePrefabFromScore(TaggedBitSet bits, ItemModule item, ItemModuleManager modules)
		{
			ItemModule module = null;
			int moduleScore = int.MinValue;
			foreach (ItemModule newModule in modules.modules)
			{
				if (bits.Has(newModule.Bits.Bits) && !item.Bits.Has(newModule.Bits.Bits))
				{
					int newModuleScore = BitSet.GetUniquenessScore(item.Bits.Bits, newModule.Bits.Bits);
					if (newModuleScore > moduleScore)
					{
						module = newModule;
						moduleScore = newModuleScore;
					}
				}
			}
			return module;
		}
		
		private static void PlaceModuleUnderItem(ItemModule item, ItemModule module)
		{
			module.transform.Replace(item.replaceObject);
			item.replaceObject = module.replaceObject;
			item.Bits |= module.Bits.Bits;
		}
	}
	
	public class MaterialThaumergeRule : DoOnceThaumaturgeRule
	{
		public override bool ApplyOnce(TaggedBitSet bits, ItemModule item, ItemModuleManager modules)
		{
			if (item.Bits.Tags.Any(member => member is MaterialTag))
				return false;
			
			if (bits.Tags.FirstOrDefault(member => member is MaterialTag) is not MaterialTag tag)
				return false;
			
			foreach (Renderer renderer in item.GetComponentsInChildren<Renderer>())
				renderer.materials = new Material[renderer.materials.Length].Fill(tag.Material);
			
			item.Bits += tag;
			
			return true;
		}
	}
}
