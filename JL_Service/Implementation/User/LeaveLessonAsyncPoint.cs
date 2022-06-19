using JL_ApiModels.Request.User;
using JL_ApiModels.Response.User;
using JL_MSSQLServer;
using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.User;
using JL_Service.Exceptions;
using JL_SignalR;
using JL_Utility.Logger;
using JL_Utility.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace JL_Service.Implementation.User
{
    public class LeaveLessonAsyncPoint : PointBase<LeaveLessonRequest, LeaveLessonResponse>, ILeaveLessonAsyncPoint
    {
        private readonly IHubContext<SignalHub> _hubContext;
        private readonly IJLLogger _logger;
        private readonly ISignalRUtility _signalRUtility;
        private readonly ILessonTabelRepository _lessonTabelRepository;
        private readonly ICourseTeacherRepository _courseTeacherRepository;

        public LeaveLessonAsyncPoint(
            IHubContext<SignalHub> _hubContext,
            IJLLogger _logger,
            ISignalRUtility _signalRUtility,
            ILessonTabelRepository _lessonTabelRepository,
            ICourseTeacherRepository _courseTeacherRepository,
            ApplicationContext _context) : base(_context)
        {
            this._logger = _logger;
            this._signalRUtility = _signalRUtility;
            this._hubContext = _hubContext;
            this._lessonTabelRepository = _lessonTabelRepository;
            this._courseTeacherRepository = _courseTeacherRepository;
        }

        public override async Task<LeaveLessonResponse> Execute(LeaveLessonRequest req, UserSettings userSettings)
        {
            var response = new LeaveLessonResponse();

            var courseTeachers = await _courseTeacherRepository.Get().Where(x => x.CourseId == req.CourseId).ToListAsync()
                ?? throw new PointException($"Преподаватели курса <{req.CourseId}> не найдены", _logger);

            // Если пользователь - преподаватель
            if (courseTeachers.Count > 0 && courseTeachers.Select(x => x.UserId).Contains(userSettings.User.Id))
            {
                await LeaveLessonAsTeacher(courseTeachers, userSettings.User.Id, req.CourseId);
            }
            else // Если пользователь студент (или иная роль)
            {
                await СloseOldTabels(userSettings.User.Id);
            }

            await _signalRUtility.SendLessonStateSignalR(req.CourseId);
            response.ShowMessage = true;
            response.Message = "Вы покинули занятие";
            return response;
        }

        /// <summary>
        /// Закрытие всех предыдущих сессий на всех занятиях
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<int> СloseOldTabels(int userId)
        {
            var oldActiveTabels = await _lessonTabelRepository.Get()
                    .Where(x =>
                    x.UserId == userId &&
                    !x.LeaveDate.HasValue)
                    .ToListAsync();

            if (oldActiveTabels != null && oldActiveTabels.Count > 0)
            {
                foreach (var tabel in oldActiveTabels)
                {
                    tabel.LeaveDate = DateTime.Now;
                    _lessonTabelRepository.Update(tabel);
                }
            }

            return 0;
        }

        /// <summary>
        /// Установка флага отсутствия на занятии для преподавателя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        /// <exception cref="PointException"></exception>
        private async Task<int> LeaveLessonAsTeacher(List<CourseTeacher> courseTeachers, int userId, int courseId)
        {
            var courseTeacher = courseTeachers.FirstOrDefault(x => x.UserId == userId && x.CourseId == courseId)
                ?? throw new PointException("Запись о преподавателе курса не найдена", _logger);

            courseTeacher.OnLesson = false;
            await _courseTeacherRepository.UpdateAsync(courseTeacher);
            return 0;
        }
    }
}
