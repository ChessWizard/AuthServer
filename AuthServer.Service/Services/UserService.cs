using AuthServer.Core.Dtos;
using AuthServer.Core.Entities;
using AuthServer.Core.Enums;
using AuthServer.Core.Services;
using AuthServer.Core.UnitofWork;
using AuthServer.Data.Context;
using AuthServer.Data.ContextAccessor;
using AuthServer.Data.UnitofWork;
using AuthServer.Service.Mapping;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthServer.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUnitofWork _unitofWork;
        private readonly ISecurityContextAccessor _contextAccessor;
        private readonly AuthServerDbContext _context;

        public UserService(UserManager<UserApp> userManager, IUnitofWork unitofWork, ISecurityContextAccessor contextAccessor, RoleManager<Role> roleManager, AuthServerDbContext context)
        {
            _userManager = userManager;
            _unitofWork = unitofWork;
            _contextAccessor = contextAccessor;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<Response<UserAppDto>> GetUserByNameAsync()
        {
            var user = await _userManager.FindByNameAsync(_contextAccessor.Username);

            if (user is null)
            {
                return Response<UserAppDto>.Error("UserName not found!", (int)HttpStatusCode.NotFound);
            }

            user.Addresses = await _unitofWork.GetRepository<Address>()
                 .GetAll()
                 .Include(x => x.Neighborhood)
                    .ThenInclude(y => y.Town)
                    .ThenInclude(y => y.City)
                 .Where(x => x.UserAppId == user.Id)
                 .ToListAsync();

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), (int)HttpStatusCode.OK);
        }

        /// <summary>
        /// Kullanıcıyı başlangıç rolüyle birlikte kaydetmeye yarar
        /// </summary>
        /// <param name="createUserDto"></param>
        /// <returns></returns>
        public async Task<Response<UserAppDto>> RegisterUserAsync(CreateUserDto createUserDto)
        {
            UserApp user = new()
            {
                UserName = createUserDto.UserName,
                NormalizedUserName = _userManager.NormalizeName(createUserDto.UserName),
                Email = createUserDto.Email,
                NormalizedEmail = _userManager.NormalizeEmail(createUserDto.Email),
                AccountType = createUserDto.AccountType,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, createUserDto.Password);

            await _context.Users
                    .AddAsync(user);

            var role = await SetUserRolesAsync(user, createUserDto.AccountType);
                await _context.UserRoles
                    .AddAsync(role);

            var result = await _unitofWork.SaveChangesAsync();

            return result > 0 ? Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), (int)HttpStatusCode.OK)
                              : Response<UserAppDto>.Error("User or role cannot create", (int)HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// İlgili kullanıcıya rol ataması yapmaya yarar
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task<IdentityUserRole<Guid>> SetUserRolesAsync(UserApp userApp, AccountType accountType)
        {
            var role = accountType switch
            {
                AccountType.Individual => RoleType.Individual,
                AccountType.Corporate => RoleType.Corporate,
                AccountType.Admin => RoleType.Admin,
                _ => RoleType.Individual
            };

            var getRole = await _context.Roles
                .FirstAsync(x => x.RoleType == role);

            IdentityUserRole<Guid> userRole = new()
            {
                UserId = userApp.Id,
                RoleId = getRole.Id
            };

            return userRole;
        }
    }
}
