using CoreNET.Common.Base;
using System;
using System.Collections.Generic;

namespace CoreNET.Common.BO
{
  public class DashboardHelper : DataHelper, IDashboard
  {
    private BaseDataControlMenu menu = null;
    private bool EnableURL => string.IsNullOrEmpty(GlobalAsp.GetRequestMobile());
    private string app = null;
    private string baseurl = null;
    public DashboardHelper()
    {
      menu = (BaseDataControlMenu)GlobalExt.GetSessionMenu();
      app = GlobalAsp.GetSessionApp();
      baseurl = GlobalAsp.GetBaseURLFull();
    }
    private static int GetCountData()
    {
      BaseBO bo = new BaseBO
      {
        ConnectionString = SQLDataSource.Instance.CS_Config
      };
      string sql = @"
        select count(*) as [ROWCOUNT] from PM01PROJECT
      ";
      sql = string.Format(sql);
      string[] fields = new string[] { "RowCount" };
      List<BaseBO> list = BaseDataAdapter.GetListObject(bo, sql, fields);
      int n = (int)list[0].GetValue("RowCount");
      return n;
    }
    private static string GetSumData()
    {
      BaseBO bo = new BaseBO
      {
        ConnectionString = SQLDataSource.Instance.CS_Config
      };
      string sql = @"
        select sum(P.PROGRESS) as [GRANDTOTAL]
        from PM01PROJECT P
      ";
      sql = string.Format(sql);
      string[] fields = new string[] { "GrandTotal" };
      List<BaseBO> list = BaseDataAdapter.GetListObject(bo, sql, fields);
      decimal n = (decimal)list[0].GetValue("GrandTotal");
      return n.ToString("#,##0");
    }

    #region Methods
    public string GetRows1()
    {
      string dcname = "CoreNET.Common.BO.Dm102packPemdaControl, CoreNET.Common.PM";
      DashboardDataObject d1 = new DashboardDataObject(EnableURL)
      {
        Title = "Paket",
        Value = 0,
      };
      d1.Percent = (d1.Value.Equals(0)) ? 0 : 100;
      string idmenu = "B33048E9-9578-4595-87EA-3A4491BBFA3E";
      d1.SetURL($"{baseurl}Page/PageTabular.aspx?app={app}&id={idmenu}&dc={dcname}");

      DashboardDataObject d2 = new DashboardDataObject(EnableURL)
      {
        Title = "Unit",
        Value = 0,
      };
      d2.Percent = (d2.Value.Equals(0)) ? 0 : 100;
      idmenu = "B33048E9-9578-4595-87EA-3A4491BBFA3E";
      d2.SetURL($"{baseurl}Page/PageTreeGrid.aspx?app={app}&id={idmenu}");

      int Total = 0;
      DashboardDataObject d3 = new DashboardDataObject(EnableURL)
      {
        Title = "Pegawai",
        Value = 0,
      };
      d3.Percent = (Total == 0) ? 0 : ((decimal)100 * (int)d3.Value) / Total;
      idmenu = "9A0655B2-CFA7-43D5-8A11-0F32E1DD2E2A";
      d3.SetURL($"{baseurl}Page/PageTabular.aspx?app={app}&id={idmenu}");

      Total = Ss10userLookupControl.GetListDataSingleton().Count;
      DashboardDataObject d4 = new DashboardDataObject(EnableURL)
      {
        Title = "User",
        Value = Ss10userLookupControl.GetListDataSingleton().Count,
      };
      d4.Percent = ((decimal)100 * (int)d4.Value) / Total;
      idmenu = "29A64E59-4D3F-4B8D-A5B4-21BE795734E0";
      d4.SetURL($"{baseurl}Page/PageTabular.aspx?app={app}&id={idmenu}");

      return DataHelper.GetMD12Row(d1, d2, d3, d4);
    }
    public string GetRows2()
    {
      string dcname = "";
      string idmenu = "B33048E9-9578-4595-87EA-3A4491BBFA3E";

      //Dm102unitAktifControl.SetInstanceNull();
      DashboardDataObject d1 = new DashboardDataObject(EnableURL)
      {
        Title = "Unit Aktif",
        Value = 0,
      };
      d1.Percent = (d1.Value.Equals(0)) ? 0 : 100;
      idmenu = Guid.NewGuid().ToString();
      dcname = "CoreNET.Common.BO.Dm102unitAktifControl, CoreNET.Common.PM";
      d1.SetURL($"{baseurl}Page/PageTabular.aspx?app={app}&id={idmenu}&dc={dcname}");

      //Pm01projectLookupByKdpemdaControl.SetInstanceNull();
      DashboardDataObject d2 = new DashboardDataObject(EnableURL)
      {
        Title = "Kegiatan",
        Value = 0,
      };
      d2.Percent = (d2.Value.Equals(0)) ? 0 : 100;
      idmenu = Guid.NewGuid().ToString();
      dcname = "CoreNET.Common.BO.Pm01projectLookupByKdpemdaControl, CoreNET.Common.PM";
      d2.SetURL($"{baseurl}Page/PageTabular.aspx?app={app}&id={idmenu}&dc={dcname}");

      //Pm104pegawaiKegControl.SetInstanceNull();
      DashboardDataObject d3 = new DashboardDataObject(EnableURL)
      {
        Title = "Anggota Tim",
        Value = 0,
      };
      d3.Percent = (d3.Value.Equals(0)) ? 0 : 100;
      idmenu = Guid.NewGuid().ToString();
      dcname = "CoreNET.Common.BO.Pm104pegawaiKegControl, CoreNET.Common.PM";
      d3.SetURL($"{baseurl}Page/PageTabular.aspx?app={app}&id={idmenu}&dc={dcname}");

      return DataHelper.GetMD4Row(d1, d2, d3);
    }
    public string GetRows3()
    {
      return string.Empty;
    }
    public string GetRows4()
    {
      return string.Empty;
    }
    public string GetRows5()
    {
      return string.Empty;
    }
    #endregion
  }
}
