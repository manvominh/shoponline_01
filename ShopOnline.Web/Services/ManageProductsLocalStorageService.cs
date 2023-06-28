using Blazored.LocalStorage;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Services
{
    public class ManageProductsLocalStorageService : IManageProductsLocalStorageService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IProductService _productService;
        private const string key = "ProductCollection";

        public ManageProductsLocalStorageService(ILocalStorageService localStorageService, IProductService productService)
        {
            this._localStorageService = localStorageService;
            this._productService = productService;
        }
        public async Task<IEnumerable<ProductDto>> GetCollection()
        {
            return await this._localStorageService.GetItemAsync<IEnumerable<ProductDto>>(key)
                            ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await this._localStorageService.RemoveItemAsync(key);
        }
        private async Task<IEnumerable<ProductDto>> AddCollection()
        {
            var productCollection = await _productService.GetItems();
            if(productCollection !=null)
            {
                await this._localStorageService.SetItemAsync(key, productCollection);
            }
            return productCollection;
        }
    }
}
