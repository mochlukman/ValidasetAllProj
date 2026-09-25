using System;
using System.Collections;
using System.Collections.Generic;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  #region Ssappmenu
  [Serializable]
  public class SsappmenuControl : BaseDataControl, IDataControl
  {
    #region Properties 
    public new string Idapp { get; set; }
    public string Idmenu { get; set; }
    public string Kdmenu { get; set; }
    public string Nmmenu { get; set; }
    public string Olist1 { get; set; }
    public string Olistdetil1 { get; set; }
    public string Olistdetil2 { get; set; }
    public string Olistdetil3 { get; set; }
    public string Olistdetil4 { get; set; }
    #endregion Properties 

    #region Methods 
    public SsappmenuControl()
    {
      XMLName = "Ssappmenu";
      ModeDB = SQLDataSource.MODE_DB_CONFIG;
      ConnectionString = SQLDataSource.Instance.CS_Config;
    }
    public new IList View()
    {
      IList list = this.View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      string template_sql = @"
        select rtrim(S.IDAPP) as IDAPP,rtrim(S.KDAPP) as KDAPP,rtrim(S.NMAPP) as NMAPP,rtrim(S.URAPP) as URAPP,
        rtrim(A.KDMENU) as KDMENU,
        case when A.STATUS=-1 then rtrim(A.NMMENU)+' (test)' else rtrim(A.NMMENU) end as NMMENU,
        rtrim(A.IDMENU) as IDMENU,rtrim(isnull(A.KDDOK,'')) as KDDOK,rtrim(isnull(A.IDXDOK,'')) as IDXDOK,
        rtrim(A.URL) as URL,
        case A.URL when '' then '' else rtrim(A.URL)+'app='+A.IDAPP+'&amp;id='+A.IDMENU+'&amp;kode='+rtrim(isnull(A.KDDOK,''))+'&amp;idx='+rtrim(isnull(A.IDXDOK,''))+'&amp;root=1' end as URLFULL,
        rtrim(A.OLIST1) as OLIST1,rtrim(A.OLISTDETIL1) as OLISTDETIL1,rtrim(A.OLISTDETIL2) as OLISTDETIL2,
        rtrim(A.OLISTDETIL3) as OLISTDETIL3,rtrim(A.OLISTDETIL4) as OLISTDETIL4,
        A.STATUS,A.KDLEVEL,rtrim(A.TYPE) as TYPE,rtrim(A.LAST_BY) as LAST_BY,A.LAST_DATE
        from {0} S
        inner join {1} A on S.IDAPP=A.IDAPP
      ";
      string[] fields = new string[] { "Idapp", "Kdapp", "Nmapp", "Urapp"
        , "Kdmenu", "Nmmenu", "Idmenu", "Idxdok", "Kddok"
        , "Url", "UrlFull", "Kdlevel", "Type"
        , "Status", "Statusicon", "Last_by", "Last_date"
        , "Olist1", "Olistdetil1", "Olistdetil2", "Olistdetil3", "Olistdetil4"
      };

      string sql = string.Empty;
      sql = string.Format(template_sql, "SSAPP", "SSAPPMENU");
      List<IDataControl> list = BaseDataAdapter.GetListDC(this, sql, fields);
      if (list.Count == 0)
      {
        sql = string.Format(template_sql, "SSAPP", "SS01APPMENU");
        list = BaseDataAdapter.GetListDC(this, sql, fields);
      }
      List<SsappmenuControl> ListData = new List<SsappmenuControl>(); 
      foreach (SsappmenuControl dc in list)
      {
        ListData.Add(dc);
      }
      return ListData;
    }
    #endregion Methods 
  }
  #endregion Ss00appmenu
}

