using CoreNET.Common.BO;
using System;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;

namespace CoreNET.Common.Base
{
  public enum ParameterRowEvent
  {
    Abort = 0,
    Blur = 1,
    Change = 2,
    Click = 3,
    DoubleClick = 4,
    Error = 5,
    Focus = 6,
    KeyDown = 7,
    KeyPress = 8,
    KeyUp = 9,
    Load = 10,
    MouseDown = 11,
    MouseMove = 12,
    MouseOut = 13,
    MouseOver = 14,
    MouseUp = 15,
    Reset = 16,
    Resize = 17,
    Select = 18,
    Submit = 19,
    Unload = 20,
  }
  #region ParameterRow
  /*
   * Kalo Ngerefactor field-property, kangan lupa tambahkan initial/default value di constructor
   * 
   * */
  public class ParameterRow : BaseBO
  {
    #region Static
    public static int GetLengthColumn(BaseBO bo, string cname)
    {
      string tname = bo.XMLName;
      int length = -1;
      SysObjects dc = new SysObjects();
      try
      {
        dc = SysObjects.GetTableSysObjSingleton(bo, tname).Find(o => o.ColumnName.Equals(cname.ToUpper()));
        if (dc != null)
        {
          length = dc.Length;
        }
      }
      catch (Exception ex)
      {
        UtilityBO.Log(dc, ex);
      }
      return length;
    }
    public static int[] WIN_DIALOG_SIZE
    {
      get
      {
        string res = (string)HttpContext.Current.Session[GlobalAsp.SESSION_SCREEN_RES];
        int[] widths = new int[] { 20, 75, 0 };
        if (res == "1024x768")
        {
          widths = new int[] { 20, 75, 0 };
        }
        return widths;
      }
    }
    #endregion
    #region Constant
    public const int SELECTION_CRITERIA_LEVEL = 1;
    public const int SELECTION_CRITERIA_TYPE = 2;
    public const int ROWS_FORM = 1;
    public const int ROWS_FILTER = 2;
    public const int MODE_ENTRY = 11;
    public const int MODE_SELECT = 12;
    public const int MODE_LOOKUP = 13;
    public const int MODE_TYPE = 14;
    public const int MODE_NUMERIC = 15;
    public const int MODE_ENTRY2 = 16;
    public const int MODE_MEMO = 17;
    public const int MODE_LOOKUP2 = 18;
    public const int MODE_DATE = 19;
    public const int MODE_DATE_RANGE = 31;
    public const int MODE_CHECK = 20;
    public const int MODE_TIME = 21;
    public const int MODE_YESNO = 22;
    public const int MODE_MINUTE = 23;
    public const int MODE_TREELOOKUP = 24;
    public const int MODE_FILEUPLOAD = 25;
    public const int MODE_HTML = 26;
    public const int MODE_FORUM = 27;
    public const int MODE_PANEL = 28;
    public const int MODE_TEMUAN = 29;
    public const int MODE_RADIO = 30;
    #endregion
    #region Field
    private string _Name;
    private object _Value;
    //private int _Position = ROWS_FORM;
    private string _LabelValue;
    //private int _ModeEdit = MODE_ENTRY;
    private Type _Type;
    private Type _Type2;
    //private Dictionary<ParameterRowEvent, string> _Listeners = new Dictionary<ParameterRowEvent, string>();
    //private ParameterRow Param2 = null;
    //private ParameterRow Param3 = null;
    //private ParameterRow Param4 = null;
    #endregion
    #region Properties
    #region Property
    #region Property AlwaysEnable
    /*
      * Gunanya apa ya?
      * */
    private bool _AlwaysEnable;
    public bool AlwaysEnable
    {
      get => _AlwaysEnable;
      set => _AlwaysEnable = value;
    }
    #endregion

    //public Dictionary<ParameterRowEvent, string> Listeners { get; set; }

