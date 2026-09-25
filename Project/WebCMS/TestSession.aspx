<%@ Page Language="C#" %>
<%@ Import Namespace="System.IO" %>
<script runat="server">
    private string logPath = @"C:\temp\session_test.txt";

    private void WriteLog(string msg)
    {
        try { File.AppendAllText(logPath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - " + msg + Environment.NewLine); }
        catch { }
    }

    void Page_Load(object sender, EventArgs e)
    {
        WriteLog("TestSession Page_Load dimulai");
        WriteLog("SessionID: " + Session.SessionID);
        WriteLog("IsNewSession: " + Session.IsNewSession);

        // Coba simpan data ke session
        Session["TestKey"] = "Hello World - " + DateTime.Now.ToString();
        WriteLog("Data disimpan ke session: " + Session["TestKey"]);

        // Tampilkan di response
        Response.Write("<h1>Test Session</h1>");
        Response.Write("SessionID: " + Session.SessionID + "<br/>");
        Response.Write("IsNewSession: " + Session.IsNewSession + "<br/>");
        Response.Write("Data di session: " + Session["TestKey"] + "<br/>");
        Response.Write("<a href='TestSession.aspx'>Refresh halaman</a>");
        Response.Write("<hr/>");
        Response.Write("<a href='MainMenu.aspx?app=1847B4EE-619F-45FE-9F5F-FD911B172601&key=P+sWcwytsBjczGtqZ6MWlQ==&sub=&kdapp=01.04.04.01.&frame='>Coba ke MainMenu</a>");
    }
</script>