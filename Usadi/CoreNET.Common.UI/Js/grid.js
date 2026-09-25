var loadPageByID = function (tabPanelID, id, title, url) {
  if (!!parent) {
    parent.loadPageByID(tabPanelID, id, title, url);
  }
};

var refreshData = function () {
  Store1.reload();
  CoreNET.Methods1.GridDeleteSelected();
  ExpandFrame();
}

var refreshDataWithSelection = function () {
  var record = null;
  try {
    record = GridPanel1.getSelectionModel().getSelected();
    Store1.reload();
  } catch (exception)
  {
  }
  try {
    GridPanel1.getSelectionModel().selectRow(record);
    if (!!GridPanel1.getSelectionModel().getSelected()) {
      record = GridPanel1.getSelectionModel().getSelected();
      FPFormEntry1.getForm().loadRecord(record)
    }
  } catch (exception)
  {
  }
}

var startEditing = function (e) {
  var grid;
  if (e.getKey() === e.ENTER) {
    grid = GridPanel1,
                    record = grid.getSelectionModel().getSelected(),
                    index = grid.store.indexOf(record);
    grid.startEditing(index, 1);
  }
};

var afterEdit = function (e) {
  /*
  Properties of 'e' include:
  e.grid - This grid
  e.record - The record being edited
  e.field - The field name being edited
  e.value - The value being set
  e.originalValue - The original value for the field, before the edit.
  e.row - The grid row index
  e.column - The grid column index
  */

  Store1.commitChanges();
  CoreNET.Methods1.btnSaveEditingCell(e.field, e.originalValue, e.record.data);
  FPFormEntry1.getForm().loadRecord(e.record)
};
