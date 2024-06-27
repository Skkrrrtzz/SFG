using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ATSAudit.DTOs
{
    //Id is dynamic because it can be a string or an int
    // public record ActionItemDTO(string? Form, string? Subform, string Id);

    public record ActionItemDTO(string? Form, string? Subform , string? Id)
    {
        // public ActionItemDTO(string form, string subform, string id) : this(form, subform, id) {}
        public ActionItemDTO(string form, string subform, int id) : this(form, subform, id.ToString()) {}
    }
}