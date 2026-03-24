using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlant.Service
{
    public interface IFacilityRepository
    {
        public Task<string> GetFacilityId();
    }
}
