using System;
using System.Collections;
using System.Text;
using Ext.Net;
using CoreNET.Common.Base;

namespace CoreNET.Common.Base
{
  public interface IDataControlTreeGrid3 : IDataControlUIEntry
  {
    Icon GetIcon();
    //string[] GetKeys();
    void SetTreeGridColumns(TreeGridColumnCollection Columns);
    TreeNode CreateRoot(IList list, int typetree, bool withroot);
    void LoadPages(IList list, string nodeid, TreeNodeCollection nodes, int typetree);
  }

}

