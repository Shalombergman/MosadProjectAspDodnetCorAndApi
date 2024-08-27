
namespace MosadApiServer.Servises;

public interface IJwtService
{
    string CreateToken(string name);
}