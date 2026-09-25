using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreNET.Common.BO;

namespace CoreNET.Common.Base
{
  public interface IWord : IPrintable
  {
    void Print(string template, string filename);
  }
  public interface IPrintable
  {
    List<RptPars> GetReports();
    string GetURLReport(int n);
    void Print(int n);
    Hashtable GetDocProperties();
  }

  public interface IEksporable
  {
    //di bawah ini deprecated atau pindahin ke ICsv (CsvUpload
    string GetFileName(int mode);
    string GetFilePath(int mode);
    string GetTemplateFilePath();
  }
}
