using GymSystemMVC.DAL.Contexts;
using GymSystemMVC.DAL.Entities;
using GymSystemMVC.DAL.Repositories.Interfaces;

namespace GymSystemMVC.DAL.Repositories.Classes
{
    public class TrainerRepository : GenaricRepository<Trainer>, ITrainerRepository
    {

        private GymDbContext gymDbContext;
        public TrainerRepository(GymDbContext _gymDbContext) : base(_gymDbContext)
        {
            gymDbContext = _gymDbContext;
        }
        // Implement any additional methods specific to TrainerRepository if needed
    }
}
