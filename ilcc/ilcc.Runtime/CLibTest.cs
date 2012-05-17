﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ilcc.Runtime
{
	unsafe static public class CLibTest
	{
		static public int Test = -1;

		public struct MyStruct
		{
			public int a;
			public int b;
			public int c;
		}

		static public void TestMethod()
		{
			Test++;
		}

		static public void TestMethod2()
		{
			int z;
			*&z = z;
		}

		static public int TestIncLeft(int a)
		{
			return ++a;
		}

		static public int TestIncRight(int a)
		{
			return a++;
		}

		static public void TestLoop(int a)
		{
			int n = 0;
			int m = 0;
			for (n = 0; n < 10; n++)
			{
				m += 7;
			}
		}

		static public void SetField()
		{
			MyStruct v = default(MyStruct);
			*&(v.a) = 1;
		}

		static public void TestCopyStruct()
		{
			var s1 = default(MyStruct);
			var s2 = default(MyStruct);
			*&s1 = s2;
		}

		static public int TestSizeof()
		{
			return sizeof(MyStruct);
		}

		static public int TestStackAlloc()
		{
			int* test = stackalloc int[10];
			int* test2;
			*&test2 = test;
			//*&test = stackalloc int[10];
			//return test[0];
			return 0;
		}
	}
}
