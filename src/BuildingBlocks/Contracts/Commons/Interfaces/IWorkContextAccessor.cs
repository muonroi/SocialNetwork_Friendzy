using Shared.Models;

namespace Contracts.Commons.Interfaces;

public interface IWorkContextAccessor
{
    WorkContextInfoModel? WorkContext { get; set; }
}