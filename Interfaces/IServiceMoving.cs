using MosadApiServer.Enums;
using MosadApiServer.Models;

namespace MosadApiServer.Interfaces
{
    public interface IServiceMoving
    {
        Task<Coordinates> CreatPinlocation(int pointX, int pointY);
        Task<Direction> GetDirectionAsync(Coordinates location1, Coordinates location2);
        Task<Coordinates> Move(Direction direction, Coordinates coordinates, int? X = 1, int? Y = 1);
        Task<Coordinates> UpdateCoordination(Coordinates coordinates, int pointX, int pointY);
        Task<double> GetDistance(Coordinates coordinates1, Coordinates coordinates2);
    }
}
