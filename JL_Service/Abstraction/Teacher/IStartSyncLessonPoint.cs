using JL_ApiModels.Request.Teacher;
using JL_ApiModels.Response.Teacher;

namespace JL_Service.Abstraction.Teacher
{
    public interface IStartSyncLessonPoint : IPoint<StartSyncLessonRequest, StartSyncLessonResponse>
    {
    }
}
