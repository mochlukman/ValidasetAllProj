using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace CoreNET.Common.Base
{
    public class SQLDataSource01
    {
        public const int MODE_DB_CONFIG = 1;
        public const int MODE_DB_DM = 2;
        public const int MODE_DB_OPERATIONAL = 3;
        public const int MODE_DB_OPERATIONAL2 = 4;
        public const int MODE_DB_LOG = 5;


        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        static ArrayList dcs = new ArrayList(new string[] {
        "WIN-47628ET8D5G\\MSSQLEBENDCMH"
        ,"dev2019.usadi.co.id"
        ,"hy-dell"
        ,"db2017"
        ,"dev-2019svr"
        ,"."
        ,".\\exp"
        ,"token"
      });

        public static bool CekAllowedServer()
        {
            return dcs.Contains(SQLDataSource01.GetSQLInstance().ToLower());
        }
        public static bool CekAllowedServer(string server)
        {
            return dcs.Contains(server.ToUpper());
        }


        public void SetConnectionString(string cs)
        {
            if ((GetConsoleWindow() != IntPtr.Zero) && dcs.Contains(GetOpDB(cs)))
            {
                _CS_Config = cs;
            }
        }
        #region Property DataSource
        private static SQLDataSource01 _Instance = new SQLDataSource01();
        public static SQLDataSource01 Instance
        {
            get => _Instance;
            set => _Instance = value;
        }
        /*
        public void SetDataSourceConfig(string chiper)
        {
            string cs_template = @"data source={0};Initial Catalog={1};user id={2};password={3};Asynchronous Processing=true";
            //string cs_template = "Initial Catalog={0};user id={1};password={2};Asynchronous Processing=true";
            //string cs = string.Format(cs_template, @".\exp", "usadisys", "usadi", "valid49");
            //string cs = string.Format(cs_template, @".", "usadisys", "sa", "password123!");
            string SQLInstance = ConfigurationManager.AppSettings["DataSource"];
            string user = ConfigurationManager.AppSettings["Userdb"];
            string pwd = ConfigurationManager.AppSettings["Pwddb"];
            string Getdb = ConfigurationManager.AppSettings["Database"];
            //string SQLInstance = SQLDataSource01.GetSQLInstance();
            //string user = SQLDataSource01.GetUserDB();
            //string pwd = SQLDataSource01.GetPwdDB();

            //string Connstring = string.Format(cs_template, @"WIN-Q2QBAO9N5B2\MSSQL2017", "smartsys", "sa", "A53t47kh!");
            string Connstring = string.Format(cs_template, ".", "smartsys", "sa", "Tanahdatar2025");
            //string Connstring = string.Format(cs_template, SQLInstance, GetOpDB(_CS_Config), user, pwd);

            //string Connstring = string.Format(cs_template, SQLInstance, Getdb, user, pwd);
            string tempchiper = En(Connstring, KeyStr);//367052F4193AD3A9
            //string tempchiper = En(Connstring, "D02DFB6883790059");

            try
            {
                // _CS_Config = De(chiper, KeyStr);
                _CS_Config = De(tempchiper, KeyStr);
                if (_CS_Config.ToLower().Contains("sys"))
                {
                    _CS_Log = _CS_Config.ToLower().Replace("sys", "log");
                }
                else
                {

                    string ConnectionString = string.Format("data source={0};initial catalog={3};user id={1};password={2};Asynchronous Processing=true",
                    //string ConnectionString = string.Format("initial catalog={0};user id={1};password={2};Asynchronous Processing=true",
                    SQLInstance, user, pwd, Getdb + "_log");

                    _CS_Log = ConnectionString;
                    //_CS_Log = "data source=LK-NB\\SQL2017;initial catalog=smartsys_log;user id=sa;password=1234!;Asynchronous Processing=true";

                }
            }
            catch (Exception ex)
            {
                UtilityBO.Log(ex);
                throw new Exception("Get your license. Your challenged key = " + GlobalLib.GetPublishedKey());
            }
        }*/
        public void SetDataSourceConfig(string chiper)
        {
            string cs_template = @"data source={0};Initial Catalog={1};user id={2};password={3};Asynchronous Processing=true";

            // 1. Parameter Database (Gunakan 127.0.0.1, localhost, atau "WIN-8CCK2RAJK2G\\SQL17")
            string SQLInstance = ConfigurationManager.AppSettings["DataSource"] ?? "127.0.0.1";
            string Getdb = ConfigurationManager.AppSettings["Database"] ?? "SMARTSYS";
            string user = ConfigurationManager.AppSettings["Userdb"] ?? "sa";
            string pwd = ConfigurationManager.AppSettings["Pwddb"] ?? "Tanahdatar2025";

            try
            {
                // 2. Format Connection String Dasar
                string connStr = string.Format(cs_template, SQLInstance, Getdb, user, pwd);

                // 3. Wajib Set SEMUA Property Connection String
                _CS_Config = connStr;
                _CS_Operational = connStr;  // <-- Penyebab utama error jika ini kosong
                _CS_Operational2 = connStr;

                // 4. Set Connection String Log
                if (Getdb.ToLower().Contains("sys"))
                {
                    _CS_Log = string.Format(cs_template, SQLInstance, Getdb.ToLower().Replace("sys", "log"), user, pwd);
                }
                else
                {
                    _CS_Log = string.Format(cs_template, SQLInstance, Getdb + "_log", user, pwd);
                }
            }
            catch (Exception ex)
            {
                UtilityBO.Log(ex);
                throw new Exception("Get your license. Your challenged key = " + GlobalLib.GetPublishedKey());
            }
        }
        public void SetDataSourceLog(string cs)
        {
            _CS_Log = cs;
        }
        public void SetDataSource(Hashtable param)
        {
            //string server = (string)param["DataSource"];
            //string cs_template = (string)param["DataSourceLog"];
            //string cs = string.Format(cs_template, server);
            //_CS_Log = cs;
            //cs_template = (string)param["DataSourceDM"];
            //cs = string.Format(cs_template, server);
            //_CS_DM = cs;
            //cs_template = (string)param["DataSourceOp"];
            //cs = string.Format(cs_template, server);
            //_CS_Operational = cs;
            //cs_template = (string)param["DataSourceOp2"];
            //cs = string.Format(cs_template, server);
            //_CS_Operational2 = cs;
        }
        #endregion

        //private static string cs_template = @"data source={0};initial catalog={1};user id={2};password={3};Asynchronous Processing=true";
        #region Property CS_Config
        private string _CS_Config;
        /*public string CS_Config
        {
          get {
            if (!BaseDataAdapter.CekAllowedApp())
            {
              BaseDataAdapter.ValidateCS(_CS_Config);
            }
            return _CS_Config;
          }
        }*/
        public string CS_Config
        {
            get
            {
                return _CS_Config;//"data source=LK-NB\\SQL2017;initial catalog=smartsys;user id=sa;password=1234!";
            }
        }
        #endregion
        #region Property CS_Log
        private string _CS_Log;
        public string CS_Log => _CS_Log;
        #endregion
        #region Property CS_Operational
        private string _CS_Operational = string.Empty;
        public string CS_Operational => _CS_Operational;
        #endregion
        #region Property CS_Operational2
        private string _CS_Operational2 = string.Empty;
        public string CS_Operational2 => _CS_Operational2;
        #endregion
        #region Property CS_DM
        private string _CS_DM = string.Empty;
        public string CS_DM => _CS_DM;
        #endregion
        /*public static string GetSQLInstance()
        {
            string sqlinstance = string.Empty;
            string cs = SQLDataSource01.Instance.CS_Config;
            string[] temps = cs.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < temps.Length; i++)
            {
                if (temps[i].ToLower().Contains("data source"))
                {
                    sqlinstance = temps[i].ToLower().Replace("data source", string.Empty).Replace("=", string.Empty).Trim();
                }
            }
            return sqlinstance;
        }*/

        public static string GetSQLInstance()
        {
            // Paksa mengembalikan nama Instance/Server yang didaftarkan di AppSettings web.config
            string sqlinstance = ConfigurationManager.AppSettings["DataSource"];
            if (!string.IsNullOrEmpty(sqlinstance))
            {
                return sqlinstance.ToLower().Trim();
            }

            // Fallback jika AppSettings kosong
            return "win-8cck2rajk2g\\sql17";
        }
        public static string GetOpDB(string cs)
        {
            if (string.IsNullOrEmpty(cs))
            {
                cs = SQLDataSource01.Instance.CS_Operational;
            }
            string[] temps = cs.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < temps.Length; i++)
            {
                if (temps[i].ToLower().Contains("initial catalog"))
                {
                    return temps[i].ToLower().Replace("initial catalog", string.Empty).Replace("=", string.Empty).Trim();
                }
                if (temps[i].ToLower().Contains("database"))
                {
                    return temps[i].ToLower().Replace("database", string.Empty).Replace("=", string.Empty).Trim();
                }
            }
            return null;
        }
        public static string GetUserDB()
        {
            try
            {
                string cs = SQLDataSource01.Instance.CS_Config;
                string[] temps = cs.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < temps.Length; i++)
                {
                    if (temps[i].ToLower().Contains("user id"))
                    {
                        return temps[i].ToLower().Replace("user id", string.Empty).Replace("=", string.Empty).Trim();
                    }
                }
            }
            catch (Exception) { }
            return null;
        }
        public static string GetPwdDB()
        {
            try
            {
                string cs = SQLDataSource01.Instance.CS_Config;
                string[] temps = cs.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < temps.Length; i++)
                {
                    if (temps[i].ToLower().Contains("password"))
                    {
                        temps = temps[i].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                        return temps[1];
                    }
                }
            }
            catch (Exception) { }
            return null;
        }

        public System.Data.SqlClient.SqlConnection GetSQLConnection(int mode)
        {
            switch (mode)
            {
                case MODE_DB_CONFIG:
                    return new System.Data.SqlClient.SqlConnection(CS_Config);
                case MODE_DB_LOG:
                    return new System.Data.SqlClient.SqlConnection(CS_Log);
                case MODE_DB_DM:
                    return new System.Data.SqlClient.SqlConnection(CS_DM);
                case MODE_DB_OPERATIONAL:
                    return new System.Data.SqlClient.SqlConnection(CS_Operational);
                case MODE_DB_OPERATIONAL2:
                    return new System.Data.SqlClient.SqlConnection(CS_Operational2);
                default://case MODE_DB_OPERATIONAL:
                    return new System.Data.SqlClient.SqlConnection(CS_Operational);
            }

        }

        // need 32 characters=>32/8 = 4-bit base encoding => initVectorByte 2^4=16
        //                               0         1         2         3
        //                               01234567890123456789012345678901
        private const int keysize = 256;
        private static byte[] initVectorBytes = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private static string KeyStr
        {
            get
            {
                //string publickey = GlobalLib.GetPublishedKey();
                string publickey = "D02DFB6883790059";
                //string str = MasterAppConstants.Instance.MasterAppID.Substring(0, 16);
                //@Todo PakeInterface, SysUtils itu bisa diakses disini.karena diakses sebelum code ini di global.asax
                string str = MasterAppConstants.Instance.MasterAppID.Substring(0, 16);

                string kek4key = En(str, str);
                string key4cs = En(publickey, kek4key);
                return key4cs;
            }
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
        private static string De(string cipherText, string passPhrase)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;
                using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                {
                    using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                        }
                    }
                }
            }
        }


    }
}
