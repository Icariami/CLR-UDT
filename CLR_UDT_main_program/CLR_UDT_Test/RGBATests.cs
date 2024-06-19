using Microsoft.Data.SqlClient;
using NUnit.Framework;
using System;
using System.Data.SqlTypes;
using System.IO;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

[TestFixture]
public class RGBATests
{
    [Test]
    public void TestNullRGBAColor()
    {
        var rgba = RGBAColor.Null;
        Assert.IsTrue(rgba.IsNull);
    }

    [Test]
    public void TestValidateValidColor()
    {
        var rgba = new RGBAColor(100, 150, 200, 0.5M);
        Assert.IsTrue(rgba.Validate());
    }

    [Test]
    public void TestValidateInvalidColor()
    {
        var rgba = new RGBAColor(300, 150, 200, 0.5M);
        Assert.IsFalse(rgba.Validate());
    }

    [Test]
    public void TestToString()
    {
        var rgba = new RGBAColor(100, 150, 200, 0.5M);
        Assert.AreEqual("(100, 150, 200, 0,5)", rgba.ToString());
    }

    [Test]
    public void TestBinarySerialization()
    {
        var rgba = new RGBAColor(100, 150, 200, 0.5M);

        using (MemoryStream ms = new MemoryStream())
        using (BinaryWriter writer = new BinaryWriter(ms))
        {
            rgba.Write(writer);
            writer.Flush();

            ms.Position = 0;

            using (BinaryReader reader = new BinaryReader(ms))
            {
                var rgbaDeserialized = new RGBAColor();
                rgbaDeserialized.Read(reader);

                Assert.AreEqual(rgba.R, rgbaDeserialized.R);
                Assert.AreEqual(rgba.G, rgbaDeserialized.G);
                Assert.AreEqual(rgba.B, rgbaDeserialized.B);
                Assert.AreEqual(rgba.A, rgbaDeserialized.A);
            }
        }
    }

    private const string connectionString = "Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;";

    [Test]
    public void TestSearch()
    {
        RGBAColorActions.Reset();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT COUNT(*) FROM RGBAs WHERE rgba.A < 0.5";
            SqlCommand command = new SqlCommand(query, connection);
            int count = (int)command.ExecuteScalar();
            Assert.AreEqual(3, count);
        }
    }

    [Test]
    public void TestSelectRGBA()
    {
        RGBAColorActions.Reset();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT COUNT(*) FROM RGBAs";
            SqlCommand command = new SqlCommand(query, connection);
            int count = (int)command.ExecuteScalar();
            Assert.AreEqual(5, count);
        }
    }
}
