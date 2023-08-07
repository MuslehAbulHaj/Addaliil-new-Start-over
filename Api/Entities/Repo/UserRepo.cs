using Api.Data;
using Api.Entities.DTOs;
using Api.Entities.Helpers;
using Api.Entities.Interfaces;
using Api.Entities.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Api.Entities.Repo
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public UserRepo(AppDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;

        }

        public async Task<MemberDto> GetMemberByIdAsync(Guid id)
        {
            return await _context.Users
                .Where(x => x.Id == id)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<MemberDto> GetMemberByNameAsync(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParms userParams)
        {
            var query = _context.Users.AsQueryable();
            query = query.Where(user => user.UserName != userParams.CurrentUsername);
            query = query.Where(user => user.Gender == userParams.Gender);

            
            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            
            query = userParams.OrderBy switch{
                "created" => query.OrderByDescending(u=>u.Created),
                _ => query.OrderByDescending(u=>u.LastActive)
            };

            // this code is the default code before changing the filter of gender 
            // and exclude current user from the list
            // var query = _context.Users
            //     .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            //     .AsNoTracking();
            //     return await PagedList<MemberDto>.createAsync(query, userParms.PageNumber, userParms.PageSize);

            return await PagedList<MemberDto>.createAsync(
                query.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider),
                userParams.PageNumber,
                userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(Guid id)
        {
            return await _context.Users
                    .Include(p => p.Photos)
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AppUser> GetUserByNameAsync(string username)
        {
            return await _context.Users
                    .Include(p => p.Photos)
                    .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                    .Include(p => p.Photos)
                    .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            var x= await _context.SaveChangesAsync();
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}