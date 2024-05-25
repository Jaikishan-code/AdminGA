using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminGA.Entity.Student
{
    [Table("GA_FEESMASTER", Schema = "STD")]

    public class GA_FEESMASTER
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GAFEE_ID { get; set; }

        [StringLength(50)]
        public string GAFEE_CLASS { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal GAFEE_FEES { get; set; }

        public Guid? GAFEE_MEDMID { get; set; }

        public Guid? GAFEE_STDCLID { get; set; }
    }
}
