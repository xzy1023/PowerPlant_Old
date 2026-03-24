using PowerPlant.Models;
using System.Threading.Tasks;
using X.PagedList;

namespace PowerPlant.Service
{
    public interface IItemRepository
    {
        public Task<IPagedList<WebMaterial>> GetWebMaterialsAsync(string? searchString, int pageSize, int? pageNumber, string? orderBy);

        public Task<WebMaterial?> GetWebMaterialByIdAsync(int webMaterialId);

        public Task<bool> IsWebMaterialExistedAsync(string itemNumber);

        public void AddWebMaterial(WebMaterial webMaterial);

        public void UpdateWebMaterial(WebMaterial webMaterial);

        public void RemoveWebMaterial(WebMaterial webMaterial);

        public Task<ItemMaster?> GetItemByItemNumberAsync(string itemNumber);

        public Task<bool> IsItemMasterExistedAsync(string itemNumber);

        public Task<bool> SaveAsync();
    }
}
