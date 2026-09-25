using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using CoreNET.Common.Base;
using CoreNET.Common.BO;

namespace CoreNET.Common.BO
{
  #region Notes
  [Serializable]
  public class NotesUIControl : NotesControl, IDataControlUIEntry
  {
    #region Methods
    public DataControlFieldCollection columns = null;
    public void SetColumnsNull()
    {
      columns = null;
    }
    public NotesUIControl()
    {
      ModeDB = SQLDataSource.MODE_DB_LOG;
      Idnotes = Guid.NewGuid().ToString();
      XMLName = "Notes";
      Tglnotes = DateTime.Now;
      Refdb = (string)HttpContext.Current.Session[GlobalAsp.SESSION_DB];
    }
    public string GetDefaultDebug()
    {
      return BaseDataControlUI.GetStaticDefaultDebug(this);
    }
    public new void Insert()//SMART_APP_LOG
    {
      ((BaseDataControl)this).Insert(BaseDataControl.DEFAULT);
    }
    public new IList View()
    {
      IList list = null;
      list = ((BaseDataControl)this).View(BaseDataControl.FILTER);
      return list;
    }
    public new BaseBO Load()
    {
      return ((BaseDataControl)this).Load(BaseDataControl.FILTER);
    }
    public IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = ViewListProperties.CreateDefaultProperties();
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Idnotes" };
      cViewListProperties.ReadOnlyFields = new String[] { };
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_EDIT_DEL;
      return cViewListProperties;
    }

    public void SetFilterKey(BaseBO bo)
    {
    }

    public void SetEditable(bool editable)
    {
      Editable = editable;
    }

    public HashTableofParameterRow GetFilters()
    {
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      return hpars;
    }

    public void FilterClick(string key)
    {
    }

