using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polynic.Models
{
    public class PH_PIDTL
    {

        public string DTLKEY { get; set; }
        public string REMARK2 { get; set; }
        public string ITEMCODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string DESCRIPTION2 { get; set; }
        public string BATCH { get; set; }
        public string LOCATION { get; set; }
        public decimal QTY { get; set; }
        public string UOM { get; set; }

        

    }
}
