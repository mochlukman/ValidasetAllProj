using System.Collections.Generic;

namespace CoreNET.Common.BO
{
  public class DashboardDataObject
  {
    public bool EnableURL { get; set; }
    public string Title { get; set; }
    public object Value { get; set; }
    public decimal Percent { get; set; }

    #region Property Percentstr
    private string _Percentstr;
    public string Percentstr
    {
      get
      {
        if (!string.IsNullOrEmpty(_Percentstr))
        {
          return _Percentstr;
        }
        else
        {
          return Percent.ToString("0.00") + "%";
        }
      }
      set => _Percentstr = value;
    }
    #endregion
    public string Subtitle { get; set; }

    private string _URL = string.Empty;

    public DashboardDataObject(bool enableURL)
    {
      EnableURL = enableURL;
    }
    public string URL
    {
      get => EnableURL ? _URL : string.Empty;
      set => _URL = value;
    }

    public const string OnClickTemplate = "parent.loadPageByID(\"Pages\",\"{0}\",\"{0}\",\"{1}\");";
    public void SetURL(string url)
    {
      string tempurl = string.Empty;
      tempurl = string.Format(OnClickTemplate, Title, url);
      URL = tempurl;
    }
  }

  public class DashboardTableObject
  {
    public string Title;
    public string[] Columns;
    public List<DashboardDataObject> Data;
    public string URLTitle;
    public string URL;
  }
}
