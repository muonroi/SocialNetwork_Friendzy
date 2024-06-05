namespace Matched.Friend.Application.Infrastructure;

public static class ManageRoles
{
    public static IServiceCollection AuthorizationRoles(this IServiceCollection services)
    {
        _ = services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyConstant.ParentPolicy, policy =>
                policy.RequireRole(ManageRolesName.UserController));

            options.AddPolicy(PolicyConstant.ChildPolicy, policy =>
                policy.RequireAssertion(context =>
                    context.User.IsInRole(ManageRolesName.GetUserByKey) ||
                    context.User.IsInRole(ManageRolesName.GetUserByKeys)));
        });

        return services;
    }
}