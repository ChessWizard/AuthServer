using AuthServer.Core.Dtos;
using AuthServer.Core.Entities;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitofWork;
using AuthServer.Data.ContextAccessor;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ISecurityContextAccessor _contextAccessor;
        private readonly UserManager<UserApp> _userManager;

        public AddressService(IUnitofWork unitofWork, UserManager<UserApp> userManager, ISecurityContextAccessor contextAccessor)
        {
            _unitofWork = unitofWork;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        public async Task<Response<NoDataDto>> CreateAddressAsync(CreateAddressDto addressDto)
        {
            var user = await _userManager.FindByIdAsync(_contextAccessor.UserId.ToString());

            if (user is null)
                return Response<NoDataDto>.Error("User not found!", (int)HttpStatusCode.NotFound);

            var neighborhood = await _unitofWork.GetRepository<Neighborhood>()
                .GetAsync(x => x.Id == addressDto.NeighborhoodId);

            if (neighborhood is null)
                return Response<NoDataDto>.Error("Neighborhood not found!", (int)HttpStatusCode.NotFound);

            Address address = new() 
            { 
                UserApp = user,
                Title = addressDto.Title, 
                Description = addressDto.Description, 
                Neighborhood = neighborhood 
            };
            await _unitofWork.GetRepository<Address>()
                .AddAsync(address);

            await _unitofWork.SaveChangesAsync();
            return Response<NoDataDto>.Success((int)HttpStatusCode.NoContent);
        }
    }
}
