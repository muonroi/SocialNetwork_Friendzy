using Shared.Models;

namespace Infrastructure.Commons;

public class WorkContextAccessor : IWorkContextAccessor
{
    private static readonly AsyncLocal<WorkContextInfoModel?> LocalWorkContext = new();

    public WorkContextInfoModel? WorkContext
    {
        get => LocalWorkContext.Value;
        set => LocalWorkContext.Value = value;
    }
}