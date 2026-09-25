using System;
using System.Data;
using System.Web;
using Ext.Net;
using Ext.Net.Utilities;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  #region Ss00appmenuReg
  [Serializable]
  public class Ss00appmenuRegControl : Ss00appmenuControl, IDataControlTreeGrid3, IHasJSScript, ICsv, IExtLoadCsv
  {
    public Ss00appmenuRegControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APPMENU;
      SetMaxModePreviewIndex(2);
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
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
    public new void SetPrimaryKey()
    {
      Idmenu = Guid.NewGuid().ToString();
    }
    public new HashTableofParameterRow GetFilters()
    {
      string idapp = GlobalAsp.GetSessionApp();
      bool enableFilter = string.IsNullOrEmpty(GlobalExt.GetRequestIdPrev()) && (idapp.Equals(MasterAppConstants.Instance.MasterAppID));
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      ParameterRow pr = Ss00appLookupControl.Instance.GetLookupParameterRowAll(this, false).SetEnable(enableFilter);
      hpars.Add(pr);
      return hpars;
    }
    //public new HashTableofParameterRow GetFilters()
    //{
    //  Ss00appControl dc = new Ss00appControl();
    //  dc.Idapp = GlobalAsp.GetSessionApp();
    //  dc = SsappLookupControl.FindAndSetValuesIntoByIdapp(dc);
    //  bool enableFilter = string.IsNullOrEmpty(GlobalExt.GetRequestIdPrev()) && (dc.Type.Equals("H"));
    //  HashTableofParameterRow hpars = new HashTableofParameterRow();
    //  string[] keys = new string[] { "Idapp", "Nmapp" };
    //  ParameterRow pr = Ss00appLookupControl.Instance.GetLookupParameterRowDetil(this, false, keys, keys).SetAllowRefresh(true).SetEnable(enableFilter);
    //  hpars.Add(pr);
    //  return hpars;
    //}

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
      if (GetModePreviewIndex() == 1)
      {
        Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kdlevel"), Width = 50, DataIndex = "Kdlevel", Align = Ext.Net.TextAlign.Center });
        Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Type"), Width = 50, DataIndex = "Type", Align = Ext.Net.TextAlign.Center });
      }
      #region Command
      TreeGridColumn col1 = new TreeGridColumn { Header = ConstantDict.Translate(string.Empty), Width = 250, Align = Ext.Net.TextAlign.Center };
      col1.XTemplate.Html = @"<a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""URL"",""URL"", ""{[GetURLEditor(values.Idmenu)]}"");'>URL</a> | <a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""Link"",""Link"", ""{[GetURLMapClass(values.Idmenu)]}"");'>Link</a> | <a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""Entry"",""Entry"", ""{[GetURLEntry(values.Idmenu)]}"");'>Entry</a> | <a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""Grid"",""Grid"", ""{[GetURLColGrid(values.Idmenu)]}"");'>Grid</a> | <a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""Tree"",""Tree"", ""{[GetURLColTree(values.Idmenu)]}"");'>Tree</a> | <a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""Filter"",""Filter"", ""{[GetURLFilter(values.Idmenu)]}"");'>Filter</a> | <a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""Csv"",""Csv"", ""{[GetURLCsv(values.Idmenu)]}"");'>CSV</a>
      ";
      Columns.Add(col1);
      #endregion
    }
    #region IExtLoadCsv
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

        if (dr["AppID"].ToString().Equals(Idapp))
        {
          Ss00appmenuControl dc = new Ss00appmenuControl();
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
              Ss00appmenuControl dcprev = (Ss00appmenuControl)BaseBO.Clone(dc);
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
            catch (Exception ex1) {UtilityBO.Log(dc,ex1);}
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
      string sql = string.Format(@"exec RevalidateSs00appmenu '{0}'", Idapp);
      BaseDataAdapter.ExecuteCmd(ConnectionString, sql);

    }
    #endregion
  }
  #endregion Ss00appmenuReg
}

