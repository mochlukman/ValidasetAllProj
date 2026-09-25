using CoreNET.Common.BO;
using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace CoreNET.Common.Base
{
  [Serializable]
  public abstract class BaseDataControlExt : BaseDataControlUIEntry, IDataControlUIEntry, IExtLoadCsv, IHasJSScript
  {
    private ImageCommand _EditCmd = null;
    public ImageCommand EditCmd
    {
      get
      {
        if (_EditCmd == null)
        {
          string typename = GetType().Name;
          bool visible = (((ViewListProperties)GetProperties()).ModeEditable != ViewListProperties.MODE_EDITABLE_READONLY)
            && (!typename.ToLower().Contains("lookup"));
          _EditCmd = new ImageCommand
          {
            CommandName = "EditForm",
            Icon = Icon.Pencil
          };
          _EditCmd.ToolTip.Text = "Klik untuk menampilkan form edit";
          _EditCmd.Hidden = !visible;
        }

        return _EditCmd;
      }
    }

    private static ImageCommand _DefaultCmd = null;
    public static ImageCommand DefaultCmd
    {
      get
      {
        if (_DefaultCmd == null)
        {
          _DefaultCmd = new ImageCommand
          {
            CommandName = "EditForm",
            Icon = Icon.Pencil
          };
          _DefaultCmd.ToolTip.Text = "Klik untuk menampilkan form edit";
        }
        return _DefaultCmd;
      }
    }

    private static ImageCommand _CmdPath = null;
    public static ImageCommand CmdPath
    {
      get
      {
        if (_CmdPath == null)
        {
          _CmdPath = new ImageCommand()
          {
            CommandName = "EditPath",
            Icon = Icon.Folder,
            ToolTip = {
              Text = "Klik tombol ini untuk menampilkan folder dokumen"
            }
          };
        }
        return _CmdPath;
      }
    }
    private static ImageCommand _CmdHelp = null;
    public static ImageCommand CmdHelp
    {
      get
      {
        if (_CmdHelp == null)
        {
          _CmdHelp = new ImageCommand()
          {
            CommandName = "EditHelp",
            Icon = Icon.Note,
            ToolTip = {
              Text = "Klik tombol ini untuk menampilkan catatan"
            }
          };
        }
        return _CmdHelp;
      }
    }
    private static ImageCommand _CmdMsg = null;
    public static ImageCommand CmdMsg
    {
      get
      {
        if (_CmdMsg == null)
        {
          _CmdMsg = new ImageCommand()
          {
            CommandName = "EditMsg",
            Icon = Icon.Comment,
            ToolTip = {
              Text = "Klik tombol ini untuk menampilkan diskusi"
            }
          };
        }
        return _CmdMsg;
      }
    }

    public static TreeGridColumn TreeColStatus
    {
      get
      {
        TreeGridColumn col1 = new TreeGridColumn { Header = ConstantDict.Translate(string.Empty), Width = 33, DataIndex = "Statusicon", Align = Ext.Net.TextAlign.Center };
        col1.XTemplate.Html = @"<a style='visibility:true;' title='{Statusname}'>
                    <img style='width: 16px; height: 16px;' title='{Statusname}' src='../Res/Icons/{Statusicon}' alt='{Statusname}'/></a>
                  ";
        return col1;
      }
    }
    public static TreeGridColumn TreeColProgress
    {
      get
      {
        TreeGridColumn col1 = new TreeGridColumn { Header = ConstantDict.Translate("%"), Width = 33, DataIndex = "Progressicon", Align = Ext.Net.TextAlign.Center };
        col1.XTemplate.Html = @"<a style='visibility:true;' title='{Progressstr}'>
                    <img style='width: 16px; height: 16px;' title='{Progressstr}' src='../Res/Icons/{Progressicon}' alt='{Progressstr}'/></a>
                  ";
        return col1;
      }
    }

    public string Validstr
    {
      get => ResourceManager.GetInstance().GetIconUrl(Icon.Accept);
      set { }
    }

    public BaseDataControlExt()
    {
      SetMaxModePreviewIndex(2);
      try
      {
        //Set Max Preview from Syscols
      }
      catch (Exception ex)
      {
        UtilityBO.Log(this, ex);
      }
    }
    public override DataControlFieldCollection GetColumns()
    {
      DataControlFieldCollection cols = GetColumn(this, ModePreviewIndex);
      return cols;
    }
    public new string[] GetKeys()
    {
      string[] keys = null;

      if (System.Configuration.ConfigurationManager.AppSettings["IsDynamicUI"] == "1")
      {
        keys = SysgetkeysLookupControl.GetCols(this);
        if (keys.Length == 0)
        {
          #region InsertDefault
          //if (System.Configuration.ConfigurationManager.AppSettings["SysGenerator"] == "1")
          //{
          //  try
          //  {              
          //    SysgetkeysControl ctrl = new SysgetkeysControl
          //    {
          //      Typename = UtilityBO.GetClassLibName(dc)
          //    };
          //    string[] strs = dc.GetKeys();
          //    string fields = string.Empty;
          //    for (int i = 0; i < strs.Length; i++)
          //    {
          //      fields += strs[i] + ",";
          //    }
          //    ctrl.Cols = fields;
          //    ctrl.Csvcols = fields;
          //    ctrl.Loadcsvcols = fields;
          //    string tablename = dc.GetValue("XMLName").ToString().ToUpper();
          //    string title = "Load CSV";
          //    ctrl.Loadconfig = tablename + "|" + ctrl.Typename + "=" + title;//PM01PROJECT|CoreNET.Common.BO.Pm01projectControl,CoreNET.Common.Sys=Daftar Menu
          //    ctrl.Insert();
          //  }
          //  catch (Exception ex)
          //  {
          //    UtilityBO.Log(dc, ex);
          //  }
          //}
          #endregion
        }
      }
      else
      {
        keys = base.GetKeys();
      }
      return keys;

    }
    public void SetTreeGridColumns(TreeGridColumnCollection Columns)
    {
      ExtTreePageUtils.SetDefaultTreeGrid(Columns, this, ModePreviewIndex);
      if (Columns.Count == 0)
      {
        //Generate
        #region Command
        TreeGridColumn col1 = new TreeGridColumn { Header = ConstantDict.Translate(string.Empty), Width = 250, Align = Ext.Net.TextAlign.Center };
        col1.XTemplate.Html = @"<a style='visibility:true;'
          onclick = 'parent.loadPageByID(""Pages"",""URL"",""URL"", ""{[GetURLEditor(values.Idmenu)]}"");'>URL</a> | <a style='visibility:true;'
          onclick = 'parent.loadPageByID(""Pages"",""Link"",""Link"", ""{[GetURL(values.Url,values.Idmenu)]}"");'>Link</a> | <a style='visibility:true;'
          onclick = 'parent.loadPageByID(""Pages"",""Entry"",""Entry"", ""{[GetURLEntry(values.Idmenu)]}"");'>Entry</a> | <a style='visibility:true;'
          onclick = 'parent.loadPageByID(""Pages"",""Grid"",""Grid"", ""{[GetURLColGrid(values.Idmenu)]}"");'>Grid</a> | <a style='visibility:true;'
          onclick = 'parent.loadPageByID(""Pages"",""Tree"",""Tree"", ""{[GetURLColTree(values.Idmenu)]}"");'>Tree</a> | <a style='visibility:true;'
          onclick = 'parent.loadPageByID(""Pages"",""Filter"",""Filter"", ""{[GetURLFilter(values.Idmenu)]}"");'>Filter</a> | <a style='visibility:true;'
          onclick = 'parent.loadPageByID(""Pages"",""Csv"",""Csv"", ""{[GetURLCsv(values.Idmenu)]}"");'>CSV</a>
        ";
        Columns.Add(col1);
        #endregion
      }
    }

    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalExt.GetRequestIdPrev());
      HashTableofParameterRow hpars = GetFilters(this, enableFilter);
      return hpars;
    }
    public override HashTableofParameterRow GetEntries()
    {
      bool enable = true;
      HashTableofParameterRow hpars = GetEntries(this, enable);
      //hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
      //  GetList(new DmstatusControl()), "Idstatus=Nmstatus", 30).SetAllowRefresh(false).SetEnable(enable));
      ///*Klo ngga Tab jadi error, ngga bisa ngesave*/
      //hpars.Add(new ParameterRowCek(this, true));
      //hpars.Add(new ParameterRowUploadFile(this, true));
      //hpars.Add(new ParameterRowHelp(this, true));
      //hpars.Add(new ParameterRowForum(this, true));
      return hpars;
    }
    #region IExtLoadCsv
    public string[] GetLoadCsvColumns()
    {
      string[] csvcolumns = SysgetkeysLookupControl.GetLoadCSVCols(this);
      return csvcolumns;
    }

    public string[] GetCsvColumns(int mode)
    {
      string[] keys = SysgetkeysLookupControl.GetCSVCols(this);
      if (keys.Length == 0)
      {
        keys = GetKeys();
      }
      return keys;
    }

    public IList GetList(int id, int mode)
    {
      return GlobalAsp.GetSessionListRows(id);
    }
    public Window GetLoadCsvWindow()
    {
      string url = string.Format(@"~/Page/CSVUpload.aspx?app={0}", GlobalAsp.GetSessionApp());
      return new WindowLoadCSV("0", "Load Menu", url, 400, 200);
    }
    #endregion Methods 

    public static HashTableofParameterRow GetEntries(IDataControlUIEntry ctrl, bool enable)
    {
      return GetEntries(ctrl, true, enable);
    }
    public static HashTableofParameterRow GetFilters(IDataControlUIEntry ctrl, bool enable)
    {
      return GetEntries(ctrl, false, enable);
    }
    private static HashTableofParameterRow GetEntries(IDataControlUIEntry ctrl, bool isentry, bool enable)
    {
      IList list = null;
      if (isentry)
      {
        list = SysgetrowsLookupControl.GetRows(ctrl);
      }
      else
      {
        list = SysgetfiltersLookupControl.GetFilters(ctrl);
      }
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      for (int i = 0; i < list.Count; i++)
      {
        try
        {
          int rowwidth = 0;
          string rowname = string.Empty;
          string rowtype = string.Empty;
          bool rowenable = enable;
          string colavaillist = string.Empty;
          bool roweditable = false;
          string[] rowavs = null;
          string tname = null;
          string fieldmap = null;
          string[] fieldmaps = null;

          IDataControl dc = (IDataControl)list[i];
          if (typeof(SysgetrowsControl).IsInstanceOfType(dc))
          {
            rowwidth = ((SysgetrowsControl)dc).Rowwidth;
            rowname = ((SysgetrowsControl)dc).Rowname;
            rowtype = ((SysgetrowsControl)dc).Rowtype;
            rowenable = ((SysgetrowsControl)dc).Rowenable && enable;
            colavaillist = ((SysgetrowsControl)dc).Rowavaillist;
            roweditable = ((SysgetrowsControl)dc).Roweditable;
          }
          else
          {
            rowwidth = ((SysgetfiltersControl)dc).Rowwidth;
            rowname = ((SysgetfiltersControl)dc).Rowname;
            rowtype = ((SysgetfiltersControl)dc).Rowtype;
            rowenable = ((SysgetfiltersControl)dc).Rowenable && enable;
            colavaillist = ((SysgetfiltersControl)dc).Rowavaillist;
            roweditable = ((SysgetfiltersControl)dc).Roweditable;
          }
          IDataControlLookup dcLookup = null;
          if (!string.IsNullOrEmpty(colavaillist))
          {
            rowavs = colavaillist.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            tname = rowavs[0];
            if (!string.IsNullOrEmpty(tname))
            {
              dcLookup = (IDataControlLookup)UtilityBO.Create(tname);
              fieldmap = (rowavs.Length > 1) ? rowavs[1] : dcLookup.GetFieldValueMap();
              fieldmaps = fieldmap.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
            }
          }
          switch (rowtype)
          {
            case "string":
              hpars.Add(new ParameterRowTextBox(ctrl, ConstantDict.GetColumnTitle(rowname), roweditable, rowwidth).SetEnable(rowenable));
              break;
            case "numeric":
              hpars.Add(new ParameterRowNumeric(ctrl, ConstantDict.GetColumnTitle(rowname), roweditable, rowwidth).SetEnable(rowenable));
              break;
            case "memo":
              hpars.Add(new ParameterRowMemo(ctrl, ConstantDict.GetColumnTitle(rowname), roweditable, rowwidth).SetEnable(rowenable));
              break;
            case "date":
            case "DateTime":
              hpars.Add(new ParameterRowDate(ctrl, ConstantDict.GetColumnTitle(rowname), roweditable).SetEnable(rowenable));
              break;
            case "text":
              hpars.Add(new ParameterRowHtml(ctrl, ConstantDict.GetColumnTitle(rowname), true).SetEnable(rowenable));
              break;
            case "lookup":
              ParameterRow pr = dcLookup.GetLookupParameterRow(dcLookup, isentry).SetAllowRefresh(!isentry).SetEnable(rowenable);
              hpars.Add(pr);
              break;
            case "mode":
              ArrayList listpar = new ArrayList();
              for (int mode = 0; mode < ((BaseBO)ctrl).MaxModePreviewIndex; mode++)
              {
                listpar.Add(new Ext.Net.Parameter() { Name = "Mode  ", Value = mode.ToString() });
              }
              hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitle(rowname),
                listpar, "Value=Name", rowwidth).SetAllowRefresh(false).SetEnable(rowenable));
              break;
            case "select":
              hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitle(rowname),
                BaseDataControl.GetList(dcLookup), fieldmap, rowwidth).SetAllowRefresh(false).SetEnable(rowenable));
              break;
            case "kdlevel":
              hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitle("Kdlevel"),
                BaseDataControl.GetList(new DmlevelControl((BaseBO)ctrl)), "Kdlevel=Nmlevel", rowwidth).SetAllowRefresh(false).SetEnable(rowenable));
              break;
            case "status":
              hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
                BaseDataControl.GetList(new DmstatusControl()), "Idstatus=Nmstatus", rowwidth).SetAllowRefresh(false).SetEnable(rowenable));
              break;
            case "type":
              hpars.Add(new ParameterRowType(ctrl, true));
              break;
            case "checked":
              hpars.Add(new ParameterRowCek(ctrl, true));
              break;
            case "file":
              hpars.Add(new ParameterRowUploadFile(ctrl, true));
              break;
            case "help":
              hpars.Add(new ParameterRowHelp(ctrl, true));
              break;
            case "forum":
              hpars.Add(new ParameterRowForum(ctrl, true));
              break;
            case "creby":
              hpars.Add(new ParameterRowCreBy(ctrl));
              break;
            case "modby":
              hpars.Add(new ParameterRowModBy(ctrl));
              break;
            case "revby":
              hpars.Add(new ParameterRowRevBy(ctrl));
              break;
            case "appby":
              hpars.Add(new ParameterRowAppBy(ctrl));
              break;
            case "signby":
              hpars.Add(new ParameterRowSignBy(ctrl));
              break;
            case "lastby":
              hpars.Add(new ParameterRowLastBy(ctrl));
              break;
            default:
              hpars.Add(new ParameterRowTextBox(ctrl, ConstantDict.GetColumnTitle(rowname), roweditable, rowwidth).SetEnable(rowenable));
              break;
          }
        }
        catch (Exception ex)
        {
          if (MasterAppConstants.Instance.StatusTesting)
          {
            throw ex;
          }
        }
      }
      return hpars;

    }

    public static DataControlFieldCollection GetColumn(IDataControlUI ctrl, int modePreviewIndex)
    {
      List<SysgetcolsControl> list = SysgetcolsLookupControl.GetColumns(ctrl, modePreviewIndex);
      DataControlFieldCollection Columns = new DataControlFieldCollection();
      for (int i = 0; i < list.Count; i++)
      {
        try
        {
          SysgetcolsControl dc = list[i];
          MyBoundField col = null;
          int mode = dc.Mode;
          int colwidth = dc.Colwidth;
          string colname = dc.Colname;
          string colalign = dc.Colalign;
          string coltype = dc.Coltype;
          string colavaillist = dc.Colavaillist;
          bool coleditable = dc.Coleditable;
          string[] rowavs = null;
          IList availableList = null;
          string tname = null;
          string fieldmap = null;
          string[] fieldmaps = null;
          if (!string.IsNullOrEmpty(colavaillist))
          {
            rowavs = colavaillist.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            tname = rowavs[0];
            if (!string.IsNullOrEmpty(tname))
            {
              IDataControlLookup dcSelect = (IDataControlLookup)UtilityBO.Create(tname);
              fieldmap = (rowavs.Length > 1) ? rowavs[1] : dcSelect.GetFieldValueMap();
              fieldmaps = fieldmap.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
              availableList = dcSelect.View();
            }
          }
          switch (coltype)
          {
            case "rating":
              Columns.Add(ExtFields.GetRatingField());
              break;
            case "icon":
              Columns.Add(Fields.Create(ConstantDict.GetColumnTitle(colname), typeof(Ext.Net.Icon), colwidth, GetAlign(colalign)));
              break;
            case "command":
              if (colwidth < 20)
              {
                colwidth = 20;
              }
              Columns.Add(Fields.Create(ConstantDict.GetColumnTitle(colname), typeof(CommandColumn), colwidth, GetAlign(colalign)));
              break;
            default:
              col = Fields.Create(ConstantDict.GetColumnTitle(colname), coltype, colwidth, GetAlign(colalign)).SetEditable(coleditable);
              if (coleditable)
              {
                col.SetAvailableValues(availableList, fieldmaps);
              }
              Columns.Add(col);
              break;
          }
        }
        catch (Exception ex)
        {
          UtilityBO.Log(ctrl, ex);
        }
      }
      return Columns;
    }
    public static HorizontalAlign GetAlign(string colalign)
    {
      switch (colalign)
      {
        case "left": return HorizontalAlign.Left;
        case "center": return HorizontalAlign.Center;
        case "right": return HorizontalAlign.Right;
      }
      return HorizontalAlign.Left;
    }
    public static Type GetType(string coltype)
    {
      switch (coltype)
      {
        case "string": return typeof(string);
        case "short": return typeof(short);
        case "int": return typeof(int);
        case "decimal": return typeof(decimal);
        case "bool": return typeof(bool);
      }
      return typeof(string);
    }
    public string[] GetScript()
    {
      string script = @"
        var GetURL = function (url,val) {
            return (url+'id={0}').format(val);
        };
        var ShowHidden = function(type) {
          if (type=='D') {
            return 'visible';/**/
          }else{
            return 'hidden';/**/
          }
        };
        var prepareCommands = function (grid, commands, record, row) {
          GetCommandStatus(commands,record.get('Status'));
        };
        var prepareCommand = function(grid, command, record, row)
        {
        };
        var prepareCellCommands = function(grid, commands, record, row, col, value)
        {
        };
        var prepareCellCommand = function(grid, command, record, row, col, value)
        {
          command.command = command.command.replace('Detil','Link');
        };
        var myiconket = function (txt) {
          if (txt == 'buletmerah.png') {
            return 'Status Hapus';
          } else if (txt == 'buletkuning.png') {
            return 'Status Normal';
          } else if (txt == 'buletbiru.png') {
            return 'Status Valid';
          }
        };      
      ";
      return new string[] { script };
    }

  }
  public abstract class BaseDataControlLog : BaseDataControlExt, IExtLoadCsv
  {
    public BaseDataControlLog()
    {
      ModeDB = SQLDataSource.MODE_DB_LOG;
    }
  }

}
