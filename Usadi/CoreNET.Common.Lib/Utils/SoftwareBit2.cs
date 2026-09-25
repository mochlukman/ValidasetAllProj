using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Security.Cryptography;

namespace CoreNET.Common.Base
{
  public class SoftwareBit2 : ISerializable
  {
    // Property

    #region Property Idapp
    private string _Idapp;
    public string Idapp
    {
      get { return _Idapp; }
      set { _Idapp = value; }
    }
    #endregion
    #region Property AppTitle
    private string _AppTitle;
    public string AppTitle
    {
      get { return _AppTitle; }
      set { _AppTitle = value; }
    }
    #endregion
    #region Property MenuDC
    private string _MenuDC;
    public string MenuDC
    {
      get { return _MenuDC; }
      set { _MenuDC = value; }
    }
    #endregion
    #region Property NamaUnit
    private string _NamaUnit;
    public string NamaUnit
    {
      get { return _NamaUnit; }
      set { _NamaUnit = En(value, Idapp); }
    }
    #endregion
    #region Property ExpiredDuration
    private string _ExpiredDuration;
    public string ExpiredDuration
    {
      get { return _ExpiredDuration; }
      set { _ExpiredDuration = value; }
    }
    #endregion
    #region Property Logo
    private Image _Logo;
    public Image Logo
    {
      get { return _Logo; }
      set { _Logo = value; }
    }
    #endregion
    #region Property IdappProperty
    private string _IdappProperty;
    public string IdappProperty
    {
      get { return _IdappProperty; }
      set { _IdappProperty = value; }
    }
    #endregion
    #region Property NmappProperty
    private string _NmappProperty;
    public string NmappProperty
    {
      get { return _NmappProperty; }
      set { _NmappProperty = value; }
    }
    #endregion

    // Methods
    public SoftwareBit2()
    {
    }

    public SoftwareBit2(SerializationInfo info, StreamingContext ctxt)
    {
      this.Idapp = (string)info.GetValue("P1", typeof(string));
      this.AppTitle = (string)info.GetValue("P2", typeof(string));
      this.MenuDC = (string)info.GetValue("P3", typeof(string));
      this.NamaUnit = (string)info.GetValue("P4", typeof(string));
      this.ExpiredDuration = (string)info.GetValue("P5", typeof(string));
      this.IdappProperty = (string)info.GetValue("C1", typeof(string));
      this.NmappProperty = (string)info.GetValue("C2", typeof(string));
      this.Logo = (Image)info.GetValue("A1", typeof(Image));
    }

    public void En()
    {
      AppTitle = En(AppTitle, Idapp);
      MenuDC = En(MenuDC, Idapp);
      NamaUnit = En(NamaUnit, Idapp);
    }
    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
      info.AddValue("P1", this.Idapp);
      info.AddValue("P2", this.AppTitle);
      info.AddValue("P3", this.MenuDC);
      info.AddValue("P4", this.NamaUnit);
      info.AddValue("P5", this.ExpiredDuration);
      info.AddValue("C1", this.IdappProperty);
      info.AddValue("C2", this.NmappProperty);
      info.AddValue("A1", this.Logo);
    }

    public void LoadLog(string fname)
    {
      //this.Log = Image.FromFile(fname);
    }


    public void ReadObject(string fname)
    {
      Stream serializationStream = File.Open(fname, FileMode.Open);
      BinaryFormatter formatter = new BinaryFormatter();
      SoftwareBit2 bit = (SoftwareBit2)formatter.Deserialize(serializationStream);
      serializationStream.Close();
      this.Idapp = bit.Idapp;
      this.AppTitle = bit.AppTitle;
      this.MenuDC = bit.MenuDC;
      this.NamaUnit = bit.NamaUnit;
      this.ExpiredDuration = bit.ExpiredDuration;
      this.IdappProperty = bit.IdappProperty;
      this.NmappProperty = bit.NmappProperty;
      this.Logo = bit.Logo;

      MasterAppConstants.Instance.SetValue(CoreNET.Common.Base.MasterAppConstants.APPID, Idapp);
      MasterAppConstants.Instance.SetValue(CoreNET.Common.Base.MasterAppConstants.APPTITLE,AppTitle);
      MasterAppConstants.Instance.SetValue(CoreNET.Common.Base.MasterAppConstants.MENUDC, MenuDC);
      MasterAppConstants.Instance.SetValue(CoreNET.Common.Base.MasterAppConstants.IDAPP_PROPERTY, IdappProperty);
      MasterAppConstants.Instance.SetValue(CoreNET.Common.Base.MasterAppConstants.NMAPP_PROPERTY, NmappProperty);
    }

    public void SaveLogo(string fname)
    {
      try
      {
        File.Delete(fname);
        this.Logo.Save(fname, ImageFormat.Gif);

      }
      catch (Exception ex)
      {
        UtilityBO.Log(ex);
        throw new Exception("Konfigurasi error. Cek konfigurasi access security dari file image!");
      }

    }

    public void SaveObject(string fname)
    {
      try
      {
        Stream serializationStream = File.Open(fname, FileMode.Create);
        new BinaryFormatter().Serialize(serializationStream, this);
        serializationStream.Close();
      }
      catch (Exception ex) {
        UtilityBO.Log(ex);}
    }

    private const int keysize = 256;
    private byte[] initVectorBytes = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private string En(string plainText, string passPhrase)
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

  }

}
