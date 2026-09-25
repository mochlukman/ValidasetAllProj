using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  public class BOSysUtils
  {
    public static string ID_SET_URL
    {
      get
      {
        if (GlobalAsp.GetSessionApp().Equals(MasterAppConstants.Instance.MasterAppID)) { return ID_SET_URL0; } else { return ID_SET_URL1; }
      }
    }

    public const string ID_SET_URL0 = "3534FCF7-06D1-432D-AF02-11592511E16F";//01.12.01.02.
    public const string ID_SET_URL1 = "559492D9-A935-4E75-B603-D3A138EDA7A3";//01.12.02.02.

    public const string ID_LIST_MENU = "A6615CB0-CBC2-4A32-8D8D-242F9799DAFA";   //01.12.02.01.       
    public const string ID_EDITCONFIG = "637317A5-5DA3-4991-9CC2-468037264102";//01.12.02.03.
    public const string ID_FORM_ENTRY = "9DF5CCF5-5776-4F4A-9531-3874C1DEE6D6";//01.12.02.04.
    public const string ID_COL_GRID = "176B89AE-94E0-4897-9923-147F3EA058E3";     //01.12.02.05.
    public const string ID_COL_TREE = "1F14A54D-5826-4830-A143-563524E47314";     //01.12.02.06.
    public const string ID_FILTERS = "D4067E24-71CB-4419-9BEB-5E6E01DFE1F6";      //01.12.02.07.
    public const string ID_KEYS = "2ACF399B-6012-4E49-8E52-A2E1B5F65028";         //01.12.02.08.
    public const string ID_MANUAL = "FA81E9A1-9BD0-4038-8320-2C24F38C37D9";         //01.12.02.09.
    public const string ID_REFERENSI = "892CCB17-2740-4951-B4CB-4558C8FA05C7";         //01.12.02.10.
    public const string ID_FORUM = "35b56a25-3546-4428-a810-d91b758f33c0";         //01.12.02.11.
    public const string ID_VERSI = "B3EC6375-70FC-4330-B1A5-A596995E67FA";         //01.12.02.20.

    public const string ID_MENU_CONFIG = "F73C792F-FB69-4C7B-BD46-8BC40EF3E966";  //01.12.03.07.
    public const string ID_MENU_STATUS = "5813F0A5-20D5-4AFB-8CEC-60985E259B4F";  //01.12.03.08.
    public const string ID_MENU_KKP = "0A58A1A6-2B21-4BEE-ABD2-27F70F2DFF2D";//01.12.03.09.
    public const string ID_LIST_FORUM = "298A3967-F01A-4CEC-AD80-2ABC7ACA6E30";
    public const string ID_APP_CONFIG = "FF05236F-9F4C-4B79-9F0F-A2A7119F08EB";
    public const string ID_ST_MENU = "FF05236F-9F4C-4B79-9F0F-A2A7119F08EB";

    public const string ID_DICTIONARY = "CCC6B321-FBB9-4697-8165-AB935FCF6F3A";
  }
}
