using System;
using System.Collections;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
//using CoreNET.Common.UI;

namespace CoreNET.Common.Base
{
  /// <summary>
  /// 
  /// </summary>
  public interface IDataControlUI : IDataControl
  {
    string GetDefaultDebug();
    IProperties GetProperties();
    DataControlFieldCollection GetColumns();
    void SetColumnsNull();
    HashTableofParameterRow GetFilters();
    void FilterClick(string key);
    void SetFilterKey(BaseBO bo);
    string[] GetReadOnlyFields(int status);
  }

  /// <summary>
  /// This interface will allow user to entri new data
  /// </summary>
  public interface IDataControlUIEntry : IDataControlUI
  {
    HashTableofParameterRow GetEntries();
    string GetDestinationFile(string filename);
    string GetURLFile(string filename, string versi);
    void AfterInsert();
    void AfterUpdate();
    void AfterDelete();
    Exception ValidateTotal();
    void SetEditable(bool editable);
    bool IsEditable();
  }
  /// <summary>
  /// This interface will allow user to entri new data
  /// </summary>
  public interface IDataControlLookup : IDataControlUIEntry
  {
    ParameterRow GetLookupParameterRow(IDataControl callerCtr, bool entry);
    string GetFieldValueMap();
  }
}
