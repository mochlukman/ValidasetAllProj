using CoreNET.Common.BO;
using System;
using System.Collections;

namespace CoreNET.Common.Base
{
  [Serializable]
  public abstract class BaseDataControlSys : BaseDataControlExt, IExtLoadCsv, IHasJSScript
  {
    public BaseDataControlSys()
    {
      ModeDB = SQLDataSource.MODE_DB_CONFIG;
      ConnectionString = SQLDataSource.Instance.CS_Config;
      CheckAuthentication();
    }
    public void CheckAuthentication()
    {
      try
      {
        //return;
        //Hanya modul admin dan developer
        if (!this.GetType().Name.ToLower().Contains("dict"))
        {
          if (string.IsNullOrEmpty(GlobalAsp.GetSessionApp()))
          {
            return;
          }

          bool isglobalasax = false;
          try
          {
            var o = System.Web.HttpContext.Current.Request;
          }
          catch (Exception)
          {
            isglobalasax = true;
          }
          if (!isglobalasax)
          {
            if (!CekAllowedApp(GlobalAsp.GetSessionApp())
              && !this.GetType().Name.ToLower().Contains("dict")
              && !string.IsNullOrEmpty(GlobalAsp.GetRequestId())
              && (UtilityUI.GetModePage() != UtilityUI.PAGE_UNDEFINED)
              && (UtilityUI.GetModePage() != UtilityUI.MAIN_MENU)
              )
            {
              throw new Exception("LBL_NOT_ALLOWED_APP");
            }
          }
        }
      }
      catch (Exception)
      {
        //errornya karena HttpContext.Current blm ada
      }
    }
    public int UpdateByUserType(string usertype, string idprop)
    {
      string idval = GetValue(idprop).ToString();
      string sqltemp = string.Empty;
      switch (usertype)
      {
        case SysUtils.STR_KT:
          sqltemp = "update {0} set REV_BY={1},REV_DATE={2} where {3}='{4}'";
          break;
        case SysUtils.STR_PT:
          sqltemp = "update {0} set APP_BY={1},APP_DATE={2} where {3}='{4}'";
          break;
        case SysUtils.STR_PJ:
          sqltemp = "update {0} set SIGN_BY={1},SIGN_DATE={2} where {3}='{4}'";
          break;
      }
      sqltemp = string.Format(sqltemp, XMLName, GlobalAsp.GetSessionUser().GetUserID(), DateTime.Now.ToString(), idprop, idval);//"yyyy-MM-dd HH:mm:ss"
      try
      {
        BaseDataAdapter.ExecuteCmd(this, sqltemp);
        return 1;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public static bool CekAllowedServer()
    {
      ArrayList list = new ArrayList(new string[] {
        "http://localhost:8468/"
        ,"http://localhost:8294/"
        ,"http://localhost:9443/"
        ,"http://localhost/"
        ,"https://sipd.usadi.id/"
        ,"http://dev2019.usadi.co.id/"
        ,"https://token.sipkd.net:9443"
      });

      return list.Contains(GlobalAsp.GetBaseURL().ToLower()) && SQLDataSource.CekAllowedServer();
    }
    public static bool CekAllowedApp(string idapp)
    {
      ArrayList list = new ArrayList(new string[] {
        MasterAppConstants.Instance.MasterAppID
        ,MasterAppConstants.ID_APP_PROGRAMMER
      });

      return list.Contains(idapp);
    }
    private ViewListProperties cViewListProperties = null;
    public new IProperties GetProperties()
    {
      if (cViewListProperties == null)
      {
        cViewListProperties = (ViewListProperties)base.GetProperties();
        cViewListProperties.AllowKeepFormExpanded = true;
        if (MasterAppConstants.Instance.StatusAdmin)
        {
          cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_EDIT_DEL;
        }
        else
        {
          cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_READONLY;
        }
      }
      return cViewListProperties;
    }

    public new void FilterClick(string key)
    {
      if (key.Contains("Idapp"))
      {
        SsappControl dc = new SsappControl() { Idapp = Idapp, Kdapp = Kdapp, Nmapp = Nmapp };
        GlobalAsp.SetSessionData(dc);
      }
    }



    public static void SetFilterKey(BaseDataControlSys thisbo, BaseBO bo)
    {
      if (typeof(IDataControlMenu).IsInstanceOfType(bo))
      {
        BaseBO dcapp = (BaseBO)GlobalAsp.GetSessionData();
        if (dcapp != null)
        {
          thisbo.Idapp = dcapp.Idapp;
          thisbo.Kdapp = dcapp.Kdapp;
          thisbo.Nmapp = dcapp.Nmapp;
        }
      }
    }


  }
}
