namespace EmployeeManagerAPI.Validations
{
    public static class ErrorMessages
    {
        public const string InvalidEmail = "Invalid email format.";
        public const string InvalidPassword = "Password must be at least 8 characters long, include a mix of upper and lower case letters, numbers, and special characters.";
        public const string InvalidFullName = "Full name must be at least 2 characters long.";
        public const string EmailAlreadyExists = "A user with this email already exists.";
    }
}
