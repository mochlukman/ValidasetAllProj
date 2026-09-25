using CoreNET.Common.Base;
using System.Collections.Generic;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.ParamControl, CoreNET.Common.BO
  public class ParamControl : BaseBO
  {
    #region Properties 
    public string Kdpar { get; set; }
    public string Nmpar { get; set; }
    #endregion Properties 

    public static List<ParamControl> GetListURLS()
    {
      List<ParamControl> list = new List<ParamControl>
      {
        new ParamControl() { Kdpar = "Page/PageTabular.aspx?", Nmpar = "Page/PageTabular.aspx?" }
        ,new ParamControl() { Kdpar = "Page/PageForm.aspx?", Nmpar = "Page/PageForm.aspx?" }
        ,new ParamControl() { Kdpar = "Page/PageTreeGrid.aspx?", Nmpar = "Page/PageTreeGrid.aspx?" }
        ,new ParamControl() { Kdpar = "Page/PageTreeGridDetil.aspx?", Nmpar = "Page/PageTreeGridDetil.aspx?" }
        ,new ParamControl() { Kdpar = "Page/PageTreePanelDetil.aspx?", Nmpar = "Page/PageTreePanelDetil.aspx?" }
      };

      return list;
    }
    public static List<ParamControl> GetListOTypes()
    {
      List<ParamControl> list = new List<ParamControl>
      {
        new ParamControl() { Kdpar = "1", Nmpar = "Olist1" }
        ,new ParamControl() { Kdpar = "11", Nmpar = "Olistdetil1" }
        ,new ParamControl() { Kdpar = "12", Nmpar = "Olistdetil2" }
        ,new ParamControl() { Kdpar = "13", Nmpar = "Olistdetil3" }
        ,new ParamControl() { Kdpar = "14", Nmpar = "Olistdetil4" }
      };

      return list;
    }
    public static List<ParamControl> GetListTypes()
    {
      List<ParamControl> list = new List<ParamControl>
      {
        new ParamControl() { Kdpar = "int", Nmpar = "int" }
        ,new ParamControl() { Kdpar = "bool", Nmpar = "bool" }
        ,new ParamControl() { Kdpar = "string", Nmpar = "string" }
        ,new ParamControl() { Kdpar = "decimal", Nmpar = "decimal" }
        ,new ParamControl() { Kdpar = "date", Nmpar = "date" }
      };

      return list;
    }
    public static List<ParamControl> GetListAlign()
    {
      List<ParamControl> list = new List<ParamControl>
      {
        new ParamControl() { Kdpar = "left", Nmpar = "left" }
        ,new ParamControl() { Kdpar = "center", Nmpar = "center" }
        ,new ParamControl() { Kdpar = "right", Nmpar = "right" }
      };

      return list;
    }
  }
  #endregion Dmparam
}

