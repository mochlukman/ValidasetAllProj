using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CoreNET.Common.Base
{
  public class BaseUserControl : System.Web.UI.UserControl
  {
    public virtual List<string> ControlsToDestroy
    {
      get
      {
        // we should return none lazy controls only because lazy controls will be autodestroyed by parent container
        return new List<string>();
      }
    }
  }
}
