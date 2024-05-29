using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using QA_Audit_Fresh.Models.Dto;

namespace QA_Audit_Fresh.Models
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

