using System.Linq;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	[CreateAssetMenu]
	public class HashFunction : ScriptableObject
	{
		public int a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z;
		
		public int Hash(string str, int inventoryCount) => Hash(str) % inventoryCount;
		
		private int Hash(string str) => str.Where(char.IsLetter).Select(char.ToLower).Select(Hash).Sum();
		
		private int Hash(char ch)
		{
			return ch switch
			{
				'a' => a,
				'b' => b,
				'c' => c,
				'd' => d,
				'e' => e,
				'f' => f,
				'g' => g,
				'h' => h,
				'i' => i,
				'j' => j,
				'k' => k,
				'l' => l,
				'm' => m,
				'n' => n,
				'o' => o,
				'p' => p,
				'q' => q,
				'r' => r,
				's' => s,
				't' => t,
				'u' => u,
				'v' => v,
				'w' => w,
				'x' => x,
				'y' => y,
				'z' => z,
				_ => 0
			};
		}
	}
}
