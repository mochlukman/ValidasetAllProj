using CoreNET.Common.Base;
using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.Ss20groupAppControl, CoreNET.Common.Sys
  [Serializable]
  public class Ss20groupAppControl : Ss20groupControl, IDataControlUIEntry
  {
    public new ImageCommand[] Cmds
    {
      get
      {
        ImageCommand cmd1 = new ImageCommand()
        {
          CommandName = "ViewMenu",
          Icon = Ext.Net.Icon.TableAdd
        };
        cmd1.ToolTip.Text = "Klik tombol ini untuk menampilkan rincian menu";
        return new ImageCommand[] { cmd1 };
      }
    }
    //public string ViewMenu
    //{
    //  get
    //  {
    //    string app = GlobalAsp.GetRequestApp();
    //    string id = "CEB0626A-9997-4315-9767-EC67925E4B62";// GlobalAsp.GetRequestId();
    //    string idprev = GlobalAsp.GetRequestId();
    //    string kode = GlobalAsp.GetRequestKode();
    //    string idx = GlobalAsp.GetRequestIndex();
    //    string strenable = "&enable=" + ((Status == 0) ? 1 : 0);
    //    string url = string.Format("PageTabular.aspx?i=1&app={0}&id={1}&idprev={2}&kode={3}&idx={4}"
    //      + strenable, app, id, idprev, kode, idx);
    //    return "Daftar Modul:" + url;
    //  }
    //}

    public Ss20groupAppControl()
    {
      XMLName = ConstantTablesSys.XMLSS20GROUP;
    }

    private ViewListProperties cViewListProperties = null;
    public new IProperties GetProperties()
    {
      if (cViewListProperties == null)
      {
        cViewListProperties = (ViewListProperties)base.GetProperties();
        cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_READONLY;
      }
      return cViewListProperties;
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(Ss20groupControl).IsInstanceOfType(bo))
      {
        Kdgroup = ((Ss20groupControl)bo).Kdgroup;
        Nmgroup = ((Ss20groupControl)bo).Nmgroup;
      }
    }
    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalAsp.GetRequestIdPrev());
      HashTableofParameterRow hpars = new HashTableofParameterRow
      {
        Ss20groupLookupControl.Instance.GetLookupParameterRow(this, false).SetEnable(enableFilter)
      };
      return hpars;
    }

    public new IList View()
    {
      IList list = this.View("App");
      return list;
    }
    public new IList View(string label)
    {
      IList list = ((BaseDataControl)this).View(label);
      List<Ss20groupControl> ListData = new List<Ss20groupControl>();
      foreach (Ss20groupControl dc in list)
      {
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        dc.Kdgroup = Kdgroup;
        dc.Nmgroup = Nmgroup;
        ListData.Add(dc);
      }
      return ListData;
    }
    public new DataControlFieldCollection GetColumns()
    {
      DataControlFieldCollection columns = new DataControlFieldCollection
      {
        ExtFields.GetRatingField(),
        Fields.Create(ConstantDict.GetColumnTitle("Stricon"), typeof(CommandColumn), Cmds, 5, HorizontalAlign.Center),
        Fields.Create(ConstantDict.GetColumnTitle("Kdapp"), typeof(string), 15, HorizontalAlign.Left),
        Fields.Create(ConstantDict.GetColumnTitle("Nmapp"), typeof(string), 70, HorizontalAlign.Left),
        Fields.Create(ConstantDict.GetColumnTitle("Idapp"), typeof(string), 40, HorizontalAlign.Center),
      };
      return columns;
    }
    public override HashTableofParameterRow GetEntries()
    {
      bool enable = false;
      if (hpars == null)
      {
        hpars = new HashTableofParameterRow
        {
          new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Kdapp"), false, 30).SetEnable(enable),
          new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Nmapp"), true, 3).SetEnable(enable),
          new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Idapp"), true, 95).SetEnable(enable),
          new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
          DmstatusLookupControl.GetListDataSingleton(string.Empty), "Idstatus=Nmstatus", 30).SetAllowRefresh(false).SetEnable(enable),
          new ParameterRowUploadFile(this, true),
          new ParameterRowHelp(this, true),
          new ParameterRowForum(this, true)
        };
      }
      return hpars;
    }

  }
  #endregion Ss20groupApp
}

