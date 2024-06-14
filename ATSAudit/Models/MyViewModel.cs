namespace ATSAudit.Models
{
    public class MyViewModel
    {
        public MyViewModel(bool isHidden, string colName) {
            this.isHidden = isHidden;
            this.colName = colName;
        }
        public bool isHidden { get; set; }
        public string colName { get; set; }
    }

}

