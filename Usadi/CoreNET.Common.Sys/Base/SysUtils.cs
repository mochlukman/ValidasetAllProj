using CoreNET.Common.BO;
using System;

namespace CoreNET.Common.Base
{
  public class SysUtils : MasterAppConstants
  {

    public const string STR_AT = "1";
    public const string STR_KS = "2";
    public const string STR_KT = "2";
    public const string STR_PT = "3";
    public const string STR_WP = "4";
    public const string STR_PJ = "4";

    public const string KDGROUP_ADMIN_SISTEM = "01.01.";
    public const string KDGROUP_ADMIN_ANGGARAN = "05.10.";

    public const string KDMENU_MODUL_ANGGARAN = "21";
    public const string KDMENU_MODUL_KAS = "31";
    public const string KDMENU_MODUL_AKUNTANSI = "51";

    public const string KDDOK_APBD = "02.10.";
    public const string KDDOK_DPA = "03.10.";
    public const string KDDOK_SPD = "03.50.";
    public const string KDDOK_SP2D = "04.50.";
    public const string KDDOK_STS = "04.40.";
    public const string KDDOK_SPJ = "04.70.";

    //klo dibikin static, nggha bisa dioverride
    public SysUtils()
    {
      /*Default Configuration For Modul Admin*/
      MasterAppConstants.Instance.SetValue(MasterAppConstants.USERDC, "CoreNET.Common.BO.Ss10userLoginControl, CoreNET.Common.Sys");
      MasterAppConstants.Instance.SetValue(MasterAppConstants.GROUPDC, "CoreNET.Common.BO.Ss20groupControl, CoreNET.Common.Sys");
      MasterAppConstants.Instance.SetValue(MasterAppConstants.MENUDC, "CoreNET.Common.BO.Ss00appmenuAdminControl, CoreNET.Common.Sys");
      MasterAppConstants.Instance.SetValue(MasterAppConstants.CLASSDC, "CoreNET.Common.BO.Ss00appmenuAdminControl, CoreNET.Common.Sys");
      MasterAppConstants.Instance.SetValue(MasterAppConstants.LOOKUPCLASSDC, "CoreNET.Common.BO.Ss00appmenuAdminControl, CoreNET.Common.Sys");
      MasterAppConstants.Instance.SetValue(MasterAppConstants.DICTIONARYDC, "CoreNET.Common.BO.Ss90dictionaryControl, CoreNET.Common.Sys");
            //MasterAppConstants.Instance.SetValue(MasterAppConstants.MASTERAPPID, DEFAULT_MASTERAPP_ID);
      MasterAppConstants.Instance.SetValue(MasterAppConstants.MASTERAPPID, DefaultMasterAppId);
      MasterAppConstants.Instance.SetValue(MasterAppConstants.APPID, "27DD1F6E-511D-4637-BCA6-DE61082B7246");
      MasterAppConstants.Instance.SetValue(MasterAppConstants.URLBASE, GlobalAsp.GetBaseURL());
      MasterAppConstants.Instance.SetValue(MasterAppConstants.APPTITLE, "Modul Admin");

      if (MasterAppConstants.Instance.StatusTesting)
      {
        //Validator.ValidateCode();
      }
    }
    public static string GetKdpemda()
    {
      return ((Ss10userControl)GlobalExt.GetSessionUser()).Kdpemda;
    }
    public static string GetKdunit()
    {
      return ((Ss10userControl)GlobalExt.GetSessionUser()).Kdunit;
    }
    public static string GetFilterKddok()
    {
      if (GlobalExt.GetSessionGroup() != null)
      {
        string filterDok = ((Ss20groupControl)GlobalExt.GetSessionGroup()).GetGroupConfigs("FILTER_DOK");
        if (string.IsNullOrEmpty(filterDok))
        {
          if (GlobalExt.GetSessionMenu() != null)
          {
            //filterDok = ((SswebmenuControl)GlobalExt.GetSessionMenu()).Filterdok;
          }
          else
          {
            return string.Empty;
          }
        }
        return filterDok;
      }
      else
      {
        return string.Empty;
      }
    }
    public static bool GetEnableFilter()
    {
      bool enablefilter = false;
      try
      {
        bool enableFilterfromMenu = false;// ((SswebmenuControl)GlobalExt.GetSessionMenu()).EnableFilter;
        bool enableFilterfromUser = ((Ss10userControl)GlobalExt.GetSessionUser()).EnableFilter;
        enablefilter = string.IsNullOrEmpty(GlobalExt.GetRequestIdPrev()) && enableFilterfromMenu && enableFilterfromUser;
      }
      catch (Exception ex)
      {
        UtilityBO.Log(ex);
      }
      return enablefilter;
    }

