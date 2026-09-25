using CoreNET.Common.BO;
using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Text;

namespace CoreNET.Common.Base
{
    [Serializable]
    public abstract class BaseDataControl : BaseBO, IDataControl
    {
        #region IDataControl Members
        #region GetDaoSingleton()
        public IBaseDao GetDaoSingleton()
        {
            IBaseDao _Dao = null;
            if (_Dao == null)
            {
                _Dao = (IBaseDao)MyDaoManager.Instance.GetDao(typeof(IBaseDao), ModeDB, this);
            }
            return _Dao;
        }
        #endregion
        #region Methods
        public string GetTableName()
        {
            return XMLName;
        }
        public int GetRowCount()
        {
            return 0;
        }
        public string[] GetReadOnlyFields(int status)
        {
            return new string[] { };
        }
        #region IDataControl
        public void Insert(String label)
        {
            Cre_by = (string.IsNullOrEmpty(Cre_by) ? ClientUserID : Cre_by);
            Cre_date = DateTime.Now;
            Last_by = (string.IsNullOrEmpty(Last_by) ? ClientUserID : Last_by);
            Last_date = Cre_date;
            GetDaoSingleton().Insert(label, this, ModeDB);
            LogUtilsBase.Log(this, EventControl.EVENT_INSERT);
        }

        public int Update(String label)
        {
            /*Ada kemungkinan, sdh diapprove tp tetep bisa diupdate*/
            //if (Rating > 0)
            //{
            //  throw new Exception(ConstantDict.Translate("LBL_APPROVED_DATA"));
            //}
            Mod_by = ClientUserID;
            Mod_date = DateTime.Now;
            Last_by = Mod_by;
            Last_date = Mod_date;
            int n = GetDaoSingleton().UpdateBy(label, this, ModeDB);
            LogUtilsBase.Log(this, EventControl.EVENT_UPDATE);
            return n;
        }

