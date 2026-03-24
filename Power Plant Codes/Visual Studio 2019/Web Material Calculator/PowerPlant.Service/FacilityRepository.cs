using Microsoft.EntityFrameworkCore;
using PowerPlant.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlant.Service
{
    public class FacilityRepository : IFacilityRepository
    {
        private readonly PPDbContext _dbContext;

        public FacilityRepository(PPDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GetFacilityId()
        {
            var query = await _dbContext.Controls.Where(x => x.Key == "Facility").FirstOrDefaultAsync();
            return query.Value1 ?? string.Empty;
        }
    }
}