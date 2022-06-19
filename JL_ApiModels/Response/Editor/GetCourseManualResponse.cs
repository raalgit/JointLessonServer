using JL_MSSQLServer.PersistModels;

namespace JL_ApiModels.Response.Editor
{
    public class GetCourseManualResponse : ResponseBase, IResponse
    {
        public Manual Manual { get; set; }
    }
}
