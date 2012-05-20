﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ilcclib.Parser;

namespace ilcclib.Tests.Parser
{
	[TestClass]
	public class CParserProgramTest
	{
		[TestMethod]
		public void TestMethod9()
		{
			var Node = CParser.StaticParseTranslationUnit(@"
				int n = 5;
				typedef unsigned int uint;

				uint m;

				void main(int argc, char** argv) {
					if (n < 10) {
						printf(""Hello World!: %d"", n);
					} else {
						prrintf(""Test!"");
					}
				}

			");
			Console.WriteLine(Node.ToYaml());
			CollectionAssert.AreEqual(
				new string[] {
					"- TranslationUnit:",
					"   - VariableDeclaration: int n",
					"      - BinaryExpression: =",
					"         - IdentifierExpression: n",
					"         - IntegerExpression: 5",
					"   - TypeDeclaration: typedef unsigned int uint",
					"   - VariableDeclaration: typedef unsigned int m",
					"   - FunctionDeclaration: void main (int argc, char * * argv)",
					"      - IfElseStatement:",
					"         - BinaryExpression: <",
					"            - IdentifierExpression: n",
					"            - IntegerExpression: 10",
					"         - ExpressionStatement:",
					"            - FunctionCallExpression:",
					"               - IdentifierExpression: printf",
					"               - ExpressionCommaList:",
					"                  - StringExpression: Hello World!: %d",
					"                  - IdentifierExpression: n",
					"         - ExpressionStatement:",
					"            - FunctionCallExpression:",
					"               - IdentifierExpression: prrintf",
					"               - ExpressionCommaList:",
					"                  - StringExpression: Test!",
				},
				Node.ToYamlLines().ToArray()
			);
		}

		[TestMethod]
		public void TestMethod10()
		{
			var Node = CParser.StaticParseTranslationUnit(@"
				typedef unsigned int uint;

				void func() {
					uint a;
				}
			");
			Console.WriteLine(Node.ToYaml());
			CollectionAssert.AreEqual(
				new string[] {
					"- TranslationUnit:",
					"   - TypeDeclaration: typedef unsigned int uint",
					"   - FunctionDeclaration: void func ()",
					"      - VariableDeclaration: typedef unsigned int a",
				},
				Node.ToYamlLines().ToArray()
			);
		}

		[TestMethod]
		public void TestVariableFunctionPointer()
		{
			var Node = CParser.StaticParseTranslationUnit(@"
				int (*callback)(int a, int b, void *c);

				void func(int (*callback)(int a, int b, void *c)) {
				}
			");
			Console.WriteLine(Node.ToYaml());
			CollectionAssert.AreEqual(
				new string[] {
					"- TranslationUnit:",
					"   - VariableDeclaration: int callback (int a, int b, void * c) * callback",
					"   - FunctionDeclaration: void func (int callback (int a, int b, void * c) * callback)",
					"      - CompoundStatement:",
				},
				Node.ToYamlLines().ToArray()
			);
		}

		[TestMethod]
		public void TestTypedefFunctionPointer()
		{
			var Node = CParser.StaticParseTranslationUnit(@"
				typedef void* (*alloc_func) (void* opaque, unsigned int items, unsigned int size);

				void func(alloc_func func) {
				}
			");
			Console.WriteLine(Node.ToYaml());
			CollectionAssert.AreEqual(
				new string[] {
					"- TranslationUnit:",
					"   - TypeDeclaration: typedef void * alloc_func (void * opaque, unsigned int items, unsigned int size) * alloc_func",
					"   - FunctionDeclaration: void func (typedef void * alloc_func (void * opaque, unsigned int items, unsigned int size) * func)",
					"      - CompoundStatement:",
				},
				Node.ToYamlLines().ToArray()
			);
		}

		[TestMethod]
		public void TestSizeof()
		{
			var Node = CParser.StaticParseTranslationUnit(@"
				typedef struct Test {
					int x, y, z;
				} Test;

				void test() {
					printf(
						""%d, %d, %d, %d, %d, %d, %d, %d, %d, %d"",
						sizeof(Test),
						sizeof(long long int), sizeof(double), sizeof(long long),
						sizeof(long int), sizeof(int), sizeof(float),
						sizeof(short),
						sizeof(char), sizeof(_Bool)
					);
				}
			");
			Console.WriteLine(Node.ToYaml());

			CollectionAssert.AreEqual(
				new string[] {
					"- TranslationUnit:",
					"   - TypeDeclaration: typedef { int x, int y, int z } Test",
					"   - FunctionDeclaration: void test ()",
					"      - ExpressionStatement:",
					"         - FunctionCallExpression:",
					"            - IdentifierExpression: printf",
					"            - ExpressionCommaList:",
					"               - StringExpression: %d, %d, %d, %d, %d, %d, %d, %d, %d, %d",
					"               - SizeofTypeExpression: typedef { int x, int y, int z }",
					"               - SizeofTypeExpression: long long int",
					"               - SizeofTypeExpression: double",
					"               - SizeofTypeExpression: long long int",
					"               - SizeofTypeExpression: long int",
					"               - SizeofTypeExpression: int",
					"               - SizeofTypeExpression: float",
					"               - SizeofTypeExpression: short",
					"               - SizeofTypeExpression: char",
					"               - SizeofTypeExpression: bool",
				},
				Node.ToYamlLines().ToArray()
			);
		}

		[TestMethod]
		public void TestOldFunctionSyntax()
		{
			var Node = CParser.StaticParseTranslationUnit(@"
				void func(a, b, c)
					int a;
					char *b;
					unsigned short * c;
				{
				}
			");
			Console.WriteLine(Node.ToYaml());
			CollectionAssert.AreEqual(
				new string[] {
					"- TranslationUnit:",
					"   - FunctionDeclaration: void func (int a, char * b, unsigned short * c)",
					"      - CompoundStatement:",
				},
				Node.ToYamlLines().ToArray()
			);
		}
	}
}
