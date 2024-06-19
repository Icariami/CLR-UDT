using NUnit.Framework;
using System;
using System.IO;
using System.Data.SqlTypes;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Microsoft.Data.SqlClient;

[TestFixture]
public class GeolocationTests
{
    [Test]
    public void TestGeolocationConstructor()
    {
        var geo = new Geolocation(45.0m, 90.0m);
        Assert.AreEqual(45.0m, geo.V1);
        Assert.AreEqual(90.0m, geo.V2);
        Assert.IsFalse(geo.IsNull);
    }

    [Test]
    public void TestGeolocationNull()
    {
        var geo = Geolocation.Null;
        Assert.IsTrue(geo.IsNull);
    }

    [Test]
    public void TestGeolocationValidation()
    {
        var geo = new Geolocation(45.0m, 90.0m);
        Assert.IsTrue(geo.Validate());

        geo = new Geolocation(-100.0m, 90.0m);
        Assert.IsFalse(geo.Validate());
    }

    [Test]
    public void TestGeolocationToString()
    {
        var geo = new Geolocation(45.0m, 90.0m);
        Assert.AreEqual("(45,0 N, 90,0 E)", geo.ToString());

        geo = new Geolocation(-45.0m, -90.0m);
        Assert.AreEqual("(45,0 S, 90,0 W)", geo.ToString());
    }

    [Test]
    public void TestGeolocationBinarySerialization()
    {
        var geo = new Geolocation(45.0m, 90.0m);

        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            geo.Write(writer);
            writer.Flush();
            ms.Position = 0;

            using (var reader = new BinaryReader(ms))
            {
                var deserializedGeo = new Geolocation();
                deserializedGeo.Read(reader);

                Assert.AreEqual(geo.V1, deserializedGeo.V1);
                Assert.AreEqual(geo.V2, deserializedGeo.V2);
            }
        }
    }

    private const string ConnectionString = "Server=(local);Database=CLR_UDT;Integrated Security=SSPI;TrustServerCertificate=True;";

    [OneTimeSetUp]
    public void Setup()
    {
        GeolocationActions.Reset();
    }

    [Test]
    public void TestInsertGeolocation()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var initialCount = GetGeolocationCount(connection);

            using (var sw = new StringWriter())
            using (var sr = new StringReader("45\n90\n"))
            {
                Console.SetOut(sw);
                Console.SetIn(sr);

                GeolocationActions.InsertGeolocation();

                Assert.IsTrue(sw.ToString().Trim().Contains("Data inserted successfully!"));
            }

            var finalCount = GetGeolocationCount(connection);
            Assert.AreEqual(initialCount + 1, finalCount);
        }
    }

    [Test]
    public void TestSelectGeolocation()
    {
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);

            GeolocationActions.SelectGeolocation();

            var output = sw.ToString().Trim();
            Assert.IsTrue(output.Contains("Geolocations table:"));
        }
    }

    [Test]
    public void TestSearchGeolocation()
    {
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);

            GeolocationActions.Search();

            var output = sw.ToString().Trim();
            Assert.IsTrue(output.Contains("Geolocations in the north-western hemisphere:"));
        }
    }

    private int GetGeolocationCount(SqlConnection connection)
    {
        using (var command = new SqlCommand("SELECT COUNT(*) FROM Geolocations", connection))
        {
            return (int)command.ExecuteScalar();
        }
    }
}
