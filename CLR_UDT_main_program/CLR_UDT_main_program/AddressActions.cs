using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class AddressActions
{

    public static bool ValidateZipCode(string zipCode)
    {
        return Regex.IsMatch(zipCode, @"^\d{2}-\d{3}$");
    }
    
    public static bool ValidateCityAndCountry(string name)
    {
        return Regex.IsMatch(name, @"^[A-Z][a-z]+$");
    }

    

    public static void InsertAddress()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection("Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;"))
            {
                

                Console.WriteLine("Enter these fields:");
                Console.Write("Street name : ");
                string streetName = Console.ReadLine();

                Console.Write("Building number : ");
                string bnr = Console.ReadLine();

                Console.Write("Apartment number : ");
                string anr = Console.ReadLine();

                Console.Write("Zip code : ");
                string zipCode = Console.ReadLine();
                while (!ValidateZipCode(zipCode))
                {
                    Console.Write("Invalid zip code. Please enter it again: ");
                    zipCode = Console.ReadLine();
                }


                
                

                Console.Write("City : ");
                string city = Console.ReadLine();
                while (!ValidateCityAndCountry(city))
                {
                    Console.Write("Invalid city name. Please enter it again: ");
                    city = Console.ReadLine();
                }

                Console.Write("Country : ");
                string country = Console.ReadLine();
                while (!ValidateCityAndCountry(country))
                {
                    Console.Write("Invalid country name. Please enter it again: ");
                    country = Console.ReadLine();
                }

               

                connection.Open();
                Address address = new Address(streetName, bnr, anr, zipCode, city, country);
                string insertQuery = "INSERT INTO Addresses VALUES (@addres)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                SqlParameter addressParam = new SqlParameter("@addres", address)
                {
                    SqlDbType = SqlDbType.Udt,
                    UdtTypeName = "[CLR_UDT].[dbo].[Address]"
                };
                insertCommand.Parameters.Add(addressParam);
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

    public static void SelectAddresses()
    {
        string sql = @"
        SELECT 
            ID,
            addres.ToString() AS address
        FROM Addresses;
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
                                string address = reader.GetString(1);

                                Console.WriteLine($"ID: {id}, Address: {address}");
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
                            InsertAddress();
                            break;
                        case 2: // select data
                            SelectAddresses();
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

    }
}