    public string Group { get; set; }
    public string Action { get; set; }
    //public int Position { get; set; }
    public int Id { get; set; }
    public bool AllowEmpty { get; set; }
    public string Label { get; set; }
    public string Hint { get; set; }
    public string LabelValue { get; set; }
    public bool Enable { get; set; }
    public bool Flexible { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public new int Row { get; set; }
    public Object Values { get; set; }
    //public IBaseDao Dao { get; set; }
    public bool Visible { get; set; }
    public bool[] VisibleControls { get; set; }
    public int ModeEdit { get; set; }
    public string KeyValue { get; set; }
    //public ImageClickEventHandler Handler { get; set; }
    public string Name
    {
      get => _Name;
      set
      {
        _Name = value;
        if (Param2 == null)
        {
          Param2 = new ParameterRow();
        }
        if (Param3 == null)
        {
          Param3 = new ParameterRow();
        }
      }
    }
    public Object Value
    {
      get => _Value;
      set
      {
        _Value = value;
        if (value == null)
        {
          _LabelValue = "";
          return;
        }
        if (_Value.GetType() == typeof(decimal))
        {
          _LabelValue = ((decimal)_Value).ToString("###,##0.00");
        }
        else if (_Value.GetType() == typeof(DateTime))
        {
          _LabelValue = ((DateTime)_Value).ToString("dd/MM/yyyy");
        }
        else if (_Value.GetType() == typeof(int))
        {
          _LabelValue = _Value.ToString();
        }
        else
        {
          _LabelValue = _Value.ToString();
        }
        _Type = value.GetType();
      }
    }

    public string Name2
    {
      get => (Param2 == null) ? "" : Param2.Name;
      set
      {
        if (Param2 != null)
        {
          Param2.Name = value;
        }
      }
    }
    public Type Type2
    {
      get => _Type2;
      set => _Type2 = value;
    }
    public string Label2
    {
      get => (Param2 == null) ? "" : Param2.Label;
      set
      {
        if (Param2 != null)
        {
          Param2.Label = value;
        }
      }
    }
    public Object Value2
    {
      get => (Param2 == null) ? "" : Param2.Value;
      set
      {
        if (Param2 != null)
        {
          Param2.Value = value;
        }
      }
    }
    public string LabelValue2 => (Param2 == null) ? "" : Param2.LabelValue;
    public bool Editable2
    {
      get => (Param2 == null) ? true : Param2.Editable;
      set
      {
        if (Param2 != null)
        {
          Param2.Editable = value;
        }
      }
    }
    public int Length2
    {
      get => (Param2 == null) ? 0 : Param2.Length;
      set
      {
        if (Param2 != null)
        {
          Param2.Length = value;
        }
      }
    }
    public int Width2
    {
      get => (Param2 == null) ? 0 : Param2.Width;
      set
      {
        if (Param2 != null)
        {
          Param2.Width = value;
        }
      }
    }
    public ParameterRow Param2 { get; set; }
    public string Name3
    {
      get => (Param3 == null) ? "" : Param3.Name;
      set => Param3.Name = value;
    }
    public Object Value3
    {
      get => (Param3 == null) ? "" : Param3.Value;
      set => Param3.Value = value;
    }
    public Object Value4
    {
      get => (Param4 == null) ? "" : Param4.Value;
      set => Param4.Value = value;
    }
    public ParameterRow Param3 { get; set; }
    public ParameterRow Param4 { get; set; }
    public bool AllowRefresh { get; set; }
    public string URL { get; set; }
    public string Regex { get; set; }
    public IDataControl DCLookup { get; set; }
    public string LabelQuery { get; set; }
    public bool HideWhenEdit { get; set; }
    public string LookupField { get; set; }

    #region Property IsTree
    private bool _IsTree = false;
    public bool IsTree
    {
      get => _IsTree;
      set => _IsTree = value;
    }
    #endregion

    //#region Property ParameterFilters
    //private string[] _ParameterFilters = new string[] { };
    //public string[] ParameterFilters
    //{
    //  get => ParameterFilters;
    //  set => ParameterFilters = value;
    //}
    //#endregion
    //#region Property Fields
    //private string[] _Fields = null;
    //public string[] Fields
    //{
    //  get
    //  {
    //    if (_Fields == null)
    //    {
    //      string[] names = Name.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
    //      return names;
    //    }
    //    else
    //    {
    //      return _Fields;
    //    }
    //  }
    //  set => _Fields = value;
    //}
    //#endregion

    #endregion

    #region Property dipakai buat Lookup/FilterTreePanel
    #region Property SelectionLevel
    private string _Level = (HttpContext.Current.Request["level"] == null) ? "" : HttpContext.Current.Request["level"];
    public string SelectionLevel
    {
      get => _Level;
      set => _Level = value;
    }
    #endregion
    #region Property SelectionType
    private string _SelectionType = "D";
    public string SelectionType
    {
      get => _SelectionType;
      set => _SelectionType = value;
    }
    #endregion
    #region Property SelectionCriteria
    private int _SelectionCriteria = SELECTION_CRITERIA_TYPE;
    public int SelectionCriteria
    {
      get => _SelectionCriteria;
      set => _SelectionCriteria = value;
    }
    #endregion
    #endregion
    #region Property dipakai buat ComboBox
    #region Property Child
    private string _Child = "";
    public string Child
    {
      get => _Child;
      set => _Child = value;
    }
    #endregion
    #region Property Parent
    private string _Parent = "";
    public string Parent
    {
      get => _Parent;
      set => _Parent = value;
    }
    #endregion
    #endregion
    #region Property dipakai buat Lookup/FilterTabular
    #region Property TargetLookupFields
    private string[] _TargetLookupFields;
    public string[] TargetLookupFields
    {
      get => _TargetLookupFields;
      set => _TargetLookupFields = value;
    }
    #endregion
    #endregion
    #region Property dipakai buat ParameterRowProgress
    #region Property MinValue
    private int _MinValue;
    public int MinValue
    {
      get => _MinValue;
      set => _MinValue = value;
    }
    #endregion
    #region Property MaxValue
    private int _MaxValue;
    public int MaxValue
    {
      get => _MaxValue;
      set => _MaxValue = value;
    }
    #endregion
    #endregion

    #region Property ErrorSelectionMsg
    private string _ErrorSelectionMsg;
    public string ErrorSelectionMsg
    {
      get => _ErrorSelectionMsg;
      set => _ErrorSelectionMsg = value;
    }
    #endregion

    #endregion
    #region Constructor
    public ParameterRow()
    {
      AllowEmpty = true;
      Enable = true;
      Editable = true;
      Length = -1;
    }
    public ParameterRow(string namelabel, object value)
      : this(namelabel, value, MODE_ENTRY, 100)
    { }

    //public ParameterRow(string namelabel, ListItemCollection objs)
    //{
    //  string[] temps = namelabel.Split(new string[] { "=" }, StringSplitOptions.None);
    //  Name = temps[0];
    //  Label = Name;
    //  if (temps.Length > 1)
    //  {
    //    string[] headers = temps[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
    //    int lang = ConstantDict.IDLOCALE;
    //    Label = (headers.Length > lang) ? headers[lang] : headers[0];
    //  }
    //  ModeEdit = ParameterRow.MODE_SELECT;
    //  Values = objs;
    //  Editable = true;
    //  Visible = true;
    //}
    public ParameterRow(string namelabel, int modeedit, Object objs, string keyvalue, int width)
    {
      string[] temps = namelabel.Split(new string[] { "=" }, StringSplitOptions.None);
      Name = temps[0];
      Label = Name;
      if (temps.Length > 1)
      {
        string[] headers = temps[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        int lang = ConstantDict.IDLOCALE;
        Label = (headers.Length > lang) ? headers[lang] : headers[0];
      }
      ModeEdit = modeedit;
      Values = objs;
      Editable = true;
      Visible = true;
      KeyValue = keyvalue;
      Width = width;
      if (width == 0)
      {
        Width = 95;
      }
    }
    public ParameterRow(String namelabel, Object value, int modeedit, int width)
    {
      string[] temps = namelabel.Split(new string[] { "=" }, StringSplitOptions.None);
      Name = temps[0];
      Label = Name;
      if (temps.Length > 1)
      {
        string[] headers = temps[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        int lang = ConstantDict.IDLOCALE;
        Label = (headers.Length > lang) ? headers[lang] : headers[0];
      }
      Value = value;
      Visible = true;
      Editable = true;

      ModeEdit = modeedit;
      Width = width;
      if (width == 0)
      {
        Width = 95;
      }

    }
    #endregion
    #region method
    public static ParameterRow GetSingleton(string name)
    {
      return null;
    }
    public ParameterRow SetChild(string child)
    {
      Child = child;
      return this;
    }
    public ParameterRow SetParent(string parent)
    {
      Parent = parent;
      return this;
    }
    public ParameterRow SetAllowRefresh(bool val)
    {
      AllowRefresh = val;
      return this;
    }
    public ParameterRow SetRegex(string regex)
    {
      Regex = regex;
      return this;
    }
    public ParameterRow SetGroup(string val)
    {
      Group = val;
      return this;
    }
    public ParameterRow SetVisible(bool val)
    {
      Visible = val;
      return this;
    }
    public ParameterRow SetHideWhenEdit(bool val)
    {
      HideWhenEdit = val;
      return this;
    }
    public ParameterRow SetAllowEmpty(bool val)
    {
      AllowEmpty = val;
      return this;
    }
    public ParameterRow SetEditable(bool val)
    {
      Editable = val;
      return this;
    }
    public ParameterRow SetAction(string action)
    {
      Action = action;
      return this;
    }
    //public ParameterRow SetEnable(IDataControl dc, bool val)
    //{
    //  if (dc != null)
    //  {
    //    string name = dc.GetType().Name;
    //    if (name.Contains("Lookup"))
    //    {
    //      this.Enable = false;
    //    }
    //    else
    //    {
    //      this.Enable = val;
    //    }
    //  }
    //  else
    //  {
    //    this.Enable = val;
    //  }
    //  return this;
    //}
    public ParameterRow SetEnable(bool val)
    {
      Enable = val;
      return this;
    }
    public ParameterRow SetFlexible(bool val)
    {
      Flexible = val;
      return this;
    }
    public ParameterRow SetURL(string url)
    {
      URL = url;
      return this;
    }
    public ParameterRow SetHint(string hint)
    {
      Hint = hint;
      return this;
    }
    public ParameterRow SetValue(object val)
    {
      Value = val;
      return this;
    }
    public ParameterRow SetLength(int length)
    {
      Length = length;
      return this;
    }
    public ParameterRow SetCanBeSelectedLevel(string level)
    {
      SelectionLevel = level;
      return this;
    }
    public ParameterRow SetLength(IDataControl dc, string cname)
    {
      Length = -1;
      Length = ParameterRow.GetLengthColumn((BaseBO)dc, cname);
      return this;
    }
    public static int GetLength(IDataControl dc, string cname)
    {
      return ParameterRow.GetLengthColumn((BaseBO)dc, cname);
    }
    #endregion
  }
  #endregion
  #region ParameterRowTextBox
  public class ParameterRowPassword : ParameterRowTextBox
  {
    public ParameterRowPassword(IDataControl dc, String namelabel) :
      base(dc, namelabel, true, 100, null)
    {
      Regex = "*";
    }
  }
  public class ParameterRowTextBox : ParameterRow
  {
    public ParameterRowTextBox(IDataControl dc, String namelabel) :
      this(dc, namelabel, true, 100, null)
    {
    }
    public ParameterRowTextBox(IDataControl dc, String namelabel, bool editable, int width) :
      this(dc, namelabel, editable, width, null)
    {
    }
    public ParameterRowTextBox(IDataControl dc, String namelabel, bool editable, int width, string regex)
    {

      string[] temps = namelabel.Split(new string[] { "=" }, StringSplitOptions.None);
      Name = temps[0];
      if (temps.Length > 1)
      {
        string[] headers = temps[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        int lang = ConstantDict.IDLOCALE;
        Label = (headers.Length > lang) ? headers[lang] : headers[0];
      }
      else
      {
        Label = Name;
      }
      if (regex != null)
      {
        Regex = regex;
      }
      Hint = ((ConstantDict.IDLOCALE == ConstantDict.EN) ? "Fill data " : "Isikan data ") + Label + "";
      Width = width;
      if (width == 0)
      {
        Width = 95;
      }
      Value = dc.GetValue(Name);
      Editable = editable;
      Visible = true;
      ModeEdit = ParameterRow.MODE_ENTRY;
      AllowEmpty = true;

      try
      {
        Length = -1;
        string objname = (string)dc.GetValue("XMLName");
        Length = ParameterRow.GetLengthColumn((BaseBO)dc, Name);
      }
      catch (Exception ex)
      {
        UtilityBO.Log(dc, ex);
        Length = -1;
      }

    }
  }
  #endregion
  #region ParameterRowNumeric
  public class ParameterRowNumeric : ParameterRow
  {
    #region Constructor
    public ParameterRowNumeric(IDataControl dc, String namelabel, bool enable, int width)
      : this(dc, namelabel, width)
    {
      Enable = enable;
    }
    public ParameterRowNumeric(IDataControl dc, String namelabel, int width)
    {

      string[] temps = namelabel.Split(new string[] { "=" }, StringSplitOptions.None);
      Name = temps[0];
      Label = Name;
      if (temps.Length > 1)
      {
        string[] headers = temps[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        int lang = ConstantDict.IDLOCALE;
        Label = (headers.Length > lang) ? headers[lang] : headers[0];
      }
      Hint = ((ConstantDict.IDLOCALE == ConstantDict.EN) ? "Fill data numeric " : "Isikan data numerik ") + Label + "";
      Value = dc.GetValue(Name);
      Visible = true;
      Width = width;
      if (width == 0)
      {
        Width = 95;
      }
      ModeEdit = ParameterRow.MODE_NUMERIC;
      Length = 20;
    }
    #endregion
  }

  public class ParameterRowProgress : ParameterRowNumeric
  {
    public ParameterRowProgress(IDataControl dc, String namelabel, int width) :
      base(dc, namelabel, width)
    {
      MinValue = 0;
      MinValue = 100;
    }
  }
  #endregion
  #region ParameterRowDate
  public class ParameterRowDate : ParameterRow
  {
    public ParameterRowDate(IDataControl dc, int modedate)
    {
      if (modedate == ParameterRow.MODE_DATE)
      {
        Name = "Tgl1";
      }
      else
      {
        Name = "Tgl1,Tgl2";
      }
      Label = ConstantDict.Translate(GlobalAsp.LBL_DATE);
      Hint = "Klik tombol untuk memilih " + Label + "";
      Value = dc.GetValue("Tgl1");
      Regex = "dd/MM/yyyy";
      Visible = true;
      ModeEdit = modedate;
      Width = 30;
    }
    public ParameterRowDate(String namelabel, IDataControl dc)
    {
      string[] temps = namelabel.Split(new string[] { "=" }, StringSplitOptions.None);
      Name = temps[0];
      Label = Name;
      if (temps.Length > 1)
      {
        string[] headers = temps[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        int lang = ConstantDict.IDLOCALE;
        Label = (headers.Length > lang) ? headers[lang] : headers[0];
      }
      Hint = "Klik tombol untuk memilih " + Label + "";
      Value = dc.GetValue(Name);
      Regex = "dd/MM/yyyy";
      Visible = true;
      ModeEdit = ParameterRow.MODE_DATE;
      Width = 30;
    }
    public ParameterRowDate(IDataControl dc, String namelabel, bool editable) :
      this(namelabel, dc)
    {
      Editable = editable;
    }
    public ParameterRowDate(String namelabel, IDataControl dc, bool editable) :
      this(namelabel, dc)
    {
      Editable = editable;
    }
    public ParameterRowDate(String namelabel, IDataControl dc, bool editable, string format) :
      this(namelabel, dc, editable)
    {
      Regex = format;
    }
  }
  #endregion
  #region ParameterRowTime
  public class ParameterRowTime : ParameterRow
  {
    public ParameterRowTime(String namelabel, IDataControl dc, bool editable, string format)
    {
      string[] temps = namelabel.Split(new string[] { "=" }, StringSplitOptions.None);
      Name = temps[0];
      Label = Name;
      if (temps.Length > 1)
      {
        string[] headers = temps[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        int lang = ConstantDict.IDLOCALE;
        Label = (headers.Length > lang) ? headers[lang] : headers[0];
      }
      Hint = "Klik tombol untuk memilih " + Label + "";
      Value = dc.GetValue(Name);
      Visible = true;
      ModeEdit = ParameterRow.MODE_TIME;
      Width = 30;
      Editable = editable;
      Regex = format;
    }
  }
  #endregion
  #region ParameterRowType
  public class ParameterRowType : ParameterRow
  {
    public ParameterRowType(IDataControl dc, bool editable)
      : this(ConstantDict.GetColumnTitle("Type"), dc, editable)
    {

    }
    public ParameterRowType(String namelabel, IDataControl dc, bool editable)
    {
      string[] temps = namelabel.Split(new string[] { "=" }, StringSplitOptions.None);
      Name = temps[0];
      Label = Name;
      if (temps.Length > 1)
      {
        string[] headers = temps[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        int lang = ConstantDict.IDLOCALE;
        Label = (headers.Length > lang) ? headers[lang] : headers[0];
      }
      Hint = (ConstantDict.IDLOCALE == ConstantDict.EN) ? ("Check " + Label + " if true ") : ("Pilih header atau detil");
      Value = dc.GetValue(Name);
      Visible = true;
      Editable = editable;
      ModeEdit = ParameterRow.MODE_TYPE;
      Width = 30;
    }
  }
  #endregion
  #region ParameterRowCek
  public class ParameterRowCek : ParameterRow
  {
    public ParameterRowCek(String namelabel, IDataControl dc)
    {
      string[] temps = namelabel.Split(new string[] { "=" }, StringSplitOptions.None);
      Name = temps[0];
      Label = Name;
      if (temps.Length > 1)
      {
        Label = temps[1];
      }
      Hint = (ConstantDict.IDLOCALE == ConstantDict.EN) ? ("Check " + Label + " if true ") : ("Isikan data " + Label + ", beri tanda ceklist jika true!");
      Value = dc.GetValue(Name);
      Visible = true;
      ModeEdit = ParameterRow.MODE_CHECK;
      Width = 30;
    }
    public ParameterRowCek(IDataControl dc, bool editable)
      : this(ConstantDict.GetColumnTitle("Valid"), dc, editable)
    {

    }
    public ParameterRowCek(IDataControl dc, String namelabel, bool editable)
      : this(namelabel, dc, editable)
    {

    }
    public ParameterRowCek(String namelabel, IDataControl dc, bool editable)
    {
      string[] temps = namelabel.Split(new string[] { "=" }, StringSplitOptions.None);
      Name = temps[0];
      Label = Name;
      if (temps.Length > 1)
      {
        string[] headers = temps[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        int lang = ConstantDict.IDLOCALE;
        Label = (headers.Length > lang) ? headers[lang] : headers[0];
      }
      Hint = (ConstantDict.IDLOCALE == ConstantDict.EN) ? ("Check " + Label + " if true ") : ("Isikan data " + Label + ", beri tanda ceklist jika true!");
      Value = dc.GetValue(Name);
      Visible = true;
      Editable = editable;
      ModeEdit = ParameterRow.MODE_CHECK;
      Width = 30;
    }
  }
  #endregion
  #region ParameterRowSelect
  public class ParameterRowSelect : ParameterRow
  {
    #region Constructor
    public ParameterRowSelect(string namelabel, IList listData, string keyvalue, int width) :
      base(namelabel, ParameterRow.MODE_SELECT, listData, keyvalue, width)
    {

    }
    #endregion
  }
  #endregion
  #region ParameterRowMemo
  public class ParameterRowMemo : ParameterRow
  {
    #region Constructor
    public ParameterRowMemo(IDataControl dc, String namelabel, int width)
      : this(dc, namelabel, true, width)
    {
    }
    public ParameterRowMemo(IDataControl dc, String namelabel, bool editable)
    {

      string[] temps = namelabel.Split(new string[] { "=" }, StringSplitOptions.None);
      Name = temps[0];
      Label = Name;
      if (temps.Length > 1)
      {
        string[] headers = temps[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        int lang = ConstantDict.IDLOCALE;
        Label = (headers.Length > lang) ? headers[lang] : headers[0];
      }
      else
      {
        Label = Name;
      }
      Hint = ((ConstantDict.IDLOCALE == ConstantDict.EN) ? "Fill data " : "Isikan data ") + Label + "";
      Width = 95;
      Value = dc.GetValue(Name);
      Editable = editable;
      Visible = true;
      ModeEdit = ParameterRow.MODE_MEMO;

      try
      {
        Length = -1;
        string objname = (string)dc.GetValue("XMLName");
        Length = ParameterRow.GetLengthColumn((BaseBO)dc, Name);
      }
      catch (Exception ex)
      {
        UtilityBO.Log(dc, ex);
        Length = -1;
      }
    }
    public ParameterRowMemo(IDataControl dc, String namelabel, bool editable, int row)
    {

      string[] temps = namelabel.Split(new string[] { "=" }, StringSplitOptions.None);
      Name = temps[0];
      if (temps.Length > 1)
      {
        string[] headers = temps[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        int lang = ConstantDict.IDLOCALE;
        Label = (headers.Length > lang) ? headers[lang] : headers[0];
      }
      else
      {
        Label = Name;
      }
      Hint = ((ConstantDict.IDLOCALE == ConstantDict.EN) ? "Fill data " : "Isikan data ") + Label + "";
      if (row != 0)
      {
        Row = row;
      }
      else
      {
        Row = 2;
      }
      Width = 95;
      Value = dc.GetValue(Name);
      Editable = editable;
      Visible = true;
      ModeEdit = ParameterRow.MODE_MEMO;
      try
      {
        Length = -1;
        string objname = (string)dc.GetValue("XMLName");
        Length = ParameterRow.GetLengthColumn((BaseBO)dc, Name);
      }
      catch (Exception ex)
      {
        UtilityBO.Log(dc, ex);
        Length = -1;
      }
    }
    #endregion
  }
  #endregion
  #region ParameterRowCek
  public class ParameterRowUploadFile : ParameterRow
  {
    public ParameterRowUploadFile(IDataControl dc, bool editable)
      : this(ConstantDict.GetColumnTitle("Path"), dc, editable)
    {
    }
    public ParameterRowUploadFile(String namelabel, IDataControl dc, bool editable)
    {
      string[] temps = namelabel.Split(new string[] { "=" }, StringSplitOptions.None);
      Name = temps[0];
      Label = Name;
      if (temps.Length > 1)
      {
        Label = temps[1];
      }
      Hint = (ConstantDict.IDLOCALE == ConstantDict.EN) ? ("Check " + Label + " if true ") : ("Isikan data " + Label + ", beri tanda ceklist jika true!");
      Value = dc.GetValue(Name);
      Visible = true;
      Editable = editable;
      ModeEdit = ParameterRow.MODE_FILEUPLOAD;
      Width = 100;
    }
  }
  #endregion
  #region ParameterRowCreBy
  public class ParameterRowCreBy : ParameterRowTextBox2
  {
    public ParameterRowCreBy(IDataControl dc) : base(dc, new string[] { "Cre_by", "Cre_date" }) { }
  }
  public class ParameterRowModBy : ParameterRowTextBox2
  {
    public ParameterRowModBy(IDataControl dc) : base(dc, new string[] { "Mod_by", "Mod_date" }) { }
  }
  public class ParameterRowRevBy : ParameterRowTextBox2
  {
    public ParameterRowRevBy(IDataControl dc) : base(dc, new string[] { "Rev_by", "Rev_date" }) { }
  }
  public class ParameterRowAppBy : ParameterRowTextBox2
  {
    public ParameterRowAppBy(IDataControl dc) : base(dc, new string[] { "App_by", "App_date" }) { }
  }
  public class ParameterRowSignBy : ParameterRowTextBox2
  {
    public ParameterRowSignBy(IDataControl dc) : base(dc, new string[] { "Sign_by", "Sign_date" }) { }
  }
  public class ParameterRowLastBy : ParameterRowTextBox2
  {
    public ParameterRowLastBy(IDataControl dc) : base(dc, new string[] { "Last_by", "Last_date" }) { }
  }
  #endregion
  #region ParameterRowTextBox2
  public class ParameterRowTextBox2 : ParameterRow
  {
    #region Constructor
    public ParameterRowTextBox2(IDataControl dc, String[] namelabels)
      : this(dc, namelabels, new int[] { 45, 30 }, new bool[] { false, false })
    {
    }
    public ParameterRowTextBox2(IDataControl dc,
      String[] namelabels, int[] widths, bool[] editables)
    {
      if ((namelabels.Length == 2) && (widths.Length < 2))
      {
        throw new Exception("Lebar array parameter widths kurang dari 2!");
      }
      if ((namelabels.Length > 0) && (widths.Length > 0) && (editables.Length > 0))
      {

        string[] temps = namelabels[0].Split(new string[] { "=" }, StringSplitOptions.None);
        Name = temps[0];
        Label = Name;
        if (temps.Length > 1)
        {
          string[] headers = temps[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
          int lang = ConstantDict.IDLOCALE;
          Label = (headers.Length > lang) ? headers[lang] : headers[0];
        }
        else
        {
          Label = Name;
        }
        Width = widths[0];
        Value = dc.GetValue(Name);
        Editable = editables[0];
        Enable = editables[0];
        Visible = true;
        ModeEdit = ParameterRow.MODE_ENTRY2;

        if ((namelabels.Length > 1) && (widths.Length > 1) && (editables.Length > 1))
        {
          ParameterRow par = new ParameterRow();
          string[] temps2 = namelabels[1].Split(new string[] { "=" }, StringSplitOptions.None);

          par.Name = temps2[0];
          par.Width = widths[1];
          if (temps2.Length > 1)
          {
            string[] headers = temps2[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int lang = ConstantDict.IDLOCALE;
            par.Label = (headers.Length > lang) ? headers[lang] : headers[0];
          }
          else
          {
            par.Label = par.Name;
          }
          par.Value = dc.GetValue(par.Name);
          par.Editable = editables[1];
          par.Enable = editables[1];
          Param2 = par;
          ModeEdit = ParameterRow.MODE_ENTRY2;

          //par = new ParameterRow();
          //Param3 = par;
          //Param3.Width = widths[2];
          Name = temps[0] + "," + temps2[0];

          if (Editable && Enable)
          {
            Length = -1;
            Length2 = -1;
            string objname = (string)dc.GetValue("XMLName");
            Length = ParameterRow.GetLengthColumn((BaseBO)dc, temps[0]);
            Length2 = ParameterRow.GetLengthColumn((BaseBO)dc, temps2[0]);
          }
        }
      }
      else
      {
        throw new Exception("Lebar array parameter tidak valid!");
      }

    }
    #endregion
  }
  #endregion
  #region ParameterRowLookup2
  public class ParameterRowLookup2 : ParameterRow
  {
    #region Constructor
    public ParameterRowLookup2(IDataControl dc,
      String[] names, String[] targetfields, int[] widths, bool[] viscontrols,
      String label, String statement)
    {

      Name = names[0];
      TargetLookupFields = targetfields;
      Width = widths[0];
      VisibleControls = viscontrols;
      Label = label;
      //Value = dc.GetValue(Name);
      //Editable = false;
      Visible = true;
      ModeEdit = ParameterRow.MODE_LOOKUP2;
      KeyValue = statement;

      if (names.Length == 1)
      {
        return;
      }

      Name = names[0] + "," + names[1];
      ParameterRow par = new ParameterRow
      {
        Name = names[1],
        Width = widths[1],
        Label = label,
        //par.Value = dc.GetValue(par.Name);
        //par.Editable = false;
        Visible = true,
        ModeEdit = ParameterRow.MODE_LOOKUP2,
        KeyValue = statement
      };
      Param2 = par;

      if (names.Length == 2)
      {
        return;
      }
      Name = names[0] + "," + names[1] + "," + names[2];
      par = new ParameterRow
      {
        Name = names[2]
      };
      //par.Value = dc.GetValue(par.Name);
      Param3 = par;

      try
      {
        if (names.Length == 3)
        {
          return;
        }
        Name = names[0] + "," + names[1] + "," + names[2] + "," + names[3];
        par = new ParameterRow();
        Param4 = par;
        par.Name = names[3];
        //par.Value = dc.GetValue(par.Name);

      }
      catch (Exception ex)
      {
        UtilityBO.Log(dc, ex);
      }
    }

    public ParameterRowLookup2(IDataControl callerdc, String[] names) :
      this(callerdc, names, new string[] { names[0] + "=" + names[0], names[1] + "=" + names[1], names[2] + "=" + names[2] },
      ParameterRow.WIN_DIALOG_SIZE, null, string.Empty, String.Empty)
    {
    }

    public ParameterRowLookup2(IDataControl callerdc, String[] names, int[] widths) :
      this(callerdc, names, new string[] { names[0] + "=" + names[0], names[1] + "=" + names[1], names[2] + "=" + names[2] },
  widths, null, string.Empty, String.Empty)
    {
    }

    public ParameterRowLookup2(IDataControl callerdc, String[] names, String[] targets) :
      this(callerdc, names, targets,
       ParameterRow.WIN_DIALOG_SIZE, null, string.Empty, String.Empty)
    {
    }

    public ParameterRowLookup2(IDataControl callerdc, String[] names, int[] widths, String[] targets) :
      this(callerdc, names, targets, widths, null, string.Empty, String.Empty)
    {
    }

    private ParameterRowLookup2(IDataControl dc,
      String[] names, int[] widths, bool[] viscontrols,
      String label, String statement)
      : this(dc, names, new string[] { names[0] + "=" + names[0], names[1] + "=" + names[1], names[2] + "=" + names[2] },
      widths, viscontrols, label, statement)
    {
    }
    #endregion
  }
  #endregion
  #region ParameterRowHtml
  public class ParameterRowHtml : ParameterRow
  {
    #region Constructor
    public ParameterRowHtml(IDataControl dc, String namelabel, bool editable)
      : this(dc, namelabel, editable, 0)
    {

    }
    public ParameterRowHtml(IDataControl dc, String namelabel, bool editable, int row)
    {

      string[] temps = namelabel.Split(new string[] { "=" }, StringSplitOptions.None);
      Name = temps[0];
      if (temps.Length > 1)
      {
        string[] headers = temps[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        int lang = ConstantDict.IDLOCALE;
        Label = (headers.Length > lang) ? headers[lang] : headers[0];
      }
      else
      {
        Label = Name;
      }
      Hint = ((ConstantDict.IDLOCALE == ConstantDict.EN) ? "Fill data " : "Isikan data ") + Label + "";
      if (row != 0)
      {
        Row = row;
      }
      else
      {
        Row = 2;
      }
      Width = 95;
      Value = dc.GetValue(Name);
      Editable = editable;
      Visible = true;
      ModeEdit = ParameterRow.MODE_HTML;
      if (Editable && Enable)
      {
        try
        {
          Length = -1;
          string objname = (string)dc.GetProperty("XMLName").GetValue(dc, null);
          Length = ParameterRow.GetLengthColumn((BaseBO)dc, Name);
        }
        catch (Exception ex)
        {
          UtilityBO.Log(dc, ex);
        }
      }
    }
    #endregion
  }
  #endregion
  #region ParameterRowHtml
  public class ParameterRowHelp : ParameterRowHtml
  {
    public ParameterRowHelp(IDataControl dc, bool editable)
      : base(dc, ConstantDict.GetColumnTitle("Help"), editable)
    {

    }
  }
  #endregion
  #region ParameterRowForum
  public class ParameterRowForum : ParameterRow
  {
    #region Constructor
    public ParameterRowForum(IDataControl dc, bool editable)
      : this(dc, ConstantDict.GetColumnTitle("Msg"), editable, 0)
    {
    }
    public ParameterRowForum(IDataControl dc, String namelabel, bool editable)
      : this(dc, namelabel, editable, 0)
    {
    }
    public ParameterRowForum(IDataControl dc, String namelabel, bool editable, int row)
    {

      string[] temps = namelabel.Split(new string[] { "=" }, StringSplitOptions.None);
      Name = temps[0];
      if (temps.Length > 1)
      {
        string[] headers = temps[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        int lang = ConstantDict.IDLOCALE;
        Label = (headers.Length > lang) ? headers[lang] : headers[0];
      }
      else
      {
        Label = Name;
      }
      Hint = ((ConstantDict.IDLOCALE == ConstantDict.EN) ? "Fill data " : "Isikan data ") + Label + "";
      if (row != 0)
      {
        Row = row;
      }
      else
      {
        Row = 2;
      }
      Width = 95;
      Value = dc.GetValue(Name);
      Editable = editable;
      Visible = true;
      ModeEdit = ParameterRow.MODE_FORUM;
      if (Editable && Enable)
      {
        try
        {
          Length = -1;
          string objname = (string)dc.GetProperty("XMLName").GetValue(dc, null);
          Length = ParameterRow.GetLengthColumn((BaseBO)dc, Name);
        }
        catch (Exception ex)
        {
          UtilityBO.Log(dc, ex);
        }
      }
    }
    #endregion
  }
  #endregion
  #region HashTableofParameterRow
  public class HashTableofParameterRow : Hashtable
  {
    private int nourut;
    public HashTableofParameterRow()
    {
      nourut = 1;
    }
    public void Add(ParameterRow par)
    {
      if (par.Visible)
      {
        par.Id = nourut++;
        Add(par.Name, par);
      }
    }
    public void Add(ParameterRow par, int id)
    {
      if (par.Visible)
      {
        par.Id = nourut++;
        Add(par.Name, par);
      }
    }
    public void Add(ParameterRow par, string value, int id)
    {
      if (par.Visible)
      {
        par.Id = nourut++;
        par.Value = value;
        Add(par.Name, par);
      }
    }
    public String[] GetAllComponentsIDKeyForSetComponent()
    {
      String[] keys = new String[Count];

      Keys.CopyTo(keys, 0);
      ArrayList IDKeys = new ArrayList();
      for (int i = 0; i < keys.Length; i++)
      {
        ParameterRow pr = (ParameterRow)this[keys[i]];
        if (pr.ModeEdit != ParameterRow.MODE_LOOKUP2)
        {
          string[] ids = pr.Name.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
          for (int j = 0; j < ids.Length; j++)
          {
            IDKeys.Add(ids[j]);
          }
        }
      }
      string[] ComponentsIDKey = new string[IDKeys.Count];
      ComponentsIDKey = (string[])IDKeys.ToArray(typeof(string));
      return ComponentsIDKey;
    }
    public String[] GetOrderKeys()
    {
      ArrayList NotEntries = new ArrayList(new string[] { "Path" });

      String[] Keys = new String[Count];
      ParameterRow[] Pars = new ParameterRow[Count];
      int[] Ids = new int[Count];
      this.Keys.CopyTo(Keys, 0);
      Values.CopyTo(Pars, 0);
      for (int i = 0; i < Pars.Length; i++)
      {
        if (!NotEntries.Contains(Pars[i].Name))
        {
          Ids[i] = Pars[i].Id;
        }
      }

      Array.Sort(Ids, Keys);
      return Keys;
    }
    public ArrayList GetEditable()
    {
      ArrayList array = new ArrayList();

      string[] keys = GetOrderKeys();
      foreach (string key in keys)
      {
        if (((ParameterRow)this[key]).Editable)
        {
          array.Add(key);
        }
      }

      return array;
    }
    public ArrayList GetReadOnlyKeys()
    {
      ArrayList array = new ArrayList();

      string[] keys = GetOrderKeys();
      foreach (string key in keys)
      {
        if (!((ParameterRow)this[key]).Editable)
        {
          array.Add(key);
        }
      }

      return array;
    }
    public ArrayList GetEnableKeys()
    {
      ArrayList array = new ArrayList();

      string[] keys = GetOrderKeys();
      foreach (string key in keys)
      {
        //if ((((ParameterRow)this[key]).Enable) && (((ParameterRow)this[key]).ModeEdit != ParameterRow.MODE_LOOKUP2))
        if (((ParameterRow)this[key]).Enable)
        {
          array.Add(key);
        }
      }

      return array;
    }
    public ArrayList GetDisableKeys()
    {
      ArrayList array = new ArrayList();

      string[] keys = GetOrderKeys();
      foreach (string key in keys)
      {
        //if ((!((ParameterRow)this[key]).Enable) || (((ParameterRow)this[key]).ModeEdit == ParameterRow.MODE_LOOKUP2))
        if (!((ParameterRow)this[key]).Enable)
        {
          array.Add(key);
          //string[] keys2 = key.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
          //foreach (string key2 in keys2)
          //{
          //  array.Add(key2);
          //}
        }
      }

      return array;
    }
    public ArrayList GetHideWhenEditKeys()
    {
      ArrayList array = new ArrayList();

      string[] keys = GetOrderKeys();
      foreach (string key in keys)
      {
        if (((ParameterRow)this[key]).HideWhenEdit)
        {
          array.Add(key);
        }
      }

      return array;
    }
    public void BindParameterRow(GridView ugrv)
    {
      ArrayList list = new ArrayList();
      String[] Keys = GetOrderKeys();
      for (int i = 0; i < Count; i++)
      {
        string key = Keys[i].ToString();
        ParameterRow cParam = (ParameterRow)this[key];
        if (cParam.Visible)
        {
          list.Add(cParam);
        }
      }

      //if (list.Count > 0)//klol dibuka masalah di lookup filter
      //{
      ugrv.DataSource = list;
      ugrv.DataBind();
      //}

    }
    public void SetEnable(bool enable)
    {
      foreach (ParameterRow dc in Values)
      {
        dc.Enable = enable;
      }
    }
  }
  #endregion
}
