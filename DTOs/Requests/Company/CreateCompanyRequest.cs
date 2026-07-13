namespace BackEnd.DTOs.Requests.Company
{
    public class CreateCompanyRequest
    {
        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? Tax { get; set; }

        public byte Status { get; set; } = 1;
    }
}