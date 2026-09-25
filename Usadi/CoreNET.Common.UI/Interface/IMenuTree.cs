using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace CoreNET.Common.Base
{
  public interface IMenuTree
  {
    IDataControl GetObjMenuTree();
    IList GetMenuTree();
    string GetLookedupProperty();
  }
}
