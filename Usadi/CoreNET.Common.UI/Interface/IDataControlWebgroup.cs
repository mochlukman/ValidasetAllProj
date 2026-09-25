using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreNET.Common.Base
{
  public interface IDataControlWebgroup : IDataControlUIEntry
  {
    string GetMenuTemplate();
    string GetGroupID();
    IDataControlUI GetObjTahun();
    IList GetListTahun();
    IList GetListData();
    IDataControlWebgroup GetSelectionObject(string groupid);
    string[] GetFieldValueTahun();
    string[] GetFieldValueGroup();
    void SetUserID(string userid);
    void SetGroupID(string groupid);
    void GetDefaultUrl(out string url, out string roleid);
  }
}
