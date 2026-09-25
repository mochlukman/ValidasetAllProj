using System;
using System.Collections;
using System.Text;
using System.Reflection;
//using CoreNET.Common.UI;

namespace CoreNET.Common.Base
{
  public interface ITable
  {
    string GetTableName();
  }
  public interface IDataControl
  {
    void Insert();
    int Update();
    int Update(string label);
    int Delete();
    int Delete(string label);
    IList View();
    IList View(string label);
    BaseBO Load();
    string[] GetKeys();

    //BaseBO Load(String label);
    //IList ExecSP(String label);

    String[] GetFields();
    void SetPrimaryKey();
    void SetCopyRowKey();
    void SetPageKey();
    PropertyInfo GetProperty(string pname);
    Object GetValue(string pname);
    void SetValue(string pname,Object val);


    //int GetRowCount();
  }
}
