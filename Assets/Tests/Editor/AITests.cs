using NUnit.Framework;
using SBEPIS.AI;
using UnityEngine;

namespace SBEPIS.Tests.EditMode
{
	public class AITests
	{
		[Test]
		public void BusTest()
		{
			Point start = new("Start", 0, 0);
			Point end =  new("End", 0, 10);
			Point busStart = new("Bus Start", 1, 1);
			Point busEnd = new("Bus End", 1, 9);
			
			start.Connect(end);
			start.Connect(busStart);
			start.Connect(busEnd);
			
			busStart.Connect(start);
			busStart.Connect(end);
			busStart.Connect(busEnd, -4);
			
			busEnd.Connect(start);
			busEnd.Connect(end);
			busEnd.Connect(busStart, -4);
			
			Assert.That(AISolver.Solve(start, end, new AIState(), out Point[] path));
			Assert.That(path, Is.EqualTo(new[]{ start, busStart, busEnd, end }));
		}
		
		[Test]
		public void CandyShopAndBusTest()
		{
			Point start = new("Start", 0, 0);
			Point end =  new("End", 0, 10);
			Point busStart = new("Bus Start", 1, 1);
			Point busEnd = new("Bus End", 1, 9);
			Point shop = new("Candy Shop", -1, 5);
			
			start.Connect(end);
			start.Connect(busStart);
			start.Connect(busEnd);
			start.Connect(shop);
			
			busStart.Connect(start);
			busStart.Connect(end);
			busStart.Connect(busEnd, -4);
			busStart.Connect(shop);
			
			busEnd.Connect(start);
			busEnd.Connect(end);
			busEnd.Connect(busStart, -4);
			busEnd.Connect(shop);
			
			shop.Connect(start);
			shop.Connect(end);
			shop.Connect(busStart);
			shop.Connect(busEnd);
			shop.Connect(shop, state => state.cash > 0 ? state.cash : Mathf.NegativeInfinity, state =>
			{
				state.cash--;
				return state;
			});
			
			Assert.That(AISolver.Solve(start, end, new AIState{ cash = 3 }, out Point[] path));
			Assert.That(path, Is.EqualTo(new[]{ start, shop, shop, shop, shop, end }));
		}
	}
}