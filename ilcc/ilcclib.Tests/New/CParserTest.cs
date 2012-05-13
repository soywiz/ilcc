﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ilcclib.New.Parser;

namespace ilcclib.Tests.New
{
	[TestClass]
	public class CParserTest
	{
		[TestMethod]
		public void TestMethod1()
		{
			var Node = CParser.StaticParseExpression("1 ? 3 * 2 + 3 * (4 + 4) : 4");
			Console.WriteLine(Node.ToYaml());
		}

		[TestMethod]
		public void TestMethod2()
		{
			var Node = CParser.StaticParseExpression("a++ + ++b");
			Console.WriteLine(Node.ToYaml());
		}

		[TestMethod]
		public void TestMethod3()
		{
			var Node = CParser.StaticParseExpression("**ptr++");
			Console.WriteLine(Node.ToYaml());
		}
	}
}
