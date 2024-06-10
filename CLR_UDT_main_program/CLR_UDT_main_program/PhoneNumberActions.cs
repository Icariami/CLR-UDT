using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    public static void InsertPhoneNumber()
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
                string insertQuery = "INSERT INTO PhoneNumbers VALUES (@phoneNumber)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                SqlParameter phoneNumberParam = new SqlParameter("@phoneNumber", phoneNumber)
                {
                    SqlDbType = SqlDbType.Udt,
                    UdtTypeName = "[CLR_UDT].[dbo].[PhoneNumber]"
                };
                insertCommand.Parameters.Add(phoneNumberParam);
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

    public static void SelectPhoneNumbers()
    {
        string sql = @"
        SELECT 
            ID,
            phoneNumber.ToString() AS phone_number
        FROM PhoneNumbers;
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
                                string phoneNumber = reader.GetString(1);

                                Console.WriteLine($"ID: {id}, Phone number: {phoneNumber}");
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
}

