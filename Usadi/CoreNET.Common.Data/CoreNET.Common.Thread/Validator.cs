using System;
using System.Collections;
using System.IO;
using System.Reflection;
using CoreNET.Common.BO;

namespace CoreNET.Common.Base
{
  public class Validator
  {

    private static Validator _Instance = null;
    public static Validator Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new Validator();
        }
        return _Instance;
      }
    }
    public static void ValidateCode()
    {

    }
    public void ScanDC(object o)
    {
      IDataControl dc = (IDataControl)o;
      try
      {
        IList list = dc.View();
        if (list != null)
        {
          //UtilityBO.Log(dc.GetType().Name, new Exception(string.Format("{0} success return {1} rows",
          //  dc.GetType().Name, list.Count)));
        }
        else
        {
          //UtilityBO.Log(dc.GetType().Name, new Exception(string.Format("{0} success return null"
          //  , dc.GetType().Name)));
        }
      }
      catch (Exception ex)
      {
        UtilityBO.Log(dc, ex);
      }
    }

    public void ScanView(Object obj)
    {
      Type type = (Type)obj;
      object o = null;
      if (type.IsClass && !type.IsAbstract)
      {
        IDataControl dc = null;
        try
        {
          o = Activator.CreateInstance(type);
          if (typeof(IDataControl).IsInstanceOfType(o))
          {
            dc = ((IDataControl)o);
            IList list = dc.View();
            if (list != null)
            {
              //UtilityBO.Log(type.Name, new Exception(string.Format("{0} success return {1} rows",
              //  type.Name, list.Count)));
            }
            else
            {
              //UtilityBO.Log(type.Name, new Exception(string.Format("{0} success return null"
              //  , type.Name)));
            }
          }
        }
        catch (Exception ex)
        {
          UtilityBO.Log(dc, ex);
        }
      }
    }
    public void ScanType(Object obj)
    {
      Type type = (Type)obj;
      string name = (type.ReflectedType != null) ? type.ReflectedType.FullName : type.FullName;
      #region Each Type
      string fname = string.Format(UtilityBO.Path + "{0}.txt", name);
      using (StreamWriter sw2 = new StreamWriter(fname))
      {
        //MemberInfo[] members = type.GetMembers();
        PropertyInfo[] props = type.GetProperties();
        MethodInfo[] methods = type.GetMethods();
        object o = null;
        if (type.IsClass && !type.IsAbstract)
        {
          try
          {
            o = Activator.CreateInstance(type);
            sw2.WriteLine(string.Format("{0} o = new {0}();", name));
            //if (type.FindMembers(MemberTypes.Method,
            //    BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance,
            //    new MemberFilter(DelegateToSearchCriteria), "GetProperties").Length > 0)

            if (typeof(IDataControlUI).IsInstanceOfType(o))
            {
              ViewListProperties vp = ((ViewListProperties)((IDataControlUI)o));
              sw2.WriteLine(string.Format("string[] psk = {0}", vp.PrimaryKeys));
            }
          }
          catch (Exception) { }
        }

        sw2.WriteLine("#region Properties");
        foreach (PropertyInfo prop in props)
        {
          string str = string.Format("{0} {1}"
            , prop.PropertyType.Name, prop.Name);
          sw2.WriteLine(str);
        }
        sw2.WriteLine();
        sw2.WriteLine("#endregion Properties");
        sw2.WriteLine();
        sw2.WriteLine("#region Methods");
        foreach (MethodInfo method in methods)
        {
          if (!method.Name.Contains("get_") && !method.Name.Contains("set_"))
          {
            string strpars = string.Empty;
            ParameterInfo[] pars = method.GetParameters();
            for (int i = 0; i < pars.Length; i++)
            {
              strpars += string.Format("{0} {1}", pars[i].ParameterType.Name, pars[i].Name);
              if (i < pars.Length - 1)
              {
                strpars += ",";
              }
            }
            string str = string.Format("{0} {1}({2})"
              , method.ReturnType.Name, method.Name, strpars);
            sw2.WriteLine(str);
          }
        }
        sw2.WriteLine("#endregion Methods");
      }
      #endregion Each Type
    }
    public static bool DelegateToSearchCriteria(MemberInfo objMemberInfo, Object objSearch)
    {
      // Compare the name of the member function with the filter criteria.
      if (objMemberInfo.Name.ToString() == objSearch.ToString())
      {
        return true;
      }
      else
      {
        return false;
      }
    }
  }
}
