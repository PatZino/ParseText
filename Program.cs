using System;

namespace ParseText
{
    class Program
    {
        static void Main(string[] args)
        {
            int counter = 0;

            // Read the file and display it line by line.  
            foreach (string line in System.IO.File.ReadLines(@"c:\Zeep\ICD-10Code.txt"))
            {
                System.Console.WriteLine(line);
                counter++;
            }

            var query = String.Format(@"hello all");
            System.Console.WriteLine("There were {0} lines.", counter);
            // Suspend the screen.  
            System.Console.ReadLine();

            ExtractCSV.ExtractCsvData3();
            Console.WriteLine("Hello World!");

        }
    }
}
