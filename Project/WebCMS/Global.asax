<%@ Application Language="C#" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        CoreNET.Common.Base.MasterAppConstants.Instance.StatusTesting = (ConfigurationManager.AppSettings["Testing"] == "1");

        CoreNET.Common.Base.MasterAppConstants.Instance.SetValue(
            CoreNET.Common.Base.MasterAppConstants.URLBASE, ConfigurationManager.AppSettings["URLBase"]);

        // Inisialisasi DataSource aman (tanpa membaca token terenkripsi dari GlobalAsp)
        try
        {
            CoreNET.Common.Base.SQLDataSource.Instance.SetDataSourceConfig(null);
        }
        catch (Exception ex)
        {
            try
            {
                System.IO.File.AppendAllText(@"C:\temp\startup_error.txt", "Application_Start Error: " + ex.ToString() + Environment.NewLine);
            }
            catch { }
        }

        // Konfigurasi IBatis
        try
        {
            IBatisNet.DataAccess.Configuration.DomDaoManagerBuilder builder = new IBatisNet.DataAccess.Configuration.DomDaoManagerBuilder();
            builder.Configure("dao.config");
        }
        catch (Exception ex)
        {
            try
            {
                System.IO.File.AppendAllText(@"C:\temp\ibatis_error.txt", "IBatis Init Error: " + ex.ToString() + Environment.NewLine);
            }
            catch { }
        }
    }

    void Application_End(object sender, EventArgs e)
    {
    }

    void Application_Error(object sender, EventArgs e)
    {
        Exception ex = Server.GetLastError();
        try
        {
            System.IO.File.AppendAllText(@"C:\temp\global_error.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + ex.ToString() + Environment.NewLine);
        }
        catch { }
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Kosongkan atau biarkan saja
    }

    void Session_End(object sender, EventArgs e)
    {
    }

    // ===== TAMBAHAN: Tambahkan SameSite=Lax ke cookie session =====
    void Application_PreSendRequestHeaders(object sender, EventArgs e)
    {
        if (Request.IsSecureConnection)
            {
                var cookie = Response.Cookies["ASP.NET_SessionId"];
                if (cookie != null)
                 {
                    cookie.Secure = true;
                    cookie.HttpOnly = true;
                    cookie.Path = "/";
                    string existing = Response.Headers["Set-Cookie"];
                    if (!string.IsNullOrEmpty(existing) && !existing.Contains("SameSite"))
                    {
                         Response.Headers.Remove("Set-Cookie");
                            Response.Headers.Add("Set-Cookie", existing + "; SameSite=None");
                     }
                }   
            }
    }

</script>