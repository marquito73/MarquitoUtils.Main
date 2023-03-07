using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Enums
{
    public class EnumContentTypeAttr : EnumClass
    {
        public string CssClass { get; private set; } = "";

        public EnumContentTypeAttr(string cssClass)
        {
            this.CssClass = cssClass;
        }

        public string GetGridCssClass()
        {
            return "grid_" + this.CssClass;
        }
    }

    public static class EnumContentTypes
    {
        public static EnumContentTypeAttr Attr(this EnumContentType contentType)
        {
            return EnumUtils.GetAttr<EnumContentTypeAttr, EnumContentType>(contentType);
        }

        public static string GetCssClass(this EnumContentType contentType)
        {
            EnumContentTypeAttr contentTypeAttr 
                = EnumUtils.GetAttr<EnumContentTypeAttr, EnumContentType>(contentType);
            return Attr(contentType).CssClass;
        }

        public static string GetGridCssClass(this EnumContentType contentType)
        {
            return "grid_" + GetCssClass(contentType);
        }
    }

    [DataContract]
    public enum EnumContentType
    {
        [EnumMember]
        [EnumContentTypeAttr("content_text")] Text,
        [EnumMember]
        [EnumContentTypeAttr("content_password")] Password,
        [EnumMember]
        [EnumContentTypeAttr("content_emailaddress")] EmailAddress,
        [EnumMember]
        [EnumContentTypeAttr("content_phonenumber")] PhoneNumber,
        [EnumMember]
        [EnumContentTypeAttr("content_number")] Number,
        [EnumMember]
        [EnumContentTypeAttr("content_currency")] Currency,
        [EnumMember]
        [EnumContentTypeAttr("content_boolean")] Boolean,
        [EnumMember]
        [EnumContentTypeAttr("content_binary")] Binary,
        [EnumMember]
        [EnumContentTypeAttr("content_date")] Date,
        [EnumMember]
        [EnumContentTypeAttr("content_time")] Time
    }
}
