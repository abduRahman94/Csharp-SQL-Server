using System;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace ProceduresDB
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [SqlUserDefinedType(Format.UserDefined, IsByteOrdered = true, MaxByteSize = 8000, Name = "Product", ValidationMethodName = "ValidateProduct")]
    public class Product : INullable, IBinarySerialize
    {
        private string _name;
        private string _color;
        private string _number;
        private int _size;
        private bool _isNull;

        public Product()
        {
        }

        public static Product Null
        {
            get
            {
                Product product = new Product();
                product.IsNull = true;
                return product;
            }
        }

        [SqlMethod(OnNullCall = false)]
        public static Product Parse(SqlString s)
        {
            if (s.IsNull)
            {
                return Null;
            }

            // string[] data = s.Value.Trim('{', '}').Split(",".ToCharArray());
            string[] data = s.Value.Split(";".ToCharArray());
            Product product = new Product();
            product.Name = data[0];
            product.Color = data[1];
            product.Number = data[2];
            product.Size = Convert.ToInt32(data[3]);
            product.IsNull = false;
            return product;
          
        }

        public override string ToString()
        {
            if (IsNull)
            {
                return "NULL";
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("{");
                builder.Append($"\"name\": \"{Name}\"");
                builder.Append(",");
                builder.Append($"\"color\": \"{Color}\"");
                builder.Append(",");
                builder.Append($"\"number\": \"{Number}\"");
                builder.Append(",");
                builder.Append($"\"size\": {Size}");
                builder.Append("}");
                return builder.ToString();
            }
        }

        private bool ValidateProduct()
        {
            if (!IsNull)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Read(BinaryReader r)
        {
            Name = r.ReadString();
            Color = r.ReadString();
            Number = r.ReadString();
            Size = r.ReadInt32();
            IsNull = r.ReadBoolean();
        }

        public void Write(BinaryWriter w)
        {
            w.Write(Name);
            w.Write(Color);
            w.Write(Number);
            w.Write(Size);
            w.Write(IsNull);
        }

        public string Name { get; set; }

        public string Color { get; set; }

        public string Number { get; set; }

        public int Size { get; set; }

        public bool IsNull { get; set; }

    }

}
