using System;
using System.Data;
using System.Data.SqlTypes;
using System.Xml.Serialization;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;
using System.IO;


[Serializable]
[SqlUserDefinedType(Format.UserDefined, MaxByteSize = 85)]
public class IBANAccountNumber : INullable, IBinarySerialize
{
    private string countryCode; // 2 letters, like pl
    private string checkDigits; // 2 digits
    private string bankSettlementNumber; // 8 digits
    private string bban; // 16 digit account number
    private string accountHolderName;
    private decimal balance;
    private bool isNull;

    public IBANAccountNumber()
    {
        IsNull = true;
        accountHolderName = " ";
        countryCode = " ";
        checkDigits = " ";
        bankSettlementNumber = " ";
        bban = " ";
    }

    public IBANAccountNumber(string countryCode, string checkDigits, string bankSettlementNumber,  string bban, string accountHolderName, decimal balance)
    {
        this.countryCode = countryCode;
        this.checkDigits = checkDigits;
        this.bankSettlementNumber = bankSettlementNumber;
        this.bban = bban;
        this.accountHolderName = accountHolderName;
        this.balance = balance;
        IsNull = false;
    }

    public bool Validate()
    {
        if (!Regex.IsMatch(countryCode, @"^[A-Z]{2}$"))
        {
            return false;
        }
        if (!Regex.IsMatch(checkDigits, @"^\d{2}$"))
        {
            return false;
        }
        if (!Regex.IsMatch(bankSettlementNumber, @"^\d{8}$"))
        {
            return false;
        }
        if (!Regex.IsMatch(bban, @"^\d{16}$"))
        {
            return false;
        }
        if (!Regex.IsMatch(accountHolderName, @"^[a-zA-Z '-]+$"))
        {
            return false;
        }

        return true;
    }

public static IBANAccountNumber Parse(SqlString s)
{
    if (s.IsNull)
    {
        return new IBANAccountNumber();
    }

    var values = s.Value.Split(',');

    if (values.Length != 3)
    {
        throw new ArgumentException("Invalid IBAN data format");
    }

    //string iban = values[0];
    //string countryCode = iban.Substring(0, 2);
    //string checkDigits = iban.Substring(2, 2);
    //string bankSettlementNumber = iban.Substring(4, 8);
    //string bban = iban.Substring(12, 16);
    //string accountHolderName = values[1];
    //decimal balance = decimal.Parse(values[2]);

    //return new IBANAccountNumber(countryCode, checkDigits, bankSettlementNumber, bban, accountHolderName, balance);
    return new IBANAccountNumber("PL", "32", "12345678", "1234123412341234", "Gosia", 234.32M);
}


    public override string ToString()
    {
        return $"{countryCode} {checkDigits} {bankSettlementNumber} {bban}, Account Holder Name: {accountHolderName}, Balance: {balance}";
    }


    public static IBANAccountNumber Null
    {
        get
        {
            IBANAccountNumber h = new()
            {
                IsNull = true
            };
            return h;
        }
    }

    public bool IsNull { get => isNull; set => isNull = value; }

    public void Read(BinaryReader r)
    {
        int nameLength = r.ReadInt32(); 
        accountHolderName = new string(r.ReadChars(nameLength)); 
        countryCode = new string(r.ReadChars(2)); 
        checkDigits = new string(r.ReadChars(2)); 
        bankSettlementNumber = new string(r.ReadChars(8)); 
        bban = new string(r.ReadChars(16));
        balance = r.ReadDecimal();
    }

    public void Write(BinaryWriter w)
    {
        w.Write(accountHolderName.Length); 
        w.Write(accountHolderName.ToCharArray()); 
        w.Write(countryCode.ToCharArray()); 
        w.Write(checkDigits.ToCharArray()); 
        w.Write(bankSettlementNumber.ToCharArray()); 
        w.Write(bban.ToCharArray()); 
        w.Write(balance);
    }
}

