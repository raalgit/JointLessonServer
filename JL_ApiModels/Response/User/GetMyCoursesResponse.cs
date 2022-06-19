using JL_MSSQLServer.PersistModels;

namespace JL_ApiModels.Response.User
{
    public class GetMyCoursesResponse : ResponseBase, IResponse
    {
        public List<Course> Courses { get; set; }
    }
}
