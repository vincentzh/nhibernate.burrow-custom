<%@ Application Language="C#" %>
<%@ Import Namespace="System.IO" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        log4net.Config.XmlConfigurator.Configure();
        DirectoryInfo dir = new DirectoryInfo(this.Server.MapPath("~/App_Data/BurrowState/"));
        if (!dir.Exists)
            dir.Create();
        else
        {
            DateTime nt = DateTime.Now.AddHours(-1);
            foreach (FileInfo f in dir.GetFiles())
            {
                if (f.CreationTime < nt)
                    f.Delete();
            }
        }
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown
       Shutdown();

    }

    private void Shutdown()
    {
        log4net.LogManager.Shutdown();
    }

    void Application_Error(object sender, EventArgs e) 
    { 
       Shutdown();

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
