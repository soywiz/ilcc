﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ilcclib.New.Parser
{
	public partial class CParser
	{
		public class ParserNodeExpressionList : Node
		{
			public ParserNodeExpressionList()
				: base(new Expression[] { })
			{
			}
		}

		public class SpecialIdentifierExpression : LiteralExpression
		{
			public string Value;

			public SpecialIdentifierExpression(string Value)
				: base()
			{
				this.Value = Value;
			}

			protected override string GetParameter()
			{
				return String.Format("{0}", Value);
			}
		}

		public class IdentifierExpression : LiteralExpression
		{
			public string Value;

			public IdentifierExpression(string Value)
				: base()
			{
				this.Value = Value;
			}

			protected override string GetParameter()
			{
				return String.Format("{0}", Value);
			}
		}

		public class IntegerExpression : LiteralExpression
		{
			public int Value;

			public IntegerExpression(int Value)
				: base()
			{
				this.Value = Value;
			}

			protected override string GetParameter()
			{
				return String.Format("{0}", Value);
			}
		}

		public class LiteralExpression : Expression
		{
			public LiteralExpression()
				: base(new Expression[] { })
			{
			}
		}

		public class TrinaryExpression : Expression
		{
			Expression Condition;
			Expression TrueCond;
			Expression FalseCond;

			public TrinaryExpression(Expression Left, Expression TrueCond, Expression FalseCond)
				: base(new Expression[] { Left, TrueCond, FalseCond })
			{
				this.Condition = Left;
				this.TrueCond = TrueCond;
				this.FalseCond = FalseCond;
			}

		}

		public enum OperatorPosition
		{
			Left,
			Right
		}

		public class UnaryExpression : Expression
		{
			string Operator;
			Expression Right;
			OperatorPosition OperatorPosition;

			public UnaryExpression(string Operator, Expression Right, OperatorPosition OperatorPosition = OperatorPosition.Left)
				: base(new Expression[] { Right })
			{
				this.Operator = Operator;
				this.Right = Right;
				this.OperatorPosition = OperatorPosition;
			}

			protected override string GetParameter()
			{
				return String.Format("{0} ({1})", Operator, OperatorPosition);
			}
		}

		public class ArrayAccessExpression : Expression
		{
			Expression Left;
			Expression Index;

			public ArrayAccessExpression(Expression Left, Expression Index)
				: base(new Expression[] { Left, Index })
			{
				this.Left = Left;
				this.Index = Index;
			}
		}

		public class BinaryExpression : Expression
		{
			Expression Left;
			string Operator;
			Expression Right;

			public BinaryExpression(Expression Left, string Operator, Expression Right)
				: base(new Expression[] { Left, Right })
			{
				this.Left = Left;
				this.Operator = Operator;
				this.Right = Right;
			}

			protected override string GetParameter()
			{
				return String.Format("{0}", Operator);
			}
		}

		public class ExpressionCommaList : Expression
		{
			IEnumerable<Expression> Expressions;

			public ExpressionCommaList(IEnumerable<Expression> Expressions)
				: base(Expressions)
			{
				this.Expressions = Expressions;
			}
		}

		public class Expression : Node
		{
			public Expression(IEnumerable<Node> Childs)
				: base(Childs.ToArray())
			{
			}

			public void CheckLeftValue()
			{
				// TODO:
				//throw (new NotImplementedException());
			}
		}

		public class Node
		{
			Node[] Childs;

			/*
			public Node(params Node[] Nodes)
			{
				this.Nodes = Nodes;
			}
			*/

			public Node(Node[] Childs)
			{
				this.Childs = Childs;
			}

			protected virtual string GetParameter()
			{
				return "";
			}

			public XElement AsXml()
			{
				return new XElement(
					GetType().Name,
					new XAttribute("value", GetParameter()),
					Childs.Select(Item => Item.AsXml())
				);
			}

			public IEnumerable<string> ToYamlLines(int Indent = 0)
			{
				yield return String.Format("{0}+ {1}: {2}", String.Concat(Enumerable.Repeat("|   ", Indent)), GetType().Name, GetParameter());
				for (int n = 0; n < Childs.Length; n++)
				{
					var Child = Childs[n];
					foreach (var Line in Child.ToYamlLines(Indent + 1))
					{
						yield return Line;
					}
				}
			}

			public string ToYaml()
			{
				return String.Join("\r\n", this.ToYamlLines());
			}
		}
	}
}
