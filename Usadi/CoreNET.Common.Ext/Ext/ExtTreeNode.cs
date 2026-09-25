using Ext.Net;
using System;
using System.Reflection;
using System.Web;



namespace CoreNET.Common.Base
{
  public class ExtTreeNode
  {
    public static Ext.Net.TreeNode GetExtTreeNode(string id, string text, int typetree, string[] names, IDataControl obj)
    {
      Icon icon = Icon.None;
      if (typeof(IDataControlTreeGrid3).IsInstanceOfType(obj))
      {
        icon = ((IDataControlTreeGrid3)obj).GetIcon();
      }
      else
      {
        icon = ((IDataControlTreeGrid3)obj).GetIcon();
      }
      return (Ext.Net.TreeNode)GetExtTreeNode(id, text, id + " " + text, typetree, names, obj, false, icon, false);
    }
    public static Ext.Net.TreeNode GetExtTreeNode(string id, string text, string qtip, int typetree, string[] names, IDataControl obj, string type)
    {
      Icon icon = Icon.None;
      if (type.Trim() == "D")
      {
        if (typeof(IDataControlTreeGrid3).IsInstanceOfType(obj))
        {
          icon = ((IDataControlTreeGrid3)obj).GetIcon();
        }
        else
        {
          icon = ((IDataControlTreeGrid3)obj).GetIcon();
        }
      }
      else
      {
        icon = Icon.Folder;
      }
      return (Ext.Net.TreeNode)GetExtTreeNode(id, text, qtip, typetree, names, obj, false, icon, false);
    }
    public static Ext.Net.TreeNodeBase GetExtTreeNode(string id, string text, int typetree, string[] names, IDataControl obj, bool async)
    {
      Icon icon = Icon.None;
      if (typeof(IDataControlTreeGrid3).IsInstanceOfType(obj))
      {
        icon = ((IDataControlTreeGrid3)obj).GetIcon();
      }
      else
      {
        icon = ((IDataControlTreeGrid3)obj).GetIcon();
      }
      return GetExtTreeNode(id, text, id + " " + text, typetree, names, obj, async, icon, false);
    }
    public static Ext.Net.TreeNodeBase GetExtTreeNode(string id, string text, int typetree, string[] names, IDataControl obj, Icon icon)
    {
      return GetExtTreeNode(id, text, id + " " + text, typetree, names, obj, false, icon, false);
    }
    public static Ext.Net.TreeNodeBase GetExtTreeNode(string id, string text, int typetree, string[] names, IDataControl obj, bool async, Icon icon)
    {
      return GetExtTreeNode(id, text, id + " " + text, typetree, names, obj, async, icon, false);
    }
    //public static Ext.Net.TreeNodeBase GetExtTreeNode(string id, string text, string qtip, int typetree, string[] names, IDataControlUI obj, bool async, Icon icon)
    //{
    //  return GetExtTreeNode(id, text, id + " " + text, typetree, names, obj, async, icon, false);
    //}
    public static Ext.Net.TreeNodeBase GetExtTreeNode(string id, string text, string qtip, int typetree, string[] names, IDataControl obj, bool async, Icon icon, bool multiselect)
    {
      #region Node
      Ext.Net.TreeNodeBase node = null;
      if (typetree == ExtTreePanelUtil.TYPE_TREEGRID)
      {
        string typepname = names[names.Length - 1];
        string type = (string)obj.GetType().GetProperty(typepname).GetValue(obj, null);
        string nodetext = text;
        if (!async)
        {
          node = new Ext.Net.TreeNode(id, nodetext, icon)
          {
            Leaf = true
          };
          //node.Expanded = true;
        }
        else
        {
          node = new Ext.Net.AsyncTreeNode(id, nodetext)
          {
            Expanded = false,
            Icon = icon
          };
        }

        bool ischeckd = multiselect;
        if (ischeckd)
        {
          node.Checked = ThreeStateBool.False;
        }
        else
        {
          node.Checked = ThreeStateBool.Undefined;
        }
      }
      else if (typetree == ExtTreePanelUtil.TYPE_TREELOOKUP)
      {
        if (!async)
        {
          node = new Ext.Net.TreeNode(id, text, icon)
          {
            Expanded = true
          };
        }
        else
        {
          node = new Ext.Net.AsyncTreeNode(id, text)
          {
            Expanded = false,
            Icon = icon
          };
        }
        bool AllowCheckedAll = true;
        if (obj.GetType().GetProperty("AllowCheckedAll") != null)
        {
          Object val = obj.GetType().GetProperty("AllowCheckedAll").GetValue(obj, null);
          AllowCheckedAll = (bool)val;
        }
        if (AllowCheckedAll)
        {
          node.Checked = ThreeStateBool.False;
        }
        else
        {
          string typepname = names[names.Length - 1];
          string type = (string)obj.GetType().GetProperty(typepname).GetValue(obj, null);
          if (type == "D")
          {
            node.Checked = ThreeStateBool.False;
          }
        }
      }
      else if (typetree == ExtTreePanelUtil.TYPE_TREEFILTER)
      {
        if (!async)
        {
          node = new Ext.Net.TreeNode(id, text, Icon.None);
        }
        else
        {
          node = new Ext.Net.AsyncTreeNode(id, text);
        }
        node.Icon = Icon.None;
        node.Expanded = false;
        node.Checked = ThreeStateBool.Undefined;
      }
      else if ((typetree == ExtTreePanelUtil.TYPE_TREEMENU) || (typetree == ExtTreePanelUtil.TYPE_TREESUBMENU))
      {
        if (!async)
        {
          node = new Ext.Net.TreeNode(id, HttpUtility.HtmlDecode(text), icon);
        }
        else
        {
          node = new Ext.Net.AsyncTreeNode(id, HttpUtility.HtmlDecode(text));
        }
        node.Icon = icon;
        node.Expanded = false;
        node.Checked = ThreeStateBool.Undefined;
      }
      for (int i = 0; i < names.Length; i++)
      {
        PropertyInfo prop = obj.GetProperty(names[i]);
        if (prop == null)
        {
          throw new Exception(ConstantDictExt.Translate("LBL_NOT_EXIST_PROP") + " = " + names[i]);
        }
        Object val = obj.GetProperty(names[i]).GetValue(obj, null);
        if (val == null)
        {
          node.CustomAttributes.Add(new ConfigItem { Name = names[i], Value = "", Mode = ParameterMode.Value });
        }
        else
        {
          node.CustomAttributes.Add(new ConfigItem { Name = names[i], Value = val.ToString(), Mode = ParameterMode.Value });
          //node.CustomAttributes.Add(new ConfigItem { Name = names[i], Value = JSON.Serialize(val), Mode = ParameterMode.Raw });
        }
      }
      node.Qtip = qtip;
      #endregion
      #region URL
      if ((typetree == ExtTreePanelUtil.TYPE_TREEMENU) ||
        (typetree == ExtTreePanelUtil.TYPE_TREESUBMENU) || (typetree == ExtTreePanelUtil.TYPE_TREEGRID))
      {
        PropertyInfo prop = obj.GetType().GetProperty("UrlFull");
        if (prop == null)
        {
          prop = obj.GetType().GetProperty("Url");
        }
        if (prop != null)
        {
          string url = (string)prop.GetValue(obj, null);
          if (typetree == ExtTreePanelUtil.TYPE_TREEMENU)
          {
            if (!string.IsNullOrEmpty(url))
            {
              node.Href = url;
            }
            else
            {
              if (HttpContext.Current.Request["no"] != null)//kalau pake no bukan id sbg identifier
              {
                node.Href = "~/Blank.aspx";
              }
            }
          }
          else if (typetree == ExtTreePanelUtil.TYPE_TREESUBMENU)
          {
            url = url.Replace("Page/","");
            if (!string.IsNullOrEmpty(url))
            {
              node.Href = url;
            }
          }
        }
      }
      #endregion

      return node;
    }
  }
}
