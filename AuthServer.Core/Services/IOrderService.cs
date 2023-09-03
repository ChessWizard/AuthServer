﻿using AuthServer.Core.Dtos;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IOrderService
    {
        Task<Response<NoDataDto>> CreateOrderAsync(CreateOrderDto createProductDto);

        Task<Response<OrderDto>> GetCurrentOrderAsync();
    }
}
