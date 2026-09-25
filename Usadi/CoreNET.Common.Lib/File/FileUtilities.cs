using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;

namespace CoreNET.Common.Base
{
  public class FileUtilities
  {
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

  }
}
