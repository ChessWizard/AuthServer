using AuthServer.Core.Configuration;
using AuthServer.Core.Dtos;
using AuthServer.Core.Entities;
using AuthServer.Core.Enums;
using AuthServer.Core.Services;
using AuthServer.Core.UnitofWork;
using AuthServer.Service.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configurations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly TokenOptionConfigurations _tokenOptions;
        private readonly IUnitofWork _unitofwork;

        public TokenService(UserManager<UserApp> userManager, IOptions<TokenOptionConfigurations> tokenOptions, IUnitofWork unitofwork)
        {
            _userManager = userManager;
            _tokenOptions = tokenOptions.Value;
            _unitofwork = unitofwork;
        }

        /// <summary>
        /// User Registration içeren servisler için token üretme mekanizması
        /// </summary>
        /// <param name="userApp"></param>
        /// <returns></returns>
        public async Task<TokenDto> CreateToken(UserApp userApp)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.RefreshTokenExpiration);

            var securityKey = SignTokenService.GetSymmetricSecurityKey(_tokenOptions.SecurityKey);

            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken tokenModel = new
                                    (issuer: _tokenOptions.Issuer,
                                      expires: accessTokenExpiration,
                                      notBefore: DateTime.Now,
                                      claims: await SetUserClaims(userApp, _tokenOptions.Audiences),
                                      signingCredentials: credentials
                                    );

            JwtSecurityTokenHandler tokenHandler = new();

            var createdToken = tokenHandler.WriteToken(tokenModel);

            TokenDto tokenDto = new()
            {
                AccessToken = createdToken,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };

            return tokenDto;
        }

        /// <summary>
        /// Client'lar arası güvenli iletişim için token üretimi
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ClientTokenDto CreateTokenByClient(ClientOptions client)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);

            var securityKey = SignTokenService.GetSymmetricSecurityKey(_tokenOptions.SecurityKey);

            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken tokenModel = new
                                    (issuer: _tokenOptions.Issuer,
                                      expires: accessTokenExpiration,
                                      notBefore: DateTime.Now,
                                      claims: SetClientClaims(client),
                                      signingCredentials: credentials
                                    );

            JwtSecurityTokenHandler tokenHandler = new();

            var createdToken = tokenHandler.WriteToken(tokenModel);

            ClientTokenDto clientTokenDto = new()
            {
                AccessToken = createdToken,
                AccessTokenExpiration = accessTokenExpiration,
            };

            return clientTokenDto;
        }

        private string CreateRefreshToken()
        {
            var bytes = new Byte[32];
            using var random = RandomNumberGenerator.Create();

            random.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }

        private async Task<List<Claim>> SetUserClaims(UserApp userApp, List<string> audiences)
        {
            var userRoles = await _userManager.GetRolesAsync(userApp);

            List<Claim> userClaims = new()
            {
                new(ClaimTypes.NameIdentifier, userApp.Id.ToString()),
                new(ClaimTypes.Email, userApp.Email),
                new(ClaimTypes.Name, userApp.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            userClaims.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            userClaims.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));

            if (userRoles.Contains(RoleType.Individual.ToString()))
            {
                var isLoyal = await IsUserLoyal(userApp);
                userClaims.Add(new(ClaimConstants.IsLoyalUser, isLoyal.ToString()));
            }
                

            return userClaims;
        }

        /// <summary>
        /// Bir kullanıcının sadakat derecesini ölçmeye yarar
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<bool> IsUserLoyal(UserApp user)
        {
            var startDate = DateTimeOffset.UtcNow.AddMonths(-1);
            var endDate = DateTimeOffset.UtcNow;

            var userUniqueOrderProducts = await _unitofwork.GetRepository<Order>()
                .GetAll()
                .Include(x => x.OrderProducts)
                    .ThenInclude(x => x.Product)
                .Where(x => x.UserAppId == user.Id)
                .Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate)
                .SelectMany(x => x.OrderProducts)
                .Select(x => x.Product.Id)
                .Distinct()
                .CountAsync();

            return userUniqueOrderProducts >= OrderConstants.LoyalCustomerUniqueProductLimit 
                                           ? true 
                                           : false;
        }

        private List<Claim> SetClientClaims(ClientOptions client)
        {
            List<Claim> clientClaims = new()
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, client.Id.ToString()),
            };
            clientClaims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            return clientClaims;
        }
    }
}
