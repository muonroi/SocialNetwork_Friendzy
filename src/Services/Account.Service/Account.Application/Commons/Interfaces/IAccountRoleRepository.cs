namespace Account.Application.Commons.Interfaces
{
    public interface IAccountRoleRepository
    {
        Task<bool> AssignAccountToRoleId(Guid accountId, Guid roleId, CancellationToken cancellationToken);

    }
}
