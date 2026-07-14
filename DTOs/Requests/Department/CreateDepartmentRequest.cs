namespace BackEnd.DTOs.Requests.Department
{
    public class CreateDepartmentRequest
    {
        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public byte Status { get; set; } = 1;

        public int? ParentId { get; set; }

        public int CompanyId { get; set; }
    }
}
