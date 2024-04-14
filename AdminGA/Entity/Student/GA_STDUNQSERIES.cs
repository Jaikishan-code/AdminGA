using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AdminGA.Entity.Student
{
    [Table("GA_STDUNQSERIES", Schema = "MST")]
    public class GA_STDUNQSERIES
    {
        [Key]
        [Column("UNQ_ID")]
        public int UNQ_ID { get; set; }

        [Column("UNQ_STDCLASS")]
        public string UNQ_STDCLASS { get; set; }

        [Column("UNQ_CODE")]
        public string UNQ_CODE { get; set; }

        [Column("UNQ_SERIES")]
        public int UNQ_SERIES { get; set; }

        [Column("UNQ_STDCLID")]
        public Guid UNQ_STDCLID { get; set; }
    }
}
