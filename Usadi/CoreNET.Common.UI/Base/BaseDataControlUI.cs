using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace CoreNET.Common.Base
{
  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  public abstract class BaseDataControlUI : BaseDataControl, IDataControlUI
  {
    #region Property
    public string UserType => GlobalAsp.GetSessionUser().GetUserType();
    //public string ClientUserID => GlobalAsp.GetSessionUser().GetUserID()
    #region Property Debug
    public string GetDefaultDebug()
    {
      return Debug;
    }
    public static string GetStaticDefaultDebug(IDataControl dc)
    {
      try
      {
        IDataControlUI dcmaster = UtilityUI.GetDataControl(1);
        IDataControlUI dcdetil1 = null;
        IDataControlUI dcdetil2 = null;
        IDataControlUI dcdetil3 = null;
        IDataControlUI dcdetil4 = null;
        string strdcdetil = string.Empty;
        try
        {
          dcdetil1 = UtilityUI.GetDataControl(11, false);
          strdcdetil += (dcdetil1 != null) ? UtilityBO.GetClassLibName(dcdetil1) : string.Empty;
          dcdetil2 = UtilityUI.GetDataControl(12, false);
          strdcdetil += (dcdetil2 != null) ? "; " + UtilityBO.GetClassLibName(dcdetil2) : string.Empty;
          dcdetil3 = UtilityUI.GetDataControl(13, false);
          strdcdetil += (dcdetil3 != null) ? "; " + UtilityBO.GetClassLibName(dcdetil3) : string.Empty;
          dcdetil4 = UtilityUI.GetDataControl(14, false);
          strdcdetil += (dcdetil4 != null) ? "; " + UtilityBO.GetClassLibName(dcdetil4) : string.Empty;
        }
        catch (Exception ex)
        {
          UtilityBO.Log(dc, ex);
        }

        string idmenu = GlobalAsp.GetRequestId();

        string str = @"
                [BaseDataControlUI]<ul>
                   <li>idApp = " + GlobalAsp.GetSessionApp() + @"</li>
                   <li>i= " + GlobalAsp.GetRequestI() + @"</li>
                   <li>idMenu = " + GlobalAsp.GetRequestId() + @"</li>
                   <li>iprev= " + GlobalAsp.GetRequestIPrev() + @"</li>
                   <li>idprev = " + GlobalAsp.GetRequestIdPrev() + @"</li>
                   <li>dcMaster = {0}</li>
                   <li>dcDetil = {1}</li>
                   <li>url = " + HttpUtility.HtmlDecode(HttpContext.Current.Request.UrlReferrer.OriginalString) + @"</li>
                   <li>UserClass=" + GlobalAsp.GetSessionAppValue(MasterAppConstants.USERDC) + @"</li>
                   <li>MenuClass=" + GlobalAsp.GetSessionAppValue(MasterAppConstants.MENUDC) + @"</li>
                   <li>user = " + ((GlobalAsp.GetSessionUser() != null) ? GlobalAsp.GetSessionUser().GetUserID() : string.Empty) + @"</li>
                   <li>usertype = " + ((GlobalAsp.GetSessionUser() != null) ? GlobalAsp.GetSessionUser().GetUserType() : string.Empty) + @"</li>
                   <li>userdebug = " + (GlobalAsp.GetSessionUser() != null ? GlobalAsp.GetSessionUser().GetUserDebugInfo() : string.Empty) + @"</li>
                   <li>sqlinstance = " + SQLDataSource.GetSQLInstance() + @"</li>
                   <li>dbconfig = " + SQLDataSource.GetOpDB(SQLDataSource.Instance.CS_Config) + @"</li>
                   <li>dbop = " + SQLDataSource.GetOpDB(((BaseDataControlUI)dc).ConnectionString) + @"</li>
                </ul>[/BaseDataControlUI] <br/>
              " + BaseBO.GetStaticDefaultDebug((BaseBO)dc);
        return string.Format(str
          , (dcmaster != null) ? UtilityBO.GetClassLibName(dcmaster) : string.Empty
          , strdcdetil
          );
      }
      catch (Exception ex)
      {
        return "<br/>[BaseDataContrlUI]<br/>" + ex.Message + "<br/>[/BaseDataContrlUI]<br/>";
      }
    }
    public new string Debug
    {
      get
      {
        if (MasterAppConstants.Instance.StatusTesting)
        {
          return _Debug + GetStaticDefaultDebug(this);
        }
        else
        {
          return _Debug;
        }
      }
      set => _Debug = value;
    }
    #endregion
    public DataControlFieldCollection columns = null;
    public HashTableofParameterRow hpars = null;
    #endregion

    #region Method
    public BaseDataControlUI()
    {
      //Errornya HttpContext bikin iispool shutdown, ga boleh naruh HttpContext di constructor
      XMLName = GetType().Name.Replace("Control", "");
      ModeDB = SQLDataSource.MODE_DB_OPERATIONAL;
    }
    public new void SetPageKey()
    {
      Last_by = GlobalAsp.GetSessionUser().GetUserID();
      Last_date = DateTime.Now;
    }
    public void SetColumnsNull()
    {
      columns = null;
      hpars = null;
    }
    public string GetURLReport(object config)
    {
      return string.Empty;//Default HTML Report
    }
    public new int Delete()
    {
      if (Rating > 0)
      {
        throw new Exception(ConstantDict.Translate("LBL_APPROVED_DATA"));
      }
      if (Valid)
      {
        throw new Exception(ConstantDict.Translate("LBL_STATUS_VALID"));
      }
      if (Status > 0)
      {
        throw new Exception(ConstantDict.Translate("LBL_STATUS_CANNOT_DELETED"));
      }
      return Delete(BaseDataControl.DEFAULT);
    }
    public new String[] GetFields()
    {
      ArrayList arListNotFields = new ArrayList(GetNotFields());
      return GetFields(arListNotFields);
    }
    public IProperties GetProperties()
    {
      ViewListProperties props = ViewListProperties.CreateDefaultProperties();
      props.LookupLabelQuery = XMLName;
      return props;
    }
    public HashTableofParameterRow GetFilters()
    {
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      return hpars;
    }
    public void FilterClick(string key)
    {

    }
    public void SetFilterKey(BaseBO bo)
    {
      AllowSuperUser = (bool)bo.GetType().GetProperty("AllowSuperUser").GetValue(bo, null);
    }
    #endregion

    #region IDataControlUI Abstract Members
    public abstract DataControlFieldCollection GetColumns();
    #endregion

    #region ILoadCsv
    public static void ExecutedSQL(IDataControlUI dc, string tname, DataTable csvData, int counter, bool isdelete)
    {
      if (csvData.Rows.Count > 0)
      {
        DataRow dr = csvData.Rows[counter];
        #region Insert Row
        try
        {
          string[] cols = ((ILoadCsv)dc).GetLoadCsvColumns();
          for (int i = 0; i < cols.Length; i++)
          {
            string[] maps = cols[i].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
            if (maps.Length > 1)
            {
              UtilityBO.SetValueForProperty(dc, maps[0], dr[maps[1]]);
              //dc.SetValue(maps[0], dr[maps[1]]);
            }
            else
            {
              UtilityBO.SetValueForProperty(dc, maps[0], dr[maps[0]]);
              //dc.SetValue(maps[0], dr[maps[0]]);
            }
          }
          dc.SetPageKey();
          //IDataControl dctes = (IDataControl)((BaseBO)dc).Clone();
          IDataControl dctes = (IDataControl)BaseBO.Clone(dc);
          if (dctes.Load() == null)
          {
            dc.SetPrimaryKey();
            dc.Insert();
          }
          else
          {
            if (isdelete)
            {
              dc.SetValue("Status", -1);
              dc.Delete();
              dc.SetPrimaryKey();
              dc.Insert();
            }
            else
            {
              dc.Update();
            }
          }
        }
        catch (Exception ex)
        {
          try
          {
            UtilityBO.Log(dc, ex);
            dc.Update();
          }
          catch (Exception ex1)
          {
            UtilityBO.Log(dc, ex1);
          }
        }
        #endregion
      }
    }
    #endregion
  }

  [Serializable]
  public abstract class BaseDataControlUIEntry : BaseDataControlUI, IDataControlUI, IDataControlUIEntry
  {
    public BaseDataControlUIEntry()
    {
      ModeDB = SQLDataSource.MODE_DB_OPERATIONAL;
    }

    #region IDataControlUIEntry Members
    public abstract HashTableofParameterRow GetEntries();
    public HashTableofParameterRow GetEntries(string[] field_entries)
    {
      IDataControlUIEntry dc = this;
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      string[] fields = dc.GetKeys();

      Hashtable entries = new Hashtable();
      for (int i = 0; i < field_entries.Length; i++)
      {
        string[] temps = field_entries[i].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
        if (temps.Length == 1)
        {
          entries.Add(temps[0], temps[0]);
        }
        else
        {
          entries.Add(temps[0], temps[1]);
        }
      }
      ArrayList ReadOnlys = new ArrayList(((ViewListProperties)dc.GetProperties()).ReadOnlyFields);
      for (int i = 0; i < fields.Length; i++)
      {
        string pname = fields[i];
        if (entries.ContainsKey(pname))//show on form entry
        {
          Object val = dc.GetValue(pname);

          bool editable = true;
          editable = (ReadOnlys.IndexOf(pname) == -1);

          ParameterRow par = null;
          string name = pname + "=" + entries[pname];
          if (pname.Equals("TYPE", StringComparison.CurrentCultureIgnoreCase))
          {
            par = new ParameterRow(name, val, ParameterRow.MODE_TYPE, 95);
          }
          else
          {
            par = new ParameterRowTextBox(dc, name, editable, 0);
          }
          hpars.Add(par, i);
        }
      }
      return hpars;
    }
    public void AfterInsert()
    {

    }
    public void AfterUpdate()
    {

    }
    public void AfterDelete()
    {

    }
    public string GetDestinationFile(string filename)
    {
      string destfile = "File\\" + filename;
      return destfile;
    }
    public string GetURLFile(string filename, string versi)
    {
      if (!filename.Contains("http"))
      {
        string url = "File/" + filename;
        return url;
      }
      else
      {
        return filename;
      }
    }
    public Exception ValidateTotal()
    {
      return null;
    }

    public void SetEditable(bool editable)
    {
      Editable = editable;
    }
    public bool IsEditable()
    {
      return Editable;
    }
    #endregion
  }

}
