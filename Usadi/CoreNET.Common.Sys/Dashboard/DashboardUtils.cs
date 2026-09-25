using CoreNET.Common.Base;
using System.Collections.Generic;

namespace CoreNET.Common.BO
{
  public class DashboardUtils
  {
    public Dictionary<string, string> HTMLs = null;
    public DashboardUtils() { }
    public DashboardUtils(string mode, string val)
    {
      HTMLs = new Dictionary<string, string>();
      IDashboard helper = null;
      switch (val)
      {
        case "11": helper = new DashboardHelper(); break;
        //case "12": helper = new ValidasiHelper(); break;
        default: helper = new DashboardHelper(); break;
      }
      HTMLs[DataHelper.ROW_HEADER] = GetRowHeader(mode);
      HTMLs[DataHelper.ROW_1] = helper.GetRows1();
      HTMLs[DataHelper.ROW_2] = helper.GetRows2();
      HTMLs[DataHelper.ROW_3] = helper.GetRows3();
      HTMLs[DataHelper.ROW_4] = helper.GetRows4();
      HTMLs[DataHelper.ROW_5] = helper.GetRows5();
    }
    #region Shared Row
    private string GetRowHeader(string mode)
    {
      string html = string.Empty;
      BaseDataControlMenu menu = (BaseDataControlMenu)GlobalExt.GetSessionMenu();
      string title = "Dashboard";
      Dictionary<string, string> lefts = null;
      Dictionary<string, string> rights = null;

      switch (mode)
      {
        case "2":
          lefts = new Dictionary<string, string>()
          {
            { "Utama","Dashboard.aspx?mode=21"}
            ,{ "History","Dashboard.aspx?mode=22"}
            ,{ "Validasi Data(1)","Dashboard.aspx?mode=23"}
          };
          rights = new Dictionary<string, string>()
          {
            { "Settings","Setting.aspx"}
            ,{ "Tentang","About.aspx"}
          };
          html = DataHelper.GetRowHeader(title, lefts, rights);
          break;
        case "1":
        default:
          lefts = new Dictionary<string, string>()
          {
            { "Utama","Dashboard.aspx?mode=11"}
            ,{ "History","Dashboard.aspx?mode=12"}
            ,{ "Validasi Data(1)","Dashboard.aspx?mode=13"}
          };
          rights = new Dictionary<string, string>()
          {
            { "Settings","Setting.aspx"}
            ,{ "Tentang","About.aspx"}
          };
          html = DataHelper.GetRowHeader(title, lefts, rights);
          break;
      }
      return html;
    }
    #endregion
  }
}
