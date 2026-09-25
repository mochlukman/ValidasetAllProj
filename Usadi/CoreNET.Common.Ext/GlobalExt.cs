using CoreNET.Common.BO;
using System;
using System.Collections;
using System.Web;

namespace CoreNET.Common.Base
{
  public class GlobalExt : GlobalAsp
  {
    #region Session List Menu and Last Menu
    public static void SetSessionMenu(IDataControlMenu cMenu)
    {
      HttpContext.Current.Session[GlobalAsp.SESSION_LAST_MENU] = cMenu;
    }
    public static void SetSessionMenu(string roleid)
    {
      if (!string.IsNullOrEmpty(roleid))
      {
        IDataControlMenu cMenu = (IDataControlMenu)HttpContext.Current.Session[GlobalAsp.SESSION_LAST_MENU];
        if (cMenu == null)
        {
          string usercname = (string)GlobalAsp.GetSessionAppValue(MasterAppConstants.MENUDC);
          cMenu = (IDataControlMenu)UtilityBO.Create(usercname);
        }
        cMenu = cMenu.FindObject(roleid);
        if (cMenu == null)
        {
          ////X.Msg.Alert(ConstantDictExt.Translate(GlobalApp.LBL_INFO), "LBL_ERROR_FIND_MENU").Show();
        }
        HttpContext.Current.Session[GlobalAsp.SESSION_LAST_MENU] = cMenu;
      }
    }
    public static IDataControlMenu GetSessionMenu()
    {
      if (HttpContext.Current != null)
      {
        IDataControlMenu cMenu = (IDataControlMenu)HttpContext.Current.Session[GlobalAsp.SESSION_LAST_MENU];
        if (cMenu == null)
        {
          try
          {
            string usercname = (string)GlobalAsp.GetSessionAppValue(MasterAppConstants.MENUDC);
            if (string.IsNullOrEmpty(usercname))
            {
              //Modul Admin
              usercname = "CoreNET.Common.BO.Ss00appmenuControl, CoreNET.Common.Sys";
            }
            cMenu = (IDataControlMenu)UtilityBO.Create(usercname);
          }
          catch (Exception ex)
          {
            throw ex;
          }
        }
        IDataControlAppuser cUser = GlobalAsp.GetSessionUser();
        cMenu.SetPageKey();
        cMenu.SetFilterKey((BaseBO)cUser);
        HttpContext.Current.Session[GlobalAsp.SESSION_LAST_MENU] = cMenu;
        return cMenu;
      }
      else
      {
        return null;
      }
    }
    #endregion
    #region RefreshSession
    public static new void RefreshSession()
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
          { GlobalAsp.SESSION_SCREEN_RES, HttpContext.Current.Session[GlobalAsp.SESSION_SCREEN_RES] },
          { GlobalAsp.SESSION_LAST_MENU, HttpContext.Current.Session[GlobalExt.SESSION_LAST_MENU] }
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
        HttpContext.Current.Session[GlobalAsp.SESSION_LAST_MENU] = MySessions[GlobalAsp.SESSION_LAST_MENU];
        HttpContext.Current.Session[session_name] = MySessions[session_name];
      }
      catch (Exception ex)
      {
        UtilityBO.Log(ex);
      }
      #endregion
    }
    #endregion

    #region static GlobalExt
    static GlobalExt()
    {
    }
    #endregion
  }
}
