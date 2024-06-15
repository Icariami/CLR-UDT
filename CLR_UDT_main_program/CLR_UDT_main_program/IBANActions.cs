using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
    private static decimal FormatBalance(decimal balance)
    {
        string balanceString = balance.ToString("F2"); // Format to 2 decimal places
        return decimal.Parse(balanceString);
    }
    public static void InsertIBAN()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                connection.Open();

                Console.WriteLine(@"Enter these fields:");

                Console.Write("Country Code (e.g. \"PL\" or \"EN\") : ");
                string countryCode = Console.ReadLine().ToUpper();
                while (!ValidateCountryCode(countryCode))
                {
                    Console.Write("Invalid country code. Please enter it again: ");
                    countryCode = Console.ReadLine().ToUpper(); ;
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

                Console.Write("BBAN (Basic Bank Account Number - 16-digit): ");
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
                TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                accountHolderName = ti.ToTitleCase(accountHolderName);

                decimal a;
                string aInput;
                do
                {
                    Console.Write("Balance : ");
                    aInput = Console.ReadLine();
                } while (!decimal.TryParse(aInput, out a) || a < 0);
                a = FormatBalance(a);

                IBAN rgba = new IBAN(countryCode, checkDigits, bankSettlementNumber, bban, accountHolderName, a);
                
                string insertQuery = "INSERT INTO BankAccounts VALUES (@ni)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                SqlParameter nipParam = new SqlParameter("@ni", rgba)
                {
                    UdtTypeName = "[CLR_UDT].[dbo].[IBAN]"
                };
                insertCommand.Parameters.Add(nipParam);
                insertCommand.ExecuteNonQuery();

                Console.WriteLine("Data inserted successfully!");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error connecting to database:");
            Console.WriteLine(ex.Message); 
        }
    }

    public static void SelectIBAN()
    {
        string sql = @"
        SELECT 
            ID,
            iban.ToString() AS RGBA
        FROM BankAccounts;
    ";

        Console.WriteLine("IBAN Bank Accounts table:");

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

                                Console.WriteLine($"ID: {id}, {iban}");
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

    public static void MainAction()
    {
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
                            InsertIBAN();
                            break;
                        case 2: 
                            SelectIBAN();
                            break;
                        case 3: 
                            Search();
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

    public static void Search()
    {    
        string sql = @"
        SELECT 
            ID,
            iban.ToString() AS RGBA
        FROM BankAccounts
        WHERE iban.CountryCode = 'PL'
        ORDER BY iban.Balance;
        ";

        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                connection.Open();
                Console.WriteLine("Bank Accounts from Poland, ordered by account's balance:");
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

                                Console.WriteLine($"ID: {id}, {iban}");
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
                    iban [dbo].[IBAN]
                );
                ";
                SqlCommand createCommand = new SqlCommand(createTableQuery, connection);
                createCommand.ExecuteNonQuery();

                IBAN rgba = new IBAN("PL", "12", "12345678", "1234123412341234", "Jan Kowalski", 112.21M);
                string insertQuery = "INSERT INTO BankAccounts VALUES (@ni)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                SqlParameter niParam = new SqlParameter("@ni", rgba) { UdtTypeName = "[CLR_UDT].[dbo].[IBAN]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                rgba = new IBAN("PL", "12", "63726453", "0001982345612345", "Jakub Nowak", 304202.11M);
                insertQuery = "INSERT INTO BankAccounts VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", rgba) { UdtTypeName = "[CLR_UDT].[dbo].[IBAN]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                rgba = new IBAN("PL", "12", "12345678", "0099887766554433", "Julia Wisniewska", 4024.45M);
                insertQuery = "INSERT INTO BankAccounts VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", rgba) { UdtTypeName = "[CLR_UDT].[dbo].[IBAN]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                rgba = new IBAN("PL", "12", "12345678", "2635412387656789", "Agata Kowalczyk", 70000.00M);
                insertQuery = "INSERT INTO BankAccounts VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", rgba) { UdtTypeName = "[CLR_UDT].[dbo].[IBAN]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                rgba = new IBAN("EN", "22", "72837465", "2899809900662435", "Maja Ostatnia", 2134.99M);
                insertQuery = "INSERT INTO BankAccounts VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", rgba) { UdtTypeName = "[CLR_UDT].[dbo].[IBAN]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                Console.WriteLine("IBAN Bank Accounts reseted successfully!");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error connecting to database:");
            Console.WriteLine(ex.Message); // Display the error message for debugging
        }
    }
}

