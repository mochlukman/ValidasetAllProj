using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CoreNET.Common.Base
{
  public class UtilityLib
  {
    public static int GetTriwulan(int bulan)
    {
      return ((bulan - 1) / 3) + 1;
    }
    public static string Encode(string str)//Algoritmanya sdh umum, pake ND aja
    {
      //byte[] data = new byte[32];

      MD5 md5 = new MD5CryptoServiceProvider();
      byte[] result = md5.ComputeHash(UTF8Encoding.Default.GetBytes(str));
      return Convert.ToBase64String(result);
    }
  }
}
