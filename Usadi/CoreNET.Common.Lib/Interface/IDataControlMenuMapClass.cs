using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreNET.Common.Base
{
  public interface IDataControlMenuMapClass : IDataControl
  {
    void LoadByRoleID(string roleid);
    void LoadLookupByRoleID(string roleid);
    string GetOList1();
    string GetOListDetil1();
    string GetOListDetil2();
    string GetOListDetil3();
    string GetOListDetil4();
    string GetLookupOList1();
    string GetLookupOListDetil1();
    string GetLookupOListDetil2();
    string GetLookupOListDetil3();
    string GetLookupOListDetil4();
  }
}
