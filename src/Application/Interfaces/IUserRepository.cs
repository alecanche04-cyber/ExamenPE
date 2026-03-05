using FoodCampus.Domain.Entities;

namespace FoodCampus.Application.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
}
