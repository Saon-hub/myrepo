namespace Artistry.ClassroomSolution.Model
{
    public class UpdateAttendanceRequest
    {
        public long AssociationId { get; set; }
        public bool Attendance { get; set; }
    }
}
