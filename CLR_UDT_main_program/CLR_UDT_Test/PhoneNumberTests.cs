using NUnit.Framework;
using System;
using System.IO;
using System.Data.SqlTypes;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Microsoft.Data.SqlClient;

[TestFixture]
public class PhoneNumberTests
{
    [Test]
    public void Constructor_Default_ShouldSetIsNullToTrue()
    {
        PhoneNumber phoneNumber = new PhoneNumber();
        Assert.IsTrue(phoneNumber.IsNull);
    }

    [Test]
    public void Constructor_Parameters_ShouldSetProperties()
    {
        PhoneNumber phoneNumber = new PhoneNumber("48", "123456789");
        Assert.AreEqual("48", phoneNumber.AreaCode);
        Assert.AreEqual("123456789", phoneNumber.Number);
        Assert.IsFalse(phoneNumber.IsNull);
    }

    [Test]
    public void Validate_ValidPhoneNumber_ShouldReturnTrue()
    {
        PhoneNumber phoneNumber = new PhoneNumber("48", "123456789");
        Assert.IsTrue(phoneNumber.Validate());
    }

    [Test]
    public void Validate_InvalidPhoneNumber_ShouldReturnFalse()
    {
        PhoneNumber phoneNumber = new PhoneNumber("48", "12345");
        Assert.IsFalse(phoneNumber.Validate());
    }

    [Test]
    public void Parse_ValidString_ShouldReturnPhoneNumber()
    {
        SqlString sqlString = new SqlString("48,123456789");
        PhoneNumber phoneNumber = PhoneNumber.Parse(sqlString);
        Assert.AreEqual("48", phoneNumber.AreaCode);
        Assert.AreEqual("123456789", phoneNumber.Number);
    }

    [Test]
    public void ToString_ShouldReturnFormattedString()
    {
        PhoneNumber phoneNumber = new PhoneNumber("48", "123456789");
        string expected = "+48 123456789";
        Assert.AreEqual(expected, phoneNumber.ToString());
    }
    private string connectionString = "Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;";


    [Test]
    public void Reset_ShouldResetDatabase()
    {
        PhoneNumberActions.Reset();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT COUNT(*) FROM PhoneNumbers";
            SqlCommand command = new SqlCommand(query, connection);
            int count = (int)command.ExecuteScalar();
            Assert.AreEqual(5, count); // Ensure 5 records are inserted by reset
        }
    }

    [Test]
    public void ValidatePhoneNumber_ValidPhoneNumber_ShouldReturnTrue()
    {
        Assert.IsTrue(PhoneNumberActions.ValidatePhoneNumber("123456789"));
    }

    [Test]
    public void ValidatePhoneNumber_InvalidPhoneNumber_ShouldReturnFalse()
    {
        Assert.IsFalse(PhoneNumberActions.ValidatePhoneNumber("12345"));
    }

    [Test]
    public void ValidateAreaCode_ValidAreaCode_ShouldReturnTrue()
    {
        Assert.IsTrue(PhoneNumberActions.ValidateAreaCode("48"));
    }

    [Test]
    public void ValidateAreaCode_InvalidAreaCode_ShouldReturnFalse()
    {
        Assert.IsFalse(PhoneNumberActions.ValidateAreaCode("4"));
    }
}
