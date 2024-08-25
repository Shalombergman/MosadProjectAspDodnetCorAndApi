using Microsoft.EntityFrameworkCore;
using MosadApiServer.Models;
using MosadApiServer.Enums;
using Microsoft.AspNetCore.Mvc;
using MosadApiServer.Utils;
using System.Drawing;
using System.Security.Cryptography.Xml;
using System.Reflection;


namespace MosadApiServer.Servises;

public class ServiceMoving
{
    static private Agent _agent = new Agent();
    static private Target _target = new Target();
    static private Direction _direction = new Direction();
    static private Coordinates _location = new Coordinates();


    public ServiceMoving(Agent agent, Target target, Direction direction, Coordinates location)
    {
        _agent = agent;
        _target = target;
        _direction = direction;
        _location = location;
    }


    public static async Task<Coordinates> CreatPinlocation(int pointX, int pointY)
    {
        if (pointX < 0 || pointX > 999 || pointY < 0 || pointY > 999)
        {
            throw new ArgumentException(
             string.Format($"coordinates corect is:{_location.x},{_location.y}: Can't get out of the matrix;"));
        }
        else
        {
            _location.y = pointY;
            _location.x = pointX;
        }
        return _location;
    }




    public static async Task<Direction> GetDirectionAsync(Coordinates location1, Coordinates location2)
    {

        int equalX = location2.x - location1.x;
        int equalY = location2.y - location1.y;
        if (equalX == 0 && equalY > 0) _direction = Direction.w;
        if (equalX == 0 && equalY < 0) _direction = Direction.e;
        if (equalX > 0 && equalY == 0) _direction = Direction.s;
        if (equalX < 0 && equalY == 0) _direction = Direction.n;
        if (equalX > 0 && equalY > 0) _direction = Direction.sw;
        if (equalX < 0 && equalY < 0) _direction = Direction.ne;
        if (equalX < 0 && equalY > 0) _direction = Direction.nw;
        if (equalX > 0 && equalY < 0) _direction = Direction.se;
        return _direction;
    }



    public static int[,] matrix = ServiceMatrix.CreateMatrix(1000, 1000);

    public static async Task<Coordinates> Move(Direction direction, Coordinates coordinates, int? X = 1,
        int? Y = 1)
    {
        //מחזיק במשתנה מערך של טאפלים של כל האפשרויות תזוזה של האובייקט
        var diractionToMove = CoordinatinMatrixDirection(coordinates.x, coordinates.y);
        //נבחר את הטאפל שנמצא באותו מיקום שנמצא הכיוון ב enum
        var (pointX, pointY) = diractionToMove[(int)direction];
        Coordinates newCoordinates = await UpdateCoordination(coordinates, pointX, pointY);
        //await UpdateCoordination(coordinates, 0, pointY); 
        return newCoordinates;
    }

    public static async Task<Coordinates> UpdateCoordination(Coordinates coordinates, int pointX, int pointY)
    {
        if (pointX < 0 || pointX > 999 || pointY < 0 || pointY > 999)
        {
            throw new ArgumentException(
                string.Format($"coordinates corect is:{coordinates.x},{coordinates.y}: Can't get out of the matrix;"));
        }
        else
        {
            coordinates.x = pointX;
            coordinates.y = pointY;
        }
        return coordinates;
    }


    public static async Task<double> GetDistance(Coordinates coordinates1, Coordinates coordinates2)
    {
        int equalX = coordinates2.x - coordinates1.x;
        int equalY = coordinates2.y - coordinates1.y;
        int absoluteValueX = Math.Abs(equalX);
        int absoluteValueY = Math.Abs(equalY);
        return Math.Sqrt(Math.Pow(absoluteValueX, 2) + Math.Pow(absoluteValueY, 2));

    }

    public static (int row, int col)[] CoordinatinMatrixDirection(int i, int j)
    {
        //הגדרת מערך ששומר לפי אינדקס טאפלים של כיוונים i,j ובכך נשווה אינדקסים של duraction
        var movementMap = new List<(int row, int col)>();
        {
            movementMap.Add((i - 1, j - 1)); // ne //index 0...
            movementMap.Add((i - 1, j)); // n
            movementMap.Add((i - 1, j + 1)); // nw
            movementMap.Add((i, j - 1)); // e
            movementMap.Add((i, j + 1)); // w
            movementMap.Add((i + 1, j - 1)); // se
            movementMap.Add((i + 1, j)); //s
            movementMap.Add((i + 1, j + 1)); // sw
        }

        return movementMap.ToArray();
    }

}





