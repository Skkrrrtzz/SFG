namespace APPCommon.Class
{
    public static class PIMESSettings
    {
        public const string databaseIP = "192.168.0.7";

        public const string hanDatabaseIP = "192.168.0.8:30015";
        public const string sqlserverDatabaseIP = "192.168.0.9";
        public const string atsDatabaseIP = "DASHBOARDPC";

        public const string mysqlDetails = ";username=imesclient;password=14J@nuary;database=pimes_system_db;sslmode=none;character set=utf8";
        public const string hanServerDetails = ";user id=SYSTEM;password=Administrator1;currentschema=PIMESLIVE;";
        public const string sqlServerDetails = "; Database=PCV55_PIMES;User Id=sa; Password=Pkunzip@112";


        public const string atsAuditDatabase = ";Initial Catalog=QA_AUDIT";
        public const string atsSFGDatabase = ";Initial Catalog=SFGDb";

        public const string atssqlServerDetails = ";Persist Security Info=True;User ID=sa;Password=test@123;Encrypt=False;Trust Server Certificate=True; MultipleActiveResultSets=true;Authentication=SqlPassword;";

        public const string mysqlConnString = "DataSource=" + databaseIP + mysqlDetails;
        public const string hanaConnString = "Server=" + hanDatabaseIP + hanServerDetails;
        public const string sqlserverConnString = "Server=" + sqlserverDatabaseIP + sqlServerDetails;


        public const string atsAuditConnString = "Data Source=" + atsDatabaseIP + atsAuditDatabase + atssqlServerDetails;
        public const string atsSFGConnString = "Data Source=" + atsDatabaseIP + atsSFGDatabase + atssqlServerDetails;


        public const string lnkLogin = "http://192.168.0.188:8081";


        public const string mailServer = "mail.pimes.com.ph";
        public const string mailAccount = "no-reply@pimes.com.ph";
        public const string mailError = "programmer@pimes.com.ph";
        public const string mailTraining = "training@pimes.com.ph";
        public const string mailSMTP = "587";
        public const string mailTestMode = "false";
        public const string mailPassword = "6NlkAodchQe";

        public const string gmailServer = "smtp.gmail.com";
        public const int gmailSMTP = 587;
        public const string gmailErrorAccount = "programmer@pimes.com.ph";
        public const string mail0Account = "noreply.pimes@gmail.com";
        public const string mail0Password = "eimknsggwdezhwyg";
        public const string mail1Account = "noreply1.pimes@gmail.com";
        public const string mail1Password = "pbufegpviubecyju";
        public const string mail2Account = "noreply2.pimes@gmail.com";
        public const string mail2Password = "xdstjltvmglhtalp";
        public const string mail3Account = "noreply3.pimes@gmail.com";
        public const string mail3Password = "chlzepiqnlhwzlzs";
        public const string mail4Account = "noreply4.pimes@gmail.com";
        public const string mail4Password = "cxpgxfhgqsxckbhw";
        public const string mail5Account = "noreply5.pimes@gmail.com";
        public const string mail5Password = "wslxgpmqigpsrumk";

        //private static string[] foundEmail = PIMESProcedures.getRandomEmail();
        //public static readonly string gmailAccount = foundEmail[0];
        //public static readonly string gmailPassword = foundEmail[1];

        public static readonly string gmailAccount = "noreply2.pimes@gmail.com";
        public static readonly string gmailPassword = "xdstjltvmglhtalp";



        //public const string serverLauncherINI = @"\\192.168.0.50\update\apps\launcher.ini";
        //public const string localLauncherINI = @"c:\PIMES App\launcher.ini";
        //public const string serverLocation = @"\\192.168.0.50\update\apps\";
        //public const string serverFile = @"\\192.168.0.50\update\apps\launcher.zip";
        //public const string localFolder = @"c:\PIMES App\";


        //public const string appsFolder = @"c:\PIMES App\Apps\";

        //public const string pdfPassword = @"wQ!RGgn6QHV+q@";
        //public const double normalWidthWindow = 1024;

        //public const string serverLauncher = @"\\192.168.0.50\update\apps\Launcher\";
        //public const string contentDB = @"\\192.168.0.50";
        //public const string contentPassword = "-@q|!5";
        //public const string contentPass = "AtVtbx";
    }
}