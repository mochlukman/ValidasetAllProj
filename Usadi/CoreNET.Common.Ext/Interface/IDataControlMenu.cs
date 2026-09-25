using System;
using System.Collections;
using System.Linq;
using System.Web.UI;
using Ext.Net;

namespace CoreNET.Common.Base
{
  public interface IDataControlMenu : IDataControlTreeGrid3
  {
    /**
     * Set AppID inside this method
     */
    //void SetOtorisasiMenu(string userid, string idapp);//deprecated
    void SetOtorisasiMenu(Page page, string idapp);
    TreeNode CreateMenu(IList list, int typetree, bool withroot);
    IList GetListRef();
    Ext.Net.Menu GetMenu(IList list, short kdlevel);
    IDataControlMenu FindObject(string roleid);
    string GetRoleid();
    string GetURL();
    string GetURLReal();
    void GetDefaultURL(out string roleid, out string url);
    string GetAppTitle(string idapp);
    string GetAppName(string idapp);
  }
}
