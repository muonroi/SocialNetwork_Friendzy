namespace Account.Application.Commons.Interfaces;

public interface IRoleRepository
{
    Task<IEnumerable<RoleDTO>> GetAllRole(CancellationToken cancellationToken);

    Task<RoleDTO?> GetRoleByRoleName(RoleConstants role, CancellationToken cancellationToken);
}