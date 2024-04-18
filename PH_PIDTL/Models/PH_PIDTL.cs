using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polynic.Models
{
    public class PH_PIDTL
    {

        public string? remark2 { get; set; }
        public string? itemcode { get; set; }
        public string? description { get; set; }
        public string? description2 { get; set; }
        public string? batch { get; set; }
        public string? location { get; set; }
        public decimal qty { get; set; }
        public string? uom { get; set; }
        public int id { get; set; }
        public DateTimeOffset checkout { get; set; }
        public DateTimeOffset? checkin { get; set; }
        public int qtyremain { get; set; }

    }
}
