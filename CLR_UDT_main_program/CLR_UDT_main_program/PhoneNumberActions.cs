using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class PhoneNumberActions
{
    public static bool ValidatePhoneNumber(string number)
    {
        return Regex.IsMatch(number, @"^\d{9}$");
    }
    public static bool ValidateAreaCode(string areaCode)
    {
        return Regex.IsMatch(areaCode, @"^\d{2}$");
    }

    public static void InsertNIP()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                connection.Open();

                Console.Write("Area Code (2-digit number): ");
                string areaCode = Console.ReadLine();
                while (!ValidateAreaCode(areaCode))
                {
                    Console.Write("Invalid area code. Please enter it again: ");
                    areaCode = Console.ReadLine();
                }

                Console.Write("Phone number (9-digit number) : ");
                string number = Console.ReadLine();
                while (!ValidatePhoneNumber(number))
                {
                    Console.Write("Invalid phone number. Please enter it again: ");
                    number = Console.ReadLine();
                }

                PhoneNumber phoneNumber = new PhoneNumber(areaCode, number);
                string insertQuery = "INSERT INTO PhoneNumbers VALUES (@nip)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                SqlParameter nipParam = new SqlParameter("@nip", phoneNumber)
                {
                    UdtTypeName = "[CLR_UDT].[dbo].[PhoneNumber]"
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

    public static void SelectPhoneNumber()
    {
        string sql = @"
        SELECT 
            ID,
            phoneNumber.ToString() 
        FROM PhoneNumbers;
        ";
        Console.WriteLine("Phone numbers table:");
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
                                string nums = reader.GetString(1);

                                Console.WriteLine($"ID: {id}, {nums}");
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

    public static void Search()
    {
        string sql = @"
        SELECT 
            ID,
            phoneNumber.ToString() 
        FROM PhoneNumbers
        WHERE phoneNumber.AreaCode = '48';
        ";

        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                connection.Open();
                Console.WriteLine("Phone numbers from Poland: ");
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string nip = reader.GetString(1);

                                Console.WriteLine($"ID: {id}, {nip}");
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
                            InsertNIP();
                            break;
                        case 2: 
                            SelectPhoneNumber();
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

    public static void Reset()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                connection.Open();

                string dropTableQuery = "DROP TABLE IF EXISTS PhoneNumbers";
                SqlCommand dropCommand = new SqlCommand(dropTableQuery, connection);
                dropCommand.ExecuteNonQuery();

                string createTableQuery = @"        
                CREATE TABLE PhoneNumbers
                (
                    ID int IDENTITY(1,1) PRIMARY KEY,
                    phoneNumber [dbo].[PhoneNumber]
                );
                ";
                SqlCommand createCommand = new SqlCommand(createTableQuery, connection);
                createCommand.ExecuteNonQuery();

                PhoneNumber phoneNumber = new PhoneNumber("48", "123456789");
                string insertQuery = "INSERT INTO PhoneNumbers VALUES (@ni)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                SqlParameter niParam = new SqlParameter("@ni", phoneNumber) { UdtTypeName = "[CLR_UDT].[dbo].[PhoneNumber]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                phoneNumber = new PhoneNumber("48", "634256789");
                insertQuery = "INSERT INTO PhoneNumbers VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", phoneNumber) { UdtTypeName = "[CLR_UDT].[dbo].[PhoneNumber]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                phoneNumber = new PhoneNumber("44", "909087654");
                insertQuery = "INSERT INTO PhoneNumbers VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", phoneNumber) { UdtTypeName = "[CLR_UDT].[dbo].[PhoneNumber]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                phoneNumber = new PhoneNumber("42", "423345267");
                insertQuery = "INSERT INTO PhoneNumbers VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", phoneNumber) { UdtTypeName = "[CLR_UDT].[dbo].[PhoneNumber]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                phoneNumber = new PhoneNumber("48", "900707121");
                insertQuery = "INSERT INTO PhoneNumbers VALUES (@ni)";
                insertCommand = new SqlCommand(insertQuery, connection);
                niParam = new SqlParameter("@ni", phoneNumber) { UdtTypeName = "[CLR_UDT].[dbo].[PhoneNumber]" };
                insertCommand.Parameters.Add(niParam);
                insertCommand.ExecuteNonQuery();

                Console.WriteLine("Phone numbers reseted successfully!");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error connecting to database:");
            Console.WriteLine(ex.Message); 
        }
    }
}

