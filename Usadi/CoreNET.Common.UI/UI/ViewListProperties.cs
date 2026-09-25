using System.Collections;

namespace CoreNET.Common.Base
{
  public class ViewListProperties : IProperties
  {
    //@TODO klo ngerefaktor property jadi get set, jangan lupa tambahkan di contructor
    public const int MODE_TABULAR = 11;
    public const int MODE_TREE = 12;
    public const int MODE_COMMENT = 13;
    public const int MODE_TEXT_EDITOR = 14;
    public const int MODE_FORM = 15;
    #region CreateDefaultProperties
    /// <summary>
    /// Creates the default object ViewListProperties with default value.
    /// </summary>
    /// <returns></returns>
    public static ViewListProperties CreateDefaultProperties()
    {
      ViewListProperties prop = new ViewListProperties();
      if (string.IsNullOrEmpty(GlobalAsp.GetRequestChild()))
      {
        prop.PageSize = 100;
      }
      return prop;
    }
    /// <summary>
    /// Creates the default object ViewListProperties with default value.
    /// </summary>
    public static ViewListProperties CreateDefaultProperties(IDataControlUI adapter)
    {
      ViewListProperties prop = new ViewListProperties(adapter);
      return prop;
    }
    #endregion
    /// <summary>
    /// Constructor
    /// </summary>
    public ViewListProperties()
    {
      AutoRefresh = true;
      RefreshParent = true;
      RefreshMaster = true;
      RefreshParentForEditor = true;
      //RefreshFilter dipake dimana ya, jadi biar keubah GetFilter ketika pilih data di grid klo di corenet lama
      RefreshFilter = true;
    }
    /// <summary>
    /// Constructor
    /// </summary>
    public ViewListProperties(IDataControlUI adapter) { Adapter = adapter; }
    #region Property Adapter
    private IDataControlUI _Adapter;
    /// <summary>
    /// Adapter for access controller that own this method
    /// </summary>
    public IDataControlUI Adapter
    {
      get => _Adapter;
      set => _Adapter = value;
    }
    #endregion
    #region Property TitlePage
    private string _TitlePage;
    /// <summary>
    /// Title in Page. If empty, will show TitleList
    /// </summary>
    public string TitlePage
    {
      get => _TitlePage;
      set => _TitlePage = value;
    }
    #endregion
    #region Property TitleList
    private string _TitleList;
    /// <summary>
    /// Title in Region Data
    /// </summary>
    public string TitleList
    {
      get => _TitleList;
      set => _TitleList = value;
    }
    #endregion
    #region Property TitleFilter
    private string _TitleFilter = "Pilih nilai di bawah ini ";
    /// <summary>
    /// Title in Lookup and DIalog
    /// </summary>
    public string TitleFilter
    {
      get => _TitleFilter;
      set => _TitleFilter = value;
    }
    #endregion
    #region Property Custom Delete Buton
    /*
     * Dipake ketika tombol delete dipake untuk pembatalan approval di CoreNET.Common.PM
     * */
    #region Property LblBtnDel
    private string _LblBtnDel = ConstantDict.Translate("BTN_DEL1");
    public string LblBtnDel
    {
      get => _LblBtnDel;
      set => _LblBtnDel = value;
    }
    #endregion
    #region Property MsgConfirmDelete
    private string _MsgConfirmDelete = ConstantDict.Translate("LBL_CONFIRM_DELETE");
    public string MsgConfirmDelete
    {
      get => _MsgConfirmDelete;
      set => _MsgConfirmDelete = value;
    }
    #endregion
    #endregion
    #region Property PageSize
    private int _PageSize = 15;
    /// <summary>
    /// Default Page Size in Grid Paging
    /// </summary>
    public int PageSize
    {
      get => _PageSize;
      set => _PageSize = value;
    }
    #endregion

    #region Property VisibleTreeView
    private bool _VisibleTreeView = false;
    /// <summary>
    /// Option to Show TreeView in Form Entry in PageTreeGrid
    /// </summary>
    public bool VisibleTreeView
    {
      get => _VisibleTreeView;
      set => _VisibleTreeView = value;
    }
    #endregion
    #region Property VisibleBottomBar
    //Opsi menampilkan Footer apda PageTreeGrid
    private bool _VisibleBottomBar = true;
    /// <summary>
    /// Show Total below Data Grid. Best Practice: Override SetTotal
    /// </summary>
    public bool VisibleBottomBar
    {
      get => _VisibleBottomBar;
      set => _VisibleBottomBar = value;
    }
    #endregion

