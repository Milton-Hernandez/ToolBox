using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logging;
using CommonStructs;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Loging.Test
{
    class Program  {

        public static void ShowExpression(Expression expr, string indent = "")
        {
            string nextIndent = indent + "  ";
    //      Console.Out.WriteLine(expr.GetType());
            if (expr is System.Linq.Expressions.ConditionalExpression)
            {
                var bin = (ConditionalExpression)expr;
                Console.Out.WriteLine("IF: ");
                ShowExpression(bin.Test,nextIndent);
                Console.Out.WriteLine("THEN: ");
                ShowExpression(bin.IfTrue, nextIndent);
                Console.Out.WriteLine("ELSE: ");
                ShowExpression(bin.IfFalse, nextIndent);
                return;
            }

            if (expr is BinaryExpression)
            {
                var bin = (BinaryExpression)expr;
                Console.Out.WriteLine(indent + bin.NodeType + ":" + bin.Type);
                ShowExpression(bin.Left, nextIndent);
                ShowExpression(bin.Right, nextIndent);
                return;
            }

            if (expr is UnaryExpression)
            {
                var bin = (UnaryExpression)expr;
                Console.Out.WriteLine(indent + bin.NodeType + ":" + bin.Type);
                ShowExpression(bin.Operand, nextIndent);
                return;
            }

            if (expr is LambdaExpression )
            {
                Console.Out.Write(indent + "Params: [");
                var lambda = (LambdaExpression)expr;
                foreach (var p in lambda.Parameters)
                    Console.Out.Write(p + ":" + p.Type + " " );
                Console.Out.WriteLine("] : " + lambda.Body.Type);

                ShowExpression(lambda.Body, nextIndent);
                return;
            }

            if (expr is MethodCallExpression)
            {
                var lambda = (MethodCallExpression)expr;

                Console.Out.WriteLine(indent + lambda.Method.Name + ":" + lambda.Type);
                foreach (var p in lambda.Arguments)
                    ShowExpression(p, nextIndent);               
                return;
            }


            if (expr is ParameterExpression || expr is ConstantExpression)
            {
                Console.Out.WriteLine(indent + expr.ToString() +":" + expr.Type);
                return;
            }

           
        }


        static void Main(string[] args)   {
            try
            {
                Money[] arr = new Money[] { 
                  new Money((decimal) 17820, Cur.CAD),
                  new Money( 2012345.34m, Cur.USD),
                  new Money(500324324.35m, Cur.EUR),
                  new Money(320335.334m, Cur.GBP)
                };
                
                arr[0] = Money.Parse("C$10,000.00");
                arr[1] = Money.Parse("$2,000,000.00");
                arr[2] = Money.Parse("$500,000,000.00");
                arr[3] = (Money) 0.0;

                double total = 0.0;
                foreach (var m in arr)
                {
                    total += m;
                    Console.Out.WriteLine(total);
                }

                Console.Out.WriteLine("Result: " + total);

            }
            catch (Exception ex)
            {
                Log.Fatal("Bad Exception", ex);
            }
        }
    }
}
