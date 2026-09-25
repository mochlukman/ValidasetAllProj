using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using Ext.Net;
using Ext.Net.Utilities;
using CoreNET.Common.Base;
using CoreNET.Common.BO;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.Ss01appmenuAdminControl, CoreNET.Common.BO
  [Serializable]
  public class Ss01appmenuAdminControl : Ss01appmenuControl, IDataControlMenu, IHasJSScript
  {
    public Ss01appmenuAdminControl()
    {
      XMLName = ConstantTablesSys.XMLSS01APPMENU;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      return cViewListProperties;
    }
    public new void SetPageKey()
    {
    }
    public new void SetFilterKey(BaseBO bo)
    {
      base.SetFilterKey(bo);
    }

    public new HashTableofParameterRow GetFilters()
    {
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      return hpars;
    }
    public new void SetTreeGridColumns(TreeGridColumnCollection Columns)
    {
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kdmenu"), Width = 200, DataIndex = "Kdmenu", Align = Ext.Net.TextAlign.Left });
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Nmmenu"), Width = 400, DataIndex = "Nmmenu", Align = Ext.Net.TextAlign.Left });
      TreeGridColumn col1 = new TreeGridColumn { Header = ConstantDict.Translate("Statusicon"), Width = 50, DataIndex = "Statusicon", Align = Ext.Net.TextAlign.Center };
      col1.XTemplate.Html = @"<a style='visibility:true;'>
                    <img style='width: 16px; height: 16px;' title='{Statusname}' src='../Res/Icons/{Statusicon}' alt='{Statusicon}'/></a>
                  ";
      Columns.Add(col1);
      string strapp = "&app=" + Idapp;
      col1 = new TreeGridColumn { Header = ConstantDict.Translate(string.Empty), Width = 280, Align = Ext.Net.TextAlign.Center };
      col1.XTemplate.Html = @"<a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""URL"",""URL"", ""{[GetURLEditor(values.Idmenu)]}"+ strapp + @""");'>URL</a> | <a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""Link"",""Link"", ""{[GetURLMapClass(values.Idmenu)]}" + strapp + @""");'>Link</a> | <a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""Entry"",""Entry"", ""{[GetURLEntry(values.Idmenu)]}" + strapp + @""");'>Entry</a> | <a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""Grid"",""Grid"", ""{[GetURLColGrid(values.Idmenu)]}" + strapp + @""");'>Grid</a> | <a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""Tree"",""Tree"", ""{[GetURLColTree(values.Idmenu)]}" + strapp + @""");'>Tree</a> | <a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""Filter"",""Filter"", ""{[GetURLFilter(values.Idmenu)]}" + strapp + @""");'>Filter</a> | <a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""Keys"",""Keys"", ""{[GetURLCsv(values.Idmenu)]}" + strapp + @""");'>Keys</a> | <a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""List Forum"",""List Forum"", ""{[GetURLListForum(values.Idmenu)]}" + strapp + @""");'>Forum</a>
      ";
      Columns.Add(col1);
      col1 = new TreeGridColumn { Header = ConstantDict.Translate("Forum"), Width = 100, Align = Ext.Net.TextAlign.Center };
      col1.XTemplate.Html = @"<a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""Forum"",""Forum"", ""{[GetURLForum(values.Idmenu)]}" + strapp + @""");'>{Nmsgs} pesan
        </a>
      ";
      Columns.Add(col1);
    }

    public new IList View()
    {
      IList list = base.View(BaseDataControl.ALL);
      return list;
    }

  }
  #endregion Ss01appmenuAdmin
}

