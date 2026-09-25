using CoreNET.Common.BO;
using Ext.Net;
using Ext.Net.Utilities;
using System;
using System.Collections;
using System.Reflection;
using System.Web.UI;

namespace CoreNET.Common.Base
{
  public class ExtStore : Store
  {
    protected int Id;
    public ExtStore(int id, IDataControlUI cCtrl, Page page)
    {
      Id = id;
      ID = "Store" + id;

      SettingStore(this, cCtrl, page);
    }
    public ExtStore(string id, IDataControlUI cCtrl, Page page)
    {
      ID = id;
      ID = "Store" + id;
      SettingStore(this, cCtrl, page);
      BindStore(this, cCtrl, true);
    }
    public ExtStore(string id, IDataControlUI cCtrl, bool session, Page page)
    {
      ID = "Store" + id;
      SettingStore(this, cCtrl, page);
      BindStore(this, cCtrl, session);
    }

    public ExtStore(string id, string[] fields, IList data)
    {
      ID = "Store" + id;
      JsonReader reader = new JsonReader
      {
        IDProperty = fields[0]
      };
      for (int i = 0; i < fields.Length; i++)
      {
        reader.Fields.Add(new RecordField(fields[i], GetRecordFieldType(typeof(string))));
      }
      Reader.Add(reader);
      DataSource = data;
      DataBind();
    }
    #region Setting Store without BindStore
    public static void SettingStore(Store store, IDataControlUI cCtrl, Page page)
    {
      string[] cols = null;
      try
      {
        cols = cCtrl.GetKeys();
      }
      catch (Exception)
      {
        cols = cCtrl.GetFields();
      }
      //cols = SysgetkeysLookupControl.GetFields(cCtrl);
      string idproperty = ((ViewListProperties)cCtrl.GetProperties()).IDProperty;
      SettingStore(store, cCtrl, cols, idproperty, page);
    }
    public static void SettingStore(Store store, IDataControlUI cCtrl, string[] cols, string idproperty, Page page)
    {
      //Learn Store Event
      //http://localhost:81/#/GridPanel/WebService_Connections/StoreEvents/
      //Validasi
      ExtGridPanel gp = (ExtGridPanel)ControlUtils.FindControl<Ext.Net.GridPanel>(page.Form, "GridPanel1");
      if (gp != null)
      {
        try
        {
          gp.Validate(page);
        }
        catch (Exception ex)
        {
          UtilityBO.Log(cCtrl, ex);
          X.Js.Alert(string.Empty);
        }
      }
      store.DirectEventConfig.Timeout = GlobalAsp.TIMEOUT;

      JsonReader reader = new JsonReader
      {
        IDProperty = idproperty
      };
      string nopname = string.Empty;
      for (int i = 0; i < cols.Length; i++)
      {
        if (cCtrl.GetProperty(cols[i]) == null)
        {
          nopname += cols[i] + ",";
        }
        else
        {
          reader.Fields.Add(new RecordField(cols[i], GetRecordFieldType(cCtrl.GetProperty(cols[i]).PropertyType)));
        }
      }
      if (!string.IsNullOrEmpty(nopname))
      {
        throw new Exception("Property " + nopname + " doesnt't exist in class " +
          cCtrl.GetType().Name + "! Error in ExtStore." + MethodBase.GetCurrentMethod() + " line 85");
      }
      store.Reader.Add(reader);

      string[] sfields = ((ViewListProperties)cCtrl.GetProperties()).SortFields;
      store.SortInfo.Field = (sfields.Length > 0) ? sfields[0] : idproperty;//optional Please use the Store's SetDefaultSort method. 
      int sortdirection = ((ViewListProperties)cCtrl.GetProperties()).SortDirection;
      store.SortInfo.Direction = (sortdirection == ViewListProperties.ASC) ? Ext.Net.SortDirection.ASC : Ext.Net.SortDirection.DESC;
    }
    #endregion
    public Store BindStore()
    {
      return BindStore(this, UtilityUI.GetDataControl(Id), true);
    }
    public Store BindStore(IDataControl ctrl)
    {
      return BindStore(this, ctrl, true);
    }
    public static Store BindStore(Store store, IDataControl ctrl)
    {
      return BindStore(store, ctrl, true);
    }
    public static Store BindStore(Store store, IDataControl ctrl, bool session)
    {
      return BindStore(store, ctrl, session, false);
    }
    public static Store BindStore(Store store, IDataControl ctrl, bool session, bool pass)
    {
      int id = GlobalAsp.GetRequestI();
      IList list = GlobalAsp.GetSessionListRows();
      if (!pass || (list == null))
      {
        string label = string.Empty;
        if (typeof(IDataControlUI).IsInstanceOfType(ctrl))
        {
          ((IDataControlUI)ctrl).SetPageKey();

          IDataControlUI dcCaller = UtilityUI.GetDataControl(id);
          string url = System.Web.HttpContext.Current.Request.Url.OriginalString;
          if ((((ViewListProperties)dcCaller.GetProperties()).EntryStyle == ViewListProperties.ENTRY_STYLE_LOOKUP)
            && url.Contains("GridLookup.aspx"))
          {
            label = ((ViewListProperties)((IDataControlUI)dcCaller).GetProperties()).LookupLabelQuery;
          }
        }
        if (string.IsNullOrEmpty(label))
        {
          list = ctrl.View();
        }
        else
        {
          list = ctrl.View(label);
        }
      }
      if (session)
      {
        GlobalAsp.SetSessionListRows(id, list);
      }
      if (!GlobalAsp.GetSessionUser().IsAdmin())
      {
        //Seharusnya Filter Non Status -1
        store.DataSource = list;
      }
      else
      {
        store.DataSource = list;
      }
      store.DataBind();
      return store;
    }
    //public static Store BindStore(Store store, IDataControl ctrl, IDataControl ctrlpar)
    //{
    //  IList list = ctrl.View(label, (BaseBO)ctrlpar);
    //  store.DataSource = list;
    //  store.DataBind();
    //  return store;
    //}
    public static Store BindStore(Store store, IDataControl ctrl, StoreRefreshDataEventArgs e)
    {
      return BindStore(store, ctrl, e, true);
    }
    public static Store BindStore(Store store, IDataControl ctrl, StoreRefreshDataEventArgs e, bool savesession)
    {
      IList list = null;
      string label = string.Empty;
      if (typeof(IDataControlUI).IsInstanceOfType(ctrl))
      {
        ((IDataControlUI)ctrl).SetPageKey();

        int id = GlobalExt.GetRequestI();
        IDataControlUI dcCaller = UtilityUI.GetDataControl(id);
        string url = System.Web.HttpContext.Current.Request.Url.OriginalString;
        if ((((ViewListProperties)dcCaller.GetProperties()).EntryStyle == ViewListProperties.ENTRY_STYLE_LOOKUP)
          && url.Contains("GridLookup.aspx"))
        {
          label = ((ViewListProperties)((IDataControlUI)dcCaller).GetProperties()).LookupLabelQuery;
        }
      }
      if (string.IsNullOrEmpty(label))
      {
        list = ctrl.View();
      }
      else
      {
        list = ctrl.View(label);
      }
      store.DataSource = list;
      store.DataBind();
      if (savesession)
      {
        int id = GlobalAsp.GetRequestI();
        GlobalAsp.SetSessionListRows(id, list);
      }
      return store;
    }
    public static RecordFieldType GetRecordFieldType(Type type)
    {
      if (type == typeof(int))
      {
        return RecordFieldType.Int;
      }
      else if (type == typeof(double))
      {
        return RecordFieldType.Float;
      }
      else if (type == typeof(decimal))
      {
        return RecordFieldType.Float;
      }
      else if (type == typeof(DateTime))
      {
        return RecordFieldType.Date;
      }
      else if (type == typeof(bool))
      {
        return RecordFieldType.Boolean;
      }
      else
      {
        return RecordFieldType.String;
      }
    }
  }
}
