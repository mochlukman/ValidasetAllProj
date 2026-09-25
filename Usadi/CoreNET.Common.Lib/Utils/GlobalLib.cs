using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CoreNET.Common.Base
{
  public class GlobalLib
  {
    #region Static Property StrID
    private static string StrID
    {
      get
      {
        string str = string.Empty;
        if (MasterAppConstants.Instance.URLBase.Length >= 16)
        {
          str = MasterAppConstants.Instance.URLBase.Substring(0, 16);
        }
        else
        {
          str = (MasterAppConstants.Instance.URLBase + MasterAppConstants.Instance.URLBase).Substring(0, 16);
        }
        return En(str, str);
      }
    }
    #endregion
    #region Static Property PublicKey
    public static string PublicKey//Nanti pake webservice dan diencrypt
    {
      get
      {
        string str = MasterAppConstants.Instance.MasterAppID.Substring(0, 6) + DateTime.Now.ToString("yyyyMMddHH");
        string chiper = En(str, MasterAppConstants.Instance.MasterAppID.Substring(0, 16));
        if (chiper.Length < 16)
        {
          while (chiper.Length < 16)
          {
            chiper += "=";
          }

          return chiper;
        }
        else
        {
          return chiper.Substring(0, 16);// "hy-dellt20171224";
        }
      }
    }
    public static string Encrypt(string text)
    {
      return En(text, GetPublishedKey());
    }

    #endregion
    public static string PublicEncrypt(string text)
    {
      return EncryptByMasterAppID(text);
    }
    private static string EncryptByMasterAppID(string text)
    {
      return En(text, MasterAppConstants.Instance.MasterAppID);
    }

    public static string GetPublishedKey()/*KeyStr untuk ConnectionString*/
    {
      string modelNo = identifier("Win32_DiskDrive", "Model");
      string manufatureID = identifier("Win32_DiskDrive", "Manufacturer");
      string signature = identifier("Win32_DiskDrive", "Signature");//Ketika konek harddisk, jadi ganti CPUID
      string totalHeads = identifier("Win32_DiskDrive", "TotalHeads");
      string servername = Environment.MachineName;
      /*
       * connect via redmehy
       * HY-DELL(Standard disk drives)943639260Seagate BUP Slim BK SCSI Disk Device25519hqS/Zc7gql68UfBhy9DL/3m8UBKnqgAnLVgVxyMPE=
       * no connect 
       * HY-DELL(Standard disk drives)3113557460INTEL SSDSCKKF512H6 SATA 512GB25519hqS/Zc7gql68UfBhy9DL/3m8UBKnqgAnLVgVxyMPE=
       * */
      string strtemp = servername + manufatureID + signature + modelNo + totalHeads + StrID;
      string publickey = En(strtemp, StrID);
      publickey = UtilityBO.GetMD5HexStringOld(publickey, 16);
      return publickey;
    }
    private const int keysize = 256;
    private static byte[] initVectorBytes = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private static string En(string plainText, string passPhrase)
    {
      byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

      PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
      byte[] keyBytes = password.GetBytes(keysize / 8);
      using (RijndaelManaged symmetricKey = new RijndaelManaged())
      {
        symmetricKey.Mode = CipherMode.CBC;
        using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
        {
          using (MemoryStream memoryStream = new MemoryStream())
          {
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
              cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
              cryptoStream.FlushFinalBlock();
              byte[] cipherTextBytes = memoryStream.ToArray();
              return Convert.ToBase64String(cipherTextBytes);
            }
          }
        }
      }

    }
    private static string identifier(string wmiClass, string wmiProperty)
    {
      string result = "";
      System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
      System.Management.ManagementObjectCollection moc = mc.GetInstances();
      foreach (System.Management.ManagementObject mo in moc)
      {
        //Only get the first one
        if (result == "")
        {
          try
          {
            result = string.IsNullOrEmpty((string)mo[wmiProperty]) ? string.Empty : mo[wmiProperty].ToString();
            break;
          }
          catch
          {
          }
        }
      }
      return result;
    }
  }
}
