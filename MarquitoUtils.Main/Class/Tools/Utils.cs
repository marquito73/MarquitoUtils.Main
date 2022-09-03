using MarquitoUtils.Main.Class.Entities.Image;
using MarquitoUtils.Main.Class.Entities.Param;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace MarquitoUtils.Main.Class.Tools
{
    /// <summary>
    /// A class with a lot of useful things
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Get a correct string datetime format
        /// </summary>
        /// <param name="dtToConvert">The datetime to convert</param>
        /// <returns>The datetime formatted as a string (YYYYMMDD_hhmmss)</returns>
        public static string GetCorrectStringDtFormat(DateTime dtToConvert)
        {
            StringBuilder sbDateTimeStringFormat = new StringBuilder();

            // 20210101_000000
            sbDateTimeStringFormat.Append(dtToConvert.Year.ToString().Trim().PadLeft(4, '0'));
            sbDateTimeStringFormat.Append(dtToConvert.Month.ToString().Trim().PadLeft(2, '0'));
            sbDateTimeStringFormat.Append(dtToConvert.Day.ToString().Trim().PadLeft(2, '0'));
            sbDateTimeStringFormat.Append("_");
            sbDateTimeStringFormat.Append(dtToConvert.Hour.ToString().Trim().PadLeft(2, '0'));
            sbDateTimeStringFormat.Append(dtToConvert.Minute.ToString().Trim().PadLeft(2, '0'));
            sbDateTimeStringFormat.Append(dtToConvert.Second.ToString().Trim().PadLeft(2, '0'));

            return sbDateTimeStringFormat.ToString();
        }

        /// <summary>
        /// Allows to know if an object is null
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>Boolean indicate if the object is null</returns>
        public static bool IsNull(object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// Allows to know if an object is not null
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>Boolean indicate if the object is not null</returns>
        public static bool IsNotNull(object obj)
        {
            return !IsNull(obj);
        }

        /// <summary>
        /// Allows to know if a string is null or empty
        /// </summary>
        /// <param name="data">The string</param>
        /// <returns>Boolean indicate if the string is null or empty</returns>
        public static bool IsNullOrEmpty(string data)
        {
            return string.IsNullOrEmpty(data);
        }

        public static bool IsEmpty(string data)
        {
            return string.IsNullOrEmpty(data);
        }

        public static bool IsEmpty<T>(IEnumerable<T> data)
        {
            return IsNull(data) || data.Count().Equals(0);
        }

        public static bool IsEmpty<T, U>(IDictionary<T, U> data)
        {
            return IsNull(data) || data.Count().Equals(0);
        }

        public static bool IsNotEmpty(string data)
        {
            return !IsEmpty(data);
        }

        public static bool IsNotEmpty<T>(IEnumerable<T> data)
        {
            return !IsEmpty(data);
        }

        public static bool IsNotEmpty<T, U>(IDictionary<T, U> data)
        {
            return !IsEmpty(data);
        }

        /// <summary>
        /// Return a default object if the object is null
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>The result</returns>
        public static T Nvl<T>(T obj) where T : class
        {
            return IsNull(obj) ? default(T) : obj;
        }

        /// <summary>
        /// Return true if two strings equal ignore case
        /// </summary>
        /// <param name="strOne">First string</param>
        /// <param name="strTwo">Second string</param>
        /// <returns>True if two strings equal ignore case</returns>
        public static bool EqualsIgnoreCase(string strOne, string strTwo)
        {
            strOne = strOne.Trim().ToLower();
            strTwo = strTwo.Trim().ToLower();

            return strOne.Equals(strTwo);
        }

        /// <summary>
        /// Return true if two strings not equal ignore case
        /// </summary>
        /// <param name="strOne">First string</param>
        /// <param name="strTwo">Second string</param>
        /// <returns>True if two strings not equal ignore case</returns>
        public static bool NotEqualsIgnoreCase(string strOne, string strTwo)
        {
            return !EqualsIgnoreCase(strOne, strTwo);
        }

        /// <summary>
        /// Return a default object if the object is null
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>The result</returns>
        /*public static List<T> Nvl<T>(List<T> objs) where T : class
        {
            return IsNull(objs) ? new List<T>() : objs;
        }*/

        public static Guid GetIdentityCode(object obj)
        {
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(GetAsBytes(obj));
            hash[6] &= 0x0f;
            hash[6] |= 0x30;
            hash[8] &= 0x3f;
            hash[8] |= 0x80;

            byte temp = hash[6];
            hash[6] = hash[7];
            hash[7] = temp;

            temp = hash[4];
            hash[4] = hash[5];
            hash[5] = temp;

            temp = hash[0];
            hash[0] = hash[3];
            hash[3] = temp;

            temp = hash[1];
            hash[1] = hash[2];
            hash[2] = temp;
            return new Guid(hash);
        }

        public static byte[] ObjectToByteArray(object obj)
        {
            /*if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }*/

            // Serialize object
            string objToString = GetSerializedObject(obj);
            // Convert oject to bytes
            return Encoding.UTF8.GetBytes(objToString);
        }

        public static T ByteArrayToObject<T>(byte[] byteArray)
        {
            /*MemoryStream ms = new MemoryStream(byteArray);

            BinaryFormatter bf = new BinaryFormatter();

            ms.Position = 0;

            return (T)bf.Deserialize(ms);*/

            // Convert bytes to object
            string stringObj = Encoding.UTF8.GetString(byteArray);
            // Deserialize bytes to object
            return GetDeserializedObject<T>(stringObj);
        }

        /// <summary>
        /// Can serialize an object to Json string
        /// </summary>
        /// <param name="data"></param>
        /// <returns>A serialized object to Json string</returns>
        public static string GetSerializedObject(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// Can deserialize an object from Json string
        /// </summary>
        /// <param name="value">A Json string</param>
        /// <returns>A deserialized object from Json string</returns>
        public static T GetDeserializedObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// Can convert string to double
        /// </summary>
        /// <param name="data">String data</param>
        /// <returns>String converted to double</returns>
        public static double GetAsDouble(string data)
        {
            double dReturn = 0;

            if (IsNullOrEmpty(data)) data = "0";
            dReturn = Convert.ToDouble(data);

            return dReturn;
        }

        /// <summary>
        /// Can convert decimal to double
        /// </summary>
        /// <param name="data">Decimal data</param>
        /// <returns>Decimal converted to double</returns>
        public static double GetAsDouble(decimal data)
        {
            double dReturn = 0;

            dReturn = Convert.ToDouble(data);

            return dReturn;
        }

        /// <summary>
        /// Can convert string to integer
        /// </summary>
        /// <param name="data">String data</param>
        /// <returns>String converted to integer</returns>
        public static int GetAsInteger(string data)
        {
            int iReturn = 0;

            if (IsNullOrEmpty(data)) data = "0";
            iReturn = Convert.ToInt32(data);

            return iReturn;
        }

        /// <summary>
        /// Can convert double to integer
        /// </summary>
        /// <param name="data">Double data</param>
        /// <returns>Double converted to integer</returns>
        public static int GetAsInteger(double data)
        {
            int iReturn = 0;

            iReturn = Convert.ToInt32(data);

            return iReturn;
        }

        /// <summary>
        /// Can convert string to long
        /// </summary>
        /// <param name="data">String data</param>
        /// <returns>String converted to long</returns>
        public static long GetAsLong(string data)
        {
            long lReturn = 0;


            if (IsNullOrEmpty(data)) data = "0";
            lReturn = Convert.ToInt64(data);

            return lReturn;
        }

        /// <summary>
        /// Can convert string to float
        /// </summary>
        /// <param name="data">String data</param>
        /// <returns>String converted to float</returns>
        public static float GetAsFloat(string data)
        {
            float fReturn = 0;


            if (IsNullOrEmpty(data)) data = "0";
            fReturn = Convert.ToSingle(data);

            return fReturn;
        }

        /// <summary>
        /// Can convert string to DateTime
        /// </summary>
        /// <param name="data">String data</param>
        /// <returns>String converted to DateTime</returns>
        public static DateTime GetAsDateTime(string data)
        {
            DateTime dtReturn = new DateTime();

            if (!IsNullOrEmpty(data)) dtReturn = Convert.ToDateTime(data);

            return dtReturn;
        }

        /// <summary>
        /// Can convert string to boolean
        /// </summary>
        /// <param name="data">String data</param>
        /// <returns>String converted to boolean</returns>
        public static bool GetAsBoolean(string data)
        {
            bool bReturn = false;

            if (IsNullOrEmpty(data)) data = "false";
            bReturn = Convert.ToBoolean(data);

            return bReturn;
        }

        /// <summary>
        /// Can convert object to bytes
        /// </summary>
        /// <param name="data">Object data</param>
        /// <returns>Object converted to bytes</returns>
        public static byte[] GetAsBytes(object data)
        {
            byte[] bytesReturn = new byte[0];

            if (IsNotNull(data))
            {
                bytesReturn = (byte[])data;
            }

            return bytesReturn;
        }

        public static string GetAsString(object data)
        {
            return Convert.ToString(data);
        }

        /// <summary>
        /// Can convert boolean to string
        /// </summary>
        /// <param name="data">Boolean data</param>
        /// <returns>Boolean converted to string</returns>
        public static string GetAsString(bool data)
        {
            return Convert.ToString(data);
        }

        /// <summary>
        /// Can convert integer to string
        /// </summary>
        /// <param name="data">Integer data</param>
        /// <returns>Integer converted to string</returns>
        public static string GetAsString(int data)
        {
            return Convert.ToString(data);
        }

        /// <summary>
        /// Can convert float to string
        /// </summary>
        /// <param name="data">Float data</param>
        /// <returns>Float converted to string</returns>
        public static string GetAsString(float data)
        {
            return Convert.ToString(data);
        }

        /// <summary>
        /// Can convert long to string
        /// </summary>
        /// <param name="data">Long data</param>
        /// <returns>Long converted to string</returns>
        public static string GetAsString(long data)
        {
            return Convert.ToString(data);
        }

        /// <summary>
        /// Can convert double to string
        /// </summary>
        /// <param name="data">Double data</param>
        /// <returns>Double converted to string</returns>
        public static string GetAsString(double data)
        {
            return Convert.ToString(data);
        }

        /// <summary>
        /// Can convert DateTime to string
        /// </summary>
        /// <param name="data">DateTime data</param>
        /// <returns>DateTime converted to string</returns>
        public static string GetAsString(DateTime data)
        {
            return Convert.ToString(data);
        }

        /// <summary>
        /// Can convert Image to 64 base string
        /// </summary>
        /// <param name="data">Image</param>
        /// <returns>Image converted to 64 base string</returns>
        public static string GetAsString(ImageData data)
        {
            return Convert.ToBase64String(data.ImageBinaryData);
        }

        /// <summary>
        /// Can convert string to parameter
        /// </summary>
        /// <param name="data">String data</param>
        /// <returns>String converted to parameter</returns>
        public static Parameter GetAsParameter(string data)
        {
            return GetAsParameter(data, '=');
        }

        /// <summary>
        /// Can convert string to parameter with specific separator
        /// </summary>
        /// <param name="data">String data</param>
        /// <param name="separator">Char separator</param>
        /// <returns>String converted to parameter</returns>
        public static Parameter GetAsParameter(string data, char separator)
        {
            string parameterName = ExtractString(data, separator, 0);
            string parameterValue = ExtractString(data, separator, 1);

            Parameter returnParam = new Parameter(parameterName, parameterValue);

            return returnParam;
        }

        /// <summary>
        /// Can extract string from another
        /// </summary>
        /// <param name="originString">Origin string</param>
        /// <param name="separator">Char separator</param>
        /// <param name="position">The position in the origin string</param>
        /// <returns>The extracted string</returns>
        public static string ExtractString(string originString, char separator, int position)
        {
            string sReturn = "";

            try
            {
                string[] stringSplit = originString.Split(separator);
                sReturn = stringSplit[position];
            }
            catch (Exception e)
            {

            }

            return sReturn;
        }

        /// <summary>
        /// Can extract list of string from a string
        /// </summary>
        /// <param name="originString">Origin string</param>
        /// <param name="separator">Char separator</param>
        /// <returns>The string list</returns>
        public static List<string> ExtractStringList(string originString, char separator)
        {
            List<string> lstStrings = new List<string>();

            string[] stringSplit = originString.Split(separator);
            lstStrings = stringSplit.ToList();

            return lstStrings;
        }

        public static byte[] ReadAllBytes(Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public static Stream BytesToStream(byte[] bytes)
        {
            return new MemoryStream(bytes);
        }

        public static bool TypeIsInheritedBy<T1, T2>(T1 typeChild, T2 typeParent)
            where T1 : Type
            where T2 : Type
        {
            //return TypeIsInheritedBy<T1, T2>();
            return typeChild.IsSubclassOf(typeParent) || typeChild.IsEquivalentTo(typeParent);
        }

        /*public static bool TypeIsInheritedBy<T1, T2>()
            where T1 : Type
            where T2 : Type
        {
            Type typeChild = typeof(T1);

            

            return typeChild.IsSubclassOf(T2) || typeChild.IsEquivalentTo(typeof(T2));
        }*/
    }
}
