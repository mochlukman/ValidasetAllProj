using CoreNET.Common.Base;
using System;

namespace CoreNET.Common.BO
{
  public class SsappmenuconfigAllControl : Ss01appmenuconfigControl
  {
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlAppuser).IsInstanceOfType(bo))
      {
        Last_by = ((IDataControlAppuser)bo).GetUserID();
      }
      else
      {
        try
        {
          string idmenu = GlobalAsp.GetRequestIdPrev();
          SsappmenuControl dc = new SsappmenuControl() { Idmenu = idmenu };
          dc = SsappmenuLookupControl.FindAndSetValuesInto(dc);
          Kdmenu = dc.Kdmenu;
          Nmmenu = dc.Nmmenu;

          IDataControl dcprev = GlobalAsp.GetEditingObject();
        }
        catch (Exception ex)
        {
          UtilityBO.Log(this, ex);
        }
      }
    }
  }
}
