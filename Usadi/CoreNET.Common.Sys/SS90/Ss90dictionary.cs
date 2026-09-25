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
using System.Linq;

namespace CoreNET.Common.BO
{
    #region Ss90dictionary
    [Serializable]
    public class Ss90dictionaryControl : BaseDataControlSys, IExtLoadCsv, IDataControlDictionary
    {
        #region Properties
        public string Keyword { get; set; }
        public string Keywordprev { get; set; }
        public string Word1 { get; set; }
        public string Word2 { get; set; }
        public string Word3 { get; set; }
        #endregion Properties

        #region Methods
        public Ss90dictionaryControl()
        {
            XMLName = ConstantTablesSys.XMLSSDICTIONARY;
        }
        public new IProperties GetProperties()
        {
            ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
            cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
            cViewListProperties.PrimaryKeys = new String[] { "Keyword" };
            cViewListProperties.IDKey = "Keyword";
            cViewListProperties.IDProperty = "Keyword";
            cViewListProperties.ReadOnlyFields = new String[] { };
            cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
            cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_EDIT_DEL;
            return cViewListProperties;
        }
        public new int Update()
        {
            Keywordprev = ((Ss90dictionaryControl)GlobalExt.GetEditingObject()).Keyword;
            int n = ((BaseDataControl)this).Update(BaseDataControl.DEFAULT);
            return n;
        }
        public override DataControlFieldCollection GetColumns()
        {
            DataControlFieldCollection columns = new DataControlFieldCollection();
            columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Keyword"), typeof(string), 30, HorizontalAlign.Left).SetEditable(false));
            columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Word1"), typeof(string), 20, HorizontalAlign.Left).SetEditable(true));
            columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Word2"), typeof(string), 20, HorizontalAlign.Left).SetEditable(true));
            columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Word3"), typeof(string), 20, HorizontalAlign.Left).SetEditable(true));
            return columns;
        }
        public override HashTableofParameterRow GetEntries()
        {
            HashTableofParameterRow hpars = new HashTableofParameterRow();
            hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Keyword"), true, 90));
            hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Word1"), true, 90));
            hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Word2"), true, 90));
            hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Word3"), true, 90));
            return hpars;
        }

        static List<Ss90dictionaryControl> _ListData = null;
        public new IList View()
        {
            if (_ListData == null)
            {
                _ListData = ((ArrayList)this.View(BaseDataControl.ALL)).Cast<Ss90dictionaryControl>().ToList();
            }
            return _ListData;
        }

        #endregion Methods
        #region IDataControlDictionary
        public void InsertKeyword(string keyword, string word1)
        {
            try
            {
                Keyword = keyword;
                Word1 = word1;
                Insert();
            }
            catch { }
        }

        public string GetKeyword()
        {
            return "Keyword";
        }

        public string GetIDWord()
        {
            return "Word1";
        }

        public string GetENWord()
        {
            return "Word2";
        }

        public string GetGEWord()
        {
            return "Word3";
        }
        #endregion IDataControlDictionary
    }
    #endregion Ss90dictionary
}