    public DataControlFieldCollection GetColumns()
    {
      DataControlFieldCollection columns = new DataControlFieldCollection();
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Idnotes"), typeof(string), 25, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Tglnotes"), typeof(DateTime), 13, HorizontalAlign.Center));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Refdb"), typeof(string), 10, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Reftabel"), typeof(string), 15, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Refid"), typeof(string), 25, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Notestype"), typeof(string), 5, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Notes"), typeof(string), 35, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitleEntry("Status"), typeof(string), 5, HorizontalAlign.Left));
      return columns;
    }
    public HashTableofParameterRow GetEntries()
    {
      bool enable = false;
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Idnotes"), false, 70).SetEnable(enable));
      hpars.Add(new ParameterRowDate(this, ConstantDict.GetColumnTitle("Tglnotes"), false).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Refdb"), false, 50).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Reftabel"), false, 50).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Refid"), false, 70).SetEnable(enable));
      hpars.Add(new ParameterRowNumeric(this, ConstantDict.GetColumnTitle("Notestype"), false, 30).SetEnable(enable));
      hpars.Add(new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Notes"), false, 3).SetEnable(enable));
      hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
        GetList(new DmstatusControl()), "Idstatus=Nmstatus", 30).SetAllowRefresh(false).SetEnable(true));
      return hpars;
    }

    #endregion Methods
    #region static
    //public static void SaveHTML(IDataControlUI dc, string html)
    //{
    //  SaveHTML(dc, html, NotesControl.DESCRIPTION);
    //}
    public static void SaveHTML(IDataControlUI dc, string html, int type)
    {
      string pk = HttpContext.Current.Request["pk"];
      if (string.IsNullOrEmpty(pk))
      {
        pk = ((ViewListProperties)dc.GetProperties()).IDKey;
      }
      NotesControl ctrl = new NotesControl();
      ctrl.Refdb = SQLDataSource.GetOpDB(((BaseBO)dc).ConnectionString);
      ctrl.Reftabel = (string)dc.GetValue("XMLName");
      if (typeof(NotesControl).IsInstanceOfType(dc))
      {
        ctrl.Refid = GlobalAsp.GetSessionUser().GetGroupingDataID();
      }
      else
      {
        object obj = null;
        if (HttpContext.Current.Request["mode"] == "help")
        {
          obj = GlobalAsp.GetRequestId();
        }
        else
        {
          obj = dc.GetValue(pk);
          if (obj == null)
          {
            throw new Exception(string.Format("Value from Property {0} could not be an ID", pk));
          }
        }
        ctrl.Refid = obj.ToString();
      }
      ctrl.Notestype = type;
      ctrl.Notes = html;
      ctrl.Insert();
    }
    //public static string LoadHTML(IDataControlUI dc)
    //{
    //  return LoadHTML(dc, NotesControl.DESCRIPTION);
    //}
    public static string LoadHTML(IDataControlUI dc, int type)
    {
      string pk = HttpContext.Current.Request["pk"];
      if (string.IsNullOrEmpty(pk))
      {
        pk = ((ViewListProperties)dc.GetProperties()).IDKey;
      }
      NotesControl ctrl = new NotesControl();
      ctrl.Refdb = SQLDataSource.GetOpDB(((BaseBO)dc).ConnectionString);
      ctrl.Reftabel = (string)dc.GetValue("XMLName");
      if (typeof(NotesControl).IsInstanceOfType(dc))
      {
        ctrl.Refid = GlobalAsp.GetSessionUser().GetGroupingDataID();
      }
      else
      {
        object obj = null;
        if (HttpContext.Current.Request["mode"] == "help")
        {
          obj = GlobalAsp.GetRequestId();
        }
        else
        {
          obj = dc.GetValue(pk);
          if (obj == null)
          {
            throw new Exception(string.Format("Value from Property {0} could not be an ID", pk));
          }
        }
        ctrl.Refid = obj.ToString();
      }
      ctrl.Notestype = type;
      if (ctrl.Load() == null)
      {
        /*@TODO insert help default*/
      }
      return ctrl.Notes;
    }
    public static void SaveMsg(IDataControlUI dc, string html)
    {
      string pk = HttpContext.Current.Request["pk"];
      if (string.IsNullOrEmpty(pk))
      {
        pk = ((ViewListProperties)dc.GetProperties()).IDKey;
      }
      NotesControl ctrl = new NotesControl();
      ctrl.Refdb = SQLDataSource.GetOpDB(((BaseBO)dc).ConnectionString);
      ctrl.Reftabel = (string)dc.GetValue("XMLName");
      if (typeof(NotesControl).IsInstanceOfType(dc))
      {
        ctrl.Refid = GlobalAsp.GetSessionUser().GetGroupingDataID();
      }
      else
      {
        object oval = dc.GetValue(pk);
        if (oval != null)
        {
          ctrl.Refid = oval.ToString();
        }
      }
      ctrl.Notestype = NotesControl.FORUM;
      ctrl.Notes = html;
      ctrl.Last_by = (GlobalAsp.GetSessionUser().GetUserID());
      ctrl.Last_date = DateTime.Now;
      ctrl.Insert();
    }
    public static IList LoadMsgs(IDataControlUI dc)
    {
      string pk = HttpContext.Current.Request["pk"];
      if (string.IsNullOrEmpty(pk))
      {
        pk = ((ViewListProperties)dc.GetProperties()).IDKey;
      }
      NotesControl ctrl = new NotesControl();
      ctrl.Refdb = SQLDataSource.GetOpDB(((BaseBO)dc).ConnectionString);
      ctrl.Reftabel = (string)dc.GetValue("XMLName");
      if (typeof(NotesControl).IsInstanceOfType(dc))
      {
        ctrl.Refid = GlobalAsp.GetSessionUser().GetGroupingDataID();
      }
      else
      {
        object oval = dc.GetValue(pk);
        if (oval != null)
        {
          ctrl.Refid = oval.ToString();
        }
      }
      ctrl.Notestype = NotesControl.FORUM;
      IList list = ctrl.View();
      return list;
    }

    public string GetDestinationFile(string filename)
    {
      return "";
    }

    public string GetURLFile(string filename, string versi)
    {
      return "";
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

    public Exception ValidateTotal()
    {
      return null;
    }

    public bool IsEditable()
    {
      return true;
    }
    #endregion
  }
  #endregion Notes
}

