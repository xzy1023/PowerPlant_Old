using Microsoft.EntityFrameworkCore;
using PowerPlant.Dtos;
using PowerPlant.Models;
using PowerPlant.Models.Context;
using PowerPlant.Tools.Extensions;
using PowerPlant.Tools.PropertyMapping;
using System;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace PowerPlant.Service
{
    public class ItemRepository : IItemRepository
    {
        private readonly PPDbContext _dbContext;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IFacilityRepository _facilityRepository;

        public ItemRepository(PPDbContext dbContext, IPropertyMappingService propertyMappingService, IFacilityRepository facilityRepository)
        {
            _dbContext = dbContext;
            _propertyMappingService = propertyMappingService;
            _facilityRepository = facilityRepository;
        }

        public async Task<IPagedList<WebMaterial>> GetWebMaterialsAsync(string? searchString, int pageSize, int? pageNumber, string? orderBy)
        {
            // Query initialization
            var facilityId = await _facilityRepository.GetFacilityId();
            IQueryable<WebMaterial> query = _dbContext.WebMaterials.Include(x => x.ItemMaster).
                Where(x => x.ItemMaster.Facility == facilityId);

            // Filter ItemNumber or ItemDesc1 or ItemDesc2 with keyword 
            if (!string.IsNullOrWhiteSpace(searchString)) query = query.Where(
                x => x.ItemNumber.Contains(searchString.Trim())
                || x.ItemMaster.ItemDesc1.Contains(searchString.Trim())
                || x.ItemMaster.ItemDesc2.Contains(searchString.Trim()));

            // Sorting
            var touristRoutesMappingDictionary = _propertyMappingService.GetPropertyMapping<WebMaterialDto, WebMaterial>();
            query = query.ApplySort(orderBy, touristRoutesMappingDictionary);

            // return one page of data
            return await query.ToPagedListAsync(pageNumber ?? 1, pageSize);
        }

        public async Task<WebMaterial?> GetWebMaterialByIdAsync(int itemNumber)
        {
            return await _dbContext.WebMaterials.Include(x => x.ItemMaster).FirstOrDefaultAsync(x => x.Rrn == itemNumber);
        }

        public async Task<bool> IsWebMaterialExistedAsync(string ItemNumber)
        {
            return await _dbContext.WebMaterials.AnyAsync(x => x.ItemNumber == ItemNumber);
        }

        public void AddWebMaterial(WebMaterial webMaterial)
        {
            if (webMaterial is null) throw new ArgumentNullException(nameof(webMaterial));
            _dbContext.WebMaterials.AddAsync(webMaterial);
        }

        public void UpdateWebMaterial(WebMaterial webMaterial)
        {
            if (webMaterial is null) throw new ArgumentNullException(nameof(webMaterial));
            _dbContext.WebMaterials.Update(webMaterial);
        }

        public void RemoveWebMaterial(WebMaterial webMaterial)
        {
            _dbContext.WebMaterials.Remove(webMaterial);
        }

        public async Task<ItemMaster?> GetItemByItemNumberAsync(string itemNumber)
        {
            var facilityId = await _facilityRepository.GetFacilityId();
            return await _dbContext.ItemMasters.Where(x => x.Facility == facilityId).FirstOrDefaultAsync(x => x.ItemNumber == itemNumber);
        }

        public async Task<bool> IsItemMasterExistedAsync(string itemNumber)
        {
            return await _dbContext.ItemMasters.AnyAsync(x => x.ItemNumber == itemNumber);
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }
    }
}