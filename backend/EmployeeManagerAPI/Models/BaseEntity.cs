namespace EmployeeManagerAPI.Models
{
    using System;

    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // מזהה ייחודי (GUID)
        public string Email { get; set; } = string.Empty; // כתובת מייל
        public string Password { get; set; } = string.Empty; // סיסמה (תישמר כהאש)
        public string FullName { get; set; } = string.Empty; // שם מלא
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // תאריך יצירת האובייקט
    }
}