        public int Delete(String label)
        {
            //Untuk log penghilangan jejak
            if ((Status == -1) || (MasterAppConstants.Instance.StatusAdmin && !Idapp.Equals(MasterAppConstants.Instance.MasterAppID)))
            {
                int n = GetDaoSingleton().DeleteBy(label, this, ModeDB);
                LogUtilsBase.Log(this, EventControl.EVENT_DELETE);
                return n;
            }
            else
            {
                Status = -1;
                int n = GetDaoSingleton().UpdateBy(label, this, ModeDB);
                LogUtilsBase.Log(this, EventControl.EVENT_DELETE);
                return n;
            }
        }
        public void Insert()
        {
            Insert(BaseDataControl.DEFAULT);
        }
        public int Update()
        {
            return Update(BaseDataControl.DEFAULT);
        }
        public int Delete()
        {
            return Delete(BaseDataControl.DEFAULT);
        }
        public IList View()
        {
            IList list = View(BaseDataControl.ALL);
            return list;
        }
        public BaseBO Load()
        {
            return Load(BaseDataControl.PK);
        }
        public IList View(String label)
        {
            IList list = null;
            try
            {
                list = GetDaoSingleton().QueryBy(label, this, ModeDB);
                //UtilityBO.Log(this, EventControl.EVENT_VIEW);
            }
            catch (Exception ex)
            {
                UtilityBO.Log(this, ex);
                if (MasterAppConstants.Instance.StatusTesting)
                {
                    throw new Exception(ex.Message + @"
                      \n1. Check Signature, does class have been extended from correct BASE CLASSES
                      \n2. Check Contructor, does CONNECTION STRING have been setted
                      \n3. Check if XML file is valid or not
                      ", ex);
                }
                else
                {
                    throw ex;
                }
            }
            return list;
        }

        public IList ExecSP(String label)
        {
            IList list = GetDaoSingleton().ExecSP(label, this, ModeDB);
            LogUtilsBase.Log(this, EventControl.EVENT_EXECSP);
            return list;
        }

        public BaseBO Load(String label)
        {
            IBaseDao dao = GetDaoSingleton();
            if (dao != null)
            {
                BaseBO bo = dao.LoadBy(label, this, ModeDB);
                if (bo != null)
                {
                    CopyPropertyBOFrom(bo);
                    LogUtilsBase.Log(this, EventControl.EVENT_LOAD);
                    return this;
                }
                else
                {
                    return bo;
                }
            }
            return null;
        }
        public string[] GetKeys()
        {
            return GetFields();
        }
        #endregion
        #endregion
        #endregion
        #region BaseDataControl Utils
        public void CopyPropertyBOTo(IDataControl dest_bo)//this is source, dest_bo is destination BO
        {
            ((BaseDataControl)dest_bo).CopyPropertyBOFrom(this);
        }
        public static decimal GetTotal(string label, IDataControl dc, string pname)
        {
            IList list = ((BaseDataControl)dc).View(label);
            decimal total = 0;
            foreach (IDataControl cCtrl in list)
            {
                Decimal temp = ((Decimal)cCtrl.GetType().GetProperty(pname).GetValue(cCtrl, null));
                total += Math.Round(temp, 2);
            }

            return total;
        }
        public static void ClearDataControl(IDataControl dc)
        {
            string[] fields = dc.GetKeys();
            for (int i = 0; i < fields.Length; i++)
            {
                string name = fields[i];
                PropertyInfo prop = dc.GetProperty(name);
                dc.SetValue(name, UtilityBO.GetDefault(prop.PropertyType));
            }
        }
        public static IList GetList(IDataControl dc)
        {
            return dc.View();
            //return GetList(dc, BaseDataControl.ALL);
        }
        public static IList GetList(IDataControl dc, string label)
        {
            return ((BaseDataControl)dc).View(label);
        }
        #endregion
        #region ILoadCsv
        public void ExecutedPreSQL(string tname, bool isdelete)
        {
        }
        //public void ExecutedPreSQL(string tname, string pk, string key, bool isdelete)
        //{
        //  if (isdelete)
        //  {
        //    string sqlQuery = string.Format(@"delete from {0} where {1}='{2}'", tname, pk, key);
        //    BaseDataAdapter.ExecuteCmd(ModeDB, sqlQuery);
        //  }
        //}
        //public void ExecutedPostSQL(string tname, string pk, string key, bool isdelete)
        //{
        //}
        public void ExecutedPostSQL(string tname, bool isdelete, string idmenu)
        {
            //string sqlQuery = string.Format(@"exec " + SQLDataSource.GetOpDB(SQLDataSource.Instance.CS_Config) + "..RevalidateByIdmenu '{0}'", idmenu);
            //BaseDataAdapter.ExecuteCmd(ConnectionString, sqlQuery);
        }
        public void ExecutedSQL(string tname, DataTable csvData, int counter, bool isdelete)
        {
            //IDataControl dc = UtilityBO.Create(this.GetType().AssemblyQualifiedName);
            //IDataControl dc = (IDataControl)this.Clone();
            IDataControl dc = (IDataControl)BaseBO.Clone(this);
            ExecutedSQL(dc, tname, csvData, counter, isdelete);
        }
        public static void ExecutedSQL(IDataControl dc, string tname, DataTable csvData, int counter, bool isdelete)
        {
            #region Properties
            string scol = string.Empty;
            //string lvlscol = "Kdlevel";
            //string typscol = "Type";
            string[] mappings = new string[] { };

            if (typeof(ILoadCsv).IsInstanceOfType(dc))
            {
                mappings = ((ILoadCsv)dc).GetLoadCsvColumns();
            }
            else
            {
                mappings = ((ICsv)dc).GetCsvColumns(MyDocumentFormat.DOC);
            }
            #endregion
            #region Construct Query
            StringBuilder stb = new StringBuilder();

            string kdakun = "";
            //string sql = "insert into " + tname + " (";
            //string val = "values (";

            for (int i = 0; i < mappings.Length; i++)
            {
                string[] maps = mappings[i].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

                string colname = maps[0];
                string propname = maps[0];
                if (maps.Length > 1)
                {
                    colname = maps[1];
                }

                DataRow dr = csvData.Rows[counter];
                string strValue = dr[colname].ToString().Trim();
                if (csvData.Columns[colname].DataType.Equals(typeof(decimal)))
                {
                    strValue = strValue.Replace(",", ".");
                }
                else if (maps[0].ToUpper().Equals(scol.ToUpper()))
                {
                    strValue = (strValue.EndsWith(".")) ? strValue : strValue + ".";
                    kdakun = strValue;
                }
                if (strValue.StartsWith("'0"))
                {
                    strValue = strValue.Replace("'0", "0");
                }
                //sql += maps[0];
                //val += string.Format("'{0}'", strValue.Replace("'", "''"));
                try
                {
                    UtilityBO.SetValueForProperty(dc, propname, strValue);
                }
                catch (Exception ex)
                {
                    UtilityBO.Log(dc, ex);
                }

                //if (i < mappings.Length - 1)
                //{
                //  sql += ",";
                //  val += ",";
                //}
            }
            if (!scol.Equals(string.Empty))
            {
                string[] kds = kdakun.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                string lvlakun = kds.Length.ToString();
                string type = (lvlakun == "5") ? "D" : "H";

                //sql += "," + lvlscol + "," + typscol;
                //val += ",'" + lvlakun + "','" + type + "'";
            }
            //sql += ")";
            //val += ")";
            //sql = sql + val;
            //stb.AppendLine(sql);
            //string sqlQuery = stb.ToString();

            dc.SetPageKey();
            dc.Load();
            dc.SetPrimaryKey();
            if (isdelete)
            {
                dc.Delete(BaseDataControl.DEFAULT);
            }
            try
            {
                dc.Insert();
            }
            catch (Exception ex1)
            {
                UtilityBO.Log(ex1);
                try
                {
                    dc.Update();
                }
                catch (Exception ex2)
                {
                    UtilityBO.Log(ex2);
                }
            }
            //deprecated
            //BaseDataAdapter.ExecuteCmd(ModeDB, sqlQuery);
            #endregion
        }
        public void ReadDataTable(DataTable csvData, int counter)
        {
            #region Properties
            string scol = string.Empty;
            string[] mappings = new string[] { };

            if (typeof(ILoadCsv).IsInstanceOfType(this))
            {
                mappings = ((ILoadCsv)this).GetLoadCsvColumns();
            }
            else
            {
                mappings = ((ICsv)this).GetCsvColumns(MyDocumentFormat.DOC);
            }
            #endregion
            #region Construct Query
            StringBuilder stb = new StringBuilder();

            string kdakun = "";

            for (int i = 0; i < mappings.Length; i++)
            {
                string[] maps = mappings[i].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

                string colname = maps[0];
                string propname = maps[0];
                if (maps.Length > 1)
                {
                    colname = maps[1];
                }

                DataRow dr = csvData.Rows[counter];
                string strValue = dr[colname].ToString().Trim();
                if (csvData.Columns[colname].DataType.Equals(typeof(decimal)))
                {
                    strValue = strValue.Replace(",", ".");
                }
                else if (maps[0].ToUpper().Equals(scol.ToUpper()))
                {
                    strValue = (strValue.EndsWith(".")) ? strValue : strValue + ".";
                    kdakun = strValue;
                }
                UtilityBO.SetValueForProperty(this, propname, strValue);
            }
            if (!scol.Equals(string.Empty))
            {
                string[] kds = kdakun.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                string lvlakun = kds.Length.ToString();
                string type = (lvlakun == "5") ? "D" : "H";
            }
            #endregion
        }

        #endregion

    }
}
