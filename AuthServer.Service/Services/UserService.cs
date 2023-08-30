using AuthServer.Core.Dtos;
using AuthServer.Core.Entities;
using AuthServer.Core.Services;
using AuthServer.Core.UnitofWork;
using AuthServer.Data.ContextAccessor;
using AuthServer.Service.Mapping;
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
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitofWork _unitofWork;
        private readonly ISecurityContextAccessor _contextAccessor;

        public UserService(UserManager<UserApp> userManager, IUnitofWork unitofWork, ISecurityContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _unitofWork = unitofWork;
            _contextAccessor = contextAccessor;
        }

        public async Task<Response<UserAppDto>> GetUserByNameAsync()
        {
            var user = await _userManager.FindByNameAsync(_contextAccessor.Username);

            if(user is null)
            {
                return Response<UserAppDto>.Error("UserName not found!", (int)HttpStatusCode.NotFound);
            }

            user.Addresses = await _unitofWork.GetRepository<Address>()
                 .GetAll()
                 .Include(x => x.Neighborhood)
                    .ThenInclude(y => y.Town)
                    .ThenInclude(y => y.City)
                 .ToListAsync();

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), (int)HttpStatusCode.OK);
        }

        public async Task<Response<UserAppDto>> RegisterUserAsync(CreateUserDto createUserDto)
        {
            UserApp user = new()
            {
                UserName = createUserDto.UserName,
                Email = createUserDto.Email,
            };

            var result = await _userManager.CreateAsync(user, createUserDto.Password);

            if(!result.Succeeded)
            {
                var errors = result.Errors
                                   .Select(x => x.Description)
                                   .ToList();

                return Response<UserAppDto>.Error(new ErrorDto(errors, true), (int)HttpStatusCode.BadRequest);
            }
            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), (int)HttpStatusCode.OK);
        }
    }
}
