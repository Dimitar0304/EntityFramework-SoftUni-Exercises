using FishingShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingShop.Core.Contracts
{
    public interface IFishingRodService
    {
        Task<int> CreateAsync(FishingRodModel fishingRod);
        Task UpdateAsync(FishingRodModel fishingRod);
        Task DeleteAsync(int id);
        Task ReadAsync(FishingRodModel fishingRod);
    }
}
