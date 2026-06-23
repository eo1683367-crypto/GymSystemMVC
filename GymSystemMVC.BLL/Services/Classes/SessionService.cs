using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using GymSystemMVC.BLL.Common;
using GymSystemMVC.BLL.Services.Interfaces;
using GymSystemMVC.BLL.ViewModels.SessionViewModels;
using GymSystemMVC.DAL.Entities;
using GymSystemMVC.DAL.Repositories.Interfaces;

namespace GymSystemMVC.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SessionService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

  
        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct);

            if (!sessions.Any()) return null;

            sessions = sessions.OrderByDescending(s => s.StartDate);

            // Auto Mapped
            var mappedSessions = mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(session.Id, ct);
            }

            return mappedSessions;
        }


        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return Result.Validation("End_Date Must Be After Start_Date");

            if (model.StartDate <= DateTime.Now) return Result.Validation("Start_Date Must Be In The Future");

            //-------------------------------------------------------------
            var trainerRepo = unitOfWork.GetRepository<Trainer>();
            var trainer = await trainerRepo.GetById(model.TrainerId, ct);      
            if (trainer is null) return Result.NotFound("Trainer Not Found");

            //-------------------------------------------------------------

            var categoryRepo = unitOfWork.GetRepository<Category>();
            var category = await categoryRepo.GetById(model.CategoryId, ct);
            if (category is null) return Result.NotFound("Category Not Found");
            //-------------------------------------------------------------


            // Map
            var session = mapper.Map<CreateSessionViewModel,Session>(model);

            var sessionRepo = unitOfWork.GetRepository<Session>(); // Genaric Repo 


            sessionRepo.Add(session);

            var rowEffected = await unitOfWork.CompleteAsync();

            return rowEffected > 0 ? Result.Ok() : Result.Fail("Failed To Create Session !");


        }


        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default)
        {
            var trainers = await unitOfWork.GetRepository<Trainer>().GetAll(false, ct);

            return mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default)
        {
            var categories = await unitOfWork.GetRepository<Category>().GetAll(false, ct);

            return mapper.Map<IEnumerable<Category>, IEnumerable<CategorySelectViewModel>>(categories);
        }

        public async Task<SessionViewModel?> GetSessionDetailsAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await unitOfWork.SessionRepository.GetSessionByIdWithTrainerAndCategoryAsync(sessionId);

            if (session is null) return null;

            var mappSession = mapper.Map<Session,SessionViewModel>(session);

            mappSession.AvailableSlots = mappSession.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(mappSession.Id,ct);

            return mappSession;
        }

        public async Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await unitOfWork.GetRepository<Session>().GetById(sessionId, ct);

            if (session is null)
                return Result<UpdateSessionViewModel>.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result<UpdateSessionViewModel>.Fail("Session Is Already Started And Cannot Be Edited");


            var booked = await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(session.Id, ct);
            if (booked > 0)
                return Result<UpdateSessionViewModel>.Fail("Session Has Booked Slots And Cannot Be Edited");

            return Result<UpdateSessionViewModel>.Ok(mapper.Map<Session, UpdateSessionViewModel>(session));
        }

        //private async Task<bool> IsSessionValidForUpdateAsync(Session session, CancellationToken ct)
        //{
        //    if (session.StartDate <= DateTime.Now) return false;

        //    var booked = await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(session.Id, ct);

        //    return booked == 0;
        //}

        public async Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var sessionRepo =  unitOfWork.GetRepository<Session>();

            var session = await sessionRepo.GetById(id, ct);

            if (session is null)
                return Result.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Can Not Edit Session That Has Aready Started");

            var bookCount = await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(session.Id, ct);

            if (bookCount > 0)
                return Result.Fail("Can Not Edit Session That Has booked slots");

            if (model.EndDate <= model.StartDate)
                return Result.Validation("End_Date Must Be After Start_Date");

            if (model.StartDate <= DateTime.Now)
                return Result.Validation("Start_Date Must Be In The Future");


            //-------------------------------------------------------------
            var trainerRepo = unitOfWork.GetRepository<Trainer>();
            var trainer = await trainerRepo.GetById(model.TrainerId, ct);
            if (trainer is null) return Result.NotFound("Trainer Not Found");
            //-------------------------------------------------------------


            session.UpdatedAt = DateTime.Now;

            // Map
            mapper.Map(model, session);

            sessionRepo.Update(session);

            var effectedRows = await unitOfWork.CompleteAsync();

            return effectedRows > 0 ? Result.Ok() : Result.Fail("Failed To Create Session");
        }

        public async Task<Result> DeleteSesssionAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await unitOfWork.GetRepository<Session>().GetById(sessionId,ct);

            if (session is null) return
                    Result.NotFound("Session Not Found");

            if (session.EndDate >= DateTime.Now)
                return Result.Fail("Can not Delete A Session That Not Yet Ended");

            var bookedCount = await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(sessionId, ct);

            if(bookedCount  > 0)
                return Result.Fail("Can Not Delete A Session That Has Booking");



            unitOfWork.GetRepository<Session>().Delete(sessionId);

            var effectedRows = await unitOfWork.CompleteAsync();

            return effectedRows > 0 ? Result.Ok() : Result.Fail("Failed To Delete This Session!");
        }

        public async Task<SessionViewModel> GetSessionById(int sessionId, CancellationToken ct)
        {
            var session = await unitOfWork.GetRepository<Session>().GetById(sessionId);

            return mapper.Map<Session,SessionViewModel>(session);
        }
    }
}
