using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseText
{
	public class DisplayAndCountLines
	{
        public static void Execute() {
            int counter = 0;
            var filePath = @"c:\Zeep\ICD-10Code.txt";

            // Read the file and display it line by line.  
            foreach (string line in System.IO.File.ReadLines(filePath))
            {
                Console.WriteLine(line);
                counter++;
            }

            Console.WriteLine("There were {0} lines.", counter);
            // Suspend the screen.  
            Console.ReadLine();
        }
	}
}
