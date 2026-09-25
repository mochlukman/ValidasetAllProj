using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;
using Ext.Net.Utilities;

namespace CoreNET.Common.Base
{
  public interface IHasMenuCommand
  {
    void SetMenuCommand(ColumnCollection column);
  }
}
