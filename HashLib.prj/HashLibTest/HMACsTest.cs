﻿using System;
using System.Collections.Generic;

using HashLib;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashLibTest
{
	[TestClass]
	public class HMACsTest
	{
		private readonly MersenneTwister m_random = new MersenneTwister(4563487);

		[TestMethod]
		public void HashLib_HMAC_All()
		{
			Test(HashFactory.HMAC.CreateHMAC(HashFactory.Crypto.BuildIn.CreateMD5CryptoServiceProvider()),
			     HashFactory.HMAC.CreateHMAC(HashFactory.Crypto.CreateMD5()));
			Test(HashFactory.HMAC.CreateHMAC(HashFactory.Crypto.BuildIn.CreateRIPEMD160Managed()),
			     HashFactory.HMAC.CreateHMAC(HashFactory.Crypto.CreateRIPEMD160()));
			Test(HashFactory.HMAC.CreateHMAC(HashFactory.Crypto.BuildIn.CreateSHA1CryptoServiceProvider()),
			     HashFactory.HMAC.CreateHMAC(HashFactory.Crypto.CreateSHA1()));
			Test(HashFactory.HMAC.CreateHMAC(HashFactory.Crypto.BuildIn.CreateSHA256Managed()),
			     HashFactory.HMAC.CreateHMAC(HashFactory.Crypto.CreateSHA256()));
			Test(HashFactory.HMAC.CreateHMAC(HashFactory.Crypto.BuildIn.CreateSHA384Managed()),
			     HashFactory.HMAC.CreateHMAC(HashFactory.Crypto.CreateSHA384()));
			Test(HashFactory.HMAC.CreateHMAC(HashFactory.Crypto.BuildIn.CreateSHA512Managed()),
			     HashFactory.HMAC.CreateHMAC(HashFactory.Crypto.CreateSHA512()));
		}

		private void Test(IHMAC a_base_hmac, IHMAC a_hmac)
		{
			Assert.AreEqual(a_base_hmac.HashSize, a_hmac.HashSize);
			Assert.AreEqual(a_base_hmac.BlockSize, a_hmac.BlockSize);

			var keys_length = new List<int> {0, 1};
			keys_length.Add(a_hmac.BlockSize - 1);
			keys_length.Add(a_hmac.BlockSize);
			keys_length.Add(a_hmac.BlockSize + 1);

			var msgs_length = new List<int>();
			msgs_length.AddRange(keys_length);
			msgs_length.Add(a_hmac.BlockSize * 4 - 1);
			msgs_length.Add(a_hmac.BlockSize * 4);
			msgs_length.Add(a_hmac.BlockSize * 4 + 1);

			foreach(int key_length in keys_length)
			{
				byte[] key = m_random.NextBytes(key_length);

				a_base_hmac.Key = key;
				a_hmac.Key = key;

				foreach(int msg_length in msgs_length)
				{
					byte[] msg = m_random.NextBytes(msg_length);

					a_base_hmac.Initialize();
					a_base_hmac.TransformBytes(msg);
					HashResult h1 = a_base_hmac.TransformFinal();

					a_hmac.Initialize();
					a_hmac.TransformBytes(msg);
					HashResult h2 = a_hmac.TransformFinal();

					Assert.AreEqual(h1, h2, a_hmac.Name);
				}
			}
		}
	}
}