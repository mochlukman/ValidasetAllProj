var loadPageByID = function (tabPanelID, id, title, url) {
  if (!!parent) {
    parent.loadPageByID(tabPanelID, id, title, url);
  }
};

var refreshData = function () {
  //Clear
  refreshTree();
};

var refreshDataWithSelection = function () {
  refreshTree();
};
var refreshTree = function () {
  loadingmask();
  CoreNET.Methods2.RefreshTree({
    success: function (result) {
      var nodes = eval(result);
      if (nodes.length > 0) {
        var path = '';
        if (!!TreeGrid1.getSelectedNodes()) {
          path = TreeGrid1.getSelectedNodes().path;
        };
        TreeGrid1.initChildren(nodes);
        if (path != '') {
          CoreNET.Methods2.ExpandTree(path);
        }
      }
      else {
        TreeGrid1.getRootNode().removeChildren();
      }
    }
  });
  //Ini harusnya pas ngesave aja
  //if (!!parent) {
  //  if (!!parent.CoreNET) {
  //    try {
  //      parent.CoreNET.RefreshMasterPage();
  //    } catch(exception)
  //    { 
  //    }
  //  }
  //}
};
var loadNode = function (tabPanel, node, path) {
  loadPage5(tabPanel, path, path, path, node.attributes.href);
}
var loadNode = function (tabPanel, node) {
  loadPage5(tabPanel, node.getPath('id', '/'), node.text, node.text, node.attributes.href);
}
var loadPageByID = function (tabPanelID, id, title, url) {
  var tabPanel = Ext.getCmp(tabPanelID);
  loadPage(tabPanel, id, title, url);
}
var loadPage = function (tabPanel, id, title, url) {
  loadPage5(tabPanel, id, title, title, url);
}
var loadPage5 = function (tabPanel, id, title, tabtip, url) {

  var tab;
  if (tabPanel != null) {
    tab = tabPanel.getItem(id);
    if (tab) {
      tabPanel.remove(tab);
      tab = null;
      //tab.close();
    }
  }
  if ((!tab) && (url != '')) {
    //alert(id);
    tab = tabPanel.add(new Ext.Panel({
      id: id,
      title: title,
      tabTip: tabtip,
      contextMenuID: "TabMenu",
      closable: true,
      autoLoad: {
        showMask: true,
        url: url + "&root=1",
        mode: "iframe",
        maskMsg: "Loading " + title
      },
      listeners: {
        update: {
          fn: function (tab, cfg) {
            cfg.iframe.setHeight(cfg.iframe.getSize().height);
          },
          scope: this,
          single: true
        }
      }
    }));
  } else {
    alert("Can't open new tab because url is empty!");
  }
  tabPanel.setActiveTab(tab);
}

var SortTreeGrid = function (index) {
  var column = TreeGrid1.columns[index];
  TreeGrid1.fireEvent('headerClick', column);
  return true;
};
var SortTreeGridByColumn = function (colname) {
  var index = 0;
  for (var i = 0; i < TreeGrid1.columns.length; i++) {
    if (TreeGrid1.columns[i].dataIndex == colname) {
      index = i;
    }
  }
  var column = TreeGrid1.columns[index];
  try {
    TreeGrid1.fireEvent('headerClick', column);
  } catch (exception) {
  }
  return true;
};

var CopyIntoTreePanel2 = function () {
  var sel = TreeGrid1.selModel.selNode;
  var index = 1;
  var node = new Ext.tree.TreeNode({ text: sel.text, id: index.toString(), leaf: true })
  var append = function (node, sel, parent) {
    if (parent) {
      index += 1;
      var newnode = new Ext.tree.TreeNode({ text: sel.parentNode.text, id: index.toString(), leaf: false, expanded: true });
      newnode.appendChild(node);
      return append(newnode, parent, parent.parentNode);
    } else {
      return node;
    }
  };
  node = append(node, sel, sel.parentNode);
  TreePanel2.root.removeChildren();
  TreePanel2.root.appendChild(node);
};
