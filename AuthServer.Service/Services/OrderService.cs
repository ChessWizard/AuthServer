using AuthServer.Core.Dtos;
using AuthServer.Core.Entities;
using AuthServer.Core.Enums;
using AuthServer.Core.Services;
using AuthServer.Core.UnitofWork;
using AuthServer.Data.Context;
using AuthServer.Data.ContextAccessor;
using AuthServer.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using SharedLibrary.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AuthServer.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ISecurityContextAccessor _contextAccessor;
        private readonly AuthServerDbContext _context;

        public OrderService(IUnitofWork unitofWork, ISecurityContextAccessor contextAccessor, AuthServerDbContext context)
        {
            _unitofWork = unitofWork;
            _contextAccessor = contextAccessor;
            _context = context;
        }

        public async Task<Response<NoDataDto>> CreateOrderAsync(CreateOrderDto createProductDto)
        {
            var user = await _context.Users
                .Include(x => x.Orders)
                .Include(x => x.Addresses)
                .FirstOrDefaultAsync(x => x.Id == _contextAccessor.UserId);

            if(user is null)
                return Response<NoDataDto>.Error("User not found!", (int)HttpStatusCode.BadRequest);

            Order order = new()
            {
                UserApp = user,
                Title = $"{user.Orders.Count() + 1}. sipariş",
                OrderStatus = OrderStatus.WaitingForApprove,
                Description = $"{user.Addresses.OrderByDescending(x => x.CreatedDate).Select(x => x.Description).FirstOrDefault()} {DateTimeOffset.UtcNow}"
            };
            await _unitofWork.GetRepository<Order>().AddAsync(order);

            foreach (var item in createProductDto.RequestedProducts)
            {
                var product = await _unitofWork.GetRepository<Product>()
                    .GetAsync(x => x.Id == item.ProductId && !x.IsDeleted);                

                if(product is null) continue;

                for (int i = 0; i < item.Count; i++)
                {                  
                    OrderProduct orderProduct = new()
                    {
                        ProductId = item.ProductId,
                        OrderId = order.Id
                    };

                    await _unitofWork.GetRepository<OrderProduct>()
                        .AddAsync(orderProduct);
                }
                
            }            
            var result = await _unitofWork.SaveChangesAsync();

            return result > 0 ? Response<NoDataDto>.Success((int)HttpStatusCode.Created)
                              : Response<NoDataDto>.Error("Order cannot create!", (int)HttpStatusCode.BadRequest);
        }

        public async Task<Response<OrderDto>> GetCurrentOrderAsync()
        {
            var order = await _unitofWork.GetRepository<Order>()
                .GetAll()
                .Include(x => x.UserApp)
                    .ThenInclude(x => x.Addresses)
                .Where(x => x.UserAppId == _contextAccessor.UserId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync();

            if (order is null)
                return Response<OrderDto>.Error("Current Order Not Found!", (int)HttpStatusCode.NotFound);

            var orderDto = ObjectMapper.Mapper.Map<OrderDto>(order);

            return Response<OrderDto>.Success(orderDto, (int)HttpStatusCode.OK);
        }
    }
}
