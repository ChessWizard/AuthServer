using AuthServer.Core.Configuration;
using AuthServer.Core.Dtos;
using AuthServer.Core.Entities;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitofWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<ClientOptions> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitofWork _unitofWork;
        private readonly IRepository<UserRefreshToken> _userRefreshTokenService;

        public AuthenticationService(IOptions<List<ClientOptions>> clients, ITokenService tokenService, UserManager<UserApp> userManager, IUnitofWork unitofWork, IRepository<UserRefreshToken> userRefreshTokenService)
        {
            _clients = clients.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _unitofWork = unitofWork;
            _userRefreshTokenService = userRefreshTokenService;
        }

        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if(loginDto is null)
                throw new ArgumentNullException(nameof(loginDto));

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
                return Response<TokenDto>.Error("Email or password is wrong!", (int)HttpStatusCode.BadRequest);

            var checkPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!checkPassword)
                return Response<TokenDto>.Error("Email or password is wrong!", (int)HttpStatusCode.BadRequest);

            var token = _tokenService.CreateToken(user);

            var userRefreshToken = await _userRefreshTokenService.GetAsync(x => x.UserAppId == user.Id);

            if(userRefreshToken is null)
            {
                UserRefreshToken refreshToken = new()
                {
                    UserAppId = user.Id,
                    Code = token.RefreshToken,
                    Expiration = token.RefreshTokenExpiration
                };
                await _userRefreshTokenService.AddAsync(refreshToken);
            }
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
                _userRefreshTokenService.Update(userRefreshToken);
            }
          
            await _unitofWork.SaveChangesAsync();

            return Response<TokenDto>.Success(token, (int)HttpStatusCode.OK);
        }

        public async Task<Response<ClientTokenDto>> CreateTokenByClientAsync(ClientLoginDto clientLoginDto)
        {
            var client = _clients.FirstOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);

            if(client is null)
            {
                return await Task.FromResult(Response<ClientTokenDto>.Error("Client Id or Client Secret was not found!", (int)HttpStatusCode.NotFound));
            }

            var clientToken = _tokenService.CreateTokenByClient(client);

            return await Task.FromResult(Response<ClientTokenDto>.Success(clientToken, (int)HttpStatusCode.OK));
        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken)
        {
            var isExistRefreshToken = await _userRefreshTokenService.GetAsync(x => x.Code == refreshToken);

            if(isExistRefreshToken is null)
            {
                return Response<TokenDto>.Error("Refresh token not found!", (int)HttpStatusCode.NotFound);
            }

            var user = await _userManager.FindByIdAsync(isExistRefreshToken.UserAppId.ToString());

            if(user is null)
            {
                return Response<TokenDto>.Error("User id not found", (int)HttpStatusCode.NotFound);
            }

            var tokenDto = _tokenService.CreateToken(user);

            isExistRefreshToken.Code = tokenDto.RefreshToken;
            isExistRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

            _userRefreshTokenService.Update(isExistRefreshToken);
            await _unitofWork.SaveChangesAsync();

            return Response<TokenDto>.Success(tokenDto, (int)HttpStatusCode.OK);
        }

        public async Task<Response<NoDataDto>> RevokeRefreshTokenAsync(string refreshToken)
        {
            var isExistRefreshToken = await _userRefreshTokenService.GetAsync(x => x.Code == refreshToken);

            if(isExistRefreshToken is null)
            {
                return Response<NoDataDto>.Error("Refresh token not found!", (int)HttpStatusCode.NotFound);
            }

            _userRefreshTokenService.Remove(isExistRefreshToken);
            await _unitofWork.SaveChangesAsync();

            return Response<NoDataDto>.Success((int)HttpStatusCode.OK);
        }
    }
}
