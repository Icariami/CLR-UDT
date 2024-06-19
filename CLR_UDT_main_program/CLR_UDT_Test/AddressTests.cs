using NUnit.Framework;
using System.Data.SqlTypes;
using System.IO;
using System.Text;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

[TestFixture]
public class AddressTests
{
    [Test]
    public void TestAddressInitialization()
    {
        Address address = new Address("PlaceName", "Street", "123", "45", "12-345", "City", "Country");

        Assert.AreEqual("PlaceName", address.PlaceName);
        Assert.AreEqual("Street", address.Street);
        Assert.AreEqual("123", address.BuildingNumber);
        Assert.AreEqual("45", address.ApartmentNumber);
        Assert.AreEqual("12-345", address.ZipCode);
        Assert.AreEqual("City", address.City);
        Assert.AreEqual("Country", address.Country);
        Assert.IsFalse(address.IsNull);
    }

    [Test]
    public void TestAddressToString()
    {
        Address address = new Address("PlaceName", "Street", "123", "45", "12-345", "City", "Country");
        string expectedOutput = "PlaceName\nStreet 123 / 45\n12-345, City, Country";
        Assert.AreEqual(expectedOutput, address.ToString());
    }

    [Test]
    public void TestAddressSerialization()
    {
        Address address = new Address("PlaceName", "Street", "123", "45", "12-345", "City", "Country");

        using (MemoryStream ms = new MemoryStream())
        {
            BinaryWriter writer = new BinaryWriter(ms, Encoding.UTF8, true);
            address.Write(writer);

            ms.Position = 0;
            BinaryReader reader = new BinaryReader(ms, Encoding.UTF8);

            Address deserializedAddress = new Address();
            deserializedAddress.Read(reader);

            Assert.AreEqual(address.PlaceName, deserializedAddress.PlaceName);
            Assert.AreEqual(address.Street, deserializedAddress.Street);
            Assert.AreEqual(address.BuildingNumber, deserializedAddress.BuildingNumber);
            Assert.AreEqual(address.ApartmentNumber, deserializedAddress.ApartmentNumber);
            Assert.AreEqual(address.ZipCode, deserializedAddress.ZipCode);
            Assert.AreEqual(address.City, deserializedAddress.City);
            Assert.AreEqual(address.Country, deserializedAddress.Country);
            Assert.AreEqual(address.IsNull, deserializedAddress.IsNull);
        }
    }

    [Test]
    public void TestValidateZipCode()
    {
        Assert.IsTrue(AddressActions.ValidateZipCode("12-345"));
        Assert.IsFalse(AddressActions.ValidateZipCode("12345"));
        Assert.IsFalse(AddressActions.ValidateZipCode("12-34a"));
    }

    [Test]
    public void TestValidateCityAndCountry()
    {
        Assert.IsTrue(AddressActions.ValidateCityAndCountry("city"));
        Assert.IsFalse(AddressActions.ValidateCityAndCountry("City"));
        Assert.IsFalse(AddressActions.ValidateCityAndCountry("city123"));
    }

    [Test]
    public void TestValidateBuildingNumber()
    {
        Assert.IsTrue(AddressActions.ValidateBuildingNumber("123"));
        Assert.IsFalse(AddressActions.ValidateBuildingNumber("-1"));
        Assert.IsFalse(AddressActions.ValidateBuildingNumber("abc"));
    }

    [Test]
    public void TestValidateApartmentNumber()
    {
        Assert.IsTrue(AddressActions.ValidateApartmentNumber("123"));
        Assert.IsTrue(AddressActions.ValidateApartmentNumber("-1"));
        Assert.IsFalse(AddressActions.ValidateApartmentNumber("abc"));
    }

}
