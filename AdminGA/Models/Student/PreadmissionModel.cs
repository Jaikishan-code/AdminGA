namespace AdminGA.Models.Student
{
    public class PreadmissionModel
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public Guid ClassID { get; set; }
        public Guid MediumID { get; set; }
        public string? MobileNumber { get; set; }
        public string? PreAdmissionCode { get; set; }

        public int Fees { get; set; }

        public string? Class { get; set; }

        public string? Medium { get; set; }
    }
}
