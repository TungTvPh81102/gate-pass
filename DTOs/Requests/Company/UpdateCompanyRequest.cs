namespace BackEnd.DTOs.Requests.Company
{
    public class UpdateCompanyRequest
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? Tax { get; set; }

        public byte? Status { get; set; }

        public string? UpdatedBy
        {
            get; set;
        }
    }
}
