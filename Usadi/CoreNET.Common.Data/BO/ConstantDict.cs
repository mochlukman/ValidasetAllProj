using System;
using System.Collections;

namespace CoreNET.Common.Base
{
  public class ConstantDict : Hashtable
  {
    public const string ADD = "Add";
    public const string EDIT = "Edit";
    public const string DELETE = "Delete";
    public const string VIEW = "View";
    public const string SPACE = " ";

    private static ConstantDict _Instance1 = null;
    private static ConstantDict _Instance2 = null;
    private static ConstantDict _Instance3 = null;
    private static ConstantDict Instance //Lebih efisien
    {
      get
      {
        if (ConstantDict.IDLOCALE == ConstantDict.ID)
        {
          if (_Instance1 == null)
          {
            _Instance1 = new ID_Dictionary();
          }
          return _Instance1;
        }
        else if (ConstantDict.IDLOCALE == ConstantDict.EN)
        {
          if (_Instance2 == null)
          {
            _Instance2 = new EN_Dictionary();
          }
          return _Instance2;
        }
        else
        {
          if (_Instance3 == null)
          {
            _Instance3 = new GE_Dictionary();
          }
          return _Instance3;
        }
      }
    }
    public static void SetInstanceNullForReloadDict()
    {
      if (MasterAppConstants.Instance.StatusTesting)
      {
        _Instance1 = null;
        _Instance2 = null;
        _Instance3 = null;
        ConstantDict.Translate(Instance.GetType().Name);
      }
    }
    private static string GetValue(string keyword)
    {
      if (string.IsNullOrEmpty(keyword))
      {
        return keyword;
      }
      string[] strs = keyword.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
      if (!MasterAppConstants.Instance.ShowTranslatedLabel)
      {
        return strs[0];
      }
      //if (keyword.Contains("icon")&& !keyword.ToLower().Contains("stricon"))
      //{
      //  return string.Empty;
      //}
      bool exist = Instance.ContainsKey(strs[0]);
      string translated = string.Empty;
      if (exist && (strs.Length == 1))
      {
        translated = (string)Instance[strs[0]];
      }
      else
      {
        if (strs.Length > 1)
        {
          translated = strs[1];
        }
        if (MasterAppConstants.Instance.StatusTesting)
        {
          ProcessNewEntry(strs[0], translated);
        }
      }
      if (string.IsNullOrEmpty(translated))
      {
        if (!strs[0].ToLower().Contains("status") && !strs[0].ToLower().Contains("icon"))
        {
          translated = strs[0];
        }
      }
      return translated;
    }
    public const int ID = 1;
    public const int EN = 2;
    public const int GE = 3;
    public static int IDLOCALE = ID;
    public static void SetLocale(string config)
    {
      if (string.IsNullOrEmpty(config))
      {
        IDLOCALE = ID;
      }
      else
      {
        IDLOCALE = int.Parse(config);
      }

    }
    public static string GetMsg(string keyword)
    {
      return Translate(keyword);
    }
    public static string Translate(string keyword)
    {
      string[] strs = keyword.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
      if (strs.Length <= 1)
      {
        return GetValue(keyword);
      }
      else
      {
        try
        {
          if (!string.IsNullOrEmpty(strs[0]))
          {
            string pname = (strs[0].EndsWith("str") ? strs[0].Replace("str", string.Empty) : strs[0]);
            return GetValue(pname + "=" + strs[1]);
          }
          else
          {
            return keyword;
          }
        }
        catch (Exception ex)
        {
          throw new Exception(string.Format("Error when translating word {0}", keyword), ex);
        }
      }
    }

    private static void ProcessNewEntry(string keyword, string word1)
    {
      try
      {
        #region Insert
        IDataControlDictionary ctr = (IDataControlDictionary)UtilityBO.Create(MasterAppConstants.Instance.DictionaryDC);
        ctr.InsertKeyword(keyword, word1);
        #endregion
      }
      catch (Exception ex)
      {
        UtilityBO.Log(ex);
      }
    }

    public static string GetColumnTitle(string coldata)
    {
      string pname = string.Empty;
      string title = string.Empty;
      if (coldata.Contains("="))
      {
        string[] strs = coldata.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
        pname = strs[0];
        string keyword = (pname.EndsWith("str") ? pname.Replace("str", string.Empty) : pname);
        title = GetValue(strs[1]);
      }
      else
      {
        pname = coldata;
        string keyword = (pname.EndsWith("str") ? pname.Replace("str", string.Empty) : pname);
        title = GetValue(keyword);
      }
      if (pname.Contains("icon") || pname.Contains("rating"))
      {
        return pname + "=";// "=<img style='width: 16px; height: 16px;' title='Status' src='../Res/Icons/buletijo.png'";
      }
      else
      {
        return pname + "=" + (string.IsNullOrEmpty(title) ? pname : title);
      }
    }
    public static string GetColumnTitleEntry(string coldata)
    {
      string title = GetValue(coldata + "Entry");
      return coldata + "=" + (string.IsNullOrEmpty(title) ? coldata : title);
    }
    public static string GetColumnTitle(string coldata, string sufix)
    {
      string title = GetValue(coldata + sufix);
      return coldata + "=" + (string.IsNullOrEmpty(title) ? coldata + sufix : title);
    }

  }

  public class EN_Dictionary : ConstantDict
  {

    public EN_Dictionary()
    {
      IDataControlDictionary ctr = (IDataControlDictionary)UtilityBO.Create(MasterAppConstants.Instance.DictionaryDC);
      IList list = ctr.View();
      for (int i = 0; i < list.Count; i++)
      {
        IDataControlDictionary temp = (IDataControlDictionary)list[i];
        Add(temp.GetValue(temp.GetKeyword()), temp.GetValue(temp.GetENWord()));
      }
    }
  }

  public class ID_Dictionary : ConstantDict
  {
    public ID_Dictionary()
    {
      //this.Add(ConstantDict.ADD, "Add");
      //this.Add(ConstantDict.EDIT, "Edit");
      //this.Add(ConstantDict.DELETE, "Delete");
      //this.Add(ConstantDict.VIEW, "Lihat");

       
      IDataControlDictionary ctr = (IDataControlDictionary)UtilityBO.Create(MasterAppConstants.Instance.DictionaryDC);
      IList list = ctr.View();
      if (list != null)
      {
        for (int i = 0; i < list.Count; i++)
        {
          IDataControlDictionary temp = (IDataControlDictionary)list[i];
          Add(temp.GetValue(temp.GetKeyword()), temp.GetValue(temp.GetIDWord()));
        }
      }
      
    }
  }

  public class GE_Dictionary : ConstantDict
  {
    public GE_Dictionary()
    {
      IDataControlDictionary ctr = (IDataControlDictionary)UtilityBO.Create(MasterAppConstants.Instance.DictionaryDC);
      IList list = ctr.View();
      if (list != null)
      {
        for (int i = 0; i < list.Count; i++)
        {
          IDataControlDictionary temp = (IDataControlDictionary)list[i];
          Add(temp.GetValue(temp.GetKeyword()), temp.GetValue(temp.GetGEWord()));
        }
      }
    }
  }
}
