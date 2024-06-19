using NUnit.Framework;
using System;
using System.IO;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Microsoft.Data.SqlClient;

[TestFixture]
public class IBANTests
{
    [Test]
    public void TestIBANConstructor()
    {
        var iban = new IBAN("PL", "12", "12345678", "1234123412341234", "Jan Kowalski", 100.50m);
        Assert.AreEqual("PL", iban.CountryCode);
        Assert.AreEqual("12", iban.CheckDigits);
        Assert.AreEqual("12345678", iban.BankSettlementNumber);
        Assert.AreEqual("1234123412341234", iban.Bban);
        Assert.AreEqual("Jan Kowalski", iban.AccountHolderName);
        Assert.AreEqual(100.50m, iban.Balance);
        Assert.IsFalse(iban.IsNull);
    }

    [Test]
    public void TestIBANNull()
    {
        var iban = IBAN.Null;
        Assert.IsTrue(iban.IsNull);
    }

    [Test]
    public void TestIBANValidation()
    {
        var iban = new IBAN("PL", "12", "12345678", "1234123412341234", "Jan Kowalski", 100.50m);
        Assert.IsTrue(iban.Validate());

        iban = new IBAN("PL", "1", "12345678", "1234123412341234", "Jan Kowalski", 100.50m);
        Assert.IsFalse(iban.Validate());
    }

    [Test]
    public void TestIBANParse()
    {
        var iban = IBAN.Parse(new SqlString("PL 12 12345678 1234123412341234 0.1 Jan Kowalski"));
        Assert.AreEqual("PL", iban.CountryCode);
        Assert.AreEqual("12", iban.CheckDigits);
        Assert.AreEqual("12345678", iban.BankSettlementNumber);
        Assert.AreEqual("1234123412341234", iban.Bban);
        Assert.AreEqual("Jan Kowalski", iban.AccountHolderName);
        Assert.AreEqual(0.1m, iban.Balance);
    }

    [Test]
    public void TestIBANToString()
    {
        var iban = new IBAN("PL", "12", "12345678", "1234123412341234", "Jan Kowalski", 100.50m);
        Assert.AreEqual("PL 12 12345678 1234123412341234 100,50 Jan Kowalski", iban.ToString());
    }

    [Test]
    public void TestIBANBinarySerialization()
    {
        var iban = new IBAN("PL", "12", "12345678", "1234123412341234", "Jan Kowalski", 100.50m);

        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            iban.Write(writer);
            writer.Flush();
            ms.Position = 0;

            using (var reader = new BinaryReader(ms))
            {
                var deserializedIban = new IBAN();
                deserializedIban.Read(reader);

                Assert.AreEqual(iban.CountryCode, deserializedIban.CountryCode);
                Assert.AreEqual(iban.CheckDigits, deserializedIban.CheckDigits);
                Assert.AreEqual(iban.BankSettlementNumber, deserializedIban.BankSettlementNumber);
                Assert.AreEqual(iban.Bban, deserializedIban.Bban);
                Assert.AreEqual(iban.AccountHolderName, deserializedIban.AccountHolderName);
                Assert.AreEqual(iban.Balance, deserializedIban.Balance);
            }
        }
    }

    private const string ConnectionString = "Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;";

    [OneTimeSetUp]
    public void Setup()
    {
        IBANActions.Reset();
    }

    [Test]
    public void TestSelectIBAN()
    {
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);

            IBANActions.SelectIBAN();

            var output = sw.ToString().Trim();
            Assert.IsTrue(output.Contains("IBAN Bank Accounts table:"));
        }
    }

    [Test]
    public void TestSearchIBAN()
    {
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);

            IBANActions.Search();

            var output = sw.ToString().Trim();
            Assert.IsTrue(output.Contains("Bank Accounts from Poland, ordered by account's balance:"));
        }
    }

    [Test]
    public void TestResetIBAN()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            IBANActions.Reset();

            var count = GetIBANCount(connection);
            Assert.AreEqual(5, count); 
        }
    }

    private int GetIBANCount(SqlConnection connection)
    {
        using (var command = new SqlCommand("SELECT COUNT(*) FROM BankAccounts", connection))
        {
            return (int)command.ExecuteScalar();
        }
    }
}
