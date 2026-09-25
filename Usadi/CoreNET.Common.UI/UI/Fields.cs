using System;
using System.Collections;
using System.Text;
using System.Web.UI.WebControls;

namespace CoreNET.Common.Base
{
  /// <summary>
  /// 
  /// </summary>
  public class Fields
  {
    public static ButtonField GetLinkBoundField(string namelabel)
    {
      return GetLinkBoundField(namelabel, typeof(string), 10);
    }
    public static ButtonField GetLinkBoundField(string namelabel, Type type)
    {
      return GetLinkBoundField(namelabel, type, 10);
    }
    public static ButtonField GetLinkBoundField(string namelabel, Type type, int width)
    {
      return GetLinkBoundField(namelabel, type, width, HorizontalAlign.Left);
    }
    public static ButtonField GetLinkBoundField(string namelabel, Type type, int width, HorizontalAlign align)
    {
      return GetLinkBoundField(namelabel, type, width, HorizontalAlign.Left, "");
    }
    public static ButtonField GetLinkBoundField(string namelabel, Type type, int width, HorizontalAlign align, string url)
    {
      ButtonField f = new ButtonField();
      string[] temps = namelabel.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
      f.DataTextField = temps[0];
      f.AccessibleHeaderText = url;
      f.FooterText = type.Name;
      f.HeaderText = temps[0];
      if (temps.Length > 1)
      {
        f.HeaderText = temps[1];
      }
      f.CommandName = "Select";
      f.ShowHeader = false;
      f.ItemStyle.Width = new Unit(width, UnitType.Percentage);
      f.HeaderStyle.CssClass = "grvtxtmod";
      if (type == typeof(decimal))
      {
        f.ItemStyle.Width = new Unit(10, UnitType.Percentage);
        f.ItemStyle.HorizontalAlign = align;
        f.DataTextFormatString = "{0:N2}";
      }
      else if (type == typeof(DateTime))
      {
        f.ItemStyle.HorizontalAlign = align;
        f.DataTextFormatString = "{0:dd/MM/yyyy}";
      }
      else if (type == typeof(Boolean))
      {
        f.ItemStyle.HorizontalAlign = align;
      }
      else if (type == typeof(int))
      {
        f.ItemStyle.Width = new Unit(5, UnitType.Percentage);
        f.ItemStyle.HorizontalAlign = align;
      }
      else //string or no set width 
      {
        f.ItemStyle.HorizontalAlign = align;
      }
      return f;
    }
    public static MyBoundField Create(string namelabel)
    {
      return Create(namelabel, typeof(string));
    }
    public static MyBoundField Create(Type type)
    {
      return GetTextBoundField(string.Empty, type, -1, HorizontalAlign.NotSet);
    }
    public static MyBoundField Create(string namelabel, Type type)
    {
      return GetTextBoundField(namelabel, type, -1, HorizontalAlign.NotSet);
    }
    public static MyBoundField Create(string namelabel, Type type, int width)
    {
      return GetTextBoundField(namelabel, type, width, HorizontalAlign.NotSet);
    }
    public static MyBoundField Create(string namelabel, Type type, object cmds, int width, HorizontalAlign halign)
    {
      MyBoundField field=GetTextBoundField(namelabel, type, width, halign, false, "");
      field.SetCommand(cmds);
      return field;
    }
    public static MyBoundField Create(Type type, object cmd)
    {
      MyBoundField col = GetTextBoundField(string.Empty, type.Name, 5, HorizontalAlign.Center, false, "");
      col.SetCommand(cmd);
      return col;
    }
    public static MyBoundField Create(string namelabel, Type type, int width, HorizontalAlign halign)
    {
      return GetTextBoundField(namelabel, type.Name, width, halign, false, "");
    }
    public static MyBoundField Create(string namelabel, string typename, int width, HorizontalAlign halign)
    {
      return GetTextBoundField(namelabel, typename, width, halign, false, "");
    }
    public static MyBoundField GetTextBoundField(string namelabel, Type type, int width, HorizontalAlign halign)
    {
      return GetTextBoundField(namelabel, type, width, halign, false, "");
    }
    public static MyBoundField GetTextBoundField(string namelabel, Type type, int width, HorizontalAlign halign, string format)
    {
      return GetTextBoundField(namelabel, type, width, halign, false, format);
    }
    public static MyBoundField GetTextBoundField(string namelabel, Type type, int width, HorizontalAlign halign, bool multiline, string format)
    {
      return GetTextBoundField(namelabel, type.Name, width, halign, false, format);
    }
    public static MyBoundField GetTextBoundField(string nmlbl, string typename, int width, HorizontalAlign halign, bool multiline, string format)
    {
      string namelabel = nmlbl;
      if (typename.ToLower().Contains("icon"))
      {
        namelabel = "Stricon=";
      }
      MyBoundField f = new MyBoundField();
      #region base
      string[] temps = namelabel.Split(new string[] { "=" }, StringSplitOptions.None);
      f.DataField = temps[0];
      f.HeaderText = temps[0];
      f.FooterText = typename;
      if (temps.Length > 1)
      {
        string[] headers = temps[1].Split(new string[] { "," }, StringSplitOptions.None);
        int lang = ConstantDict.IDLOCALE;
        f.HeaderText = (headers.Length > lang) ? headers[lang] : headers[0];
      }

      if (typename.ToLower().Contains("decimal") || typename.ToLower().Contains("money"))
      {
        f.ItemStyle.Width = new Unit(10, UnitType.Percentage);
        f.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
        f.DataFormatString = string.IsNullOrEmpty(format) ? "0.00" : format;//"{0:N2}"
      }
      else if (typename.ToLower().Contains("date"))
      {
        f.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        //f.DataFormatString = string.IsNullOrEmpty(format)? "{0:dd/MM/yyyy}" : format;
        f.DataFormatString = string.IsNullOrEmpty(format) ? "dd/MM/yyyy" : format;
      }
      else if (typename.ToLower().Contains("bool"))
      {
        f.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
      }
      else if (typename.ToLower().Contains("int"))
      {
        f.ItemStyle.Width = new Unit(5, UnitType.Percentage);
        f.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
      }
      else //string or no set width 
      {
        f.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
        if (typename.ToLower().Contains("string") && (multiline))
        {
          f.HtmlEncode = false;
          f.AccessibleHeaderText = "<br/>";
        }
      }
      if (width != -1)
      {
        f.ItemStyle.Width = new Unit(width, UnitType.Percentage);
      }
      if (typename.ToLower().Contains("icon"))
      {
        //if(dc)
        f.ItemStyle.Width = new Unit(5, UnitType.Percentage);
      }
      if (halign != HorizontalAlign.NotSet)
      {
        f.ItemStyle.HorizontalAlign = halign;
      }
      #endregion
      return f;
    }

