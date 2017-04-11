using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace rsa_implementation
{
    class RSA
    {
        private char blockSeporator = '*';
        private static List<int> arr;
        private static Random rnd;
        private BigInteger e;
        private BigInteger d;
        private BigInteger n;

        public RSA()
        {
            this.initParams();
        }

        public RSA(char blockSeporator)
        {
            this.blockSeporator = blockSeporator;
             this.initParams();
        }

        private void initParams()
        {
            arr = initPrimeNum();
            rnd = new Random();
            generatePQ();
        }

        private List<int> initPrimeNum()
        {
            List<int> arr = new List<int>();
            int low = 10000, high = 20000, i, flag;
            while (low < high)
            {
                flag = 0;

                for (i = 2; i <= low / 2; ++i)
                {
                    if (low % i == 0)
                    {
                        flag = 1;
                        break;
                    }
                }

                if (flag == 0)
                    arr.Add(low);

                ++low;
            }
            return arr;
        }

        private void getPrimeNum(Random rnd, List<int> arr, out int n1, out int n2)
        {
            int firstNumber = arr[rnd.Next(arr.Count)];
            int secondNumber = arr[rnd.Next(arr.Count)];
            if (firstNumber == secondNumber)
            {
                this.getPrimeNum(rnd, arr, out n1, out n2);
            }
            else
            {
                n1 = firstNumber;
                n2 = secondNumber;
            }
        }

        private BigInteger getN(int p, int q)
        {
            return p * q;
        }

        private BigInteger getFi(int p, int q)
        {
            return (p - 1) * (q - 1);
        }

        private BigInteger NOD(BigInteger a, BigInteger b)
        {
            if (a == b)
                return a;
            else
                if (a > b)
                return NOD(a - b, b);
            else
                return NOD(b - a, a);
        }

        private BigInteger getPrime(Random rnd, BigInteger fi)
        {
            BigInteger e = rnd.Next(1, Math.Abs((int)fi));
            do
            {
                if (NOD(e, fi) == 1) break;
                else e++;
            } while (true);
            if (e >= fi)
            {
                e--;
                do
                {
                    if (NOD(e, fi) == 1) break;
                    else e--;
                } while (true);
            }
            return e;
        }

        private BigInteger getD(BigInteger e, BigInteger fi)
        {
            BigInteger i = fi, v = 0, d = 1;
            while (e > 0)
            {
                BigInteger t = i / e, x = e;
                e = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= fi;
            if (v < 0) v = (v + fi) % fi;
            return v;
        }

        private string encript(int symbol, BigInteger keyE, BigInteger keyN)
        {
            return BigInteger.ModPow(symbol, keyE, keyN).ToString();
        }

        private int decript(int symbol, BigInteger keyD, BigInteger keyN)
        {
            return (int)(BigInteger.ModPow(symbol, keyD, keyN));
        }

        private string textEncript(string text, BigInteger e, BigInteger n)
        {
            string eText = "";
            foreach (var symbol in text)
            {
                eText += encript(symbol, e, n) + this.blockSeporator;
            }
            return eText;
        }

        private string textDecript(string text, BigInteger d, BigInteger n)
        {
            string[] arr = text.Split(this.blockSeporator);
            string dText = "";
            foreach (var symbol in arr)
            {
                if (symbol != String.Empty)
                    dText += Convert.ToChar(decript(int.Parse(symbol), d, n));
            }
            return dText;
        }

        public string getEncriptedText(string originalText)
        {
            return textEncript(originalText, this.e, this.n);
        }

        public string getDecryptedText(string encryptedText)
        {
            return textDecript(encryptedText, this.d, this.n);

        }

        private void generatePQ()
        {
            int q;
            int p;
            getPrimeNum(rnd, arr, out q, out p);
            BigInteger fi = getFi(p, q);
            this.n = getN(p, q);
            this.e = getPrime(rnd, fi);
            this.d = getD(e, fi);
        }
    }

}
