using JL_MSSQLServer.PersistModels;

namespace JL_ApiModels.Response.User
{
    public class GetCourseDataResponse : ResponseBase, IResponse
    {
        public List<CourseTeacher> CourseTeachers { get; set; }
        public bool IsTeacher { get; set; }
        public bool LessonIsActive { get; set; }
    }
}
