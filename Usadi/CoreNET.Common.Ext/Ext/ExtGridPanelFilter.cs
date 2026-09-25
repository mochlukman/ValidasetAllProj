using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using Ext.Net;
using Ext.Net.Utilities;
using CoreNET.Common.Base;

namespace CoreNET.Common.Base
{

  public class ExtGridPanelFilter : ExtGridPanel
  {
    //Filter GridPanel
    public ExtGridPanelFilter(IDataControlUI dc, ParameterRow pr, string id, string targetFormPanel, int target)
    {
      this.ID = "GP" + id;
      this.Region = Region.Center;
      this.AutoWidth = true;
      this.AutoHeight = false;
      this.Border = false;
      this.StripeRows = true;
      this.TrackMouseOver = true;
      this.LoadMask.ShowMask = true;
      string type = pr.DCLookup.GetType().AssemblyQualifiedName; 
      string label = pr.LabelQuery;
      int totwidth = 700;//ExtWindows.DEFAULT_WIDTH - 50;

      DataControlFieldCollection cols = new DataControlFieldCollection();
      if (typeof(BaseDataControlExt).IsInstanceOfType(dc))
      {
        cols = ((BaseDataControlExt)dc).GetColumns();
      }
      if (cols.Count == 0)
      {
        cols = dc.GetColumns();
        //@todo Insert ke database
      }

      SetColumnCollection(this, dc, cols, totwidth, MODE_SINGLE_SELECT);//tadinya GetColumnFilter

      this.Plugins.Add(BuildGridFilters(dc, id));
    }

    public const int ENTRY = 0;
    public const int GRID = 1;
    public const int TREE = 2;
    //protected PagingToolbar BuildLookupPagingToolbar(string winid, GridPanel gridPanel, string targetFormPanel, int target)
    //{
    //  PagingToolbar pageTB = new PagingToolbar();
    //  pageTB.HideLabels = true;
    //  pageTB.HideRefresh = false;
    //  pageTB.ID = "PageTBFilter" + winid;
    //  Ext.Net.Button btnSelect = new Ext.Net.Button() { ID = "btnSelect" + this.ID, Text = "Pilih", Icon = Icon.Accept };
    //  if (target == ENTRY)
    //  {
    //    btnSelect.Listeners.Click.Handler =
    //      "parent." + targetFormPanel + ".getForm().loadRecord(#{" + gridPanel.ID + "}.getSelectionModel().getSelected());" +
    //      "parent." + winid + ".hide();";
    //  }
    //  else
    //  {
    //    btnSelect.Listeners.Click.Handler =
    //      "parent." + targetFormPanel + ".getForm().loadRecord(#{" + gridPanel.ID + "}.getSelectionModel().getSelected());" +
    //      "parent.Store1.reload();" +
    //      //"#{StoreDet1}.reload();" +
    //      ((target == GRID) ? "" : "refreshTree();") +
    //      "parent." + winid + ".hide();";
    //  }
    //  pageTB.Add(btnSelect);

    //  Ext.Net.Button btnCancel = new Ext.Net.Button() { ID = "btnCancel" + this.ID, Text = "Batal", Icon = Icon.Cancel };
    //  btnCancel.Listeners.Click.Handler = "parent." + winid + ".hide();";
    //  pageTB.Add(btnCancel);
    //  return pageTB;
    //}
  }
}
