using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices; 

namespace Functions
{
    /*
    public class Solution
    {
        private const int TSIZE = 10;

        [ThreadStatic]  static ValueEntry[] input;
        [ThreadStatic]  static ValueEntry[] output;

        public Function F1 = new Sum();
        public Function F2 = new Sub();
        public Function F3 = new Mul();
        public Function F4 = new Div();

        public float[] res;
        public float[][] inp;

        public Solution(float[][] i)
        {
            inp = i;
            res = new float[inp.Length];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DoIt(int from, int to)
        {
            input = new ValueEntry[5];
            output = new ValueEntry[1];
            output[0].ResetTo(0.0f);
            input[0].ResetTo(0.0f);
            input[1].ResetTo(0.0f);
            input[2].ResetTo(0.0f);
            input[3].ResetTo(0.0f);
            input[4].ResetTo(0.0f);

            for (int i = from; i < to; i++)
            {
                input[0].Payload.FloatValue = inp[i][0];    //This is the place for selecting the variables
                input[1].Payload.FloatValue = inp[i][0];
                input[2].Payload.FloatValue = inp[i][0];
                input[3].Payload.FloatValue = inp[i][0];
                input[4].Payload.FloatValue = inp[i][0];
                F1.Execute(input, 5, output);
                input[0].Payload.FloatValue = output[0].Payload.FloatValue;
                input[1].Payload.FloatValue = 5.0f;
                F4.Execute(input, 2, output);
                res[i] = output[0].Payload.FloatValue;
            }  
        }



        public float[] Execute()
        {
            int chunk = (int) inp.Length / TSIZE;
            Task[] t = new Task[TSIZE];
            for (int i = 0; i < TSIZE; i++)
            {
                int from = i * chunk;
                int to = from + chunk;
                t[i] = Task.Run(() => DoIt(from, to));
            }
            Task.WaitAll(t);
            return res;
        }

    }*/
    

    public class Program
    {

     /*   [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Div(ValueEntry[] arg_input, int iN, ValueEntry[] arg_output)
        {
           float res = (float) (arg_input[0].Payload.FloatValue / arg_input[1].Payload.FloatValue);
           arg_output[0].Payload.FloatValue = res;
        }

       [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sum(ValueEntry[] arg_input, int iN, ValueEntry[] arg_output)
        {
            float sum = 0.0f;
            for (int i = 0; i < iN; i++)
            {
                float loc = arg_input[i].Payload.FloatValue;
                sum = sum + loc;
            }
            arg_output[0].Payload.FloatValue = sum;
        } */

        public static float Sum2(float a, float b, float c, float d, float e)
        {
            return a + b + c + d + e;
        }

        public static float Sum3(float[] arg)
        {
            return arg[0] + arg[1] + arg[2] + arg[3] + arg[4];
        }


      //  public static ValueEntry[] Col(params ValueEntry[] arg)
      //  {
      //      return arg;
      //  }
    
      
        static void Main(string[] args)
        {
            Datum d = new Datum();
            Console.Out.WriteLine(d.Status);
            Console.Out.WriteLine(d.DataType);

            d.Status = DStatus.ERR;
            d.DataType = DType.Cardinal;

            Console.Out.WriteLine(d.Status);
            Console.Out.WriteLine(d.DataType);

            d.Reset();

            Console.Out.WriteLine(d.Status);
            Console.Out.WriteLine(d.DataType);

            d.ResetTo(5.0f);
            Console.Out.WriteLine("Comparator: " + (d == 5.0f));


            DateTime d2 = new DateTime(1938, 1, 1);
            DateTime d3 = new DateTime(2069, 1, 1);
            var i = d3 - d2;
            Console.Out.WriteLine(String.Format("{0:#,##0}", i.TotalSeconds));

           /* int N = Int32.Parse(args[0]);

            float[][] matrix = new float[N][];

            for(int i=0; i<N; i++) {
                matrix[i] = new float[5];
                matrix[i][0] = (float) i;
                matrix[i][1] = (float) i / 2;
                matrix[i][2] = (float) i / 3;
                matrix[i][3] = (float) i / 4;
                matrix[i][4] = (float) i / 5;
            }

            var sols = new Solution[10];
            for(int i=0; i<10; i++)
                sols[i] = new Solution(matrix);

            Stopwatch sp = new Stopwatch();
            sp.Start();
            Parallel.For(0, 10, (i) =>
            {
                float[] ret = sols[i].Execute();
            });
            sp.Stop();
            Console.Out.WriteLine("Size: " + N + " .Elapsed: " + sp.ElapsedMilliseconds + " msecs.");
            sp.Restart();
            for(int i=0; i<N; i++) {
                var a = (float) i;
                var b = (float) i / 2;
                var c = (float) i / 3;
                var d = (float) i / 4;
                var e = (float) i / 5;
                var res = (a+b+c+d+e) / 5;
            }
            sp.Stop();
            Console.Out.WriteLine("Size: " + N + " .Elapsed: " + sp.ElapsedMilliseconds + " msecs.");*/
        }
    } 


}
