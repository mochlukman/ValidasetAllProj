using System.Collections.Generic;

namespace CoreNET.Common.Base
{
  public class MasterAppConstants
  {
    public const string ID_APP_STATUS = "68065186-FF1B-4EB3-8A6B-6D1FDCFD3BAD";//00.00.01.
    public const string ID_APP_PROGRAMMER = "2418FA19-7538-4FC6-A3FF-FD47B7BC6501";//00.00.02.
    public const string ID_APP_DEMO = "16c838dd-e9d3-4ded-8bb4-1ecc3ade422b";//00.00.03.

    private static MasterAppConstants _Instance = null;
    public static MasterAppConstants Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new MasterAppConstants();
        }
        return _Instance;
      }
    }

    private Dictionary<int, object> _Param = null;
    public Dictionary<int, object> Param
    {
      get
      {
        if (_Param == null)
        {
          _Param = new Dictionary<int, object>();
        }
        return _Param;
      }
    }

    #region Property StatusAdmin
    private bool _StatusAdmin = false;
    public bool StatusAdmin
    {
      get => _StatusAdmin;
      set => _StatusAdmin = value;
    }
    #endregion
    #region Property StatusTesting
    private bool _StatusTesting;
    public bool StatusTesting
    {
      get => _StatusTesting;
      set => _StatusTesting = value;
    }
    #endregion
    #region Property StatusServer
    private bool _StatusServer = true;
    public bool StatusServer
    {
      get => _StatusServer;
      set => _StatusServer = value;
    }
    #endregion
    #region Property LimitLoad
    private bool _LimitLoad = false;
    public bool LimitLoad
    {
      get => _LimitLoad;
      set => _LimitLoad = value;
    }
    #endregion

    #region Property IdappProperty
    public string IdappProperty => (string)MasterAppConstants.Instance.Param[MasterAppConstants.IDAPP_PROPERTY];
    #endregion
    #region Property NmappProperty
    public string NmappProperty => (string)MasterAppConstants.Instance.Param[MasterAppConstants.NMAPP_PROPERTY];
    #endregion
    #region Property LogException
    private bool _LogException = true;
    public bool LogException
    {
      get => _LogException;
      set => _LogException = value;
    }
    #endregion
    #region Property AppDC
    public string AppDC
    {
      get
      {
        string _AppDC = (string)MasterAppConstants.Instance.Param[MasterAppConstants.APPDC];
        if (string.IsNullOrEmpty(_AppDC))
        {
          _AppDC = "CoreNET.Common.BO.IDataControlAppuser, CoreNET.Common";
        }
        return _AppDC;
      }
    }
    #endregion
    #region Property UserDC
    public string UserDC
    {
      get
      {
        string _UserDC = (string)MasterAppConstants.Instance.Param[MasterAppConstants.USERDC];
        if (string.IsNullOrEmpty(_UserDC))
        {
          _UserDC = "CoreNET.Common.BO.IDataControlAppuser, CoreNET.Common";
        }
        return _UserDC;
      }
    }
    #endregion
    #region Property MenuDC
    public string MenuDC => (string)MasterAppConstants.Instance.Param[MasterAppConstants.MENUDC];
    #endregion
    #region Property GroupDC
    public string GroupDC => (string)MasterAppConstants.Instance.Param[MasterAppConstants.GROUPDC];
    #endregion
    #region Property ClassDC
    public string ClassDC => (string)MasterAppConstants.Instance.Param[MasterAppConstants.CLASSDC];
    #endregion
    #region Property LookupClassDC
    public string LookupClassDC => (string)MasterAppConstants.Instance.Param[MasterAppConstants.LOOKUPCLASSDC];
    #endregion
    #region Property DictionaryDC
    public const string DEFAULT_DICTIONARY_DC = "CoreNET.Common.BO.Ss90dictionaryControl, CoreNET.Common.Sys";
    public string DictionaryDC => (string)MasterAppConstants.Instance.Param[MasterAppConstants.DICTIONARYDC];
    #endregion
    #region Property MasterAppID
    //public string DEFAULT_MASTERAPP_ID = "27DD1F6E-511D-4637-BCA6-DE61082B7246";

    internal const string DEFAULT_MASTERAPP_ID = "27DD1F6E-511D-4637-BCA6-DE61082B7246";

    public static string DefaultMasterAppId
            => DEFAULT_MASTERAPP_ID;
    public string MasterAppID => (string)MasterAppConstants.Instance.Param[MasterAppConstants.MASTERAPPID];
    #endregion
    #region Property AppID
    public string AppID => (string)MasterAppConstants.Instance.Param[MasterAppConstants.APPID];
    #endregion
    #region Property AppTitle
    public string AppTitle => (string)MasterAppConstants.Instance.Param[MasterAppConstants.APPTITLE];
    #endregion
    #region Property URLBase
    public string URLBase => (string)MasterAppConstants.Instance.Param[MasterAppConstants.URLBASE];
    #endregion

    #region Property ShowContextMenu
    private bool _ShowContextMenu = true;
    public bool ShowContextMenu
    {
      get => _ShowContextMenu;
      set => _ShowContextMenu = value;
    }
    #endregion

    #region Property ShowTranslatedLabel
    private bool _ShowTranslatedLabel = true;
    public bool ShowTranslatedLabel
    {
      get => _ShowTranslatedLabel;
      set => _ShowTranslatedLabel = value;
    }
    #endregion

    public const string STR_ADMIN = "A";
    public const string STR_DEVELOPER = "D";
    public const string STR_TESTER = "T";
    public const string STR_USER = "1";

    public const int APPID = 11100;
    public const int APPTITLE = 11200;
    public const int APPIDREF = 11300;
    public const int MASTERAPPID = 2;
    public const int APPDC = 3;
    public const int USERDC = 4;
    public const int GROUPDC = 5;
    public const int MENUDC = 6;
    public const int CLASSDC = 7;
    public const int LOOKUPCLASSDC = 8;
    public const int DICTIONARYDC = 9;
    public const int URLBASE = 10;
    //public const int LOG_EXCEPTION = 11;
    public const int IDAPP_PROPERTY = 12;
    public const int NMAPP_PROPERTY = 13;
    public void SetValue(int mode, object value)
    {
      Param[mode] = value;
    }

    protected MasterAppConstants()
    {
      /*Create Object AppUtils in Global.asax*/
      SetValue(APPDC, "CoreNET.Common.BO.SsappControl, CoreNET.Common.Data");
      SetValue(USERDC, "CoreNET.Common.BO.Ss10userLoginControl, CoreNET.Common.Sys");
      SetValue(GROUPDC, "CoreNET.Common.BO.Ss20groupControl, CoreNET.Common.Sys");
      //SetValue(MENUDC, "CoreNET.Common.BO.Ss00appmenuControl, CoreNET.Common.Sys");
      //SetValue(CLASSDC, "CoreNET.Common.BO.Ss00appmenuControl, CoreNET.Common.Sys");
      //SetValue(LOOKUPCLASSDC, "CoreNET.Common.BO.Ss00appmenuControl, CoreNET.Common.Sys");
      SetValue(DICTIONARYDC, "CoreNET.Common.BO.Ss90dictionaryControl, CoreNET.Common.Sys");
      SetValue(MASTERAPPID, DEFAULT_MASTERAPP_ID);
      SetValue(APPID, DEFAULT_MASTERAPP_ID);
      SetValue(URLBASE, "https://localhost/");
      SetValue(IDAPP_PROPERTY, "Idapp");
      SetValue(NMAPP_PROPERTY, "Nmapp");
      LogException = true;
    }
  }
}
