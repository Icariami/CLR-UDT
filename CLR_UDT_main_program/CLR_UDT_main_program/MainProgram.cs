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
5 - Geolocation
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
                            int action1;
                            do
                            {
                                string userInput1 = Console.ReadLine();
                                if (int.TryParse(userInput1, out action1))
                                {
                                    if (action1 >= 1 && action1 <= 3)
                                    {
                                        switch (action1)
                                        {
                                            case 1:
                                                IBANActions.InsertBankAccount();
                                                break;
                                            case 2: // select data
                                                IBANActions.SelectBankAccount();
                                                break;
                                            case 3: // search data
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
                            break;
                        case 2:
                            PrintOptions("NIP");
                            int action2;
                            do
                            {
                                string userInput2 = Console.ReadLine();
                                if (int.TryParse(userInput2, out action2))
                                {
                                    if (action2 >= 1 && action2 <= 3)
                                    {
                                        switch (action2)
                                        {
                                            case 1:
                                                NipActions.InsertNIP();
                                                break;
                                            case 2: // select data
                                                NipActions.SelectNIP();
                                                break;
                                            case 3: // search data
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
                            break;
                        case 3:
                            PrintOptions("Address");
                            int action3;
                            do
                            {
                                string userInput3 = Console.ReadLine();
                                if (int.TryParse(userInput3, out action3))
                                {
                                    if (action3 >= 1 && action3 <= 3)
                                    {
                                        switch (action3)
                                        {
                                            case 1:
                                                AddressActions.InsertAddress();
                                                break;
                                            case 2: // select data
                                                AddressActions.SelectAddresses();
                                                break;
                                            case 3: // search data
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
                            break;
                        case 4:
                            PrintOptions("Phone Number");
                            int action4;
                            do
                            {
                                string userInput4 = Console.ReadLine();
                                if (int.TryParse(userInput4, out action4))
                                {
                                    if (action4 >= 1 && action4 <= 3)
                                    {
                                        switch (action4)
                                        {
                                            case 1:
                                                PhoneNumberActions.InsertPhoneNumber();
                                                break;
                                            case 2: // select data
                                                PhoneNumberActions.SelectPhoneNumbers();
                                                break;
                                            case 3: // search data
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
                            break;
                        case 5:
                            PrintOptions("Geolocation");
                            GeolocationActions.MainAction();
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
    }
}

