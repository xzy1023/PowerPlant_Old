using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PowerPlant.Dtos;
using PowerPlant.Models;
using PowerPlant.Service;
using PowerPlant.Tools.PropertyMapping;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Web_Material_Calculator.Models;
using Web_Material_Calculator.Models.Enum;
using X.PagedList;
using Web_Material_Calculator.Models.ViewModel;

namespace Web_Material_Calculator.Controllers
{
    public class WebMaterialController : Controller
    {
        private readonly ILogger<WebMaterialController> _logger;
        private readonly IMapper _mapper;
        private readonly IItemRepository _itemRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public static WebMaterialIndexModel WebMaterialIndexModel { get; set; } = new();

        public WebMaterialController(
            ILogger<WebMaterialController> logger,
            IMapper mapper,
            IItemRepository itemRepository,
            IPropertyMappingService propertyMappingService)
        {
            _logger = logger;
            _mapper = mapper;
            _itemRepository = itemRepository;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<IActionResult> IndexAsync(string? searchString, string? orderBy, int? pageNumber, int? selectedRrn)
        {
            // if (webMaterialIndexModel != null) WebMaterialIndexModel = webMaterialIndexModel;

            // Validate orderBy parameter
            if (!_propertyMappingService.IsMappingValidated<WebMaterialDto, WebMaterial>(orderBy))
                return BadRequest($"OrderBy parameter: {orderBy} is not validated in the model property mapping");

            // Set default order by as ItemNumber
            if (string.IsNullOrWhiteSpace(orderBy)) orderBy = "ItemNumber";
            var pageSize = 15;  // TODO make pagesize to configurable 
            var webMaterialsFromRepo = await _itemRepository.GetWebMaterialsAsync(searchString, pageSize, pageNumber, orderBy);

            // WebMaterialIndexModel
            WebMaterialIndexModel.WebMaterialDtos = webMaterialsFromRepo.Select(m => _mapper.Map<WebMaterialDto>(m));
            if (selectedRrn != null)
                WebMaterialIndexModel.EditedWebMaterialDto = _mapper.Map<WebMaterialDto>(await _itemRepository.GetWebMaterialByIdAsync((int)selectedRrn));
            else
            {
                WebMaterialIndexModel.EditedWebMaterialDto = null;
                WebMaterialIndexModel.AlertMessage = null;
            }

            // ViewBags
            ViewBag.Role = WebAuthorize().UserRole.ToString();
            ViewBag.SearchString = searchString;
            ViewBag.OrderBy = orderBy;
            ViewBag.PageNumber = pageNumber;
            ViewBag.SelectedRrn = selectedRrn;

            return View(WebMaterialIndexModel);
        }

        public async Task<IActionResult> DetailsAsync(int rrn, double? calculatedRollDiameter)
        {
            var webMaterialFromRepo = await _itemRepository.GetWebMaterialByIdAsync(rrn);
            var webMaterialDto = _mapper.Map<WebMaterialDto>(webMaterialFromRepo);
            if (webMaterialDto is null) return BadRequest($"parameter: {nameof(rrn)}, WebMaterial ID: {rrn} is not validated");

            var webMaterialLookupDtos = webMaterialDto.InitalwebMaterialLookupDtos(calculatedRollDiameter);

            ViewBag.WebMaterialDto = webMaterialDto;
            ViewBag.WebMaterialLookupDtos = webMaterialLookupDtos;
            ViewBag.CalculatedRollDiameter = calculatedRollDiameter;
            ViewBag.Rrn = rrn;

            return View();
        }

        [HttpPost]
        //public async Task<IActionResult> CreateAsync([Bind("ItemNumber, RollDiameter, CoreDiameter, Length, Imps")] WebMaterialDto webMaterialDto)
        public async Task<IActionResult> CreateAsync(WebMaterialIndexModel webMaterialIndexModel)
        {
            if (!await IsSavedItemValidated(webMaterialIndexModel.EditedWebMaterialDto.ItemNumber))
            {
                // WebMaterialIndexModel.EditedWebMaterialDto = webMaterialIndexModel.EditedWebMaterialDto;
                return RedirectToAction(nameof(Index), new { searchString = webMaterialIndexModel.EditedWebMaterialDto.ItemNumber, selectedRrn = 0 });
            }

            webMaterialIndexModel.EditedWebMaterialDto.CreatedBy = User.Identity.Name;
            var webMaterial = _mapper.Map<WebMaterial>(webMaterialIndexModel.EditedWebMaterialDto);
            _itemRepository.AddWebMaterial(webMaterial);

            if (await _itemRepository.SaveAsync())
                WebMaterialIndexModel.AlertMessage = $"ItemNumber: {webMaterial.ItemNumber} is successfully created";
            else
                WebMaterialIndexModel.AlertMessage = $"ItemNumber: {webMaterial.ItemNumber} is not successfully created";

            return RedirectToAction(nameof(Index), new
            {
                searchString = webMaterialIndexModel.EditedWebMaterialDto.ItemNumber,
                //selectedRrn = webMaterialIndexModel.EditedWebMaterialDto.Rrn
                selectedRrn = 0
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(WebMaterialIndexModel webMaterialIndexModel)
        {
            if (WebMaterialIndexModel.EditedWebMaterialDto.ItemNumber != webMaterialIndexModel.EditedWebMaterialDto.ItemNumber)
            {
                var searchString = WebMaterialIndexModel.EditedWebMaterialDto.ItemNumber.ToString();
                if (!await IsSavedItemValidated(webMaterialIndexModel.EditedWebMaterialDto.ItemNumber))
                {
                    WebMaterialIndexModel.EditedWebMaterialDto = webMaterialIndexModel.EditedWebMaterialDto;
                    return RedirectToAction(nameof(Index),
                        new
                        {
                            searchString = searchString,
                            selectedRrn = webMaterialIndexModel.EditedWebMaterialDto.Rrn
                        });
                }
            }

            _itemRepository.UpdateWebMaterial(_mapper.Map<WebMaterial>(webMaterialIndexModel.EditedWebMaterialDto));

            if (await _itemRepository.SaveAsync())
                WebMaterialIndexModel.AlertMessage = $"ItemNumber: {webMaterialIndexModel.EditedWebMaterialDto.ItemNumber} is successfully updated";
            else
                WebMaterialIndexModel.AlertMessage = $"ItemNumber: {webMaterialIndexModel.EditedWebMaterialDto.ItemNumber} is not successfully updated";

            return RedirectToAction(nameof(Index), new { searchString = webMaterialIndexModel.EditedWebMaterialDto.ItemNumber, selectedRrn = 0 });
        }

        public async Task<IActionResult> DeleteAsync(int rrn)
        {
            var webMaterialFromRepo = await _itemRepository.GetWebMaterialByIdAsync(rrn);
            return View(_mapper.Map<WebMaterialDto>(webMaterialFromRepo));
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDeleteAsync(int rrn)
        {
            var webMaterialFromRepo = await _itemRepository.GetWebMaterialByIdAsync(rrn);
            _itemRepository.RemoveWebMaterial(webMaterialFromRepo);

            if (await _itemRepository.SaveAsync())
                WebMaterialIndexModel.AlertMessage = $"ItemNumber: {webMaterialFromRepo.ItemNumber} is successfully deleted";
            else
                WebMaterialIndexModel.AlertMessage = $"ItemNumber: {webMaterialFromRepo.ItemNumber} is not successfully deleted";

            return RedirectToAction(nameof(Index), new { selectedRrn = 0 });

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // User Role Control
        private static List<UserConfig> GetUserConfig()
        {
            string jsonFilePath = Environment.CurrentDirectory + "\\Configs\\UserConfig.json";
            string jsonTxt = System.IO.File.ReadAllText(jsonFilePath);
            return JsonSerializer.Deserialize<List<UserConfig>>(jsonTxt);
        }

        private User WebAuthorize()
        {
            List<UserConfig> userRoles = GetUserConfig();
            User user = new()
            {
                UserAccount = User.Identity.Name
            };
            var userRole = userRoles.Where(userRole => userRole.UserAccount.Contains(user.UserAccount)).FirstOrDefault()?.Role;
            if (Enum.TryParse(userRole, out UserRole role)) user.UserRole = role;
            return user;
        }

        private async Task<bool> IsSavedItemValidated(string ItemNumber)
        {
            // AlertMessage
            WebMaterialIndexModel.AlertMessage = null;
            // ItemNumber in WebMaterial(table) is unique
            if (await _itemRepository.IsWebMaterialExistedAsync(ItemNumber))
            {
                WebMaterialIndexModel.AlertMessage = $"ItemNumber: {ItemNumber} is already existed in the WebMaterial list. Update aborted.";
                return false;
            }
            // ItemNumber must exist in ItemMaster(table)
            if (!await _itemRepository.IsItemMasterExistedAsync(ItemNumber))
            {
                WebMaterialIndexModel.AlertMessage = $"ItemNumber: {ItemNumber} is not existed in the AX ItemMaster list. Update aborted.";
                return false;
            }
            return true;
        }
    }
}