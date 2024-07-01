using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ATSAudit.DTOs
{
    //Id is dynamic because it can be a string or an int
    // public record ActionItemDTO(string? Form, string? Subform, string Id);

    public record struct ActionItemDTO(string Form, string CPARId, string Subform, string Id)
    {
        // public ActionItemDTO(string form, string subform, string id) : this(form, subform, id) {}
        public ActionItemDTO(string Form, int CPARId, string Subform, int Id) : this(Form, CPARId.ToString(), Subform, Id.ToString()) {}
        public ActionItemDTO(string Form, int CPARId, string Subform, string Id ) : this(Form, CPARId.ToString(), Subform, Id) {}
    }

    public record struct DeleteActionItemDTO(string Form, string CPARId, string Subform, string Id, string Filename);
}