using System;
using System.Data;
using System.Data.SqlTypes;
using System.Xml.Serialization;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;
using System.IO;


[Serializable]
[SqlUserDefinedType(Format.UserDefined, MaxByteSize = 85)]
public class IBAN : INullable, IBinarySerialize
{
    private string countryCode; // "PL"
    private string checkDigits; // "12", 2-digit number
    private string bankSettlementNumber; // 8-digit number
    private string bban; // 16-digit number
    private string accountHolderName;
    private decimal balance;
    private bool isNull;

    public IBAN()
    {
        isNull = true;
    }

    public IBAN(string countryCode, string checkDigits, string bankSettlementNumber, string bban, string accountHolderName, decimal balance)
    {
        this.countryCode = countryCode;
        this.balance = balance;
        this.checkDigits = checkDigits;
        this.bankSettlementNumber = bankSettlementNumber;
        this.bban = bban;
        this.accountHolderName = accountHolderName;
        isNull = false;
    }

    public static IBAN Null
    {
        get
        {
            IBAN n = new();
            
            return n;
        }
    }

    public decimal Balance { get => balance; private set => balance = value; }
    public string CountryCode { get => countryCode; set => countryCode = value; }
    public string CheckDigits { get => checkDigits; set => checkDigits = value; }
    public string BankSettlementNumber { get => bankSettlementNumber; set => bankSettlementNumber = value; }
    public string Bban { get => bban; set => bban = value; }
    public string AccountHolderName { get => accountHolderName; set => accountHolderName = value; }
    public bool IsNull { get => isNull; set => isNull = value; }

    public bool Validate()
    {
        if (!Regex.IsMatch(countryCode, @"^[A-Z]{2}$")) return false;       
        if (!Regex.IsMatch(checkDigits, @"^\d{2}$")) return false;
        if (!Regex.IsMatch(bankSettlementNumber, @"^\d{8}$")) return false;
        if (!Regex.IsMatch(bban, @"^\d{16}$")) return false;
        if (!Regex.IsMatch(accountHolderName, @"^[a-zA-Z '-]+$")) return false;
        return true;
    }

    public static IBAN Parse(SqlString s)
    {
        if(s.IsNull)
        {
            return new IBAN();
        }

        return new IBAN("PL", "12", "12345678", "1234123412341234", "Jan Kowalski", 0.1M);
    }

    public void Read(BinaryReader r)
    {
        countryCode = r.ReadString();
        checkDigits = r.ReadString();
        bankSettlementNumber = r.ReadString();
        bban = r.ReadString();
        accountHolderName = r.ReadString();
        balance = r.ReadDecimal();
    }

    public void Write(BinaryWriter w)
    {
        w.Write(countryCode);
        w.Write(checkDigits);
        w.Write(bankSettlementNumber);
        w.Write(bban);
        w.Write(accountHolderName);
        w.Write(balance);
    }
    public override string ToString()
    {
        return $"{countryCode} {checkDigits} {bankSettlementNumber} {bban} {balance} {accountHolderName}";
    }
}

