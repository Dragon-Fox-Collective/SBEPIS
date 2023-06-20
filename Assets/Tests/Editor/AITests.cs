using NUnit.Framework;
using SBEPIS.AI;
using UnityEngine;

namespace SBEPIS.Tests.EditMode
{
	public class AITests
	{
		private static readonly Point Start = new("Start", 0, 0);
		private static readonly Point End =  new("End", 0, 10);
		private static readonly Point BusStart = new("Bus Start", 1, 1);
		private static readonly Point BusEnd = new("Bus End", 1, 9);
		private static readonly Point Shop = new("Candy Shop", -1, 5);
		
		static AITests()
		{
			Start.Connect(End);
			Start.Connect(BusStart);
			Start.Connect(BusEnd);
			Start.Connect(Shop);
			
			BusStart.Connect(Start);
			BusStart.Connect(End);
			BusStart.Connect(BusEnd, -4);
			BusStart.Connect(Shop);
			
			BusEnd.Connect(Start);
			BusEnd.Connect(End);
			BusEnd.Connect(BusStart, -4);
			BusEnd.Connect(Shop);
			
			Shop.Connect(Start);
			Shop.Connect(End);
			Shop.Connect(BusStart);
			Shop.Connect(BusEnd);
			Shop.Connect(Shop, state => state.cash > 0 ? state.cash : Mathf.NegativeInfinity, state =>
			{
				state.cash--;
				return state;
			});
		}
		
		[Test]
		public void AISolver_UsesBus_WhenItHasNoMoney()
		{
			Assert.That(AISolver.Solve(Start, End, new AIState{ cash = 0 }, out Point[] path));
			Assert.That(path, Is.EqualTo(new[]{ Start, BusStart, BusEnd, End }));
		}
		
		[Test]
		public void AISolver_GoesToCandyShop_WhenItHasMoney()
		{
			Assert.That(AISolver.Solve(Start, End, new AIState{ cash = 3 }, out Point[] path));
			Assert.That(path, Is.EqualTo(new[]{ Start, Shop, Shop, Shop, Shop, End }));
		}
	}
}