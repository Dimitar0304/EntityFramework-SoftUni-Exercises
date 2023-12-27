using FishingShop.Core.Contracts;
using FishingShop.Core.Models;
using FishingShop.Infrastructure.Data.Common;
using FishingShop.Infrastructure.Models;

namespace FishingShop.Core.Services
{
    public class FishingRodService : IFishingRodService
    {
        private IRepository repository;
        public FishingRodService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task<int> CreateAsync(FishingRodModel fishingRod)
        {
            FishingRod fishingRod1 = new FishingRod()
            {
                Model = fishingRod.Model,
                Price = fishingRod.Price,
                Lenght = fishingRod.Lenght,
            };
            await repository.AddAsync(fishingRod1);
            await repository.SaveChangesAsync();
            return fishingRod1.Id;
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task ReadAsync(FishingRodModel fishingRod)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(FishingRodModel fishingRod)
        {
            throw new NotImplementedException();
        }
    }
}
