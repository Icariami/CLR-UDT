using System;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Xml.Serialization;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Data.SqlClient;

public class IBANActions
{
    public static bool ValidateCountryCode(string countryCode)
    {
        return Regex.IsMatch(countryCode, @"^[A-Z]{2}$");
    }

    public static bool ValidateCheckDigits(string checkDigits)
    {
        return Regex.IsMatch(checkDigits, @"^\d{2}$");
    }

    public static bool ValidateBankSettlementNumber(string bankSettlementNumber)
    {
        return Regex.IsMatch(bankSettlementNumber, @"^\d{8}$");
    }

    public static bool ValidateBban(string bban)
    {
        return Regex.IsMatch(bban, @"^\d{16}$");
    }

    public static bool ValidateAccountHolderName(string name)
    {
        return Regex.IsMatch(name, @"^[a-zA-Z '-]+$");
    }

    public static void InsertBankAccount()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                connection.Open();

                Console.WriteLine(@"Enter these fields:");
                Console.Write("Country Code (e.g. \"PL\" or \"EN\") : ");
                string countryCode = Console.ReadLine();
                while (!ValidateCountryCode(countryCode))
                {
                    Console.Write("Invalid country code. Please enter it again: ");
                    countryCode = Console.ReadLine();
                }

                Console.Write("Check digits (two-digit number) : ");
                string checkDigits = Console.ReadLine();
                while (!ValidateCheckDigits(checkDigits))
                {
                    Console.Write("Invalid check digits. Please enter it again: ");
                    checkDigits = Console.ReadLine();
                }

                Console.Write("Bank Settlement Number (8-digit number) : ");
                string bankSettlementNumber = Console.ReadLine();
                while (!ValidateBankSettlementNumber(bankSettlementNumber))
                {
                    Console.Write("Invalid bank settlement number. Please enter it again: ");
                    bankSettlementNumber = Console.ReadLine();
                }

                Console.Write("BBAN (Basic Bank Account Number): ");
                string bban = Console.ReadLine();
                while (!ValidateBban(bban))
                {
                    Console.Write("Invalid bban. Please enter it again: ");
                    bban = Console.ReadLine();
                }

                Console.Write("Account Holder Name: ");
                string accountHolderName = Console.ReadLine();
                while (!ValidateAccountHolderName(accountHolderName))
                {
                    Console.Write("Invalid account holder name. Please enter it again: ");
                    accountHolderName = Console.ReadLine();
                }

                decimal balance;
                string balanceInput;
                do
                {
                    Console.Write("Account balance (decimal number): ");
                    balanceInput = Console.ReadLine();
                } while (!decimal.TryParse(balanceInput, out balance));

                IBANAccountNumber iban = new IBANAccountNumber(countryCode, checkDigits, bankSettlementNumber, bban, accountHolderName, balance);
                string insertQuery = "INSERT INTO BankAccounts VALUES (@iban)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                SqlParameter ibanParam = new SqlParameter("@iban", iban) { UdtTypeName = "[CLR_UDT].[dbo].[IBANAccountNumber]" };
                insertCommand.Parameters.Add(ibanParam);
                insertCommand.ExecuteNonQuery();

                Console.WriteLine("Data inserted successfully!");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error connecting to database:");
            Console.WriteLine(ex.Message); // Display the error message for debugging
        }

    }

    public static void SelectBankAccount()
    {
        string sql = @"
        SELECT 
            ID,
            iban.ToString() AS IBAN
        FROM BankAccounts;
    ";
        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string iban = reader.GetString(1);

                                Console.WriteLine($"ID: {id}, IBAN: {iban}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No records found for the provided table.");
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error connecting to database:");
            Console.WriteLine(ex.Message); 
        }

    }

    public static void SearchData()
    {
        string choice = "Choose ";
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
                            InsertBankAccount();
                            break;
                        case 2: // select data
                            SelectBankAccount();
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
    }

    public static void MainAction()
    {
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
                            InsertBankAccount();
                            break;
                        case 2: // select data
                            SelectBankAccount();
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
    }

    public static void Reset()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                connection.Open();

                string dropTableQuery = "DROP TABLE IF EXISTS BankAccounts";
                SqlCommand dropCommand = new SqlCommand(dropTableQuery, connection);
                dropCommand.ExecuteNonQuery();

               
                string createTableQuery = @"
        
                CREATE TABLE BankAccounts
                (
                    ID int IDENTITY(1,1) PRIMARY KEY,
                    iban [dbo].[IBANAccountNumber]
                );
                ";
                SqlCommand createCommand = new SqlCommand(createTableQuery, connection);
                createCommand.ExecuteNonQuery();

                IBANAccountNumber iban = new IBANAccountNumber("PL", "12", "52637487", "1287645367459876", "Jan Kowalski", 2764.76M);
                string insertQuery = "INSERT INTO BankAccounts VALUES (@iban)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                SqlParameter ibanParam = new SqlParameter("@iban", iban) { UdtTypeName = "[CLR_UDT].[dbo].[IBANAccountNumber]" };
                insertCommand.Parameters.Add(ibanParam);
                insertCommand.ExecuteNonQuery();

                iban = new IBANAccountNumber("PL", "12", "02546378", "9834567234568098", "Julia Nowak", 91876.12M);
                insertQuery = "INSERT INTO BankAccounts VALUES (@iban)";
                insertCommand = new SqlCommand(insertQuery, connection);
                ibanParam = new SqlParameter("@iban", iban) { UdtTypeName = "[CLR_UDT].[dbo].[IBANAccountNumber]" };
                insertCommand.Parameters.Add(ibanParam);
                insertCommand.ExecuteNonQuery();

                iban = new IBANAccountNumber("PL", "12", "73541234", "0192837465748392", "Stefan Nowy", 876.12M);
                insertQuery = "INSERT INTO BankAccounts VALUES (@iban)";
                insertCommand = new SqlCommand(insertQuery, connection);
                ibanParam = new SqlParameter("@iban", iban) { UdtTypeName = "[CLR_UDT].[dbo].[IBANAccountNumber]" };
                insertCommand.Parameters.Add(ibanParam);
                insertCommand.ExecuteNonQuery();

                iban = new IBANAccountNumber("PL", "12", "64758734", "3121089780523413", "Natalia Wisniewska", 12.42M);
                insertQuery = "INSERT INTO BankAccounts VALUES (@iban)";
                insertCommand = new SqlCommand(insertQuery, connection);
                ibanParam = new SqlParameter("@iban", iban) { UdtTypeName = "[CLR_UDT].[dbo].[IBANAccountNumber]" };
                insertCommand.Parameters.Add(ibanParam);
                insertCommand.ExecuteNonQuery();

                iban = new IBANAccountNumber("PL", "12", "64758734", "9384123000912345", "Jakub Czerwony", 162.52M);
                insertQuery = "INSERT INTO BankAccounts VALUES (@iban)";
                insertCommand = new SqlCommand(insertQuery, connection);
                ibanParam = new SqlParameter("@iban", iban) { UdtTypeName = "[CLR_UDT].[dbo].[IBANAccountNumber]" };
                insertCommand.Parameters.Add(ibanParam);
                insertCommand.ExecuteNonQuery();

                Console.WriteLine("Bank accounts reseted successfully!");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error connecting to database:");
            Console.WriteLine(ex.Message); // Display the error message for debugging
        }
    }
}
