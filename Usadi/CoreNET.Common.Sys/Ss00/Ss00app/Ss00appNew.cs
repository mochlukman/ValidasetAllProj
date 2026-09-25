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
  #region CoreNET.Common.BO.Ss00appNewControl, CoreNET.Common.BO
  [Serializable]
  public class Ss00appNewControl :  Ss00appLinkControl, IDataControlTreeGrid3
  {
    public Ss00appNewControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APP;
      SetMaxModePreviewIndex(1);
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      return cViewListProperties;
    }
    public new void SetTreeGridColumns(TreeGridColumnCollection Columns)
    {
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kdapp"), Width = 200, DataIndex = "Kdapp", Align = Ext.Net.TextAlign.Left });
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Nmapp"), Width = 500, DataIndex = "Nmapp", Align = Ext.Net.TextAlign.Left });
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Nchild"), Width = 70, DataIndex = "Nchild", Align = Ext.Net.TextAlign.Center });
      Columns.Add(BaseDataControlExt.TreeColStatus);
      if (GetModePreviewIndex() == 1)
      {
        #region Command
        TreeGridColumn col1 = new TreeGridColumn { Header = ConstantDict.Translate(string.Empty), Width = 200, Align = Ext.Net.TextAlign.Center };
        col1.XTemplate.Html = @"
        <a style='visibility:true;'
          onclick = 'parent.loadPageByID(""Pages"",""Edit {Kdapp}"",""Edit {Kdapp}"", ""{[GetURLEdit(values.Kdapp)]}"");'>Edit</a>
        |<a style='visibility:true;'
          onclick = 'parent.loadPageByID(""Pages"",""Config {Kdapp}"",""Config {Kdapp}"", ""{[GetURLConfig(values.Kdapp)]}"");'>Config</a>
        |<a style='visibility:true;'
          onclick = 'parent.loadPageByID(""Pages"",""Login {Kdapp}"",""Login {Kdapp}"", ""{[GetURLLogin(values.Kdapp)]}"");'>Login</a>
        |<a style='visibility:true;'
          onclick = 'parent.loadPageByID(""Pages"",""Menu {Kdapp}"",""Menu {Kdapp}"", ""{[GetURLMenu(values.Kdapp)]}"");'>Menu</a>
        ";
        Columns.Add(col1);
      }

      //col1 = new TreeGridColumn { Header = ConstantDict.Translate(string.Empty), Width = 50, Align = Ext.Net.TextAlign.Center };
      //col1.XTemplate.Html = @"<a style='visibility:true;'
      //  onclick = 'parent.loadPageByID(""Pages"",""Login {Kdapp}"",""Login {Kdapp}"", ""{[GetURLLogin(values.Kdapp)]}"");'>Login</a><a style='visibility:true;'
      //";
      //Columns.Add(col1);

      //col1 = new TreeGridColumn { Header = ConstantDict.Translate(string.Empty), Width = 50, Align = Ext.Net.TextAlign.Center };
      //col1.XTemplate.Html = @"<a style='visibility:true;'
      //  onclick = 'parent.loadPageByID(""Pages"",""Menu {Kdapp}"",""Menu {Kdapp}"", ""{[GetURLMenu(values.Kdapp)]}"");'>Menu</a><a style='visibility:true;'
      //";
      //Columns.Add(col1);
      #endregion
    }
  }
  #endregion Ss00appNew
}

