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
    #region CommonEnums
 
    public enum DStatus : byte {
             UNK = 0, 
             VOID, 
             ERR, 
             OK
        };


        public enum DType : byte
        {
             Undef        =  0x0,
             Bool         = 0x10, 
             Categorical  = 0x20,
             Cardinal     = 0x30,
             Instant      = 0x40,
             Date         = 0x50,
             Count        = 0x60,
             Int          = 0x70,
             Amount       = 0x80,
             Field        = 0x90
        };

       
    public static class Values {
        public static readonly float Epsilon    =  0.00000000000001f;
        public static readonly float NegEpsilon = -0.00000000000001f;
        public static readonly DateTime BoT   = new DateTime(1938, 1, 1);
        public static readonly DateTime EoT   = new DateTime(2069, 1, 1);
        public static readonly DateTime YZero = new DateTime();

        public static bool AproxEqual(float a, float b)
        {
            var temp = a - b;
            if (temp > Epsilon || temp < NegEpsilon)
                return false;
            return true;
        }
    }
    #endregion
    public class Value
    {
        public DStatus Status;
        public DType DataType;
        public ushort? ShortVal;
        public double? FloatValue;
        public long?   IntValue;
        public Value[] ValArray;
    }

    


    [StructLayout(LayoutKind.Explicit)]
    public struct Datum
    {
        [FieldOffset(0)]  public long       Memento;
        [FieldOffset(0)]  public DStatus    Status;
        [FieldOffset(1)]  public DType      DataType;
        [FieldOffset(2)]  public ushort     Category;
        [FieldOffset(2)]  public short      Mantissa;
        [FieldOffset(4)]  public uint       UIntValue;
        [FieldOffset(4)]  public int        IntValue;
        [FieldOffset(4)]  public float      FloatValue;

        #region Reset Methods
        public void Reset(long arg = 0) { Memento = arg; }

        public void ResetTo(DateTime arg, bool dateOnly = false)
        {
            if (arg < Values.BoT || arg > Values.EoT || dateOnly)
            {
                var nd = new DateTime(arg.Year, arg.Month, arg.Day);
                int days = (int)Values.YZero.Subtract(nd).TotalDays;
                DataType = DType.Date;
                IntValue = days;
            }
            else
            {
                DataType = DType.Instant;
                UIntValue = (uint)(arg - Values.BoT).TotalSeconds;
            }
        }

        public void ResetTo(int arg)
        {
            Status = DStatus.OK;
            DataType = DType.Int;
            IntValue = arg;
        }

        public void ResetTo(uint arg)
        {
            Status = DStatus.OK;
            DataType = DType.Count;
            UIntValue = arg;
        }

        public void ResetTo(ushort cat, uint val, bool ordered = false)
        {
            Status = DStatus.OK;
            if (ordered)
                DataType = DType.Cardinal;
            else
                DataType = DType.Categorical;
            Category = cat;
            UIntValue = val;
        }

        public void ResetTo(float arg, bool positive = false)
        {
            Status = DStatus.OK;
            if (positive)
            {
                DataType = DType.Amount;
                if (arg < 0.0f)
                    Status = DStatus.ERR;
            }
            else
                DataType = DType.Field;
            FloatValue = arg;
        }

        public void ResetTo(double arg, bool positive = false)
        {
            ResetTo((float)arg, positive);
        }

        public void ResetTo(bool arg)
        {
            Status = DStatus.OK;
            DataType = DType.Bool;
            IntValue = arg ? 1 : 0;
        }

        #endregion

        #region Equality Ops       
        public override bool Equals(object m2) {
            if (!(m2 is Datum)) {
                Datum d2 = (Datum)m2;
                return (d2.Memento == Memento);
            }
            return false;
        }
       
        public static bool operator ==(Datum m1, Datum m2)
        {
            return m1.Equals(m2);
        }

        public static bool operator !=(Datum m1, Datum m2)
        {
            return !m1.Equals(m2);
        }

        #region Int Comparators
        public static bool operator ==(Datum m1, int i1){
            return ( m1.Status   == DStatus.OK &&
                     m1.DataType == DType.Int &&
                     m1.IntValue == i1);
        }

        public static bool operator !=(Datum m1, int i1)
        {
            return !(m1 == i1);
        }

        public static bool operator ==(int i1, Datum m1)
        {
            return m1 == i1;
        }

        public static bool operator !=(int i1, Datum m1)
        {
            return !(m1 == i1);
        }
        #endregion

        #region UInt Comparator
        public static bool operator ==(Datum m1, uint i1)
        {
            return (m1.Status == DStatus.OK &&
                     m1.DataType == DType.Count &&
                     m1.UIntValue == i1);
        }

        public static bool operator !=(Datum m1, uint i1)
        {
            return !(m1 == i1);
        }

        public static bool operator ==(uint i1, Datum m1)
        {
            return m1 == i1;
        }

        public static bool operator !=(uint i1, Datum m1)
        {
            return !(m1 == i1);
        }
        #endregion

        #region Float Comparator
        public static bool operator ==(Datum m1, float i1)  {
            return ( m1.Status == DStatus.OK &&
                    (m1.DataType == DType.Field || m1.DataType == DType.Amount) &&
                     Values.AproxEqual(m1.FloatValue,i1));
        }

        public static bool operator !=(Datum m1, float i1) {
            return !(m1 == i1);
        }

        public static bool operator ==(float i1, Datum m1) {
            return m1 == i1;
        }

        public static bool operator !=(float i1, Datum m1) {
            return !(m1 == i1);
        }
        #endregion

        #region Double Comparator
        public static bool operator ==(Datum m1, double i1)
        {
            return (m1 == (float) i1);
        }

        public static bool operator !=(Datum m1, double i1)
        {
            return !(m1 == i1);
        }

        public static bool operator ==(double i1, Datum m1)
        {
            return m1 == i1;
        }

        public static bool operator !=(double i1, Datum m1)
        {
            return !(m1 == i1);
        }
        #endregion

        #region Bool Comparator
        public static bool operator ==(Datum m1, bool i1)
        {
            return (m1.Status == DStatus.OK &&
                    (m1.DataType == DType.Bool) &&
                    (((m1.UIntValue != 0) & i1) || ((m1.UIntValue == 0) & !i1)));
        }

        public static bool operator !=(Datum m1, bool i1)
        {
            return !(m1 == i1);
        }

        public static bool operator ==(bool i1, Datum m1)
        {
            return m1 == i1;
        }

        public static bool operator !=(bool i1, Datum m1)
        {
            return !(m1 == i1);
        }
        #endregion

        //Date and Instant Comparators

        //Categorical Comparators

        #endregion
    }
}
