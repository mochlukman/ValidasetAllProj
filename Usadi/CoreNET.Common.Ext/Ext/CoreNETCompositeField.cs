using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;

namespace CoreNET.Common.Base
{
    public class CoreNETCompositeField : CompositeField
    {

        #region Property DEFAULT_FP_WIDTH
        private static int _DEFAULT_FP_WIDTH = 500;// ExtWindows.DEFAULT_WIDTH;
        public static int DEFAULT_FP_WIDTH
        {
            get => _DEFAULT_FP_WIDTH;
            set => _DEFAULT_FP_WIDTH = value;
        }
        #endregion

        #region Property ModeEdit
        private int _ModeEdit;
        public int ModeEdit
        {
            get => _ModeEdit;
            set => _ModeEdit = value;
        }
        #endregion

        public CoreNETCompositeField(ParameterRow pr, int target)
        {
            AddControls(pr, GlobalAsp.ENTRY, target);
        }
        public CoreNETCompositeField(ParameterRow pr, int mode, int target)
        {
            AddControls(pr, mode, target);
        }
        public void AddControls(ParameterRow pr, int mode, int target)
        {
            string id = (mode == GlobalAsp.FILTER) ? "cfFilter" + pr.Name : "cfEntry" + pr.Name;
            ID = id.Replace(",", string.Empty);//pr.name bisa jd ada karakter koma
            Width = DEFAULT_FP_WIDTH - 150;
            LabelPad = 5;
            ToolTip = pr.Hint;
            FieldLabel = pr.Label;
            LabelWidth = ExtWindows.DEFAULT_LABEL_WIDTH;
            AnchorHorizontal = "100%";
            ModeEdit = pr.ModeEdit;
            if (pr.ModeEdit == ParameterRow.MODE_CHECK)
            {
                #region ParameterRow.MODE_CHECK
                Checkbox textField = new Checkbox
                {
                    ID = (mode == GlobalAsp.FILTER) ? "textFilter" + pr.Name : "textEntry" + pr.Name,
                    DataIndex = pr.Name,
                    AnchorHorizontal = "100%",
                    Width = pr.Width * (DEFAULT_FP_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100
                };

                //if (ExtGridPanel.MODE_EDIT.Equals(HttpContext.Current.Session[ExtGridPanel.SESSION_MODE]))
                if (UtilityExt.IsModeEdit())
                {
                    textField.Enabled = pr.Editable;
                }
                textField.Checked = (bool)pr.Value;
                Items.Add(textField);
                #endregion ParameterRow.MODE_CHECK
            }
            else if ((pr.ModeEdit == ParameterRow.MODE_DATE))
            {
                #region ParameterRow.MODE_DATE
                DateField dateField = new DateField
                {
                    ID = (mode == GlobalAsp.FILTER) ? "textFilter" + pr.Name : "textEntry" + pr.Name,
                    DataIndex = pr.Name,// "Tgl1";//
                    Format = "dd/MM/yyyy", //pr.Regex;
                    AnchorHorizontal = "100%",
                    MaxLength = pr.Length,
                    Width = 100// pr.Width * (DEFAULT_FP_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100;
                };
                //if (ExtGridPanel.MODE_EDIT.Equals(HttpContext.Current.Session[ExtGridPanel.SESSION_MODE]))
                if (UtilityExt.IsModeEdit())
                {
                    dateField.Selectable = pr.Editable;
                }
                dateField.Selectable = pr.Enable;
                if (pr.AllowRefresh)
                {
                    if (mode == GlobalAsp.FILTER)/*analisis dulu: gimana klo asinkronus?trus id and displaytext?*/
                    {
                        dateField.Listeners.Change.Handler +=
                        ((target == ExtGridPanelFilter.GRID) ?
                          "  Store1.reload();" :
                          "  refreshTree();");
                    }
                }
                Items.Add(dateField);
                #endregion ParameterRow.MODE_DATE
            }
            else if ((pr.ModeEdit == ParameterRow.MODE_DATE_RANGE))
            {
                #region ParameterRow.MODE_DATE_RANGE
                //int year = ((DateTime)pr.Value).Year;
                //DateTime d1 = new DateTime(year, 1, 1);
                //DateTime d2 = new DateTime(year, 12, 31);
                DateField DateAwalTahun = new DateField() { ID = "DateAwalTahun", DataIndex = "YearStartDate", Hidden = true };
                DateAwalTahun.Vtype = "daterange";
                DateAwalTahun.CustomConfig.Add(new ConfigItem() { Name = "endDateField", Value = "#{DateTgl1}", Mode = ParameterMode.Value });//pake kondisi

                DateField DateAkhirTahun = new DateField() { ID = "DateAkhirTahun", DataIndex = "YearEndDate", Hidden = true };
                DateAkhirTahun.Vtype = "daterange";
                DateAkhirTahun.CustomConfig.Add(new ConfigItem() { Name = "startDateField", Value = "#{DateTgl2}", Mode = ParameterMode.Value });//pake kondisi

                Items.Add(DateAwalTahun);
                Items.Add(DateAkhirTahun);

                DateField dateField = new DateField() { ID = "DateTgl1" };
                dateField.DataIndex = "Tgl1";
                dateField.Format = "dd/MM/yyyy"; //pr.Regex;
                dateField.AnchorHorizontal = "100%";
                dateField.Vtype = "daterange";
                dateField.MaxLength = pr.Length;
                dateField.Width = 100;// 
                dateField.CustomConfig.Add(new ConfigItem() { Name = "startDateField", Value = "#{DateAwalTahun}", Mode = ParameterMode.Value });//pake kondisi
                dateField.CustomConfig.Add(new ConfigItem() { Name = "endDateField", Value = "#{DateTgl2}", Mode = ParameterMode.Value });
                if (UtilityExt.IsModeEdit())
                {
                    dateField.Selectable = pr.Editable;
                }
                dateField.Selectable = pr.Enable;
                dateField.Enabled = pr.Enable;
                if (pr.AllowRefresh)
                {
                    if (mode == GlobalAsp.FILTER)/*analisis dulu: gimana klo asinkronus?trus id and displaytext?*/
                    {
                        dateField.Listeners.Change.Handler +=
                        ((target == ExtGridPanelFilter.GRID) ?
                          "  Store1.reload();" :
                          "  refreshTree();");
                    }
                }
                Items.Add(dateField);

                DateField dateField2 = new DateField() { ID = "DateTgl2" };
                dateField2.FieldLabel = "Sampai";
                dateField2.DataIndex = "Tgl2";
                dateField2.Format = "dd/MM/yyyy"; //pr.Regex;
                dateField2.AnchorHorizontal = "100%";
                dateField2.Vtype = "daterange";
                dateField2.MaxLength = pr.Length;
                dateField2.Width = 100;// 
                dateField2.CustomConfig.Add(new ConfigItem() { Name = "startDateField", Value = "#{DateTgl1}", Mode = ParameterMode.Value });
                dateField2.CustomConfig.Add(new ConfigItem() { Name = "endDateField", Value = "#{DateAkhirTahun}", Mode = ParameterMode.Value });//pake kondisi


                if (UtilityExt.IsModeEdit())
                {
                    dateField2.Selectable = pr.Editable;
                }
                dateField2.Selectable = pr.Enable;
                dateField2.Enabled = pr.Enable;
                if (pr.AllowRefresh)
                {
                    if (mode == GlobalAsp.FILTER)/*analisis dulu: gimana klo asinkronus?trus id and displaytext?*/
                    {
                        dateField2.Listeners.Change.Handler +=
                        ((target == ExtGridPanelFilter.GRID) ?
                          "  Store1.reload();" :
                          "  refreshTree();");
                    }
                }
                Items.Add(dateField2);
                #endregion ParameterRow.MODE_DATE_RANGE
            }
            else if ((pr.ModeEdit == ParameterRow.MODE_TIME))
            {
                #region ParameterRow.MODE_TIME
                TimeField dateField = new TimeField
                {
                    ID = (mode == GlobalAsp.FILTER) ? "textFilter" + pr.Name : "textEntry" + pr.Name,
                    DataIndex = pr.Name,
                    Format = pr.Regex,
                    AnchorHorizontal = "100%",
                    Increment = 60,
                    SelectedTime = ((DateTime)pr.Value).TimeOfDay,
                    Width = pr.Width * (DEFAULT_FP_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100
                };
                //if (ExtGridPanel.MODE_EDIT.Equals(HttpContext.Current.Session[ExtGridPanel.SESSION_MODE]))
                if (UtilityExt.IsModeEdit())
                {
                    dateField.Enabled = pr.Editable;
                }
                Items.Add(dateField);
                #endregion ParameterRow.MODE_TIME
            }
            else if ((pr.ModeEdit == ParameterRow.MODE_MINUTE))
            {
                #region ParameterRow.MODE_MINUTE
                DateField dateField = new DateField
                {
                    ID = (mode == GlobalAsp.FILTER) ? "textFilter" + pr.Name : "textEntry" + pr.Name,
                    DataIndex = pr.Name,
                    Format = "dd-MM-yyyy",
                    AnchorHorizontal = "100%",
                    MaxLength = pr.Length,
                    Width = pr.Width * (DEFAULT_FP_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100
                };
                //if (ExtGridPanel.MODE_EDIT.Equals(HttpContext.Current.Session[ExtGridPanel.SESSION_MODE]))
                if (UtilityExt.IsModeEdit())
                {
                    dateField.Enabled = pr.Editable;
                }
                Items.Add(dateField);

                TimeField timeField = new TimeField
                {
                    ID = (mode == GlobalAsp.FILTER) ? "textFilter" + pr.Name : "textEntry" + pr.Name,
                    DataIndex = pr.Name,
                    Format = "HH",
                    AnchorHorizontal = "100%",
                    Increment = 60,
                    SelectedTime = ((DateTime)pr.Value).TimeOfDay,
                    Width = pr.Width * (DEFAULT_FP_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100
                };
                //if (ExtGridPanel.MODE_EDIT.Equals(HttpContext.Current.Session[ExtGridPanel.SESSION_MODE]))
                if (UtilityExt.IsModeEdit())
                {
                    timeField.Enabled = pr.Editable;
                }
                Items.Add(timeField);

                TimeField minuteField = new TimeField
                {
                    ID = (mode == GlobalAsp.FILTER) ? "textFilter" + pr.Name : "textEntry" + pr.Name,
                    DataIndex = pr.Name,
                    Format = "mm",
                    AnchorHorizontal = "100%",
                    Increment = 1,
                    SelectedTime = ((DateTime)pr.Value).TimeOfDay,
                    Width = pr.Width * (DEFAULT_FP_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100
                };
                //if (ExtGridPanel.MODE_EDIT.Equals(HttpContext.Current.Session[ExtGridPanel.SESSION_MODE]))
                if (UtilityExt.IsModeEdit())
                {
                    minuteField.Enabled = pr.Editable;
                }
                Items.Add(minuteField);
                #endregion ParameterRow.MODE_MINUTE
            }
            else if ((pr.ModeEdit == ParameterRow.MODE_ENTRY))
            {
                #region ParameterRow.MODE_ENTRY
                TextField textField = new TextField
                {
                    ID = (mode == GlobalAsp.FILTER) ? "textFilter" + pr.Name : "textEntry" + pr.Name,
                    DataIndex = pr.Name,
                    ToolTip = pr.Hint,
                    AnchorHorizontal = "100%",
                    MaxLength = pr.Length,
                    MaxLengthText = "Jumlah karakter tidak boleh lebih dari {0} karakter",
                    InputType = InputType.Text, //(pr.Name.ToLower().Contains("pwd")) ? InputType.Password : InputType.Text;
                                                //textField.MaskRe = pr.Regex;
                                                //textField.Regex = pr.Regex;//maxlength ga ngaruh
                    Width = pr.Width * (DEFAULT_FP_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100,
                    AllowBlank = pr.AllowEmpty
                };
                if (!string.IsNullOrEmpty(pr.Regex))
                {
                    if (pr.Regex.Equals("*"))
                    {
                        textField.InputType = InputType.Password;
                    }
                    else
                    {
                        textField.MaskRe = pr.Regex;
                    }
                }
                //textField.MaskRe=pr.Mas
                //if (pr.Listeners.Count > 0)
                //{
                //  foreach (KeyValuePair<ParameterRowEvent, string> rowEvent in pr.Listeners)
                //  {
                //    if (rowEvent.Key.Equals(ParameterRowEvent.Blur))
                //    {
                //      textField.Listeners.Blur.Handler = rowEvent.Value;
                //    }
                //  }
                //}

                //if (ExtGridPanel.MODE_EDIT.Equals(HttpContext.Current.Session[ExtGridPanel.SESSION_MODE]))
                if (UtilityExt.IsModeEdit() || (mode == GlobalAsp.FILTER))
                {
                    textField.ReadOnly = !pr.Editable;
                    textField.Selectable = pr.Editable;
                    Items.Add(textField);
                }
                else
                {
                    Items.Add(textField);
                }
                #region pr.Action
                if (!string.IsNullOrEmpty(pr.Action))
                {
                    Ext.Net.Button btnLookup = new Ext.Net.Button
                    {
                        ID = (mode == GlobalAsp.FILTER) ? "btnFilter" + pr.Name : "btnEntry" + pr.Name,
                        CommandArgument = pr.Name
                    };
                    btnLookup.Listeners.Click.Handler = string.Format("CoreNET.Methods1.EntryClick('{0}');", pr.Action);
                    btnLookup.Icon = Icon.Connect;
                    Items.Add(btnLookup);
                }
                #endregion

                #endregion ParameterRow.MODE_ENTRY
            }
            else if ((pr.ModeEdit == ParameterRow.MODE_ENTRY2))
            {
                #region ParameterRow.MODE_ENTRY2
                DataIndex = pr.Name;
                string[] names = pr.Name.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                TextField textField1 = new TextField
                {
                    ID = (mode == GlobalAsp.FILTER) ? "textFilter" + names[0] : "textEntry" + names[0],
                    DataIndex = names[0],
                    AnchorHorizontal = "100%",
                    Width =
                   (mode == GlobalAsp.FILTER) ?
                   pr.Width * (ExtWindows.MAX_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100 :
                   pr.Width * (DEFAULT_FP_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100,
                    ReadOnly = true,
                    Enabled = false,
                    AllowBlank = true
                };
                Items.Add(textField1);

                if (names.Length > 1)
                {
                    TextField textField2 = new TextField
                    {
                        ID = (mode == GlobalAsp.FILTER) ? "textFilter" + names[1] : "textEntry" + names[1],
                        DataIndex = names[1],
                        AnchorHorizontal = "100%",
                        Width =
                       (mode == GlobalAsp.FILTER) ?
                       pr.Width2 * (ExtWindows.MAX_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100 :
                       pr.Width2 * (DEFAULT_FP_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100,
                        ReadOnly = true,
                        AllowBlank = true,
                        Enabled = false
                    };
                    Items.Add(textField2);
                }
                #endregion ParameterRow.MODE_ENTRY2
            }
            else if ((pr.ModeEdit == ParameterRow.MODE_LOOKUP))
            {
                #region ParameterRow.MODE_LOOKUP
                TextField textField = new TextField
                {
                    ID = (mode == GlobalAsp.FILTER) ? "textFilter" + pr.Name : "textEntry" + pr.Name,
                    DataIndex = pr.Name,
                    AnchorHorizontal = "100%",
                    MaxLength = pr.Length,
                    AllowBlank = pr.AllowEmpty,
                    Width = pr.Width * (DEFAULT_FP_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100
                };
                Items.Add(textField);
                #endregion ParameterRow.MODE_LOOKUP
            }
            else if ((pr.ModeEdit == ParameterRow.MODE_LOOKUP2))
            {
                #region ParameterRow.MODE_LOOKUP2
                DataIndex = pr.Name;
                string[] names = pr.Name.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                TextField textField1 = new TextField
                {
                    ID = (mode == GlobalAsp.FILTER) ? "textFilter" + names[0] : "textEntry" + names[0],
                    DataIndex = names[0],
                    AnchorHorizontal = "100%",
                    Width =
                   (mode == GlobalAsp.FILTER) ?
                   pr.Width * (ExtWindows.MAX_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100 :
                   pr.Width * (DEFAULT_FP_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100,
                    ReadOnly = true,
                    Enabled = false,
                    AllowBlank = pr.AllowEmpty
                };
                Items.Add(textField1);

                Ext.Net.Button btnLookup = new Ext.Net.Button
                {
                    ID = (mode == GlobalAsp.FILTER) ? "btnFilter" + names[0] : "btnEntry" + names[0],
                    CommandArgument = pr.Name
                };

                btnLookup.Listeners.Click.Handler = "ExpandFrame();CoreNET.Methods2.btnLookup_DirectClick(" + mode + ",\"" + pr.Name + "\")";//keliatan laodingnya//
                btnLookup.Icon = Icon.Magnifier;
                btnLookup.Enabled = pr.Enable && pr.Editable;
                btnLookup.Disabled = !btnLookup.Enabled;
                Items.Add(btnLookup);

                if (names.Length > 1)
                {
                    TextField textField2 = new TextField
                    {
                        ID = (mode == GlobalAsp.FILTER) ? "textFilter" + names[1] : "textEntry" + names[1],
                        DataIndex = names[1],
                        AnchorHorizontal = "100%",
                        Width =
                       (mode == GlobalAsp.FILTER) ?
                       pr.Width2 * (ExtWindows.MAX_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100 :
                       pr.Width2 * (DEFAULT_FP_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100,
                        ReadOnly = true,
                        AllowBlank = pr.AllowEmpty,
                        Enabled = false
                    };
                    if (pr.Width < 1 && btnLookup.Visible)
                    {
                        textField2.Style.Add("left", "25px");
                    }
                    if (pr.Width2 == 0)
                    {
                        textField2.Hidden = true;
                    }
                    Items.Add(textField2);

                    List<string> keys = new List<string>(new string[] { names[0], names[1] });
                    if (names.Length > 2)
                    {
                        for (int i = 2; i < names.Length; i++)//Hidden Value bisa lebih dari 1
                        {
                            if (!keys.Contains(names[i]))
                            {
                                TextField textField3 = new TextField
                                {
                                    ID = (mode == GlobalAsp.FILTER) ? "textFilter" + names[i] : "textEntry" + names[i],
                                    DataIndex = names[i],
                                    Hidden = true
                                };
                                Items.Add(textField3);
                                keys.Add(names[i]);
                            }
                        }
                    }
                }
                if (string.IsNullOrEmpty(pr.Label))
                {
                    LabelWidth = 0;
                    LabelPad = 0;
                }
                #endregion ParameterRow.MODE_LOOKUP2
            }
            else if ((pr.ModeEdit == ParameterRow.MODE_MEMO))
            {
                #region ParameterRow.MODE_MEMO
                TextArea textArea = new TextArea
                {
                    ID = (mode == GlobalAsp.FILTER) ? "textFilter" + pr.Name : "textEntry" + pr.Name,
                    DataIndex = pr.Name,
                    Note = pr.Hint,
                    AnchorHorizontal = "10%",
                    MaxLength = pr.Length,
                    AllowBlank = pr.AllowEmpty,
                    Width = 95 * (DEFAULT_FP_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100
                };
                //if (ExtGridPanel.MODE_EDIT.Equals(HttpContext.Current.Session[ExtGridPanel.SESSION_MODE]))
                if (UtilityExt.IsModeEdit() || (mode == GlobalAsp.FILTER))
                {
                    textArea.ReadOnly = !pr.Editable;
                    textArea.Selectable = pr.Editable;
                }
                Items.Add(textArea);
                #endregion ParameterRow.MODE_MEMO
            }
            else if ((pr.ModeEdit == ParameterRow.MODE_HTML))
            {
                #region ParameterRow.MODE_HTML
                Ext.Net.Panel PnlEditor = new Ext.Net.Panel
                {
                    ID = "pnlEditor" + pr.Name,
                    PType = pr.Name
                };
                PnlEditor.AutoLoad.Url = UtilityExt.ValidateURL(this, string.Format("~/Page/TextEditor.aspx?app=" + GlobalAsp.GetRequestApp() + "&roleid=" + HttpContext.Current.Request["roleid"] + "&id=" + GlobalAsp.GetRequestId() + "&title=Prosedur&pname=" + pr.Name));
                PnlEditor.AutoLoad.NoCache = true;
                PnlEditor.AutoLoad.Mode = LoadMode.IFrame;
                PnlEditor.LoadContent();
                PnlEditor.AnchorHorizontal = "100%";
                PnlEditor.Width = 95 * (DEFAULT_FP_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100;
                //if (ExtGridPanel.MODE_EDIT.Equals(HttpContext.Current.Session[ExtGridPanel.SESSION_MODE]))
                if (UtilityExt.IsModeEdit() || (mode == GlobalAsp.FILTER))
                {
                    PnlEditor.Enabled = pr.Editable;
                }
                Items.Add(PnlEditor);
                #endregion ParameterRow.MODE_HTML
            }
            else if ((pr.ModeEdit == ParameterRow.MODE_TEMUAN))
            {
                #region Temuan
                Ext.Net.Panel PnlEditor = new Ext.Net.Panel
                {
                    ID = "pnlEditor" + pr.Name,
                    PType = pr.Name
                };
                PnlEditor.AutoLoad.Url = UtilityExt.ValidateURL(this, string.Format("~/PageEWP/KKP_Temuan2.aspx?app=" + GlobalAsp.GetRequestApp() + "&roleid=" + HttpContext.Current.Request["roleid"] + "&id=" + GlobalAsp.GetRequestId()));
                PnlEditor.AutoLoad.NoCache = true;
                PnlEditor.AutoLoad.Mode = LoadMode.IFrame;
                PnlEditor.LoadContent();
                PnlEditor.AnchorHorizontal = "100%";
                PnlEditor.Width = 95 * (DEFAULT_FP_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100;
                Items.Add(PnlEditor);
                #endregion
            }
            else if ((pr.ModeEdit == ParameterRow.MODE_NUMERIC))
            {
                #region ParameterRow.MODE_ENTRY
                TextField textField = new TextField
                {
                    ID = (mode == GlobalAsp.FILTER) ? "textFilter" + pr.Name : "textEntry" + pr.Name,
                    ToolTip = pr.Hint,
                    DataIndex = pr.Name,
                    AnchorHorizontal = "100%"
                };
                textField.Style.Add("text-align", "right");
                textField.MaskRe = @"/[0-9\,]/";
                textField.MaxLength = pr.Length;
                textField.MaxLengthText = "Jumlah karakter tidak boleh lebih dari {0} karakter";
                textField.InputType = (pr.Name.ToLower().Contains("pwd")) ? InputType.Password : InputType.Text;
                textField.Width = pr.Width * (DEFAULT_FP_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100;
                textField.AllowBlank = pr.AllowEmpty;
                //if (pr.Listeners.Count > 0)
                //{
                //  foreach (KeyValuePair<ParameterRowEvent, string> rowEvent in pr.Listeners)
                //  {
                //    if (rowEvent.Key.Equals(ParameterRowEvent.Blur))
                //    {
                //      textField.Listeners.Blur.Handler = rowEvent.Value;
                //    }
                //  }
                //}

                //if (ExtGridPanel.MODE_EDIT.Equals(HttpContext.Current.Session[ExtGridPanel.SESSION_MODE]))
                if (UtilityExt.IsModeEdit() || (mode == GlobalAsp.FILTER))
                {
                    textField.ReadOnly = !pr.Editable;
                    textField.Selectable = pr.Editable;
                    Items.Add(textField);
                }
                else
                {
                    Items.Add(textField);
                }

                #endregion ParameterRow.MODE_ENTRY

            }
            else if ((pr.ModeEdit == ParameterRow.MODE_SELECT))
            {
                #region ParameterRow.MODE_SELECT
                ComboBox comboBox = new ComboBox
                {
                    ID = (mode == GlobalAsp.FILTER) ? "textFilter" + pr.Name : "textEntry" + pr.Name,
                    DataIndex = pr.Name,
                    EmptyText = GlobalAsp.SELECT + " " + pr.Label,
                    AutoFocus = false,
                    ForceSelection = false,
                    AutoScroll = false,
                    SelectOnFocus = false,
                    Editable = false,
                    Enabled = pr.Enable,
                    ReadOnly = !pr.Enable,
                    TypeAhead = true,
                    Mode = DataLoadMode.Local,
                    TriggerAction = TriggerAction.All
                };
                comboBox.ForceSelection = true;
                comboBox.AnchorHorizontal = "100%";
                comboBox.AllowBlank = pr.AllowEmpty;
                string[] fields = pr.KeyValue.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if ((pr.KeyValue != null) && (pr.KeyValue != ""))
                {
                    comboBox.DisplayField = fields[1];
                    comboBox.ValueField = fields[0];
                    ExtStore store = new ExtStore(pr.Name, fields, (IList)pr.Values);
                    if (pr.Parent != "")
                    {
                        store.BaseParams.Add(new Ext.Net.Parameter() { Name = pr.Parent, Mode = ParameterMode.Raw, Value = "#{CB" + pr.Parent + "}.getValue()" });
                    }
                    store.RefreshData += new Store.AjaxRefreshDataEventHandler(store_RefreshData);
                    comboBox.Store.Add(store);
                }
                else
                {
                    ListItemCollection vals = (ListItemCollection)pr.Values;
                    foreach (Ext.Net.ListItem item in vals)
                    {
                        comboBox.Items.Add(item);
                    }
                }
                comboBox.Width = pr.Width * (DEFAULT_FP_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100;
                if (pr.AllowRefresh)
                {
                    comboBox.Listeners.Select.Handler = "setComboValue('" + pr.Name + "',record.data." + fields[0] + ");";//aneh
                    comboBox.Listeners.Select.Handler += (pr.Child == "") ? "" : "#{CB" + pr.Child + "}.clearValue();#{CB" + pr.Child + "}.store.reload();";
                    if (mode == GlobalAsp.FILTER)
                    {
                        comboBox.Listeners.Select.Handler += $"selectclick('{pr.Name}',{target});";
                        //comboBox.Listeners.Change.Handler =
                        //((target == ExtGridPanelFilter.GRID) ?
                        //  "  Store1.reload();" :/*analisis dulu: gimana klo asinkronus?trus id and displaytext?*/
                        //  "  refreshTree();");
                    }
                }
                Items.Add(comboBox);
                #endregion ParameterRow.MODE_SELECT
            }
            else if ((pr.ModeEdit == ParameterRow.MODE_RADIO))
            {
                #region ParameterRow.MODE_RADIO
                IList values = (IList)pr.Values;
                string[] strs = null;
                if (values != null)
                {
                    strs = pr.KeyValue.Split(new string[] { "=" }, StringSplitOptions.None);
                }
                RadioGroup radioGroup = new RadioGroup
                {
                    ID = pr.Name.Replace(",", string.Empty),
                    DataIndex = pr.Name,
                    AutoWidth = true,
                    ColumnsNumber = values.Count
                };
                //radioGroup.ColumnsWidths = new string[] { "75", "75" };
                for (int i = 0; i < values.Count; i++)
                {
                    Radio radio1 = new Radio
                    {
                        ID = (mode == GlobalAsp.FILTER) ? "textFilter" + pr.Name : "textEntry" + pr.Name,
                        Width = new Unit(75, UnitType.Pixel),
                        DataIndex = pr.Name,
                        BoxLabel = (string)values[i].GetType().GetProperty(strs[1]).GetValue(values[i], null),//"H";
                        InputValue = (string)values[i].GetType().GetProperty(strs[0]).GetValue(values[i], null)//"H";
                    };
                    radioGroup.Items.Add(radio1);
                }
                Items.Add(radioGroup);
                #endregion ParameterRow.MODE_RADIO
            }
            else if ((pr.ModeEdit == ParameterRow.MODE_TYPE))
            {
                #region ParameterRow.MODE_TYPE
                IList values = (IList)pr.Values;
                string[] strs = null;
                if (values != null)
                {
                    strs = pr.KeyValue.Split(new string[] { "=" }, StringSplitOptions.None);
                }
                RadioGroup radioGroup = new RadioGroup
                {
                    ID = ((mode == GlobalAsp.FILTER) ? "textFilter" + pr.Name : "textEntry" + pr.Name).Replace(",", string.Empty),
                    DataIndex = pr.Name,
                    AutoWidth = true,
                    //radioGroup.ColumnsNumber = 2;
                    ColumnsWidths = new string[] { "75", "75" }
                };
                Radio radio1 = new Radio
                {
                    ID = (mode == GlobalAsp.FILTER) ? "textFilterValueH" + pr.Name : "textEntryValueH" + pr.Name,
                    Width = new Unit(75, UnitType.Pixel),
                    DataIndex = pr.Name,
                    BoxLabel = (values == null) ? "Header" : (string)values[0].GetType().GetProperty(strs[1]).GetValue(values[0], null),//"H";
                    InputValue = (values == null) ? "H" : (string)values[0].GetType().GetProperty(strs[0]).GetValue(values[0], null)//"H";
                };
                radioGroup.Items.Add(radio1);
                Radio radio2 = new Radio
                {
                    ID = (mode == GlobalAsp.FILTER) ? "textFilterValueD" + pr.Name : "textEntryValueD" + pr.Name,
                    Width = new Unit(75, UnitType.Pixel),
                    DataIndex = pr.Name,
                    BoxLabel = (values == null) ? "Detil" : (string)values[1].GetType().GetProperty(strs[1]).GetValue(values[1], null),//"D";
                    InputValue = (values == null) ? "D" : (string)values[1].GetType().GetProperty(strs[0]).GetValue(values[1], null)//"D";
                };
                radioGroup.Items.Add(radio2);

                Items.Add(radioGroup);
                #endregion ParameterRow.MODE_TYPE
            }
            else if ((pr.ModeEdit == ParameterRow.MODE_YESNO))
            {
                #region ParameterRow.MODE_YESNO
                IList values = (IList)pr.Values;
                string[] strs = null;
                if (values != null)
                {
                    strs = pr.KeyValue.Split(new string[] { "=" }, StringSplitOptions.None);
                }

                RadioGroup radioGroup = new RadioGroup
                {
                    ID = ((mode == GlobalAsp.FILTER) ? "textFilter" + pr.Name : "textEntry" + pr.Name).Replace(",", string.Empty),
                    DataIndex = pr.Name,
                    ColumnsWidths = new string[] { "75", "75" }
                };
                Radio radio1 = new Radio
                {
                    ID = "radioYa" + pr.Name,
                    DataIndex = pr.Name,
                    BoxLabel = (values == null) ? "Ya" : (string)values[0].GetType().GetProperty(strs[1]).GetValue(values[0], null),//"H";
                    InputValue = (values == null) ? "1" : (string)values[0].GetType().GetProperty(strs[0]).GetValue(values[0], null)//"H";
                };
                radioGroup.Items.Add(radio1);
                Radio radio2 = new Radio
                {
                    ID = "radioTidak" + pr.Name,
                    DataIndex = pr.Name,
                    BoxLabel = (values == null) ? "Tidak" : (string)values[1].GetType().GetProperty(strs[1]).GetValue(values[1], null),//"D";
                    InputValue = (values == null) ? "0" : (string)values[1].GetType().GetProperty(strs[0]).GetValue(values[1], null)//"D";
                };
                radioGroup.Items.Add(radio2);

                if (pr.AllowRefresh)
                {
                    radioGroup.Listeners.Change.Handler = @"
            if(!!CoreNET.Methods1)
            {
              CoreNET.Methods1.btnRadioClick();
            }
          ";
                }
                Items.Add(radioGroup);
                #endregion ParameterRow.MODE_YESNO
            }
            else if ((pr.ModeEdit == ParameterRow.MODE_FILEUPLOAD))
            {
                #region ParameterRow.MODE_FILEUPLOAD
                FileUploadField fufield = new FileUploadField
                {
                    ID = "FileUploadField1",
                    DataIndex = pr.Name,
                    Width = 90 * (DEFAULT_FP_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100
                };
                Items.Add(fufield);
                #endregion ParameterRow.MODE_FILEUPLOAD
            }
            else
            {
            }
        }

        public void store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            IDataControlUI dc = UtilityUI.GetDataControl(GlobalAsp.GetRequestI());
            dc.GetProperty(e.Parameters[0].Name).SetValue(dc, e.Parameters[0].Value, null);
        }
        public static void GetValues(FormPanel formPanel, IDataControlUI dc)
        {
            if (formPanel == null)
            {
                return;
            }
            for (int i = 0; i < formPanel.Items.Count; i++)
            {
                Ext.Net.Component item = formPanel.Items[i];
                ItemsCollection<Ext.Net.Component> Items = null;
                if (typeof(CompositeField).IsInstanceOfType(item))
                {
                    CompositeField field = (CompositeField)item;
                    Items = field.Items;
                    GetValues(Items, dc);
                }
                else if (typeof(Ext.Net.FormPanel).IsInstanceOfType(item))
                {
                    Ext.Net.FormPanel field = (Ext.Net.FormPanel)item;
                    GetValues(field, dc);//recursive
                }
                else//Component
                {
                    GetValue(item, dc);
                }
            }
        }
        public static void GetValues(ItemsCollection<Ext.Net.Component> Items, IDataControlUI dc)
        {
            for (int j = 0; j < Items.Count; j++)
            {
                Ext.Net.Component c = Items[j];
                if (typeof(Ext.Net.Panel).IsInstanceOfType(c))
                {
                    GetValues(((Ext.Net.Panel)c).Items, dc);
                }
                else if (typeof(Container).IsInstanceOfType(c))
                {
                    GetValues(((Container)c).Items, dc);
                }
                else if (typeof(FieldSet).IsInstanceOfType(c))
                {
                    GetValues(((FieldSet)c).Items, dc);
                }
                else if (typeof(CompositeField).IsInstanceOfType(c))
                {
                    GetValues(((CompositeField)c).Items, dc);
                }
                else
                {
                    GetValue(c, dc);
                }
            }
        }
        public static void GetValue(Ext.Net.Component c, IDataControlUI dc)
        {
            PropertyInfo propDataIndex = c.GetType().GetProperty("DataIndex");
            if (propDataIndex != null)
            {
                string propname = (string)propDataIndex.GetValue(c, null);
                PropertyInfo prop = dc.GetProperty(propname);
                if (prop != null)
                {
                    object val = dc.GetValue(propname);

                    //switch (c.GetType())
                    //{
                    //  case typeof(FileUploadField):
                    //    FileUploadField ctrl = ((FileUploadField)c);
                    //    val = (ctrl.HasFile) ? Path.GetFileName(ctrl.PostedFile.FileName) : "";
                    //    break;
                    //  default:
                    //    val = ((TextFieldBase)c).Text;
                    //    break;
                    //}
                    if (c.GetType().BaseType == typeof(TextFieldBase))
                    {
                        if (c.GetType() == typeof(FileUploadField))
                        {
                            FileUploadField ctrl = ((FileUploadField)c);
                            val = (ctrl.HasFile) ? Path.GetFileName(ctrl.PostedFile.FileName) : "";
                        }
                        else
                        {
                            val = ((TextFieldBase)c).Text;
                        }
                    }
                    else if (c.GetType() == typeof(NumberField))
                    {
                        val = ((NumberField)c).Value;
                    }
                    else if (c.GetType() == typeof(DateField))
                    {
                        DateTime prevdate = (DateTime)val;
                        if (prevdate != null && (prevdate != new DateTime()))
                        {
                            val = ((DateField)c).SelectedDate;
                            if (prevdate.Year != ((DateTime)val).Year)
                            {
                                bool allowMultiYear = ((ViewListProperties)dc.GetProperties()).AllowMultiYear;
                                if (!allowMultiYear)
                                {
                                    throw new Exception(ConstantDictExt.Translate("LBL_ERROR_OUTBOUND_DATE"));
                                }
                            }
                        }
                        val = ((DateField)c).Value.ToString();

                    }
                    else if (c.GetType() == typeof(ComboBox))
                    {
                        val = ((ComboBox)c).Text;
                    }
                    else if (c.GetType() == typeof(RadioGroup))
                    {
                        if (((RadioGroup)c).Enabled && !((RadioGroup)c).ReadOnly)
                        {
                            ((RadioGroup)c).CheckedItems.ForEach(delegate (Radio radio)
                            {
                                val = radio.InputValue;
                            });
                        }
                        else
                        {
                            val = dc.GetProperty(propname).GetValue(dc, null);
                        }
                    }
                    else if (c.GetType() == typeof(Checkbox))
                    {
                        val = ((Checkbox)c).Checked;
                    }

                    UtilityExt.SetValueFromUI(dc, prop, val);
                }
            }
        }
        public static void SetEnableFilter(IDataControlUI dc, bool enable)
        {
            HashTableofParameterRow hps = dc.GetFilters();
            foreach (ParameterRow pr in hps.Values)
            {
                pr.SetEnable(enable);
            }
        }
        /*
         * Komponen di formpanel diset nilainya dari dc
         */
        public static void SetValues(FormPanel formPanel, IDataControlUI dc)
        {
            if (MasterAppConstants.Instance.StatusTesting)
            {
                ArrayList naProps = new ArrayList();
                CekProps(naProps, formPanel.Items, dc);
                if (naProps.Count > 0)
                {
                    string props = (string)naProps[0];
                    for (int i = 1; i < naProps.Count; i++)
                    {
                        props += "," + naProps[i];
                    }
                    throw new Exception("This properties " + props + " not exist!");
                }
            }
            for (int i = 0; i < formPanel.Items.Count; i++)
            {
                Ext.Net.Component item = formPanel.Items[i];
                PropertyInfo propDataIndex = item.GetType().GetProperty("DataIndex");
                if (typeof(BoxComponentBase).IsInstanceOfType(item) &&
                  (typeof(CompositeField).IsInstanceOfType(item) ||
                  typeof(Ext.Net.FormPanel).IsInstanceOfType(item) || (propDataIndex == null))
                )
                {
                    ItemsCollection<Ext.Net.Component> Items = null;
                    if (typeof(CompositeField).IsInstanceOfType(item))
                    {
                        CompositeField field = (CompositeField)item;
                        Items = field.Items;
                        SetValues(Items, dc);
                    }
                    else if (typeof(Ext.Net.FormPanel).IsInstanceOfType(item))
                    {
                        Ext.Net.FormPanel field = (Ext.Net.FormPanel)item;
                        SetValues(field, dc);//recursive
                    }
                }
                else
                {
                    Field field = null;
                    if (typeof(Ext.Net.Field).IsInstanceOfType(item))
                    {
                        field = (Field)item;//formPanel.Items[i];
                    }
                    string propname = field.DataIndex;
                    PropertyInfo prop = dc.GetProperty(propname);
                    if (prop != null)
                    {
                        object val = prop.GetValue(dc, null);
                        if (propname.EndsWith("str"))//decimal
                        {
                            field.SetValue(val.ToString().Replace(".", ""));
                        }
                        else
                        {
                            if (prop.PropertyType == typeof(DateTime))
                            {
                                if ((DateTime)val != new DateTime())
                                {
                                    field.SetValue(val);
                                }
                            }
                            else
                            {
                                field.SetValue(val);
                            }
                        }
                    }

                }
            }
        }

        private static void CekProps(ArrayList naProps, ItemsCollection<Ext.Net.Component> Items, IDataControlUI dc)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Ext.Net.Component item = Items[i];
                if (typeof(CoreNETCompositeField).IsInstanceOfType(item))
                {
                    ItemsCollection<Ext.Net.Component> cfItems = ((CoreNETCompositeField)item).Items;
                    CekProps(naProps, cfItems, dc);
                }
                else if (typeof(FormPanel).IsInstanceOfType(item))
                {
                    ItemsCollection<Ext.Net.Component> cfItems = ((FormPanel)item).Items;
                    CekProps(naProps, cfItems, dc);
                }
                else if (typeof(Ext.Net.Panel).IsInstanceOfType(item))
                {
                    ItemsCollection<Ext.Net.Component> cfItems = ((Ext.Net.Panel)item).Items;
                    CekProps(naProps, cfItems, dc);
                }
                else
                {
                    if (item.GetType().GetProperty("DataIndex") != null)
                    {
                        string pname = (string)item.GetType().GetProperty("DataIndex").GetValue(item, null);
                        if (!string.IsNullOrEmpty(pname) && !pname.Contains(","))
                        {
                            if (dc.GetProperty(pname) == null)
                            {
                                naProps.Add(pname);
                            }
                        }
                    }
                }
            }
        }
        public static void SetValues(ItemsCollection<Ext.Net.Component> Items, IDataControlUI dc)
        {
            string[] notfields = new string[] { };
            ArrayList ArNotFields = new ArrayList(notfields);
            for (int j = 0; j < Items.Count; j++)
            {
                Ext.Net.Component c = Items[j];
                if (typeof(Ext.Net.Panel).IsInstanceOfType(c))
                {
                    SetValues(((Ext.Net.Panel)c).Items, dc);
                }
                else if (typeof(Container).IsInstanceOfType(c))
                {
                    SetValues(((Container)c).Items, dc);
                }
                else if (typeof(FieldSet).IsInstanceOfType(c))
                {
                    SetValues(((FieldSet)c).Items, dc);
                }
                else if (typeof(CompositeField).IsInstanceOfType(c))
                {
                    SetValues(((CompositeField)c).Items, dc);
                }
                else if ((c.GetType() != typeof(Ext.Net.Button)) && (c.GetType() != typeof(Ext.Net.Label)))
                {
                    string propname = (string)c.GetType().GetProperty("DataIndex").GetValue(c, null);
                    if (ArNotFields.IndexOf(propname) == -1)
                    {
                        //if (dc.GetProperty(propname) == null)
                        //{
                        //  throw new Exception("Property " + propname + " pada kelas " + dc.GetType().FullName + " tidak ada (CoreNETCompositeField.cs line 659)");
                        //}
                        if (dc.GetProperty(propname) != null)
                        {
                            object val = dc.GetProperty(propname).GetValue(dc, null);

                            if (c.GetType() == typeof(TextField))
                            {
                                if (propname.EndsWith("str"))//decimal
                                {
                                    ((TextField)c).SetValue(val == null ? "" : val);
                                    //((TextField)c).SetValue(val == null ? "" : val.ToString().Replace(".", ""));
                                }
                                else if (dc.GetProperty(propname).PropertyType == typeof(DateTime))
                                {
                                    ((TextField)c).SetValue((DateTime)val == new DateTime() ? "" : ((DateTime)val).ToString("dd/MM/yyyy"));
                                }
                                else
                                {
                                    ((TextField)c).SetValue(val == null ? "" : val);
                                }
                            }
                            else if (c.GetType() == typeof(Ext.Net.Label))
                            {
                                ((Ext.Net.Label)c).Text = (string)(val == null ? "" : val);
                            }
                            else if (c.GetType() == typeof(NumberField))
                            {
                                ((NumberField)c).SetValue(val == null ? "" : val);
                            }
                            else if (c.GetType() == typeof(TextArea))
                            {
                                ((TextArea)c).SetValue(val == null ? "" : val);
                            }
                            else if (c.GetType() == typeof(DateField))
                            {
                                int tahun = (int)dc.GetValue(GlobalAsp.TAHUN);
                                if (tahun == 0)
                                {
                                    tahun = DateTime.Now.Year;
                                }
                                DateTime now = new DateTime(tahun, DateTime.Now.Month, DateTime.Now.Day);
                                ((DateField)c).SetValue((val == null) || (val.Equals(new DateTime())) ? now : val);
                            }
                            else if (c.GetType() == typeof(FileUploadField))
                            {
                                ((FileUploadField)c).SetValue(val == null ? "" : val);
                            }
                            else if (c.GetType() == typeof(RadioGroup))
                            {
                                if ((val != null) && (((string)val) != ""))
                                {
                                    ((RadioGroup)c).SetValue(val);
                                }
                            }
                            else if (c.GetType() == typeof(Checkbox))
                            {
                                if (val != null)
                                {
                                    ((Checkbox)c).SetValue(val);
                                }
                            }
                            else if (c.GetType() == typeof(ComboBox))
                            {
                                if ((val == null) || (val.Equals("")))
                                {
                                    ((ComboBox)c).SelectedIndex = -1;
                                }
                                else
                                {
                                    ((ComboBox)c).SetValue(val == null ? "" : val);
                                }
                            }
                        }
                    }
                }
                else if (c.GetType() == typeof(Ext.Net.Label))
                {
                    string propname = (string)c.GetType().GetProperty("ID").GetValue(c, null);
                    PropertyInfo prop = dc.GetProperty(propname);
                    object val = prop.GetValue(dc, null);
                    if (prop.PropertyType.Equals(typeof(DateTime)))
                    {
                        ((Ext.Net.Label)c).Text = ((val == null) || (val.Equals(new DateTime())) ? "" : ((DateTime)val).ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        ((Ext.Net.Label)c).Text = (string)(val == null ? "" : val);
                    }
                }
            }
        }

        public static void SetWhenAdd(FormPanel formPanel, IDataControlUIEntry dc)
        {
            ArrayList array = new ArrayList(dc.GetEntries().GetOrderKeys());
            SetBoolProperty(array, formPanel, dc.GetEntries(), false);

            array = dc.GetEntries().GetDisableKeys();
            SetBoolProperty(array, formPanel, dc.GetEntries(), true);
        }
        public static void SetDisableAll(FormPanel formPanel, IDataControlUIEntry dc)
        {
            ArrayList array = new ArrayList(dc.GetEntries().GetOrderKeys());
            SetBoolProperty(array, formPanel, dc.GetEntries(), true);
        }
        public static void SetWhenEdit(FormPanel formPanel, IDataControlUIEntry dc)
        {
            ArrayList array = new ArrayList(dc.GetEntries().GetOrderKeys());
            SetBoolProperty(array, formPanel, dc.GetEntries(), true);

            ArrayList array1 = dc.GetEntries().GetEnableKeys();
            ArrayList array2 = dc.GetEntries().GetEditable();
            ArrayList newarray = new ArrayList();
            foreach (string val in array)
            {
                if (array1.Contains(val) && array2.Contains(val))
                {
                    newarray.Add(val);
                }
            }
            SetBoolProperty(newarray, formPanel, dc.GetEntries(), false);
        }
        public static void SetBoolProperty(ArrayList array, ItemsCollection<Ext.Net.Component> Items, bool ro)//ro readonly
        {
            for (int j = 0; j < Items.Count; j++)
            {
                Ext.Net.Component c = Items[j];
                if (c.GetType() == typeof(Ext.Net.Panel))
                {
                    string propname = (string)c.GetType().GetProperty("PType").GetValue(c, null);
                    ((Ext.Net.Panel)c).Enabled = !ro;
                }
                else if ((c.GetType() != typeof(Ext.Net.Button)) && (c.GetType() != typeof(Ext.Net.Label)))
                {
                    string propname = (string)c.GetType().GetProperty("DataIndex").GetValue(c, null);

                    if (c.GetType() == typeof(TextField))
                    {
                        ((TextField)c).ReadOnly = array.Contains(propname) ? ro : !ro;
                        ((TextField)c).Selectable = !((TextField)c).ReadOnly;
                    }
                    else if (c.GetType() == typeof(NumberField))
                    {
                        ((NumberField)c).ReadOnly = array.Contains(propname) ? ro : !ro;
                        ((NumberField)c).Selectable = !((NumberField)c).ReadOnly;
                    }
                    else if (c.GetType() == typeof(DateField))
                    {
                        if (array.Count > 0)
                        {
                            if (!array.Contains("Tgl1") && !array.Contains("Tgl2"))
                            {
                                ro = (array.Contains(propname) ? ro : !ro);
                            }
                            else
                            {
                                ro = (((array.Contains("Tgl1") || array.Contains("Tgl2")) && (propname.Equals("Tgl1") || propname.Equals("Tgl2"))) ? ro : !ro);
                            }

                            ((DateField)c).ReadOnly = ro;
                            ((DateField)c).Selectable = !((DateField)c).ReadOnly;
                        }
                    }
                    else if (c.GetType() == typeof(TextArea))
                    {
                        ((TextArea)c).ReadOnly = array.Contains(propname) ? ro : !ro;
                        ((TextArea)c).Selectable = !((TextArea)c).ReadOnly;
                    }
                    else if (c.GetType() == typeof(FileUploadField))
                    {
                        ((FileUploadField)c).ReadOnly = array.Contains(propname) ? ro : !ro;
                        ((FileUploadField)c).Selectable = !((FileUploadField)c).ReadOnly;
                    }
                    else if (c.GetType() == typeof(RadioGroup))
                    {
                        ((RadioGroup)c).ReadOnly = array.Contains(propname) ? ro : !ro;
                        ((RadioGroup)c).Selectable = !((RadioGroup)c).ReadOnly;
                    }
                    else if (c.GetType() == typeof(Checkbox))
                    {
                        ((Checkbox)c).ReadOnly = array.Contains(propname) ? ro : !ro;
                        ((Checkbox)c).Selectable = !((Checkbox)c).ReadOnly;
                    }
                    else if (c.GetType() == typeof(ComboBox))
                    {
                        ((ComboBox)c).ReadOnly = array.Contains(propname) ? ro : !ro;
                        ((ComboBox)c).Selectable = !((ComboBox)c).ReadOnly;
                    }
                }
                else
                {

                }
            }
        }
        public static void SetBoolProperty(ArrayList array, FormPanel formPanel, HashTableofParameterRow hps, bool ro)//ro readonly
        {
            for (int i = 0; i < formPanel.Items.Count; i++)
            {
                Ext.Net.Component item = formPanel.Items[i];
                if (typeof(BoxComponentBase).IsInstanceOfType(item) && (item.GetType() != typeof(TextField)))
                {
                    ItemsCollection<Ext.Net.Component> Items = null;
                    bool valid = true;
                    if (typeof(CoreNETCompositeField).IsInstanceOfType(item))
                    {
                        CoreNETCompositeField field = (CoreNETCompositeField)item;
                        valid = field.ModeEdit != ParameterRow.MODE_LOOKUP2;
                        Items = field.Items;
                        if (valid)
                        {
                            SetBoolProperty(array, Items, ro);
                        }
                        else
                        {
                            Ext.Net.Button btn = (Ext.Net.Button)field.Items[1];
                            btn.Disabled = array.Contains(field.DataIndex) ? ro : !ro;
                            btn.Enabled = !btn.Disabled;
                            ParameterRow pr = (ParameterRow)hps[field.DataIndex];
                            if (pr != null && pr.AlwaysEnable)
                            {
                                btn.Disabled = false;
                                btn.Enabled = !btn.Disabled;
                            }
                        }
                    }
                    else if (typeof(Ext.Net.FormPanel).IsInstanceOfType(item))
                    {
                        Ext.Net.FormPanel field = (Ext.Net.FormPanel)item;
                        Items = field.Items;
                        SetBoolProperty(array, field, hps, ro);
                        //for (int j = 0; j < Items.Count; j++)
                        //{
                        //  CoreNETCompositeField childItem = (CoreNETCompositeField)Items[j];
                        //  ItemsCollection<Ext.Net.Component> ChildItems = childItem.Items;
                        //  SetBoolProperty(array, ChildItems, hps, ro);
                        //}
                    }
                }
                else
                {
                    if (item.GetType() == typeof(TextField))
                    {
                        string propname = ((TextField)item).DataIndex;
                        ((TextField)item).ReadOnly = array.Contains(propname) ? ro : !ro;
                        ((TextField)item).Selectable = !((TextField)item).ReadOnly;
                    }
                }
            }
        }
        public static void SetVisibleAll(FormPanel formPanel)
        {
            for (int i = 0; i < formPanel.Items.Count; i++)
            {
                Ext.Net.Component item = formPanel.Items[i];
                item.Hidden = false;
            }
        }
        public static void SetVisibleAll(FormPanel formPanel, string[] array)
        {
            for (int i = 0; i < formPanel.Items.Count; i++)
            {
                Ext.Net.Component item = formPanel.Items[i];
                if (item.GetType() == typeof(CoreNETCompositeField))
                {
                    CompositeField field = (CompositeField)item;
                    for (int j = 0; j < field.Items.Count; j++)
                    {
                        Ext.Net.Component c = field.Items[j];
                        if ((c.GetType() != typeof(Ext.Net.Button)) && (c.GetType() != typeof(Ext.Net.Label)) && (c.GetType() != typeof(Ext.Net.Panel)))
                        {
                            string propname = (string)c.GetType().GetProperty("DataIndex").GetValue(c, null);
                            foreach (string s in array)
                            {
                                if (s.Equals(propname))
                                {
                                    field.Hidden = false;
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void SetHideWhenEdit(FormPanel formPanel, HashTableofParameterRow hps)
        {
            ArrayList array = hps.GetHideWhenEditKeys();

            for (int i = 0; i < formPanel.Items.Count; i++)
            {
                Ext.Net.Component item = formPanel.Items[i];
                if (item.GetType() == typeof(CoreNETCompositeField))
                {
                    CoreNETCompositeField field = (CoreNETCompositeField)item;//formPanel.Items[i];

                    for (int j = 0; j < field.Items.Count; j++)
                    {
                        Ext.Net.Component c = field.Items[j];
                        if (c.GetType() == typeof(Ext.Net.Panel))
                        {
                            string propname = (string)c.GetType().GetProperty("PType").GetValue(c, null);
                            if (array.Contains(propname))
                            {
                                //field.Visible = false;
                                field.Hidden = true;
                            }
                        }
                        else if ((c.GetType() != typeof(Ext.Net.Button)) && (c.GetType() != typeof(Ext.Net.Label)))
                        {
                            string propname = (string)c.GetType().GetProperty("DataIndex").GetValue(c, null);

                            if (array.Contains(propname))
                            {
                                //field.Visible = false;
                                field.Hidden = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
