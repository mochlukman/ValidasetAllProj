using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;


namespace CoreNET.Common.Base
{
  public class UtilityBO
  {
    public static string GetFileVersion(string libname)
    {
      //FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
      //return fvi.FileVersion;

      FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(libname);
      string ver = fvi.FileVersion;
      return ver;
    }

    public static string GetBaseClassLibName(object dc)
    {
      Type type = dc.GetType().BaseType;
      return GetClassLibName(type);
    }
    public static string GetClassLibName(object dc)
    {
      Type type = dc.GetType();
      return GetClassLibName(type);
    }
    public static string GetClassLibName(Type type)
    {
      string[] strs = type.AssemblyQualifiedName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
      string typename = strs[0] + "," + strs[1];
      return typename;
    }
    public static void GetStackMethod(int level, long tick)
    {
      try
      {
        StackTrace st = new StackTrace();
        string msg = string.Empty;
        for (int i = 1; i <= level; i++)
        {
          StackFrame sf = st.GetFrame(i);
          msg += sf.ToString() + "\n";
        }
        msg += "\n" + tick + " milisecond\n\n";
        System.IO.File.AppendAllText("d:\\log.txt", msg);
      }
      catch (Exception ex)
      {
        UtilityBO.Log(ex);
      }
    }
    public static string Path = "D:/Files/Error/";

