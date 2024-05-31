namespace Infrastructure.Commons;

public class WorkContextAccessor : IWorkContextAccessor
{
    private static readonly AsyncLocal<WorkContextInfoDTO?> LocalWorkContext = new();

    public WorkContextInfoDTO? WorkContext
    {
        get => LocalWorkContext.Value;
        set => LocalWorkContext.Value = value;
    }
}