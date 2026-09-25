using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections.Generic;
using System.Text;

namespace CoreNET.Common.Base
{
  /// <summary>
  /// 
  /// </summary>
  public class TextBoxRowTemplate : ITemplate
  {
    private string columnName;

    public TextBoxRowTemplate(string field)
    {
      columnName = field;
    }

    public void InstantiateIn(System.Web.UI.Control container)
    {
      TextBox utxt = new TextBox();
      utxt.ID = "utxt_Value";

      utxt.DataBinding += new EventHandler(this.TextBinding);
      container.Controls.Add(utxt);
    }
    private void TextBinding(Object sender, EventArgs e)
    {
      TextBox utxt = (TextBox)sender;
      GridViewRow row = (GridViewRow)utxt.NamingContainer;
      utxt.Text = DataBinder.Eval(row.DataItem, columnName).ToString();
    }
  }

  public class TextBoxRow2Template : ITemplate
  {
    private string columnName1;
    private string columnName2;
    private string columLabel2;

    public TextBoxRow2Template(string field1,string field2,string label2)
    {
      columnName1 = field1;
      columnName2 = field2;
      columLabel2 = label2;
    }

    public void InstantiateIn(System.Web.UI.Control container)
    {
      TextBox utxt1 = new TextBox();
      utxt1.ID = "utxt_Value1";
      utxt1.DataBinding += new EventHandler(this.TextBinding1);
      container.Controls.Add(utxt1);

      Label ulbl2 = new Label();
      ulbl2.ID = "ulbl_Value2";
      container.Controls.Add(ulbl2);

      TextBox utxt2 = new TextBox();
      utxt2.ID = "utxt_Value2";
      utxt2.DataBinding += new EventHandler(this.TextBinding2);
      container.Controls.Add(utxt2);
}
    private void TextBinding1(Object sender, EventArgs e)
    {
      TextBox utxt = (TextBox)sender;
      GridViewRow row = (GridViewRow)utxt.NamingContainer;
      utxt.Text = DataBinder.Eval(row.DataItem, columnName1).ToString();
    }
    private void TextBinding2(Object sender, EventArgs e)
    {
      TextBox utxt = (TextBox)sender;
      GridViewRow row = (GridViewRow)utxt.NamingContainer;
      utxt.Text = DataBinder.Eval(row.DataItem, columnName2).ToString();
    }
  }

}