    public static TemplateField GetTextBoxField(string title, string key, Type type)
    {
      TemplateField f = new TemplateField();
      f.HeaderText = title;
      f.FooterText = type.Name;
      f.ShowHeader = false;
      if ((type == typeof(decimal)) || (type == typeof(DateTime)))
      {
        f.ItemStyle.Width = new Unit(10, UnitType.Percentage);
      }
      else if (type == typeof(int))
      {
        f.ItemStyle.Width = new Unit(5, UnitType.Percentage);
      }
      else
      {

      }
      f.HeaderStyle.CssClass = "grvtxtmod";
      f.ItemTemplate = new TextBoxRowTemplate(key);
      return f;
    }

  }

  public class MyBoundField : BoundField
  {
    #region Property IsList
    private bool _IsList;
    public bool IsList
    {
      get { return _IsList; }
      set { _IsList = value; }
    }
    #endregion
    #region Property KeyField
    private string _KeyField;
    public string KeyField
    {
      get { return _KeyField; }
      set { _KeyField = value; }
    }
    #endregion
    #region Property KeyFields
    private string[] _KeyFields;
    public string[] KeyFields
    {
      get { return _KeyFields; }
      set { _KeyFields = value; }
    }
    #endregion
    #region Property AvailableValues
    private IList _AvailableValues;
    public IList AvailableValues
    {
      get { return _AvailableValues; }
      set { _AvailableValues = value; }
    }
    #endregion
    #region Property Editable
    private bool _Editable;
    public bool Editable
    {
      get { return _Editable; }
      set { _Editable = value; }
    }
    #endregion      
    #region Property IsLink
    private bool _IsLink;
    public bool IsLink
    {
      get { return _IsLink; }
      set { _IsLink = value; }
    }
    #endregion
    #region Property Locked
    private bool _Locked;
    public bool Locked
    {
      get { return _Locked; }
      set { _Locked = value; }
    }
    #endregion
    #region Property IsCommand
    private bool _IsCommand;
    public bool IsCommand
    {
      get { return _IsCommand; }
      set { _IsCommand = value; }
    }
    #endregion
    #region Property Command
    private object _Command;
    public object Command
    {
      get { return _Command; }
      set {
        if (value != null)
        {
          IsCommand = true;
          _Command = value;
        }
      }
    }
    #endregion
    #region Property Fn
    private string _Fn;
    public string Fn
    {
      get { return _Fn; }
      set { _Fn = value; }
    }
    #endregion

    #region Method
    public MyBoundField SetIsList(bool val)
    {
      this.IsList = val;
      return this;
    }
    public MyBoundField SetKeyField(string field)
    {
      this.KeyField = field;
      return this;
    }
    public MyBoundField SetKeyFields(string[] fields)
    {
      this.KeyFields = fields;
      return this;
    }
    public MyBoundField SetIsLink(bool islink)
    {
      this.IsLink = islink;
      return this;
    }
    public MyBoundField SetEditable(bool editable)
    {
      this.Editable = editable;
      return this;
    }
    public MyBoundField SetLocked(bool locked)
    {
      this.Locked = locked;
      return this;
    }
    public MyBoundField SetAvailableValues(IList values, string[] keyfields)
    {
      this.AvailableValues = values;
      this.KeyFields = keyfields;
      return this;
    }
    public MyBoundField SetVisible(bool visible)
    {
      this.Visible = visible;
      return this;
    }
    public MyBoundField SetCommand(object cmd)
    {
      this.Command = cmd;
      return this;
    }
    public MyBoundField SetFn(string fn)
    {
      this.Fn = fn;
      return this;
    }

    #endregion
  }
}
