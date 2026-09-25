using System;
using System.Collections;
using System.Text;

namespace CoreNET.Common.Base
{
  public interface IBaseDao
  {
    void Insert(String label, BaseBO BO, int mode);
    int UpdateBy(String label, BaseBO BO, int mode);
    int DeleteBy(String label, BaseBO BO, int mode);
    BaseBO LoadBy(String label, BaseBO BO, int mode);
    IList QueryBy(String label, BaseBO BO, int mode);
    //IList QueryByHashtable(string label, Hashtable param, int mode);
    IList ExecSP(String label, BaseBO BO, int mode);

  }
}
