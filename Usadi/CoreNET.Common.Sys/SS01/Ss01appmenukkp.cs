using CoreNET.Common.Base;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreNET.Common.BO
{
  #region Ss01appmenukkp
  [Serializable]
  public class Ss01appmenukkpControl : BaseDataControlSys, IDataControlUIEntry, IExtLoadCsv, IHasJSScript
  {
    #region Properties 
    public string Iddok { get; set; }
    public string Idmenu { get; set; }
    public string Kdmenu { get; set; }
    public string Nmmenu { get; set; }
    public string Kddok { get; set; }
    public string Idxdok { get; set; }
    public string Lastnodok { get; set; }
    public string Nodok { get; set; }
    public string Urdok { get; set; }
    #region Property Statusicon
    private string _Statusicon = ExtIcon.GetStringIcon(MyIcon.StatusEmpty);
    public new string Statusicon
    {
      get => _Statusicon;
      set => _Statusicon = value;
    }
    #endregion

    #region Property Path
    public new string Path
    {
      get => "KKP/" + Cre_date.ToString("yyyy") + "/" + Cre_date.ToString("MM") + "/" + Cre_date.ToString("dd") + "/" + Iddok;
      set => base.Path = value;
    }
    #endregion

    #endregion Properties 
    #region Methods 
    public Ss01appmenukkpControl()
    {
      XMLName = ConstantTablesSys.XMLSS01APPMENUKKP;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Idmenu", "Iddok" };
      cViewListProperties.IDProperty = "Nodok";
      cViewListProperties.IDKey = "Iddok";
      cViewListProperties.ReadOnlyFields = new String[] { "Idmenu" };
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_EDIT;
      return cViewListProperties;
    }

    public new void SetPageKey()
    {
      Idmenu = GlobalAsp.GetRequestId();
    }

    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalAsp.GetRequestIdPrev()) &&
        string.IsNullOrEmpty(GlobalAsp.GetRequestVal());
      HashTableofParameterRow hpars = new HashTableofParameterRow
      {
        new Ss01appmenuLookupControl().GetLookupParameterRow(this, false).SetEnable(enableFilter)
      };
      return hpars;
    }


    public new void SetPrimaryKey()
    {
      Cre_by = GlobalAsp.GetSessionUser().GetUserID();
      Cre_date = DateTime.Now;//Inisialisasi Path untuk upload file
      Iddok = Guid.NewGuid().ToString();
      Kddok = GlobalAsp.GetRequestKode();
      Idxdok = GlobalAsp.GetRequestIndex();
      Nodok = Idxdok;
      UtilityUI.GetNoUrut(this, "Nodok", 3, BaseDataControl.LAST, Idxdok, string.Empty);
    }

    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlMenu).IsInstanceOfType(bo))
      {
        if (!string.IsNullOrEmpty(GlobalAsp.GetRequestVal()))
        {
          Idmenu = GlobalAsp.GetRequestVal();
        }
        SsappmenuLookupControl.FindAndSetValuesInto(this);
        //Idapp = GlobalAsp.GetSessionApp();//harusnya IDAPP si IDMENU
        //SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
      }

    }
    public static Ss01appmenukkpControl GetLastNodok(string idmenu, string kddok)
    {
      Ss01appmenukkpControl dc = new Ss01appmenukkpControl
      {
        Idmenu = idmenu,
        Nodok = kddok
      };
      IList list = dc.View(BaseDataControl.LAST);
      if (list.Count == 0)
      {
        return null;
      }
      else
      {
        return (Ss01appmenukkpControl)list[0];
      }

    }
    public new IList View()
    {
      Kddok = GlobalAsp.GetRequestKode();
      Idxdok = GlobalAsp.GetRequestIndex();
      Nodok = Idxdok;
      IList list = View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      IList list = ((BaseDataControl)this).View(label);
      List<Ss01appmenukkpControl> ListData = new List<Ss01appmenukkpControl>();
      foreach (Ss01appmenukkpControl dc in list)
      {
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      return ListData;
    }

    public new void Insert()
    {
      SetPrimaryKey();
      base.Insert();
      try
      {
        Kddok = GlobalAsp.GetRequestKode();
        if (!string.IsNullOrEmpty(Kddok))
        {
          BaseDataAdapter.ExecuteCmd(this, string.Format("exec SetJenisDok '{0}','{1}'", Iddok, Kddok));
        }
      }
      catch (Exception ex)
      {
        UtilityBO.Log(this, ex);
      }
    }

    public new int Update()
    {
      return -1;
      //int n = -1;
      //Statusicon = ExtIcon.GetStringIcon(ExtIcon.GetIcon(this));
      //Pm01menumenuControl dcMenu = (Pm01menumenuControl)GlobalExt.GetSessionMenu();
      //switch (dcMenu.Usertype)
      //{
      //  case PMUtils.STR_AT:
      //    n = base.Update(BaseDataControl.DEFAULT);
      //    try
      //    {
      //      Kddok = GlobalAsp.GetRequestKode();
      //      if (!string.IsNullOrEmpty(Kddok))
      //      {
      //        BaseDataAdapter.ExecuteCmd(this, string.Format("exec SetJenisDok '{0}','{1}'", Iddok, Kddok));
      //      }
      //    }
      //    catch (Exception ex) 
      //    { 
      //      UtilityBO.Log(dc, ex);
      //    }
      //    break;
      //  default:
      //    n = UpdateByUserType(dcMenu.Usertype, ((ViewListProperties)GetProperties()).IDKey);
      //    break;
      //}

      //try
      //{
      //  Pm01menumenuControl dc = new Pm01menumenuControl();
      //  dc.Idmenu = HttpContext.Current.Request["menu"];
      //  dc.Idmenu = HttpContext.Current.Request["id"];
      //  dc = Pm01menumenuLookupControl.FindAndSetValuesInto(dc);
      //  dc.Nodok = Nodok;
      //  dc.Nfiles = Nfiles;
      //  dc.Statusicon = Statusicon;
      //  dc.Progress = Progress;
      //  dc.Update();
      //}
      //catch (Exception ex) 
      //{ 
      //  UtilityBO.Log(dc, ex);
      //}
      //return n;
    }
    public new string[] GetScript()
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
          if(record.get('Nodok')===record.get('Lastnodok'))
          {
            GetCommandStatus(commands,record.get('Status'));
          }
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
      ";
      return new string[] { script };
    }
    public new bool IsEditable()
    {
      bool enable = string.IsNullOrEmpty(Nodok) || (!string.IsNullOrEmpty(Nodok) && Nodok.Equals(Lastnodok) && true);
      return enable && Editable;
    }

    public new HashTableofParameterRow GetEntries()
    {
      bool enable = IsEditable();
      HashTableofParameterRow hpars = base.GetEntries();
      foreach (string key in hpars.Keys)
      {
        ParameterRow pr = (ParameterRow)hpars[key];
        pr.SetEnable(enable && pr.Enable);
      }
      return hpars;
    }

    #endregion Methods 
  }
  #endregion Ss01appmenukkp
}

