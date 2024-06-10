using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MainProgram
{

    public static void Main(string[] args)
    {
        string introduction =
@"
CLR User-Defined Types - choose a table, and then an action to do with it:

Choose table 
1 - IBAN Bank account number
2 - NIP
3 - Address
4 - Phone Number
5 -
6 -

";
        
        Console.WriteLine(introduction);

        int table;
        do
        {
            string userInput = Console.ReadLine();
            if (int.TryParse(userInput, out table))
            {
                if (table >=1 && table <= 6)
                {
                    switch (table)
                    {
                        case 1:
                            PrintOptions("IBAN Bank account number");
                            break;
                        case 2:
                            PrintOptions("NIP");
                            break;
                        case 3:
                            PrintOptions("Address");
                            break;
                        case 4:
                            PrintOptions("Phone Number");
                            break;
                        case 5:
                            PrintOptions(" ");
                            break;
                        case 6:
                            PrintOptions(" ");
                            break;
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please choose a number from 1 to 6.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please choose a number from 1 to 6.");
            }
        } while (true);
        
    }

    public static void PrintOptions(string tableName)
    {

        string chooseOption = "\nChoose an action:\n";
        chooseOption += "1 - Insert data into table \n";
        chooseOption += "2. Read data from table\n";
        chooseOption += "3. Search data in table\n";
        Console.WriteLine("\n" + tableName + " table");
        Console.WriteLine(chooseOption);

        int action;
        do
        {
            string userInput = Console.ReadLine();
            if (int.TryParse(userInput, out action))
            {
                if (action >= 1 && action <= 3)
                {
                    switch(action)
                    {
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please choose a number from 1 to 3.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number from 1 to 3.");
            }
        } while (true);
    }
}

