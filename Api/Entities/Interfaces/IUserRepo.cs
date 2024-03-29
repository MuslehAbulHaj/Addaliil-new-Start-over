using Api.Entities.DTOs;
using Api.Entities.Helpers;
using Api.Entities.Models;

namespace Api.Entities.Interfaces
{
    public interface IUserRepo
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(Guid id);
        Task<AppUser> GetUserByNameAsync(string username);
        Task<PagedList<MemberDto>> GetMembersAsync(UserParms userParms);

        Task<MemberDto> GetMemberByIdAsync(Guid id);
        Task<MemberDto> GetMemberByNameAsync(string username);
    }
}