using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreNET.Common.BO
{
  public class ParamValue
  {
    public string Param { get; set; }
    public object Value { get; set; }

    public ParamValue(string par, object val)
    {
      Param = par;
      Value = val;
    }
  }
}