    public bool CanUpdateMaster { get; set; }
    #region Property CanRedirect
    private bool _CanRedirect;
    public bool CanRedirect
    {
      get => _CanRedirect;
      set => _CanRedirect = value;
    }
    #endregion
    #region Property SortFields
    private string[] _SortFields = new string[] { };
    /// <summary>
    /// Fields for Sorting
    /// </summary>
    public string[] SortFields
    {
      get => _SortFields;
      set => _SortFields = value;
    }
    #endregion

    public const int ASC = 1;
    public const int DESC = 2;
    #region Property SortDirection
    private int _SortDirection = ASC;
    /// <summary>
    /// Sorting Direction
    /// </summary>
    public int SortDirection
    {
      get => _SortDirection;
      set => _SortDirection = value;
    }
    #endregion
    #region Property PrimaryKeys
    /*Default hiiden textfield di form entry*/
    private string[] _PrimaryKeys = new string[] { };
    /// <summary>
    /// Fields that are Primary Keys of Table in DB
    /// </summary>
    public string[] PrimaryKeys
    {
      get => _PrimaryKeys;
      set => _PrimaryKeys = value;
    }

    #endregion
    #region Property IDKey
    /*Key untuk insert ke Log.Notes*/
    private string _IDKey;
    public string IDKey
    {
      get
      {
        if (string.IsNullOrEmpty(_IDKey))
        {
          return _PrimaryKeys[_PrimaryKeys.Length - 1].Replace("Kd", "Id");
        }
        else
        {
          return _IDKey;
        }
      }
      set => _IDKey = value;
    }
    #endregion

    #region Property RefreshParentForEditor
    private bool _RefreshParentForEditor = true;
    /// <summary>
    /// Refresh Parent for Editor
    /// </summary>
    public bool RefreshParentForEditor
    {
      get => _RefreshParentForEditor;
      set => _RefreshParentForEditor = value;
    }
    #endregion
    #region Property ReadOnlyFields
    private string[] _ReadOnlyFields = new string[] { };
    /// <summary>
    /// Fields in Form Entry that has value from Filter
    /// </summary>
    public string[] ReadOnlyFields
    {
      get => _ReadOnlyFields;
      set => _ReadOnlyFields = value;
    }
    #endregion

    #region Property AllowKeepFormExpanded
    private bool _AllowKeepFormExpanded = false;
    /// <summary>
    /// Allow Form Entry To Be Always Expanded or Not
    /// </summary>
    public bool AllowKeepFormExpanded
    {
      get => _AllowKeepFormExpanded;
      set => _AllowKeepFormExpanded = value;
    }
    #endregion
    #region Property AllowMultiYear
    private bool _AllowMultiYear = true;
    /// <summary>
    /// Allow multi year to prevent validation of Select Dates
    /// </summary>
    public bool AllowMultiYear
    {
      get => _AllowMultiYear;
      set => _AllowMultiYear = value;
    }
    #endregion
    #region Property AllowMultiDelete
    private bool _AllowMultiDelete = false;
    /// <summary>
    /// Allow Multi Delete using Checkbox or Not
    /// </summary>
    public bool AllowMultiDelete
    {
      get => _AllowMultiDelete;
      set => _AllowMultiDelete = value;
    }
    #endregion

    #region Property IDProperty
    private string _IDProperty = string.Empty;
    public string IDProperty
    {
      get => _IDProperty;
      set => _IDProperty = value;
    }
    #endregion
    #region Property IsSetVisible
    private bool _IsSetVisible = false;
    public bool IsSetVisible
    {
      get => _IsSetVisible;
      set => _IsSetVisible = value;
    }
    #endregion
    #region Property ModeToolbar
    public const int MODE_TOOLBAR_MINIMALIS = 0;//00000
    public const int MODE_TOOLBAR_NORMAL = 1;//00000
    public const int MODE_TOOLBAR_PRINT = 2;//00000
    public const int MODE_TOOLBAR_NORMAL_PRINT = 3;//00000
    private int _ModeToolbar = MODE_TOOLBAR_NORMAL_PRINT;
    public int ModeToolbar
    {
      get => _ModeToolbar;
      set => _ModeToolbar = value;
    }
    #endregion
    #region Property ModeEditable
    public const int MODE_EDITABLE_READONLY = 0;//00000
    public const int MODE_EDITABLE_ADD = 1;//00001
    public const int MODE_EDITABLE_EDIT = 2;//00010
    public const int MODE_EDITABLE_ADD_EDIT = 3;//00011
    public const int MODE_EDITABLE_DEL = 4;//00100
    public const int MODE_EDITABLE_ADD_DEL = 5;//00101
    public const int MODE_EDITABLE_EDIT_DEL = 6;//00110
    public const int MODE_EDITABLE_ADD_EDIT_DEL = 7;//00111
    public const int MODE_EDITABLE_LOADCSV = 8;//01000
    public const int MODE_EDITABLE_ADD_LOADCSV = 9;//01001
    public const int MODE_EDITABLE_EDIT_LOADCSV = 10;//01010
    public const int MODE_EDITABLE_ADD_EDIT_LOADCSV = 11;//01011
    public const int MODE_EDITABLE_DEL_LOADCSV = 12;//01100
    public const int MODE_EDITABLE_ADD_DEL_LOADCSV = 13;//01101
    public const int MODE_EDITABLE_EDIT_DEL_LOADCSV = 14;//01110
    public const int MODE_EDITABLE_ADD_EDIT_DEL_LOADCSV = 15;//01111

