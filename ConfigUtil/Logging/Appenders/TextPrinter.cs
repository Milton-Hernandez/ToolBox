﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ToolBox
{
    public class LogEntry
    {
        public ConsoleColor[] Colors { get; private set; }
        public string[] Msg { get; private set; }
        public DateTime Time { get; private set; }

        public LogEntry(ConsoleColor[] c, string[] m, DateTime t)
        {
            Colors = c;
            Msg = m;
            Time = t;
        }
    }


    public abstract class TextPrinter 
    {
        public static readonly ConsoleColor[] DEFAULT = { ConsoleColor.Green };

        public abstract void Write(ConsoleColor color, string str);

        public string Separator { get; set; }


        public TextPrinter(string sep)
        {
            Separator = sep;
        }

        public abstract string GetTime(DateTime t); 

        public void Print(DateTime time, ConsoleColor[] colors, params string[] tokens)
        {
            lock (this)
            {
                Write(colors[0], GetTime(time));
                Write(ConsoleColor.Gray, Separator);

                for (int i = 0; i < tokens.Length; i++)
                {
                    Write(colors[i < colors.Length - 1 ? i + 1 : colors.Length - 1], tokens[i]);
                    if (i < (tokens.Length - 1))
                        Write(ConsoleColor.Gray, Separator);
                }
                Write(ConsoleColor.White, Environment.NewLine);
            }
        }
    }
}
