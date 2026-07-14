namespace BackEnd.DTOs.Responses.Department
{
    public class DepartmentResponse
    {
        public int Id { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public int CompanyId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public byte Status { get; set; }

        public string? Description { get; set; }

        public DepartmentTreeResponse? Parent { get; set; }
    }
}