    public static void Log(Exception ex)
    {
      string methname = UtilityBO.GetCurrentMethod(1);
      Log(null, methname, string.Empty, ex);
    }
    public static void Log(IDataControl bo, Exception ex)
    {
      string methname = UtilityBO.GetCurrentMethod(1);
      if (bo != null)
      {
        Log(bo, methname, (string)bo.GetValue("Debug"), ex);
      }
      else
      {
        Log(bo, methname, string.Empty, ex);
      }
    }
    public static void Log(IDataControl bo, string methname, string debuginfo, Exception ex)
    {
      if (MasterAppConstants.Instance.LogException)
      {
        try
        {
          if (!Directory.Exists(UtilityBO.Path))
          {
            try
            {
              System.IO.Directory.CreateDirectory(UtilityBO.Path);
            }
            catch (Exception)
            {
              UtilityBO.Path = "C:/Windows/Temp/CoreNET/";
              System.IO.Directory.CreateDirectory(UtilityBO.Path);
            }
          }
          string fname = string.Format(methname + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");
          string pathfname = Path + fname;
          using (StreamWriter sw = new StreamWriter(pathfname))
          {
            sw.WriteLine(string.Format("Error = {0}", ex.Message));
            string InnerMsg = string.Empty;
            Exception innerEx = ex.InnerException;
            while (innerEx != null)
            {
              InnerMsg += innerEx.Message + "\n";
              innerEx = innerEx.InnerException;
            }
            sw.WriteLine(string.Format("Inner Error = {0} : ", InnerMsg));
            if (bo != null)
            {
              bo.SetValue("Debug", InnerMsg);
            }

            InnerMsg = string.Empty;
            innerEx = ex;
            while (innerEx != null)
            {
              InnerMsg += innerEx.StackTrace + "\n";
              innerEx = innerEx.InnerException;
            }
            if (bo != null)
            {
              bo.SetValue("Debug", InnerMsg);
            }
            sw.WriteLine(string.Format("Stack Trace = {0} : ", InnerMsg));
            sw.WriteLine(string.Format("Method Trace = {0} : ", GetMethodsInfo()));


            sw.WriteLine(string.Format("Debug Info = {0} : ", debuginfo));
          }
        }
        catch (Exception)
        {
        }
      }
      if (MasterAppConstants.Instance.StatusTesting)
      {
        //Kadang klo dithrow, ada prilakuk di UI yg ngga responsif, semua event mati
        //throw ex;
      }
    }
    public static string GetCurrentMethod()
    {
      return GetCurrentMethod(0);
    }
    public static string GetCurrentMethod(int uplevel)
    {
      //MethodBase.GetCurrentMethod().Name;
      StackTrace st = new StackTrace();
      StackFrame sf = st.GetFrame(uplevel + 1);
      MethodBase method = sf.GetMethod();
      return method.DeclaringType.Name + "." + method.Name;
    }
    public static string GetMethodsInfo()
    {
      string info = string.Empty;
      StackTrace st = new StackTrace();
      int uplevel = 2;
      StackFrame sf = st.GetFrame(uplevel);
      while (sf != null)
      {
        MethodBase method = sf.GetMethod();
        if (method.DeclaringType != null)
        {
          info += "=>" + method.DeclaringType.Name + "." + method.Name;
        }
        sf = st.GetFrame(++uplevel);
      }
      return info;
    }
    public static void SetValueForProperty(Object O, string propname, Object objvalue)
    {
      PropertyInfo Prop = O.GetType().GetProperty(propname);
      if (Prop == null)
      {
        return;
      }

      Object value = null;
      if (typeof(DBNull).IsInstanceOfType(objvalue))
      {
        if (Prop.CanWrite)
        {
          if (!Prop.PropertyType.Equals(typeof(DateTime)))
          {
            if (Prop.PropertyType.Equals(typeof(string)))
            {
              Prop.SetValue(O, string.Empty, null);
            }
            else
            {
              Prop.SetValue(O, null, null);
            }
          }
        }
      }
      else if (typeof(string).IsInstanceOfType(objvalue))
      {
        if (Prop.CanWrite)
        {
          string strValue = objvalue.ToString().Trim();
          if (Prop.PropertyType.Equals(typeof(decimal)))
          {
            strValue = strValue.Replace(".", "");
            strValue = strValue.Replace(",", ".");
          }
          SetValueForProperty(O, propname, (string)objvalue);
        }
      }
      else
      {
        try
        {
          if (Prop.CanWrite)
          {
            if (objvalue != null)
            {
              if (Prop.PropertyType == typeof(int))
              {
                value = Convert.ToInt32(objvalue);
                //value = (int)objvalue;
              }
              else if (Prop.PropertyType == typeof(short))
              {
                value = Convert.ToInt16(objvalue);
                //value = (short)objvalue;
              }
              else if (Prop.PropertyType == typeof(double))
              {
                value = Convert.ToDouble(objvalue);
              }
              else if (Prop.PropertyType == typeof(decimal))
              {
                value = Convert.ToDecimal(objvalue);
              }
              else if (Prop.PropertyType == typeof(DateTime))
              {
                value = (DateTime)objvalue;
              }
              else
              {
                value = objvalue;
              }
            }
            else
            {
              value = objvalue;
            }
            Prop.SetValue(O, value, null);
          }
        }
        catch (Exception ex)
        {
          ((IDataControl)O).SetValue("Debug", string.Format("Error casting on class '{0}' property '{1}', nilai='{2}'", O.GetType().FullName, propname, objvalue));
          UtilityBO.Log((IDataControl)O, ex);
          value = null;
        }
      }
    }
    public static void SetValueForProperty(object obj, string propname, string strvalue)
    {
      PropertyInfo Prop = obj.GetType().GetProperty(propname);
      object value = null;
      if (!(strvalue.Equals("") || (strvalue == null)) || (Prop.PropertyType == typeof(string)))
      {
        try
        {
          if (Prop.PropertyType == typeof(int))
          {
            value = int.Parse(strvalue);
          }
          else if (Prop.PropertyType == typeof(short))
          {
            value = short.Parse(strvalue);
          }
          else if (Prop.PropertyType == typeof(long))
          {
            value = long.Parse(strvalue);
          }
          else if (Prop.PropertyType == typeof(double))
          {
            value = double.Parse(strvalue);
          }
          else if (Prop.PropertyType == typeof(decimal))
          {
            //string temp = strvalue.Replace(",", ".");di Riskmanagement jadi error
            //dipindah di  SetValueForProperty(object obj, string propname, object strvalue)
            value = decimal.Parse(strvalue);
          }
          else if (Prop.PropertyType == typeof(DateTime))
          {
            try
            {
              value = DateTime.Parse(strvalue);
            }
            catch (Exception ex)
            {
              UtilityBO.Log(ex);
              try
              {
                value = DateTime.Parse(strvalue.Substring(1, 10));
              }
              catch (Exception ex1)
              {
                UtilityBO.Log(ex1);
                if (strvalue.Contains("-"))
                {
                  string[] temps = strvalue.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                  value = new DateTime(int.Parse(temps[0]), int.Parse(temps[1]), int.Parse(temps[2]));
                }
                else if (strvalue.Contains("/"))
                {
                  string[] temps = strvalue.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                  value = new DateTime(int.Parse(temps[0]), int.Parse(temps[1]), int.Parse(temps[2]));
                }
              }
            }
          }
          else if (Prop.PropertyType == typeof(bool))
          {
            value = bool.Parse(strvalue);
          }
          else
          {
            value = strvalue;
          }
        }
        catch (Exception ex)
        {
          value = null;
          throw ex;
        }
        try
        {
          Prop.SetValue(obj, value, null);
        }
        catch (Exception ex)
        {
          UtilityBO.Log(ex);
        }
      }
      else
      {
        value = GetDefault(Prop.PropertyType);
        try
        {
          Prop.SetValue(obj, value, null);
        }
        catch (Exception ex)
        {
          UtilityBO.Log(ex);
        }
      }

    }
    public static Object GetDefault(Type type)
    {
      if (type == typeof(int))
      {
        return 0;
      }
      else if (type == typeof(bool))
      {
        return false;
      }
      else if (type == typeof(decimal))
      {
        return new decimal(0);
      }
      else if (type == typeof(double))
      {
        return (double)0;
      }
      else if (type == typeof(DateTime))
      {
        return new DateTime();
      }
      else if (type == typeof(string))
      {
        return string.Empty;
      }
      else if (type == typeof(string[]))
      {
        return new string[] { };
      }
      else if (type == typeof(DBNull))
      {
        return DBNull.Value;
      }
      else
      {
        return Activator.CreateInstance(type); ;
      }
    }

    public static IDataControl LoadDataControl(string fname, string strtype)
    {
      IDataControl dc = null;
      Assembly assembly = Assembly.LoadFile(fname);
      foreach (Type type in assembly.GetTypes())
      {
        if (type.FullName.EndsWith(strtype))
        {
          dc = (IDataControl)Activator.CreateInstance(type);
        }
      }
      if (dc == null)
      {
        dc = (IDataControl)Activator.CreateInstance(Type.GetType("Valid49.BO." + strtype));
      }
      return dc;
    }

    public static string FindNearestMatch(ICollection keys, string key)
    {
      foreach (string k in keys)
      {
        if (k.Replace(" ", "").Replace("’", "").Replace("'", "").ToLower() == key.Replace(" ", "").Replace("’", "").Replace("'", "").ToLower())
        {
          return k;
        }
      }
      return null;
    }

    public static string GetParentCode(string kode)
    {
      try
      {
        if (!kode.Trim().EndsWith("."))
        {
          kode = kode.Trim() + ".";
        }
        int pos1 = kode.LastIndexOf(".", kode.Trim().Length - 2);
        if (pos1 < 0)
        {
          return string.Empty;
        }
        return kode.Trim().Substring(0, pos1 + 1);
      }
      catch (Exception ex)
      {
        UtilityBO.Log(ex);
        return string.Empty;
      }

    }
    public static String IntToStr(long n, int len)
    {
      if (len < 0)
      {
        return string.Empty;
      }
      else if (len == 0)
      {
        return n + "";
      }
      else
      {
        String s = n + "";
        for (int i = s.Length; i < len; i++)
        {
          s = "0" + s;
        }
        return s;
      }
    }
    /*
     * 
     * */
    public static void SetDate(int year, int n, out DateTime Tgl1, out DateTime Tgl2)
    {
      //int year = DateTime.Now.Year;
      int month = DateTime.Now.Month;
      SetDate(year, month, n, out Tgl1, out Tgl2);
    }
    public static void SetDate(int year, int month, int n, out DateTime Tgl1, out DateTime Tgl2)
    {
      switch (n)
      {
        case 1://Tahun
          Tgl1 = new DateTime(year, 1, 1);
          Tgl2 = new DateTime(year, 12, 31);
          break;
        case 2://Semester
          int m2 = ((month - 1) / 6 + 1) * 6 - 5;
          int n2 = ((month - 1) / 6 + 1) * 6;
          Tgl1 = new DateTime(year, m2, 1);
          Tgl2 = new DateTime(year, n2, DateTime.DaysInMonth(year, n2));
          break;
        case 3://Triwulan
          int m3 = ((month - 1) / 3 + 1) * 3 - 2;
          int n3 = ((month - 1) / 3 + 1) * 3;
          Tgl1 = new DateTime(year, m3, 1);
          Tgl2 = new DateTime(year, n3, DateTime.DaysInMonth(year, n3));
          break;
        case 4://Bulan
          Tgl1 = new DateTime(year, month, 1);
          Tgl2 = new DateTime(year, month, DateTime.DaysInMonth(year, month));
          break;
        default:
          Tgl1 = new DateTime(year, month, DateTime.Now.Day);
          Tgl2 = new DateTime(year, month, DateTime.Now.Day);
          break;
      }
    }
    public static void GenerateCSV(IList list, string fname, string[] props)
    {
      using (StreamWriter sw = new StreamWriter(fname))
      {
        foreach (IDataControl dp in list)
        {
          string line = "";
          for (int i = 0; i < props.Length; i++)
          {
            object obj = dp.GetType().GetProperty(props[i]).GetValue(dp, null);
            string str = "";
            if ((obj != null) && (obj.GetType() == typeof(string)))
            {
              str = (string)obj;
              if (str.EndsWith("."))
              {
                str = str.Substring(0, str.Length - 1);
              }
            }
            else if ((obj != null) && (obj.GetType() == typeof(decimal)))
            {
              str = ((decimal)obj).ToString();
              str = str.Replace(",", ".");
            }
            line += str;
            if (i < props.Length - 1)
            {
              line += ";";
            }
          }
          sw.WriteLine(line);
        }
      }
    }
    public static string GetInitkey(string key)
    {
      return GetDotNetHash(key + "19490807", 16);
    }
    public static IDataControl Create(string fullclassname)
    {
      IDataControl obj = null;
      try
      {
        if (!string.IsNullOrEmpty(fullclassname))
        {
          Type T = Type.GetType(fullclassname);
          obj = Activator.CreateInstance(T) as IDataControl;
        }
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message + " on classname=" + fullclassname);
      }
      return obj;
    }

