namespace BackEnd.DTOs.Responses.Department
{
    public class DepartmentTreeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? ParentId { get; set; }
    }
}