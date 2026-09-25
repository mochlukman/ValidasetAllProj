using CoreNET.Common.Base;
using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace CoreNET.Common.BO
{
    #region Ss00appLookup
    [Serializable]
    public class Ss00appLookupControl : Ss00appControl, IDataControlTreeGrid3
    {
        #region Singleton
        private static Ss00appLookupControl _Instance = null;
        public static Ss00appLookupControl Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Ss00appLookupControl();
                }
                return _Instance;
            }
        }
        //private static List<SsappControl> _ListData = null;
        //public static void SetListDataNull()
        //{
        //  _ListData = null;
        //}
        //public static List<SsappControl> GetListDataSingleton()
        //{
        //  if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
        //  {
        //    Ss00appLookupControl dc = new Ss00appLookupControl();
        //    dc.SetPageKey();
        //    _ListData = (List<SsappControl>)dc.View(BaseDataControl.LOOKUP);
        //  }
        //  return _ListData;
        //}
        //public static Ss00appControl FindAndSetValuesIntoByIdapp(IDataControlUI dc)
        //{
        //  Ss00appControl founddc = new Ss00appControl
        //  {
        //    Idapp = (string)dc.GetValue("Idapp")
        //  };
        //  founddc = (Ss00appControl)founddc.Load(BaseDataControl.ID);
        //  if (founddc != null)
        //  {
        //    dc.SetValue("Idapp", founddc.Idapp);
        //    dc.SetValue("Kdapp", founddc.Kdapp);
        //    dc.SetValue("Nmapp", founddc.Nmapp);
        //  }
        //  return founddc;
        //}
        //public static Ss00appControl FindAndSetValuesIntoByKdapp(IDataControlUI dc)
        //{
        //  Ss00appControl founddc = new Ss00appControl
        //  {
        //    Kdapp = (string)dc.GetValue("Kdapp")
        //  };
        //  founddc = (Ss00appControl)founddc.Load(BaseDataControl.PK);
        //  if (founddc != null)
        //  {
        //    dc.SetValue("Idapp", founddc.Idapp);
        //    dc.SetValue("Kdapp", founddc.Kdapp);
        //    dc.SetValue("Nmapp", founddc.Nmapp);
        //  }
        //  return founddc;
        //}
        public static string[] GetFieldValueProps()
        {
            return new string[] { "Kdapp", "Nmapp" };
        }
        #endregion
        public Ss00appLookupControl()
        {
            XMLName = ConstantTablesSys.XMLSS00APP;
        }
        public new IProperties GetProperties()
        {
            ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
            return cViewListProperties;
        }
        public new DataControlFieldCollection GetColumns()
        {
            if (columns == null)
            {
                columns = new DataControlFieldCollection
                    {
                      Fields.Create(ConstantDict.GetColumnTitle("Kdapp"), typeof(string), 30, HorizontalAlign.Left),
                      Fields.Create(ConstantDict.GetColumnTitle("Nmapp"), typeof(string), 50, HorizontalAlign.Left),
                    };
            }
            return columns;
        }


        public new void SetFilterKey(BaseBO bo)
        {
            base.SetFilterKey(bo);
            if (bo.GetProperty("FilterKdapp") != null)
            {
                Kdapp = (string)bo.GetValue("FilterKdapp");
            }
            else
            {
                Kdapp = string.Empty;
            }
        }

        public new IList View()
        {
            List<SsappControl> list = (List<SsappControl>)SsappLookupControl.GetListDataSingleton();
            list = list.FindAll(o => o.Kdapp.StartsWith(Kdapp));
            if (!MasterAppConstants.Instance.StatusAdmin)
            {
                list = list.FindAll(o => o.Last_by.StartsWith(GlobalAsp.GetSessionUser().GetUserID()));
            }
            return list;
        }
        public new void SetTreeGridColumns(TreeGridColumnCollection Columns)
        {
            Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kdapp"), Width = 200, DataIndex = "Kdapp", Align = Ext.Net.TextAlign.Left });
            Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Nmapp"), Width = 500, DataIndex = "Nmapp", Align = Ext.Net.TextAlign.Left });
        }

        public ParameterRow GetLookupParameterRow(IDataControl callerCtr, bool entry)
        {
            string[] keys = new String[] { "Kdapp", "Nmapp", "Idapp" };
            string[] targets = new String[] { "Kdapp=Kdapp", "Nmapp=Nmapp", "Idapp=Idapp" };
            return GetLookupParameterRowDetil((IDataControlUI)callerCtr, entry, keys, targets);
        }
        public ParameterRow GetLookupParameterRowDetil(IDataControlUI callerCtr, bool entry)
        {
            string[] keys = new String[] { "Kdapp", "Nmapp", "Idapp" };
            string[] targets = new String[] { "Kdapp=Kdapp", "Nmapp=Nmapp", "Idapp=Idapp" };
            return GetLookupParameterRowDetil(callerCtr, entry, keys, targets);
        }
        public ParameterRow GetLookupParameterRowAll(IDataControlUI callerCtr, bool entry)
        {
            string[] keys = new String[] { "Kdapp", "Nmapp", "Idapp" };
            string[] targets = new String[] { "Kdapp=Kdapp", "Nmapp=Nmapp", "Idapp=Idapp" };
            return GetLookupParameterRowAll(callerCtr, entry, keys, targets);
        }

        public ParameterRow GetLookupParameterRowDetil(IDataControlUI callerCtr, bool entry, string[] keys, string[] targets)
        {
            Ss00appLookupControl dclookup = new Ss00appLookupControl();
            dclookup.Kdapp = (callerCtr.GetProperty("FilterKdapp") != null) ? (string)callerCtr.GetValue("FilterKdapp") : dclookup.Kdapp;
            string title = ConstantDict.Translate(dclookup.XMLName + "=Modul");
            ParameterRowLookup2 par = new ParameterRowLookup2(callerCtr, keys, new int[] { 20, 75, 0 }, targets)
            {
                Label = title,
                VisibleControls = new bool[] { true, true, !entry },
                AllowRefresh = !entry,
                AllowEmpty = false,
                DCLookup = dclookup,
                IsTree = true,
                SelectionCriteria = ParameterRow.SELECTION_CRITERIA_TYPE,
                SelectionType = "D"
            };
            return par;
        }
        public ParameterRow GetLookupParameterRowAll(IDataControl callerCtr, bool entry, string[] keys, string[] targets)
        {
            Ss00appLookupControl dclookup = new Ss00appLookupControl();
            string title = ConstantDict.Translate(dclookup.XMLName);
            ParameterRowLookup2 par = new ParameterRowLookup2(callerCtr, keys, new int[] { 20, 75, 0 }, targets)
            {
                Label = title,
                VisibleControls = new bool[] { true, true, !entry },
                AllowRefresh = !entry,
                DCLookup = dclookup,
                IsTree = true,
                SelectionCriteria = ParameterRow.SELECTION_CRITERIA_LEVEL,
                SelectionLevel = "1,2,3,4"
            };
            return par;
        }

        public ParameterRow GetLookupParameterRowByUser(IDataControlUI callerCtr, bool entry)
        {
            string[] keys = new String[] { "Kdapp", "Nmapp", "Idapp" };
            string[] targets = new String[] { "Kdapp=Kdapp", "Nmapp=Nmapp", "Idapp=Idapp" };
            Ss00appLookupControl dclookup = new Ss00appLookupControl();
            dclookup.Kdapp = (callerCtr.GetProperty("FilterKdapp") != null) ? (string)callerCtr.GetValue("FilterKdapp") : dclookup.Kdapp;
            string title = ConstantDict.Translate(dclookup.XMLName + "=Modul");
            ParameterRowLookup2 par = new ParameterRowLookup2(callerCtr, keys, new int[] { 20, 75, 0 }, targets)
            {
                Label = title,
                VisibleControls = new bool[] { true, true, !entry },
                AllowRefresh = !entry,
                AllowEmpty = false,
                DCLookup = dclookup,
                IsTree = false,
                SelectionCriteria = ParameterRow.SELECTION_CRITERIA_TYPE,
                SelectionType = "D"
            };
            return par;
        }
        public string GetFieldValueMap()
        {
            return "Idapp=Nmapp";
        }
    }
    #endregion Ss00appLookup
}

