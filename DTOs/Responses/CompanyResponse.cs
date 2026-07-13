namespace BackEnd.DTOs.Responses
{
    public class CompanyResponse
    {
        public int Id { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public byte Status { get; set; }

        public string? Description { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? Tax { get; set; }

        public string? CreatedBy { get; set; }

        public string? CreatedAt { get; set; }
    }
}
