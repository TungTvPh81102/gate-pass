namespace BackEnd.DTOs.Requests.Department
{
    public class CreateDepartmentRequest
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public byte Status { get; set; } = 1;

        public int? ParentId { get; set; }

        public int CompanyId { get; set; }
    }
}