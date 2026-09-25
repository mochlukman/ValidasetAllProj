using CoreNET.Common.Base;
using CoreNET.Common.BO;
using Ext.Net;
using System;
using System.Configuration;
using System.IO;

public partial class Login : System.Web.UI.Page
{
    private string logPath = @"C:\temp\login_debug.txt";

    private void WriteLog(string msg)
    {
        try { File.AppendAllText(logPath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - " + msg + Environment.NewLine); }
        catch { }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        WriteLog("Page_Load dimulai");
        CoreNET.Common.Base.AssemblyUtils.WriteBeginLog();

        GlobalExt.InitHome(Page, null);
        if (!Page.IsPostBack)
        {
            MasterAppConstants.Instance.ShowContextMenu = true;
            MasterAppConstants.Instance.ShowTranslatedLabel = true;
            string idapp = GlobalAsp.GetIdApp();
            WriteLog("idapp = " + idapp);

            if (MasterAppConstants.Instance.StatusTesting)
            {
                MasterAppConstants.Instance.ShowContextMenu = (ConfigurationManager.AppSettings["ShowContextMenu"] == "1");
                MasterAppConstants.Instance.ShowTranslatedLabel = (ConfigurationManager.AppSettings["ShowTranslatedLabel"] == "1");
            }

            if (!string.IsNullOrEmpty(idapp))
            {
                MasterAppConstants.Instance.StatusAdmin = false;
                GlobalAsp.SetConfiguration(idapp, false);

                string dcdict = (string)SsappconfigLookupControl.Instance.Params["DCDictionary"];
                if (!string.IsNullOrEmpty(dcdict) && !MasterAppConstants.Instance.DictionaryDC.Equals(dcdict))
                {
                    MasterAppConstants.Instance.SetValue(MasterAppConstants.DICTIONARYDC, dcdict);
                    ConstantDict.SetInstanceNullForReloadDict();
                }

                if (ConfigurationManager.AppSettings["ShowKodeApp"] != "1")
                {
                    Title = (string)GlobalAsp.GetSessionAppValue(MasterAppConstants.APPTITLE);
                    if (MasterAppConstants.Instance.StatusTesting) Title += " (Versi Beta)";
                }
                else
                {
                    string idappfromreq = "0";
                    string idappcosfail = "0";
                    WriteLog("Sebelum SetDataSourceConfig");
                    CoreNET.Common.Base.SQLDataSource.Instance.SetDataSourceConfig(CoreNET.Common.Base.GlobalAsp.GetDataSourceConfig());
                    WriteLog("Setelah SetDataSourceConfig");
                    // ... (kode debug lainnya)
                }
            }
            else
            {
                string msg = ConstantDict.Translate(GlobalAsp.MSG_ERRORAPP);
                X.Msg.Alert(GlobalAsp.GetConfigLabelInfo(), msg).Show();
                WriteLog("idapp kosong, keluar");
                return;
            }
        }
        WriteLog("Page_Load selesai");
    }

    [DirectMethod(Timeout = 3600000)]
    public void ProcessLogin() { }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        WriteLog("=== btnLogin_Click dimulai ===");
        string key = utxt_Code.Value;
        WriteLog("Key: " + key);

        if (string.IsNullOrEmpty(key))
        {
            WriteLog("Key kosong, tampilkan error");
            ClientScript.RegisterStartupScript(this.GetType(), "error", "alert('Key tidak valid, silakan coba lagi.');", true);
            return;
        }

        string app = GlobalAsp.GetSessionApp();
        WriteLog("SessionApp: " + app);

        try
        {
            WriteLog("Sebelum SetSessionUser");
            GlobalExt.SetSessionUser(app, key);
            WriteLog("Setelah SetSessionUser - SUKSES");
        }
        catch (Exception ex)
        {
            WriteLog("ERROR: " + ex.Message);
            ClientScript.RegisterStartupScript(this.GetType(), "error", "alert('" + ex.Message.Replace("'", "\\'") + "');", true);
            return;
        }

        string url = GlobalAsp.GetMenuURL() + "?app=" + app +
                     "&key=" + key +
                     "&sub=" + ConfigurationManager.AppSettings["IsEtalase"] +
                     "&kdapp=" + GlobalAsp.GetRequestKdapp() +
                     "&frame=" + GlobalAsp.GetRequestFrame();

        WriteLog("Redirect ke: " + url);
        Response.Redirect(url);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string sub = ConfigurationManager.AppSettings["IsEtalase"];
        string urlout = GlobalAsp.GetLogoutURL() + "?local=" + Request["local"] + "&kdapp=" + GlobalAsp.GetRequestKdapp() + "&sub=" + sub;
        Response.Redirect(urlout);
    }
}