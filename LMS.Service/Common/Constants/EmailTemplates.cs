using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Common.Constants
{
    public static class EmailTemplates
    {
        public const string TestEmail = "yazanrabae78@gmail.com";
        public const string TestName = "Yazan";
        public const string _loginUrl = "http://localhost:5094";
        public static string CreateUserSubject(string role)
        {
            return $@"Create {role}";
        }
        public static string CreateUserBody(string recipientName, string recipientEmail, string password)
        {
            return $@"
Dear {recipientName},

Welcome to the LMS platform! Your account has been successfully created.

Here are your login details:
Email: {recipientEmail}  
Password: {password}  
Login URL: {_loginUrl}

Best regards,  
LMS Team
            ";
        }

        public static string UpdateUserSubject(string role)
        {
            return $@"Update {role}";
        }
        public static string UpdateUserBody(string recipientName, string recipientEmail, string password)
        {
            return $@"
Dear {recipientName},

Your account has been successfully updated.

Here are your new login details:
Email: {recipientEmail}  
Password: {password}  
Login URL: {_loginUrl}

Best regards,  
LMS Team
            ";
        }

        public static string StudentEnrollSubject(string courseName)
        {
            return $"Enrollment Request: {courseName}";
        }

        public static string StudentEnrollBody(string instructorName, string studentName, string courseName)
        {
            return $@"
Dear {instructorName},

You have received a new enrollment request for your course **{courseName}**.

Student Name: {studentName}

Please review the pending enrollments at your earliest convenience.

Best regards,  
LMS Team
        ";
        }

        public static string EnrollmentStatusSubject(bool isAccepted, string courseName)
        {
            return (isAccepted ? "Enrollment Accepted" : "Enrollment Rejected") + ": " + courseName;
        }

        public static string EnrollmentStatusBody(bool isAccepted, string recipientName, string courseTitle)
        {
            if (isAccepted)
            {
                return $@"
Dear {recipientName},

Congratulations! You have been accepted into the course: {courseTitle}.

You can now access the course by logging into your LMS account.

Best regards,  
LMS Team
";
            }
            else
            {
                return $@"
Dear {recipientName},

We regret to inform you that your enrollment request for the course: {courseTitle} has been rejected.

If you have any questions, please contact support.

Best regards,  
LMS Team
";
            }
        }

        public static string RemovedFromCourseSubject(string courseTitle)
        {
            return "Removed from Course: " + courseTitle;
        }

        public static string RemovedFromCourseBody(string recipientName, string courseTitle, string instructorName)
        {
            return $@"
Dear {recipientName},

We regret to inform you that you have been removed from the course: {courseTitle} by the instructor {instructorName}.

If you believe this was a mistake or want more information, please reach out to your instructor or LMS support.

Best regards,  
LMS Team
";
        }
    }
}
