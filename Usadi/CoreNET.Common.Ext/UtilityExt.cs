using Ext.Net;
using Ext.Net.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.UI;

namespace CoreNET.Common.Base
{
  public class UtilityExt
  {
    public const int MODE_NORMAL = 0;
    public const int MODE_INSERT = 1;
    public const int MODE_INSERT_POST = 11;
    public const int MODE_INSERTCOPY = 2;
    public const int MODE_INSERTCOPY_POST = 12;
    public const int MODE_INSERTPACK = 4;
    public const int MODE_INSERTPACK_POST = 14;
    public const int MODE_EDIT = 8;
    public const int MODE_EDIT_POST = 18;
    private static UtilityExt _Instance = null;
    public static UtilityExt Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new UtilityExt();
        }
        return _Instance;
      }
    }
    #region Property ScreenWidth & ScreenHeight
    public static int ScreenWidth
    {
      get
      {
        if (HttpContext.Current.Session["ScreenResolution"] != null)
        {
          string strres = (string)HttpContext.Current.Session["ScreenResolution"];
          string[] res = strres.Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);
          return int.Parse(res[0]);
        }
        else
        {
          return 1366;
        }
      }
    }
    public static int ScreenHeight
    {
      get
      {
        if (HttpContext.Current.Session["ScreenResolution"] != null)
        {
          string strres = (string)HttpContext.Current.Session["ScreenResolution"];
          string[] res = strres.Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);
          return int.Parse(res[1]);
        }
        else
        {
          return 768;
        }
      }
    }


    #region Property HeightExpandTopFrame
    private int _HeightExpandTopFrame = 700;
    public int HeightExpandTopFrame
    {
      get => _HeightExpandTopFrame;
      set => _HeightExpandTopFrame = value;
    }
    #endregion

    #region Property HeightNormalizeFrame
    private int _HeightNormalizeFrame = 250;
    public int HeightNormalizeFrame
    {
      get => _HeightNormalizeFrame;
      set => _HeightNormalizeFrame = value;
    }
    #endregion

    #endregion
    public static IDataControlUI Create(string fullclassname)
    {
      IDataControlUI dc = null;
      try
      {
        Type T = Type.GetType(fullclassname);
        dc = Activator.CreateInstance(T) as IDataControlUI;
      }
      catch (Exception ex)
      {
        UtilityBO.Log(dc, ex);
      }
      return dc;
    }
    public static void SetValueFromUI(IDataControl obj, PropertyInfo prop, object ovalue)
    {
      if ((prop != null) && (prop.CanWrite))
      {
        SetValueFromUI(obj, prop.Name, prop.PropertyType, ovalue);
      }
    }
    public static void SetValueFromUI(IDataControl obj, string pname, Type ptype, object ovalue)
    {
      PropertyInfo prop = obj.GetProperty(pname);
      if ((prop != null) && (prop.CanWrite))
      {
        object value = ((BaseBO)obj).GetValue(pname);
        if ((ovalue != null) && !string.IsNullOrEmpty(ovalue.ToString()))
        {
          try
          {
            if (ptype == typeof(int))
            {
              value = int.Parse(ovalue.ToString());
            }
            else if (ptype == typeof(short))
            {
              value = short.Parse(ovalue.ToString()); //(long)((double)ovalue);
            }
            else if (ptype == typeof(long))
            {
              value = long.Parse(ovalue.ToString()); //(long)((double)ovalue);
            }
            else if (ptype == typeof(double))
            {
              value = ovalue;//double.Parse((string)ovalue,System.Globalization.NumberStyles.AllowDecimalPoint);
            }
            else if (ptype == typeof(decimal))
            {
              value = decimal.Parse(ovalue.ToString());
            }
            else if (ptype == typeof(DateTime))
            {
              value = DateTime.Parse(ovalue.ToString());
            }
            else if (ptype == typeof(bool))
            {
              if (ovalue.GetType() == typeof(bool))
              {
                value = ovalue;
              }
              else
              {
                value = bool.Parse((string)ovalue);
              }
            }
            else
            {
              value = ovalue;
            }
          }
          catch (Exception ex)
          {
            value = null;
            string s = "Casting value ke type property gagal!";
            throw new Exception(s, ex);
          }
          //try
          //{
          ((BaseBO)obj).SetValue(pname, value);
          //}
          //catch (Exception ex) 
          //{ 
          //  UtilityBO.Log(dc, ex);
          //}
        }
        else
        {
          value = UtilityBO.GetDefault(ptype);
          try
          {
            ((BaseBO)obj).SetValue(pname, value);
          }
          catch (Exception ex)
          {
            string meth = UtilityBO.GetCurrentMethod(1);
            string msg = string.Format("Property {0} in class {1}", pname, obj.GetType().FullName);
            UtilityBO.Log(obj, meth, msg, ex);
          }
        }
      }
    }
    public static Icon GetIcon(string str)
    {
      if (str.Equals("home"))
      {
        return Icon.ApplicationHome;
      }
      else if (str.Equals("logout"))
      {
        return Icon.ApplicationGo;
      }
      else
      {
        return Icon.ApplicationForm;
      }
    }
    public static BaseBO GoToSelectedObject(IList list, IDataControl dc, string[] props)
    {
      int i = 0;
      BaseBO bo = null;
      try
      {
        bool found = false;
        while ((i < list.Count) && (!found))
        {
          bo = (BaseBO)list[i];
          if (bo.IsEqualValue((BaseBO)dc, props))
          {
            found = true;
          }
          else
          {
            i++;
          }
        }
      }
      catch (Exception ex)
      {
        UtilityBO.Log(dc, ex);
      }
      return bo;
    }
    public static void SetNonIFrameAutoLoad(Ext.Net.LoadConfig AutoLoad)
    {
      AutoLoad.ReloadOnEvent = true;
      AutoLoad.ShowMask = true;
      AutoLoad.NoCache = true;
    }
    public static void SetIFrameAutoLoad(Ext.Net.LoadConfig AutoLoad)
    {
      AutoLoad.Mode = LoadMode.IFrame;
      AutoLoad.ReloadOnEvent = true;
      AutoLoad.ShowMask = true;
      AutoLoad.NoCache = true;
    }
    public static string ValidateURL(object sender, string url)
    {
      return url;
    }
    public static void SetURL(LoadConfig AutoLoad, string url)
    {
      string validurl = ValidateURL(null, url);
      AutoLoad.Url = validurl;
    }
    public static void SetURL(Ext.Net.Panel pnl, IURL ctrl)
    {
      SetPropertyUrl(pnl, ctrl);
      pnl.Listeners.Activate.Handler = "this.loadContent();";
    }
    public static void LoadUrl(Ext.Net.Panel pnl, string url)
    {
      if (!string.IsNullOrEmpty(url))
      {
        SetURL(pnl.AutoLoad, url);
        //pnl.LoadContent();//double load
      }
    }
    public static void LoadUrl(Ext.Net.Panel pnl, IURL ctrl)
    {
      SetPropertyUrl(pnl, ctrl);
      pnl.LoadContent();
    }
    private static void SetPropertyUrl(Ext.Net.Panel pnl, IURL ctrl)
    {
      //pnl.Icon = Icon.Application;
      pnl.Title = ctrl.GetTitle();
      pnl.ToolTip = ctrl.GetTabTip();
      pnl.AutoLoad.Url = ValidateURL(null, ctrl.ConstructURL());
      UtilityExt.SetIFrameAutoLoad(pnl.AutoLoad);
    }
    #region dari UtilityExt
    public static void SetModeEditable(int modeedit)
    {
      HttpContext.Current.Session[ExtGridPanel.SESSION_MODE] = modeedit;
    }
    public static int GetModeEditable()
    {
      if (HttpContext.Current.Session[ExtGridPanel.SESSION_MODE] == null)
      {
        return UtilityExt.MODE_NORMAL;
      }
      else
      {
        return (int)HttpContext.Current.Session[ExtGridPanel.SESSION_MODE];
      }
    }
    public static bool IsModeAddFromDialogForm()
    {
      return !string.IsNullOrEmpty(HttpContext.Current.Request["me"]) && (HttpContext.Current.Request["me"].Equals("add"));
      //return (GetModeEditable() == UtilityExt.MODE_INSERTPACK);
    }
    public static bool IsModeAdd()
    {
      return (GetModeEditable() == UtilityExt.MODE_INSERT)
        || (GetModeEditable() == UtilityExt.MODE_INSERTCOPY)
        || (GetModeEditable() == UtilityExt.MODE_INSERTPACK);
    }
    public static bool IsModeEdit()
    {
      return (GetModeEditable() == UtilityExt.MODE_EDIT);
    }
    public static void SetValue(IDataControlUI dc, Dictionary<string, object> row)
    {
      SetValueExcludeRO(dc, row);
    }
    public static void SetValueExcludeRO(IDataControlUI dc, Dictionary<string, object> row)
    {
      ArrayList array = new ArrayList();
      array.AddRange(((BaseDataControl)dc).GetNotFields());
      array.AddRange(((ViewListProperties)dc.GetProperties()).ReadOnlyFields);
      array.AddRange(new string[] { "checked", "nodeType" });
      foreach (string key in row.Keys)
      {
        if ((array == null) || !array.Contains(key))
        {
          try
          {
            PropertyInfo prop = dc.GetProperty(key);
            if ((prop == null) && (key.Contains("textEntry")))
            {
              prop = dc.GetProperty(key.Replace("textEntry", string.Empty));
            }
            if ((prop != null) && (prop.CanWrite))
            {
              SetValueFromUI(dc, prop, row[key]);
            }
          }
          catch (Exception ex)
          {
            string meth = UtilityBO.GetCurrentMethod(1);
            string msg = string.Format("Property {0} in class {1}", key, dc.GetType().FullName);
            UtilityBO.Log(dc, meth, msg, ex);
          }
        }
      }
    }
    public static void SetValue(IDataControlUI dc, string json)
    {
      try
      {
        string newjson = json;
        if (!json.StartsWith("[{"))
        {
          newjson = "[" + json + "]";
        }

        ArrayList array = new ArrayList();
        array.AddRange(((BaseDataControl)dc).GetNotFields());
        if (!json.Equals(string.Empty))
        {
          Dictionary<string, object>[] datas = JSON.Deserialize<Dictionary<string, object>[]>(newjson);

          foreach (Dictionary<string, object> row in datas)
          {
            UtilityExt.SetValue(dc, row);
          }
        }
        else
        {
          UtilityUI.ResetEntry(dc);
        }
      }
      catch (Exception ex)
      {
        UtilityBO.Log(dc, ex);
        //Maybe error casting in 296
      }
    }
    private static void SetValueObject(IDataControlUI dc, string json)
    {
      try
      {
        ArrayList array = new ArrayList();
        array.AddRange(((BaseDataControl)dc).GetNotFields());
        if (!json.Equals(string.Empty))
        {
          JsonSerializerSettings setting = new Newtonsoft.Json.JsonSerializerSettings()
          {
            NullValueHandling = NullValueHandling.Ignore
          };
          //dc = (IDataControlUI)JSON.Deserialize(json, dc.GetType(), setting);
          IDataControlUI temp = (IDataControlUI)JsonConvert.DeserializeObject(json, dc.GetType(), setting);
          ((BaseDataControl)dc).CopyPropertyBOFrom((BaseBO)temp);
        }
        else
        {
          UtilityUI.ResetEntry(dc);
        }
      }
      catch (Exception ex)
      {
        UtilityBO.Log(dc, ex);
      }
    }
    public static void PrepareForAdd(FormPanel FPFormEntry1, string json)
    {
      UtilityExt.SetModeEditable(UtilityExt.MODE_INSERT);
      //HttpContext.Current.Session[ExtGridPanel.SESSION_MODE] = ExtGridPanel.MODE_INSERT;
      int id = GlobalAsp.GetRequestI();
      IDataControlUIEntry dc = (IDataControlUIEntry)UtilityUI.GetDataControl(id);
      if (!json.Contains("[]"))/*Untuk tambah anak kan?*/
      {
        //IDataControlUIEntry prev = (IDataControlUIEntry)((BaseBO)dc).Clone();
        SetValue(dc, json);
        IDataControlUIEntry prev = (IDataControlUIEntry)BaseBO.Clone(dc);
        GlobalAsp.SetEditingObject(prev);
      }
      else
      {
        GlobalAsp.SetEditingObject(null);
      }
      if (FPFormEntry1 != null)
      {
        UtilityUI.ResetEntry(dc);
        dc.SetPageKey();
        dc.SetPrimaryKey();
        if (((ViewListProperties)dc.GetProperties()).IsSetVisible)
        {
          CoreNETCompositeField.SetVisibleAll(FPFormEntry1, dc.GetEntries().GetOrderKeys());
          FPFormEntry1.Reload();
        }
        CoreNETCompositeField.SetWhenAdd(FPFormEntry1, dc);
        FPFormEntry1.ForceLayout = true;
        FPFormEntry1.DoLayout();
        FPFormEntry1.Reset();
        CoreNETCompositeField.SetValues(FPFormEntry1, dc);
      }
    }
    public static void PrepareForEdit(FormPanel FPFormEntry1, string json)
    {
      int id = GlobalAsp.GetRequestI();
      IDataControlUIEntry dc = (IDataControlUIEntry)UtilityUI.GetDataControl(id);
      if (!string.IsNullOrEmpty(json))
      {
        SetValue(dc, json);
      }
      #region Potential Error
      //Klo ini dikomen, btnback bisa kembali, artinya ada kelas yg ngga seriazable
      //  [Serializable]
      if (!dc.GetType().IsSerializable)
      {
        throw new Exception(string.Format("{0} is not serializable", dc.GetType().FullName));
      }
      //IDataControlUIEntry prev = (IDataControlUIEntry)((BaseBO)dc).Clone();
      IDataControlUIEntry prev = (IDataControlUIEntry)BaseBO.Clone(dc);
      GlobalAsp.SetEditingObject(dc);
      #endregion
      if (FPFormEntry1 != null)
      {
        CoreNETCompositeField.SetWhenEdit(FPFormEntry1, dc);
        CoreNETCompositeField.SetHideWhenEdit(FPFormEntry1, dc.GetEntries());
        FPFormEntry1.ForceLayout = true;
        FPFormEntry1.DoLayout();
        CoreNETCompositeField.SetValues(FPFormEntry1, dc);
        //FPFormEntry1.SetValues(dc);//prilakunya blm tau
      }
    }
    public static void PrepareForCopy(FormPanel FPFormEntry1, string json)
    {
      UtilityExt.SetModeEditable(UtilityExt.MODE_INSERTCOPY);
      //HttpContext.Current.Session[ExtGridPanel.SESSION_MODE] = ExtGridPanel.MODE_INSERT;
      int id = GlobalAsp.GetRequestI();
      IDataControlUIEntry dc = (IDataControlUIEntry)UtilityUI.GetDataControl(id);
      SetValue(dc, json);
      //IDataControlUIEntry prev = (IDataControlUIEntry)((BaseBO)dc).Clone();
      IDataControlUIEntry prev = (IDataControlUIEntry)BaseBO.Clone(dc);
      GlobalAsp.SetEditingObject(prev);

      if (FPFormEntry1 != null)
      {
        CoreNETCompositeField.SetWhenAdd(FPFormEntry1, dc);
        FPFormEntry1.ForceLayout = true;
        FPFormEntry1.DoLayout();
        string pk = ((ViewListProperties)dc.GetProperties()).IDProperty;
        PropertyInfo prop = dc.GetProperty(pk);
        if (string.IsNullOrEmpty(pk) || (prop == null))
        {
          string msg = ConstantDict.Translate("LBL_INVALID_PROPERTY=Property {0} invalid!");
          throw new Exception(string.Format(msg, pk));
        }
        //dc.SetCopyRowKey();
        CoreNETCompositeField.SetValues(FPFormEntry1, dc);
      }
    }
    public static void RefreshForEdit(FormPanel FPFormEntry1)
    {
      int id = GlobalAsp.GetRequestI();
      IDataControlUIEntry dc = (IDataControlUIEntry)UtilityUI.GetDataControl(id);
      CoreNETCompositeField.GetValues(FPFormEntry1, dc);
      //FPFormEntry1.LoadContent();
      if (FPFormEntry1 != null)
      {
        FPFormEntry1.UpdateContent();
        CoreNETCompositeField.SetWhenEdit(FPFormEntry1, dc);
        CoreNETCompositeField.SetHideWhenEdit(FPFormEntry1, dc.GetEntries());
        FPFormEntry1.ForceLayout = true;
        FPFormEntry1.DoLayout();
        CoreNETCompositeField.SetValues(FPFormEntry1, dc);
      }
    }
    public static void PrepareForReview(FormPanel FPFormEntry1, string json)
    {
      int id = GlobalAsp.GetRequestI();
      IDataControlUIEntry dc = (IDataControlUIEntry)UtilityUI.GetDataControl(id);
      SetValue(dc, json);
      PrepareForReview(FPFormEntry1, dc);
    }
    public static void PrepareForReview(FormPanel FPFormEntry1, IDataControlUIEntry dc)
    {
      int id = GlobalAsp.GetRequestI();
      /*execute before CoreNET.Methods1.btnSaveEditingCell, make unexpected result, so i comment this */
      //HttpContext.Current.Session[GlobalApp.SESSION_OBJECT_PREV + id] = ((BaseBO)dc).Clone();
      if (FPFormEntry1 != null)
      {
        CoreNETCompositeField.SetDisableAll(FPFormEntry1, dc);
        CoreNETCompositeField.SetHideWhenEdit(FPFormEntry1, dc.GetEntries());
        FPFormEntry1.ForceLayout = true;
        FPFormEntry1.DoLayout();
        CoreNETCompositeField.SetValues(FPFormEntry1, dc);
      }
    }
    public static void PrepareAfterInsert(FormPanel FPFormEntry1)
    {
      int id = GlobalAsp.GetRequestI();
      IDataControlUIEntry dc = (IDataControlUIEntry)UtilityUI.GetDataControl(id);
      UtilityExt.SetModeEditable(UtilityExt.GetModeEditable() + 10);//UtilityExt.MODE_NORMAL
      //UtilityExt.SetModeEditable(UtilityExt.MODE_NORMAL);
      //HttpContext.Current.Session[ExtGridPanel.SESSION_MODE] = ExtGridPanel.MODE_NORMAL;
      if (FPFormEntry1 != null)
      {
        CoreNETCompositeField.SetDisableAll(FPFormEntry1, dc);
        CoreNETCompositeField.SetHideWhenEdit(FPFormEntry1, dc.GetEntries());
        //FPFormEntry1.Reset();
        FPFormEntry1.ForceLayout = true;
        FPFormEntry1.DoLayout();
        CoreNETCompositeField.SetValues(FPFormEntry1, dc);
      }
    }
    public static void DeleteMultiple(string json)
    {
      int id = GlobalAsp.GetRequestI();
      IDataControlUIEntry dc = (IDataControlUIEntry)UtilityUI.GetDataControl(id);

      string newjson = json;
      if (!json.Contains("["))
      {
        newjson = "[" + json + "]";
      }
      Dictionary<string, object>[] data = JSON.Deserialize<Dictionary<string, object>[]>(newjson);
      foreach (Dictionary<string, object> row in data)
      {
        try
        {
          UtilityExt.SetValueExcludeRO(dc, row);
          dc.Delete();
        }
        catch (Exception ex)
        {
          UtilityBO.Log(dc, ex);
          throw ex;
        }
      }
      dc.AfterDelete();
    }



    public static void DeleteTreeGrid(IDataControlUI dc, string json)
    {
      try
      {
        SetValue(dc, json);
        dc.Delete();
        try
        {
          X.Js.Call("refreshData");
        }
        catch (Exception ex)
        {
          UtilityBO.Log(dc, ex);
        }
      }
      catch (Exception ex)
      {
        X.MessageBox.Alert("Informasi", ex.Message).Show();
      }
    }
    public static void PrepareViewWindow(int id, Control container, IDataControlUIEntry dc, string json, string cmdname)
    {
      SetValueObject(dc, json);
      //IDataControlUIEntry prev = (IDataControlUIEntry)((BaseBO)dc).Clone();
      IDataControlUIEntry prev = (IDataControlUIEntry)BaseBO.Clone(dc);
      GlobalAsp.SetEditingObject(prev);
      UtilityUI.SetDataControl(id, dc);
      PrepareViewWindow(id, (Ext.Net.Window)container, dc, cmdname);
    }
    private static void PrepareViewWindow(int id, Ext.Net.Window container, IDataControlUIEntry dc, string cmdname)
    {
      string str = null;
      try
      {
        object oval = dc.GetValue(cmdname);
        if (oval != null)
        {
          str = oval.ToString();
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Property name " + cmdname + " doesn't exist or " + ex.Message);
      }
      string[] strs = null;
      if (str.Contains("|"))
      {
        strs = str.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
      }
      else
      {
        strs = str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
      }
      string idcomp = cmdname;
      string title = strs[0];
      string url = strs[1];
      if (cmdname.StartsWith(GlobalAsp.LINK))
      {
        X.Js.Call("parent.loadPageByID", new string[] { "Pages", title, title, url });
      }
      else
      {
        container.Title = title;
        container.ToolTip = title;
        container.AutoLoad.Url = ValidateURL(null, url);
        UtilityExt.SetIFrameAutoLoad(container.AutoLoad);
        container.LoadContent();
      }
    }
    public static void SaveFormPanel(int id, Control container, string formpanel, IDataControlUI dc)
    {
      SaveFormPanel(id, container, formpanel, dc, true);
    }
    public static void SaveFormPanel(int id, Control container, string formpanel, IDataControlUI dc, bool callsetpk)
    {
      FormPanel FPFormEntry1 = ControlUtils.FindControl<FormPanel>(container, formpanel);
      if (FPFormEntry1 != null)
      {
        CoreNETCompositeField.GetValues(FPFormEntry1, dc);
        //dc.SetPageKey();//Harusnya ngga perlu
        try
        {
          //if (((string)HttpContext.Current.Session[ExtGridPanel.SESSION_MODE]).Equals(ExtGridPanel.MODE_EDIT))
          if (UtilityExt.IsModeEdit())
          {
            //Jika ada race condition, tambagkan set pk didalam method Update
            dc.Update();
            GlobalAsp.SetEditingObject(null);
            try
            {
              X.Js.Call("refreshDataWithSelection");
            }
            catch (Exception ex)
            {
              UtilityBO.Log(dc, ex);
            }
          }
          else
          {
            dc.Insert();
            try
            {
              #region Kalau mau inserted date kadi selection
              GridPanel GridPanel1 = ControlUtils.FindControl<GridPanel>(container, "GridPanel" + id);
              RowSelectionModel sm = GridPanel1.SelectionModel.Primary as RowSelectionModel;
              string idproperty = ((ViewListProperties)dc.GetProperties()).IDProperty;
              object oval = dc.GetValue(idproperty);
              if (oval != null)
              {
                sm.SelectedRows.Add(new SelectedRow(oval.ToString()));
              }
              //if (!string.IsNullOrEmpty(idproperty))
              //{
              //  PropertyInfo prop = dc.GetProperty(idproperty);
              //  if (prop != null)
              //  {
              //    object oval = dc.GetValue(idproperty);
              //    if (oval != null)
              //    {
              //      sm.SelectedRows.Add(new SelectedRow(oval.ToString()));
              //    }
              //  }
              //}
              sm.UpdateSelection();
              X.Js.Call("refreshDataWithSelection");
              #endregion
              //X.Js.Call("refreshData");
            }
            catch (Exception ex)
            {
              X.Js.Call("refreshData");
              UtilityBO.Log(dc, ex);
            }
          }
          UtilityExt.SetModeEditable(UtilityExt.GetModeEditable() + 10);//UtilityExt.MODE_NORMAL
          //UtilityExt.SetModeEditable(UtilityExt.MODE_NORMAL);
          //HttpContext.Current.Session[ExtGridPanel.SESSION_MODE] = ExtGridPanel.MODE_NORMAL;
        }
        catch (Exception ex)
        {
          X.MessageBox.Alert("Informasi", ex.Message).Show();
        }
      }
    }
    #endregion
  }
}
