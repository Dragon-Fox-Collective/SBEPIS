using NUnit.Framework;

namespace SBEPIS.Tests.EditMode
{
	public class CaptureCodeTestSuite
	{
		[Test]
		public void CaptureCodeHashes()
		{
			Assert.AreEqual(0b110110110110110110111110111111011100011100110110, CaptureCodeUtils.HashCaptureCode("sSS!?sss"));
		}

		[Test]
		public void CaptureHashUnhashes()
		{
			Assert.AreEqual("sSS!?sss", CaptureCodeUtils.UnhashCaptcha(0b110110110110110110111110111111011100011100110110));
		}

		[Test]
		public void CaptureHashDigitizes()
		{
			Assert.AreEqual(63, CaptureCodeUtils.GetCaptchaDigit(0b110110110110110110111110111111011100011100110110, 3));
		}

		[Test]
		public void CaptureHashPercentizes()
		{
			Assert.AreEqual(1, CaptureCodeUtils.GetCaptchaPercent(0b110110110110110110111110111111011100011100110110, 3));
		}

		[Test]
		public void CaptureHashCharacterizes()
		{
			Assert.AreEqual('!', CaptureCodeUtils.GetCaptchaChar(0b110110110110110110111110111111011100011100110110, 3));
		}

		[Test]
		public void CaptureHashBitizes()
		{
			Assert.AreEqual(false, CaptureCodeUtils.GetCaptchaBit(0b110110110110110110111110111111011100011100110110, 3));
		}
	}
}
