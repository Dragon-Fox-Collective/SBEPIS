using System;
using NUnit.Framework;
using SBEPIS.AI;
using UnityEngine;

namespace SBEPIS.Tests.EditMode
{
	public class AITests
	{
		private static readonly Point Start = new("Start", new AIPointState{ new PositionState{ position = new Vector2(0, 0) } });
		private static readonly Point End =  new("End", new AIPointState{ new PositionState{ position = new Vector2(0, 10) } });
		private static readonly Point BusStart = new("Bus Start", new AIPointState{ new PositionState{ position = new Vector2(1, 1) } });
		private static readonly Point BusEnd = new("Bus End", new AIPointState{ new PositionState{ position = new Vector2(1, 9) } });
		private static readonly Point Shop = new("Candy Shop", new AIPointState{ new PositionState{ position = new Vector2(-1, 5) } });
		
		static AITests()
		{
			ConnectBiWayByDistance(Start, End);
			ConnectBiWayByDistance(Start, BusStart);
			ConnectBiWayByDistance(Start, BusEnd);
			ConnectBiWayByDistance(Start, Shop);
			
			ConnectBiWayByDistance(BusStart, End);
			ConnectBiWay(BusStart, BusEnd, -4);
			ConnectBiWayByDistance(BusStart, Shop);
			
			ConnectBiWayByDistance(BusEnd, End);
			ConnectBiWayByDistance(BusEnd, Shop);
			
			ConnectBiWayByDistance(Shop, End);
			Shop.Connect<CashState>(Shop, state => state.Spend(), state => state.cash + 1);
		}
		
		[Test]
		public void AISolver_UsesBus_WhenItHasNoMoneyOrEnergy()
		{
			Assert.That(AISolver.Solve(Start, End, new AIState{ new CashState{ cash = 0 }, new EnergyState{ energy = 0 } }, out Point[] path));
			Assert.That(path, Is.EqualTo(new[]{ Start, BusStart, BusEnd, End }));
		}
		
		[Test]
		public void AISolver_GoesToCandyShop_WhenItHasMoney()
		{
			Assert.That(AISolver.Solve(Start, End, new AIState{ new CashState{ cash = 3 }, new EnergyState{ energy = 0 } }, out Point[] path));
			Assert.That(path, Is.EqualTo(new[]{ Start, Shop, Shop, Shop, Shop, End }));
		}
		
		[Test]
		public void AISolver_TeleportsToEnd_WhenItHasEnergy()
		{
			Assert.That(AISolver.Solve(Start, End, new AIState{ new CashState{ cash = 0 }, new EnergyState{ energy = 1 } }, out Point[] path));
			Assert.That(path, Is.EqualTo(new[]{ Start, End }));
		}
		
		private static void ConnectBiWayByDistance(Point a, Point b)
		{
			ConnectBiWay(a, b, -Vector2.Distance(a.state.Get<PositionState>().position, b.state.Get<PositionState>().position)); // 1 v/m
			ConnectBiWay<EnergyState>(a, b, state => state.Spend(), _ => 0);
		}
		
		private static void ConnectBiWay<TStateComponent>(Point a, Point b, Func<TStateComponent, TStateComponent> stateModifier, Func<TStateComponent, float> valueCalculator) where TStateComponent : struct, AIStateComponent
		{
			a.Connect(b, stateModifier, valueCalculator);
			b.Connect(a, stateModifier, valueCalculator);
		}
		
		private static void ConnectBiWay(Point a, Point b, float value)
		{
			a.Connect(b, value);
			b.Connect(a, value);
		}
		
		private struct PositionState : AIPointComponent
		{
			public Vector2 position;
			
			public override string ToString() => $"{position}";
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
		
		private struct EnergyState : AIStateComponent
		{
			public int energy;
			
			public EnergyState Spend()
			{
				EnergyState state = this;
				state.energy--;
				return state;
			}
			
			public float GetValue() => energy >= 0 ? 0 : Mathf.NegativeInfinity;
			public AIStateComponent StepState() => this;
			
			public override string ToString() => $"{energy} energy left";
		}
	}
}