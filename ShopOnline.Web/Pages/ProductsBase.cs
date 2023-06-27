using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductsBase:ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }

        //public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Products = await ProductService.GetItems();
                var shoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                //await ClearLocalStorage();

                //Products =  await ManageProductsLocalStorageService.GetCollection();

                //var shoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();

                var totalQty = shoppingCartItems.Sum(i => i.Qty);

                ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);

            }
            catch (Exception ex)
            {
                //ErrorMessage = ex.Message;
                throw;
            }
            
        }
        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
        {
            return from product in Products
                   group product by product.CategoryId into prodByCatGroup
                   orderby prodByCatGroup.Key
                   select prodByCatGroup;
        }
        protected string GetCategoryName(IGrouping<int, ProductDto> groupedByProductDtos)
        {
            return groupedByProductDtos.FirstOrDefault(pg => pg.CategoryId == groupedByProductDtos.Key).CategoryName;
        }
    }
}
