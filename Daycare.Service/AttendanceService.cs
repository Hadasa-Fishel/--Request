using Daycare.Core;
using Daycare.Core.Entities;
using Daycare.Core.Interfaces;

namespace Daycare.Service
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IDataContext _context;
        private static int _nextId = 3;

        public AttendanceService(IDataContext context)
        {
            _context = context;
        }

        public IEnumerable<Attendance> GetAttendances()
        {
            return _context.Attendances;
        }

        public Attendance GetAttendance(int id)
        {
            return _context.Attendances.FirstOrDefault(a => a.Id == id);
        }

        public (bool Success, string ErrorMessage, Attendance Data) AddAttendance(Attendance newAttendance)
        {
           
            var existingToday = _context.Attendances.FirstOrDefault(a =>
                a.ChildId == newAttendance.ChildId &&
                a.Date.Date == newAttendance.Date.Date);

            if (existingToday != null)
            {
                return (false, $"Attendance for child {newAttendance.ChildId} already exists for {newAttendance.Date:dd/MM/yyyy}", null);
            }

            newAttendance.Id = _nextId++;
            newAttendance.IsPresent = true;
            _context.Attendances.Add(newAttendance);

            return (true, null, newAttendance);
        }

        public bool UpdateAttendance(int id, Attendance updatedAttendance)
        {
            var existingAttendance = _context.Attendances.FirstOrDefault(a => a.Id == id);
            if (existingAttendance == null) return false;

            existingAttendance.CheckInTime = updatedAttendance.CheckInTime;
            existingAttendance.CheckOutTime = updatedAttendance.CheckOutTime;
            existingAttendance.Notes = updatedAttendance.Notes;
            existingAttendance.IsPresent = updatedAttendance.IsPresent;
            existingAttendance.DailyReport = updatedAttendance.DailyReport;

            return true;
        }

        public bool DeleteAttendance(int id)
        {
            var attendanceToRemove = _context.Attendances.FirstOrDefault(a => a.Id == id);
            if (attendanceToRemove == null) return false;

            _context.Attendances.Remove(attendanceToRemove);
            return true;
        }

        public object GetDailyAttendance(DateTime date)
        {
            var dailyAttendances = _context.Attendances
                .Where(a => a.Date.Date == date.Date)
                .ToList();

            if (!dailyAttendances.Any()) return null;

            return new
            {
                Date = date.Date,
                TotalRecords = dailyAttendances.Count,
                Present = dailyAttendances.Count(a => a.IsPresent),
                StillAtDaycare = dailyAttendances.Count(a => a.IsCurrentlyAtDaycare),
                Records = dailyAttendances
            };
        }

        public (bool Success, string ErrorMessage, TimeSpan? CheckOutTime) CheckOut(int id)
        {
            var attendance = _context.Attendances.FirstOrDefault(a => a.Id == id);

            if (attendance == null) return (false, "NotFound", null);

            // לוגיקה עסקית: בדיקה אם כבר יצא
            if (attendance.CheckOutTime != null)
            {
                return (false, "Child has already been checked out.", null);
            }

            attendance.CheckOutTime = DateTime.Now.TimeOfDay;
            return (true, null, attendance.CheckOutTime);
        }

        public IEnumerable<Attendance> GetAttendanceByChild(int childId)
        {
            return _context.Attendances
                .Where(a => a.ChildId == childId)
                .OrderByDescending(a => a.Date)
                .ToList();
        }
    }
}