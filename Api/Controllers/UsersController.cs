using Api.Entities.DTOs;
using Api.Entities.Interfaces;
using Api.Entities.Models;
using Api.Extentions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepo _user;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public UsersController(IUserRepo user, IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
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
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _user.GetMemberByNameAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var user = await _user.GetUserByNameAsync(User.GetUserName());

            if (user == null) return NotFound();

            _mapper.Map(memberUpdateDto, user);

            if (await _user.SaveAllAsync()) return NoContent();

            return BadRequest("Faild to update user.");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _user.GetUserByNameAsync(User.GetUserName());
            if (user == null) return NotFound();

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            //if the add photo file successfully done then
            // "result" will have url &public id of new added photo that will be stored in the DB
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0) photo.IsMain = true; else photo.IsMain = false;

            user.Photos.Add(photo);

            if (await _user.SaveAllAsync())
            {

                //after create new photo the action respose shoild be matching  create = 201
                //and to get that respond we use (CreatedAtAction) repose as below
                return CreatedAtAction(nameof(GetUser),
                    new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem in adding Photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _user.GetUserByNameAsync(User.GetUserName());
            if (user == null) return NotFound();
            if (user.Photos.Count > 0)
            {
                int currentMainPhotoId = user.Photos.FirstOrDefault(x => x.IsMain == true).Id;
                if (currentMainPhotoId == photoId) return BadRequest("Photo is already a main photo");

                if (user.Photos.FirstOrDefault(x => x.Id == photoId) == null) return BadRequest("Bad photo Id");

                var photo = user.Photos.FirstOrDefault(x => x.Id == photoId).IsMain = true;
                user.Photos.FirstOrDefault(x => x.Id == photoId).IsMain = true;
                user.Photos.FirstOrDefault(x => x.Id == currentMainPhotoId).IsMain = false;

                if (await _user.SaveAllAsync())
                {
                    //NoContent because it's update only.
                    return NoContent();
                }


            }
            return BadRequest("Problem in update photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _user.GetUserByNameAsync(User.GetUserName());
            if (user == null) return NotFound();

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null) return NotFound();

            if (photo.IsMain == true) return BadRequest("You canot delete Main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);
            if (await _user.SaveAllAsync()) return Ok();

            return BadRequest("problem in deleting photo");
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