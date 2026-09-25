using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreNET.Common.Base
{
  public class UtilityUI
  {
    #region Refactor BaseDataControlUIEntry
    public static IDataControlUI Create(string fullclassname)
    {
      IDataControlUI obj = null;
      if (!string.IsNullOrEmpty(fullclassname))
      {
        Type T = Type.GetType(fullclassname);
        obj = Activator.CreateInstance(T) as IDataControlUI;
      }
      return obj;
    }
    public static void ResetEntry(IDataControlUI dc)
    {
      string[] fields = dc.GetFields();
      ArrayList array = new ArrayList();
      array.AddRange(((BaseDataControl)dc).GetNotFields());
      array.AddRange(((ViewListProperties)(dc.GetProperties())).ReadOnlyFields);
      for (int i = 0; i < fields.Length; i++)
      {
        string name = fields[i].Trim();
        if ((array == null) || !array.Contains(name))
        {
          PropertyInfo prop = dc.GetProperty(name);
          if (prop.CanWrite)
          {
            try
            {
              dc.SetValue(name, UtilityBO.GetDefault(prop.PropertyType));
            }
            catch (Exception ex)
            {
              UtilityBO.Log(dc, ex);
            }
          }
        }
      }
    }
    public static void ResetFields(IDataControlUI dc)
    {
      string[] fields = dc.GetFields();
      ArrayList array = new ArrayList();
      array.AddRange(((BaseDataControl)dc).GetNotFields());
      for (int i = 0; i < fields.Length; i++)
      {
        string name = fields[i].Trim();
        if ((array == null) || !array.Contains(name))
        {
          PropertyInfo prop = dc.GetProperty(name);
          if (prop.CanWrite)
          {
            dc.SetValue(name, UtilityBO.GetDefault(prop.PropertyType));
          }
        }
      }
    }
    public void CopyPropertyBOExcludeROFrom(IDataControlUI source_dc, IDataControlUI dest_dc)//Exclue ReadOnly
    {
      Dictionary<string, object> ro = new Dictionary<string, object>();
      for (int i = 0; i < ((ViewListProperties)dest_dc.GetProperties()).ReadOnlyFields.Length; i++)
      {
        string key = ((ViewListProperties)dest_dc.GetProperties()).ReadOnlyFields[i];
        ro.Add(key, dest_dc.GetValue(key));
      }
      ((BaseBO)dest_dc).CopyPropertyBOFrom((BaseBO)source_dc);
      for (int i = 0; i < ((ViewListProperties)dest_dc.GetProperties()).ReadOnlyFields.Length; i++)
      {
        string key = ((ViewListProperties)dest_dc.GetProperties()).ReadOnlyFields[i];
        dest_dc.SetValue(key, ro[key]);
      }
    }
    //protected void ResetFormEntries(string[] NotResetFields)
    //{
    //  ArrayList nrfields = new ArrayList(NotResetFields);
    //  ArrayList fields = new ArrayList();
    //  ArrayList readonlyfields = new ArrayList(((ViewListProperties)GetProperties()).ReadOnlyFields);
    //  HashTableofParameterRow hpars = GetEntries();
    //  foreach (ParameterRow pr in hpars.Values)
    //  {
    //    string[] temps = pr.Name.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
    //    foreach (string field in temps)
    //    {
    //      fields.Add(field);
    //    }
    //  }
    //  foreach (string field in fields)
    //  {
    //    if (readonlyfields.IndexOf(field) == -1)
    //    {
    //      if (nrfields.IndexOf(field) == -1)
    //      {
    //        PropertyInfo prop = GetProperty(field);
    //        SetValue(field, UtilityBO.GetDefault(prop.PropertyType));
    //      }
    //    }
    //  }
    //}
    public static string GetNoUrutByYear(IDataControl dc, string exp)
    {
      int length = 5;
      string tname = ((BaseBO)dc).XMLName;
      string sql = @"
        select top 1 [NO] as [NO] from {0} 
        where {1}
        order by [NO] desc
      ";
      sql = string.Format(sql, tname, exp);
      string[] fields = new string[] { "No" };
      List<IDataControl> list = BaseDataAdapter.GetListDC(dc, sql, fields);

      int n = 0;
      if (list.Count > 0)
      {
        n = int.Parse((string)list[0].GetValue("No"));
      }
      string strnourut = UtilityBO.IntToStr(++n, length);

      return strnourut;
    }

    /*
    private static void GetNoUrut(IDataControl dc, string field, string label)
    {
      int length = 2;
      string objname = dc.GetType().Name.Replace("Control", "");
      SysObjects obj = new SysObjects();
      ISysObjectsDao _dao = (ISysObjectsDao)MyDaoManager.Instance.GetDao(typeof(ISysObjectsDao), obj.ModeDB, obj); // GetFuctions.GetDaoManager().GetDao(typeof(ISysObjectsDao));
      obj.TableName = objname;
      obj.ColumnName = field;
      obj = (SysObjects)_dao.LoadBy(BaseDataControl.PK, obj, SQLDataSource.MODE_DB_OPERATIONAL);
      if (obj != null)
      {
        length = obj.Length;
      }
      GetNoUrut(dc, field, length, label, "", "");
    }
    private static void GetNoUrut(IDataControl dc, string field, int length)
    {
      GetNoUrut(dc, field, length, BaseDataControl.ALL, "", "");
    }
    private static void GetNoUrut(IDataControl dc, string field, int length, string prefix, string sufix)
    {
      GetNoUrut(dc, field, length, BaseDataControl.ALL, prefix, sufix);
    }
    private static string GetNoUrut(IDataControl dc, string field, int length, string label)
    {
      IList list = ((BaseDataControl)dc).View(label);
      return GetNoUrut(dc, list, field, length, label);
    }
    private static string GetNoUrut(IDataControl dc, IList list, string field, int length, string label)
    {
      //Cek Lagi
      int n = 0;
      if (list.Count > 0)
      {
        try
        {
          //mengambil nomor yang paling terakhir
          IDataControl tempdc = (IDataControl)list[list.Count - 1];
          Object val = tempdc.GetProperty(field).GetValue(tempdc, null);
          string num = ((string)val).Substring(0, length);
          n = int.Parse(num);
        }
        catch (Exception ex)
        {
          UtilityBO.Log(dc, ex);
          n = list.Count;
        }
      }


      string nourut = UtilityBO.IntToStr(n + 1, length);
      try
      {
        dc.GetProperty(field).SetValue(dc, nourut, null);
      }
      catch (Exception ex)
      {
        UtilityBO.Log(dc, ex);
      }
      return nourut;
    }
    //*/

    #region ok
    public static int GetNoUrut(IDataControl dc, string field, int length, string label, string prefix, string sufix)
    {
      long myLongValue = GetNoUrutLong(dc, field, length, label, prefix, sufix);
      int myIntValue = unchecked((int)myLongValue);
      return myIntValue;
    }
    public static long GetNoUrutLong(IDataControl dc, string field, int length, string label, string prefix, string sufix)
    {
      IList list = ((BaseDataControl)dc).View(label);
      long myLongValue = GetNoUrut(dc, field, length, list, prefix, sufix);
      return myLongValue;
    }
    public static int GetNoUrut(IDataControl dc, string sql, string[] fields, int length, string prefix, string sufix)
    {
      long myLongValue = GetNoUrutLong(dc, sql, fields, length, prefix, sufix);
      int myIntValue = unchecked((int)myLongValue);
      return myIntValue;
    }
    public static long GetNoUrutLong(IDataControl dc, string sql, string[] fields, int length, string prefix, string sufix)
    {
      List<IDataControl> list = BaseDataAdapter.GetListDC(dc, sql, fields);
      string field = fields[0];
      long myLongValue = GetNoUrut(dc, field, length, list, prefix, sufix);
      return myLongValue;
    }
    private static long GetNoUrut(IDataControl dc, string field, int length, IList list, string prefix, string sufix)
    {
      long n = 0;
      if (list.Count > 0)
      {
        try
        {
          Object val = null;
          if (typeof(IDataControl).IsInstanceOfType(list[list.Count - 1]))
          {
            IDataControl tempdc = (IDataControl)list[list.Count - 1];
            val = tempdc.GetProperty(field).GetValue(tempdc, null);
          }
          else if (typeof(Hashtable).IsInstanceOfType(list[list.Count - 1]))
          {
            Hashtable tempdc = (Hashtable)list[list.Count - 1];
            val = tempdc[field];
          }

          string num = ((string)val);
          try
          {
            if (prefix != "")
            {
              num = num.Substring(prefix.Length);//num.Replace(prefix, "");
            }
          }
          catch (Exception ex)
          {
            UtilityBO.Log(dc, ex);
          }
          try
          {
            if (sufix != "")
            {
              num = num.Substring(0, num.Length - sufix.Length);//num.Replace(sufix, "");
            }
          }
          catch (Exception ex)
          {
            UtilityBO.Log(dc, ex);
          }
          //length = (num.Trim().Length);//no override parameter
          n = long.Parse(num);
        }
        catch (Exception ex)
        {
          UtilityBO.Log(dc, ex);
          n = list.Count;
        }
      }
      long nourut = n + 1;
      string strnourut = UtilityBO.IntToStr(nourut, length);
      dc.GetProperty(field).SetValue(dc, prefix + strnourut + sufix, null);
      return nourut;
    }
    #endregion

    public static void SetNull(IDataControl dc, string[] fields)
    {
      for (int i = 0; i < fields.Length; i++)
      {
        Object value = dc.GetProperty(fields[i]).GetValue(dc, null);
        string strval = (value == null) ? "" : value.ToString().Trim();
        if ((strval == null) || (strval.Equals("")))
        {
          dc.GetProperty(fields[i]).SetValue(dc, null, null);
        }
      }
    }
    public static ListItemCollection GetSelectionItems(string[] fields)
    {
      ListItemCollection col = new ListItemCollection();
      for (int i = 1; i <= fields.Length; i++)
      {
        if (!fields[i - 1].Equals(""))
        {
          col.Add(new ListItem(fields[i - 1], i.ToString()));
        }
      }
      return col;
    }


    #endregion
    #region Deprecated
    #region Error Message
    public static void AddMessage(HttpContext page, Exception e)
    {
      AddMessage((Page)page.CurrentHandler, GetExceptionMessage(e));
    }
    public static void AddMessage(Page page, Exception e)
    {
      AddMessage(page, GetExceptionMessage(e));
    }
    public static void AddMessage(Page page, string msg)
    {
      string key = "msg01";
      string newmsg = msg.Replace("\r", "").Replace("\n", "").Replace("\"", "'");
      string script = "<script>alert(\"" + newmsg + "\")</script>";
      AddScriptOnPostBackRender((Page)HttpContext.Current.CurrentHandler, key, script);
    }
    public static void AddMessage(string msg)
    {
      string key = "msg01";
      string newmsg = msg.Replace("\r", "").Replace("\n", "").Replace("\"", "'");
      string script = "<script>alert(\"" + newmsg + "\")</script>";
      AddScriptOnPostBackRender((Page)HttpContext.Current.CurrentHandler, key, script);
    }
    #endregion

    #region Procedure Search Control #deprecated
    private static Control C = null;
    private static Control SearchControl(Control Page, String ID)
    {
      if (Page.Controls.Count > 0)
      {
        Boolean found = false;
        for (int i = 0; (i < Page.Controls.Count) && (!found); i++)
        {
          C = Page.Controls[i];
          if (C.ID != null)
          {
            found = C.ID.Equals(ID) || (C.ID.Contains(ID) && !C.ID.Contains("ulbl") && (C.ID.Length - 5 == ID.Length));
          }
        }

        if (found)
        {
          return C;
        }
        else
        {
          for (int i = 0; (i < Page.Controls.Count) && (!found); i++)
          {
            C = Page.Controls[i];
            C = SearchControl(C, ID);//recursive
            if (C != null)
            {
              found = C.ID.Equals(ID) || (C.ID.Contains(ID) && !C.ID.Contains("ulbl") && (C.ID.Length - 5 == ID.Length));
            }
          }
          if (found)
          {
            return C;
          }
        }
      }
      return null;
    }
    #endregion

    #region Private Method
    public static void SetValueForUI(Object ctrl, Object obj, PropertyInfo Prop)
    {
      Object value = null;
      if (Prop != null)
      {
        value = Prop.GetValue(obj, null);
      }
      Type type = Prop.PropertyType;
      SetValueForUI(ctrl, value, type);
    }
    /*Format value in object into string for UI component*/
    public static void SetValueForUI(Object ctrl, Object value, Type PropertyType)
    {
      if (value != null)
      {
        if (ctrl.GetType().GetProperty("Text") != null)
        {
          try
          {
            PropertyInfo PropUI = ctrl.GetType().GetProperty("Text");
            if (PropertyType == typeof(DateTime))
            {
              if ((DateTime)value == new DateTime())
              {
                PropUI.SetValue(ctrl, "", null);
              }
              else
              {
                PropUI.SetValue(ctrl, ((DateTime)value).ToString("dd/MM/yyyy").Trim(), null);
              }
            }
            else if (PropertyType == typeof(Double))
            {
              PropUI.SetValue(ctrl, ((Double)value).ToString("#,##0.00").Trim(), null);
            }
            else if (PropertyType == typeof(Decimal))
            {
              PropUI.SetValue(ctrl, ((Decimal)value).ToString("#,##0.00").Trim(), null);
            }
            else
            {
              PropUI.SetValue(ctrl, value.ToString().Trim(), null);
            }

          }
          catch (Exception ex)
          {
            throw ex;
          }
        }

        if (ctrl.GetType().GetProperty("SelectedDate") != null)
        {
          try
          {
            PropertyInfo PropUI = ctrl.GetType().GetProperty("SelectedDate");
            PropUI.SetValue(ctrl, value, null);
          }
          catch (Exception ex)
          {
            throw ex;
          }
        }
      }
    }
    //public static void SetValueForUI(Object ctrl, Object value, Type PropertyType)
    //{
    //  if (value != null)
    //  {
    //    if (ctrl.GetType().GetProperty("Text") != null)
    //    {
    //      try
    //      {
    //        PropertyInfo PropUI = ctrl.GetType().GetProperty("Text");
    //        if (PropertyType == typeof(DateTime))
    //        {
    //          PropUI.SetValue(ctrl, ((DateTime)value).ToString("dd/MM/yyyy").Trim(), null);
    //        }
    //        else if (PropertyType == typeof(Double))
    //        {
    //          PropUI.SetValue(ctrl, ((Double)value).ToString("#,##0.00").Trim(), null);
    //        }
    //        else if (PropertyType == typeof(Decimal))
    //        {
    //          PropUI.SetValue(ctrl, ((Decimal)value).ToString("#,##0.00").Trim(), null);
    //        }
    //        else
    //        {
    //          PropUI.SetValue(ctrl, value.ToString().Trim(), null);
    //        }

    //      }
    //      catch (Exception ex)
    //      {
    //        WindowDebug.ShowMessage(Page,ConstantDict.TranslateException(ex, MethodBase.GetCurrentMethod().Name));
    //      }
    //    }

    //    if (ctrl.GetType().GetProperty("SelectedDate") != null)
    //    {
    //      try
    //      {
    //        PropertyInfo PropUI = ctrl.GetType().GetProperty("SelectedDate");
    //        PropUI.SetValue(ctrl, value, null);
    //      }
    //      catch (Exception ex)
    //      {
    //        WindowDebug.ShowMessage(Page,ConstantDict.TranslateException(ex, MethodBase.GetCurrentMethod().Name));
    //      }
    //    }
    //  }
    //}
    public static void SetValueFromUI(Control ctrl, object obj, PropertyInfo Prop)
    {
      Object value = null;
      string uniqid = ctrl.GetType().GetProperty("UniqueID").GetValue(ctrl, null).ToString();
      value = HttpContext.Current.Request[uniqid];
      if ((value == null) || (value.Equals("")))
      {
        value = ctrl.GetType().GetProperty("Text").GetValue(ctrl, null);
      }
      if (!(value.Equals("") || (value == null)) || (Prop.PropertyType == typeof(string)))
      {
        try
        {
          if (Prop.PropertyType == typeof(int))
          {
            value = int.Parse(value.ToString());
          }
          else if (Prop.PropertyType == typeof(long))
          {
            value = long.Parse(value.ToString());
          }
          else if (Prop.PropertyType == typeof(double))
          {
            value = double.Parse(value.ToString());
          }
          else if (Prop.PropertyType == typeof(decimal))
          {
            value = decimal.Parse(value.ToString());
          }
          else if (Prop.PropertyType == typeof(DateTime))
          {
            value = DateTime.Parse(value.ToString());
          }
          else if (Prop.PropertyType == typeof(bool))
          {
            value = ((CheckBox)ctrl).Checked;
          }
        }
        catch (Exception ex)
        {
          value = null;
          throw ex;
        }
        try
        {
          ((BaseBO)obj).SetValue(Prop.Name, value);
        }
        catch (Exception ex)
        {
          UtilityBO.Log((IDataControl)obj, ex);
        }
      }
      else
      {
        value = UtilityBO.GetDefault(Prop.PropertyType);
        try
        {
          ((BaseBO)obj).SetValue(Prop.Name, value);
        }
        catch (Exception ex)
        {
          UtilityBO.Log((IDataControl)obj, ex);
        }
      }

    }
    public static string GetInnerExceptionMessage(Exception e)
    {
      Exception ex = e.InnerException;
      String str = e.Message;
      if (ex != null)
      {
        str = ex.Message;
      }
      return str;
    }
    public static string GetExceptionMessage(Exception e)
    {
      String str = ConstantDict.IDLOCALE == ConstantDict.EN ? "Undifined Error!" : "Error tidak terdefinisi!";
      if (e != null)
      {
        if (e.Message != null)
        {
          string pre = "";
          if ((e.TargetSite != null) && (false))
          {
            string strMethodName = e.TargetSite.Name;
            string strClassName = ((System.Reflection.MemberInfo)(e.TargetSite)).ReflectedType.Name;
            string strErrLineNo = e.StackTrace;
            pre = "MethodName: " + strMethodName + " | ClassName: " + strClassName + " | ErrLineNo: " + strErrLineNo;
            pre += "<br>Message: ";
          }

          str = pre + e.Message;
        }
        return GetExceptionMessage(str, e, str);
      }
      else
      {
        return str;
      }
    }
    public void logException(Exception ex)
    {

      string strMethodName = ex.TargetSite.Name;
      string strClassName = ((System.Reflection.MemberInfo)(ex.TargetSite)).ReflectedType.Name;
      string strErrLineNo = ex.StackTrace;
      StreamWriter srWriterObj = new StreamWriter(HttpContext.Current.Request.PhysicalApplicationPath + @"\Zlog\Exception.log", true)
      {
        NewLine = ("\r\n")
      };
      srWriterObj.WriteLine("Error: " + ex.Message + " on " + System.DateTime.Now.ToString());
      srWriterObj.NewLine = ("\r\n");
      srWriterObj.WriteLine("MethodName: " + strMethodName + " | ClassName: " + strClassName + " | ErrLineNo: " + strErrLineNo);
      srWriterObj.Flush();

      srWriterObj.Close();

    }
    private static string GetExceptionMessage(string pre, Exception e, string cmp)
    {
      Exception ex = e.InnerException;
      if (ex == null)
      {
        return pre;
      }
      else
      {

        if (cmp.Contains(ex.Message))
        {
          return pre;
        }
        else
        {
          int n;
          if (!int.TryParse(ex.Message, out n))
          {
            return GetExceptionMessage(pre + "<br>" + ex.Message, ex, ex.Message);
          }
          else
          {
            return pre;
          }
        }
      }

    }
    public static void AddScriptOnPostBackRender(Page page, string key, string script)
    {
      page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), key, script);
    }
    #endregion
    #endregion
    #region ParseEkspresi
    public static decimal ParseEkspresiToDecimal(string expresi)
    {
      return (decimal)ParseEkspresi(expresi);
    }
    public static double ParseEkspresi(string expresi)
    {
      double n = 0;
      try
      {
        string temp = expresi.Trim().ToLower().Replace(".", ",");
        string exp = "";
        for (int i = 0; i < temp.Length; i++)
        {
          char ch = temp[i];
          if (char.IsDigit(ch))
          {
            exp += ch;
          }
          else
          {
            switch (ch)
            {
              case '(':
              case ')':
              case '/':
              case '+':
              case '-':
              case '*':
              case '.':
              case ',':
              case 'x': exp += ch; break;
            }

          }
        }
        n = evaluate(ToSimpleExp(exp));
      }
      catch (Exception ex)
      {
        UtilityBO.Log(ex);
      }
      return n;
    }

    public static double evaluate(string s_exp)
    {
      string[] l_ops = new string[] { "+", "x", "*", "-", "/" };
      double N;

      MatchCollection mc = Regex.Matches(s_exp, "[-|+|x|*|/]");
      string[] ops = new string[mc.Count];
      for (int i = 0; i < ops.Length; i++)
      {
        ops[i] = mc[i].Value;
      }

      string[] nbrs = Regex.Split(s_exp, "[-|+|x|*|/]");

      double[] dbls = new double[nbrs.Length];
      for (int i = 0; i < dbls.Length; i++)
      {
        dbls[i] = double.Parse(nbrs[i]);
      }


      N = dbls[0];
      for (int i = 1; i < dbls.Length; i++)
      {
        switch (ops[i - 1])
        {
          case "+":
            N += dbls[i];
            break;
          case "*":
          case "x":
            N *= dbls[i];
            break;
          case "-":
            N -= dbls[i];
            break;
          case "/":
            N /= dbls[i];
            break;
        }
      }
      return N;
    }

    private static string ToSimpleExp(string exp)
    {
      if ((exp.IndexOf("(") == -1) && (exp.IndexOf(")") == -1))
      {
        return evaluate(exp).ToString();
      }
      else
      {
        MatchCollection mc = Regex.Matches(exp, "\\([0-9+x*-/]+\\)");
        for (int i = 0; i < mc.Count; i++)
        {
          string str = mc[i].Value;
          str = str.Substring(1, str.Length - 2);
          string nstr = ToSimpleExp(str);
          exp = exp.Replace(mc[i].Value, nstr);
        }
        return ToSimpleExp(exp);
      }
    }
    #endregion
    public static void ProcessInfo()
    {
      ProcessInfo(true);
    }
    public static void ProcessInfo(bool iscreatenew)
    {
      if (GlobalAsp.CekSession())
      {
        HttpContext page = HttpContext.Current;
        if ((HttpContext.Current.Request["back"] == "1") || (HttpContext.Current.Request["passdc"] == "1"))
        {
          return;
        }
        #region try to release memory by free session--Ternyata ngga diremove sejak kapan ya?tp bagus lah, biar ngga create terus2an
        Hashtable Sessions = new Hashtable();
        ////Sessions.Add(GlobalApp.SESSION_OBJECT_DEVELOPER_1, HttpContext.Current.Session[GlobalApp.SESSION_OBJECT_DEVELOPER_1]);
        ////Sessions.Add(GlobalApp.SESSION_CURRENT_OBJECT, HttpContext.Current.Session[GlobalApp.SESSION_CURRENT_OBJECT]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST1, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST1]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST2, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST2]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST3, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST3]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST4, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST4]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL1, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL1]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL2, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL2]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL3, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL3]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL4, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL4]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL2, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL2]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL3, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL3]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL4, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL4]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1a, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1a]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1b, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1b]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL2a, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL2a]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL2b, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL2b]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL2c, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL2c]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL3a, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL3a]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL3b, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL3b]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL3c, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL3c]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL4a, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL4a]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL4b, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL4b]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL4c, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL4c]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP1, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP1]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP2, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP2]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP3, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP3]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP4, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP4]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP1, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP1]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP2, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP2]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP3, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP3]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP4, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP4]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP2c, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP2c]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP3c, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP3c]);
        //Sessions.Add(GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP4c, HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP4c]);

        //Sessions.Add(GlobalAsp.SESSION_LIST_ROWS1, HttpContext.Current.Session[GlobalAsp.SESSION_LIST_ROWS1]);
        //Sessions.Add(GlobalAsp.SESSION_LIST_ROWS2, HttpContext.Current.Session[GlobalAsp.SESSION_LIST_ROWS2]);
        //Sessions.Add(GlobalAsp.SESSION_LIST_ROWS3, HttpContext.Current.Session[GlobalAsp.SESSION_LIST_ROWS3]);

        if (iscreatenew)
        {
          //for (IEnumerator en = Sessions.GetEnumerator(); en.MoveNext(); )//ada session yg jgn dihapus?testing lagi
          //{
          //  string key = (string)((DictionaryEntry)en.Current).Key;
          //  IDataControl dc = (IDataControl)((DictionaryEntry)en.Current).Value;
          //  HttpContext.Current.Session.Remove((string)key);
          //}
        }
        System.GC.Collect();//selalu dieksekusi
        #endregion

        /*replace id=menuid by id=roleid */
        string roleid = GlobalAsp.GetRequestId();
        //long nid = GlobalApp.GetRequestId();
        HttpContext.Current.Session[GlobalAsp.SESSION_DB] = HttpContext.Current.Session[GlobalAsp.SESSION_DB_CONFIG];

        #region Load Web Grid Control
        //try
        {
          string cname = (string)GlobalAsp.GetSessionAppValue(MasterAppConstants.CLASSDC);
          IDataControlMenuMapClass dc = (IDataControlMenuMapClass)UtilityBO.Create(cname);

          //string cname = MasterAppConstants.Instance.ClassDC;
          //IDataControlMenuMapClass dc = (IDataControlMenuMapClass)UtilityBO.Create(cname);
          try
          {
            if (!string.IsNullOrEmpty(GlobalAsp.GetRequestDc()))
            {
              //klo pake RequestDc sementara baru OList1
              string dcname = GlobalAsp.GetRequestDc();
              int i = GlobalAsp.GetRequestI();
              switch (i)
              {
                case 1: dc.SetValue("Olist1", dcname); break;
                case 11: dc.SetValue("Olistdetil1", dcname); break;
                case 12: dc.SetValue("Olistdetil2", dcname); break;
                case 13: dc.SetValue("Olistdetil3", dcname); break;
                case 14: dc.SetValue("Olistdetil4", dcname); break;
              }
            }
            else
            {
              dc.LoadByRoleID(roleid);
            }
          }
          catch (Exception ex)
          {
            string msgerror = string.Format(@"
              1.Cek apakah ada IDMENU null -> exec RevalidateSS01appmenu '', karena error={1}"
            , GlobalAsp.GetSessionApp(), ex.Message);
            throw new Exception(msgerror);
          }
          if (iscreatenew)
          {
            page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST1] = UtilityBO.Create(dc.GetOList1());
            if (page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST1] == null)
            {
              throw new Exception(MethodBase.GetCurrentMethod() +
                ": Error create object for Olist1=" + dc.GetOList1() + " line 813\n" +
                "If empty, cek double IDMENU = " + roleid);
            }
            page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL1] = UtilityBO.Create(dc.GetOListDetil1());
            page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL2] = UtilityBO.Create(dc.GetOListDetil2());
            page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL3] = UtilityBO.Create(dc.GetOListDetil3());
            page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL4] = UtilityBO.Create(dc.GetOListDetil4());
          }
          else
          {
            //IDataControlUI prevdc = GetDataControl(GlobalApp.GetRequestI(), false);
            //if (prevdc != null)
            //{
            //UtilityUI.ResetFields(prevdc);//bermasalah, tidak sama dengan create baru
            //}
            //page.Session[ConstantApp.SESSION_FORM_ENTRY] = gridc.Formentry;
            if (page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST1] == null)
            {
              page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST1] = UtilityBO.Create(dc.GetOList1());
            }
            if (page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL1] == null)
            {
              page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL1] = UtilityBO.Create(dc.GetOListDetil1());
            }
            if (page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL2] == null)
            {
              page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL2] = UtilityBO.Create(dc.GetOListDetil2());
            }
            if (page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL3] == null)
            {
              page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL3] = UtilityBO.Create(dc.GetOListDetil3());
            }
            if (page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL4] == null)
            {
              page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL4] = UtilityBO.Create(dc.GetOListDetil4());
            }
          }
        }
        //catch (Exception ex) 
        //{ 
        //  UtilityBO.Log(dc, ex);
        //}
        #endregion

        #region Load Web Lookup Control
        //try
        {
          string cname = (string)GlobalAsp.GetSessionAppValue(MasterAppConstants.LOOKUPCLASSDC);
          IDataControlMenuMapClass dc = (IDataControlMenuMapClass)UtilityBO.Create(cname);

          //string cname = MasterAppConstants.Instance.LookupClassDC;
          //IDataControlMenuMapClass dc = (IDataControlMenuMapClass)UtilityBO.Create(cname);
          //dc.LoadByMenuID(nid);
          dc.LoadLookupByRoleID(roleid);
          if (iscreatenew)
          {
            page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP1] = UtilityBO.Create(dc.GetLookupOList1());
            page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP1] = UtilityBO.Create(dc.GetLookupOListDetil1());
            page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP2] = UtilityBO.Create(dc.GetLookupOListDetil2());
            page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP3] = UtilityBO.Create(dc.GetLookupOListDetil3());
            page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP4] = UtilityBO.Create(dc.GetLookupOListDetil4());
          }
          else
          {
            if (page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP1] == null)
            {
              page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP1] = UtilityBO.Create(dc.GetLookupOList1());
            }
            if (page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP1] == null)
            {
              page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP1] = UtilityBO.Create(dc.GetLookupOListDetil1());
            }
            if (page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP2] == null)
            {
              page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP2] = UtilityBO.Create(dc.GetLookupOListDetil2());
            }
            if (page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP3] == null)
            {
              page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP3] = UtilityBO.Create(dc.GetLookupOListDetil3());
            }
            if (page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP4] == null)
            {
              page.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP4] = UtilityBO.Create(dc.GetLookupOListDetil4());
            }
          }
        }
        //catch (Exception ex)
        //{
        //  throw ex;
        //}
        #endregion

        #region Default
        //HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST] = HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST1];
        //HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP] = HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP1];
        //HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL] = HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL1];
        //HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP] = HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP1];
        //HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1] = HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1a];
        //HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL2] = HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1b];
        //HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL3] = HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1c];
        //HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL4] = HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1d];
        //HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL] = HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1];
        //HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP] = HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP1];
        #endregion

        HttpContext.Current.Session[GlobalAsp.SESSION_DB] = HttpContext.Current.Session[GlobalAsp.SESSION_DB_OPERATIONAL];
      }
    }
    public static int getLastNotesNumber(string strCmt)
    {
      string notesMarker = @"<div ID=""notes";
      if (string.IsNullOrEmpty(strCmt))
      {
        return 0;
      }

      int pos = strCmt.LastIndexOf(notesMarker) + notesMarker.Length;
      if ((pos >= 0) && (strCmt.Length > pos))
      {
        int postEnd = strCmt.IndexOf(" ", pos);
        string lastNoteNumberStr = strCmt.Substring(pos, postEnd - pos - 1);
        return int.Parse(lastNoteNumberStr);
      }
      return 0;
    }
    public static int GetMinDigit(decimal sum)
    {
      string str = sum.ToString("#").Trim();
      int i = str.Length - 1;
      int j = 0;
      while ((i >= 0) && (str[i--] == '0'))
      {
        j++;
      }

      if (j < 3)
      {
        int n = (int)(((Int64)sum) % 100);
        if (n % 25 == 0)
        {
          return j;
        }
        else
        {
          j = (n > 100) ? 3 : 2;
        }
      }
      return j;
    }
    public static Int64 Round(decimal sum, int digit)
    {
      Int64 tempnilai = (Int64)sum;
      int dig = tempnilai.ToString().Trim().Length - 1;
      if (dig < digit)
      {
        dig = dig - 1;
      }
      else
      {
        dig = digit;
      }
      Int64 mod = ((Int64)Math.Pow(10, dig));
      if (mod != 0)
      {
        tempnilai = tempnilai - (tempnilai % mod);
      }
      else
      {
        tempnilai = 0;
      }
      return tempnilai;
    }
    #region ExtPage
    public static string SESSION_PAGE => "session_extpage" + GlobalAsp.GetRequestId();
    public static string SESSION_MENUPAGE = "session_menupage";
    public static string SESSION_MENUPAGE_SUB = "session_menupage_SUB";
    public const int PAGE_UNDEFINED = 99;
    public const int PAGE_MASTER = 1;
    public const int PAGE_TABULAR = 2;
    public const int PAGE_TREE_GRID = 3;
    public const int PAGE_TREE_GRID_DETIL = 4;
    public const int PAGE_TREE_PANEL_DETIL = 5;
    public const int PAGE_TEXT_EDITOR = 6;
    public const int PAGE_FORM = 7;
    public const int PAGE_JURNAL = 8;
    public const int GRID_BALANCE = 9;
    public const int PAGE_MENU = 10;

    public const int MAIN_MENU = 20;

    public static void SetSessionExtPage(int modepage)
    {
      HttpContext.Current.Session[SESSION_PAGE] = modepage;
    }
    public static int GetSessionExtPage()
    {
      if (HttpContext.Current.Session[SESSION_PAGE] != null)
      {
        return (int)HttpContext.Current.Session[SESSION_PAGE];
      }
      else
      {
        return PAGE_UNDEFINED;
      }
    }
    public static int GetRequestExtPage()
    {
      string url = HttpContext.Current.Request.Url.ToString();
      return GetModePage(url);
    }
    public static int GetModePage()
    {
      string url = HttpContext.Current.Request.Url.ToString();
      return GetModePage(url);
    }
    public static bool IsModePageTree()
    {
      int mode = GetModePage();

      return (mode == PAGE_TREE_GRID) || (mode == PAGE_TREE_GRID_DETIL) || (mode == PAGE_TREE_PANEL_DETIL);
    }

    public static void SetSessionMenuPage(string url, string kdmenu, bool isroot)
    {
      if (isroot)
      {
        HttpContext.Current.Session[SESSION_MENUPAGE] = GetModePage(url);
      }
      else
      {
        HttpContext.Current.Session[SESSION_MENUPAGE_SUB] = GetModePage(url);
      }
    }

    public static int GetSessionMenuPage()
    {
      return GetSessionMenuPage(true);
    }
    public static int GetSessionMenuPage(bool isroot)
    {
      if (isroot)
      {
        return (int)HttpContext.Current.Session[SESSION_MENUPAGE];
      }
      else
      {
        return (int)HttpContext.Current.Session[SESSION_MENUPAGE_SUB];
      }
    }

    public static int GetModePage(string url)
    {
      if (url.Contains("PageMasterDetil.aspx"))
      {
        return PAGE_MASTER;
      }
      else if (url.Contains("PageTabular.aspx"))
      {
        return PAGE_TABULAR;
      }
      else if (url.Contains("PageTreeGrid.aspx"))
      {
        return PAGE_TREE_GRID;
      }
      else if (url.Contains("PageTreeGridDetil.aspx"))
      {
        return PAGE_TREE_GRID_DETIL;
      }
      else if (url.Contains("PageTreePanelDetil.aspx"))
      {
        return PAGE_TREE_PANEL_DETIL;
      }
      else if (url.Contains("TextEditor.aspx"))
      {
        return PAGE_TEXT_EDITOR;
      }
      else if (url.Contains("PageForm.aspx"))
      {
        return PAGE_FORM;
      }
      else if (url.Contains("PageMenu.aspx"))
      {
        return PAGE_MENU;
      }
      else if (url.Contains("PageJurnal.aspx"))
      {
        return PAGE_JURNAL;
      }
      else if (url.Contains("GridBalance.aspx"))
      {
        return GRID_BALANCE;
      }
      else if (url.Contains("MainMenu.aspx"))
      {
        return MAIN_MENU;
      }
      else
      {
        return PAGE_UNDEFINED;
      }
    }
    //public static IDataControlUI GetDataControl(string roleid, int i)
    //{
    //  //IDataControlUI cDataControl = (IDataControlUI)HttpContext.Current.Session["objlist1#" + GlobalAsp.GetSessionApp() + "-" + roleid + "."];
    //  IDataControlUI cDataControl = (IDataControlUI)HttpContext.Current.Session["objlist1#" + GlobalAsp.GetSessionApp() + "-" + roleid + "."];
    //  return cDataControl;
    //}
    public static IDataControlUI GetDataControl(int i)
    {
      return GetDataControl(i, true);
    }
    public static IDataControlUI GetDataControlPrev(int i)//GetDataControl(Request["idprev"])
    {
      GlobalAsp.IsGetPrev = true;//mutual conclusion?
      IDataControlUI dc = GetDataControl(i, false);
      GlobalAsp.IsGetPrev = false;
      return dc;
    }
    /*ToDo GANTI PAKE HASHTABLE PARAMS, biar ngga banyak IF*/
    public static IDataControlUI GetDataControl(int i, bool isthrowexceptionifnull)
    {
      if (GlobalAsp.CekSession())
      {
        IDataControlUI cDataControl = null;
        switch (i)
        {
          case 1: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST1]; break;
          case 2: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST2]; break;
          case 3: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST3]; break;
          case 4: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST4]; break;
          case 11: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL1]; break;
          case 12: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL2]; break;
          case 13: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL3]; break;
          case 14: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL4]; break;
          case 21: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL2]; break;
          case 31: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL3]; break;
          case 41: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL4]; break;
          case 111: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1a]; break;
          case 112: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1b]; break;
          case 113: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1c]; break;
          case 114: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1d]; break;
          case 121: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL2a]; break;
          case 122: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL2b]; break;
          case 123: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL2c]; break;
          case 124: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL2d]; break;
          case 131: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL3a]; break;
          case 132: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL3b]; break;
          case 133: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL3c]; break;
          case 134: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL3d]; break;
          case 141: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL4a]; break;
          case 142: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL4b]; break;
          case 143: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL4c]; break;
          case 144: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL4d]; break;
        }

        if ((cDataControl == null) && (isthrowexceptionifnull))
        {
          if (MasterAppConstants.Instance.StatusTesting)
          {
            throw new Exception("GetDataControl(" + i + ") error for ID " + GlobalAsp.GetRequestId() + @". 
              Cek class constructor atau method IDataControlMenu.LoadByRoleID(string idmenu) atau tabel SS01APPMENU");
          }
          else
          {
            throw new Exception("Mapping Controller Error for ID " + GlobalAsp.GetRequestId());
          }
        }
        if (cDataControl != null)
        {
          ((BaseBO)cDataControl).Index = i;
          return cDataControl;
        }
        else
        {
          return null;
        }
      }
      else
      {
        throw new Exception("LBL_SESSION_EXPIRED");
      }
    }
    public static void SetDataControl(int i, IDataControl dc)
    {

      switch (i)
      {
        case 1: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST1] = dc; break;
        case 2: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST2] = dc; break;
        case 3: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST3] = dc; break;
        case 4: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST4] = dc; break;
        case 11: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL1] = dc; break;
        case 12: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL2] = dc; break;
        case 13: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL3] = dc; break;
        case 14: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL4] = dc; break;
        case 21: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL2] = dc; break;
        case 31: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL3] = dc; break;
        case 41: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL4] = dc; break;
        case 111: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1a] = dc; break;
        case 112: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1b] = dc; break;
        case 113: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1c] = dc; break;
        case 114: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL1d] = dc; break;
        case 121: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL2a] = dc; break;
        case 122: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL2b] = dc; break;
        case 123: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL2c] = dc; break;
        case 124: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL2d] = dc; break;
        case 131: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL3a] = dc; break;
        case 132: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL3b] = dc; break;
        case 133: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL3c] = dc; break;
        case 134: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL3d] = dc; break;
        case 141: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL4a] = dc; break;
        case 142: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL4b] = dc; break;
        case 143: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL4c] = dc; break;
        case 144: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL4d] = dc; break;
      }
    }
    public static IDataControlUI GetDataControlLookup(int i)
    {
      IDataControlUI cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP1];
      switch (i)
      {
        case 1: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP1]; break;
        case 2: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP2]; break;
        case 3: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP3]; break;
        case 4: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP4]; break;
        case 11: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP1]; break;
        case 12: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP2]; break;
        case 13: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP3]; break;
        case 14: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP4]; break;
        case 111: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP1a]; break;
        case 112: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP1b]; break;
        case 113: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP1c]; break;
        case 114: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP1d]; break;
        case 121: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP2a]; break;
        case 122: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP2b]; break;
        case 123: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP2c]; break;
        case 124: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP2d]; break;
        case 131: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP3a]; break;
        case 132: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP3b]; break;
        case 133: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP3c]; break;
        case 134: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP3d]; break;
        case 141: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP4a]; break;
        case 142: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP4b]; break;
        case 143: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP4c]; break;
        case 144: cDataControl = (IDataControlUI)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP4d]; break;
      }
      if (cDataControl == null)
      {
        return null;
      }
      return cDataControl;
    }
    public static void SetDataControlLookup(int i, IDataControl dc)
    {

      switch (i)
      {
        case 1: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP1] = dc; break;
        case 2: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP2] = dc; break;
        case 3: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP3] = dc; break;
        case 4: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_LOOKUP4] = dc; break;
        case 11: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP1] = dc; break;
        case 12: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP2] = dc; break;
        case 13: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP3] = dc; break;
        case 14: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_LOOKUP4] = dc; break;
        case 111: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP1a] = dc; break;
        case 112: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP1b] = dc; break;
        case 113: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP1c] = dc; break;
        case 114: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP1d] = dc; break;
        case 121: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP2a] = dc; break;
        case 122: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP2b] = dc; break;
        case 123: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP2c] = dc; break;
        case 124: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP2d] = dc; break;
        case 131: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP3a] = dc; break;
        case 132: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP3b] = dc; break;
        case 133: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP3c] = dc; break;
        case 134: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP3d] = dc; break;
        case 141: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP4a] = dc; break;
        case 142: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP4b] = dc; break;
        case 143: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP4c] = dc; break;
        case 144: HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL_LOOKUP4d] = dc; break;
      }
    }
    public static int GetPageSize(IDataControlUI dc)
    {
      int size = ((ViewListProperties)dc.GetProperties()).PageSize;
      size = (((HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL] == null) &&
        (HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL] == null)) ? size :
        ((HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST_DETIL_DETIL] == null) ? 10 : 5));
      return size;
    }

    public static string ToQueryString(object foo)
    {
      //var foo = new EditListItemActionModel()
      //{
      //  Id = 1,
      //  State = 26,
      //  Prefix = "f",
      //  Index = "oo",
      //  ParentID = null
      //};

      IEnumerable<string> properties = from p in foo.GetType().GetProperties()
                                       where p.GetValue(foo, null) != null
                                       select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(foo, null).ToString());

      // queryString will be set to "Id=1&State=26&Prefix=f&Index=oo"                  
      string queryString = String.Join("&", properties.ToArray());

      return queryString;
    }
    #endregion


    public static System.Net.IPAddress GetIPAddress()
    {
      IPAddress ip = IPAddress.Parse("127.0.0.1");
      string strHostName = System.Net.Dns.GetHostName();
      IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
      IPAddress[] addr = ipEntry.AddressList;
      foreach (IPAddress ipaddr in addr)
      {
        if (ipaddr.AddressFamily.Equals(System.Net.Sockets.AddressFamily.InterNetwork))
        {
          ip = ipaddr;
          break;
        }
      }
      return ip;
    }
    /*
     * 
      *How to Use:
      *string IP = Request.UserHostName;
      *string compName = CompNameHelper.DetermineCompName(IP);
     * 
     */
    public static string DetermineCompName(string IP)
    {
      IPAddress myIP = IPAddress.Parse(IP);
      IPHostEntry GetIPHost = Dns.GetHostEntry(myIP);
      List<string> compName = GetIPHost.HostName.ToString().Split('.').ToList();
      return compName.First();
    }
    public static string GetClientCompIP()
    {
      string IP = HttpContext.Current.Request.UserHostName;
      return IP;
    }
    public static string GetClientCompName()
    {
      try
      {
        string IP = HttpContext.Current.Request.UserHostName;
        string compName = UtilityUI.DetermineCompName(IP);
        return compName;
      }
      catch (Exception)
      {
        return string.Empty;
      }
    }
  }

}
