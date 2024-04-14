using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AdminGA.Entity.Student
{
    [Table("GA_STDMEDIUM", Schema = "STD")]
    public class GA_STDMEDIUM
    {
        [Key]
        [Column("MED_ID")]
        public int MED_ID { get; set; }

        [Column("MED_MID")]
        public Guid MED_MID { get; set; }

        [Column("MED_MEDIUM")]
        public string MED_MEDIUM { get; set; }
    }
}
