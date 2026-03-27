using LinqConsoleLab.EN.Data;

namespace LinqConsoleLab.EN.Exercises;

public sealed class LinqExercises
{
    /// <summary>
    /// Task:
    /// Find all students who live in Warsaw.
    /// Return the index number, full name, and city.
    ///
    /// SQL:
    /// SELECT IndexNumber, FirstName, LastName, City
    /// FROM Students
    /// WHERE City = 'Warsaw';
    /// </summary>
    public IEnumerable<string> Task01_StudentsFromWarsaw()
    {
        var res = from s in UniversityData.Students
            where s.City.Equals("Warsaw")
            select $"{s.IndexNumber} {s.FirstName} {s.LastName} {s.City}";
        return res;
    }

    /// <summary>
    /// Task:
    /// Build a list of all student email addresses.
    /// Use projection so that you do not return whole objects.
    ///
    /// SQL:
    /// SELECT Email
    /// FROM Students;
    /// </summary>
    public IEnumerable<string> Task02_StudentEmailAddresses()
    {
        var res = from s in  UniversityData.Students
            select s.Email;
        return res;
    }

    /// <summary>
    /// Task:
    /// Sort students alphabetically by last name and then by first name.
    /// Return the index number and full name.
    ///
    /// SQL:
    /// SELECT IndexNumber, FirstName, LastName
    /// FROM Students
    /// ORDER BY LastName, FirstName;
    /// </summary>
    public IEnumerable<string> Task03_StudentsSortedAlphabetically()
    {
        var res =  from s in UniversityData.Students
            orderby s.FirstName, s.LastName
            select $"{s.IndexNumber} {s.FirstName} {s.LastName}";
        return res;
    }

    /// <summary>
    /// Task:
    /// Find the first course from the Analytics category.
    /// If such a course does not exist, return a text message.
    ///
    /// SQL:
    /// SELECT TOP 1 Title, StartDate
    /// FROM Courses
    /// WHERE Category = 'Analytics';
    /// </summary>
    public IEnumerable<string> Task04_FirstAnalyticsCourse()
    {
        var course = UniversityData.Courses
            .Where(c => c.Category == "Analytics")
            .Select(c => $"{c.Title} - {c.StartDate}")
            .FirstOrDefault();

        if (!string.IsNullOrEmpty(course))
        {
            return new List<string> { course };
        }
        else
        {
            return new List<string> { "No Analytics course found" };
        }
    }

    /// <summary>
    /// Task:
    /// Check whether there is at least one inactive enrollment in the data set.
    /// Return one line with a True/False or Yes/No answer.
    ///
    /// SQL:
    /// SELECT CASE WHEN EXISTS (
    ///     SELECT 1
    ///     FROM Enrollments
    ///     WHERE IsActive = 0
    /// ) THEN 1 ELSE 0 END;
    /// </summary>
    public IEnumerable<string> Task05_IsThereAnyInactiveEnrollment()
    {
        var hasInactive = UniversityData.Enrollments
            .Any(e => e.IsActive == false);

        return new List<string> { hasInactive.ToString() };
    }

    /// <summary>
    /// Task:
    /// Check whether every lecturer has a department assigned.
    /// Use a method that validates the condition for the whole collection.
    ///
    /// SQL:
    /// SELECT CASE WHEN COUNT(*) = COUNT(Department)
    /// THEN 1 ELSE 0 END
    /// FROM Lecturers;
    /// </summary>
    public IEnumerable<string> Task06_DoAllLecturersHaveDepartment()
    {
        var allHaveDepartment = UniversityData.Lecturers
            .All(l => String.IsNullOrEmpty(l.Department));

        return new List<string> { allHaveDepartment.ToString() };
    }

    /// <summary>
    /// Task:
    /// Count how many active enrollments exist in the system.
    ///
    /// SQL:
    /// SELECT COUNT(*)
    /// FROM Enrollments
    /// WHERE IsActive = 1;
    /// </summary>
    public IEnumerable<string> Task07_CountActiveEnrollments()
    {
        var numEnrollments = UniversityData.Enrollments
            .Count(e => e.IsActive);

        return new List<string> { numEnrollments.ToString() };
    }

    /// <summary>
    /// Task:
    /// Return a sorted list of distinct student cities.
    ///
    /// SQL:
    /// SELECT DISTINCT City
    /// FROM Students
    /// ORDER BY City;
    /// </summary>
    public IEnumerable<string> Task08_DistinctStudentCities()
    {
        var distinctOrdered = UniversityData.Students.Select(s => s.City)
            .Distinct()
            .OrderBy(s => s);
       return distinctOrdered;
    }

