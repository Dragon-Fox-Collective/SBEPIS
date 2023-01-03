using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace SBEPIS.Bits
{
	public class BitSetField : BaseField<object>
	{
		private readonly TextField codeField;
		private readonly ListView bitList;

		public BitSet bits
		{
			get => value as BitSet;
			set => this.value = value;
		}

		public BitSetField() : this(null) { }
		public BitSetField(string label) : base(label, null)
		{
			if (bits is null)
				base.SetValueWithoutNotify(new BitSet());
			
			VisualElement container = new()
			{
				style =
				{
					width = new Length(100, LengthUnit.Percent)
				}
			};
			Add(container);

			codeField = new TextField();
			codeField.value = BitManager.instance.bits.BitSetToCode(bits);
			codeField.isDelayed = true;
			codeField.RegisterValueChangedCallback(e => bits = BitManager.instance.bits.BitSetFromCode(e.newValue));
			container.Add(codeField);

			List<List<Bit>> props = BitManager.instance.bits.Select((bit, i) => new { bit, i }).GroupBy(x => x.i / BitManager.instance.bits.numBitsInCharacterGeneral).Select(g => g.Select(x => x.bit).ToList()).ToList();

			Dictionary<Toggle, EventCallback<ChangeEvent<bool>>> callbacks = new();

			bitList = new ListView();
			bitList.itemsSource = props;
			bitList.fixedItemHeight = 16 * (BitManager.instance.bits.numBitsInCharacterGeneral + 0.5f);
			bitList.style.maxHeight = bitList.fixedItemHeight * 2.5f;
			bitList.selectionType = SelectionType.None;
			bitList.showAlternatingRowBackgrounds = AlternatingRowBackground.All;
			bitList.showBorder = true;
			bitList.makeItem = () => new ListView();
			bitList.bindItem = (e, i) =>
			{
				ListView list = e as ListView;
				list.itemsSource = props[i];
				list.fixedItemHeight = 16;
				list.style.paddingTop = 4;
				list.makeItem = () => new Toggle();
				list.bindItem = (e, j) =>
				{
					Toggle toggle = e as Toggle;
					toggle.text = props[i][j].ToString();
					toggle.SetValueWithoutNotify(BitManager.instance.bits.IsBitSetBitAt(bits, i, j));
					toggle.style.width = new Length(95, LengthUnit.Percent);
					if (callbacks.ContainsKey(toggle))
					{
						toggle.UnregisterValueChangedCallback(callbacks[toggle]);
						callbacks.Remove(toggle);
					}
					void Callback(ChangeEvent<bool> e)
					{
						BitSet bit = BitManager.instance.bits.BitSetFromBitAt(i, j);
						if (e.newValue)
							bits |= bit;
						else
							bits &= BitManager.instance.bits - bit;
					}
					toggle.RegisterValueChangedCallback(Callback);
					callbacks[toggle] = Callback;
				};
			};
			container.Add(bitList);
		}

		public override void SetValueWithoutNotify(object newValue)
		{
			BitSet newBits = newValue as BitSet ?? new BitSet();
			base.SetValueWithoutNotify(newBits);
			codeField.SetValueWithoutNotify(BitManager.instance.bits.BitSetToCode(newBits));
			bitList.RefreshItems();
		}
	}
}
