using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArborEditor
{
	[System.Serializable]
	internal class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
	{
		[SerializeField]
		private TKey[] _Keys;

		[SerializeField]
		private TValue[] _Values;

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			Clear();

			if (_Keys == null || _Values == null)
			{
				return;
			}

			if (_Keys.Length != _Values.Length)
			{
				return;
			}

			for (int i = 0; i < _Keys.Length; i++)
			{
				Add(_Keys[i], _Values[i]);
			}
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			int count = this.Count;

			_Keys = new TKey[count];
			_Values = new TValue[count];

			int index = 0;


			foreach (var pair in this)
			{
				_Keys[index] = pair.Key;
				_Values[index] = pair.Value;

				index++;
			}
		}
	}
}