using CoreNET.Common.Base;
using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace CoreNET.Common.BO
{
  #region Ss01appmenuReg
  [Serializable]
  public class Ss01appmenuRegControl : Ss01appmenuControl, IDataControlTreeGrid3, IHasJSScript, ICsv, IExtLoadCsv
  {
    public Ss01appmenuRegControl()
    {
      XMLName = ConstantTablesSys.XMLSS01APPMENU;
      SetMaxModePreviewIndex(2);
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.ReadOnlyFields = new String[] { "Idapp" };
      cViewListProperties.AutoRefresh = false;
      if (UtilityUI.GetModePage() == UtilityUI.PAGE_TABULAR)
      {
        cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_DEL_LOADCSV;
      }
      else
      {
        cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_EDIT_DEL_LOADCSV;
      }
      return cViewListProperties;
    }

    public new void SetFilterKey(BaseBO bo)
    {
      BaseDataControlSys.SetFilterKey(this, bo);
      //Ketika daftar menu adalah daftar aplikasi, maka idapp ambil dari QS val
      if (!string.IsNullOrEmpty(GlobalAsp.GetRequestVal()))
      {
        Idapp = GlobalAsp.GetRequestVal();
        SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
      }
      else
      {
        if (typeof(IDataControlMenu).IsInstanceOfType(bo))//Open page firstly
        {
          if (!GlobalAsp.GetSessionApp().Equals(MasterAppConstants.Instance.MasterAppID)
                  && !GlobalAsp.GetSessionApp().Equals(MasterAppConstants.ID_APP_PROGRAMMER))
          {
            Idapp = GlobalAsp.GetSessionApp();
            SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
          }
        }
        else if (typeof(Ss00appControl).IsInstanceOfType(bo))//Set when select in Lookup
        {
          Idapp = ((Ss00appControl)bo).Idapp;
          Kdapp = ((Ss00appControl)bo).Kdapp;
          Nmapp = ((Ss00appControl)bo).Nmapp;
        }
        else
        {
          Idapp = GlobalAsp.GetSessionApp();
          SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
        }
      }
    }

    public new void SetPrimaryKey()
    {
      if (GlobalAsp.GetEditingObject() != null)
      {
        Kdmenu = ((Ss01appmenuControl)GlobalAsp.GetEditingObject()).Kdmenu + "XX.";
        Type = (Kdmenu.Equals("XX.")) ? "H" : "D";
        Kdlevel = (Kdmenu.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries)).Length;
      }
      else
      {
        Kdmenu = "XX.";
        Kdlevel = 1;
        Type = "H";
      }
      Idmenu = Guid.NewGuid().ToString();
      Olist1 = "Namespace.MyClassControl,MyLibrary";
    }
    public new IList View()
    {
      IList list = null;
      list = View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      IList list = ((BaseDataControl)this).View(label);
      List<Ss01appmenuControl> ListData = new List<Ss01appmenuControl>();
      foreach (Ss01appmenuControl dc in list)
      {
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        dc.Kdlevel = dc.Kdmenu.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Length;
        dc.Update("Kdlevel");
        ListData.Add(dc);
      }
      return ListData;
    }

    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalExt.GetRequestIdPrev())&& string.IsNullOrEmpty(GlobalExt.GetRequestVal());
      bool isFilterObj = (GetType().Name.Contains("Filter"));
      if (!isFilterObj)
      {
        if (!GlobalAsp.GetSessionApp().Equals(MasterAppConstants.Instance.MasterAppID)
          && !GlobalAsp.GetSessionApp().Equals(MasterAppConstants.ID_APP_PROGRAMMER)
          && !GlobalAsp.GetSessionApp().Equals(MasterAppConstants.ID_APP_STATUS)
          )
        {
          Idapp = GlobalAsp.GetRequestApp();
          SsappControl dc = SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
          bool enableByType = (dc.Type.Equals("H"));
          enableFilter = enableFilter && enableByType;
        }
      }
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      ParameterRow pr = null;
      if (MasterAppConstants.Instance.StatusAdmin)
      {
        pr = Ss00appLookupControl.Instance.GetLookupParameterRowDetil(this, false).SetEnable(enableFilter);
      }
      else
      {
        pr = Ss00appLookupControl.Instance.GetLookupParameterRowByUser(this, false).SetEnable(enableFilter);
      }
      hpars.Add(pr);
      return hpars;
    }
    public override DataControlFieldCollection GetColumns()
    {
      DataControlFieldCollection columns = new DataControlFieldCollection
      {
        Fields.Create(ConstantDict.GetColumnTitle("Statusicon"), typeof(Icon), 5, HorizontalAlign.Center),
        Fields.Create(ConstantDict.GetColumnTitle("Kdmenu"), typeof(string), EditCmd, 15, HorizontalAlign.Left).SetEditable(true),
        Fields.Create(ConstantDict.GetColumnTitle("Nmmenu"), typeof(string), 40, HorizontalAlign.Left).SetEditable(true)
      };
      if (ModePreviewIndex == 2)
      {
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Idxdok"), typeof(string), 25, HorizontalAlign.Center).SetEditable(true));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdpack"), typeof(string), 20, HorizontalAlign.Left));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kddok"), typeof(string), 20, HorizontalAlign.Center));
      }
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Url"), typeof(string), 45, HorizontalAlign.Left).SetEditable(true));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Olist1"), typeof(string), 30, HorizontalAlign.Left).SetEditable(true));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdlevel"), typeof(int), 7, HorizontalAlign.Center).SetEditable(true));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Type"), typeof(string), 7, HorizontalAlign.Center).SetEditable(true));
      return columns;
    }
    public new void SetTreeGridColumns(TreeGridColumnCollection Columns)
    {
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kdmenu"), Width = 150, DataIndex = "Kdmenu", Align = Ext.Net.TextAlign.Left });
      if (GetModePreviewIndex() == 2)
      {
        Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kddok"), Width = 100, DataIndex = "Kddok", Align = Ext.Net.TextAlign.Center });
        Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Idxdok"), Width = 100, DataIndex = "Idxdok", Align = Ext.Net.TextAlign.Center });
      }
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Nmmenu"), Width = 350, DataIndex = "Nmmenu", Align = Ext.Net.TextAlign.Left });
      if (GetModePreviewIndex() == 1)
      {
        Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Url"), Width = 400, DataIndex = "Url", Align = Ext.Net.TextAlign.Left });
      }
      Columns.Add(TreeColStatus);
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kdlevel"), Width = 50, DataIndex = "Kdlevel", Align = Ext.Net.TextAlign.Center });
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Type"), Width = 50, DataIndex = "Type", Align = Ext.Net.TextAlign.Center });
      #region Command
      //TreeGridColumn col1 = new TreeGridColumn { Header = ConstantDict.Translate(string.Empty), Width = 250, Align = Ext.Net.TextAlign.Center };
      //col1.XTemplate.Html = @"<a style='visibility:true;'
      //  onclick = 'parent.loadPageByID(""Pages"",""URL"",""URL"", ""{[GetURLEditor(values.Idmenu)]}"");'>URL</a> | <a style='visibility:true;'
      //  onclick = 'parent.loadPageByID(""Pages"",""Link"",""Link"", ""{[GetURLMapClass(values.Idmenu)]}"");'>Link</a> | <a style='visibility:true;'
      //  onclick = 'parent.loadPageByID(""Pages"",""Entry"",""Entry"", ""{[GetURLEntry(values.Idmenu)]}"");'>Entry</a> | <a style='visibility:true;'
      //  onclick = 'parent.loadPageByID(""Pages"",""Grid"",""Grid"", ""{[GetURLColGrid(values.Idmenu)]}"");'>Grid</a> | <a style='visibility:true;'
      //  onclick = 'parent.loadPageByID(""Pages"",""Tree"",""Tree"", ""{[GetURLColTree(values.Idmenu)]}"");'>Tree</a> | <a style='visibility:true;'
      //  onclick = 'parent.loadPageByID(""Pages"",""Filter"",""Filter"", ""{[GetURLFilter(values.Idmenu)]}"");'>Filter</a> | <a style='visibility:true;'
      //  onclick = 'parent.loadPageByID(""Pages"",""Csv"",""Csv"", ""{[GetURLCsv(values.Idmenu)]}"");'>CSV</a>
      //";
      //Columns.Add(col1);
      #endregion
    }
    public override HashTableofParameterRow GetEntries()
    {
      bool enable = true;
      HashTableofParameterRow hpars = new HashTableofParameterRow
      {
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Kdmenu"), true, 30).SetEnable(enable),
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Nmmenu"), true, 95).SetEnable(enable),
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Idmenu"), true, 70).SetEnable(false),
      };
      hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitle("Url"),
        ParamControl.GetListURLS(), "Kdpar=Nmpar", 95).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Olist1"), true, 95).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Olistdetil1"), true, 95).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Olistdetil2"), true, 95).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Olistdetil3"), true, 95).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Olistdetil4"), true, 95).SetEnable(enable));
      hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitle("Kdlevel"),
        GetList(new DmlevelControl(this)), "Kdlevel=Nmlevel", 30).SetEnable(enable));
      hpars.Add(new ParameterRowType(this, true));
      hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
        GetList(new DmstatusControl()), "Idstatus=Nmstatus", 30).SetAllowRefresh(true).SetEnable(enable));
      hpars.Add(new ParameterRowCek(this, true));
      hpars.Add(new ParameterRowUploadFile(this, true));
      hpars.Add(new ParameterRowHelp(this, true));
      hpars.Add(new ParameterRowForum(this, true));
      return hpars;
    }
    #region IExtLoadCsv
    public new string[] GetCsvColumns(int mode)
    {
      return new string[] { "Kdmenu", "Nmmenu", "Url", "Kdlevel", "Type" };
      //return new string[] { "Kdmenu", "Kddok", "Idxdok", "Nmmenu", "Url", "Olist1", "Olistdetil1", "Olistdetil2", "Olistdetil3", "Olistdetil4", "Kdlevel", "Type" };
    }
    public new string[] GetLoadCsvColumns()
    {
      return new string[] { "Kdmenu", "Nmmenu", "Url", "Olist1","Kdlevel", "Type" };
      //return new string[] { "Kdmenu", "Kddok", "Idxdok", "Nmmenu", "Url", "Olist1", "Olistdetil1", "Olistdetil2", "Olistdetil3", "Olistdetil4", "Kdlevel", "Type" };
    }
    public new void ExecutedPreSQL(string tname, bool isdelete)
    {
      if (isdelete)
      {
        string sqlQuery = string.Format(@"delete from {0} where {1}='{2}'", tname, "Idapp", Idapp);
        BaseDataAdapter.ExecuteCmd(ConnectionString, sqlQuery);
      }
    }
    public new void ExecutedSQL(string tname, DataTable csvData, int counter, bool isdelete)
    {
      if (csvData.Rows.Count > 0)
      {
        DataRow dr = csvData.Rows[counter];

        if ((!csvData.Columns.Contains("AppID")) || (dr["AppID"].ToString().Equals(Idapp)))
        {
          Ss01appmenuControl dc = new Ss01appmenuControl();
          try
          {
            /* Klo exist, lanjutkan proses insert */
            //dr["IDMENU"] = Guid.NewGuid().ToString();
            //base.ExecutedSQL(tname, csvData, counter);
            string[] cols = GetLoadCsvColumns();
            for (int i = 0; i < cols.Length; i++)
            {
              string[] maps = cols[i].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
              if (maps.Length > 1)
              {
                dc.SetValue(maps[0], dr[maps[1]]);
              }
              else
              {
                dc.SetValue(maps[0], dr[maps[0]]);
              }
            }
            dc.Idapp = Idapp;
            dc.SetPageKey();
            //IDataControl dctes = (IDataControl)((BaseBO)dc).Clone();
            IDataControl dctes = (IDataControl)BaseBO.Clone(dc);
            if (dctes.Load() == null)
            {
              if (string.IsNullOrEmpty(dc.Idmenu))
              {
                dc.Idmenu = Guid.NewGuid().ToString();
              }
              dc.SetPrimaryKey();
              //Ss01appmenuControl dcprev = (Ss01appmenuControl)dc.Clone();
              Ss01appmenuControl dcprev = (Ss01appmenuControl)BaseBO.Clone(dc);
              dcprev.Load();
              dc.Insert();
            }
            else
            {
              if (isdelete)
              {
                dc.Status = -1;
                dc.Delete();
                dc.SetPageKey();
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
            UtilityBO.Log(dc, ex);
            try
            {
              dc.Update();
            }
            catch (Exception ex1) { UtilityBO.Log(dc, ex1); }
          }
        }
        else
        {
          throw new Exception("Data CSV tidak valid!");
        }
      }
    }
    public new void ExecutedPostSQL(string tname, bool isdelete, string idmenu)
    {
      string sql = string.Format(@"exec RevalidateSs01appmenu '{0}'", Idapp);
      BaseDataAdapter.ExecuteCmd(ConnectionString, sql);
      //Validate Type dan Kdlevel juga ya
    }
    #endregion
  }
  #endregion Ss01appmenuReg
}

