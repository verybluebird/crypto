using System;
using System.Numerics;

namespace kript2
{
    class Program
    {
        static bool keys_generator(ref BigInteger y, ref BigInteger g, ref BigInteger p, ref BigInteger x)
        {
            // Генерирует 2 ключа: публичный (y, g, p) и приватный (x)
            // true - ключи успешно сгенерировались, иначе false
            Console.WriteLine("Введите p: ");
            p = BigInteger.Parse(Console.ReadLine());
            Console.WriteLine("Введите g: ");
            g = BigInteger.Parse(Console.ReadLine());

            if (!primality_test(p))
            {
                Console.WriteLine("Число p не является простым.");
                return false;
            }

            if (!is_primitive_root(g, p))
            {
                Console.WriteLine("Число g не является первообразным корнем по модулю p.");
                return false;
            }

            x = RandomIntegerBelow(p - 1);
            y = BigInteger.ModPow(g,x,p);
            return true;
        }
        
        static BigInteger gcd(BigInteger a, BigInteger b)
        {
            // Алгоритм Евклида
            return b == 0 ? a : gcd(b, a % b);
        }
        static BigInteger phi(BigInteger n)
        {
            // Функция Эйлера
            BigInteger result = 1;
            BigInteger previous = -1;
            for (int i = 2; i <= n / i;)
            {
                if (n % i == 0)
                {
                    if (i == previous)
                        result *= i;
                    else
                    {
                        result *= i - 1;
                        previous = i;
                    }
                    n /= i;
                }
                else
                    i++;
            }
            BigInteger p = n;
            if (n > 1)
                if (p == previous)
                    result *= p;
                else
                    result *= p - 1;
            return result;
        }
        static bool primality_test(BigInteger n)
        {
            // Является ли число n простым?
            if (n == 1)
                return false;
            if (n % 2 == 0 && n != 2)
                return false;
            for (int i = 3; i <= n / i; i += 2)
                if (n % i == 0)
                    return false;
            return true;
        }

        static BigInteger RandomIntegerBelow(BigInteger N)
        {
            byte[] bytes = N.ToByteArray();
            BigInteger R;
            Random random = new Random();
            do
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= (byte)0x7F;
                R = new BigInteger(bytes);
            } while (R >= N);

            if (R == 0)
                R += 2;
            if (R == 1)
                R += 1;

            return R;
        }

        static bool is_primitive_root(BigInteger g, BigInteger m)
        {
            // Является ли число g первообразным корнем по модулю m?
            if (g >= m || g < 0 || gcd(g, m) != 1)
                return false;

            BigInteger n = phi(m);
            BigInteger phi_m = n;
            BigInteger previous = -1;
            for (BigInteger i = 2; i <= n / i; i++)
            {
                while (n % i == 0)
                {
                    if (i != previous && BigInteger.ModPow(g, phi_m / i,m) == 1)
                        return false;
                    n /= i;
                    previous = i;
                }
            }

            if (n != 1)
            {
                if (n != previous && BigInteger.ModPow(g, phi_m / n, m)  == 1)
                    return false;
            }
            return true;
        }
        static void Encrytion(BigInteger p, BigInteger g, BigInteger y)
        {
            BigInteger k = RandomIntegerBelow(p - 1);
            Console.WriteLine("Введите сообщение: ");
            BigInteger M = Convert.ToInt32(Console.ReadLine());
            BigInteger s = BigInteger.ModPow(y, k, p);
            BigInteger a = BigInteger.ModPow(g, k, p);
           
            BigInteger b = (s * M) % p;
            Console.WriteLine("a = " + a + " b = " + b);

        }
        static void Decrytion(BigInteger x, BigInteger a, BigInteger b, BigInteger p)
        {
            BigInteger M;
            BigInteger ret = BigInteger.ModPow(a, p - 1 - x, p);
            M = ((b % p) * ret) % p;

            Console.WriteLine("Расшифрованное сообщение M = " + M);
        }
        static void Main(string[] args)
        {
            BigInteger p = 0; BigInteger g = 0; BigInteger x = 0; BigInteger y = 0;
            if (keys_generator(ref y, ref g, ref p, ref x))
            {
                Encrytion(p, g, y);
                Console.WriteLine("Введите a: ");
                BigInteger a = BigInteger.Parse(Console.ReadLine());
                Console.WriteLine("Введите b: ");
                BigInteger b = BigInteger.Parse(Console.ReadLine());
                Decrytion(x, a, b, p);
            }

        }
    }
}