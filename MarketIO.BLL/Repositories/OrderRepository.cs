﻿using MarketIO.DAL.Data;
using MarketIO.DAL.Domain;
using MarketIO.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketIO.BLL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MarketIODbContext _appDbContext;
        private readonly ShoppingCart _shoppingCart;

        public OrderRepository(MarketIODbContext appDbContext, ShoppingCart shoppingCart)
        {
            _appDbContext = appDbContext;
            _shoppingCart = shoppingCart;
        }

        public void CreateOrder(Orders order)
        {
            order.Order_Date = DateTime.Now;

            var shoppingCartItems = _shoppingCart.ShoppingCartItems;
            order.OrderTotal = _shoppingCart.GetTotal();

            order.ShoppingCartItems = new List<ShoppingCartItem>();

            //adding the order with its details
            foreach (var shoppingCartItem in shoppingCartItems)
            {
                var orderDetail = new ShoppingCartItem
                {
                    Amount = shoppingCartItem.Amount,
                    Product_Id = shoppingCartItem.Product.Product_Id,
                    Current_Price = shoppingCartItem.Product.Price
                };

                order.ShoppingCartItems.Add(orderDetail);
            }

            _appDbContext.Orders.Add(order);

            _appDbContext.SaveChanges();
        }
    }
}
