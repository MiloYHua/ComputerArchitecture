using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Runtime.CompilerServices;

namespace OpCodeMath
{
    internal class Program
    {
        enum OpCode : byte
        {
            Add = 0x0,
            Subtract = 0x1,
            Multiply = 0x2,
            Divide = 0x3
        }
        
        static sbyte Sgn(sbyte num)
        {
            if (num < 0) return -1;
            else return 1;
        }
        static byte Add(byte a, byte b) => (byte)(a + b);
        static byte Subtract(byte a, byte b) => Add(a, (byte)(~b + 1));
        static byte Multiply(byte a, byte b)
        {
            int index = 0;
            byte increment = 0;

        awesomeLabel:
            increment = Add(increment, a);
            index++;

            if (index == b) goto end;
            goto awesomeLabel;

        end:
            return increment;
        }
        static byte Divide(byte a, byte b)
        {
            byte count = 0;

        awesomeLabel:
            a = Subtract(a, b);
            count++;

            if (a < b) goto end;
            goto awesomeLabel;

        end:
            return count;
        }
        static sbyte SignedDivide(sbyte a, sbyte b)
        {
            byte count = 0;
            byte indexer = (byte)Math.Abs(a);
            byte aB = (byte)Math.Abs(b);

        awesomeLabel:
            indexer = Subtract(indexer, aB);
            count++;

            if (indexer < aB) goto end;
            goto awesomeLabel;

        end:
            return (sbyte)Multiply(Multiply(count, (byte)Sgn(a)), (byte)Sgn(b));
        }

        static Dictionary<OpCode, Func<byte, byte, byte>> Operations = new()
        {
            [OpCode.Add] = Add,
            [OpCode.Subtract] = Subtract,
            [OpCode.Multiply] = Multiply,
            [OpCode.Divide] = Divide
        };

        static void Main(string[] args)
        {
            unchecked
            {
                byte bob = Divide(20, 2);

                sbyte sBob = SignedDivide(10, -2);
            }
        }
    }
}
