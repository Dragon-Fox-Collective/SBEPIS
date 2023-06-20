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
			Start.ConnectDistance(End);
			Start.ConnectDistance(BusStart);
			Start.ConnectDistance(BusEnd);
			Start.ConnectDistance(Shop);
			
			BusStart.ConnectDistance(Start);
			BusStart.ConnectDistance(End);
			BusStart.Connect(BusEnd, -4);
			BusStart.ConnectDistance(Shop);
			
			BusEnd.ConnectDistance(Start);
			BusEnd.ConnectDistance(End);
			BusEnd.Connect(BusStart, -4);
			BusEnd.ConnectDistance(Shop);
			
			Shop.ConnectDistance(Start);
			Shop.ConnectDistance(End);
			Shop.ConnectDistance(BusStart);
			Shop.ConnectDistance(BusEnd);
			Shop.Connect<CashState>(Shop, state => state.Spend());
		}
		
		[Test]
		public void AISolver_UsesBus_WhenItHasNoMoney()
		{
			Assert.That(AISolver.Solve(Start, End, new AIState{ new CashState{ cash = 3 } }, out Point[] path));
			Assert.That(path, Is.EqualTo(new[]{ Start, BusStart, BusEnd, End }));
		}
		
		[Test]
		public void AISolver_GoesToCandyShop_WhenItHasMoney()
		{
			Assert.That(AISolver.Solve(Start, End, new AIState{ new CashState{ cash = 3 } }, out Point[] path));
			Assert.That(path, Is.EqualTo(new[]{ Start, Shop, Shop, Shop, Shop, End }));
		}
		
		private struct CashState : AIStateComponent
		{
			public int cash;
			
			public CashState Spend()
			{
				CashState state = this;
				state.cash--;
				return state;
			}
			
			public float GetValue() => cash >= 0 ? 0 : Mathf.NegativeInfinity;
			public AIStateComponent StepState() => this;
			
			public override string ToString() => $"${cash} left";
		}
	}
}