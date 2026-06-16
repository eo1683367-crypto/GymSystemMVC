using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using GymSystemMVC.BLL.Common;
using GymSystemMVC.BLL.Services.Interfaces;
using GymSystemMVC.BLL.ViewModels.TrainerViewModels;
using GymSystemMVC.DAL.Entities;
using GymSystemMVC.DAL.Entities.Enums;
using GymSystemMVC.DAL.Repositories.Classes;
using GymSystemMVC.DAL.Repositories.Interfaces;

namespace GymSystemMVC.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TrainerService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

    

        // GET
        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await unitOfWork.GetRepository<Trainer>().GetAll(false,ct);

            if (trainers is null) return [];

            // Auto Mapped
            return mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerViewModel>>(trainers);
        }

        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default)
        {
           var trainer = await unitOfWork.GetRepository<Trainer>().GetById(trainerId, ct);
           
            if (trainer is null) return null;


            // Auto Mapped
            return mapper.Map<Trainer, TrainerViewModel>(trainer);

        }
        public async Task<TrainerToUpdateViewModel> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetById(trainerId, ct);

            if(trainer is null) return null;

            // Auto Mapped
            return mapper.Map<Trainer, TrainerToUpdateViewModel>(trainer);
        }

        // POST

        public async Task<Result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var emailExisting = await unitOfWork.GetRepository<Trainer>().AnyAsync(m => m.Email == model.Email, ct);
            if (emailExisting)
                return Result.Fail("Email Already Exists", ResultKind.Conflict);

            var phoneExisting = await unitOfWork.GetRepository<Trainer>().AnyAsync(m => m.Phone == model.Phone, ct);
            if (phoneExisting)
                return Result.Fail("Phone Already Exists", ResultKind.Conflict);

            // Auto Mapped
            var trainer = mapper.Map<CreateTrainerViewModel, Trainer>(model);


            unitOfWork.GetRepository<Trainer>().Add(trainer);

            var result = await unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Failed To Create Trainer");
        }

        public async Task<Result> UpdateTrainerDetailsAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetById(trainerId, ct);

            if (trainer is null)
                return Result.NotFound("Trainer Not Found");

            if (await unitOfWork.GetRepository<Trainer>().AnyAsync(m => m.Email == model.Email && m.Id != trainerId, ct))
                return Result.Fail("Email Already Used By Another Trainer", ResultKind.Conflict);

            if (await unitOfWork.GetRepository<Trainer>().AnyAsync(m => m.Phone == model.Phone && m.Id != trainerId, ct))
                return Result.Fail("Phone Already Used By Another Trainer", ResultKind.Conflict);


            trainer.Email = model.Email;
            trainer.Phone = model.Phone;           
            trainer.Specialize = model.Specialties;
            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.Address.Street = model.Street;
            trainer.Address.City = model.City;

            unitOfWork.GetRepository<Trainer>().Update(trainer);
            var result = await unitOfWork.CompleteAsync();
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Update Trainer");
        }

        public async Task<Result> DeleteTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetById(trainerId, ct);

            if (trainer is null)
                Result.NotFound("Trainer Not Found");

            var activeSessions = await unitOfWork.GetRepository<Session>().AnyAsync(s => s.TrainerId == trainerId && s.EndDate > DateTime.Now , ct);
            
            if (activeSessions)
                return Result.Fail("Cannot Delete Trainer With Active Sessions");

            unitOfWork.GetRepository<Trainer>().Delete(trainerId);
            var result = await unitOfWork.CompleteAsync();
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Delete Trainer");
        }
    }
}
