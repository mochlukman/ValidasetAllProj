using CoreNET.Common.BO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;

namespace CoreNET.Common.Base
{
    public class GlobalAsp
    {
        #region Mode Application
        public const int MODE_APPLICATION_WEB = 1;
        public const int MODE_APPLICATION_DESKTOP = 2;
        public const int MODE_APPLICATION_MOBILE = 3;
        public static int MODE_APPLICATION = MODE_APPLICATION_WEB;
        #endregion
        #region Constants
        private const string APP = "app";
        private const string NO = "no";
        private const string ROLEID = "roleid";
        private const string SVR = "svr";
        private const string SUFFIX = "sufix";
        //private const string LASTROLEID = "lastroleid";

        public const int TIMEOUT = 3600000;
        public const int STATE_CANADD = 11;
        public const int STATE_EDITING = 12;

        public const string TAHUN = "Tahun";
        public const string LINK = "Link";
        public const string SELECT = "Select";
        public const string LOOKUP = "Lookup";

        public const string DELIMITER_MENU = ">";
        public const string ROOT = "Root";
        public const string SUFFIX_FK = "_FK";
        public const string LBL_DATE = "LBL_DATE";
        public const string LBL_INFO = "LBL_INFO";
        public const string LBL_INFORMATION = "LBL_INFORMATION";
        public const string MSG_NULL_SESSION = "MSG_NULL_SESSION";
        public static string LBL_CONFIG_INFORMATION => ConfigurationManager.AppSettings[LBL_INFORMATION];
        public static string LBL_CONFIG_NULL_SESSION => ConfigurationManager.AppSettings[MSG_NULL_SESSION];

        public const string LBL_CONFIRM = "LBL_CONFIRM";
        public const string LBL_YES = "LBL_YES";
        public const string LBL_NO = "LBL_NO";

        public const int LOOKUP_FILTER = 3;
        public const int FILTER = 1;
        public const int ENTRY = 2;

