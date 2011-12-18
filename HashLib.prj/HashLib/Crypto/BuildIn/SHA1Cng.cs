﻿using System;

namespace HashLib.Crypto.BuildIn
{
	class SHA1Cng : HashCryptoBuildIn
	{
		public SHA1Cng()
			: base(new System.Security.Cryptography.SHA1Cng(), 64)
		{
		}
	}
}