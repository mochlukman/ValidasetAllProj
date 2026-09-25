using System;
using System.Web;

namespace CoreNET.Common.Base
{
  public class ConstantDictUI : ConstantDict
  {
    public static string TranslateInfo(string methname)
    {
      if ((HttpContext.Current.Request != null) && (HttpContext.Current.Request.UrlReferrer != null))
      {
        string[] url = HttpContext.Current.Request.UrlReferrer.Segments;
        string keyword = "INFO_" + url[url.Length - 1] + methname;
        return Translate(keyword.Replace(".", "_"));
      }
      else
      {
        return "INFO_" + methname;
      }
    }
    public static string TranslateException(Exception ex, string methname)
    {
      //UtilityBO.Log(dc, ex);
      if (MasterAppConstants.Instance.StatusTesting)
      {
        if (ex != null)
        {
          Exception error = ex;

          string message = string.Empty;

          string stacktrace = ex.StackTrace;
          string[] msgs = stacktrace.Split(new string[] { " at", "at " }, StringSplitOptions.RemoveEmptyEntries);
          for (int i = msgs.Length - 1; i > 0; i--)
          {
            //if (true)
            if (!message.Contains(msgs[i]))
            {
              string[] msgsj = msgs[i].Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
              for (int j = 0; j < msgsj.Length; j++)
              {
                if (msgsj[j].Contains("line"))
                {
                  message += "- " + msgsj[j].Trim();
                }
              }
              message += "<br/> ";// + msgs[i].Trim();
            }
          }

          string message2 = "<br/> because :<br/>";
          while (error != null)
          {
            if (!message2.Contains(error.Message))
            {
              message2 += error.Message + "<br/>\n";
            }
            error = error.InnerException;
            if (error != null && error.InnerException != null)
            {
              message2 += "<br/>because :<br/>";
            }
          }//*/
          msgs = message2.Split(new string[] { "In:" }, StringSplitOptions.RemoveEmptyEntries);
          message += msgs[0];

          if (msgs.Length > 1)
          {
            msgs = msgs[1].Split(new string[] { " at", "at " }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = msgs.Length - 1; i >= 0; i--)
            {
              if (msgs[i].Contains("line"))
              {
                message += " at " + msgs[i].Trim();
              }
            }
          }
          return "[Mode Testing] : The error occured in <br/>" + message;
        }
        else
        {
          if ((HttpContext.Current.Request != null) && (HttpContext.Current.Request.UrlReferrer != null))
          {
            string[] url = HttpContext.Current.Request.UrlReferrer.Segments;
            string keyword = "ERROR_" + url[url.Length - 1] + methname;
            return Translate(keyword.Replace(".", "_"));
          }
          else
          {
            string keyword = "ERROR_" + methname;
            return Translate(keyword.Replace(".", "_"));
          }

        }
      }
      else
      {
        Exception error = ex;
        while (error.InnerException != null)
        {
          error = error.InnerException;
        }
        string message = error.Message;
        string keyword = message;
        return Translate(keyword.Replace(".", "_"));
      }
    }
  }

}
