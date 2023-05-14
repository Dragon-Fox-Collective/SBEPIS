//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Animatorに関する名前を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// A class that handles names related to Animator.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class AnimatorName : ISerializationCallbackReceiver
	{
		[SerializeField]
		private string _Name;

#if ARBOR_DOC_JA
		/// <summary>
		/// 名前
		/// </summary>
#else
		/// <summary>
		/// Name
		/// </summary>
#endif
		public string name
		{
			get
			{
				return _Name;
			}
			set
			{
				if (_Name != value)
				{
					_Name = value;

					UpdateHash();
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Animator.StringToHash(name)を返す。nameがnullか空の場合はnullとなる。
		/// </summary>
#else
		/// <summary>
		/// Returns Animator.StringToHash (name). If name is null or empty, it will be null.
		/// </summary>
#endif
		public int? hash
		{
			get;
			private set;
		}

		void UpdateHash()
		{
			if (!string.IsNullOrEmpty(_Name))
			{
				hash = Animator.StringToHash(_Name);
			}
			else
			{
				hash = null;
			}
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			UpdateHash();
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}
	}
}