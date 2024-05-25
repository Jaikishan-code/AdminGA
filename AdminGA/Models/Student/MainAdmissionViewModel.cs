namespace AdminGA.Models.Student
{
    public class MainAdmissionViewModel
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

        public decimal? YearlyFees { get; set; }

        public decimal? Discount { get; set; }
        public decimal? MonthlyFees { get; set; }

        // Additional fields for Other Details section
        public string? MotherName { get; set; }
        public string? MotherPhoneNumber { get; set; }
        public string? FatherPhoneNumber { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Batch { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }

    }
}
