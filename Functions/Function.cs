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
  /*  public abstract class Function
    {
        public abstract void Execute(ValueEntry[] arg_input, int iN, ValueEntry[] arg_output);
    }

    public sealed class Sum : Function
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public override sealed  void Execute(ValueEntry[] arg_input, int iN, ValueEntry[] arg_output)
       {
            float sum = 0.0f;
            for (int i = 0; i < iN; i++)
            {
                float loc = arg_input[i].Payload.FloatValue;
                sum = sum + loc;
            }
            arg_output[0].Payload.FloatValue = sum;
        }
    }

    public sealed class Sub : Function
    {
       [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void Execute(ValueEntry[] arg_input, int iN, ValueEntry[] arg_output)
        {
            float sub = arg_input[0].Payload.FloatValue - arg_input[1].Payload.FloatValue;
            arg_output[0].Payload.FloatValue = sub;            
        }
    }


    public sealed class Mul : Function
    {
       [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void Execute(ValueEntry[] arg_input, int iN, ValueEntry[] arg_output)
        {
            float sum = 0.0f;
            for (int i = 0; i < iN; i++)
            {
                float loc = arg_input[i].Payload.FloatValue;
                sum = sum * loc;
            }
            arg_output[0].Payload.FloatValue = sum;
        }
    }

    public sealed class Div : Function
    {
       [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void Execute(ValueEntry[] arg_input, int iN, ValueEntry[] arg_output)
        {
            float sub = arg_input[0].Payload.FloatValue - arg_input[1].Payload.FloatValue;
            arg_output[0].Payload.FloatValue = sub;
        }
    }

    */
}
