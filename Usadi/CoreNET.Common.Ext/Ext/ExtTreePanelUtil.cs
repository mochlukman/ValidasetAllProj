using System;
using System.Collections;
using System.Reflection;
using System.Web;


namespace CoreNET.Common.Base
{
  public class ExtTreePanelUtil
  {
    #region Dynamic Load
    private int i = 0;
    public void InitTree()
    {
      i = 0;
    }
    public int GetIndexTree()
    {
      return i++;
    }
    //deprecated
    public static string GetUraian(string str, string[] fieldUraian)
    {
      return GetUraian(str, fieldUraian[1]);
    }
    public static string GetUraian(string str)
    {
      return GetUraian(str, GlobalExt.DELIMITER_MENU);
    }
    public static string GetUraian(string str, string delimiter)
    {
      if (str != null)
      {
        string[] strs = str.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
        int len = 100;
        if (strs.Length > 0)
        {
          if (strs[strs.Length - 1].Length < len)
          {
            return (strs[strs.Length - 1].Trim());
          }
          else
          {
            return (strs[strs.Length - 1].Substring(0, len) + "..");
          }
        }
        else
        {
          return "";
        }
      }
      else
      {
        return "";
      }
    }
    public const int TYPE_TREEMENU = 1;
    public const int TYPE_TREESUBMENU = 2;
    public const int TYPE_TREEGRID = 3;
    public const int TYPE_TREELOOKUP = 4;
    public const int TYPE_TREEFILTER = 5;
    public Ext.Net.TreeNode CreateTree(string ID, IList list, string[] keys, string[] fieldKode, string[] fieldUraian, int typetree, bool withroot)
    {
      //fieldUraian[0] field uraian, fieldUraian[1] delimiter
      if (list.Count > 0)
      {
        if (withroot)
        {
          InitTree();
          int m = GetIndexTree();
          int n = m + 1;
          Object obj = list[m];
          Object nextobj = null;
          string type = "D";
          string kode = (string)obj.GetType().GetProperty(fieldKode[0]).GetValue(obj, null);
          if (n < list.Count)
          {
            nextobj = list[n];
            string nextkode = (string)nextobj.GetType().GetProperty(fieldKode[0]).GetValue(nextobj, null);
            type = "D";
            if (nextkode.Trim().StartsWith(kode.Trim()))
            {
              type = "H";
            }
          }
          else
          {
            type = "D";
          }
          string uraian = (string)obj.GetType().GetProperty(fieldUraian[0]).GetValue(obj, null);

          string tippname = keys[keys.Length - 4];//..,tip,url,level,type
          PropertyInfo prop = obj.GetType().GetProperty(tippname);
          string qtip = kode + uraian;
          if (prop != null)
          {
            string tip = ((IDataControl)obj).GetValue(tippname).ToString();//(string)prop.GetValue(tippname, null);
            if (!string.IsNullOrEmpty(tip))
            {
              qtip = tip;
            }
          }
          //string idproperty = ((ViewListProperties)((IDataControlUI)obj).GetProperties()).IDProperty;
          //string id = (string)((BaseBO)obj).GetValue(idproperty);
          Ext.Net.TreeNode root = ExtTreeNode.GetExtTreeNode(ID + i, GetUraian(uraian, fieldUraian), qtip, typetree, new string[] { }, (IDataControlUI)obj, type);
          //Ext.Net.TreeNode root = ExtTreeNode.GetExtTreeNode(id, GetUraian(uraian, fieldUraian), qtip, typetree, new string[] { }, (IDataControlUI)obj, type);

          IDataControlUI dc = (IDataControlUI)list[0];
          string valkey = "";
          try
          {
            string key = ((ViewListProperties)dc.GetProperties()).IDProperty;
            valkey = (string)obj.GetType().GetProperty(key).GetValue(obj, null);
          }
          catch (Exception ex)
          {
            UtilityBO.Log(dc, ex);
          }
          root.NodeID = ID;// valkey;
          root.Expanded = true;

          BindChild(keys, root, list, false, fieldKode, fieldUraian, typetree);

          return root;
        }
        else
        {
          Ext.Net.TreeNode root = new Ext.Net.TreeNode(ID)
          {
            Expanded = true,
            NodeID = ID
          };
          InitTree();
          IDataControl dc = (IDataControl)list[0];
          BindChild(keys, root, list, false, fieldKode, fieldUraian, typetree);

          return root;
        }
      }
      else
      {
        Ext.Net.TreeNode root = new Ext.Net.TreeNode((ConstantDictExt.IDLOCALE == ConstantDictExt.EN) ? "Empty" : "Kosong")
        {
          NodeID = ID + i
        };
        return root;
      }
    }

