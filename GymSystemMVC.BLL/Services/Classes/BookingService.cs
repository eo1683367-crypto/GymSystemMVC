using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using GymSystemMVC.BLL.Common;
using GymSystemMVC.BLL.Services.Interfaces;
using GymSystemMVC.BLL.ViewModels.BookingViewModels;
using GymSystemMVC.BLL.ViewModels.MemberShipsViewModels;
using GymSystemMVC.BLL.ViewModels.SessionViewModels;
using GymSystemMVC.DAL.Entities;
using GymSystemMVC.DAL.Repositories.Interfaces;

namespace GymSystemMVC.BLL.Services.Classes
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public BookingService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            // catch Session
            var sessions = await unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(s => s.EndDate >= DateTime.Now , ct);
            // Mapped
            var mappedSessions = mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);
            // Calculate Available Slot
            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(session.Id, ct);
            }
            // return Sessions
            return mappedSessions;
        }
        public async Task<IEnumerable<MemberForSessionViewModel>> GetMembersForSessionsAsync(int sessionId, CancellationToken ct = default)
        {
           var bookings = await unitOfWork.BookingRepository.GetBookingWithMemberBySessionIdAsync(sessionId, ct);

            var session = await unitOfWork.SessionRepository.GetByIdAsync(sessionId, ct);

            return bookings.Select(booking => new MemberForSessionViewModel
            {
                MemberId = booking.MemberId,
                SessionId = booking.SessionId,
                BookingDate = booking.CreatedAt,
                MemberName = booking.Member.Name,
                IsAttended = session?.StartDate > DateTime.Now ? false : booking.IsAttended,
            }).ToList();
        }
        public async Task<Result> CreateBookingAsync(CreateBookingViewModel model, CancellationToken ct = default)
        {
            var sessoion = await unitOfWork.SessionRepository.GetByIdAsync(model.SessionId, ct);

            if (sessoion is null)
                return Result.NotFound("Session Is Not Found");

            if (sessoion.StartDate <= DateTime.Now)
                return Result.Fail("You Can Not Booked A Session That Aready Started");

            var memberShip = await unitOfWork.MemberShipRepository
                                             .AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateTime.Now,ct);

            if (!memberShip)
                return Result.Fail("You Don't Have Active MemberShip");


            var areadyBooked = await unitOfWork.BookingRepository
                                               .AnyAsync(b => b.MemberId == model.MemberId && b.SessionId == model.SessionId, ct);

            if (areadyBooked)
                return Result.Fail("You Aready Booked This Session");


            var bookedSlots = await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(model.SessionId,ct);

            if (bookedSlots >= sessoion.Capacity)
                return Result.Fail("Session Is Full Capacity");

            var booking = new Booking
            {
                MemberId = model.MemberId,
                SessionId = model.SessionId,
                IsAttended = false,
                CreatedAt = DateTime.Now,
            };


            unitOfWork.BookingRepository.Add(booking);

            var rowEffected = await unitOfWork.CompleteAsync();

            return rowEffected > 0 ? Result.Ok() : Result.Fail("Fail To Book This Session");
        }
        public async Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(int sessionId, CancellationToken ct = default)
        {
            var bookings = await unitOfWork.BookingRepository.GetAllAsync(b => b.SessionId == sessionId, false, ct);

            var bookedMemberIds = bookings.Select(b => b.MemberId);

            var availableMembers = await unitOfWork.GetRepository<Member>().GetAllAsync(m => !bookedMemberIds.Contains(m.Id));

            return mapper.Map<IEnumerable<Member>, IEnumerable<MemberSelectListViewModel>>(availableMembers);
        }
        public async Task<Result> MarkAttendedAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var booking = await unitOfWork.BookingRepository.FirstOrDefaultAsync(
                                                            b => b.MemberId == memberId
                                                            && b.SessionId == sessionId ,true,ct);

            if (booking is null)
                return Result.NotFound("Booking Is Not Found");

            booking.IsAttended = true;
            booking.UpdatedAt = DateTime.Now;

            unitOfWork.BookingRepository.Update(booking);

            var rowEffected = await unitOfWork.CompleteAsync();

            return rowEffected > 0 ? Result.Ok() : Result.Fail("Failed To Mark This Member As Attended");
        }
        public async Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var session = await unitOfWork.SessionRepository.GetByIdAsync(sessionId, ct);

            if (session is null)
                return Result.NotFound("Session Is Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Can't Cancel Booking For Session That Aready Started");


            var booking = await unitOfWork.BookingRepository.FirstOrDefaultAsync(
                                                          b => b.MemberId == memberId
                                                          && b.SessionId == sessionId, true, ct);

            if (booking is null)
                return Result.NotFound("Booking Is Not Found");



            unitOfWork.BookingRepository.Delete(booking);

            var rowEffected = await unitOfWork.CompleteAsync();

            return rowEffected > 0 ? Result.Ok() : Result.Fail("Can Not Cancelled MemberShip");
        }
    }
}
