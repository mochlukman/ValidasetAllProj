using CoreNET.Common.Base;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.Ss00appHelpControl, CoreNET.Common.BO
  /**
   * Struktur Menu dan Monitoring di Modul Testing
   * as DCMenu where Idapp='68065186-FF1B-4EB3-8A6B-6D1FDCFD3BAD'
   * */
  [Serializable]
  public class Ss00appHelpControl : Ss00appMonControl, IDataControlTreeGrid3, IHasJSScript
  {
    public Ss00appHelpControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APP;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.IDKey = "Idapp";//Mereset parent
      return cViewListProperties;
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (!string.IsNullOrEmpty(GlobalAsp.GetRequestVal()))
      {
        Idapp2 = GlobalAsp.GetRequestVal();
      }
    }
    public new HashTableofParameterRow GetEntries()
    {
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      hpars.Add(new ParameterRowHelp(this, true));
      return hpars;
    }
  }
  #endregion Ss00appLink
}

