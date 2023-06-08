using NUnit.Framework;
using SBEPIS.Bits;
using System;
using UnityEngine;

namespace SBEPIS.Tests.EditMode
{
	public class CallbackListTests
	{
		[Test]
		public void CallbackListAdd_CallsAddCallback()
		{
			CallbackList<int> list = new();
			int addedItem = 0;
			list.onAddItem.AddListener(item => addedItem = item);
			list.Add(1);
			Assert.That(addedItem, Is.EqualTo(1));
		}
		
		[Test]
		public void CallbackListRemove_CallsRemoveCallback()
		{
			CallbackList<int> list = new();
			list.Add(1);
			int removedItem = 0;
			list.onRemoveItem.AddListener(item => removedItem = item);
			list.Remove(1);
			Assert.That(removedItem, Is.EqualTo(1));
		}
		
		[Test]
		public void CallbackListIndex_CallsAddAndRemoveCallbacks()
		{
			CallbackList<int> list = new();
			list.Add(1);
			int addedItem = 0;
			list.onAddItem.AddListener(item => addedItem = item);
			int removedItem = 0;
			list.onRemoveItem.AddListener(item => removedItem = item);
			list[0] = 2;
			Assert.That(addedItem, Is.EqualTo(2));
			Assert.That(removedItem, Is.EqualTo(1));
		}
	}
}
