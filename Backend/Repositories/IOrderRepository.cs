﻿using PhoneVault.Models;

namespace PhoneVault.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderById(int id);
        Task AddOrder(Order order);
        Task DeleteOrder(int id);
        Task UpdateOrder(Order order);
    }
}

