﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClass.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine(FileHelper.ReadUseFs(@"F:\temp.txt"));
            System.Console.ReadKey();
        }
    }
}