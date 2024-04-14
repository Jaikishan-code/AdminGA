using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminGA.Entity.Student
{
    [Table("GA_STDPREADMISSION", Schema = "STD")]
    public class GA_STDPREADMISSION
    {
        [Key]
        [Column("STD_ID")]
        public int STD_ID { get; set; }

        [Column("STD_UNIQUECODE")]
        public string STD_UNIQUECODE { get; set; }

        [Column("STD_FNAME")]
        public string STD_FNAME { get; set; }

        [Column("STD_LNAME")]
        public string STD_LNAME { get; set; }

        [Column("STD_FANAME")]
        public string STD_FANAME { get; set; }

        [Column("STD_CLASS")]
        public string STD_CLASS { get; set; }

        [Column("STD_MOBILENO")]
        public string STD_MOBILENO { get; set; }

        [Column("STD_AFEES")]
        public decimal STD_AFEES { get; set; }

        [Column("STD_STSID")]
        public int STD_STSID { get; set; }

        [Column("STD_MEDIUM")]
        public string STD_MEDIUM { get; set; }

        [Column("STD_ENTRYDATE")]
        public DateTime STD_ENTRYDATE { get; set; }
        [Column("STD_CLID")]
        public Guid STD_CLID { get; set; }
        [Column("STD_MEDID")]
        public Guid STD_MEDID { get; set; }
    }
}
