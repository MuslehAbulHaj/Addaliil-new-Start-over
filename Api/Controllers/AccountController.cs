using System.Security.Cryptography;
using System.Text;
using Api.Data;
using Api.Entities.DTOs;
using Api.Entities.Interfaces;
using Api.Entities.Models;
using API.Entities.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(AppDbContext context, ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (await UsersExists(registerDto.Username)) return BadRequest("The user is already taken!");

            var user = _mapper.Map<AppUser>(registerDto);

            using var hmac = new HMACSHA512();


            user.UserName = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash //generate random hash string that need array of bytes
                        (Encoding.UTF8.GetBytes(registerDto.Password));
            //this cutting password into 
            //required form of computedhash function needs to be converted to byte array
            user.PasswordSalt = hmac.Key; // hmac comes with auto-generated key that will used in Salt




            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto()
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            //this function is only checking the username & password are matched 
            //based on what we stored in Db as hased password
            var user = await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);

            if (user == null) return Unauthorized("User name is not found");

            //we give the stored password salt as  the key of HMAC 
            var hmac = new HMACSHA512(user.PasswordSalt);
            // then we generate new hash based on the key
            var comuptedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            //if they are not matched it will return with unauthorized response
            for (int i = 0; i < user.PasswordHash.Length; i++)
            {
                if (user.PasswordHash[i] != comuptedHash[i]) return Unauthorized("Wrong password!");
            }
            string x = user.Photos.FirstOrDefault(p => p.IsMain == true).Url;

            return new UserDto()
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain == true).Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }


        private async Task<bool> UsersExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}