    /// <summary>
    /// Task:
    /// Return the three newest enrollments.
    /// Show the enrollment date, student identifier, and course identifier.
    ///
    /// SQL:
    /// SELECT TOP 3 EnrollmentDate, StudentId, CourseId
    /// FROM Enrollments
    /// ORDER BY EnrollmentDate DESC;
    /// </summary>
    public IEnumerable<string> Task09_ThreeNewestEnrollments()
    {
        var top3Enroll =  (from e in UniversityData.Enrollments
            orderby e.EnrollmentDate descending
            select $"{e.EnrollmentDate} {e.StudentId} {e.CourseId}").Take(3);
        return top3Enroll;
    }

    /// <summary>
    /// Task:
    /// Implement simple pagination for the course list.
    /// Assume a page size of 2 and return the second page of data.
    ///
    /// SQL:
    /// SELECT Title, Category
    /// FROM Courses
    /// ORDER BY Title
    /// OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY;
    /// </summary>
    public IEnumerable<string> Task10_SecondPageOfCourses()
    {
        var courses = UniversityData.Courses
            .OrderBy(c => c.Title)
            .Skip(2)
            .Take(2)
            .Select(c => $"{c.Title} - {c.Category}");
        return courses;
    }

    /// <summary>
    /// Task:
    /// Join students with enrollments by StudentId.
    /// Return the full student name and the enrollment date.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, e.EnrollmentDate
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId;
    /// </summary>
    public IEnumerable<string> Task11_JoinStudentsWithEnrollments()
    {
        var joinedEnrollments = from s in UniversityData.Students
            join e in UniversityData.Enrollments on s.Id equals e.StudentId
            select $"{s.FirstName} {s.LastName} {e.EnrollmentDate}";
        return joinedEnrollments;
    }

    /// <summary>
    /// Task:
    /// Prepare all student-course pairs based on enrollments.
    /// Use an approach that flattens the data into a single result sequence.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, c.Title
    /// FROM Enrollments e
    /// JOIN Students s ON s.Id = e.StudentId
    /// JOIN Courses c ON c.Id = e.CourseId;
    /// </summary>
    public IEnumerable<string> Task12_StudentCoursePairs()
    {
        var joinedEnrollments = from e in UniversityData.Enrollments
            join s in UniversityData.Students on e.StudentId equals s.Id
            join c in UniversityData.Courses on e.CourseId equals c.Id
            select $"{s.FirstName} {s.LastName} {c.Title}";
        return joinedEnrollments;
    }

    /// <summary>
    /// Task:
    /// Group enrollments by course and return the course title together with the number of enrollments.
    ///
    /// SQL:
    /// SELECT c.Title, COUNT(*)
    /// FROM Enrollments e
    /// JOIN Courses c ON c.Id = e.CourseId
    /// GROUP BY c.Title;
    /// </summary>
    public IEnumerable<string> Task13_GroupEnrollmentsByCourse()
    {
        var numEnrollments = from e in UniversityData.Enrollments
            join c in UniversityData.Courses on e.CourseId equals c.Id
            group c by c.Title into grp
                select $"{grp.Key} {grp.Count()}";
        return numEnrollments;
    }

    /// <summary>
    /// Task:
    /// Calculate the average final grade for each course.
    /// Ignore records where the final grade is null.
    ///
    /// SQL:
    /// SELECT c.Title, AVG(e.FinalGrade)
    /// FROM Enrollments e
    /// JOIN Courses c ON c.Id = e.CourseId
    /// WHERE e.FinalGrade IS NOT NULL
    /// GROUP BY c.Title;
    /// </summary>
    public IEnumerable<string> Task14_AverageGradePerCourse()
    {
        var avgGrade = from e in UniversityData.Enrollments
            join c in UniversityData.Courses on e.CourseId equals c.Id
            where e.FinalGrade.HasValue
            group e by c.Title into grp
            select $"{grp.Key} {grp.Average(e => e.FinalGrade)}";

        return avgGrade;
    }

    /// <summary>
    /// Task:
    /// For each lecturer, count how many courses are assigned to that lecturer.
    /// Return the full lecturer name and the course count.
    ///
    /// SQL:
    /// SELECT l.FirstName, l.LastName, COUNT(c.Id)
    /// FROM Lecturers l
    /// LEFT JOIN Courses c ON c.LecturerId = l.Id
    /// GROUP BY l.FirstName, l.LastName;
    /// </summary>
    public IEnumerable<string> Task15_LecturersAndCourseCounts()
    {
        var countLec = from l in UniversityData.Lecturers
            join c in UniversityData.Courses on l.Id equals c.LecturerId
            group l by new { l.FirstName, l.LastName } into grp
            select $"{grp.Key.FirstName} {grp.Key.LastName}: {grp.Count()}";

        return countLec.ToList();
    }

