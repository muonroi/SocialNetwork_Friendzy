namespace User.Application.Commons.Interfaces;

public interface IUserRepository : IRepositoryBaseAsync<UserEntity, long>
{
    Task<UserDto?> GetUserByInput(string input, CancellationToken cancellationToken);

    Task<IEnumerable<UserDto>?> GetUsersByInput(string input, CancellationToken cancellationToken);
}