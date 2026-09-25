using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreNET.Common.Base
{
  public interface IReferences
  {
    List<FileReferences> GetListReferensi();
  }

  public class FileReferences
  {
    #region Property FileName
    private string _FileName;
    public string FileName
    {
      get { return _FileName; }
      set { _FileName = value; }
    }
    #endregion
    #region Property Size
    private long _Size;
    public long Size 
    { 
      get {return _Size; }
      set {_Size = value; }       
    }
    #endregion
    #region Property CreationTime
    private DateTime _CreationTime;
    public DateTime CreationTime 
    { 
      get {return _CreationTime; }
      set {_CreationTime = value; }       
    }
    #endregion      
    #region Property LastAccessTime
    private DateTime _LastAccessTime;
    public DateTime LastAccessTime 
    { 
      get {return _LastAccessTime; }
      set {_LastAccessTime = value; }       
    }
    #endregion      
    #region Property Url
    private string _Url;
    public string Url 
    { 
      get {return _Url; }
      set {_Url = value; }       
    }
    #endregion      
  }
}
