using CoreNET.Common.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CoreNET.Common.BO
{
    #region Ssapp
    [Serializable]
    public class SsappControl : BaseDataControl, IDataControl, IDataControlApp
    {
        #region Properties 
        public static bool IsOptimized { get; set; } = true;
        public string Grpack { get; set; }
        public string Kdnmapp { get => Kdapp + Nmapp; set { } }
        public new string Url { get; set; }
        public string Icon { get; set; }
        public string QSSub { get; set; }
        #endregion Properties 

        #region Methods 
        public SsappControl()
        {
            XMLName = "Ssapp";
            ModeDB = SQLDataSource.MODE_DB_CONFIG;
            ConnectionString = SQLDataSource.Instance.CS_Config;
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
        private static string EncryptCS(string text, string publickey)
        {
            string str = MasterAppConstants.Instance.MasterAppID.Substring(0, 16);
            string kek4key = En(str, str);

            string key4cs = En(publickey, kek4key);
            string KeyStr = key4cs;
            string chiperCS = En(text, KeyStr);
            return chiperCS;
        }
        //public string InsertDefaultReg(string sqlinstance, string challenge_key)
        //{
        //  string idreg = Guid.NewGuid().ToString();
        //  string idapp = MasterAppConstants.Instance.AppID;
        //  string Csid = challenge_key.Trim();
        //  string Csserver = sqlinstance;
        //  string Csdb = "smartsys";
        //  string Csuser = "usadi";
        //  string Cspwd = "valid49";
        //  string cs_template = @"data source={0};Initial Catalog={1};user id={2};password={3};Asynchronous Processing=true";
        //  string cs = string.Format(cs_template, Csserver, Csdb, Csuser, Cspwd);
        //  string Cskey = EncryptCS(cs, Csid);


        //  string sql = @"
        //    insert into SS00APPREG(IDREG,IDAPP,NAMA,CSID,CSSERVER,CSDB
        //      ,CSUSER,CSPWD,CSKEY,STATUS,LAST_BY,LAST_DATE)
        //    values ('{0}','{1}','{2}','{3}','{4}','{5}'
        //      ,'{6}','{7}','{8}','{9}','{10}','{11}')
        //  ";
        //  sql = string.Format(sql, idreg, idapp, "anonymous", Csid, Csserver, Csdb, Csuser, Cspwd, Cskey
        //    , 0, "api", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        //  MasterAppConstants.Instance.Param[MasterAppConstants.APPID] = MasterAppConstants.ID_APP_PROGRAMMER;//Force akses smartsys
        //  BaseDataAdapter.ExecuteCmd(this, sql);

        //  return Cskey;

        //}
        public new IList View()
        {
            IList list = View(BaseDataControl.ALL);
            return list;
        }
        public new IList View(string label)
        {

            string template_sql = @"
        select rtrim(S.KDAPP) as KDAPP,rtrim(S.NMAPP) as NMAPP,rtrim(S.URAPP) as URAPP
        ,rtrim(S.URL) as URL
        ,case S.URL when '' then '' else rtrim(S.URL)+'&kdapp='+rtrim(S.KDAPP) end as URLFULL,
        rtrim(S.IDAPP) as IDAPP
        ,S.STATUS,S.KDLEVEL,rtrim(S.TYPE) as TYPE
        ,rtrim(isnull(S.LAST_BY,'')) as LAST_BY,S.LAST_DATE
        from {0} S
        order by S.KDAPP
      ";
            string[] fields = GetKeys();
            //string[] fields = new string[] { "Kdapp", "Urapp", "Idapp", "Icon"
            //  , "Status", "Statusicon"
            //  , "Last_by", "Last_date", "Nmapp", "Url", "UrlFull", "Kdlevel", "Type" };

            string sql = string.Empty;
            string cs = SQLDataSource.GetOpDB(ConnectionString);
            sql = string.Format(template_sql, "SSAPP");
            List<IDataControl> list = BaseDataAdapter.GetListDC(this, sql, fields);
            if (list.Count == 0)
            {
                sql = string.Format(template_sql, "SS00APP");
                list = BaseDataAdapter.GetListDC(this, sql, fields);
            }
            List<SsappControl> ListData = new List<SsappControl>(); //list.ConvertAll(new Converter<IDataControl, SsappControl>(delegate (IDataControl par) { return (SsappControl)par; }));
            foreach (SsappControl dc in list)
            {
                try
                {
                    dc.Nmapp = ConstantDict.Translate("App" + dc.Kdapp + "=" + dc.Nmapp);
                }
                catch (Exception ex)
                {
                    UtilityBO.Log(this, ex);
                }
                ListData.Add(dc);
            }
            return ListData;
        }

        public new BaseBO Load()
        {
            return Load(BaseDataControl.PK);
        }
        public new BaseBO Load(string label)
        {
            //string where = "";
            //if (label.Equals(BaseDataControl.PK))
            //{
            //    where = $"and KDAPP='{Kdapp}'";
            //}
            //else
            //{
            //    where = $"and IDAPP='{Idapp}'";
            //}

            //string template_sql = @"
            //    select rtrim(S.KDAPP) as KDAPP,rtrim(S.NMAPP) as NMAPP,rtrim(S.URAPP) as URAPP
            //    ,rtrim(S.URL) as URL
            //    ,case S.URL when '' then '' else rtrim(S.URL)+'&kdapp='+rtrim(S.KDAPP) end as URLFULL,
            //    rtrim(S.IDAPP) as IDAPP
            //    ,S.STATUS,S.KDLEVEL,rtrim(S.TYPE) as TYPE
            //    ,rtrim(isnull(S.LAST_BY,'')) as LAST_BY,S.LAST_DATE
            //    from {0} S
            //    where 1=1
            //    {1}
            //    order by S.KDAPP
            //  ";
            //string[] fields = GetKeys();

            //string sql = string.Empty;
            //string cs = SQLDataSource.GetOpDB(ConnectionString);
            //sql = string.Format(template_sql, "SSAPP", where);
            //List<IDataControl> list = null;
            //try
            //{
            //    list = BaseDataAdapter.GetListDC(this, sql, fields);
            //}
            //catch { }
            //if ((list == null) || (list.Count == 0))
            //{
            //    sql = string.Format(template_sql, "SS00APP", where);
            //    list = BaseDataAdapter.GetListDC(this, sql, fields);
            //}

            //if (list.Count == 1)
            //{
            //    SsappControl dcresult = (SsappControl)list[0];
            //    dcresult.CopyPropertyBOTo(this);
            //    return this;
            //}
            //else
            //{
            //    return null;
            //}

            //Ini bikn ciclyc
            List<SsappControl> list = SsappLookupControl.GetListDataSingleton();
            SsappControl dcRoot = null;
            if (label.Equals(BaseDataControl.PK))
            {
                dcRoot = (SsappControl)list.Find(o => o.Kdapp.Equals(Kdapp));
            }
            else
            {
                dcRoot = (SsappControl)list.Find(o => o.Idapp.Equals(Idapp));
            }
            if (dcRoot != null)
            {
                dcRoot.CopyPropertyBOTo(this);
            }
            return this;
        }
        public void SetEditable(bool editable)
        {
            Editable = editable;
        }

        public void ResetParams(string idapp)
        {
            SsappconfigLookupControl.Instance.ResetParams(idapp);
        }
        public string GetAppParam(string par)
        {
            return (string)SsappconfigLookupControl.Instance.Params[par];
        }
        public string ParentKdapp { get => ParentKode(Kdapp); set { } }

        public static bool IsRootCondition(SsappControl dc)
        {
            return (dc.Kdlevel == 1);
        }
        public new string[] GetKeys()
        {
            return new string[] { "Tahun","Idapp","Kdapp", "Nmapp", "Urapp"
        ,"Nchild","Nfiles","Progress"
        , "Status", "Statusicon", "Statusname", "Stricon"
        , "Progress","Progressstr","Progressicon"
        , "Last_by", "Last_date", "Rating"
        , "Url","Kdnmapp","UrlFull","Kdlevel","Type" };
        }

        public static List<SsappControl> GetChildren(List<SsappControl> domainset, SsappControl parent)
        {
            List<SsappControl> children = domainset.FindAll(o => o.Kdapp.StartsWith(parent.Kdapp) && (o.Kdlevel == parent.Kdlevel + 1));
            return children;
        }
        #endregion
        #region Sum All
        public static void Update(List<SsappControl> domainset)
        {
            List<SsappControl> roots = domainset.FindAll(i => IsRootCondition(i));
            if (roots.Count > 0)
            {
                foreach (SsappControl r in roots)
                {
                    UpdatedRecFields bobotProgress = SumChildren(domainset, r);
                    r.SetValue(bobotProgress);
                }
            }
        }

        private static UpdatedRecFields SumChildren(List<SsappControl> domainsetParent, SsappControl parent)
        {
            UpdatedRecFields localBobotProgress = new UpdatedRecFields();
            List<SsappControl> localDomainset = domainsetParent.FindAll(o => o.Kdapp.StartsWith(parent.Kdapp));

            List<SsappControl> children = GetChildren(localDomainset, parent);
            int localNumChildren = 0;
            if ((children.Count > 0) && (parent.Type == "H"))
            {
                localBobotProgress.Nchild = children.Count;
                foreach (SsappControl o in children)
                {
                    localNumChildren++;
                    UpdatedRecFields childrenBobotProgress = SumChildren(localDomainset, o);
                    localBobotProgress.Sum(childrenBobotProgress);
                }
                parent.SetValue(localBobotProgress);
                return localBobotProgress;
            }
            else
            {
                localBobotProgress.SetValue(parent);
                localBobotProgress.Nchild = 0;
                return localBobotProgress;
            }
        }

        private void SetValue(UpdatedRecFields rec)
        {
            Nchild = rec.Nchild;
            Progress = (rec.Nchild == 0) ? Progress : rec.Progress / rec.Nchild;
        }

        private class UpdatedRecFields
        {
            public int Nchild = 0;
            public decimal Progress = 0;

            public void Sum(UpdatedRecFields rec)
            {
                Nchild += rec.Nchild;
                Progress += rec.Progress;
            }

            public void SetValue(SsappControl rec)
            {
                Nchild = rec.Nchild;
                Progress = rec.Progress;
            }
        }
        #endregion

    }
    #endregion Ssapp
}

