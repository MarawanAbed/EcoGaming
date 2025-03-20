using AutoMapper;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Abstraction;
using DataAccessLayer.Enitites;
using DataAccessLayer.Repo.Abstraction;

namespace BusinessLogicLayer.Services.Implementation
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IMapper _mapper;

        public OrderServices(IOrderRepo orderRepo,IMapper mapper)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
        }

        public async Task AddOrder(OrderDto order)
        {
            await _orderRepo.AddOrder(_mapper.Map<Order>(order));
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrders()
        {
            var orders = await _orderRepo.GetAllOrders();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrder(int orderId)
        {
           var order= await _orderRepo.GetOrderById(orderId);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> GetOrderById(string userId)
        {
            return _mapper.Map<OrderDto>(await _orderRepo.GetOrderById(userId));
        }


        public async Task<List<OrderDto>> GetOrdersByUserId(string userId)
        {
            var orders = await _orderRepo.GetOrdersByUserId(userId);
            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task RemoveOrder(int orderId)
        {
            await _orderRepo.DeleteOrder(orderId);
        }

        public async Task UpdateOrder(OrderDto order)
        {
            await _orderRepo.UpdateOrder(_mapper.Map<Order>(order));
        }
    }
}