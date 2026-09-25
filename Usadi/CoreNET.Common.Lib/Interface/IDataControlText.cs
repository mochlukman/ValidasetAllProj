using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreNET.Common.Base
{
  public interface IDataControlText : IDataControl
  {
    string GetText(int mode);
  }
}
