using Microsoft.Data.SqlClient;
using NUnit.Framework;
using System.Data.SqlTypes;
using System.IO;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

[TestFixture]
public class NIPTests
{
    [Test]
    public void Constructor_Default_ShouldSetIsNullToTrue()
    {
        NIP nip = new NIP();
        Assert.IsTrue(nip.IsNull);
    }

    [Test]
    public void Constructor_Parameters_ShouldSetProperties()
    {
        NIP nip = new NIP("1234567890", "Test Firm");
        Assert.AreEqual("1234567890", nip.Nip);
        Assert.AreEqual("Test Firm", nip.FirmName);
        Assert.IsFalse(nip.IsNull);
    }

    private string connectionString = "Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;";

    [SetUp]
    public void SetUp()
    {
        // Reset the database before each test
        NipActions.Reset();
    }

    [Test]
    public void InsertNIP_ValidData_ShouldInsertRecord()
    {
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);

            using (StringReader sr = new StringReader("Test Firm\n1234567890\n"))
            {
                Console.SetIn(sr);
                NipActions.InsertNIP();
            }

            string expected = "Data inserted successfully!";
            Assert.IsTrue(sw.ToString().Contains(expected));
        }

        // Verify the record was inserted
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT COUNT(*) FROM NIPs WHERE nip.Nip = '1234567890' AND nip.FirmName = 'Test Firm'";
            SqlCommand command = new SqlCommand(query, connection);
            int count = (int)command.ExecuteScalar();
            Assert.AreEqual(1, count);
        }
    }
}
