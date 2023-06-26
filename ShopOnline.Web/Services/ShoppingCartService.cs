using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient _httpClient;

        public ShoppingCartService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<CartItemToAddDto>("api/ShoppingCart", cartItemToAddDto);
                if(response.IsSuccessStatusCode)
                {
                    if(response.StatusCode == System.Net.HttpStatusCode.NoContent) 
                    {
                        return default(CartItemDto);
                    }
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} Message: - {message}");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<CartItemDto> DeleteItem(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/ShoppingCart/{id}");
                if(response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                return default(CartItemDto);
            }
            catch (Exception ex)
            {
                //Log Exception
                throw;
            }
        }

        public async Task<List<CartItemDto>> GetItems(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/ShoppingCart/{userId}/GetItems");
                if(response.IsSuccessStatusCode)
                {
                    if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<CartItemDto>().ToList();
                    }
                    return await response.Content.ReadFromJsonAsync<List<CartItemDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status code:{response.StatusCode} Message: - {message}");
                }
            }
            catch (Exception )
            {

                throw;
            }
        }
    }
}
