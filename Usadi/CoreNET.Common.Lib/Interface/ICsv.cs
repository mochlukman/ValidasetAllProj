using System;
using System.Data;
using System.Collections;
using System.Linq;
using System.Text;

namespace CoreNET.Common.Base
{
  //public interface IBackupHTML
  //{
  //  void BackupHTML();
  //  void RestoreHTMLRow(string key);
  //}
  public interface ICsv : IDataControl
  {
    string[] GetCsvColumns(int mode);
    IList GetList(int id, int mode);
  }
  public interface ILoadCsv : ICsv
  {
    string[] GetLoadCsvColumns();
    void ExecutedPreSQL(string tname, bool isdelete);
    //void ExecutedPreSQL(string tname, string pk, string key, bool isdelete);
    void ExecutedSQL(string tname, System.Data.DataTable csvData, int counter, bool isdelete);
    //void ExecutedPostSQL(string tname, string pk, string key, bool isdelete);
    void ExecutedPostSQL(string tname, bool isdelete, string idmenu);
  }

  public class MyDocumentFormat
  {
    public const int DOC = 1;
    public const int CSV = 2;
  }
}
