using System;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace CoreNET.Common.Base
{
    public class SQLDataSource
    {
        public const int MODE_DB_CONFIG = 1;
        public const int MODE_DB_DM = 2;
        public const int MODE_DB_OPERATIONAL = 3;
        public const int MODE_DB_OPERATIONAL2 = 4;
        public const int MODE_DB_LOG = 5;

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        static ArrayList dcs = new ArrayList(new string[] {
            "WIN-47628ET8D5G\\MSSQLEBENDCMH",
            "dev2019.usadi.co.id",
            "hy-dell",
            "db2017",
            "dev-2019svr",
            ".",
            ".\\exp",
            "token",
            "localhost",
            "127.0.0.1",
            "127.0.0.1\\sql17",
            ".\\sql17"
        });

        // Static Constructor: Menjamin Connection String SELALU terinisialisasi saat class ini di-load
        static SQLDataSource()
        {
            Instance.SetDataSourceConfig(null);
        }

        public static bool CekAllowedServer()
        {
            return true;
        }

        public static bool CekAllowedServer(string server)
        {
            return true; // Bypass pemeriksaan nama server dari Request Proxy
        }

        public void SetConnectionString(string cs)
        {
            if (!string.IsNullOrEmpty(cs))
            {
                _CS_Config = cs;
                _CS_Operational = cs;
                _CS_Operational2 = cs;
            }
        }

        #region Property DataSource
        private static SQLDataSource _Instance = new SQLDataSource();
        public static SQLDataSource Instance
        {
            get => _Instance;
            set => _Instance = value;
        }

        public void SetDataSourceConfig(string chiper)
        {
            string cs_template = @"data source={0};Initial Catalog={1};user id={2};password={3};Asynchronous Processing=true";

            // Hardcode fallback ke 127.0.0.1\SQL17 jika AppSettings tidak terbaca
            string SQLInstance = ConfigurationManager.AppSettings["DataSource"] ?? @"127.0.0.1\SQL17";
            string Getdb = ConfigurationManager.AppSettings["Database"] ?? "SMARTSYS";
            string user = ConfigurationManager.AppSettings["Userdb"] ?? "sa";
            string pwd = ConfigurationManager.AppSettings["Pwddb"] ?? "Tanahdatar2025";

            // Set connection string utama & operational (WAJIB TERISI SEMUA)
            string mainCS = string.Format(cs_template, SQLInstance, Getdb, user, pwd);
            _CS_Config = mainCS;
            _CS_Operational = mainCS;
            _CS_Operational2 = mainCS;

            // Set connection string log
            string logDb = Getdb.ToLower().Contains("sys") ? Getdb.ToLower().Replace("sys", "log") : Getdb + "_log";
            _CS_Log = string.Format(cs_template, SQLInstance, logDb, user, pwd);
        }

        public void SetDataSourceLog(string cs)
        {
            _CS_Log = cs;
        }

        public void SetDataSource(Hashtable param)
        {
        }
        #endregion

        #region Property CS_Config
        private string _CS_Config;
        public string CS_Config => string.IsNullOrEmpty(_CS_Config) ? _CS_Operational : _CS_Config;
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

        public static string GetSQLInstance()
        {
            string sqlinstance = ConfigurationManager.AppSettings["DataSource"];
            if (!string.IsNullOrEmpty(sqlinstance))
            {
                return sqlinstance.ToLower().Trim();
            }
            return @"127.0.0.1\sql17";
        }

        public static string GetOpDB(string cs)
        {
            if (string.IsNullOrEmpty(cs))
            {
                cs = SQLDataSource.Instance.CS_Operational;
            }
            if (string.IsNullOrEmpty(cs)) return "SMARTSYS";

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
            return "SMARTSYS";
        }

        public static string GetUserDB()
        {
            try
            {
                string cs = SQLDataSource.Instance.CS_Config;
                if (string.IsNullOrEmpty(cs)) return "sa";

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
            return "sa";
        }

        public static string GetPwdDB()
        {
            try
            {
                string cs = SQLDataSource.Instance.CS_Config;
                if (string.IsNullOrEmpty(cs)) return "Tanahdatar2025";

                string[] temps = cs.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < temps.Length; i++)
                {
                    if (temps[i].ToLower().Contains("password"))
                    {
                        string[] pwdParts = temps[i].Split(new char[] { '=' }, 2);
                        if (pwdParts.Length > 1) return pwdParts[1];
                    }
                }
            }
            catch (Exception) { }
            return "Tanahdatar2025";
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
                default:
                    return new System.Data.SqlClient.SqlConnection(CS_Operational);
            }
        }

        #region Security Enkripsi & Dekripsi
        private const int keysize = 256;
        private static byte[] initVectorBytes = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private static string KeyStr
        {
            get
            {
                string publickey = GlobalLib.GetPublishedKey();
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
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText", "cipherText tidak boleh null atau kosong");

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
        #endregion
    }
}