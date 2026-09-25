using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreNET.Common.Base
{
  public interface IDataControlDictionary : IDataControl
  {
    void InsertKeyword(string keyword,string word1);
    string GetKeyword();
    string GetIDWord();
    string GetENWord();
    string GetGEWord();
  }
}