        public static string SESSION_INITKEY => "initkey";
        public const string SESSION_SCREEN_RES = "ScreenResolution";
        public static string ServerPath => HttpContext.Current.Server.MapPath(".");
        //public static string DataPath//deprecated GetDataDir
        //{
        //  get
        //  {
        //    return (string)CoreNET.Common.BO.SsappconfigLookupControl.Instance.Params["DataDir"];
        //  }
        //}
        #endregion
        public static string MSG_ERRORAPP => ConstantDict.Translate("LBL_CEK_APP_CONFIG=Application doesn't exist or you connect to wrong datasource");
        #region GetRequest
        //Redundant
        //UtilityUI.GetRequestExtPage();
        public static int GetModePage()
        {
            if (HttpContext.Current != null)
            {
                string url = HttpContext.Current.Request.Url.AbsoluteUri;
                if (url.Contains("PageMasterDetil.aspx"))
                {
                    return UtilityUI.PAGE_MASTER;
                }
                else if (url.Contains("PageForm.aspx"))
                {
                    return UtilityUI.PAGE_FORM;
                }
                else if (url.Contains("PageTabular.aspx"))
                {
                    return UtilityUI.PAGE_TABULAR;
                }
                else if (url.Contains("PageTreeGrid.aspx"))
                {
                    return UtilityUI.PAGE_TREE_GRID;
                }
                else if (url.Contains("PageTreeGridDetil.aspx"))
                {
                    return UtilityUI.PAGE_TREE_GRID_DETIL;
                }
                else if (url.Contains("PageTreePanelDetil.aspx"))
                {
                    return UtilityUI.PAGE_TREE_PANEL_DETIL;
                }
                else if (url.Contains("TextEditor.aspx"))
                {
                    return UtilityUI.PAGE_TEXT_EDITOR;
                }
                else
                {
                    return UtilityUI.PAGE_UNDEFINED;
                }
            }
            else
            {
                return UtilityUI.PAGE_UNDEFINED;
            }
        }
        public static string GetRequestURL()
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.Url.OriginalString;
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetRequestDc()
        {
            return HttpContext.Current.Request["dc"];
        }
        public static string GetRequestDebug()
        {
            return HttpContext.Current.Request["debug"];
        }
        public static string GetRequestEnable()
        {
            return HttpContext.Current.Request["enable"];
        }
        public static string GetRequestChild()
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request["child"];
            }
            else
            {
                return null;
            }
        }
        public static string GetRequestFrame()
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request["frame"];
            }
            else
            {
                return null;
            }
        }
        public static string GetRequestKdapp()
        {
            string kdapp = GlobalAsp.GetRequestUrlKdapp();
            if (string.IsNullOrEmpty(kdapp))
            {
                kdapp = HttpContext.Current.Request["kdapp"];

                if (string.IsNullOrEmpty(kdapp))
                {
                    string kd = ConfigurationManager.AppSettings["App"];
                    if (!string.IsNullOrEmpty(kd))
                    {
                        kdapp = kd.Trim();
                    }
                }
            }
            return kdapp;
        }
        public static string GetRequestApp()
        {
            string idapp = "1847B4EE-619F-45FE-9F5F-FD911B172601";//HttpContext.Current.Request["app"];
            return idapp;
        }
        public static string GetIdApp()
        {
            string idapp = HttpContext.Current.Request["app"];
            if (string.IsNullOrEmpty(idapp))
            {
                SsappControl dc = new SsappControl
                {
                    Kdapp = GetRequestKdapp()
                };
                dc.Load(BaseDataControl.PK);

                //dc = SsappLookupControl.FindAndSetValuesIntoByKdapp(dc);
                idapp = dc.Idapp;
            }

            return idapp;
        }
        public static string GetRequestAppPrev()
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
            {
                return HttpContext.Current.Request["appprev"];
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetRequestStatus()
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
            {
                return HttpContext.Current.Request["status"];
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetRequestToolbar()
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["tb"]) ? "1" : HttpContext.Current.Request["tb"];
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetRequestId()
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
            {
                return HttpContext.Current.Request["id"];
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetRequestIdPrev()
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
            {
                return HttpContext.Current.Request["idprev"];
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetRequestTarget()
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
            {
                return HttpContext.Current.Request["target"];
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetRequestType()
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
            {
                return HttpContext.Current.Request["type"];
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetRequestProject()
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
            {
                return HttpContext.Current.Request["project"];
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetRequestKode()
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
            {
                return HttpContext.Current.Request["kode"];
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetRequestMobile()
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
            {
                return HttpContext.Current.Request["mobile"];
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetRequestMode()
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
            {
                return HttpContext.Current.Request["mode"];
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetRequestKey()
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
            {
                return HttpContext.Current.Request["key"];
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetRequestIndex()
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
            {
                return HttpContext.Current.Request["idx"];
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetRequest(string par)
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
            {
                return HttpContext.Current.Request[par];
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetRequestVal()
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
            {
                return HttpContext.Current.Request["val"];
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetRequestSub()
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
            {
                return (HttpContext.Current.Request["sub"] != null) ? HttpContext.Current.Request["sub"] : string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }
        public static int GetRequestI()
        {
            if (HttpContext.Current != null)
            {
                return int.Parse(string.IsNullOrEmpty(HttpContext.Current.Request["i"]) ? "1" : HttpContext.Current.Request["i"]);
            }
            else
            {
                return 0;
            }
        }
        public static int GetRequestIPrev()
        {
            if (HttpContext.Current != null)
            {
                return int.Parse(string.IsNullOrEmpty(HttpContext.Current.Request["iprev"]) ? "0" : HttpContext.Current.Request["iprev"]);
            }
            else
            {
                return 0;
            }
        }
        public static int GetLastIndexFromRequestI()
        {
            if (HttpContext.Current != null)
            {
                string strI = GetRequestI().ToString();
                return int.Parse(strI[strI.Length - 1].ToString());
            }
            else
            {
                return 0;
            }
        }
        #endregion
        #region Session User,Users dan Group
        protected static string SESSION_DAO_MANAGER => "DaoManager";
        protected static string SESSION_DATA_GROUP => "DataGroup" + GetSessionApp();
        protected static string SESSION_DATA_USER => "DataUser" + GetSessionApp();
        protected static string SESSION_DATA_USER_PREV => "DataUser" + GetRequestAppPrev();
        protected static string SESSION_USER_KEY => "UserKey" + GetSessionApp();
        protected static string SESSION_DATA_USERS => "DataUsers" + GetSessionApp();

        #region Constant
        public const string USER_ADMIN = "A";
        public const string USER_DEVELOPER = "D";//Programmer
        public const string USER_TESTER = "T";
        public const string USER_LEVEL_1 = "1";
        public const string USER_LEVEL_2 = "2";
        public const string USER_LEVEL_3 = "3";
        public const string USER_LEVEL_4 = "4";
        public const string USER_LEVEL_5 = "5";
        #endregion

        public static string GetSessionKey()
        {
            return (string)HttpContext.Current.Session[SESSION_USER_KEY];
        }
        public static void SetSessionUser(IDataControlAppuser dc)
        {
            HttpContext.Current.Session[SESSION_DATA_USER] = dc;
        }
        private static IDataControlAppuserUI GetUserControl(string appid, string key)
        {
            HttpContext.Current.Session[SESSION_USER_KEY] = key;
            string decodedString = DecryptStringAES(key);/*Pake GlobalAsp.GetSessionApp();+Date*/
            CoreNET.Common.Base.AssemblyUtils.WriteEndLog();
            if (decodedString == "keyError")
            {
                throw new Exception(ConstantDict.Translate("LBL_ERROR_MUST_REFRESH"));
            }
            string[] strs = decodedString.Split(new string[] { "|" }, StringSplitOptions.None);
            string userID = strs[0];
            string userPwd = strs[1];
            #region Variable Session User
            IDataControlAppuserUI cUser = (IDataControlAppuserUI)HttpContext.Current.Session[GlobalAsp.SESSION_DATA_USER];
            if (cUser == null)
            {
                string usercname = (string)GlobalAsp.GetSessionAppValue(MasterAppConstants.USERDC);
                if (string.IsNullOrEmpty(usercname))
                {
                    throw new Exception(GlobalAsp.MSG_ERRORAPP);
                }
                cUser = (IDataControlAppuserUI)UtilityBO.Create(usercname);
            }
            cUser.SetUserID(GetSessionApp(), userID);

            /*load dari db, bukan Singleton*/
            /*klo session null bikin crash*/
            IDataControlAppuserUI bo = (IDataControlAppuserUI)(cUser.Load());//lgs dr db
            CoreNET.Common.Base.AssemblyUtils.WriteEndLog();

            if (bo == null)
            {
                string msg = ConstantDict.Translate("LBL_ERROR_USER_NOT_EXIST");
                if (msg.Contains("{0}"))
                {
                    msg = string.Format(msg, userID);
                }
                throw new Exception(msg);
            }
            if (
              !SQLDataSource.GetSQLInstance().ToLower().Equals("dev-2019svr")
              && !SQLDataSource.GetSQLInstance().ToLower().Equals("dev2019.usadi.co.id")
              && !SQLDataSource.GetSQLInstance().ToLower().Contains(@".\exp")
              && !SQLDataSource.GetSQLInstance().ToLower().Contains(@"token")
              && !SQLDataSource.GetOpDB(SQLDataSource.Instance.CS_Config).ToLower().Equals("dbakm")
              && !HttpContext.Current.Request.Url.AbsoluteUri.Contains("54321")//Buat Embedded IISExpress 
              && !HttpContext.Current.Request.Url.AbsoluteUri.Contains("27002"))//
            {
                try
                {
                    if (!CekHashData(bo))
                    {
                        var usertype = bo.GetValue("Usertype");
                        string validkey = GetHashData(bo);
                        string validAdmin = GlobalAsp.EncryptTypeAdmin(bo);
                        Exception inner = new Exception(usertype.Equals("A") ? validAdmin : validkey);
                        throw new Exception("ERROR_DATA_USER", inner);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            CoreNET.Common.Base.AssemblyUtils.WriteEndLog();

            string chiperreset = UtilityBO.GetMD5HexStringForStorePwd("dt1u54d1!");
            string chiper = UtilityBO.GetMD5HexStringForStorePwd(userPwd);/*Pake nama library, usadi.valid49.sys*/
            if (!chiper.Equals(bo.GetUserPwd()))
            {
                if (MasterAppConstants.Instance.StatusTesting)
                {
                    throw new Exception(ConstantDict.Translate("LBL_ERROR_PASSWORD_INVALID") + $"reset:{chiperreset}");
                }
                else
                {
                    throw new Exception(ConstantDict.Translate("LBL_ERROR_PASSWORD_INVALID"));
                }
            }
            //cUser.Load();//dari singleton
            #region Log
            BaseBO basebo = (BaseBO)bo;
            basebo.Idapp = appid;
            if (MasterAppConstants.Instance.StatusTesting)
            {
                string msg = $@"\n<p>UserKey={key}</p>";
                string pk = $@"\n<p>PublickKey={GlobalLib.PublicKey}</p>";
                bo.SetValue("Debug", bo.GetValue("Debug") + msg + pk);
            }
            LogUtils.Log(basebo, LogUtils.STR_EVENT_LOGIN);
            #endregion
            CoreNET.Common.Base.AssemblyUtils.WriteEndLog();
            #endregion
            return bo;
        }
        public static bool CekHashData(IDataControlAppuser dcUser)
        {
            string userType = (string)dcUser.GetValue("Usertype");
            if ((!string.IsNullOrEmpty(userType)) && (userType.Equals(MasterAppConstants.STR_ADMIN)))
            {
                string datasource = GlobalAsp.GetDataSource();
                string chiper = GlobalAsp.EncryptTypeAdmin(dcUser);
                bool valid = chiper.Equals(dcUser.GetValue("Userket")) || SQLDataSource.CekAllowedServer(datasource);
                if (MasterAppConstants.Instance.StatusTesting && !valid)
                {
                    string key = (GlobalAsp.GetDataSource() + GlobalAsp.GetDataSourceDB() + chiper);
                    throw new Exception(string.Format("Error key for {0}", key));
                }
                return valid;
            }
            else
            {
                string chiper = GetHashData(dcUser);
                bool valid = chiper.Equals(dcUser.GetValue("Userket"));
                return valid;
            }
        }
        public static bool IsClientSuperCreator()
        {
            string compnane = UtilityUI.GetClientCompName();
            return true;
        }
        public const string CLIENT_SUPER_CREATOR = "hy-dell";
        public static string EncryptTypeAdmin(IDataControlAppuser dcUser)
        {
            if (IsClientSuperCreator())
            {
                string key = (GlobalAsp.GetDataSource() + GlobalAsp.GetDataSourceDB() + GetHashData(dcUser));
                return UtilityBO.GetDotNetHash(key, 8);
            }
            else
            {
                throw new Exception(ConstantDict.Translate("ERROR_INVALID_CLIENT"));
            }
        }
        public static string GetHashData(IDataControlAppuser dcUser)
        {
            string id = GlobalAsp.GetDataSourceDB() + MasterAppConstants.Instance.MasterAppID;
            string challenge = id
              + dcUser.GetValue("Userid")
              + dcUser.GetValue("Usertype")
              + dcUser.GetValue("Kdpemda")
              + dcUser.GetValue("Usernama")
              + dcUser.GetValue("Userpwd")
              + dcUser.GetValue("Userblock")
              + dcUser.GetValue("Usernip")
              + dcUser.GetValue("Userhp")
              + dcUser.GetValue("Useremail")
              + dcUser.GetValue("Useruraian")
              + string.Empty;
            //return challenge.GetHashCode();
            return UtilityBO.GetDotNetHash(challenge, 8);//dcUser.Last_by + dcUser.Last_date, selalu berubah
        }
        //public static IDataControlAppuserUI SetUserKey(string appid, string key)
        //{
        //  IDataControlAppuserUI cUser = GetUserControl(appid, key);
        //  return cUser;
        //}
        public static void SetSessionUser(string appid, string key)
        {
            IDataControlAppuserUI cUser = GetUserControl(appid, key);
            #region Variable Session User
            HttpContext.Current.Session[GlobalAsp.SESSION_DATA_USER] = cUser;//untuk SSO ngga berlaku, ganti ke code di line berikutnya
            BaseBO.ClientUserID = GlobalAsp.GetSessionUser().GetUserID();
            BaseBO.ClientCompIP = UtilityUI.GetClientCompIP();
            BaseBO.ClientCompName = UtilityUI.GetClientCompName();
            cUser.SetUserOL(cUser.GetUserID(), key);
            #endregion
            #region Variable Application Users
            ArrayList cUsers = null;
            Object sesUsers = HttpContext.Current.Application[GlobalAsp.SESSION_DATA_USERS];
            if (sesUsers == null)
            {
                cUsers = new ArrayList();
            }
            else
            {
                cUsers = (ArrayList)sesUsers;
            }
            if (cUser != null)
            {
                if (!cUsers.Contains(cUser.GetUserID().Trim()))
                {
                    cUsers.Add(cUser.GetUserID().Trim());
                }
            }
            HttpContext.Current.Application[GlobalAsp.SESSION_DATA_USERS] = cUsers;
            #endregion
        }
        public static void UpdatePassword(string key)
        {
            string str = key;
            int mod4 = str.Length % 4;
            if (mod4 > 0)
            {
                str += new string('=', 4 - mod4);
            }
            byte[] data = Convert.FromBase64String(str);
            string decodedString = Encoding.UTF8.GetString(data);
            string[] strs = decodedString.Split(new string[] { "|" }, StringSplitOptions.None);
            string userID = strs[0];
            string userPwd = strs[1];
            string newUserPwd = strs[2];
            #region Variable Session User
            IDataControlAppuserUI cUser = (IDataControlAppuserUI)HttpContext.Current.Session[GlobalAsp.SESSION_DATA_USER];
            if (cUser == null)
            {
                string usercname = (string)GlobalAsp.GetSessionAppValue(MasterAppConstants.USERDC);
                if (string.IsNullOrEmpty(usercname))
                {
                    throw new Exception(GlobalAsp.MSG_ERRORAPP);
                }
                cUser = (IDataControlAppuserUI)UtilityBO.Create(usercname);
            }
            cUser.SetUserID(GetSessionApp(), userID);
            if (cUser.Load() == null)
            {
                throw new Exception(ConstantDict.Translate("LBL_ERROR_USER_NOT_EXIST"));
            }
            //string chiper = En(userPwd, KeyStr);
            string chiper = UtilityBO.GetMD5HexStringForStorePwd(userPwd);
            if (!chiper.Equals(cUser.GetUserPwd()))
            {
                throw new Exception(ConstantDict.Translate("LBL_ERROR_PASSWORD_INVALID"));
            }
            //chiper = En(newUserPwd, KeyStr);
            chiper = UtilityBO.GetMD5HexStringForStorePwd(newUserPwd);
            cUser.UpdatePwd(chiper);
            HttpContext.Current.Session[GlobalAsp.SESSION_DATA_USER] = cUser;
            #endregion
        }
        public static IDataControlAppuserUI GetSessionUser()
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Session != null))
            {
                IDataControlAppuserUI cuser = (IDataControlAppuserUI)HttpContext.Current.Session[GlobalAsp.SESSION_DATA_USER];
                if (cuser == null)
                {
                    cuser = (IDataControlAppuserUI)HttpContext.Current.Session[GlobalAsp.SESSION_DATA_USER_PREV];
                }
                return cuser;
            }
            else
            {
                return null;
            }
        }
        public void RemoveSessionUser()
        {
            ArrayList cUsers = null;
            Object sesUsers = HttpContext.Current.Application[GlobalAsp.SESSION_DATA_USERS];
            if (sesUsers == null)
            {
                cUsers = new ArrayList();
            }
            else
            {
                cUsers = (ArrayList)sesUsers;
            }
            IDataControlAppuserUI cUser = (IDataControlAppuserUI)GlobalAsp.GetSessionUser();
            if (cUser != null)
            {
                if (cUsers.Contains(cUser.GetUserID().Trim()))
                {
                    cUsers.Remove(cUser.GetUserID().Trim());
                }
            }
            HttpContext.Current.Application[GlobalAsp.SESSION_DATA_USERS] = cUsers;
            HttpContext.Current.Application[GlobalAsp.SESSION_DATA_USER] = null;
        }
        public static ArrayList GetSessionUsers()
        {
            if (HttpContext.Current != null)
            {
                return (ArrayList)HttpContext.Current.Application[GlobalAsp.SESSION_DATA_USERS];
            }
            else
            {
                return new ArrayList();
            }
        }
        public static void SetSessionUsers(ArrayList cUsers)
        {
            HttpContext.Current.Application[GlobalAsp.SESSION_DATA_USERS] = cUsers;
        }
        public static IDataControlWebgroup GetSessionGroup()
        {
            IDataControlWebgroup cgroup = (IDataControlWebgroup)HttpContext.Current.Session[GlobalAsp.SESSION_DATA_GROUP];
            if (cgroup == null)
            {
                string groupdc = (string)GlobalAsp.GetSessionAppValue(MasterAppConstants.GROUPDC);
                string usercname = groupdc;
                cgroup = (IDataControlWebgroup)UtilityBO.Create(usercname);
                HttpContext.Current.Session[GlobalAsp.SESSION_DATA_GROUP] = cgroup;
            }
            return cgroup;
        }
        public static void SetSessionGroup(string kdgroup)
        {
            IDataControlWebgroup cgroup = GlobalAsp.GetSessionGroup();
            cgroup.SetGroupID(kdgroup);
            cgroup.Load();
            HttpContext.Current.Session[GlobalAsp.SESSION_DATA_GROUP] = cgroup;
        }
        public static void SetSessionGroup(IDataControlWebgroup cGroup)
        {
            HttpContext.Current.Session[GlobalAsp.SESSION_DATA_GROUP] = cGroup;
        }
        public static void SetSessionData(string sesname, BaseBO data)
        {
            HttpContext.Current.Session[sesname] = data;
        }
        public static void SetSessionData(BaseBO data)
        {
            HttpContext.Current.Session[GlobalAsp.GetSessionApp()] = data;
        }
        public static BaseBO GetSessionData()
        {
            return (BaseBO)HttpContext.Current.Session[GlobalAsp.GetSessionApp()];
        }
        public static BaseBO GetSessionData(string sesname)
        {
            return (BaseBO)HttpContext.Current.Session[sesname];
        }
        #endregion
        #region Session List Menu and Last Menu
        //public const string SESSION_APP = "app";
        public static string SESSION_APP_PARAMS
        {
            get
            {
                if (!string.IsNullOrEmpty(GetRequestApp()))
                {
                    return "appparam" + GetRequestApp();
                }
                else
                {
                    string kdapp = ConfigurationManager.AppSettings["app"];
                    if (string.IsNullOrEmpty(kdapp))
                    {
                        kdapp = GetRequestKdapp();
                        //if (string.IsNullOrEmpty(kdapp))//redundant cyclic
                        //{
                        //  kdapp = GetRequestUrlKdapp();//redundan. 2019-09-25, prioritas GetRequestUrlKdapp dulu
                        //}
                    }
                    SsappControl dc = new SsappControl
                    {
                        Kdapp = kdapp
                    };
                    dc.Load();
                    return "appparam" + dc.Idapp;
                }
            }
        }
        //public static void SetSessionApp(string idapp)
        //{
        //  HttpContext.Current.Session[SESSION_APP] = idapp;
        //}
        public static object GetSessionAppValue(int mode)
        {
            Dictionary<int, object> Params = (Dictionary<int, object>)HttpContext.Current.Application[SESSION_APP_PARAMS];
            if (Params == null)
            {
                string idapp = GetIdApp();
                GlobalAsp.SetConfiguration(idapp, false);
            }
            if (Params != null)
            {
                if (Params.ContainsKey(mode))
                {
                    return Params[mode];
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
            //else
            //{
            //  return MasterAppConstants.Instance.Param[mode];
            //}
        }

        private static void SetSessionAppValue(int mode, object value)
        {
            Dictionary<int, object> Params = (Dictionary<int, object>)HttpContext.Current.Application[SESSION_APP_PARAMS];
            if (Params == null)
            {
                Params = new Dictionary<int, object>();
            }
            Params[mode] = value;
            HttpContext.Current.Application[SESSION_APP_PARAMS] = Params;
        }
        public static Dictionary<int, object> GetSessionAppParam()
        {
            Dictionary<int, object> Params = (Dictionary<int, object>)HttpContext.Current.Application[SESSION_APP_PARAMS];
            return Params;
        }
        public static string GetSessionMasterAppID()
        {
            if (HttpContext.Current != null)
            {
                Dictionary<int, object> Params = (Dictionary<int, object>)HttpContext.Current.Application[SESSION_APP_PARAMS];
                if (Params != null)
                {
                    return (string)Params[MasterAppConstants.MASTERAPPID];
                }
            }
            return string.Empty;
        }
        public static string GetSessionApp()
        {
            if (HttpContext.Current != null)
            {
                Dictionary<int, object> Params = (Dictionary<int, object>)HttpContext.Current.Application[SESSION_APP_PARAMS];
                if (Params != null)
                {
                    return (string)Params[MasterAppConstants.APPID];
                }
                else
                {
                    string idapp = GetRequestApp();
                    GlobalAsp.SetConfiguration(idapp, false);
                    return idapp;
                    //return MasterAppConstants.Instance.AppID;
                }
            }
            else
            {
                return null;
            }
        }
        //public const string SESSION_USERDC = "userdc";
        //public static void SetSessionUserDC(string userdc)
        //{
        //  HttpContext.Current.Session[SESSION_USERDC] = userdc;
        //}
        //public static string GetSessionUserDC()
        //{
        //  return (string)HttpContext.Current.Session[SESSION_USERDC];
        //}
        //public const string SESSION_GROUPDC = "groupdc";
        //public static void SetSessionGroupDC(string groupdc)
        //{
        //  HttpContext.Current.Session[SESSION_GROUPDC] = groupdc;
        //}
        //public static string GetSessionGroupDC()
        //{
        //  return (string)HttpContext.Current.Session[SESSION_GROUPDC];
        //}

        public static string SESSION_LAST_MENU { get { return "lastmenu" + GlobalAsp.GetRequestApp(); ; } }
        public static string SESSION_LIST_MENU { get { return "listmenu" + GlobalAsp.GetRequestApp(); ; } }
        public static string SESSION_LIST_MENU_DEBUG { get { return "listmenudebug" + GlobalAsp.GetRequestApp(); ; } }
        public static string SESSION_LIST_MENU_ID => "listmenu" + GetRequestId();
        //public static string SESSION_LIST_MENU_ID => "listmenu" + GetRequestIndex();
        public static string SESSION_LIST_MENU_REF { get { return "listmenuref" + GlobalAsp.GetRequestApp(); ; } }
        public static void SetSessionListMenu(IList listmenu)
        {
            HttpContext.Current.Session[GlobalAsp.SESSION_LIST_MENU] = listmenu;
        }
        public static IList GetSessionListRows()
        {
            if (HttpContext.Current != null)
            {
                int id = GlobalAsp.GetRequestI();
                return GetSessionListRows(id);
            }
            else
            {
                return new ArrayList();
            }
        }
        public static IList GetSessionListRows(int id)
        {
            if (HttpContext.Current != null)
            {
                return (IList)HttpContext.Current.Session[GlobalAsp.SESSION_LIST_ROWS + id];
            }
            else
            {
                return new ArrayList();
            }
        }
        public static IList GetSessionListRows(string menuid, int id)
        {
            if (HttpContext.Current != null)
            {
                string session_name = "listrows" + menuid + "." + id;

                return (IList)HttpContext.Current.Session[session_name];
            }
            else
            {
                return new ArrayList();
            }
        }
        public static void SetSessionListRows(IList list)
        {
            int id = GlobalAsp.GetRequestI();
            SetSessionListRows(id, list);
        }
        public static void SetSessionListRows(int id, IList list)
        {
            HttpContext.Current.Session[GlobalAsp.SESSION_LIST_ROWS + id] = list;
        }

        public static IList GetSessionListMenu()
        {
            return (IList)HttpContext.Current.Session[GlobalAsp.SESSION_LIST_MENU];
        }
        public static void SetSessionListSubMenu(IList listmenu)
        {
            HttpContext.Current.Session[GlobalAsp.SESSION_LIST_MENU_ID] = listmenu;
        }
        public static IList GetSessionListSubMenu()
        {
            return (IList)HttpContext.Current.Session[GlobalAsp.SESSION_LIST_MENU_ID];
        }
        public static void SetSessionListDebugMenu(IList listmenu)
        {
            HttpContext.Current.Session[GlobalAsp.SESSION_LIST_MENU_DEBUG] = listmenu;
        }
        public static IList GetSessionLisDebugMenu()
        {
            return (IList)HttpContext.Current.Session[GlobalAsp.SESSION_LIST_MENU_DEBUG];
        }
        public static void SetSessionListMenuRef(IList listmenu)
        {
            HttpContext.Current.Session[GlobalAsp.SESSION_LIST_MENU_REF] = listmenu;
        }
        public static IList GetSessionListMenuRef()
        {
            return (IList)HttpContext.Current.Session[GlobalAsp.SESSION_LIST_MENU_REF];
        }
        #endregion
        #region GetConfigurationValue
        public static string GetBaseURLFull()
        {
            return GlobalAsp.GetBaseURL() + GlobalAsp.GetConfigURLPrefix();
        }
        public static string GetBaseURL()
        {
            return ConfigurationManager.AppSettings["URLBase"];
        }
        public static string GetConfigTitlePortal()
        {
            return ConfigurationManager.AppSettings["TitlePortal"];
        }
        public static string GetConfigPrefixPortal()
        {
            return ConfigurationManager.AppSettings["PrefixPortal"];
        }
        public static string GetConfigSubTitlePortal()
        {
            return ConfigurationManager.AppSettings["SubTitlePortal"];
        }
        public static string GetConfigLabelInfo()
        {
            return ConfigurationManager.AppSettings["LBL_INFORMASI"];
        }
        public static string GetConfigURLPrefix()
        {
            return ConfigurationManager.AppSettings["URLPrefix"];
        }
        public static string GetHomePortalURL()
        {
            return ConfigurationManager.AppSettings["URLHomePortal"];
        }
        public static string GetHomeURL()
        {
            string homeurl = (string)CoreNET.Common.BO.SsappconfigLookupControl.Instance.Params["URLHome"];
            if (string.IsNullOrEmpty(homeurl))
            {
                return ConfigurationManager.AppSettings["URLHome"];
            }
            else
            {
                return homeurl;
            }
        }
        public static string GetLogoutURL()
        {
            string homeurl = (string)CoreNET.Common.BO.SsappconfigLookupControl.Instance.Params["URLLogout"];
            if (string.IsNullOrEmpty(homeurl))
            {
                return ConfigurationManager.AppSettings["URLLogout"];
            }
            else
            {
                return homeurl;
            }
        }
        public static string GetDataDir()
        {
            string homeurl = (string)CoreNET.Common.BO.SsappconfigLookupControl.Instance.Params["DataDir"];
            if (string.IsNullOrEmpty(homeurl))
            {
                return ConfigurationManager.AppSettings["DataDir"];
            }
            else
            {
                return homeurl;
            }
        }
        public static string GetMenuURL()
        {
            string homeurl = ConfigurationManager.AppSettings["URLMenu"];
            if (string.IsNullOrEmpty(homeurl))
            {
                return (string)CoreNET.Common.BO.SsappconfigLookupControl.Instance.Params["URLMenu"];
            }
            else
            {
                return homeurl;
            }
        }
        public static string GetDataURL()
        {
            string homeurl = (string)CoreNET.Common.BO.SsappconfigLookupControl.Instance.Params["URLData"];
            if (string.IsNullOrEmpty(homeurl))
            {
                return ConfigurationManager.AppSettings["URLData"];
            }
            else
            {
                return homeurl;
            }
        }
        public static string GetBlankURL()
        {
            string homeurl = (string)CoreNET.Common.BO.SsappconfigLookupControl.Instance.Params["URLBlank"];
            if (string.IsNullOrEmpty(homeurl))
            {
                return ConfigurationManager.AppSettings["URLBlank"];
            }
            else
            {
                return homeurl;
            }
        }
        public static string GetDataSourceConfig()
        {
            return ConfigurationManager.AppSettings["DataSourceSys"];
        }
        public static string GetDataSource()
        {
            return ConfigurationManager.AppSettings["DataSource"];
        }
        public static string GetDataSourceDB()
        {
            return ConfigurationManager.AppSettings["DataSourceDB"];
        }
        #endregion
        #region Property CurrentException
        public static Exception CurrentException
        {
            get => (Exception)HttpContext.Current.Session["CURRENT_EXCEPTION"];
            set => HttpContext.Current.Session["CURRENT_EXCEPTION"] = value;
        }
        #endregion
        #region PrevObject
        //SESSION_OBJECT_PREV jadikan private ya
        public static IDataControlUIEntry GetEditingObject()
        {
            int i = GetRequestI();
            return (IDataControlUIEntry)HttpContext.Current.Session[SESSION_OBJECT_PREV + i];
        }
        public static void SetEditingObject(IDataControlUIEntry dc)
        {
            int i = GetRequestI();
            HttpContext.Current.Session[SESSION_OBJECT_PREV + i] = dc;
        }
        #endregion

        #region MenuID/GlobalAsp.GetSessionApp(); Management

        public static string PREFIXMENUID
        {
            get
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request[APP]))
                {
                    return "#0";
                }
                else
                {
                    return "#0" + HttpContext.Current.Request[APP];
                }
            }
        }
        public static string MENUID
        {
            get
            {
                if (IsGetPrev)
                {
                    return GetRequestIdPrev();
                }
                else
                {
                    return GetRequestId();
                }
            }
        }

        #region Property IsGetPrev//Klo static mutual conclusion
        public static bool IsGetPrev
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_ISPREV] == null)
                {
                    return false;
                }
                else
                {
                    return (bool)HttpContext.Current.Session[SESSION_ISPREV];
                }
            }
            set => HttpContext.Current.Session[SESSION_ISPREV] = value;
        }
        #endregion

        public static string SESSION_LANGUAGE { get { return "lang" + GlobalAsp.GetRequestApp(); ; } }
        public static string SESSION_TAHAP { get { return "thp" + GlobalAsp.GetRequestApp(); ; } }
        public static string SESSION_KDTAHAP { get { return "thp" + GlobalAsp.GetRequestApp(); ; } }
        public static string SESSION_JTESTPACK { get { return "jtestpack" + GlobalAsp.GetRequestApp(); ; } }
        public static string SESSION_CONFIG { get { return "config" + GlobalAsp.GetRequestApp(); ; } }
        public static string SESSION_PROJECT { get { return "project" + GlobalAsp.GetRequestApp(); ; } }
        public static string SESSION_AUDITOBJ { get { return "auditobj" + GlobalAsp.GetRequestApp(); ; } }
        public static string SESSION_WEBUSER { get { return "webuser" + GlobalAsp.GetRequestApp(); ; } }
        public static string SESSION_WEBUSER_TIM { get { return "webusertim" + GlobalAsp.GetRequestApp(); ; } }

        public static string SESSION_DB { get { return "db" + GlobalAsp.GetRequestApp(); ; } }//Current Used DB
        public static string SESSION_DB_CONFIG { get { return "configdb" + GlobalAsp.GetRequestApp(); ; } }
        public static string SESSION_DB_OPERATIONAL { get { return "operatedb" + GlobalAsp.GetRequestApp(); ; } }
        public static string SESSION_DB_LOG { get { return "logdb" + GlobalAsp.GetRequestApp(); ; } }

        //public static string SESSION_UID_PREFIX { get { return "uid"; } }
        //public static string SESSION_UID { get { return SESSION_UID_PREFIX + GlobalAsp.GetSessionApp();; } }
        //public static string SESSION_PWD_PREFIX { get { return "pwd"; } }
        //public static string SESSION_PWD { get { return SESSION_PWD_PREFIX + GlobalAsp.GetSessionApp();; } }
        //public static string SESSION_CS { get { return "cs" + GlobalAsp.GetSessionApp();; } }


        public static string CURRENT_DIR => "dir" + MENUID + "." + HttpContext.Current.Request["title"];

        public static string SESSION_ISPREV => "Isprev" + HttpContext.Current.Request["id"];

        public static string DATA_ROWS => "rows" + MENUID + ".";
        public static string SESSION_ERROR_CLASS => "errclass" + MENUID + ".";
        public static string SESSION_TEMP => "temp" + MENUID + ".";
        public static string SESSION_LINK => "link" + MENUID + ".";
        public static string SESSION_LINK2 => "link2" + MENUID + ".";
        public static string SESSION_LINK3 => "link3" + MENUID + ".";
        public static string SESSION_POSITION => "position" + MENUID + ".";
        public static string POSITION_FORM => "form" + MENUID + ".";
        public static string POSITION_LIST => "list" + MENUID + ".";
        public static string POSITION_DETIL => "detil" + MENUID + ".";
        public static string POSITION_DETIL_DETIL => "detildetil" + MENUID + ".";
        public static string SESSION_CURRENT_OBJECT => "curobj" + MENUID + ".";
        public static string SESSION_OBJECT_DEVELOPER_1 => "objdev1" + MENUID + ".";
        public static string SESSION_OBJECT_PREV => "objprev" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_REPORT => "objrpt" + MENUID + ".";
        public static string SESSION_OBJECT_MENU => "objmenu" + MENUID + ".";
        public static string SESSION_LIST_ROWS_PREFIX => "listrows#" + GlobalAsp.GetSessionApp() + "-";
        public static string SESSION_LIST_ROWS => "listrows" + MENUID + ".";
        public static string SESSION_LIST_ROWS1 => "listrows" + MENUID + ".1";
        public static string SESSION_LIST_ROWS11 => "listrows" + MENUID + ".11";
        public static string SESSION_LIST_ROWS2 => "listrows" + MENUID + ".2";
        public static string SESSION_LIST_ROWS3 => "listrows" + MENUID + ".3";
        public static string SESSION_LIST_LOOKUP_ROWS => "listlookuprows" + MENUID + ".";
        public static string SESSION_LIST_LOOKUP_ROWS1 => "listlookuprows" + MENUID + ".1";
        public static string SESSION_LIST_LOOKUP_ROWS2 => "listlookuprows" + MENUID + ".2";
        public static string SESSION_LIST_LOOKUP_ROWS3 => "listlookuprows" + MENUID + ".3";
        public static string SESSION_OBJECT_FOR_LIST1_PREFIX => "objlist1#" + GlobalAsp.GetSessionApp() + "-";
        public static string SESSION_OBJECT_FOR_LIST => "objlist" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST1 => "objlist1" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST2 => "objlist2" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST3 => "objlist3" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST4 => "objlist4" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_LOOKUP => "objlistlook" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_LOOKUP1 => "objlistlook1" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_LOOKUP2 => "objlistlook2" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_LOOKUP3 => "objlistlook3" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_LOOKUP4 => "objlistlook4" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL => "objlistdetil" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL1 => "objlistdetil1" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL1a => "objlistdetil1a" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL1b => "objlistdetil1b" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL1c => "objlistdetil1c" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL1d => "objlistdetil1d" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL2 => "objlistdetil2" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL2a => "objlistdetil2a" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL2b => "objlistdetil2b" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL2c => "objlistdetil2c" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL2d => "objlistdetil2d" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL3 => "objlistdetil3" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL3a => "objlistdetil3a" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL3b => "objlistdetil3b" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL3c => "objlistdetil3c" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL3d => "objlistdetil3d" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL4 => "objlistdetil4" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL4a => "objlistdetil4a" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL4b => "objlistdetil4b" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL4c => "objlistdetil4c" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL4d => "objlistdetil4d" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP => "objlistdetilloook" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP1 => "objlistdetilloook1" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP2 => "objlistdetilloook2" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP3 => "objlistdetilloook3" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP4 => "objlistdetilloook4" + MENUID + ".";

        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL => "objlistdetildetil" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL1 => "objlistdetildetil1" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL1a => "objlistdetildetil1a" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL1b => "objlistdetildetil1b" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL1c => "objlistdetildetil1c" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL1d => "objlistdetildetil1d" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL2 => "objlistdetildetil2" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL2a => "objlistdetildetil2a" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL2b => "objlistdetildetil2b" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL2c => "objlistdetildetil2c" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL2d => "objlistdetildetil2d" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL3 => "objlistdetildetil3" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL3a => "objlistdetildetil3a" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL3b => "objlistdetildetil3b" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL3c => "objlistdetildetil3c" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL3d => "objlistdetildetil3d" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL4 => "objlistdetildetil4" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL4a => "objlistdetildetil4a" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL4b => "objlistdetildetil4b" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL4c => "objlistdetildetil4c" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL4d => "objlistdetildetil4d" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP => "objlistdetilloookdetil" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP1 => "objlistdetilloookdetil1" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP1a => "objlistdetilloookdetil1a" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP1b => "objlistdetilloookdetil1b" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP1c => "objlistdetilloookdetil1c" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP1d => "objlistdetilloookdetil1d" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP2 => "objlistdetilloookdetil2" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP2a => "objlistdetilloookdetil2a" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP2b => "objlistdetilloookdetil2b" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP2c => "objlistdetilloookdetil2c" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP2d => "objlistdetilloookdetil2d" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP3 => "objlistdetilloookdetil3" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP3a => "objlistdetilloookdetil3a" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP3b => "objlistdetilloookdetil3b" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP3c => "objlistdetilloookdetil3c" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP3d => "objlistdetilloookdetil3d" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP4 => "objlistdetilloookdetil4" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP4a => "objlistdetilloookdetil4a" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP4b => "objlistdetilloookdetil4b" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP4c => "objlistdetilloookdetil4c" + MENUID + ".";
        public static string SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP4d => "objlistdetilloookdetil4d" + MENUID + ".";
        #endregion

        #region ComponenID
        public const string BTN_ADD1 = "BTN_ADD1";
        public const string BTN_ADD2 = "BTN_ADD2";
        public const string BTN_EDIT1 = "BTN_EDIT1";
        public const string BTN_EDIT2 = "BTN_EDIT2";
        public const string BTN_DEL1 = "BTN_DEL1";
        public const string BTN_DEL2 = "BTN_DEL2";
        public const string BTN_SAVE1 = "BTN_SAVE1";
        public const string BTN_SAVE_WIN1 = "BTN_SAVE_WIN1";
        public const string BTN_CANCEL1 = "BTN_CANCEL1";
        public const string BTN_BACK1 = "BTN_BACK1";
        public const string BTN_CLOSE1 = "BTN_CLOSE1";
        public const string BTN_REFRESH1 = "BTN_REFRESH1";
        public const string BTN_RESET1 = "BTN_RESET1";
        public const string BTN_SELECT1 = "BTN_SELECT1";
        public const string BTN_MASTER = "BTN_MASTER";
        public const string BTN_PREVIEW_TABLE = "BTN_PREVIEW_TABLE";
        public const string BTN_PREVIEW_TREE = "BTN_PREVIEW_TREE";
        public const string BTN_PREVIEW_TREE2 = "BTN_PREVIEW_TREE2";
        public const string BTN_LOADCSV1 = "BTN_LOADCSV1";
        public const string BTN_CSV1 = "BTN_CSV1";
        public const string BTN_WORD1 = "BTN_WORD1";
        public const string BTN_PRINT1 = "BTN_PRINT1";
        public const string BTN_PRINT2 = "BTN_PRINT2";
        public const string BTN_SETTING1 = "BTN_SETTING1";

        public const string BTN_XLS1 = "BTN_XLS1";
        public const string BTN_HELP1 = "BTN_HELP1";
        public const string BTN_FORUM1 = "BTN_FORUM1";
        public const string BTN_DEBUG1 = "BTN_DEBUG1";
        public const string BTN_MENU1 = "BTN_MENU1";
        public const string BTN_MENU2 = "BTN_MENU2";
        public const string BTN_FILE1 = "BTN_FILE1";
        public const string BTN_PROFILE1 = "BTN_PROFILE1";
        public const string BTN_FILE_MANAGEMENT = "BTN_FILE_MANAGEMENT";
        public const string BTN_CHANGE_PASSWORD = "BTN_CHANGE_PASSWORD";

        public const string MENU_INFO1 = "MENU_INFO1";
        public const string MENU_ENTRY1 = "MENU_ENTRY1";
        public const string MENU_ERROR1 = "MENU_ERROR1";
        public const string MENU_INFO2 = "MENU_INFO2";
        public const string MENU_ENTRY2 = "MENU_ENTRY2";
        public const string MENU_GRID1 = "MENU_GRID1";
        public const string MENU_TREE1 = "MENU_TREE1";
        public const string MENU_FILTER1 = "MENU_FILTER1";
        public const string MENU_COLS1 = "MENU_COLS1";
        public const string MENU_CONFIG1 = "MENU_CONFIG1";
        public const string MENU_CONFIG2 = "MENU_CONFIG2";

        public const string MENU_LISTMENU1 = "MENU_LISTMENU1";
        public const string MENU_EDITMENU1 = "MENU_EDITMENU1";
        public const string MENU_EDITCONFIG1 = "MENU_EDITCONFIG1";
        public const string MENU_EDITKEYS1 = "MENU_EDITKEYS1";
        public const string MENU_EDITROWS1 = "MENU_EDITROWS1";
        public const string MENU_EDITCOLS1 = "MENU_EDITCOLS1";
        public const string MENU_EDITTREECOLS1 = "MENU_EDITTREECOLS1";
        public const string MENU_EDITFILTERS1 = "MENU_EDITFILTERS1";
        public const string MENU_EDITMANUAL1 = "MENU_EDITMANUAL1";
        public const string MENU_EDITREF1 = "MENU_EDITREF1";
        public const string MENU_EDITFORUM1 = "MENU_EDITFORUM1";
        public const string MENU_EDITVERSI1 = "MENU_EDITVERSI1";

        public const string MENU_DICT1 = "MENU_DICT1";
        #endregion

        #region RefreshSession
        public static void RefreshSession()
        {
            #region Remove Session
            try
            {
                Hashtable MySessions = new Hashtable
        {
          { GlobalAsp.SESSION_DAO_MANAGER, HttpContext.Current.Session[GlobalAsp.SESSION_DAO_MANAGER] },
          { GlobalAsp.SESSION_DB, HttpContext.Current.Session[GlobalAsp.SESSION_DB] },
          { GlobalAsp.SESSION_DB_CONFIG, HttpContext.Current.Session[GlobalAsp.SESSION_DB_CONFIG] },
          { GlobalAsp.SESSION_DB_OPERATIONAL, HttpContext.Current.Session[GlobalAsp.SESSION_DB_OPERATIONAL] },
          { GlobalAsp.SESSION_LANGUAGE, HttpContext.Current.Session[GlobalAsp.SESSION_LANGUAGE] },
          { GlobalAsp.SESSION_KDTAHAP, HttpContext.Current.Session[GlobalAsp.SESSION_KDTAHAP] },
          { GlobalAsp.SESSION_DATA_USER, HttpContext.Current.Session[GlobalAsp.SESSION_DATA_USER] },
          { GlobalAsp.SESSION_DATA_USERS, HttpContext.Current.Session[GlobalAsp.SESSION_DATA_USERS] },
          { GlobalAsp.SESSION_DATA_GROUP, HttpContext.Current.Session[GlobalAsp.SESSION_DATA_GROUP] },
          { GlobalAsp.SESSION_SCREEN_RES, HttpContext.Current.Session[GlobalAsp.SESSION_SCREEN_RES] }
        };
                string session_name = (string)GlobalAsp.GetSessionGroup().GetValue("XMLName");
                MySessions.Add(session_name, HttpContext.Current.Session[session_name]);

                if (!(HttpContext.Current.Request["refresh"] != null && HttpContext.Current.Request["refresh"] == "0"))
                {
                    HttpContext.Current.Session.Clear();
                    HttpContext.Current.Session.RemoveAll();
                    System.GC.Collect();
                }

                HttpContext.Current.Session[GlobalAsp.SESSION_DAO_MANAGER] = MySessions[GlobalAsp.SESSION_DAO_MANAGER];
                HttpContext.Current.Session[GlobalAsp.SESSION_DB] = MySessions[GlobalAsp.SESSION_DB];
                HttpContext.Current.Session[GlobalAsp.SESSION_DB_CONFIG] = MySessions[GlobalAsp.SESSION_DB_CONFIG];
                HttpContext.Current.Session[GlobalAsp.SESSION_DB_OPERATIONAL] = MySessions[GlobalAsp.SESSION_DB_OPERATIONAL];
                HttpContext.Current.Session[GlobalAsp.SESSION_LANGUAGE] = MySessions[GlobalAsp.SESSION_LANGUAGE];
                HttpContext.Current.Session[GlobalAsp.SESSION_KDTAHAP] = MySessions[GlobalAsp.SESSION_KDTAHAP];
                HttpContext.Current.Session[GlobalAsp.SESSION_DATA_USER] = MySessions[GlobalAsp.SESSION_DATA_USER];
                HttpContext.Current.Session[GlobalAsp.SESSION_DATA_USERS] = MySessions[GlobalAsp.SESSION_DATA_USERS];
                HttpContext.Current.Session[GlobalAsp.SESSION_DATA_GROUP] = MySessions[GlobalAsp.SESSION_DATA_GROUP];
                HttpContext.Current.Session[GlobalAsp.SESSION_SCREEN_RES] = MySessions[GlobalAsp.SESSION_SCREEN_RES];
                HttpContext.Current.Session[session_name] = MySessions[session_name];
            }
            catch (Exception ex)
            {
                UtilityBO.Log(ex);
            }
            #endregion
        }
        #endregion
        #region CekSesion
        public const string DAO = "dao.config";
        public static string EncryptCSPublished(string text, string publickey)
        {
            if (CekSession())
            {
                return EncryptCS(text, publickey);
            }
            throw new Exception("You have to use your user key.");
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

        public static bool CekSessionGrid(Page page)//Ada security ceknya, simpen ke session, nanti di gridpanel dipanggil, jd method ini ngga bisa dihapus
        {
            if (!page.IsPostBack)
            {
                string script = AssemblyUtils.ReadFile("CoreNET.Common.Js.common.js");
                page.ClientScript.RegisterStartupScript(typeof(string), "common_js", script, true);
                script = AssemblyUtils.ReadFile("CoreNET.Common.Js.grid.js");
                page.ClientScript.RegisterStartupScript(typeof(string), "grid_js", script, true);
            }
            return CekSession();
        }
        public static bool CekSessionTree(Page page)//Ada security ceknya, simpen ke session, nanti di gridpanel dipanggil, jd method ini ngga bisa dihapus
        {
            string script = AssemblyUtils.ReadFile("CoreNET.Common.Js.common.js");
            page.ClientScript.RegisterStartupScript(typeof(string), "common_js", script, true);
            script = AssemblyUtils.ReadFile("CoreNET.Common.Js.tree.js");
            page.ClientScript.RegisterStartupScript(typeof(string), "tree_js", script, true);
            return CekSession();
        }
        public static bool CekSessionMenu(Page page)//Ada security ceknya, simpen ke session, nanti di gridpanel dipanggil, jd method ini ngga bisa dihapus
        {
            bool ok = false;
            if (!page.IsPostBack)
            {
                string script = AssemblyUtils.ReadFile("CoreNET.Common.Js.menu.js");
                page.ClientScript.RegisterStartupScript(typeof(string), "menu_js", script, true);

                if ((GetSessionAppParam() == null) && (!MasterAppConstants.Instance.StatusAdmin))
                {
                    //BuildDao();
                    //CoreNET.Common.Base.ConstantDict.Translate(idapp);//Inisialisasi Dictionary

                    MasterAppConstants.Instance.StatusTesting = (ConfigurationManager.AppSettings["Testing"] == "1");
                    if (MasterAppConstants.Instance.StatusTesting)
                    {
                        page.Title += " (Versi Beta)";
                    }
                    string idapp = GlobalAsp.GetRequestApp();
                    if (!string.IsNullOrEmpty(idapp))
                    {
                        SetConfiguration(idapp, false);
                    }
                    else
                    {
                        //Klo mode admin, keep following lines commented
                        //if (string.IsNullOrEmpty(GlobalAsp.GetRequestApp()))
                        //{
                        //  string urlhome = HttpContext.Current.Request.Url.OriginalString;
                        //  GlobalAsp.SetURLHome(urlhome);
                        //}
                    }
                    if (File.Exists("log.sbt"))
                    {
                        try
                        {
                            GlobalAsp.ReadObject("log.sbt");
                        }
                        catch (Exception ex)
                        {
                            UtilityBO.Log(ex);
                        }
                    }
                }
                #region Authentication
                if (GlobalAsp.GetSessionUser() == null)
                {
                    if (!page.GetType().Name.Equals("portal_aspx"))
                    {
                        string key = GetRequestKey();
                        if (string.IsNullOrEmpty(key))
                        {
                            throw new Exception("SESSION_EXPIRED");
                        }
                        key = key.Replace(" ", "+");
                        if (GlobalAsp.GetSessionApp() != null)
                        {
                            GlobalAsp.SetSessionUser(GlobalAsp.GetSessionApp(), key);
                            IDataControlWebgroup dcGroup = GlobalAsp.GetSessionGroup();
                            IDataControlAppuserUI dcUser = GlobalAsp.GetSessionUser();

                            dcGroup.SetFilterKey((BaseBO)dcUser);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                #endregion
                HttpContext.Current.Application[DAO] = "1";
            }
            ok = CekSession();
            return ok;
        }
        public static bool CekSessionPage(Page page)//Ada security ceknya, simpen ke session, nanti di gridpanel dipanggil, jd method ini ngga bisa dihapus
        {
            string script = AssemblyUtils.ReadFile("CoreNET.Common.Js.common.js");
            page.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), script, true);
            return CekSession();
        }
        public static bool CekSession()//Ada security ceknya, simpen ke session, nanti di gridpanel dipanggil, jd method ini ngga bisa dihapus
        {
            bool cek = true;
            IDataControlAppuserUI user = (IDataControlAppuserUI)GlobalAsp.GetSessionUser();
            MyDaoManager dao = MyDaoManager.Instance;
            if ((user == null) || (dao == null))
            {
                cek = false;
            }
            else
            {
                cek = Validate();//ini jg bisa untuk validasinya, tp method UtilityUI.CekSession msh bisa dihapus di pagenya
            }
            return cek;
        }
        public static string GetURLSessionKey()
        {
            string url = HttpContext.Current.Request.Url.OriginalString;
            if (HttpContext.Current.Request.UrlReferrer != null)
            {
                url = HttpContext.Current.Request.UrlReferrer.OriginalString;
            }
            string sessionkey = UtilityBO.GetDotNetHash(url, 16);
            return sessionkey;
        }

        #region Private
        private static bool Validate()
        {
            //CekRegistry
            if (!!string.IsNullOrEmpty((string)HttpContext.Current.Application[DAO]))
            {
                int n = BuildDao();
                if (n != -1)
                {
                    HttpContext.Current.Application[DAO] = "1";
                }
                if (HttpContext.Current.Application[DAO] == null)
                {

                    if (MasterAppConstants.Instance.StatusTesting)
                    {
                        throw new Exception("Error Build DAO. If problem resist, call the developer of this product");
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static int BuildDao()
        {
            try
            {
                MasterAppConstants.Instance.SetValue(MasterAppConstants.URLBASE, ConfigurationManager.AppSettings["URLBase"]);
                string cs_template = @"data source={0};Initial Catalog={1};user id={2};password={3};Asynchronous Processing=true";
                ////string cs = string.Format(cs_template, ".\sql2", "SMARTSYS", "usadi", "valid49");
                string cs = string.Format(cs_template, ".", "SMARTSYS", "usadi", "valid49");
                string chiper = EncryptCS(cs, GlobalLib.GetPublishedKey());
                //cs = string.Format(cs_template, "dev", "SIPD01DM", "sa", "password123!");
                //chiper = En(cs, KeyStr);
                SQLDataSource.Instance.SetDataSourceConfig(GlobalAsp.GetDataSourceConfig());
                IBatisNet.DataAccess.Configuration.DomDaoManagerBuilder builder = new IBatisNet.DataAccess.Configuration.DomDaoManagerBuilder();
                builder.Configure(DAO);
                //string script = GetScript();
                //page.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), script, true);
                //SetRegistry
                //script = GetPopUpScript();
                return 1;
            }
            catch (Exception ex)
            {
                if (typeof(System.Security.Cryptography.CryptographicException).IsInstanceOfType(ex))
                {
                    throw new Exception(string.Format(ex.Message + " on key = {0}", GlobalLib.GetPublishedKey()));
                }
                else
                {
                    throw ex;
                }
            }
        }
        private static string GetScript()
        {
            //Tambahkan script, klo tekan control-H akan muncul about, versi dari framework, di setiap page, dan pikirkan untuk 
            //mencegah programmer meremove it
            string script = @"
      
      ";
            return script;
        }
        private static string GetPopUpScript()
        {
            string script = string.Format(@"
        alert('{0}');
      ", "Core.NET Framework 2017");

            return script;
        }
        #endregion

        #endregion
        #region Userpwd
        static GlobalAsp()
        {
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
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                    catch
                    {
                    }
                }
            }
            return result;
        }
        private const int keysize = 256;
        private static byte[] initVectorBytes = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private static string KeyStr/*KeyStr untuk encrypted user, Biar bisa beda aplikasi, selama UserDC classnya sama*/
        {
            /*get
            {
                string DictionaryDC = (string)MasterAppConstants.DEFAULT_DICTIONARY_DC;//
                                                                                       //string DictionaryDC = (string)MasterAppConstants.Instance.DictionaryDC;//
                if (MasterAppConstants.Instance.StatusTesting)
                {
                    //DictionaryDC = (string)MasterAppConstants.Instance.DictionaryDC;//
                    DictionaryDC = (string)MasterAppConstants.DEFAULT_DICTIONARY_DC;//
                }
                Type t = Type.GetType(DictionaryDC);
                string assembly = t.Assembly.ManifestModule.Name.ToLower();
                string publickey = En(assembly, assembly);
                return publickey;
            }*/
            get
            {
                string publickey = GlobalLib.GetPublishedKey();
                //string publickey = "367052F4193AD3A9";
                //string str = MasterAppConstants.Instance.MasterAppID.Substring(0, 16);
                //@Todo PakeInterface, SysUtils itu bisa diakses disini.karena diakses sebelum code ini di global.asax
                string str = MasterAppConstants.Instance.MasterAppID.Substring(0, 16);

                string kek4key = En(str, str);
                string key4cs = En(publickey, kek4key);
                return key4cs;
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
        public static bool IsBase64(string base64String)
        {
            // Credit: oybek https://stackoverflow.com/users/794764/oybek
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
            {
                return false;
            }

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (Exception)
            {
                // Handle the exception
            }
            return false;
        }
        public static string Decrypt(string text)
        {
            if (CekSession())
            {
                return De(text, (string)MasterAppConstants.Instance.MasterAppID);
            }
            throw new Exception("You have to login first");
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

        #region public key encryption from browser to server (prevent sniffing)
        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.   
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold   
            // the decrypted text.   
            string plaintext = null;

            // Create an RijndaelManaged object   
            // with the specified key and IV.   
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                //Settings   
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.   
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.   
                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream   
                                // and place them in a string.   
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
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

        private static string DecryptStringAES(string cipherText)
        {
            byte[] keybytes = Encoding.UTF8.GetBytes(GlobalLib.PublicKey);
            byte[] iv = Encoding.UTF8.GetBytes(GlobalLib.PublicKey); //new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //Encoding.UTF8.GetBytes("8080808080808080");//

            byte[] encrypted = Convert.FromBase64String(cipherText);
            string decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }
        public static string GetRequestUrlKdapp()
        {
            string kdapp = string.Empty;
            string idapp = GlobalAsp.GetRequestApp();
            string urlFull = HttpContext.Current.Request.Url.OriginalString;
            try
            {
                string url = urlFull.Substring(0, urlFull.IndexOf("?"));
                //Sebelumnya BAHAYA, harus Request["key"] ada karakter "/", sekarang URL tanpa query string
                string[] strs = url.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                string path = url.Substring(0, url.IndexOf("?"));
                if (strs[strs.Length - 1].Contains(".aspx"))
                {
                    kdapp = strs[strs.Length - 2];//ambil virtual directorinya
                }
                else
                {
                    kdapp = strs[strs.Length - 1];//
                }

                Uri uri = new Uri(url);
                //string filename = string.Empty;
                //if (uri.IsFile)
                //{
                //  filename = System.IO.Path.GetFileName(uri.LocalPath);
                //}
                ;
                if (kdapp.Contains(uri.Host) || kdapp.All(Char.IsLetter))
                {
                    kdapp = string.Empty;
                }
                else
                {
                    kdapp += (kdapp.EndsWith(".")) ? string.Empty : ".";
                }
            }
            catch (Exception ex)
            {
                UtilityBO.Log(ex);
            }
            return kdapp;
        }
        public static void InitHome(Page page, System.Web.UI.WebControls.ImageButton btnLogin)
        {
            if (string.IsNullOrEmpty(SQLDataSource.Instance.CS_Config))
            {
                throw new Exception("Get your license. Your challenged key = " + GlobalLib.GetPublishedKey());
            }
            //bool ok = !string.IsNullOrEmpty((string)HttpContext.Current.Application[DAO]);
            string kdapp = string.Empty;
            string idapp = string.Empty;
            string cname = string.Empty;
            string idappfromreq = "0";
            string idappcosfail = "0";
            #region Proses
            Dictionary<int, object> Params = (Dictionary<int, object>)HttpContext.Current.Application[SESSION_APP_PARAMS];

            if ((Params == null) || string.IsNullOrEmpty((string)Params[MasterAppConstants.APPID])
              || Params[MasterAppConstants.APPID].Equals(MasterAppConstants.MASTERAPPID))
            {
                idapp = GlobalAsp.GetRequestApp();
                try
                {
                    if (string.IsNullOrEmpty(idapp))
                    {
                        kdapp = GetRequestKdapp();
                        if (string.IsNullOrEmpty(kdapp))
                        {
                            kdapp = GetRequestUrlKdapp();
                        }
                        kdapp = (kdapp.EndsWith(".")) ? kdapp : kdapp + ".";
                        cname = MasterAppConstants.Instance.AppDC;
                        IDataControl dc = UtilityBO.Create(cname);
                        dc.SetValue("Kdapp", kdapp);
                        ((SsappControl)dc).Load(BaseDataControl.PK);
                        if (dc != null)
                        {
                            idapp = (string)dc.GetValue("Idapp");
                        }
                        else
                        {
                            idappcosfail = "1";
                        }
                    }
                    else
                    {
                        idappfromreq = "1";
                    }
                }
                catch (Exception ex)
                {
                    UtilityBO.Log(ex);
                }
                if (!string.IsNullOrEmpty(idapp))
                {
                    SetConfiguration(idapp, false);
                }
                else
                {
                    //SessionConfigParam null, get from MasterAppConstant.Instance=>Modul Admin
                }
            }
            if (btnLogin != null)
            {
                //double
                ////btnLogin.OnClientClick = "return validateLogin();";
            }
            string script = AssemblyUtils.ReadFile("CoreNET.Common.Js.aes.js");
            //page.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), script, true);

            string publickey = GlobalLib.PublicKey;// ;
            script += @"
    function validateLogin() {
      var txtUserName = document.getElementById('txtUser').value.trim();
      var txtpassword = document.getElementById('txtPwd').value.trim();
      if (txtUserName == '') {
        alert('Please enter UserName');
        return false;
      }
      else if (txtpassword == '') {
        alert('Please enter Password');
        return false;
      } else {
        encode();
        document.getElementById('txtPwd').value = '';  
        //Ext.net.DirectMethods.ProcessLogin();      
        return true;
      }
    }
    var confirm = function () {
      if (confirm('Are you sure to delete?'))
        return true;
      else
        return false;
    };
    var encode = function () {
      var val1 = document.getElementById('txtUser').value;
      var val2 = document.getElementById('txtPwd').value;
      document.getElementById('utxt_Code').value = SubmitsEncry(val1 + '|' + val2);
      //document.getElementById('utxt_Code').value = b64EncodeUnicode(val1 + '|' + val2);
    };
    function b64EncodeUnicode(str) {
      // first we use encodeURIComponent to get percent-encoded UTF-8,
      // then we convert the percent encodings into raw bytes which
      // can be fed into btoa.
      return btoa(encodeURIComponent(str).replace(/%([0-9A-F]{2})/g,
        function toSolidBytes(match, p1) {
          return String.fromCharCode('0x' + p1);
        }));
    }
    function b64DecodeUnicode(str) {
      // Going backwards: from bytestream, to percent-encoding, to original string.
      return decodeURIComponent(atob(str).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
      }).join(''));
    }
    function SubmitsEncry(str) {

      var key = CryptoJS.enc.Utf8.parse('" + publickey + @"'); //'8080808080808080'
      var iv = CryptoJS.enc.Utf8.parse('" + publickey + @"');

      var encryptedlogin = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(str), key,
                {
                  keySize: 128 / 8,
                  iv: iv,
                  mode: CryptoJS.mode.CBC,
                  padding: CryptoJS.pad.Pkcs7
                });

      return encryptedlogin;
    } 
    ";
            page.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), script, true);
            #endregion Proses
            //string url = HttpContext.Current.Request.Url.OriginalString;
            kdapp = GetRequestUrlKdapp();
            idapp = GlobalAsp.GetSessionApp();
            string sub = GetRequestSub();
            cname = MasterAppConstants.Instance.AppDC;
            if (ConfigurationManager.AppSettings["ShowKodeApp"] == "1")
            {
                page.Title += string.Format("(Kdapp={0};Idapp={1};ByReq={2};ByFail={3};Sub={4};Cname={5})",
                  kdapp, idapp, idappfromreq, idappcosfail, sub, cname);
            }
            else
            {
                page.Title += " - " + GlobalAsp.GetSessionAppValue(MasterAppConstants.APPTITLE);
            }

        }
        #endregion
        #region Encoder/DecoderBase64String
        private string Decode(string encodedString)
        {
            string str = encodedString;
            int mod4 = str.Length % 4;
            if (mod4 > 0)
            {
                str += new string('=', 4 - mod4);
            }
            byte[] data = Convert.FromBase64String(str);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }
        /* Function JS
        function b64EncodeUnicode(str) {
          // first we use encodeURIComponent to get percent-encoded UTF-8,
          // then we convert the percent encodings into raw bytes which
          // can be fed into btoa.
          return btoa(encodeURIComponent(str).replace(/%([0-9A-F]{2})/g,
            function toSolidBytes(match, p1) {
              return String.fromCharCode('0x' + p1);
            }));
        }
        function b64DecodeUnicode(str) {
          // Going backwards: from bytestream, to percent-encoding, to original string.
          return decodeURIComponent(atob(str).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
          }).join(''));
        }
        */
        #endregion
        #endregion

        public static void ReadObject(string fname)
        {
            Stream serializationStream = File.Open(fname, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            SoftwareBit2 bit = (SoftwareBit2)formatter.Deserialize(serializationStream);
            serializationStream.Close();
            bit.Idapp = bit.Idapp;
            bit.AppTitle = bit.AppTitle;
            bit.MenuDC = bit.MenuDC;
            bit.NamaUnit = bit.NamaUnit;
            bit.ExpiredDuration = bit.ExpiredDuration;
            bit.Logo = bit.Logo;

            MasterAppConstants.Instance.SetValue(MasterAppConstants.APPID, bit.Idapp);
            MasterAppConstants.Instance.SetValue(MasterAppConstants.APPTITLE, bit.AppTitle);

            if (!MasterAppConstants.Instance.StatusTesting)
            {
                MasterAppConstants.Instance.SetValue(CoreNET.Common.Base.MasterAppConstants.MENUDC, De(bit.MenuDC, bit.Idapp));
            }
        }
        public static void SetConfiguration(string idapp, bool isstatic)
        {
            if (!string.IsNullOrEmpty(idapp))
            {
                if (isstatic)
                {
                    MasterAppConstants.Instance.SetValue(MasterAppConstants.APPID, idapp);
                    SsappconfigLookupControl.Instance.ResetParams(idapp);
                    MasterAppConstants.Instance.SetValue(MasterAppConstants.APPIDREF, (string)SsappconfigLookupControl.Instance.Params["AppIDRef"]);
                    MasterAppConstants.Instance.SetValue(MasterAppConstants.APPTITLE, (string)SsappconfigLookupControl.Instance.Params["AppName"]);
                    MasterAppConstants.Instance.SetValue(MasterAppConstants.USERDC, (string)SsappconfigLookupControl.Instance.Params["DCUser"]);
                    MasterAppConstants.Instance.SetValue(MasterAppConstants.GROUPDC, (string)SsappconfigLookupControl.Instance.Params["DCGroup"]);
                    MasterAppConstants.Instance.SetValue(MasterAppConstants.MENUDC, (string)SsappconfigLookupControl.Instance.Params["DCMenu"]);
                    MasterAppConstants.Instance.SetValue(MasterAppConstants.CLASSDC, (string)SsappconfigLookupControl.Instance.Params["DCClass"]);
                    MasterAppConstants.Instance.SetValue(MasterAppConstants.LOOKUPCLASSDC, (string)SsappconfigLookupControl.Instance.Params["DCLookupClass"]);
                    MasterAppConstants.Instance.SetValue(MasterAppConstants.DICTIONARYDC, (string)SsappconfigLookupControl.Instance.Params["DCDictionary"]);
                    MasterAppConstants.Instance.LogException = (ConfigurationManager.AppSettings["ShowLogException"] == "1");
                    //deprecated
                    //SQLDataSource.Instance.SetDataSource(CoreNET.Common.BO.SsappconfigLookupControl.Instance.Params);
                    MasterAppConstants.Instance.SetValue(MasterAppConstants.IDAPP_PROPERTY, (string)SsappconfigLookupControl.Instance.Params["PropertyIDAPP"]);
                    MasterAppConstants.Instance.SetValue(MasterAppConstants.NMAPP_PROPERTY, (string)SsappconfigLookupControl.Instance.Params["PropertyNMAPP"]);
                    MasterAppConstants.Instance.SetValue(MasterAppConstants.APPID, (string)SsappconfigLookupControl.Instance.Params["PropertyIDAPP"]);
                }
                else
                {
                    string cname = MasterAppConstants.Instance.AppDC;
                    IDataControlApp dc = (IDataControlApp)UtilityBO.Create(cname);

                    dc.ResetParams(idapp);
                    GlobalAsp.SetSessionAppValue(MasterAppConstants.APPID, idapp);
                    GlobalAsp.SetSessionAppValue(MasterAppConstants.APPIDREF, (string)dc.GetAppParam("AppIDRef"));// SsappconfigLookupControl.Instance.Params["AppName"]);
                    GlobalAsp.SetSessionAppValue(MasterAppConstants.APPTITLE, (string)dc.GetAppParam("AppName"));// SsappconfigLookupControl.Instance.Params["AppName"]);
                    GlobalAsp.SetSessionAppValue(MasterAppConstants.USERDC, (string)dc.GetAppParam("DCUser"));// SsappconfigLookupControl.Instance.Params["DCUser"]);
                    GlobalAsp.SetSessionAppValue(MasterAppConstants.GROUPDC, (string)dc.GetAppParam("DCGroup"));// SsappconfigLookupControl.Instance.Params["DCGroup"]);
                    GlobalAsp.SetSessionAppValue(MasterAppConstants.MENUDC, (string)dc.GetAppParam("DCMenu"));// SsappconfigLookupControl.Instance.Params["DCMenu"]);
                    GlobalAsp.SetSessionAppValue(MasterAppConstants.CLASSDC, (string)dc.GetAppParam("DCClass"));// SsappconfigLookupControl.Instance.Params["DCClass"]);
                    GlobalAsp.SetSessionAppValue(MasterAppConstants.LOOKUPCLASSDC, (string)dc.GetAppParam("DCLookupClass"));// SsappconfigLookupControl.Instance.Params["DCLookupClass"]);
                    GlobalAsp.SetSessionAppValue(MasterAppConstants.DICTIONARYDC, (string)dc.GetAppParam("DCDictionary"));// SsappconfigLookupControl.Instance.Params["DCDictionary"]);
                    MasterAppConstants.Instance.LogException = (ConfigurationManager.AppSettings["ShowLogException"] == "1");

                    GlobalAsp.SetSessionAppValue(MasterAppConstants.IDAPP_PROPERTY, (string)dc.GetAppParam("PropertyIDAPP"));// SsappconfigLookupControl.Instance.Params["PropertyIDAPP"]);
                    GlobalAsp.SetSessionAppValue(MasterAppConstants.NMAPP_PROPERTY, (string)dc.GetAppParam("PropertyNMAPP"));// SsappconfigLookupControl.Instance.Params["PropertyNMAPP"]);
                }
            }
        }

        public static bool GetExpired(Page page)
        {
            bool valid = false;
            string fname = page.Server.MapPath("~\\bin\\CoreNET.Common.UI.dll");
            FileInfo Fvalid = new FileInfo(fname);
            DateTime d = Fvalid.LastWriteTime;
            valid = d.AddDays(360) > DateTime.Now;
            return valid;
        }

        public static bool GetExpired(Page page, int n)
        {
            bool valid = false;
            string fname = page.Server.MapPath("~\\bin\\CoreNET.Common.UI.dll");
            FileInfo Fvalid = new FileInfo(fname);
            DateTime d = Fvalid.LastWriteTime;
            valid = d.AddDays(360 - n) > DateTime.Now;
            return valid;
        }

    }
}
