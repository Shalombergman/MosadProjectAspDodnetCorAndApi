using Microsoft.EntityFrameworkCore;
using MosadApiServer.Models;
using MosadApiServer.Enums;
using Microsoft.AspNetCore.Mvc;
using MosadApiServer.Utils;
using System.Drawing;
using System.Security.Cryptography.Xml;
using System.Reflection;
using MosadApiServer.Data;
using MosadApiServer.Controllers;
using MosadApiServer.Interfaces;


namespace MosadApiServer.Servises;

public class ServiceMoving :IServiceMoving
{

    private Coordinates _coordinates = new Coordinates();
    private ApplicationDbContext _context;
    private readonly ServiceMission _serviceMission;


    //private AgentsController _agentsController;




    public ServiceMoving(ApplicationDbContext context, ServiceMission serviceMission /*AgentsController agentsController*/)
    {
        this._serviceMission = serviceMission;
        this._context = context;
        

        //this._agentsController = agentsController;
    }


    public async Task<Coordinates> CreatPinlocation(int pointX, int pointY)
    {
        if (pointX < 0 || pointX > 999 || pointY < 0 || pointY > 999)
        {
            throw new ArgumentException(
             string.Format($"coordinates corect is:{this._coordinates.x},{this._coordinates.y}: Can't get out of the matrix;"));
        }
        else
        {
            this._coordinates.y = pointY;
            this._coordinates.x = pointX;
            await _serviceMission.OfferedMission();
        }

        return _coordinates;
    }




    public  async Task<Direction> GetDirectionAsync(Coordinates location1, Coordinates location2)
    {
        Direction _direction;
        if (location1 != null && location2 != null) {
           
            int equalX = location2.x - location1.x;
            int equalY = location2.y - location1.y;
            if (equalX == 0 && equalY > 0) _direction = Direction.w;
            else if (equalX == 0 && equalY < 0) _direction = Direction.e;
            else if (equalX > 0 && equalY == 0) _direction = Direction.s;
            else if (equalX < 0 && equalY == 0) _direction = Direction.n;
            else if (equalX > 0 && equalY > 0) _direction = Direction.sw;
            else if (equalX < 0 && equalY < 0) _direction = Direction.ne;
            else if (equalX < 0 && equalY > 0) _direction = Direction.nw;
            else if (equalX > 0 && equalY < 0) _direction = Direction.se;
            else _direction = Direction.L;
            return _direction;
        }
        throw new Exception("teh once or mor item location is null");
             
    }



    public  int[,] matrix = ServiceMatrix.CreateMatrix(1000, 1000);

    public  async Task<Coordinates> Move(Direction direction, Coordinates coordinates, int? X = 1,
        int? Y = 1)
    {
        _coordinates = coordinates;
        if (coordinates == null) throw new ArgumentNullException(nameof(coordinates));
        //מחזיק במשתנה מערך של טאפלים של כל האפשרויות תזוזה של האובייקט
        var diractionToMove = CoordinatinMatrixDirection(_coordinates.x, _coordinates.y);
        //נבחר את הטאפל שנמצא באותו מיקום שנמצא הכיוון ב enum
        var (pointX, pointY) = diractionToMove[(int)direction];
        Coordinates newCoordinates = await UpdateCoordination(_coordinates, pointX, pointY);
        
        //await UpdateCoordination(coordinates, 0, pointY); 
        return newCoordinates;
    }

    public  async Task<Coordinates> UpdateCoordination(Coordinates coordinates, int pointX, int pointY)
    {
        _coordinates = coordinates;
        if (pointX < 0 || pointX > 999 || pointY < 0 || pointY > 999)
        {
            throw new ArgumentException(
                string.Format($"coordinates corect is:{_coordinates.x},{_coordinates.y}: Can't get out of the matrix;"));
        }
        else
        {
            _coordinates.x = pointX;
            _coordinates.y = pointY;
            await this._serviceMission.OfferedMission();
            await this._context.SaveChangesAsync();
        }
        return _coordinates;
    }


    public  async Task<double> GetDistance(Coordinates coordinates1, Coordinates coordinates2)
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





