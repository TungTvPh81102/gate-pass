using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models.Entities
{
    public class Company
    {
        public int Id { get; set; }

        public required string Code { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? Tax { get; set; }

        public byte Status { get; set; } = 1;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
    }
}