    public static int GetModeEditableByMenuSession()
    {
      //Urutan prioritas ValidUnit,User,Group,Menu
      string strModeEditableFromGroup = string.Empty;// ((SswebgroupControl)GlobalExt.GetSessionGroup()).GetGroupConfigs("MODE_EDITABLE");
      int modeEditablefromGroup = -1;
      if (!string.IsNullOrEmpty(strModeEditableFromGroup))
      {
        modeEditablefromGroup = int.Parse(strModeEditableFromGroup);
      }
      int modeEditablefromUser = ((Ss10userControl)GlobalExt.GetSessionUser()).ModeEditable;
      int modeEditablefromMenu = -1;// ((SswebmenuControl)GlobalExt.GetSessionMenu()).Modeeditable;
      if (modeEditablefromUser != -1)
      {
        return modeEditablefromUser;
      }
      else
      {
        if (modeEditablefromGroup != -1)
        {
          return modeEditablefromGroup;
        }
        else
        {
          return modeEditablefromMenu;
        }
      }
    }


    public static string GetDefaultNodok()
    {
      string kdmenu = GlobalExt.GetRequestId();
      string nodok = string.Empty;// ((SswebgroupControl)GlobalExt.GetSessionGroup()).GetGroupConfigs(kdmenu,"DEFAULT_NODOK");
      return nodok;
    }
    public static void SetFormatNodok(BaseDataControlUIEntry dc)
    {
      SetFormatNodok("Nodok", dc);
    }
    public static void SetFormatNodok(string field, BaseDataControlUIEntry dc)
    {
      try
      {
        string kdmenu = GlobalExt.GetRequestId();
        string format = "{No}/{Kdunit}/{Year}";// ((SswebgroupControl)GlobalExt.GetSessionGroup()).GetGroupConfigs(kdmenu, "DEFAULT_NODOK");
        format = format.Replace("{Year}", GetCurrentYear().ToString());
        format = format.Replace("{Kdunit}", (string)dc.GetValue("Kdunit"));
        string sufix = format.Replace("{No}", string.Empty);
        dc.SetValue(field, format);
        UtilityUI.GetNoUrut(dc, field, 5, BaseDataControl.LAST, string.Empty, sufix);

      }
      catch (Exception ex)
      {
        UtilityBO.Log(dc, ex);
      }
    }

    public static string GetSatkerPPKD()
    {
      string kdmenu = GlobalExt.GetRequestId();
      string satkerppkd = string.Empty;//((SswebgroupControl)GlobalExt.GetSessionGroup()).GetGroupConfigs("SATKER_PPKD");
      return satkerppkd;
    }
    public static int GetCurrentYear()
    {
      if (GlobalExt.GetSessionUser() != null)
      {
        if (!string.IsNullOrEmpty((string)GlobalExt.GetSessionUser().GetValueProperty(GlobalExt.TAHUN)))
        {
          int year = int.Parse(GlobalExt.GetSessionUser().GetValueProperty(GlobalExt.TAHUN).ToString());
          return year;
        }
      }
      return DateTime.Now.Year;
    }
    public static void GetRangeDate(int mode, out DateTime tgl1, out DateTime tgl2)
    {
      UtilityBO.SetDate(GetCurrentYear(), mode, out tgl1, out tgl2);
    }

    public static int GetModeEditableByUserType()
    {
      Ss10userLoginControl user = (Ss10userLoginControl)GlobalAsp.GetSessionUser();
      string usertype = user.Usertype;
      switch (usertype)
      {
        case MasterAppConstants.STR_ADMIN:
          return ViewListProperties.MODE_EDITABLE_ADD_EDIT_DEL_LOADCSV;
        case MasterAppConstants.STR_DEVELOPER:
          return ViewListProperties.MODE_EDITABLE_ADD_EDIT_DEL_LOADCSV;
        case MasterAppConstants.STR_TESTER:
          return ViewListProperties.MODE_EDITABLE_READONLY;
        case Ss10usertypesControl.USER_LEVEL_1:
          return ViewListProperties.MODE_EDITABLE_ADD_EDIT_LOADCSV;
        case Ss10usertypesControl.USER_LEVEL_2:
          return ViewListProperties.MODE_EDITABLE_ADD_EDIT;
        case Ss10usertypesControl.USER_LEVEL_3:
          return ViewListProperties.MODE_EDITABLE_ADD_EDIT;
        case Ss10usertypesControl.USER_LEVEL_4:
          return ViewListProperties.MODE_EDITABLE_ADD_EDIT;
        case Ss10usertypesControl.USER_LEVEL_5:
          return ViewListProperties.MODE_EDITABLE_ADD_EDIT;
      }
      return ViewListProperties.MODE_EDITABLE_READONLY;
    }
  }
}
