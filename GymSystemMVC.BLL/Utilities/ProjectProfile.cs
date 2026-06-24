using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using GymSystemMVC.BLL.ViewModels.MemberShipsViewModels;
using GymSystemMVC.BLL.ViewModels.MembersViewModels;
using GymSystemMVC.BLL.ViewModels.PlanViewModels;
using GymSystemMVC.BLL.ViewModels.SessionViewModels;
using GymSystemMVC.BLL.ViewModels.TrainerViewModels;
using GymSystemMVC.DAL.Entities;

namespace GymSystemMVC.BLL.Utilities
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            MapSession();
            MapPlan();
            MapMember();
            MapTrainer();
            MapMemberShip();
        }

        private void MapTrainer()
        {
            
            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBith.ToShortDateString()))
                .ForMember(dest => dest.Specialties, opt => opt.MapFrom(src => $"{src.Specialize} Trainer"))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
                    $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));

          
            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dest => dest.Specialties, opt => opt.MapFrom(src => src.Specialize))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber));

   
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.DateOfBith, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.Specialize, opt => opt.MapFrom(src => src.Specialties))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    Street = src.Street,
                    City = src.City,
                    BuildingNumber = src.BuildingNumber
                }));
        }
        private void MapMember()
        {
            
            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBith.ToShortDateString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
                    $"{src.Address.BuildingNumber} - {src.Address.City} - {src.Address.Street}"))

                // دول بيتملوا manually من الـ MemberShip، مش من الـ Member
                .ForMember(dest => dest.PlanName, opt => opt.Ignore())
                .ForMember(dest => dest.MembershipStartDate, opt => opt.Ignore())
                .ForMember(dest => dest.MembershipEndDate, opt => opt.Ignore());

            // Member Have Nav Prop Address
            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber));

            
            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.DateOfBith, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    Street = src.Street,
                    City = src.City,
                    BuildingNumber = src.BuildingNumber
                }))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));

            
            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();
        }
        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>()
             .ForMember(dest => dest.DurationDays, opt => opt.MapFrom(src => src.Duration))
             .ReverseMap();

            CreateMap<Plan, UpdatePlanViewModel>()
             .ForMember(dest => dest.DurationDays, opt => opt.MapFrom(src => src.Duration))
             .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name))
             .ReverseMap();

        }
        private void MapSession()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName , opt=> opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.TrainerName , opt=> opt.MapFrom(src => src.Trainer.Name))
                .ReverseMap();

            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>();


            CreateMap<Session,UpdateSessionViewModel>().ReverseMap();

        }

        private void MapMemberShip()
        {
            CreateMap<MemberShip, MemberShipViewModel>()
                 .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                 .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Plan.Name))
                 .ForMember(dest => dest.StartDate , opt => opt.MapFrom(src=>src.CreatedAt))
                 .ReverseMap(); 

            CreateMap<CreateMemberShipViewModel, MemberShip>().ReverseMap();
            CreateMap<Member, MemberSelectListViewModel>().ReverseMap();
            CreateMap<Plan, PlanSelectListViewModel>().ReverseMap();


        }
       
    }
}
