using NUnit.Framework;

namespace SBEPIS.Tests.EditMode
{
	public class CaptureCodeTests
	{
		private static string code = "sSS/+sss";
		private static long hash = 0b101100101100101100111110111111010010010010101100;

		[Test]
		public void CaptureCodeHashes()
		{
			Assert.AreEqual(hash, CaptureCodeUtils.HashCaptureCode(code));
		}

		[Test]
		public void CaptureHashUnhashes()
		{
			Assert.AreEqual(code, CaptureCodeUtils.UnhashCaptureHash(hash));
		}

		[Test]
		public void CaptureHashDigitizes()
		{
			Assert.AreEqual(63, CaptureCodeUtils.GetCaptureDigit(hash, 3));
		}

		[Test]
		public void CaptureHashPercentizes()
		{
			Assert.AreEqual(1, CaptureCodeUtils.GetCapturePercent(hash, 3));
		}

		[Test]
		public void CaptureHashCharacterizes()
		{
			Assert.AreEqual('/', CaptureCodeUtils.GetCaptureChar(hash, 3));
		}

		[Test]
		public void CaptureHashBitizes()
		{
			Assert.AreEqual(true, CaptureCodeUtils.GetCaptureBit(hash, 3));
		}
	}
}
