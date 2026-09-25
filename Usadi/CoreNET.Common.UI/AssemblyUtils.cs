using CoreNET.Common.BO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace CoreNET.Common.Base
{
  public class AssemblyUtils
  {
    public static bool ProfilingActive
    {
      get
      {
        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["IsProfiling"]))
        {
          return ConfigurationManager.AppSettings["IsProfiling"].Equals("1");
        }
        else
        {
          return false;
        }
      }
    }
    //Assembly.GetManifestResourceNames Method.
    public static string ReadFile(string resourceName)
    {
      Assembly assembly = Assembly.GetExecutingAssembly();
      return ReadFile(resourceName, assembly);
    }
    public static string ReadFile(string resourceName, Assembly assembly)
    {
      string result = string.Empty;
      //var resourceName = "CoreNET.Common.BO.Js.page.js";

      using (Stream stream = assembly.GetManifestResourceStream(resourceName))
      using (StreamReader reader = new StreamReader(stream))
      {
        result = reader.ReadToEnd();
      }

      return result;
    }

    public static void WriteToFileScript(string line)
    {
      string dir = "Content\\js\\";
      Directory.CreateDirectory(dir);
      WriteToFile(line, dir + "corenet.js");
    }
    public static void WriteToFile(string line, string fname)
    {
      using (StreamWriter sw = new StreamWriter(fname))
      {
        sw.WriteLine(line);
      }
    }

    public static void WriteBeginLog()
    {
      WriteBeginLog(false, 2);
    }
    public static void WriteBeginLog(bool StartApp, int level)
    {
      if (MasterAppConstants.Instance.StatusTesting)
      {
        string mthodname = string.Empty;

        mthodname = UtilityBO.GetCurrentMethod(level);
        LogUtils.Start = DateTime.Now;
        string fname = UtilityBO.Path + string.Format(mthodname + LogUtils.Start.ToString("yyyyMMddHHmmss") + ".txt");
        if (!StartApp)
        {
          fname = string.Format(LogUtils.LogFileName, mthodname);
        }

        if (ProfilingActive)
        {
          try
          {
            using (StreamWriter sw1 = new StreamWriter(fname, true))
            {
              if (StartApp)
              {
                sw1.WriteLine(string.Format("Method {0} begin at {1}"
                  , mthodname, LogUtils.Start.ToString("dd/MM/yyyy HH:mm:ss")));
              }
              else
              {
                string type = string.Empty;
                if (GlobalAsp.CekSession())
                {
                  IDataControl dc = null;
                  try
                  {
                    dc = UtilityUI.GetDataControl(1);
                  }
                  catch (Exception)
                  {
                    dc = GlobalAsp.GetSessionUser();
                  }
                  type = UtilityBO.GetClassLibName(dc);
                }
                else
                {
                  type = "StartPage";
                }
                string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
                sw1.WriteLine(string.Format("Method {0} begin at {1} for {2}.{3}"
                  , mthodname, LogUtils.Start.ToString("dd/MM/yyyy HH:mm:ss"), url, type));
              }
            }
          }
          catch (Exception)
          {

          }
        }
      }
    }
    public static void WriteEndLog()
    {
      WriteEndLog(false, 2);
    }
    public static void WriteEndLog(bool StartApp, int level)
    {
      string mthodname = null;

      mthodname = UtilityBO.GetCurrentMethod(level);
      string fname = UtilityBO.Path + string.Format(mthodname + LogUtils.Start.ToString("yyyyMMddHHmmss") + ".txt");
      if (!StartApp)
      {
        fname = string.Format(LogUtils.LogFileName, mthodname);
      }

      DateTime now = DateTime.Now;
      TimeSpan sub = now.Subtract(LogUtils.Start);
      if (ProfilingActive)
      {
        try
        {
          using (StreamWriter sw1 = new StreamWriter(fname, true))
          {
            sw1.WriteLine(string.Format("Method {0} finished at {1} for {2} ms", mthodname, now.ToString("dd/MM/yyyy HH:mm:ss"),
              sub.Milliseconds));

            if (!StartApp)
            {
              try
              {
                IDataControlUI dc = UtilityUI.GetDataControl(1);
                if (dc != null)
                {
                  //Jalau Error saja, jadi di log error aja writenya
                  //sw1.WriteLine((string)dc.GetValue("Debug"));
                }
              }
              catch (Exception)
              {

              }
            }
          }
        }catch(Exception)
        {

        }
      }
    }

    public static void WriteToAnyFile(Assembly assembly, string resourceName, string fname)
    {
      Stream stream = assembly.GetManifestResourceStream(resourceName);
      FileStream fileStream = new FileStream(fname, FileMode.CreateNew, FileAccess.ReadWrite);
      for (int i = 0; i < stream.Length; i++)
      {
        fileStream.WriteByte((byte)stream.ReadByte());
      }

      fileStream.Close();
    }
    public static List<Type> LoadTypes(string assemblyPath)
    {
      Assembly assembly = Assembly.LoadFile(assemblyPath);
      Module[] Moduls = assembly.GetModules();
      List<Type> List = new List<Type>();
      foreach (Module m in Moduls)
      {
        Type[] Types = new Type[] { };
        try
        {
          Types = m.GetTypes();
        }
        catch (Exception ex)
        {
          //Types = m.FindTypes(Module.FilterTypeNameIgnoreCase, "*");
          UtilityBO.Log(ex);
        }
        List.AddRange(Types);
      }
      return List;
    }
    public static List<Type> LoadTypes(string[] assemblyPaths)
    {
      List<Type> List = new List<Type>();
      foreach (string path in assemblyPaths)
      {
        List<Type> Types = LoadTypes(path);
        List.AddRange(Types);
      }
      return List;
    }
  }
}
