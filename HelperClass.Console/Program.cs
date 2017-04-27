using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HelperClass.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var r = new RSACrypt();
            var a = "";
            var b = "";
            r.RSAKey(out a, out b);
            System.Console.ReadKey();
        }
    }
}
