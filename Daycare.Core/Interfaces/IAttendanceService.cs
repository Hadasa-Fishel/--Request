using Daycare.Core.Entities;

namespace Daycare.Core.Interfaces
{
    public interface IAttendanceService
    {
        IEnumerable<Attendance> GetAttendances();
        Attendance GetAttendance(int id);

        // מחזיר האם הצליח, הודעת שגיאה (אם יש), ואת האובייקט שנוצר
        (bool Success, string ErrorMessage, Attendance Data) AddAttendance(Attendance newAttendance);

        bool UpdateAttendance(int id, Attendance updatedAttendance);
        bool DeleteAttendance(int id);

        object GetDailyAttendance(DateTime date);

        // מחזיר האם הצליח, שגיאה, ואת זמן היציאה
        (bool Success, string ErrorMessage, TimeSpan? CheckOutTime) CheckOut(int id);

        IEnumerable<Attendance> GetAttendanceByChild(int childId);
    }
}