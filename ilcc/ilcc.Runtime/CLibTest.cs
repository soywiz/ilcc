﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

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
			public IntPtr Ptr;
			public fixed int Demo[8];
		}

		[StructLayout(LayoutKind.Sequential, Size = sizeof(int) * 16)]
		public struct FixedSizeVectorTest
		{
			public int FirstElement;
		}

		//public fixed int Test2[7][1];

		static public void TestMethod()
		{
			Test++;
		}

		static public void TestMethod2()
		{
			int z;
			*&z = sizeof(MyStruct);
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

		/*
		static public void VarArgFunc(__arglist)
		{
		}

		static public void TestVarArg()
		{
			VarArgFunc(__arglist(1, 2, 4));
		}
		*/

		static public int TestStackAlloc()
		{
			int* test = stackalloc int[10];
			int* test2;
			*&test2 = test;
			//*&test = stackalloc int[10];
			//return test[0];
			return 0;
		}

		static public int TestCallRunTypeMain(string[] Args)
		{
			return CLibUtils.RunTypeMain(typeof(CLibTest), Args);
		}

		/*
		static public void TestCall2()
		{
			Console.WriteLine("Hello World!");
		}

		delegate void MyTestDelegate();

		static public void TestCall()
		{
			//void* Test = (void*)TestCall2;
			var Pointer = CLibUtils.MethodInfoToPointer(((MyTestDelegate)TestCall2).Method);
			var MethodInfo = CLibUtils.PointerToMethodInfo(Pointer);
			var Delegate2 = (MyTestDelegate)Delegate.CreateDelegate(typeof(Action), MethodInfo);
			Delegate2();
		}
		*/
	}
}
