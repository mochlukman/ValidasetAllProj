using CoreNET.Common.Base;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreNET.Common.BO
{
  #region Ss10userapp
  [Serializable]
  public class Ss10userappControl : Ss10userControl, IDataControlUIEntry, IExtLoadCsv, IHasJSScript
  {
    #region Methods 
    public Ss10userappControl()
    {
      XMLName = ConstantTablesSys.XMLSS10USERAPP;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Userid", "Idapp" };
      cViewListProperties.ReadOnlyFields = new String[] { "Idapp" };
      cViewListProperties.IDProperty = "Userid";
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_LOOKUP;
      if (MasterAppConstants.Instance.StatusAdmin)
      {
        cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_DEL;
      }
      else
      {
        cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_READONLY;
      }
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_LOOKUP;
      cViewListProperties.LookupDC = "CoreNET.Common.BO.Ss10userControl, CoreNET.Common.Sys";
      cViewListProperties.LookupLabelQuery = "LookupByApp";
      return cViewListProperties;
    }
    public new void SetPageKey()
    {
      Status = 0;
      //Kdapp = GlobalAsp.GetConfigPrefixPortal();
    }

    public new void SetFilterKey(BaseBO bo)
    {
      if (!string.IsNullOrEmpty(GlobalAsp.GetRequestVal()))
      {
        Idapp = GlobalAsp.GetRequestVal();
        SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
      }
      else
      {
        if (typeof(IDataControlMenu).IsInstanceOfType(bo))
        {
          Kdpemda = !string.IsNullOrEmpty(Kdpemda) ? Kdpemda : ((Ss10userControl)GlobalAsp.GetSessionUser()).Kdpemda;
          Nmpemda = !string.IsNullOrEmpty(Nmpemda) ? Nmpemda : ((Ss10userControl)GlobalAsp.GetSessionUser()).Nmpemda;
          Last_by = GlobalAsp.GetSessionUser().GetUserID();
        }
        else if (typeof(Ss10userControl).IsInstanceOfType(bo))
        {
          Userid = ((Ss10userControl)bo).Userid;
        }
        else if (typeof(Ss00appControl).IsInstanceOfType(bo))
        {
          Idapp = ((Ss00appControl)bo).Idapp;
        }
      }
    }
    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalExt.GetRequestIdPrev())
        && string.IsNullOrEmpty(GlobalExt.GetRequestVal());
      HashTableofParameterRow hpars = new HashTableofParameterRow
      {
        Ss00appLookupControl.Instance.GetLookupParameterRowAll(this, false).SetEnable(enableFilter)
      };
      return hpars;
    }


    public new void Insert()
    {
      if (!string.IsNullOrEmpty(Idapp))
      {
        if (IsValid())
        {
          ((BaseDataControl)this).Insert();
        }
        else
        {
          throw new Exception(ConstantDict.Translate("LBL_INVALID_INSERT"));
        }
      }
    }

    public new int Update()
    {
      int n = 0;

      n = base.Update();
      Ss10userControl dc = new Ss10userControl
      {
        Userid = Userid
      };
      dc.Load();
      dc.CopyPropertyBOFrom(this, new string[] { "Usernama", "Userblock", "Userhp", "Useremail", "Useruraian" });
      dc.Update();

      return n;
    }

    //Untuk Insert
    private bool IsValid()
    {
      bool valid = true;

      string sql = @"
        select count(*) as [ROWCOUNT] from SS10USERAPP where IDAPP='{0}' and USERID='{1}'
      ";
      sql = string.Format(sql, Idapp, Userid);
      string[] fields = new string[] { "RowCount" };
      List<IDataControl> list = BaseDataAdapter.GetListDC(this, sql, fields);
      valid = (((int)list[0].GetValue("RowCount")) == 0);
      return valid;
    }

    public new int Delete()
    {
      //Status = -1;//force delete
      return ((BaseDataControl)this).Delete();
    }

    public new BaseBO Load()
    {
      BaseBO bo = ((BaseDataControlUI)this).Load(BaseDataControl.PK);
      return bo;
    }

    public new IList View()
    {
      IList list = View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      IList list = ((BaseDataControl)this).View(label);
      List<Ss10userControl> ListData = new List<Ss10userControl>();
      foreach (Ss10userControl dc in list)
      {
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      return ListData;
    }
    #endregion Methods 
  }
  #endregion Ss10userapp
}

