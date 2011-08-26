﻿using System;
using System.IO;

using NUnit.Framework;

namespace CRYFORCE.Engine.Test
{
	[TestFixture]
	class IN2itionTest
	{
		#region Setup/Teardown

		[SetUp]
		public void SetUp()
		{
			SetUpIsOK = true; // Указываем, что была произведена установка
		}

		#endregion

		/// <summary>Была произведена установка?</summary>
		public bool SetUpIsOK { get; set; }

		/// <summary>
		/// Тест корректности работы методов установки/считывания бит
		/// </summary>
		[Test]
		public void IN2itionBitTest()
		{
			if(!SetUpIsOK)
			{
				SetUp();
			}

			try
			{
				IN2ition eIN2ition = new IN2ition();

				byte[] BitMask = new byte[] {0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80};
				
				int bitIdx = 0;
				byte target = 0x00;
				foreach(var bitMaskItem in BitMask)
				{
					// Пробегаясь по всем битам текущего значения битовой маски...
					for(int nBitMaskItem = 0; nBitMaskItem < 8; nBitMaskItem++)
					{
						if(nBitMaskItem == bitIdx)
						{
							//...должны получать единицу только там, где она есть...
							if(eIN2ition.GetBit(bitMaskItem, nBitMaskItem) != 0x01)
							{
								throw new InvalidDataException("IN2itionBitTest: Wrong GetBit()!");
							}
						}
						else
						{
							//...и нули только там, где они есть
							if(eIN2ition.GetBit(bitMaskItem, nBitMaskItem) != 0x00)
							{
								throw new InvalidDataException("IN2itionBitTest: Wrong GetBit()!");
							}
						}
					}
					
					// Накапливаем биты в приемнике
					eIN2ition.SetBit(ref target, bitIdx, 0x01);

					bitIdx++;
				}

				if(target != 0xFF)
				{
					throw new InvalidDataException("IN2itionBitTest: Wrong SetBit()!");
				}
			} // try
			catch(Exception e)
			{
				Console.WriteLine(e.ToString());
				Assert.Fail();
			}
		}
	}
}