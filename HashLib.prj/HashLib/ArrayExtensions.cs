﻿using System;
using System.Diagnostics;

namespace HashLib
{
	[DebuggerStepThrough]
	public static class ArrayExtensions
	{
		public static void Clear(this Array a_array)
		{
			Debug.Assert(a_array != null);

			Array.Clear(a_array, 0, a_array.Length);
		}

		public static T[] SubArray<T>(this T[] a_array, int a_index, int a_count = -1)
		{
			Debug.Assert(a_array != null);
			Debug.Assert(a_index >= 0);

			if(a_count == -1)
				a_count = a_array.Length - a_index;

			Debug.Assert(a_index + a_count <= a_array.Length);

			var result = new T[a_count];
			Array.Copy(a_array, a_index, result, 0, a_count);
			return result;
		}
	}
}