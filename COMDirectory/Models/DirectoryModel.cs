namespace COMDirectory.Models
{
    public class DirectoryModel
    {
        public int itemno { get; set; }
        public int directoryid { get; set; }
        public int empcode { get; set; }
        public string empbu { get; set; }
        public string fullname { get; set; }
        public string empposition { get; set; }
        public string localno { get; set; }
        public string encodeby { get; set; }
        public string email { get; set; }
        public byte[] empbyte { get; set; }
    }

    public class DirectoryAcknowledgeModel
    {
        public int acknowledgeid { get; set; }
        public string pdfname { get; set; }
        public string pdfrev { get; set; }
        public string empname { get; set; }
        public DateTime? acknowledgedate { get; set; }

    }

    public class DirectoryReportModel
    {
        public int acknowledgeid { get; set; }
        public string pdfname { get; set; }
        public int pdfrev { get; set; }
        public string empname { get; set; }
        public DateTime? acknowledgedate { get; set; }

    }

}
