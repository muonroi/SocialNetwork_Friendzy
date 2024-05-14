namespace Infrastructure.Factorys;

public class GrpcConfigClientFactory<T> where T : ClientBase<T>
{
    public T CreateClient(string address)
    {
        GrpcChannel channel = GrpcChannel.ForAddress(address);
        return (T)Activator.CreateInstance(typeof(T), channel)!;
    }
}