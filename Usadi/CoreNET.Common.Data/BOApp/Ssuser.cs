using CoreNET.Common.Base;
using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Data;

namespace CoreNET.Common.BO
{
  /*Ini belum dipakai ya, jadi return result XML ngga boleh ke sini*/
  #region Ss10user
  [Serializable]
  public class SsuserControl : BaseDataControl
  {
    #region Properties Ss10user
    public string Nip { get; set; }
    public new string Userid { get; set; }
    public string Userblock { get; set; }
    public string Useremail { get; set; }
    public string Userhp { get; set; }
    public string Userket { get; set; }
    public string Usernama { get; set; }
    public string Usernip { get; set; }
    public int Userno { get; set; }
    public string Userpwd { get; set; }
    public int Userstatus { get; set; }
    public string Usertype { get; set; }
    public string Useruraian { get; set; }
    public string Uturaian { get; set; }
    #endregion
    #region Properties Ss10userapp
    private string _UseridForEntry = string.Empty;
    public string UseridForEntry
    {
      get => string.IsNullOrEmpty(_UseridForEntry) ? Userid : _UseridForEntry;
      set => _UseridForEntry = value;
    }
    public string Idrole { get; set; }
    public string Nmrole { get; set; }
    #endregion Properties Ss10userapp 
    #region Ss10userLoginControl
    public string Nodok { get; set; }//Not DB Field
    public string Uraian { get; set; }//Not DB Field
    public string DefaultNodok { get; set; }//Not DB Field
    public string DefaultUraian { get; set; }//Not DB Field
    public string DefaultKdunit { get; set; }//Not DB Field
    public string DefaultNmunit { get; set; }//Not DB Field
    public string Kdpemda { get; set; }//Not DB Field
    public string Nmpemda { get; set; }//Not DB Field
    public string Idunit { get; set; }//Not DB Field
    public string Kdunit { get; set; }//Not DB Field
    public string Nmunit { get; set; }//Not DB Field
    public string Kdgroup { get; set; }//Not DB Field
    public int ModeEditable { get; set; }//Not DB Field
    #endregion Properties

    #region Methods
    public SsuserControl()
    {
      XMLName = "Ssuser";
      ModeDB = SQLDataSource.MODE_DB_CONFIG;
      ConnectionString = SQLDataSource.Instance.CS_Config;
    }

    public void UpdatePwd(string pwd)
    {
      Userpwd = pwd;//Sdh diencrypt GlobalAsp.En(pwd);
      Update("Pwd");
    }


    public new BaseBO Load()
    {
      string sql = @"
        select * from {0} where USERID='{1}'
      ";
      if (SQLDataSource.GetOpDB(ConnectionString).ToLower().Equals("smartsys"))
      {
        sql = string.Format(sql, "SS10USER", Userid);
      }
      else
      {
        sql = string.Format(sql, "SSUSER", Userid);
      }

      string[] fields = new string[] { "Userid", "Usertype", "Usernama", "Userpwd", "Userblock", "Usernip", "Userhp"
        , "Useremail", "Userno", "Useruraian", "Userstatus", "Userket", "Idrole"
        , "Status", "Last_by", "Last_date", "Statusicon", "Userid", "Url", "Kdlevel", "Type" };

      List<IDataControl> list = BaseDataAdapter.GetListDC(this, sql, fields);

      if (list.Count == 0)
      {
        return null;
      }
      else
      {
        List<SsuserControl> ListData = list.ConvertAll(new Converter<IDataControl, SsuserControl>(delegate (IDataControl par) { return (SsuserControl)par; }));
        return ListData[0];
      }
    }
    private const int keysize = 256;
    private static byte[] initVectorBytes = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private static string KeyStr/*KeyStr untuk encrypted user, Biar bisa beda aplikasi, selama UserDC classnya sama*/
    {
      get
      {
        string DictionaryDC = (string)MasterAppConstants.DEFAULT_DICTIONARY_DC;//
        //string DictionaryDC = (string)MasterAppConstants.Instance.DictionaryDC;//
        if (MasterAppConstants.Instance.StatusTesting)
        {
          //DictionaryDC = (string)MasterAppConstants.Instance.DictionaryDC;//
          DictionaryDC = (string)MasterAppConstants.DEFAULT_DICTIONARY_DC;//
        }
        Type t = System.Type.GetType(DictionaryDC);
        string assembly = t.Assembly.ManifestModule.Name.ToLower();
        string publickey = En(assembly, assembly);
        return publickey;
      }
    }
    public static string En(string plainText)
    {
      return En(plainText, KeyStr);
    }
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


    public static string Encode(string plainText)
    {
      byte[] keybytes = Encoding.UTF8.GetBytes(GlobalLib.PublicKey);
      byte[] iv = Encoding.UTF8.GetBytes(GlobalLib.PublicKey); //new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //Encoding.UTF8.GetBytes("8080808080808080");//

      //byte[] encrypted = Convert.FromBase64String(cipherText);
      byte[] encrypted = EncryptStringToBytes(plainText, keybytes, iv);
      return Convert.ToBase64String(encrypted);
    }
    private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
    {
      // Check arguments.   
      if (plainText == null || plainText.Length <= 0)
      {
        throw new ArgumentNullException("plainText");
      }
      if (key == null || key.Length <= 0)
      {
        throw new ArgumentNullException("key");
      }
      if (iv == null || iv.Length <= 0)
      {
        throw new ArgumentNullException("key");
      }
      byte[] encrypted;
      // Create a RijndaelManaged object   
      // with the specified key and IV.   
      using (RijndaelManaged rijAlg = new RijndaelManaged())
      {
        rijAlg.Mode = CipherMode.CBC;
        rijAlg.Padding = PaddingMode.PKCS7;
        rijAlg.FeedbackSize = 128;

        rijAlg.Key = key;
        rijAlg.IV = iv;

        // Create a decrytor to perform the stream transform.   
        ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

        // Create the streams used for encryption.   
        using (MemoryStream msEncrypt = new MemoryStream())
        {
          using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
          {
            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
            {
              //Write all data to the stream.   
              swEncrypt.Write(plainText);
            }
            encrypted = msEncrypt.ToArray();
          }
        }
      }
      // Return the encrypted bytes from the memory stream.   
      return encrypted;
    }

    #endregion Methods
  }
  #endregion Ssappuser
}

