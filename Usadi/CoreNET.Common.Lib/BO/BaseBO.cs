using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace CoreNET.Common.Base
{
  [Serializable]
  public class BaseBO //: ISerializable//: ICloneable//, ISerializable
  {
    public static string ClientCompName = string.Empty;
    public static string ClientCompIP = string.Empty;
    public static string ClientUserID = string.Empty;
    #region Constant
    public const int RANGE_YEAR = 1;
    public const int RANGE_6MONTHS = 2;
    public const int RANGE_3MONTHS = 3;
    public const int RANGE_MONTH = 4;
    public const int RANGE_DAY = 5;

    public const int NORMAL = 0;
    public const int CAN_BE_INSERTED = 1;
    public const int CAN_BE_UPDATED = 2;
    public const int ADD_CHILD = 3;
    public const int ADD_NODE = 4;

    public const string ALL = "All";
    public const string DELETE = "Delete";
    public const string INSERT = "Insert";
    public const string NOURUT = "Nourut";
    public const string JURNAL = "Jurnal";
    public const string COMMENT = "Comment";
    public const string REPORT = "Report";
    public const string LAST = "Last";
    public const string DISTINCT = "Distinct";
    public const string UNION = "Union";
    public const string USER = "User";
    public const string LIST = "List";
    public const string STATUS = "Status";
    public const string RANGE = "Range";
    public const string TOTAL = "Total";
    public const string DEFAULT = "";
    public const string DETIL = "Detil";
    public const string TYPE = "Type";
    public const string PARAM = "Param";
    public const string FILTER = "Filter";
    public const string REG = "Reg";
    public const string LOOKUP = "Lookup";
    public const string LOOKUP_FOR_SELF = "LookupForSelf";
    public const string PARENT = "Parent";
    public const string PK = "PK";
    public const string LOGIN = "Login";
    public const string ID = "ID";
    public const string KEY = "Key";
    public const string SYSTEM_ERR_CANT_INSERT_DUPLICATE = "Cannot insert duplicate key";
    public const string SYSTEM_ERR_CHILDREN_STILL_EXIST = "Children still exist";
    public const string SYSTEM_ERR_CONVERT_NUM_TO_MONEY = "Arithmetic overflow error converting numeric to data type money";
    #endregion
    #region Property
    #region Property Debug
    public static string GetStaticDefaultDebug(BaseBO bo)
    {
      string db = SQLDataSource.GetOpDB(bo.ConnectionString);
      //                  <li>Properties={4}</li>
      string temp = string.Format(@"
                [BaseBO]<ul>
                  <li>DBName={0}</li>
                  <li>XMLName ={1}</li>
                </ul>[BaseBO]<br/>"
          ,
         db, bo.XMLName, bo.Idapp, bo.Userid, bo.GetFieldsString());
      return temp;
    }
    protected string _Debug;
    public string Debug
    {
      get
      {
        if (MasterAppConstants.Instance.StatusTesting)
        {
          return _Debug + GetStaticDefaultDebug(this);
        }
        else
        {
          return string.Empty;
        }
      }
      set => _Debug = value;
    }
    #endregion
    #region Property ConnectionString
    private string _ConnectionString;
    public string ConnectionString
    {
      get => _ConnectionString;
      set => _ConnectionString = value;
    }
    #endregion
    #region Property Idapp
    private string _Idapp;
    public string Idapp
    {
      get => _Idapp;
      set => _Idapp = value;
    }
    #endregion
    #region Property Kdapp
    private string _Kdapp;
    public string Kdapp
    {
      get => _Kdapp;
      set => _Kdapp = value;
    }
    #endregion
    #region Property Nmapp
    private string _Nmapp;
    public string Nmapp
    {
      get => _Nmapp;
      set => _Nmapp = value;
    }
    #endregion
    public string Idapp2 { get; set; }
    public string Kdapp2 { get; set; }
    public string Nmapp2 { get; set; }
    #region Property Urapp
    private string _Urapp;
    public string Urapp
    {
      get => _Urapp;
      set => _Urapp = value;
    }
    #endregion
    #region Property Iduser
    //Property Userid dipakai di GetColumns SS10USER, so dia adalah property class 
    //Userid must exclude from GetNotFields and Iduser as a replace
    private string _Iduser;
    public string Iduser
    {
      get
      {
        if (string.IsNullOrEmpty(_Iduser))
        {
          return Userid;
        }
        else
        {
          return _Iduser;
        }
      }
      set => _Iduser = value;
    }
    #endregion
    #region Property DBName
    private string _DBName;
    public string DBName
    {
      get => _DBName;
      set => _DBName = value;
    }
    #endregion
    #region Property Hal
    private int _Hal = 0;
    public int Hal
    {
      get => _Hal;
      set => _Hal = value;
    }
    #endregion
    #region Property RowCount
    private int _RowCount = 0;
    public int RowCount
    {
      get => _RowCount;
      set => _RowCount = value;
    }
    #endregion
    #region Property Row
    private int _Row = 0;
    public int Row
    {
      get => _Row;
      set => _Row = value;
    }
    #endregion
    #region Property Ngroup
    private int _Ngroup;
    public int Ngroup
    {
      get => _Ngroup;
      set => _Ngroup = value;
    }
    #endregion
    #region Property Nchild
    private int _Nchild;
    public int Nchild
    {
      get => _Nchild;
      set => _Nchild = value;
    }
    #endregion
    #region Property Nfiles
    private int _Nfiles;
    public int Nfiles
    {
      get => _Nfiles;
      set => _Nfiles = value;
    }
    #endregion
    #region Property Nmsgs
    private int _Nmsgs;
    public int Nmsgs
    {
      get => _Nmsgs;
      set => _Nmsgs = value;
    }
    #endregion
    #region Property Progress
    private decimal _Progress;
    public decimal Progress
    {
      get => _Progress;
      set => _Progress = value;
    }
    public string Progressstr
    {
      get => Progress.ToString("#,##0.00");
      //set => decimal.TryParse(value.Replace(".", "").Replace(",", "."), out _Progress);
      set => decimal.TryParse(value, out _Progress);
    }
    #endregion
    #region Property Progressicon
    public string Progressicon => GetProgressicon(Progress);
    public static string GetProgressicon(decimal progress)
    {
      if (progress == 0)
      {
        return "bulat-0.png";
      }
      else if (progress > 0 && progress <= 25)
      {
        return "bulat-25.png";
      }
      else if (progress > 25 && progress <= 50)
      {
        return "bulat-50.png";
      }
      else if (progress > 50 && progress < 100)
      {
        return "bulat-75.png";
      }
      else if (progress == 100)
      {
        return "bulat-100.png";
      }
      else
      {
        return "buletijo.png";
      }
    }
    #endregion

    #region Property Html
    private string _Html;
    public string Html
    {
      get => _Html;
      set => _Html = value;
    }
    #endregion
    #region Property Message
    private string _Message;
    public string Message
    {
      get => _Message;
      set => _Message = value;
    }
    #endregion
    #region Property ForceOtentikasi
    private bool _ForceOtentikasi;
    public bool ForceOtentikasi
    {
      get => _ForceOtentikasi;
      set => _ForceOtentikasi = value;
    }
    #endregion
    #region Property AllowSuperUser
    private bool _AllowSuperUser;
    public bool AllowSuperUser
    {
      get => _AllowSuperUser;
      set => _AllowSuperUser = value;
    }
    #endregion
    #region Property Ket
    private string _Ket;
    public string Ket
    {
      get => _Ket;
      set => _Ket = value;
    }
    #endregion
    #region Property Forum
    private string _Forum;
    public string Forum
    {
      get => _Forum;
      set => _Forum = value;
    }
    #endregion
    #region Property Sinkron
    private bool _Sinkron;
    public bool Sinkron
    {
      get => _Sinkron;
      set => _Sinkron = value;
    }
    #endregion
    #region Property Userid
    private string _Userid;
    public string Userid
    {
      get => _Userid;
      set => _Userid = value;
    }
    #endregion

    #region Property ModeDB
    private int _ModeDB = 0;
    public int ModeDB
    {
      get
      {
        if (_ModeDB <= 0)
        {
          return SQLDataSource.MODE_DB_OPERATIONAL;
        }
        else
        {
          return _ModeDB;
        }
      }
      set => _ModeDB = value;
    }
    #endregion
    #region Property XMLName
    private string _XMLName;
    public string XMLName
    {
      get => _XMLName;
      set => _XMLName = value;
    }
    #endregion

    #region Property Keys
    ////Perlu Serialize
    //private string[] _Keys = new string[] { "Key", "Field", "Value", "Type" };
    //public string[] Keys
    //{
    //  get => _Keys;
    //  set => _Keys = value;
    //}
    #endregion
    #region Property UniqueID
    private string _UniqueID;
    public string UniqueID
    {
      get => _UniqueID;
      set => _UniqueID = value;
    }
    #endregion
    #region Property Mode
    private int _ModePage;
    public int ModePage
    {
      get => _ModePage;
      set => _ModePage = value;
    }
    #endregion
    #region ModePreviewIndex
    #region Property MaxModePreviewIndex
    private int _MaxModePreviewIndex = 0;
    public int MaxModePreviewIndex
    {
      get
      {
        if (_MaxModePreviewIndex == 0)
        {
          _MaxModePreviewIndex = 2;
        }
        return _MaxModePreviewIndex;
      }
      set => _MaxModePreviewIndex = value;
    }
    #endregion
    private int _ModePreviewIndex = 0;

    public int ModePreviewIndex
    {
      get => _ModePreviewIndex;
      set => _ModePreviewIndex = value;
    }
    //user bisa ganti mode preview (query,kolom,grid/tree) runtime
    public int GetMaxModePreviewIndex()
    {
      return _MaxModePreviewIndex;
    }
    public void SetMaxModePreviewIndex(int index)
    {
      _MaxModePreviewIndex = index;
    }
    public int GetModePreviewIndex()
    {
      return ModePreviewIndex;
    }
    public void ChangeModePreviewIndex()
    {
      _ModePreviewIndex++;
      if (_ModePreviewIndex > _MaxModePreviewIndex)
      {
        _ModePreviewIndex = 0;
      }
    }
    #endregion
    #region Selected
    private bool _Selected;
    public bool Selected
    {
      get => _Selected;
      set => _Selected = value;
    }
    #endregion
    #region Adaptee
    private Object _Adaptee;
    public Object Adaptee
    {
      get => _Adaptee;
      set => _Adaptee = value;
    }
    #endregion
    #region Property No
    private string _No;
    public string No
    {
      get => string.IsNullOrEmpty(_No) ? string.Empty : _No.Trim();
      set => _No = value;
    }
    #endregion
    #region Property Tgl1
    private DateTime _Tgl1 = DateTime.Parse(DateTime.Now.ToShortDateString());
    public DateTime Tgl1
    {
      get => (_Tgl1 == new DateTime()) ? DateTime.Parse(DateTime.Now.ToShortDateString()) : _Tgl1;
      set => _Tgl1 = value;
    }
    #endregion

    public DateTime YearStartDate { get; set; }
    public DateTime YearEndDate { get; set; }

    #region Property Tgl2
    private DateTime _Tgl2 = DateTime.Parse(DateTime.Now.ToShortDateString());
    public DateTime Tgl2
    {
      get => _Tgl2;
      set => _Tgl2 = value;
    }
    #endregion
    #region Property Path
    private string _Path = string.Empty;
    public string Path
    {
      get => _Path;
      set => _Path = value;
    }
    #endregion
    #region Property Help
    private string _Help = string.Empty;
    public string Help
    {
      get => _Help;
      set => _Help = value;
    }
    #endregion
    #region Property Msg
    private string _Msg;
    public string Msg
    {
      get => _Msg;
      set => _Msg = value;
    }
    #endregion
    #region Property IsTesting
    private bool _IsTesting;
    public bool IsTesting
    {
      get => _IsTesting;
      set => _IsTesting = value;
    }
    #endregion
    #region Property Nourut
    private int _Nourut;
    public int Nourut
    {
      get => _Nourut;
      set => _Nourut = value;
    }
    #endregion
    #region Property GrandTotal
    private decimal _GrandTotal;
    public decimal GrandTotal
    {
      get => _GrandTotal;
      set => _GrandTotal = value;
    }
    #endregion
    #region Property Editable
    /*
     * Buat simpan status editable karena BtnAdd/BtnEdit di Page
     * */
    private bool _Editable = true;
    public bool Editable
    {
      get => _Editable;
      set => _Editable = value;
    }
    #endregion
    #region Property EnableFilter
    private bool _EnableFilter;
    public bool EnableFilter
    {
      get => _EnableFilter;
      set => _EnableFilter = value;
    }
    #endregion
    #region Property Valid
    private bool _Valid;
    public bool Valid
    {
      get => _Valid;
      set => _Valid = value;
    }
    #endregion

    #region Property Tahun
    private int _Tahun = DateTime.Now.Year;
    public int Tahun
    {
      get => _Tahun;
      set => _Tahun = value;
    }
    #endregion
    #region Property Bulan
    private int _Bulan = DateTime.Now.Month;
    public int Bulan
    {
      get => _Bulan;
      set => _Bulan = value;
    }
    #endregion

    public int Index { get; set; }//Current Request[i]
    public decimal Rating
    {
      get
      {
        int rating = 0;
        if (string.IsNullOrEmpty(Sign_by))
        {
          if (string.IsNullOrEmpty(App_by))
          {
            if (string.IsNullOrEmpty(Rev_by))
            {
              rating = 0;
            }
            else
            {
              rating = 1;
            }
          }
          else
          {
            rating = 2;
          }
        }
        else
        {
          rating = 3;
        }
        return rating;
      }
    }


    #region Property Status
    public int Status { get; set; }
    public string Statusicon { get; set; }
    public string Statusname { get; set; }
    #endregion
    #region Property Status
    public int Status2 { get; set; }
    public string Statusicon2 { get; set; }
    public string Statusname2 { get; set; }
    #endregion

    public string Stricon { get => Statusicon + "=" + Statusname; }
    public string Url { get; set; }
    public string UrlFull { get; set; }
    //#region Property UrlFull
    //private string _UrlFull;
    //public string UrlFull
    //{
    //  get
    //  {
    //    //Mungkin ke depannya harus direviu.Boleh dong UrlFull aja yang terisi
    //    //Melanggar prinsip dependency
    //    if (string.IsNullOrEmpty(Url))
    //    {
    //      return string.Empty;
    //    }
    //    else
    //    {
    //      if (MasterAppConstants.Instance.StatusTesting)
    //      {
    //        return _UrlFull;// + "&olist1=" + Olist1;
    //      }
    //      else
    //      {
    //        return _UrlFull;
    //      }
    //    }
    //  }
    //  set { _UrlFull = value; }
    //}
    //#endregion

    #region Property KdlevelType
    public void SetLevelType(string plevel)
    {
      if (GetProperty(plevel) != null)
      {
        string kdrole = (string)GetValue(plevel);
        if (!string.IsNullOrEmpty(kdrole))
        {
          string[] strs = kdrole.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
          Kdlevel = strs.Length;
        }
        //Type = (Kdlevel == 6) ? "D" : "H";//Bisa Beda2
      }
    }
    private int _Kdlevel = 0;
    public int Kdlevel
    {
      get => _Kdlevel;
      set => _Kdlevel = value;
    }
    public string Type { get; set; }
    #endregion

    public string App_by { get; set; }
    public DateTime App_date { get; set; }
    public string Cre_by { get; set; }
    public DateTime Cre_date { get; set; }
    public string Mod_by { get; set; }
    public DateTime Mod_date { get; set; }
    public string Rev_by { get; set; }
    public DateTime Rev_date { get; set; }
    public string Sign_by { get; set; }
    public DateTime Sign_date { get; set; }
    public string Last_by { get; set; }
    #region Property Last_date
    private DateTime _Last_date = DateTime.Now;
    public DateTime Last_date
    {
      get => (_Last_date == null) || (_Last_date == new DateTime()) ? DateTime.Now : _Last_date;
      set => _Last_date = value;
    }

    #endregion

    public string Kode { get; set; }//Default dipake di GetFilter SysgettreecolsUI.cs
    public string ParentKode(string kode)
    {
      try
      {
        int pos1 = kode.LastIndexOf(".", kode.Trim().Length - 2);
        if (pos1 < 0)
        {
          return string.Empty;
        }
        return kode.Trim().Substring(0, pos1 + 1);
      }
      catch (Exception ex)
      {
        UtilityBO.Log(ex);
        return string.Empty;
      }
    }

    #endregion
    #region Method
    public BaseBO()
    {
      Cre_date = DateTime.Now;
      Tahun = DateTime.Now.Year;
      Bulan = DateTime.Now.Month;
      Status = 0;
      Status2 = 0;
      SetDefaultDate(RANGE_DAY);
    }
    #region method IsMirror()
    public bool IsMirror(BaseBO mirror)
    {
      bool ismirror = true;
      PropertyInfo[] Props = GetType().GetProperties();
      for (int i = 0; (i < Props.Length) && (ismirror); i++)
      {
        Object value1 = Props[i].GetValue(this, null);
        Object value2 = Props[i].GetValue(mirror, null);
        if (value1 != null)
        {
          ismirror = (value1 == value2);
          if ((value1 != null) && (value2 != null) && !ismirror)
          {
            ismirror = value1.ToString().Trim().Equals(value2.ToString().Trim());
          }
        }
      }
      return ismirror;
    }
    public bool IsEqualValue(BaseBO mirror, string[] keys)
    {
      bool ismirror = true;
      PropertyInfo[] Props = GetType().GetProperties();
      for (int i = 0; (i < keys.Length) && (ismirror); i++)
      {
        Object value1 = GetType().GetProperty(keys[i]).GetValue(this, null);
        Object value2 = GetType().GetProperty(keys[i]).GetValue(mirror, null);
        if (value1 != null)
        {
          ismirror = (value1.Equals(value2));
          if ((value1 != null) && (value2 != null) && !ismirror)
          {
            ismirror = value1.ToString().Trim().Equals(value2.ToString().Trim());
          }
        }
      }
      return ismirror;
    }
    #endregion
    #region method Mirror()
    public void MirrorTo(BaseBO destobj)//destobj is destination object
    {
      Mirror(destobj);
    }
    public void Mirror(BaseBO destobj)//destobj is destination object
    {
      Mirror(destobj, true);
    }
    public void Mirror(BaseBO destobj, bool useReadOnlyFields)//destobj is destination object
    {
      PropertyInfo[] staticProps = destobj.GetType().GetProperties(BindingFlags.Static);
      ArrayList ReadOnlyProps = new ArrayList(staticProps.Length);
      for (int i = 0; i < staticProps.Length; i++)
      {
        ReadOnlyProps.Add(staticProps[i].Name);
      }
      if (useReadOnlyFields)
      {
        string[] notfields = GetNotFields();
        for (int i = 0; i < notfields.Length; i++)
        {
          ReadOnlyProps.Add(notfields[i]);
        }
      }

      //PropertyInfo[] Props = this.GetType().GetProperties();
      PropertyInfo[] Props = destobj.GetType().GetProperties();
      for (int i = 0; i < Props.Length; i++)
      {
        PropertyInfo prop = Props[i];
        if (ReadOnlyProps.IndexOf(prop.Name) < 0)
        {
          String pname = prop.Name;
          try
          {
            if (!pname.ToLower().StartsWith("link")
              && !pname.ToLower().StartsWith("view")
              && !pname.ToLower().EndsWith("str")
              && !pname.ToLower().Contains("debug")
              && !pname.ToLower().Contains("maxmodepreviewindex")//ini klo dikomen, jadi error
              && prop.CanWrite
            )
            {
              Object value = prop.GetValue(this, null);
              destobj.SetValue(pname, value);
            }
          }
          catch (Exception ex)
          {
            string meth = UtilityBO.GetCurrentMethod(1);
            string msg = string.Format("Property {0} in class {1}", pname, GetType().FullName);
            UtilityBO.Log((IDataControl)this, meth, msg, ex);
          }
        }
      }
    }
    #endregion
    #region method Clone()
    public static BaseBO Clone(IDataControl bo)
    {
      BaseBO newbo = (BaseBO)UtilityBO.Create(bo.GetType().AssemblyQualifiedName);
      if (newbo != null)
      {
        ((BaseBO)bo).Mirror(newbo, false);
      }
      return newbo;
    }
    public static BaseBO Clone2<BaseBO>(BaseBO source)
    {
      if (!typeof(BaseBO).IsSerializable)
      {
        throw new ArgumentException("The type must be serializable.", "source");
      }

      // Don't serialize a null object, simply return the default for that object
      if (Object.ReferenceEquals(source, null))
      {
        return default(BaseBO);
      }

      IFormatter formatter = new BinaryFormatter();
      Stream stream = new MemoryStream();
      using (stream)
      {
        formatter.Serialize(stream, source);
        stream.Seek(0, SeekOrigin.Begin);
        return (BaseBO)formatter.Deserialize(stream);
      }
    }
    /// <summary>
    /// Clone the object, and returning a reference to a cloned object.
    /// </summary>
    /// <returns>Reference to the new cloned 
    /// object.</returns>
    public object Clone1()
    {
      //First we create an instance of this specific type.
      object newObject = Activator.CreateInstance(GetType());

      //We get the array of fields for the new type instance.
      PropertyInfo[] fields = newObject.GetType().GetProperties();

      //int i = 0;

      //foreach (PropertyInfo fi in this.GetType().GetProperties())
      for (int i = 0; i < fields.Length; i++)
      {
        string pname = fields[i].Name;
        PropertyInfo fi = GetType().GetProperty(pname);
        if (fi.CanWrite)
        {
          //We query if the fiels support the ICloneable interface.
          Type ICloneType = fi.PropertyType.
                      GetInterface("ICloneable", true);

          if (ICloneType != null)
          {
            //Getting the ICloneable interface from the object.
            ICloneable IClone = (ICloneable)fi.GetValue(this, null);

            //We use the clone method to set the new value to the field.
            if (IClone != null)
            {
              if (IClone.GetType() == GetType())
              {

              }
              else
              {
                try
                {
                  fields[i].SetValue(newObject, IClone.Clone(), null);
                }
                catch (Exception ex)
                {
                  throw new Exception("Error on " + fields[i] + " -> " + ex.Message);
                }
              }
            }
          }
          else
          {
            // If the field doesn't support the ICloneable 
            // interface then just set it.
            fields[i].SetValue(newObject, fi.GetValue(this, null), null);
          }

          //Now we check if the object support the 
          //IEnumerable interface, so if it does
          //we need to enumerate all its items and check if 
          //they support the ICloneable interface.
          Type IEnumerableType = fi.PropertyType.GetInterface
                          ("IEnumerable", true);
          if (IEnumerableType != null)
          {
            //Get the IEnumerable interface from the field.
            IEnumerable IEnum = (IEnumerable)fi.GetValue(this, null);

            //This version support the IList and the 
            //IDictionary interfaces to iterate on collections.
            Type IListType = fields[i].PropertyType.GetInterface
                                ("IList", true);
            Type IDicType = fields[i].PropertyType.GetInterface
                                ("IDictionary", true);

            int j = 0;
            if (IListType != null)
            {
              //Getting the IList interface.
              IList list = (IList)fields[i].GetValue(newObject, null);

              foreach (object obj in IEnum)
              {
                //Checking to see if the current item 
                //support the ICloneable interface.
                ICloneType = obj.GetType().
                    GetInterface("ICloneable", true);

                if (ICloneType != null)
                {
                  //If it does support the ICloneable interface, 
                  //we use it to set the clone of
                  //the object in the list.
                  ICloneable clone = (ICloneable)obj;

                  list[j] = clone.Clone();
                }

                //NOTE: If the item in the list is not 
                //support the ICloneable interface then in the 
                //cloned list this item will be the same 
                //item as in the original list
                //(as long as this type is a reference type).

                j++;
              }
            }
            else if (IDicType != null)
            {
              //Getting the dictionary interface.
              IDictionary dic = (IDictionary)fields[i].
                                  GetValue(newObject, null);
              j = 0;
              if (IEnum != null)
              {
                foreach (DictionaryEntry de in IEnum)
                {
                  //Checking to see if the item 
                  //support the ICloneable interface.
                  ICloneType = de.Value.GetType().
                      GetInterface("ICloneable", true);

                  if (ICloneType != null)
                  {
                    ICloneable clone = (ICloneable)de.Value;

                    dic[de.Key] = clone.Clone();
                  }
                  j++;
                }
              }
            }
          }
        }
      }
      return newObject;
    }
    public PropertyInfo GetProperty(string pname)/*Boleh dipake untuk pengecekan ada tidak property, jd jgn ada exception */
    {
      PropertyInfo prop = GetType().GetProperty(pname);
      return prop;//Kalau ngga ada, return null
    }
    public void SetValue(string pname, Object val)
    {
      PropertyInfo prop = GetProperty(pname);
      if (prop == null)
      {
        throw new Exception("Tidak ada property " + pname + " pada class " + GetType().Name + " atau related objectnya!");
      }
      else
      {
        UtilityBO.SetValueForProperty(this, pname, val);
        //prop.SetValue(this, val, null);
      }
    }
    public Object GetValue(string pname)
    {
      PropertyInfo prop = GetProperty(pname);
      if (prop == null)
      {
        throw new Exception("Tidak ada property " + pname + " pada class " + GetType().Name + " atau related objectnya!");
      }
      else
      {
        return prop.GetValue(this, null);
      }
    }
    #endregion
    public String[] GetNotFields()
    {
      string[] notfields = new string[] {
        "Adaptee",
        "ConnectionString",
        "MaxModePreviewIndex",
        "ModePreviewIndex",
        "ModeDB",
        "ModePage",
        "DBName",
        "XMLName",
        "Keys",
        "AllowSuperUser",
        "ForceOtentikasi",
        "Message",
        "Row",
        "RowCount",
        "Sinkron",
        "Hal",
        "Debug"
      };
      return notfields;
    }
    public String[] GetFields(ArrayList arListNotFields)
    {
      PropertyInfo[] Props = GetType().GetProperties();
      ArrayList arList = new ArrayList();
      for (int i = 0; i < Props.Length; i++)
      {
        string pname = Props[i].Name;
        if (!arListNotFields.Contains(pname))
        {
          arList.Add(pname);
        }
      }
      string[] fields = new string[arList.Count];
      arList.CopyTo(fields);
      return fields;
    }
    public string GetFieldsString()
    {
      ArrayList arListNotFields = new ArrayList(new string[] { "UniqueID", "Path", "Help", "Msg" });

      string[] strs = GetFields();
      string str = string.Empty;
      for (int i = 0; i < strs.Length; i++)
      {
        string tmp = strs[i].ToLower();
        if (!tmp.Contains("date") && !tmp.Contains("by") && !arListNotFields.Contains("by") && !arListNotFields.Contains(tmp))
        {
          str += strs[i] + ",";
        }
      }
      return str;
    }
    public String[] GetFields()
    {
      ArrayList arListNotFields = new ArrayList(GetNotFields());
      return GetFields(arListNotFields);
    }
    //public static IDataControl Create(string fullclassname)
    //{
    //  IDataControl obj = null;
    //  try
    //  {
    //    if (!string.IsNullOrEmpty(fullclassname))
    //    {
    //      Type T = Type.GetType(fullclassname);
    //      obj = Activator.CreateInstance(T) as IDataControl;
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    throw new Exception(ex.Message + " on classname=" + fullclassname);
    //  }
    //  return obj;
    //}
    public static Object Create(string className)
    {
      //Get the current assembly object
      Assembly assembly = Assembly.GetExecutingAssembly();

      //Get the name of the assembly (this will include the public token and version number
      AssemblyName assemblyName = assembly.GetName();

      //Use just the name concat to the class chosen to get the type of the object
      Type t = assembly.GetType(assemblyName.Name + "." + className);

      //Create the object, cast it and return it to the caller
      return Activator.CreateInstance(t);
    }
    #endregion

    #region Refactor From BaseDataControl
    public void SetDefaultDate(int mode)
    {
      SetDefaultDate(DateTime.Now.Year, DateTime.Now.Month, mode);
    }
    public void SetDefaultDate(int year, int month, int mode)
    {
      DateTime tgl1;
      DateTime tgl2;
      UtilityBO.SetDate(year, month, mode, out tgl1, out tgl2);
      Tgl1 = tgl1;
      Tgl2 = tgl2;
      YearStartDate = new DateTime(year - 1, 1, 1);
      YearEndDate = new DateTime(year, 12, 31);
    }
    public void SetDefaultDateStrict(int year, int month, int mode)
    {
      DateTime tgl1;
      DateTime tgl2;
      UtilityBO.SetDate(year, month, mode, out tgl1, out tgl2);
      Tgl1 = tgl1;
      Tgl2 = tgl2;
      YearStartDate = new DateTime(year, 1, 1);
      YearEndDate = new DateTime(year, 12, 31);
    }
    public void SetCopyRowKey() { }
    public void SetPrimaryKey() { }
    public void SetPageKey() { }
    public void CopyPropertyBOFrom(BaseBO source_bo, string[] props)
    {
      foreach (string pname in props)
      {
        SetValue(pname, source_bo.GetValue(pname));
      }
    }
    public void CopyPropertyBOFrom(BaseBO source_bo)//this is destination, source_bo is source BO
    {
      CopyPropertyBO(source_bo);
    }
    public void CopyPropertyBO(BaseBO source_bo)//this is destination, source_bo is source BO
    {
      if (source_bo != null)
      {
        //Mode dan Dao ga boleh ditimpa, cukup property databasenya aja
        int modePage = ModePage;
        int modeDB = ModeDB;
        //string dao = Dao;
        string xmlname = XMLName;
        source_bo.Mirror(this);
        ModePage = modePage;
        ModeDB = modeDB;
        //Dao = dao;
        XMLName = xmlname;
      }
    }

    public void CekFields(string[] _Fields)
    {
      for (int i = 0; i < _Fields.Length; i++)
      {
        PropertyInfo prop = GetType().GetProperty(_Fields[i]);
        if (prop == null)
        {
          throw new Exception("Field " + _Fields[i] + " pada Fields tidak terdefinisi!");
        }
      }
    }
    #endregion
  }
}
