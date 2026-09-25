using CoreNET.Common.Base;
using System;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.Ss10userappSsuserappControl, CoreNET.Common.BO
  [Serializable]
  public class SsuserappControl : Ss10userappControl, IDataControlUIEntry, IHasJSScript
  {
    public SsuserappControl()
    {
      XMLName = ConstantTablesSys.XMLSS10USERAPP;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_EDIT_DEL;
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
      return cViewListProperties;
    }

    public new void SetPageKey()
    {
      Kdapp = GlobalAsp.GetConfigPrefixPortal();
    }

    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalExt.GetRequestIdPrev());
      HashTableofParameterRow hpars = new HashTableofParameterRow
      {
        Ss00appLookupControl.Instance.GetLookupParameterRowAll(this, false).SetEnable(enableFilter)
      };
      return hpars;
    }

    public new void Insert()
    {
      if (IsValid())
      {
        Ss10userControl dc = new Ss10userControl
        {
          Userid = Userid,
          Usertype = Usertype,
          Usernama = Usernama,
          Userblock = Userblock,
          Userhp = Userhp,
          Useremail = Useremail,
          Useruraian = Useruraian
        };
        if (dc.Load() == null)
        {
          dc.Insert();
          dc.Userpwd = UtilityBO.GetMD5HexStringForStorePwd(Userpwd);
          //dc.Userpwd = GlobalAsp.En(Userpwd);
          dc.UpdatePwd(dc.Userpwd);
        }
        if (Load() == null)
        {
          base.Insert();
        }
      }
      else
      {
        throw new Exception(ConstantDict.Translate("LBL_INVALID_INSERT"));
      }
    }

    public new int Update()
    {
      int n = 0;
      if (IsValid())
      {
        Ss10userControl dc = new Ss10userControl
        {
          Userid = Userid,
          Usernama = Usernama,
          Userblock = Userblock,
          Userhp = Userhp,
          Useremail = Useremail,
          Useruraian = Useruraian
        };
        n = dc.Update();
      }
      else
      {
        throw new Exception(ConstantDict.Translate("LBL_INVALID_UPDATE"));
      }
      return n;
    }

    private bool IsValid()
    {
      bool valid = true;
      return valid;
    }

  }
  #endregion Ss10userappSsuserapp
}