    private int _ModeEditable = MODE_EDITABLE_ADD_EDIT_DEL;
    public int ModeEditable
    {
      get => _ModeEditable;
      set => _ModeEditable = value;
    }
    public bool IsToolbarPrint()
    {
      return (ModeToolbar & MODE_TOOLBAR_PRINT) == MODE_TOOLBAR_PRINT;
    }
    public bool IsToolbarNormal()
    {
      return (ModeToolbar & MODE_TOOLBAR_NORMAL) == MODE_TOOLBAR_NORMAL;
    }
    public bool IsModeAdd()
    {
      return (ModeEditable & MODE_EDITABLE_ADD) == MODE_EDITABLE_ADD;
    }
    public static bool IsModeAdd(int ModeEditable)
    {
      return (ModeEditable & MODE_EDITABLE_ADD) == MODE_EDITABLE_ADD;
    }
    public static bool IsModeEdit(int ModeEditable)
    {
      return (ModeEditable & MODE_EDITABLE_EDIT) == MODE_EDITABLE_EDIT;
    }
    public bool IsModeEdit()
    {
      return (ModeEditable & MODE_EDITABLE_EDIT) == MODE_EDITABLE_EDIT;
    }
    public bool IsModeDelete()
    {
      return (ModeEditable & MODE_EDITABLE_DEL) == MODE_EDITABLE_DEL;
    }
    public bool IsLoadCSV()
    {
      return (ModeEditable & MODE_EDITABLE_LOADCSV) == MODE_EDITABLE_LOADCSV;
    }
    #endregion
    #region Property ForceInsert
    /// <summary>
    /// Insert tanpa popup dialog, force to call Insert Methos
    /// </summary>
    private bool _ForceInsert = false;
    /// <summary>
    /// Force to Insert without Validate. BestParactice: Override Insert.
    /// </summary>
    public bool ForceInsert
    {
      get => _ForceInsert;
      set => _ForceInsert = value;
    }
    #endregion
    #region Property ForceEdit
    /// <summary>
    /// Insert tanpa popup dialog, force to call Insert Methos
    /// </summary>
    private bool _ForceEdit = false;
    /// <summary>
    /// Force to Edit without Validate. BestParactice: Override Edit
    /// </summary>
    public bool ForceEdit
    {
      get => _ForceEdit;
      set => _ForceEdit = value;
    }
    #endregion
    #region Property ForceDelete
    /// <summary>
    /// Insert tanpa popup dialog, force to call Insert Methos
    /// </summary>
    private bool _ForceDelete = false;
    /// <summary>
    /// Force to Delete without Validate. BestParactice: Override Delete.
    /// </summary>
    public bool ForceDelete
    {
      get => _ForceDelete;
      set => _ForceDelete = value;
    }
    #endregion
    #region Property RefreshFilter
    private bool _RefreshFilter = false;
    /// <summary>
    /// Refresh Filter while Refresh Data in Grid or TreeGrid
    /// </summary>
    public bool RefreshFilter
    {
      get => _RefreshFilter;
      set => _RefreshFilter = value;
    }
    #endregion
    #region Property EntryStyle
    public const int ENTRY_STYLE_FORM = 1;//FormEntry sama untuk Add dan Edit
    public const int ENTRY_STYLE_LOOKUP = 2;//GridLookup
    public const int ENTRY_STYLE_LOOKUP_ENTRY = 3;//PageTabular bisa di Edit

