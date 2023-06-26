﻿using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public List<CartItemDto> ShoppingCartItems { get; set; }
        public string ErrorMessage { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
        protected async Task DeleteCartItem_Click(int id)
        {
            var cartItemDto = await ShoppingCartService.DeleteItem(id);
            await RemoveCartItem(id);
        }

        private CartItemDto GetCartItemDto(int id)
        {
            return ShoppingCartItems.FirstOrDefault(x => x.Id == id);
        }
        private async Task RemoveCartItem(int id)
        {
            var cartItemDto = GetCartItemDto(id);
            ShoppingCartItems.Remove(cartItemDto);
        }
    }
}