    public const string FILE_SBT = "~\\log.sbt";
    public const string GROUP_ADMINISTRATOR = "1";

    public static string GetMACAddress()
    {
      string macaddr = String.Empty;
      IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
      NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
      Console.WriteLine("Interface information for {0}.{1}     ",
              computerProperties.HostName, computerProperties.DomainName);
      if (nics == null || nics.Length < 1)
      {
        return string.Empty;
      }

      //search Ethernet Adapter
      bool found = false;
      int i = 0;
      NetworkInterface adapter = null;
      while ((i < nics.Length) && (!found))
      {
        adapter = nics[i];
        if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
        {
          found = true;
        }
        else
        {
          i++;
          adapter = null;
        }
      }
      if (adapter == null)
      {
        found = false;
        i = 0;
        adapter = null;
        while ((i < nics.Length) && (!found))
        {
          adapter = nics[i];
          if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet3Megabit)
          {
            found = true;
          }
          else
          {
            i++;
            adapter = null;
          }
        }
      }
      if (adapter == null)
      {
        found = false;
        i = 0;
        adapter = null;
        while ((i < nics.Length) && (!found))
        {
          adapter = nics[i];
          if (adapter.NetworkInterfaceType == NetworkInterfaceType.FastEthernetFx)
          {
            found = true;
          }
          else
          {
            i++;
            adapter = null;
          }
        }
      }
      if (adapter == null)
      {
        found = false;
        i = 0;
        adapter = null;
        while ((i < nics.Length) && (!found))
        {
          adapter = nics[i];
          if (adapter.NetworkInterfaceType == NetworkInterfaceType.FastEthernetT)
          {
            found = true;
          }
          else
          {
            i++;
            adapter = null;
          }
        }
      }
      if (adapter == null)
      {
        found = false;
        i = 0;
        adapter = null;
        while ((i < nics.Length) && (!found))
        {
          adapter = nics[i];
          if (adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
          {
            found = true;
          }
          else
          {
            i++;
            adapter = null;
          }
        }
      }

      if (adapter != null)
      {
        PhysicalAddress address = adapter.GetPhysicalAddress();
        byte[] bytes = address.GetAddressBytes();
        for (i = 0; i < bytes.Length; i++)
        {
          macaddr += bytes[i].ToString("X2");
          if (i != bytes.Length - 1)
          {
            //macaddr += "-";
          }
        }
        ////Console.WriteLine("  Number of interfaces ..... : {0}", nics.Length);
        //foreach (NetworkInterface adapter in nics)
        //{
        //  IPInterfaceProperties properties = adapter.GetIPProperties(); //  .GetIPInterfaceProperties();
        //  //Console.WriteLine();
        //  //Console.WriteLine(adapter.Description);
        //  //Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
        //  //Console.WriteLine("  Interface type ....... : {0}", adapter.NetworkInterfaceType);
        //  //Console.Write("  Physical address ...... : ");
        //  if(adapter.NetworkInterfaceType in {NetworkInterfaceType.Ethernet})
        //}
        return macaddr;
      }
      else
      {
        return string.Empty;
      }
    }
    public static string GetCPUID()
    {
      string cpuInfo = string.Empty;
      if (cpuInfo == string.Empty)
      {
        Object val = null;
        try
        {
          ManagementClass mc = new ManagementClass("win32_processor");
          ManagementObjectCollection moc = mc.GetInstances(); foreach (ManagementObject mo in moc)
          {
            if (cpuInfo == string.Empty)
            {
              //Get only the first CPU's ID
              val = mo.Properties["processorID"].Value;
              cpuInfo = val.ToString();
              break;
            }
          }
          string diskid = GetDiskID();// GetMACAddress();
          cpuInfo = diskid + cpuInfo.Substring(8, 8);
        }
        catch (Exception ex)
        {
          UtilityBO.Log(ex);
          string diskid = GetDiskID();// GetMACAddress();
          cpuInfo = diskid;
        }
      }
      return cpuInfo;

    }
    public static string GetDiskID()
    {
      try
      {
        string strDriveLetter = "C";
        ManagementObject disk =
            new ManagementObject("win32_logicaldisk.deviceid=\"" + strDriveLetter + ":\"");
        disk.Get();
        return disk["VolumeSerialNumber"].ToString();

      }
      catch (Exception ex)
      {
        UtilityBO.Log(ex);
        return string.Empty;
      }
    }
    public static string GeneratePwdSIKD(string cpuid)
    {
      string Initkey = UtilityBO.GetDotNetHash("SIKD19490807", 16);
      return UtilityBO.GetPwdDb(Initkey, cpuid);
    }
    public static string GetPwdDb(string key, string cpuid)
    {
      string pwd = UtilityBO.GetDotNetHash(key + cpuid, 16);
      return pwd;
    }
    public static string GetPwd(string cpuid)
    {
      long decAgain = Math.Abs(long.Parse(cpuid, System.Globalization.NumberStyles.HexNumber));
      long n = decAgain % 19490708;
      n += (n / 10 == 0) ? 10 : 0;
      n += (n / 100 == 0) ? 300 : 0;
      n += (n / 1000 == 0) ? 5000 : 0;
      n += (n / 10000 == 0) ? 90000 : 0;
      n += (n / 100000 == 0) ? 400000 : 0;
      n += (n / 1000000 == 0) ? 8000000 : 0;
      n += (n / 10000000 == 0) ? 700000000 : 0;
      return n.ToString();
    }
    public static string GetRandomPassword(string challenge)
    {
      return GetDotNetHash(challenge, 8);
    }
    public static string GetHexStringForStorePwd(string input)
    {
      byte[] inputSalt, inputHash;
      using (HMACSHA256 hmac = new HMACSHA256())
      {
        inputSalt = hmac.Key;
        inputHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
      }
      return $"{Convert.ToBase64String(inputSalt)}:{Convert.ToBase64String(inputHash)}";
    }
    public static bool Validate(string hashedInput, string input)
    {
      string[] parts = hashedInput.Split(':');
      byte[] inputSalt = Convert.FromBase64String(parts[0]);
      byte[] inputHash = Convert.FromBase64String(parts[1]);
      using (HMACSHA256 hmac = new HMACSHA256(inputSalt))
      {
        byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
        for (int i = 0; i < computedHash.Length; i++)
        {
          if (computedHash[i] != inputHash[i])
          {
            return false;
          }
        }
        return true;
      }
    }
    public static string GetMD5HexStringForStorePwd(string challenge)
    {
      return GetMD5HexStringOld(challenge, 32);
    }
    public static string GetDotNetHash(string challenge, int length)
    {
      int hash = challenge.GetHashCode();
      string code = hash.ToString("X2");
      if (code.Length > length)
      {
        return code.Substring(0, length);
      }
      else
      {
        return code + IntToStr(0, length - code.Length - 1);
      }

    }
    public static string GetMD5HexStringOld(string challenge, int length)
    {
      MD5 md5 = new MD5CryptoServiceProvider();
      byte[] result = md5.ComputeHash(Encoding.ASCII.GetBytes(challenge));

      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < result.Length; i++)
      {
        sb.Append(result[i].ToString("X2"));
      }
      if (sb.Length > length)
      {
        return sb.ToString().Substring(0, length);
      }
      else
      {
        return sb.ToString() + IntToStr(0, length - sb.Length - 1);
      }
    }
    public static string GetPwdMD5ResponseOnly(string initsecret)
    {
      DateTime cnow = DateTime.Now;
      long day = cnow.DayOfYear * 86400;
      long year = (cnow.Year - 2000) * 365 * 86400;
      long hour = cnow.Hour * 3600;
      long minute = cnow.Minute * 60;
      long totdetik = day + year + hour + minute + cnow.Second;

      string epoc = totdetik + initsecret;

      MD5 md5 = new MD5CryptoServiceProvider();
      byte[] result = md5.ComputeHash(Encoding.ASCII.GetBytes(epoc));

      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < result.Length; i++)
      {
        sb.Append(result[i].ToString("X2"));
      }
      return sb.ToString();
    }
    public static void SetNull(Object obj)
    {
      if (obj == null)
      {
        return;
      }

      Type typ = obj.GetType().GetInterface("IDisposable", true);
      if (typ != null)
      {
        ((IDisposable)obj).Dispose();
        return;
      }
      if (obj.GetType() == typeof(DateTime))
      {
        obj = null;
        return;
      }
      if (obj.GetType() == typeof(String[]))
      {
        for (int i = 0; i < ((Array)obj).Length; i++)
        {
          SetNull(((Array)obj).GetValue(i));
        }
        return;
      }
      //todo gimana remove data array, harus dilooping dong
      PropertyInfo[] props = obj.GetType().GetProperties();
      if ((props == null) || (props.Length == 0))
      {
        obj = null;
      }
      else
      {
        for (int i = 0; i < props.Length; i++)
        {
          try
          {
            Object val = props[i].GetValue(obj, null);
            if (val.GetType() == typeof(DateTime))
            {
              val = null;
            }
            else
            {
              SetNull(val);
            }
          }
          catch (Exception ex)
          {
            UtilityBO.Log(ex);
          }
        }
      }
    }
    public static void CopyFields(IDataControl obj1, IDataControl obj2)
    {
      string[] fields = obj1.GetKeys();
      for (int i = 0; i < fields.Length; i++)
      {
        Object o = obj1.GetType().GetProperty(fields[i]).GetValue(obj1, null);
        try
        {
          obj2.GetType().GetProperty(fields[i]).SetValue(obj2, o, null);
        }
        catch (Exception ex)
        {
          UtilityBO.Log(ex);
        }
      }
    }
  }
}
