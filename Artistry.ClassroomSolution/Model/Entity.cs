namespace Artistry.ClassroomSolution.Model
{
    public class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int EntityTypeId { get; set; }
    }
}
