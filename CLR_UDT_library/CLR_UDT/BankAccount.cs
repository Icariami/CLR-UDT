using System;
using System.Data;
using System.Data.SqlTypes;
using System.Xml.Serialization;
using Microsoft.SqlServer.Server;

namespace CLR_UDT
{

    [Serializable]
    [SqlUserDefinedType(Format.UserDefined)]
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
            isNull = true;
            accountHolderName = "";
        }

        public IBANAccountNumber(string countryCode, string checkDigits, string bankSettlementNumber,  string bban, string accountHolderName, decimal balance)
        {
            this.countryCode = countryCode;
            this.checkDigits = checkDigits;
            this.bankSettlementNumber = bankSettlementNumber;
            this.bban = bban;
            this.accountHolderName = accountHolderName;
            this.balance = balance;
            isNull = false;
        }

        public string CountryCode
        {
            get;
            set;
        }

        public string CheckDigits
        {
            get;
            set;
        }

        public string BankSettlementNumber
        {
            get;
            set;
        }

        public string Bban
        {
            get;
            set;
        }

        public string AccountHolderName
        {
            get { return accountHolderName; }
            set { accountHolderName = value; }
        }

        public decimal Balance
        {
            get { return balance; }
            set { balance = value; }
        }

        public bool IsNull { get; private set; }

        public override string ToString()
        {
            return $"IBAN: {CountryCode}{checkDigits}{bankSettlementNumber} {bban}, Account Holder Name: {AccountHolderName}, Balance: {Balance}";
        }


        public static IBANAccountNumber Null
        {
            get
            {
                IBANAccountNumber h = new()
                {
                    isNull = true
                };
                return h;
            }
        }

        public SqlString ToSqlString()
        {
            return new SqlString($"({countryCode}{checkDigits}{bankSettlementNumber}{bban},{SqlString.Null},{SqlString.Null},{balance})");
        }

        public SqlMetaData[] GetSqlMetaData()
        {
            var metaData = new SqlMetaData[6];

            metaData[0] = new SqlMetaData("AccountHolderName", SqlDbType.NVarChar, 50, 4 + 50);  // 4 bytes for length + 50 for string
            metaData[1] = new SqlMetaData("Balance", SqlDbType.Decimal, 18, 8);  // 8 bytes for decimal

            metaData[2] = new SqlMetaData("CountryCode", SqlDbType.NVarChar, 2, 2 + 4); // 2 bytes for code + 4 bytes for length
            metaData[3] = new SqlMetaData("CheckDigits", SqlDbType.NVarChar, 2, 2 + 4); // 2 bytes for digits + 4 bytes for length
            metaData[4] = new SqlMetaData("BankSettlementNumber", SqlDbType.NVarChar, 8, 8 + 4);
            metaData[5] = new SqlMetaData("BBAN", SqlDbType.NVarChar, 30, 30 + 4); // 30 bytes for BBAN + 4 bytes for length

            return metaData;
        }

        public void ReadSqlValue(SqlString s)
        {
            var values = s.Value.Split(',');

            // Parse IBAN components (assuming format: countryCode+checkDigits+bban)
            string iban = values[0];
            countryCode = iban.Substring(0, 2);
            checkDigits = iban.Substring(2, 2);
            bankSettlementNumber = iban.Substring(4, 8);
            bban = iban.Substring(12);

            AccountHolderName = values[1];
            Balance = decimal.Parse(values[3]);
        }

        public void WriteSqlValue(SqlString s)
        {
            s = $"{countryCode}{checkDigits}{bankSettlementNumber}{bban},{SqlString.Null},{SqlString.Null},{balance}";
        }

        public void Read(BinaryReader r)
        {
            // Read Account Holder Name (String)
            int nameLength = r.ReadInt32(); // Read the length of the string first
            accountHolderName = new string(r.ReadChars(nameLength)); // Read the characters based on length
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
}
