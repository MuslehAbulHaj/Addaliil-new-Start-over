using Api.Data;
using Api.Entities.DTOs;
using Api.Entities.Interfaces;
using Api.Entities.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepo _user;
        private readonly IMapper _mapper;
        public UsersController(IUserRepo user, IMapper mapper)
        {
            _mapper = mapper;
            _user = user;
            
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            return Ok(await _user.GetMembersAsync());
        }

        // [HttpGet("GetUserById/{id}")]
        // public async Task<ActionResult<MemberDto>> GetUser(Guid id)
        // {
        //     return await _user.GetMemberByIdAsync(id);
        // }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUserById(string username)
        {
            return await _user.GetMemberByNameAsync(username);
        }
    }
}

/*
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
*/