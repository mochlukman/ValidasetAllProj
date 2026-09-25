using CoreNET.Common.Base;
using System;
using System.Collections.Generic;

namespace CoreNET.Common.BO
{
  #region SsappuserSIPD
  [Serializable]
  public class Ss10userLoginControl : Ss10userControl, IDataControlAppuserUI, IHasJSScript
  {
    public Ss10userLoginControl()
    {
      XMLName = ConstantTablesSys.XMLSS10USER;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      return cViewListProperties;
    }
    public override HashTableofParameterRow GetEntries()
    {
      hpars = new HashTableofParameterRow();
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Userid")));
      hpars.Add(new ParameterRowPassword(this, ConstantDict.GetColumnTitle("Userpwd")));
      return hpars;
    }
    public new BaseBO Load()
    {
      BaseBO bo = Load(BaseDataControl.PK);
      if (bo == null)
      {
        Ss10userControl dcuser = new Ss10userControl();
        dcuser.Userid = Userid;
        bo = dcuser.Load(BaseDataControl.ID);

        if (bo != null)
        {
          SsappControl dcapp = SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
          bool allowadd = dcapp.Last_by.Equals(Userid) || dcuser.Usertype.Equals(GlobalAsp.USER_ADMIN);
          //Modul Programmer
          allowadd = allowadd || (Idapp.Equals("2418FA19-7538-4FC6-A3FF-FD47B7BC6501") && dcuser.Usertype.Equals(GlobalAsp.USER_DEVELOPER));
          if (allowadd)
          {
            Ss10userappControl dcuserapp = new Ss10userappControl();
            dcuserapp.Idapp = Idapp;
            dcuserapp.Userid = Userid;
            dcuserapp.Insert();
            bo = Load(BaseDataControl.PK);
          }
        }
      }
      if (bo != null)
      {
        MasterAppConstants.Instance.StatusAdmin = ((Ss10userControl)bo).Usertype.Equals(MasterAppConstants.STR_ADMIN);
        if (Idapp == MasterAppConstants.Instance.MasterAppID)
        {
          if (MasterAppConstants.Instance.StatusAdmin)
          {
            return bo;
          }
          else
          {
            return null;
          }
        }
        else
        {
          return bo;
        }
      }
      else
      {
        return null;
      }
    }

    public new void Insert()
    {
      base.Insert();
    }
    public void InsertUser()
    {
      #region InsertUser
      try
      {
        Ss10userControl dc = new Ss10userControl();
        dc.SetPrimaryKey();
        dc.Userid = Userid;
        dc.Insert();
      }
      catch (Exception) { }
      #endregion
    }
    public void InsertModul()
    {
      #region InsertModul
      try
      {
        //Insert Generated Modul
        Ss10userappControl dc = new Ss10userappControl();
        dc.SetPrimaryKey();
        dc.Kdapp = "";
        dc.Nmapp = Nmapp;
        dc.Status = 0;
        dc.Insert();

        string idapp = dc.Idapp;//Generated in SetPrimaryKey
        string sql = $@"exec RevalidateSS00AppConfig '{idapp}'";
        BaseDataAdapter.ExecuteCmd(this, sql);

        Ss10userappControl dcUser = new Ss10userappControl();
        dcUser.Idapp = idapp;
        dcUser.Userid = Userid;
        dcUser.Status = 0;
        dcUser.Insert();

      }
      catch (Exception) { }
      #endregion
    }
    public void InsertUserModul()
    {
      #region InsertUserModul
      try
      {
      }
      catch (Exception) { }
      #endregion
    }

  }
  #endregion SsappuserSIPD

}

