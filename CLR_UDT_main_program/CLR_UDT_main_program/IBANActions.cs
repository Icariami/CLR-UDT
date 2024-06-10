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
    public static void MainIBANActions()
    { 

        string connectionString = "Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;";
        string countryCode = "PL";
        string checkDigits = "12";
        string bankSettlementNumber = "34567890";
        string bban = "1234567890123456";
        string accountHolderName = "Jan Kowalski";
        decimal balance = 1234.56M;

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Connection established successfully!");
               
                IBANAccountNumber iban = new IBANAccountNumber(countryCode, checkDigits, bankSettlementNumber, bban, accountHolderName, balance);

                string insertQuery = "INSERT INTO BankAccounts VALUES (@iban)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);

                SqlParameter ibanParam = new SqlParameter("@iban", iban) { UdtTypeName = "[CLR_UDT].[dbo].[IBANAccountNumber]" }; 
                insertCommand.Parameters.Add(ibanParam);

                insertCommand.ExecuteNonQuery();

                Console.WriteLine("Data inserted successfully!");

                SelectBankAccount(connection);

                Console.ReadLine();

            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error connecting to database:");
            Console.WriteLine(ex.Message); // Display the error message for debugging
        }
    }

    public static void InsertBankAccount(SqlConnection connection, IBANAccountNumber iban)
    {
        string sql = "INSERT INTO BankAccounts VALUES (@iban)";
        using (SqlCommand command = new SqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@iban", iban);
            command.ExecuteNonQuery();
        }
    }

    public static void SelectBankAccount(SqlConnection connection)
{
    string sql = @"
        SELECT 
            ID,
            iban.ToString() AS IBAN
        FROM BankAccounts;
    ";

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
