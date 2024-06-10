using System;
using System.Data;
using System.Data.SqlTypes;
using System.Xml.Serialization;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;
using System.IO;

namespace CLR_UDT
{
    [Serializable]
    [SqlUserDefinedType(Format.UserDefined, MaxByteSize = 85)]
    internal class PhoneNumber : INullable, IBinarySerialize
    {
        private string areaCode;
        private string number;
        private bool isNull;

        public PhoneNumber()
        {
            isNull = true;
            areaCode = " ";
            number = " ";
        }

        public PhoneNumber(string areaCode, string number)
        {
            this.areaCode = areaCode;
            this.number = number;
            isNull = false;
        }

        public string AreaCode { get; set; }
        public string Number { get; set; }
        public bool IsNull { get { return isNull; } }

        public static PhoneNumber Null
        {
            get
            {
                PhoneNumber p = new PhoneNumber();
                return p;
            }
        }

        public bool Validate()
        {
            if (!Regex.IsMatch(areaCode, @"^\d{2}$"))
            {
                return false;
            }
            if (!Regex.IsMatch(number, @"^\d{9}$"))
            {
                return false;
            }
            return true;
        }

        public static PhoneNumber Parse(SqlString s)
        {
            if (s.IsNull)
            {
                return new PhoneNumber();
            }

            var values = s.Value.Split(',');

            if (values.Length != 2)
            {
                throw new ArgumentException("Invalid Phone Number data format");
            }

            string areaCode = values[0];
            string number = values[1];

            return new PhoneNumber(areaCode, number);
        }

        public override string ToString()
        {
            return $"+{areaCode} {number}";
        }

        public SqlString ToSqlString()
        {
            return new SqlString($"+{areaCode} {number}");
        }

        //public SqlMetaData[] GetSqlMetaData()
        //{
        //    var metaData = new SqlMetaData[2];

        //    metaData[0] = new SqlMetaData("AreaCode", SqlDbType.NVarChar, 2, 4 + 2);
        //    metaData[1] = new SqlMetaData("Number", SqlDbType.NVarChar, 9, 9 + 4);

        //    return metaData;
        //}

        public void Read(BinaryReader r)
        {
            areaCode = new string(r.ReadChars(2));
            number = new string(r.ReadChars(9));
        }

        public void Write(BinaryWriter w)
        {
            w.Write(areaCode.ToCharArray());
            w.Write(number.ToCharArray());
        }

    }
}
