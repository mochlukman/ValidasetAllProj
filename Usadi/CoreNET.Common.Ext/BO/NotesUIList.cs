using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using Ext.Net;
using Ext.Net.Utilities;
using CoreNET.Common.Base;
using CoreNET.Common.BO;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.NotesUIListControl, CoreNET.Common.BO
  [Serializable]
  public class NotesUIListControl :  NotesUIControl, IDataControlUIEntry
  {
    public NotesUIListControl()
    {
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_EDIT_DEL;
      return cViewListProperties;
    }
    public new void SetPageKey()
    {
      Refid = GlobalAsp.GetRequestIdPrev();
      Notestype = NotesControl.FORUM;
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlMenu).IsInstanceOfType(bo))
      {
        Last_by = ((BaseBO)bo).Userid;
      }
    }

    public new IList View()
    {
      IList list = base.View(BaseDataControl.ALL);
      return list;
    }

  }
  #endregion NotesUIList
}

