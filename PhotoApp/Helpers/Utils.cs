using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoApp.Helpers
{
    public class Utils
    {
        public static String sconvert(Object obj){
            if(obj == null) {
                return "";
            }
            else {
                return obj.ToString();
            }
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static string GetHex(byte[] bytes)
        {
            char[] chars = new char[bytes.Length];
            for(int i = 0; i < bytes.Length; i++)
            {
                chars[i] = bytes[i].ToString("X2").First();
            }
            return string.Concat(chars);
        }

        public static bool ArrayCompare<T>(T[] a1, T[] a2) where T : IComparable
        {
            if (a1.Length != a2.Length) return false;
            for (int i = 0; i < a1.Length; i++)
            {
                if (a1[i].CompareTo(a2[i]) != 0) return false;
            }
            return true;
        }
    }
}