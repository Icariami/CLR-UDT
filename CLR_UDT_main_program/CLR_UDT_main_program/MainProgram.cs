using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MainProgram
{

    public static void Main(string[] args)
    {
        string introduction1 =
@"
CLR User-Defined Types - choose a table, and then an action to do with it:
";
        string introduction2 =
@"
Choose table 
1 - IBAN Bank account number
2 - NIP
3 - Address
4 - Phone Number
5 - Geolocation
6 - RGBA Color

";
        
        Console.WriteLine(introduction1);
        string continueInput;
        do
        {
            Console.WriteLine(introduction2);
            int table;
            do
            {
                string userInput = Console.ReadLine();
                if (int.TryParse(userInput, out table))
                {
                    if (table >= 1 && table <= 6)
                    {
                        switch (table)
                        {
                            case 1:
                                PrintOptions("IBAN Bank account number");
                                IBANActions.MainAction();
                                break;
                            case 2:
                                PrintOptions("NIP");
                                NipActions.MainAction();
                                break;
                            case 3:
                                PrintOptions("Address");
                                AddressActions.MainAction();
                                break;
                            case 4:
                                PrintOptions("Phone Number");
                                PhoneNumberActions.MainAction();
                                break;
                            case 5:
                                PrintOptions("Geolocation");
                                GeolocationActions.MainAction();
                                break;
                            case 6:
                                PrintOptions("RGBA Color");
                                RGBAColorActions.MainAction();
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

            Console.WriteLine("\nDo you want to perform another action? (y/n or q to quit)");
            continueInput = Console.ReadLine().ToLower();
        } while (continueInput == "y" || continueInput == "yes");

        Console.WriteLine("Exiting program...");

    }

    public static void PrintOptions(string tableName)
    {

        string chooseOption = "\nChoose an action:\n";
        chooseOption += "1 - Insert data into table \n";
        chooseOption += "2 - Read data from table\n";
        chooseOption += "3 - Search data in table\n";
        Console.WriteLine("\n" + tableName + " table");
        Console.WriteLine(chooseOption);     
    }
}

