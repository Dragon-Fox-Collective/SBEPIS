using NUnit.Framework;
using SBEPIS.Utils;

namespace SBEPIS.Tests.EditMode
{
	public class TreeTests
	{
		[Test]
		public void TreeLeftLeftBalancingWorks()
		{
			Tree<string> tree = new();
			tree.Add("book", true);
			tree.Add("laptop", true);
			tree.Add("violin", true);
			CollectionAssert.AreEqual(new[]{ "book", "laptop", "violin" }, tree);
		}
		
		[Test]
		public void TreeRightRightBalancingWorks()
		{
			Tree<string> tree = new();
			tree.Add("violin", true);
			tree.Add("laptop", true);
			tree.Add("book", true);
			CollectionAssert.AreEqual(new[]{ "book", "laptop", "violin" }, tree);
		}
		
		[Test]
		public void TreeLeftRightBalancingWorks()
		{
			Tree<string> tree = new();
			tree.Add("violin", true);
			tree.Add("w magnet", true);
			tree.Add("pillow", true);
			tree.Add("umbrella", true);
			tree.Add("laptop", true);
			tree.Add("book", true);
			CollectionAssert.AreEqual(new[]{ "book", "laptop", "pillow", "umbrella", "violin", "w magnet" }, tree);
		}
		
		[Test]
		public void TreeRightLeftBalancingWorks()
		{
			Tree<string> tree = new();
			tree.Add("laptop", true);
			tree.Add("book", true);
			tree.Add("violin", true);
			tree.Add("umbrella", true);
			tree.Add("w magnet", true);
			tree.Add("pillow", true);
			CollectionAssert.AreEqual(new[]{ "book", "laptop", "pillow", "umbrella", "violin", "w magnet" }, tree);
		}
	}
}