    /// <summary>
    /// Task:
    /// For each student, find the highest final grade.
    /// Skip students who do not have any graded enrollment yet.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, MAX(e.FinalGrade)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.FinalGrade IS NOT NULL
    /// GROUP BY s.FirstName, s.LastName;
    /// </summary>
    public IEnumerable<string> Task16_HighestGradePerStudent()
    {
        var highestGrade = from s in UniversityData.Students
            join e in UniversityData.Enrollments on s.Id equals e.StudentId
            where e.FinalGrade.HasValue
            group e by new { s.FirstName, s.LastName } into grp
            select $"{grp.Key.FirstName} {grp.Key.LastName}: {grp.Max(e => e.FinalGrade)}";

        return highestGrade;
    }

    /// <summary>
    /// Challenge:
    /// Find students who have more than one active enrollment.
    /// Return the full name and the number of active courses.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, COUNT(*)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.IsActive = 1
    /// GROUP BY s.FirstName, s.LastName
    /// HAVING COUNT(*) > 1;
    /// </summary>
    public IEnumerable<string> Challenge01_StudentsWithMoreThanOneActiveCourse()
    {
        var moreThan1Enrollment = from e in UniversityData.Enrollments
            join s in UniversityData.Students on e.StudentId equals s.Id
            where e.IsActive == true
                group e by new { s.FirstName, s.LastName } into grp
            where grp.Count() > 1
                select $"{grp.Key.FirstName} {grp.Key.LastName}: {grp.Count()}";
        return moreThan1Enrollment;
    }

    /// <summary>
    /// Challenge:
    /// List the courses that start in April 2026 and do not have any final grades assigned yet.
    ///
    /// SQL:
    /// SELECT c.Title
    /// FROM Courses c
    /// JOIN Enrollments e ON c.Id = e.CourseId
    /// WHERE MONTH(c.StartDate) = 4 AND YEAR(c.StartDate) = 2026
    /// GROUP BY c.Title
    /// HAVING SUM(CASE WHEN e.FinalGrade IS NOT NULL THEN 1 ELSE 0 END) = 0;
    /// </summary>
    public IEnumerable<string> Challenge02_AprilCoursesWithoutFinalGrades()
    {
        var res = (from c in UniversityData.Courses
            join e in UniversityData.Enrollments on c.Id equals e.CourseId
            where c.StartDate.Month == 4 && c.StartDate.Year == 2026
                && e.FinalGrade == null
            select c.Title).Distinct();
        return res;
    }

    /// <summary>
    /// Challenge:
    /// Calculate the average final grade for every lecturer across all of their courses.
    /// Ignore missing grades but still keep the lecturers in mind as the reporting dimension.
    ///
    /// SQL:
    /// SELECT l.FirstName, l.LastName, AVG(e.FinalGrade)
    /// FROM Lecturers l
    /// LEFT JOIN Courses c ON c.LecturerId = l.Id
    /// LEFT JOIN Enrollments e ON e.CourseId = c.Id
    /// WHERE e.FinalGrade IS NOT NULL
    /// GROUP BY l.FirstName, l.LastName;
    /// </summary>
    public IEnumerable<string> Challenge03_LecturersAndAverageGradeAcrossTheirCourses()
    {
        var res = from l in UniversityData.Lecturers
            join c in UniversityData.Courses on l.Id equals c.LecturerId
            join e in UniversityData.Enrollments on c.Id equals e.CourseId
            where e.FinalGrade != null
            group e.FinalGrade by new { l.FirstName, l.LastName } into g
            select $"{g.Key.FirstName} {g.Key.LastName}: {g.Average():F2}";

        return res;
    }

    /// <summary>
    /// Challenge:
    /// Show student cities and the number of active enrollments created by students from each city.
    /// Sort the result by the active enrollment count in descending order.
    ///
    /// SQL:
    /// SELECT s.City, COUNT(*)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.IsActive = 1
    /// GROUP BY s.City
    /// ORDER BY COUNT(*) DESC;
    /// </summary>
    public IEnumerable<string> Challenge04_CitiesAndActiveEnrollmentCounts()
    {
        var res = from s in UniversityData.Students
            join e in UniversityData.Enrollments on s.Id equals e.StudentId
            where e.IsActive == true
            group s by s.City into cityGroup
            orderby cityGroup.Count() descending
            select $"{cityGroup.Key}: {cityGroup.Count()}";

        return res;
    }

    private static NotImplementedException NotImplemented(string methodName)
    {
        return new NotImplementedException(
            $"Complete method {methodName} in Exercises/LinqExercises.cs and run the command again.");
    }
}
