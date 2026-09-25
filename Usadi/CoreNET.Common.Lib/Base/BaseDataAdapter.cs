using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.Serialization;

namespace CoreNET.Common.Base
{
    #region BaseDataAdapter
    public class BaseDataAdapter : IDeserializationCallback, IDisposable
    {
        public const int MODE_SQLSERVER = 0;
        public const int MODE_SQLCE = 1;
        public static object GetNullValue(System.Reflection.PropertyInfo prop, object val)
        {
            object oTemp = null;
            Type type = val.GetType();

            if (prop.PropertyType == typeof(DateTime))
            {
                oTemp = new DateTime();
            }
            else if (prop.PropertyType == typeof(int))
            {
                oTemp = -1;
            }
            else
            {
                oTemp = UtilityBO.GetDefault(prop.PropertyType);
            }
            return oTemp;
        }
        //2019-07-07 Error di Fitur Neraca Modul Akuntansi Primos (Akneraca.cs)
        public static List<IDataControl> GetListTreeDC(IDataControl ctrl, string sql, string fieldLevel)
        {
            string[] fields = ctrl.GetKeys();
            return GetListTreeDC(ctrl, sql, fieldLevel, fields);
        }
        public static List<IDataControl> GetListTreeDC(IDataControl ctrl, string sql, string fieldLevel, string[] fields)
        {
            int modedb = ((BaseBO)ctrl).ModeDB;
            string cs = ValidateCS(((BaseBO)ctrl).ConnectionString);
            DbConnection Connection = new SqlConnection(cs);
            DbCommand SelectCmd = new SqlCommand(ValidateSQL(sql), (SqlConnection)Connection);

            List<IDataControl> list = new List<IDataControl>();
            int max = 0;
            int counter = 0;
            try
            {
                Connection.Open();
                #region Try
                DbDataReader rdr = SelectCmd.ExecuteReader();
                try
                {
                    if (MasterAppConstants.Instance.LimitLoad)
                    {
                        max = 300;
                    }
                    Object prev = null;
                    List<string> fList = new List<string>();
                    for (int i = 0; i < fields.Length; i++)
                    {
                        try
                        {
                            rdr.GetOrdinal(fields[i]);
                            fList.Add(fields[i]);
                        }
                        catch { }
                    }

                    while (rdr.Read() && (++counter < max || max == 0))
                    {
                        IDataControl dc = (IDataControl)Activator.CreateInstance(ctrl.GetType());
                        for (int i = 0; i < fList.Count; i++)//fList sdh divalidasi
                        {
                            try
                            {
                                string pname = fList[i];
                                object val = rdr[pname];
                                System.Reflection.PropertyInfo prop = dc.GetProperty(pname);
                                if (prop.PropertyType == typeof(string))
                                {
                                    if (prop.CanWrite)
                                    {
                                        prop.SetValue(dc, (val.GetType() == typeof(DBNull)) ? GetNullValue(prop, val) :
                                        (val.GetType() == typeof(string)) ? ((string)val).Trim() : val.ToString().Replace("'", ""), null);
                                    }
                                }
                                else
                                {
                                    if (prop.CanWrite)
                                    {
                                        if (val.GetType() == typeof(DBNull))
                                        {
                                            prop.SetValue(dc, GetNullValue(prop, val), null);
                                        }
                                        else
                                        {
                                            UtilityBO.SetValueForProperty(dc, pname, val);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                UtilityBO.Log(ex);
                            }
                        }
                        if (prev != null)
                        {
                            try
                            {
                                string fieldLevelPrev = (string)prev.GetType().GetProperty(fieldLevel).GetValue(prev, null).ToString();
                                string fieldLevelCurrent = (string)dc.GetProperty(fieldLevel).GetValue(dc, null).ToString();
                                if (fieldLevelCurrent.StartsWith(fieldLevelPrev))
                                {
                                    prev.GetType().GetProperty("Type").SetValue(prev, "H", null);
                                }
                                else
                                {
                                    prev.GetType().GetProperty("Type").SetValue(prev, "D", null);
                                }
                            }
                            catch (Exception ex)
                            {
                                UtilityBO.Log(ex);
                            }
                        }

                        list.Add(dc);
                        prev = dc;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    rdr.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Close();
            }
            return list;
        }
        public static List<IDataControl> GetListDC(IDataControl ctrl, string sql)
        {
            string[] fields = ctrl.GetKeys();
            return GetListDC(ctrl, sql, fields);
        }
        public static List<IDataControl> GetListDC(IDataControl ctrl, string sql, string[] fields)
        {
            int modedb = ((BaseBO)ctrl).ModeDB;
            string cs = ValidateCS(((BaseBO)ctrl).ConnectionString);
            DbConnection Connection = new SqlConnection(cs);
            DbCommand SelectCmd = new SqlCommand(ValidateSQL(sql), (SqlConnection)Connection);

            List<IDataControl> list = new List<IDataControl>();
            #region Loading
            try
            {
                Connection.Open();
                DbDataReader rdr = SelectCmd.ExecuteReader();
                try
                {
                    List<string> fList = new List<string>();
                    for (int i = 0; i < fields.Length; i++)
                    {
                        try
                        {
                            rdr.GetOrdinal(fields[i]);
                            fList.Add(fields[i]);
                        }
                        catch { }
                    }
                    while (rdr.Read())
                    {
                        IDataControl dc = (IDataControl)Activator.CreateInstance(ctrl.GetType());
                        for (int i = 0; i < fList.Count; i++)
                        {
                            string pname = fList[i];
                            try
                            {
                                System.Reflection.PropertyInfo prop = dc.GetProperty(pname);
                                if (prop != null)
                                {
                                    object val = rdr[pname];
                                    if (prop.PropertyType == typeof(string))
                                    {
                                        if (prop.CanWrite)
                                        {
                                            prop.SetValue(dc, (val.GetType() == typeof(DBNull)) ? GetNullValue(prop, val) :
                                            (val.GetType() == typeof(string)) ? ((string)val).Trim() : val.ToString().Replace("'", ""), null);
                                        }
                                    }
                                    else
                                    {
                                        if (prop.CanWrite)
                                        {
                                            if (val.GetType() == typeof(DBNull))
                                            {
                                                prop.SetValue(dc, GetNullValue(prop, val), null);
                                            }
                                            else
                                            {
                                                UtilityBO.SetValueForProperty(dc, pname, val.ToString());
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (MasterAppConstants.Instance.StatusTesting)
                                    {
                                        throw new Exception(string.Format("No property with name={0}", pname));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                UtilityBO.Log(ex);
                            }
                        }
                        list.Add(dc);
                    }
                }
                catch (Exception ex) { UtilityBO.Log(ex); }
            }
            catch (Exception ex)
            {
                ctrl.SetValue("Debug", string.Format(@"
          <br/>Class Name = {0};
          <br/>Call Method = {1};
          <br/>Connection String= {2};
        ", ctrl.GetType().FullName, UtilityBO.GetCurrentMethod(1), ((BaseBO)ctrl).ConnectionString));
                UtilityBO.Log(ctrl, ex);
                //Exception newex = new Exception(((BaseBO)ctrl).ConnectionString, ex);
            }
            finally
            {
                Connection.Close();
            }
            #endregion
            return list;
        }
        public static List<BaseBO> GetListObject(BaseBO ctrl, string sql, string[] fields)
        {
            PropertyInfo prop = ctrl.GetType().GetProperty("ConnectionString");
            string cs = ValidateCS((string)prop.GetValue(ctrl, null));
            DbConnection Connection = new SqlConnection(cs);
            DbCommand SelectCmd = new SqlCommand(ValidateSQL(sql), (SqlConnection)Connection);

            List<BaseBO> list = new List<BaseBO>();
            #region Loading
            try
            {
                Connection.Open();
                DbDataReader rdr = SelectCmd.ExecuteReader();
                try
                {
                    List<string> fList = new List<string>();
                    for (int i = 0; i < fields.Length; i++)
                    {
                        try
                        {
                            rdr.GetOrdinal(fields[i]);
                            fList.Add(fields[i]);
                        }
                        catch { }
                    }
                    while (rdr.Read())
                    {
                        BaseBO dc = (BaseBO)Activator.CreateInstance(ctrl.GetType());
                        for (int i = 0; i < fList.Count; i++)
                        {
                            try
                            {
                                string pname = fList[i];
                                object val = rdr[pname];
                                prop = dc.GetType().GetProperty(pname);
                                if (prop.PropertyType == typeof(string))
                                {
                                    if (prop.CanWrite)
                                    {
                                        prop.SetValue(dc, (val.GetType() == typeof(DBNull)) ? GetNullValue(prop, val) :
                                        (val.GetType() == typeof(string)) ? ((string)val).Trim() : val.ToString().Replace("'", ""), null);
                                    }
                                }
                                else
                                {
                                    if (prop.CanWrite)
                                    {
                                        if (val.GetType() == typeof(DBNull))
                                        {
                                            prop.SetValue(dc, GetNullValue(prop, val), null);
                                        }
                                        else
                                        {
                                            UtilityBO.SetValueForProperty(dc, pname, val);
                                        }
                                        //prop.SetValue(dc, (val.GetType() == typeof(DBNull)) ? GetNullValue(prop, val) :
                                        //(val.GetType() == typeof(string)) ? ((string)val).Trim() : val, null);
                                    }
                                }
                            }
                            catch (Exception ex) { UtilityBO.Log(ex); }
                        }
                        list.Add(dc);
                    }
                }
                catch (Exception ex) { UtilityBO.Log(ex); }
                {
                }
            }
            catch (Exception ex) { UtilityBO.Log(ex); }
            finally
            {
                Connection.Close();
            }
            #endregion
            return list;
        }
        public static void ExecuteCmd(IDataControl ctrl, string sql)
        {
            int modedb = ((BaseBO)ctrl).ModeDB;
            string cs = ((BaseBO)ctrl).ConnectionString;
            ExecuteCmd(ValidateCS(cs), ValidateSQL(sql));
        }

        #region static
        private static DbCommand CreateCommandObject(string cs)
        {
            DbConnection con = null;
            DbCommand cmd = null;
            con = new SqlConnection(ValidateCS(cs));// SQLDataSource.Instance.GetSQLConnection(mode);
            cmd = new System.Data.SqlClient.SqlCommand
            {
                CommandTimeout = 600,
                Connection = (SqlConnection)con
            };
            return cmd;
        }
        public static string ValidateSQL(string sql)
        {
            //if (!CekAllowedApp())
            //{
            //  if (sql.ToLower().Contains("db")
            //    || sql.ToLower().Contains("sys")
            //    || sql.ToLower().Contains("tables")
            //    || sql.ToLower().Contains("column")
            //    )
            //  {

            //    throw new Exception("LBL_INVALID_QUERY");
            //  }
            //}
            return sql;
        }
        public static string ValidateCS(string cs)
        {
            //if (!CekAllowedApp())
            //{
            //  if (cs.ToLower().Contains("smart")
            //    || cs.ToLower().Contains("sys")
            //    )
            //  {

            //    throw new Exception("LBL_INVALID_QUERY");
            //  }
            //}
            return cs;
        }
        public static bool CekAllowedApp()
        {
            return true;
            //string idapp = MasterAppConstants.Instance.AppID;
            //ArrayList list = new ArrayList(new string[] {
            //  MasterAppConstants.Instance.MasterAppID
            //  ,MasterAppConstants.ID_APP_PROGRAMMER
            //});

            //return list.Contains(idapp);
        }
        public static void ExecuteCmd(string cs, string sql)
        {
            using (DbCommand cmd = CreateCommandObject(ValidateCS(cs)))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = ValidateSQL(sql);
                using (DbConnection con = cmd.Connection)
                {
                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        #endregion
        void IDeserializationCallback.OnDeserialization(object sender)
        {
            //Connection = SQLDataSource.Instance.GetSQLConnection(SQLDataSource.MODE_DB_OPERATIONAL);
        }
        public void Dispose()
        {
            //  if (_Connection != null)
            //  {
            //    if (_Connection.State == ConnectionState.Open)
            //    {
            //      _Connection.Close();
            //    }
            //    _Connection.Dispose();
            //  }
            //  if (_SelectCmd != null) _SelectCmd.Dispose();
            //  if (_UpdateCmd != null) _UpdateCmd.Dispose();
        }
    }
    #endregion
}
