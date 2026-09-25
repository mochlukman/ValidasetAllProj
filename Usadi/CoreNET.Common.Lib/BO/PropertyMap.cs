using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.PropertyMapControl, CoreNET.Common.BO
  public class PropertyMapControl : BaseBO
  {
    #region Properties 
    public string Propname { get; set; }
    public string Propmap { get; set; }
    public object Value { get; set; }
    #endregion Properties 

    public string GetColumnName()
    {
      return string.Format("{0}={1}", Propname, Propmap);
    }
  }
  #endregion Dmparam
}

