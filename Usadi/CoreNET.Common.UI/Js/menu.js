var onClick = function () {
  document.getElementById("loading-mask").style.display = '';
};
/* Home */
var home = function () {
  CoreNET.Home();
}
/* Supress Error */
window.onerror = function (message, url, lineNumber) {
  // code to execute on an error  
  return true; // prevents browser error messages
}
var refreshData = function () {
  //Clear
  refreshTree();
};

var refreshDataWithSelection = function () {
  refreshTree();
};
var refreshTree = function () {
  onClick();
  CoreNET.SettingGroup({
    success: function (result) {
      var nodes = eval(result);
      if (nodes.length > 0) {
        var path = '';
        if (!!TreePanel1.getSelectedNodes()) {
          path = TreePanel1.getSelectedNodes().path;
        };
        TreePanel1.initChildren(nodes);
        if (path != '') {
          TreePanel1.selectPath(path);
        }
      }
      else {
        TreeGrid1.getRootNode().removeChildren();
      }
    }
  });
  closeAllPages();

};
var closeAllPages = function () {
  var tp = Pages,
    tabs = tp.items.getRange(),
    i;

  for (i = tabs.length-1; i >= 1; i--) {
    try {
      if (!!tabs[i].close) tabs[i].close();
      if (!!tabs[i].destroy) tabs[i].destroy();
      Pages.remove(Pages.getItem(i));//.close();
    } catch (exception) { }
  }
}

var filterclick = function (key) {
  CoreNET.FilterClick(key)
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
var linkify = function (title, url) {
  var tab = Pages.getItem(title);
  loadPage(Pages, title, title, url);
}
