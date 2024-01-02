using System;
using KBCore.Refs;
using SBEPIS.Bits;
using SBEPIS.Items;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Thaumergy
{
	public class BootlegAlchemyStation : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere]
		private Transform alchemizePoint;
		
		public UnityEvent OnResultChange = new();
		
		private Item bits1;
		public Item Bits1
		{
			get => bits1;
			set
			{
				bits1 = value;
				UpdateResult();
			}
		}
		private Item bits2;
		public Item Bits2
		{
			get => bits2;
			set
			{
				bits2 = value;
				UpdateResult();
			}
		}
		private Item tags1;
		public Item Tags1
		{
			get => tags1;
			set
			{
				tags1 = value;
				UpdateResult();
			}
		}
		private Item tags2;
		public Item Tags2
		{
			get => tags2;
			set
			{
				tags2 = value;
				UpdateResult();
			}
		}

		private AlchemyMode mode = AlchemyMode.Or;
		public AlchemyMode Mode
		{
			get => mode;
			set
			{
				mode = value;
				UpdateResult();
			}
		}

		public TaggedBitSet Result { get; private set; }

		private void Start()
		{
			UpdateResult();
		}

		private void UpdateResult()
		{
			BitSet code = Bits1 && Bits2 ? Mode switch
				{
					AlchemyMode.And => Bits1.Module.Bits.Bits & Bits2.Module.Bits.Bits,
					AlchemyMode.Or => Bits1.Module.Bits.Bits | Bits2.Module.Bits.Bits,
					_ => throw new NotImplementedException()
				}
				: Bits1 ? Bits1.Module.Bits.Bits
				: Bits2 ? Bits2.Module.Bits.Bits
				: BitSet.Empty;
			
			Result = Tags1 && Tags2 ? TagAppender.Instance.Append(Tags1.Module.Bits, Tags2.Module.Bits, code)
				: Tags1 ? TagAppender.Instance.Append(Tags1.Module.Bits, code)
				: Tags2 ? TagAppender.Instance.Append(Tags2.Module.Bits, code)
				: TagAppender.Instance.Append(code);
			
			OnResultChange.Invoke();
		}

		public void Alchemize()
		{
			Item item = Thaumerger.Instance.Thaumerge(Result, ItemModuleManager.Instance);
			item.transform.SetPositionAndRotation(alchemizePoint.position, alchemizePoint.rotation);
			if (item.TryGetComponent(out Rigidbody itemRigidbody))
			{
				itemRigidbody.position = alchemizePoint.position;
				itemRigidbody.rotation = alchemizePoint.rotation;
			}
		}

		public void ToggleMode()
		{
			Mode = Mode switch {
				AlchemyMode.And => AlchemyMode.Or,
				AlchemyMode.Or => AlchemyMode.And,
				_ => throw new NotImplementedException()
			};
		}
	}
	
	public enum AlchemyMode
	{
		And, Or
	}
}
