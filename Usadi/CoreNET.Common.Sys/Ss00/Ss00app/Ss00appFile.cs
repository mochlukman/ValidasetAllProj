using CoreNET.Common.Base;
using System;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.Ss00appFileControl, CoreNET.Common.BO
  /**
   * Struktur Menu dan Monitoring di Modul Testing
   * as DCMenu where Idapp='68065186-FF1B-4EB3-8A6B-6D1FDCFD3BAD'
   * */
  [Serializable]
  public class Ss00appFileControl : Ss00appMonControl, IDataControlTreeGrid3, IHasJSScript
  {
    public Ss00appFileControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APP;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.IDKey = "Idapp";//Mereset parent
      return cViewListProperties;
    }
    public new HashTableofParameterRow GetEntries()
    {
      HashTableofParameterRow hpars = new HashTableofParameterRow
      {
        new ParameterRowUploadFile(this, true)
      };
      return hpars;
    }
  }
  #endregion Ss00appLink
}

