using System;
using System.Collections.Generic;
using System.Text;
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

        public TrainerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

    

        // GET
        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await unitOfWork.GetRepository<Trainer>().GetAll(false,ct);

            if (trainers is null) return [];

            var trainerViewModels = trainers.Select(t => new TrainerViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialties = t.Specialize.ToString(),
            });
            return trainerViewModels;
        }

        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default)
        {
           var trainer = await unitOfWork.GetRepository<Trainer>().GetById(trainerId, ct);
           
            if (trainer is null) return null;

            var trainerViewModel = new TrainerViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBith.ToShortDateString(),
                Specialties = $"{trainer.Specialize} Trainer",
                Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}"
            };



            return trainerViewModel;
        }
        public async Task<TrainerToUpdateViewModel> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetById(trainerId, ct);

            if(trainer is null) return null;

            return new TrainerToUpdateViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Gender = trainer.Gender,
                DateOfBith = trainer.DateOfBith,
                Specialties = trainer.Specialize,
                City = trainer.Address.City,
                Street = trainer.Address.Street,
                BuildingNumber = trainer.Address.BuildingNumber
            };
        }

        // POST

        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var emailExisting = await unitOfWork.GetRepository<Trainer>().AnyAsync(m => m.Email == model.Email, ct);
            var phoneExisting = await unitOfWork.GetRepository<Trainer>().AnyAsync(m => m.Phone == model.Phone, ct);

            if (emailExisting || phoneExisting) return false;


            var trainer = new Trainer
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBith = model.DateOfBirth,
                Gender = model.Gender,

                Address = new Address
                {
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                    City = model.City
                },

                Specialize = model.Specialties

            };

            unitOfWork.GetRepository<Trainer>().Add(trainer);

            var result = await unitOfWork.CompleteAsync();

            return result > 0;
        }

        public async Task<bool> UpdateTrainerDetailsAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetById(trainerId, ct);

            if (trainer is null) return false;


            if (await unitOfWork.GetRepository<Trainer>().AnyAsync(m => m.Email == model.Email && m.Id != trainerId, ct))
                return false;
            if (await unitOfWork.GetRepository<Trainer>().AnyAsync(m => m.Phone == model.Phone && m.Id != trainerId, ct))
                return false;

         
            trainer.Email = model.Email;
            trainer.Phone = model.Phone;           
            trainer.Specialize = model.Specialties;

            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.Address.Street = model.Street;
            trainer.Address.City = model.City;

            unitOfWork.GetRepository<Trainer>().Update(trainer);
            var result = await unitOfWork.CompleteAsync();
            return result > 0;
        }

        public async Task<bool> DeleteTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetById(trainerId, ct);

            if (trainer is null) return false;

            var activeSessions = await unitOfWork.GetRepository<Session>().AnyAsync(s => s.TrainerId == trainerId && s.EndDate > DateTime.Now , ct);
            
            if (activeSessions) return false;

            unitOfWork.GetRepository<Trainer>().Delete(trainerId);
            var result = await unitOfWork.CompleteAsync();
            return result > 0;
        }
    }
}
