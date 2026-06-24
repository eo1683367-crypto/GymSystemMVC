using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using GymSystemMVC.DAL.Entities;

namespace GymSystemMVC.DAL.Repositories.Interfaces
{
    public interface IMemberShipRepository : IGenaricRepository<MemberShip>
    { 
        Task<IEnumerable<MemberShip>> GetMemberShipsWithMemebersAndPlansAsync(Expression<Func<MemberShip, bool>>? expression,CancellationToken ct);


    }
}
