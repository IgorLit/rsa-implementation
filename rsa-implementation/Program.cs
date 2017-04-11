using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsa_implementation
{
    class Program
    {
        static void Main(string[] args)
        {
            RSA rsa = new RSA();
            Console.WriteLine(rsa.getEncriptedText("hello"));

            Console.WriteLine(rsa.getDecryptedText(rsa.getEncriptedText("hello")));
        }
    }
}
