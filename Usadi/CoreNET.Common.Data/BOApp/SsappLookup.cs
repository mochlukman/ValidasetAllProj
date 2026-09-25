using System;
using System.Collections.Generic;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
    #region SsappLookup
    [Serializable]
    public class SsappLookupControl : SsappControl, IDataControl
    {
        #region Singleton
        private static SsappLookupControl _Instance = null;
        public static SsappLookupControl Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SsappLookupControl();
                }
                return _Instance;
            }
        }
        private static List<SsappControl> _ListData = null;
        public static void SetListDataNull()
        {
            _ListData = null;
        }
        public static List<SsappControl> GetListDataSingleton()
        {
            if (_ListData == null)
            {
                SsappLookupControl dc = new SsappLookupControl();
                dc.SetPageKey();
                _ListData = (List<SsappControl>)dc.View(BaseDataControl.LOOKUP);
            }
            return _ListData;
        }
        public static SsappControl FindAndSetValuesIntoByIdapp(IDataControl dc)
        {
            string idapp = (string)dc.GetValue("Idapp");
            SsappControl founddc = null;
            if (IsOptimized)
            {
                founddc = GetListDataSingleton().Find(o => o.Idapp.Equals(idapp));
            }
            else
            {
                founddc = new SsappControl
                {
                    Idapp = idapp
                };
                founddc = (SsappControl)founddc.Load(BaseDataControl.ID);
            }
            if (founddc != null)
            {
                dc.SetValue("Idapp", founddc.Idapp);
                dc.SetValue("Kdapp", founddc.Kdapp);
                dc.SetValue("Nmapp", founddc.Nmapp);
            }
            return founddc;
        }
        public static SsappControl FindAndSetValuesIntoByKdapp(IDataControl dc)
        {
            string kdapp = (string)dc.GetValue("Kdapp");
            SsappControl founddc = null;
            if (IsOptimized)
            {
                founddc = GetListDataSingleton().Find(o => o.Kdapp.Equals(kdapp));
            }
            else
            {
                founddc = new SsappControl
                {
                    Kdapp = kdapp
                };
                founddc = (SsappControl)founddc.Load(BaseDataControl.PK);
            }
            if (founddc != null)
            {
                dc.SetValue("Idapp", founddc.Idapp);
                dc.SetValue("Kdapp", founddc.Kdapp);
                dc.SetValue("Nmapp", founddc.Nmapp);
            }
            return founddc;
        }
        public static string[] GetFieldValueProps()
        {
            return new string[] { "Kdapp", "Nmapp" };
        }
        #endregion
        public SsappLookupControl()
        {
            XMLName = "Ssapp";
            ModeDB = SQLDataSource.MODE_DB_CONFIG;
        }
    }
    #endregion SsappLookup
}