    public const int ENTRY_STYLE_TREE = 11;//TreeLookup
    public const int ENTRY_STYLE_TREE_ENTRY = 12;//PageTreeGrid bisa di Edit

    public const int ENTRY_STYLE_LOOKUP_FORM = 21;//FormEntry untuk SetPack (Add dan Edit beda)


    private int _EntryStyle = ENTRY_STYLE_FORM;
    /// <summary>
    /// Mode Entry
    /// </summary>
    public int EntryStyle
    {
      get => _EntryStyle;
      set => _EntryStyle = value;
    }
    #endregion
    #region Fitur Entry Style Form
    #region Property FormGroupLabels
    private string[] _FormGroupLabels = new string[] { };
    public string[] FormGroupLabels
    {
      get => _FormGroupLabels;
      set => _FormGroupLabels = value;
    }
    #endregion
    #endregion
    #region Fitur Entry Style Table Lookup
    #region Property LookupLabelQuery
    private string _LookupLabelQuery = string.Empty;
    public string LookupLabelQuery
    {
      get => _LookupLabelQuery;
      set => _LookupLabelQuery = value;
    }
    #endregion

    #region Property DetilLookupOnly
    private bool _DetilLookupOnly = false;
    /// <summary>
    /// TreeLookup:Option when Only Detail Type Could Be Inserted, Not Header
    /// </summary>
    public bool DetilLookupOnly
    {
      get => _DetilLookupOnly;
      set => _DetilLookupOnly = value;
    }
    #endregion

    public string LookupDC { get; set; }
    #region Property LookupField
    private string _LookupField = null;
    public string LookupField
    {
      get
      {
        if (_LookupField == null)
        {
          return _PrimaryKeys[0];
        }
        else
        {
          return _LookupField;
        }
      }
      set => _LookupField = value;
    }
    #endregion
    #endregion

    #region Property AutoRefresh
    private bool _AutoRefresh = true;
    /// <summary>
    /// Allow refresh after Saving Editing Cell or Not
    /// </summary>
    public bool AutoRefresh
    {
      get => _AutoRefresh;
      set => _AutoRefresh = value;
    }
    #endregion
    #region Property RefreshMaster
    private bool _RefreshMaster = true;
    /// <summary>
    /// Refresh Master Page in PageMasterDetil
    /// </summary>
    public bool RefreshMaster
    {
      get => _RefreshMaster;
      set => _RefreshMaster = value;
    }
    #endregion
    #region Property CanUpdateDetilState
    private bool _CanUpdateDetilState = false;
    public bool CanUpdateDetilState
    {
      get => _CanUpdateDetilState;
      set => _CanUpdateDetilState = value;
    }
    #endregion
    #region Property RefreshParent
    private bool _RefreshParent = true;//defaultnya true
                                       /// <summary>
                                       /// Refresh Parent after Save Detil. Execute parent.refreshDataWithSelection();
                                       /// </summary>
    public bool RefreshParent
    {
      get => _RefreshParent;
      set => _RefreshParent = value;
    }
    #endregion

    #region Fitur Export
    #region Property CanExport
    private bool _CanExport = false;
    public bool CanExport
    {
      get => _CanExport;
      set => _CanExport = value;
    }
    #endregion
    #region Property CanModeTree
    private bool _CanModeTree = true;
    public bool CanModeTree
    {
      get => _CanModeTree;
      set => _CanModeTree = value;
    }
    #endregion

    #region Property Delimiter
    private string _Delimiter = ";";
    /// <summary>
    /// UploadCSV:Delimiter for LoadCSV
    /// </summary>
    public string Delimiter
    {
      get => _Delimiter;
      set => _Delimiter = value;
    }
    #endregion
    #endregion
    #region Fitur Master-Detail
    //#region Property DefaultTabListDetil
    //private int _DefaultTabListDetil = 0;
    //public int DefaultTabListDetil
    //{
    //  get => _DefaultTabListDetil;
    //  set => _DefaultTabListDetil = value;
    //}
    //#endregion
    //#region Property ForcePageSize
    //private bool _ForcePageSize = false;
    //public bool ForcePageSize
    //{
    //  get => _ForcePageSize;
    //  set => _ForcePageSize = value;
    //}
    //#endregion
    #endregion
    #region Property HasTreeRoot
    private bool _HasTreeRoot = true;
    public bool HasTreeRoot
    {
      get => _HasTreeRoot;
      set => _HasTreeRoot = value;
    }
    #endregion
  }
}
