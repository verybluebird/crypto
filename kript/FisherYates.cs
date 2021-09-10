using System;
using System.IO;

namespace cryptography_1
{
    class Program
    {
        static void Main(string[] args)
        {
            const int N = 33;
            char[] alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя".ToCharArray();
            int j;
            char tmp;
            for (int i = N - 1; i > 0; i--)
            {
                j = new Random().Next(0, i);
                tmp = alphabet[i];
                alphabet[i] = alphabet[j];
                alphabet[j] = tmp;
            }
            File.WriteAllText("key.txt", new String(alphabet));
        }
    }
}