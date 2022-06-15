using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Enums
{
    public class EnumContentType
    {
        public string CssClass { get; private set; } = "";

        private EnumContentType(string cssClass)
        {
            this.CssClass = cssClass;
        }

        public string GetGridCssClass()
        {
            return "grid_" + this.CssClass;
        }

        public static EnumContentType Text
        {
            get
            {
                return new EnumContentType("content_text");
            }
        }

        public static EnumContentType Password
        {
            get
            {
                return new EnumContentType("content_password");
            }
        }

        public static EnumContentType EmailAddress
        {
            get
            {
                return new EnumContentType("content_emailaddress");
            }
        }

        public static EnumContentType PhoneNumber
        {
            get
            {
                return new EnumContentType("content_phonenumber");
            }
        }

        public static EnumContentType Number
        {
            get
            {
                return new EnumContentType("content_number");
            }
        }

        public static EnumContentType Currency
        {
            get
            {
                return new EnumContentType("content_currency");
            }
        }

        public static EnumContentType Boolean
        {
            get
            {
                return new EnumContentType("content_boolean");
            }
        }

        public static EnumContentType Binary
        {
            get
            {
                return new EnumContentType("content_binary");
            }
        }
    }
}
