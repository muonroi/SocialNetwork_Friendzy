namespace Contracts.Commons.Interfaces;

public interface IWorkContextAccessor
{
    WorkContextInfoDTO? WorkContext { get; set; }
}