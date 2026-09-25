using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CoreNET.Common.BO;

namespace CoreNET.Common.Base
{
  public class ConstantDictExt : ConstantDictUI
  {

    public static string TranslateTabTip(Ext.Net.Component c)
    {
      string text = Translate("TIPS_" + c.ID);
      if (c.ID.Equals(text))
      {
        return (string)c.GetType().GetProperty("Text").GetValue(c, null);
      }
      else
      {
        return text;
      }
    }
  }

}
