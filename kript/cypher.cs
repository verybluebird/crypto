using System;
using System.IO;
using System.Text.RegularExpressions;

namespace kript
{
    class cypher
    {
        static void Main(string[] args)
        {
            string key = null, text = null, en_text = null;
            readFiles(ref key, ref text);
            formatText(ref text);
            replace(key, text, out en_text);
            writeFiles(ref en_text);
        }

        static void readFiles(ref string key, ref string text) //читает ключ и шифруемый текст из файла
        {
            Console.WriteLine("Enter filename of key and text: ");
            string[] Names = new string[2];
            //for (int i = 0; i < 2; i++)
            //    Names[i] = Console.ReadLine();
            Names[0] = "key.txt";
            Names[1] = "text.txt";

            //считывание ключа
            using (FileStream fstream = File.OpenRead(Names[0]))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                key = System.Text.Encoding.UTF8.GetString(array);

                Console.WriteLine($"Ключ: {key}");
            }
            //считывание текста
            using (FileStream fstream = File.OpenRead(Names[1]))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                text = System.Text.Encoding.UTF8.GetString(array);

                Console.WriteLine($"Текст: {text}");
            }


        }
        static void formatText(ref string text) //удаляет из текста все символы, кроме русских букв(включая пробелы), приводит текст к нижнему регистру
        {
            string pattern = @"[a-zA-z0-9\W]+";
            string target = "";
            Regex regex = new Regex(pattern);
            string result = regex.Replace(text, target);
            text = result.ToLower();

            Console.WriteLine($"Текст: {text}");

        }
        static void replace(string key, string text, out string en_text) //шифрует текст
        {
            en_text = "";
            char[] rus_low = { 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я' };
            char[] keys = key.ToCharArray();
            foreach (var c in text)
                en_text += keys[Array.IndexOf(rus_low, c)];
        }

        static void writeFiles(ref string text)
        {
            string filename;
            Console.WriteLine("Enter filename to write: ");
            filename = Console.ReadLine();
            using (FileStream fstream = new FileStream(filename, FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.UTF8.GetBytes(text);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
                Console.WriteLine("Текст записан в файл");
            }
        }
    }
}