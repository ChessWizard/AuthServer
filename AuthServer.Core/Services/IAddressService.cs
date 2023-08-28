using AuthServer.Core.Dtos;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IAddressService
    {
        public Task<Response<NoDataDto>> CreateAddressAsync(Guid userId, CreateAddressDto addressDto);
    }
}
