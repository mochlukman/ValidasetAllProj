using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreNET.Common.Base
{
  public interface IPageMaster
  {
    Dictionary<string, object> GetPageMasterProperties();
  }
  public class PageMasterProperties
  {
    public const string TITLE = "Title";
    public const string URLMASTER = "URLMaster";
    public const string URLDETILS = "URLDetils";
    public const string MODE = "Mode";

    public const string MODE_TOP_DOWN = "1";
    public const string MODE_LEFT_RIGHT = "2";

    public static int GetNChilds(IPageMaster dc)
    {
      int nchild = 1;
      if (typeof(string[]).IsInstanceOfType(dc.GetPageMasterProperties()[URLDETILS]))
      {
        nchild = ((string[])dc.GetPageMasterProperties()[URLDETILS]).Length;
      }
      return nchild;
    }
  }
}
