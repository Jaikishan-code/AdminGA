using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AdminGA.Entity.Student
{
   
        [Table("GA_STDCLASS", Schema = "STD")]
        public class GA_STDCLASS
        {
            [Key]
            public int STD_ID { get; set; }

            public Guid STD_CLID { get; set; }

            public string STD_CLNAME { get; set; }
        }
    
}
