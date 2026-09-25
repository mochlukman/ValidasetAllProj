using System;
using System.Collections;
using System.Collections.Generic;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  #region Dmstatus
  [Serializable]
  public class DmstatusControl : BaseDataControl, IDataControl
  {
    #region Constant
    public const int CANCEL = -1;
    public const int CREATED = 0;
    public const int REVIEWED = 1;
    public const int VALID = 1;
    public const int APPROVED = 2;
    public const int SIGNEDOFF = 3;
    #endregion
    #region Properties
    public int Idstatus { get; set; }
    public string Nmstatus { get; set; }
    public string Ketstatus { get; set; }
    #endregion Properties 
    #region Methods 
    public DmstatusControl()
    {
      XMLName = "Dmstatus";
      ModeDB = SQLDataSource.MODE_DB_CONFIG;
      ConnectionString = SQLDataSource.Instance.CS_Config;
    }
    public void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlAppuser).IsInstanceOfType(bo))
      {
        Last_by = ((IDataControlAppuser)bo).GetUserID();
      }
    }
    public new IList View()
    {
      IList list = this.View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      IList list = ((BaseDataControl)this).View(label);
      List<DmstatusControl> ListData = new List<DmstatusControl>();
      foreach (DmstatusControl dc in list)
      {
        ListData.Add(dc);
      }
      //Update(ListData);
      return ListData;
    }
    #endregion Methods 
    #region IDataControlUIEntry
    public void SetEditable(bool editable)
    {
      Editable = editable;
    }
    public void AfterInsert()
    {
    }

    public void AfterUpdate()
    {
    }

    public void AfterDelete()
    {
    }
    public void FilterClick(string key)
    {
    }
    public string GetDestinationFile(string filename)
    {
      return Path;
    }

    public string GetURLFile(string filename, string versi)
    {
      return Path;
    }


    public Exception ValidateTotal()
    {
      return null;
    }
    #endregion Methods 
    public string[] GetScript()
    {
      string script = @"
        var ShowHidden = function (type) {
          if (type=='D') {
            return 'visible';/**/
          }else{
            return 'hidden';/**/
          }
        };
        var prepareCommands = function (grid, commands, record, row) {
          if (true)
          {
            GetCommandStatus(commands, record.get('Status'));
          }
        };
        var prepareCommand = function(grid, command, record, row)
        {
        };
        var prepareCellCommands = function(grid, commands, record, row, col, value)
        {
        };
        var prepareCellCommand = function(grid, command, record, row, col, value)
        {
          command.command = command.command.replace('Detil', 'Link');
        };
      ";
      return new string[] { script };
    }
  }
  #endregion Dmstatus
}