    public void BindChild(string[] keys, Ext.Net.TreeNode root, IList list, bool withparent, string[] fieldKode, string[] fieldUraian, int typetree)
    {
      while (i < list.Count)
      {
        int m = GetIndexTree();
        int n = m + 1;
        Object obj = list[m];
        Object nextobj = null;
        if (n < list.Count)
        {
          nextobj = list[n];
        }

        string type = "D";
        string kode = (string)obj.GetType().GetProperty(fieldKode[0]).GetValue(obj, null);
        if (n < list.Count)
        {
          nextobj = list[n];
          string nextkode = (string)nextobj.GetType().GetProperty(fieldKode[0]).GetValue(nextobj, null);
          type = "D";
          if (nextkode.Trim().StartsWith(kode.Trim()))
          {
            type = "H";
          }
        }
        else
        {
          type = "D";
        }

        string uraian = (string)obj.GetType().GetProperty(fieldUraian[0]).GetValue(obj, null);

        string tippname = keys[keys.Length - 4];//..,..,url,level,type
        PropertyInfo prop = obj.GetType().GetProperty(tippname);
        string qtip = kode + uraian;
        if (prop != null)
        {
          string tip = ((IDataControl)obj).GetValue(tippname).ToString();//(string)prop.GetValue(tippname, null);
          if (!string.IsNullOrEmpty(tip))
          {
            qtip = tip;
          }
        }
        IDataControlUI dc = (IDataControlUI)obj;
        //string idproperty = ((ViewListProperties)((IDataControlUI)obj).GetProperties()).IDProperty;
        //string id = (string)((BaseBO)obj).GetValue(idproperty);
        Ext.Net.TreeNode Root = ExtTreeNode.GetExtTreeNode(root.NodeID + i, GetUraian(uraian, fieldUraian), qtip, typetree, new string[] { }, dc, type);
        //Ext.Net.TreeNode Root = ExtTreeNode.GetExtTreeNode(id, GetUraian(uraian, fieldUraian), qtip, typetree, new string[] { }, dc, type);
        string valkey = "";
        try
        {
          string key = ((ViewListProperties)dc.GetProperties()).IDProperty;
          valkey = (string)dc.GetProperty(key).GetValue(dc, null);
        }
        catch (Exception ex)
        {
          UtilityBO.Log(dc, ex);
        }
        Root.NodeID = valkey;
        if ((typetree == TYPE_TREEMENU) || (typetree == TYPE_TREESUBMENU) || (typetree == TYPE_TREEGRID))
        {
          string urlpname = keys[keys.Length - 3];//..,..,url,level,type
          PropertyInfo propurl = obj.GetType().GetProperty(urlpname);

          //@TODO cek ID dari URL harus sama dengan MENUID nya, klo blm ada tambahin.buat utility aja

          if (propurl != null)
          {
            string url = (string)propurl.GetValue(obj, null);
            if ((typetree == TYPE_TREEMENU) || (typetree == TYPE_TREESUBMENU))
            {
              if ((url != null) && (url != ""))
              {
                if (typetree == TYPE_TREESUBMENU)
                {
                  url = url.Replace("Page/", "");
                }
                if (url.Contains("?"))
                {
                  Root.Href = url + "&local=" + HttpContext.Current.Request["local"];
                }
                else
                {
                  Root.Href = url + "?local=" + HttpContext.Current.Request["local"];
                }
              }
              else
              {
                if (HttpContext.Current.Request["no"] != null)//kalau pake no bukan id sbg identifier
                {
                  Root.Href = "~/Blank.aspx";
                }
              }
            }
          }
        }
        else if (typetree == TYPE_TREELOOKUP)
        {
          Root.Text = kode + " " + Root.Text;
        }
        BuiltTree(Root, obj, list, keys, withparent, fieldKode, fieldUraian, typetree);
        root.Nodes.Add(Root);
      }
    }
    public void BuiltTree(Ext.Net.TreeNode Root, Object menu, IList list,
      string[] keys, bool withparent, string[] fieldKode, string[] fieldUraian, int typetree)
    {
      string typepname = "Type";
      typepname = keys[keys.Length - 1];
      BuiltTree(Root, menu, list, keys, withparent, fieldKode, fieldUraian, typetree, typepname);
    }
    public void BuiltTree(Ext.Net.TreeNode Root, Object menu, IList list,
      string[] keys, bool withparent, string[] fieldKode, string[] fieldUraian, int typetree, string typepname)
    {
      string type = (string)menu.GetType().GetProperty(typepname).GetValue(menu, null);
      if (string.IsNullOrEmpty(type))
      {
        return;
      }
      string detil = type.Trim();
      if (detil == "D")
      {
        return;
      }
      else
      {
        while (i < list.Count)
        {
          int m = i;// GetIndexTree();jangan manju dulu
          int n = m + 1;
          Object obj = list[m];
          Object nextobj = null;
          string kodeParent = (string)menu.GetType().GetProperty(fieldKode[0]).GetValue(menu, null);
          string kode = (string)obj.GetType().GetProperty(fieldKode[0]).GetValue(obj, null);
          if (n < list.Count)
          {
            nextobj = list[n];
            string nextkode = (string)nextobj.GetType().GetProperty(fieldKode[0]).GetValue(nextobj, null);
            type = "D";
            if (nextkode.Trim().StartsWith(kode.Trim()))
            {
              type = "H";
            }
          }
          else
          {
            type = "D";
          }

          string uraian = (string)obj.GetType().GetProperty(fieldUraian[0]).GetValue(obj, null);
          string tippname = keys[keys.Length - 4];//..,..,url,level,type
          PropertyInfo prop = obj.GetType().GetProperty(tippname);
          string qtip = kode + uraian;
          if (prop != null)
          {
            string tip = ((IDataControl)obj).GetValue(tippname).ToString();//(string)prop.GetValue(tippname, null);
            if (!string.IsNullOrEmpty(tip))
            {
              qtip = tip;
            }
          }
          IDataControlUI dc = (IDataControlUI)obj;
          //string idproperty = ((ViewListProperties)((IDataControlUI)obj).GetProperties()).IDProperty;
          //string id = (string)((BaseBO)obj).GetValue(idproperty);
          Ext.Net.TreeNode NewNode = ExtTreeNode.GetExtTreeNode(Root.NodeID + i, GetUraian(uraian, fieldUraian), qtip, typetree, new string[] { }, dc, type.Trim());
          //Ext.Net.TreeNode NewNode = ExtTreeNode.GetExtTreeNode(id, GetUraian(uraian, fieldUraian), qtip, typetree, new string[] { }, dc, type.Trim());
          string valkey = "";
          try
          {
            string key = ((ViewListProperties)dc.GetProperties()).IDProperty;
            valkey = (string)dc.GetProperty(key).GetValue(dc, null);
          }
          catch (Exception ex)
          {
            UtilityBO.Log(dc, ex);
          }

          NewNode.NodeID = valkey;
          if ((typetree == TYPE_TREEMENU) || (typetree == TYPE_TREESUBMENU) || (typetree == TYPE_TREEGRID))
          {
            string urlpname = keys[keys.Length - 3];//..,..,url,level,type
            PropertyInfo propurl = obj.GetType().GetProperty(urlpname);
            if (propurl != null)
            {
              string url = (string)propurl.GetValue(obj, null);
              if (typetree == TYPE_TREESUBMENU)
              {
                url = url.Replace("Page/", "");
              }

              if ((typetree == TYPE_TREEMENU) || (typetree == TYPE_TREESUBMENU))
              {
                if ((url != null) && (url != ""))
                {
                  if (url.Contains("?"))
                  {
                    NewNode.Href = url + "&local=" + HttpContext.Current.Request["local"];
                  }
                  else
                  {
                    NewNode.Href = url + "?local=" + HttpContext.Current.Request["local"];
                  }
                }
                else
                {
                  if (HttpContext.Current.Request["no"] != null)
                  {
                    Root.Href = "~/Blank.aspx";
                  }
                }
              }
            }
          }
          else if (typetree == TYPE_TREELOOKUP)
          {
            NewNode.Text = kode + " " + NewNode.Text;
          }
          if (kode.StartsWith(kodeParent.Trim()))//kondisi berhenti karena ngebaca kdper
          {
            Root.Nodes.Add(NewNode);
            i++;
            BuiltTree(NewNode, obj, list, keys, withparent, fieldKode, fieldUraian, typetree);
          }
          else
          {
            return;
          }
        }
      }
    }
    #endregion
  }
}
