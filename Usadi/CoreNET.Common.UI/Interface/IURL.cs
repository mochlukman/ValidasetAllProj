using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreNET.Common.Base
{
  public interface IURL
  {
    string ConstructURL();
    string GetTitle();//Knp ngga property aja?
    string GetTabTip();//Knp ngga property aja?
  }
}
