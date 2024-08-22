using Microsoft.EntityFrameworkCore;
using MosadApiServer.Models;
using MosadApiServer.Enums;
using Microsoft.AspNetCore.Mvc;
using MosadApiServer.Utils;
using System.Drawing;
using System.Security.Cryptography.Xml;


namespace MosadApiServer.Data;

public class LogicToMoving
{
    static private Agent _agent = new Agent();
    static private Target _target = new Target();
    static private Direction _direction = new Direction();
    static private Coordinates _location = new Coordinates();

    public LogicToMoving(Agent agent, Target target, Direction direction,Coordinates location)
    {
        _agent = agent;
        _target = target;
        _direction = direction;
        _location = location;
    }
    static int x1 = _agent.location.x;
    static int y1 = _agent.location.y;
    static int x2 = _target.location.x;
    static int y2 = _target.location.y;
    public static async Task<Coordinates> CreatPinlocation(int x,int y)
    {
        _agent.location.x  =  x;
        _agent.location.y =  y;
        return _agent.location;
    }
    public static async Task<IActionResult> GetDiractionAsync(Coordinates location1,Coordinates location2)
    {

        int equalX = location2.x - location1.x;
        int equalY = location2.y - location1.y;
        int absoluteValueX = Math.Abs(equalX);
        int absoluteValueY = Math.Abs(equalY);
       
        if (equalX == 0 && equalY > 0)//w
        {
            _direction = Direction.w;
            for (int j = 1; j <= absoluteValueY; j++)
            {
                location1.y += 1;
            }
        }
        if (equalX == 0 && equalY < 0)//e
        {
            _direction = Direction.e;
            for (int j = 1; j <= absoluteValueY; j++)
            {
                location1.y -= 1;
            }
            
        }
        if (equalX > 0 && equalY == 0)//s
        {
            _direction = Direction.s;
            for (int i = 1; i <= absoluteValueX; i++)
            {
                location1.x += 1;
            }
        }
        if (equalX < 0 && equalY == 0)//n
        {
            _direction = Direction.n;
            for (int i = 1; i <= absoluteValueX; i++)
            {
                location1.x -= 1;
            }
        }
        if (equalX > 0 && equalY > 0)//sw
        {
            _direction = Direction.sw;
            for (int i = 1; i <= absoluteValueX; i++)
            {
                location1.x += 1;
            }
            for (int j = 1; j <= absoluteValueY; j++)
            {
                location1.y += 1;
            }
        }
        if (equalX < 0 && equalY < 0)//ne
        {
            _direction = Direction.ne;
            for (int i = 1; i <= absoluteValueX; i++)
            {
                location1.x -= 1;
            }
            for (int j = 1; j <= absoluteValueY; j++)
            {
                location1.y -= 1;
            }
        }
        if (equalX < 0 && equalY > 0)//nw
        {
            _direction = Direction.nw;
            for (int i = 1; i <= absoluteValueX; i++)
            {
                location1.x -= 1;
            }
            for (int j = 1; j <= absoluteValueY; j++)
            {
                location1.y += 1;
            }
        }
        if (equalX > 0 && equalY < 0)//se
        {
            _direction = Direction.se;
            for (int i = 1; i <= absoluteValueX; i++)
            {
                location1.x += 1;
            }
           
            for (int j = 1; j <= absoluteValueY; j++)
            {
                location1.y -= 1;
            }
            
        }
        return await Task.FromResult(new OkObjectResult($"{location1} "+
            " " + $" move in to : {_direction}"+
            " " + $" in direction of : {_direction}" +
            " " + $" X:{absoluteValueX} " +
            " " + $" and Y:{absoluteValueY}")); ;
    }
    public static async Task<Coordinates> Move(Direction direction, Coordinates? coordinates, int? absX =1,int? absY=1 )
    {
        switch (direction)
        {
            case Direction.w:
                {
                    for (int j = 1; j <= absY; j++)
                    {
                        if (coordinates.y >= 0 && coordinates.y < 1000)
                        {
                            coordinates.y += 1;
                        }
                        else
                        {
                            throw new ArgumentException(String.Format($"coordinates corect is:{coordinates.x},{coordinates.y}: Can't get out of the matrix;"));
                        }
                        break;
                    }
                }
                return coordinates;
                break;


            case Direction.e:
                {
                    for (int j = 1; j <= absY; j++)
                    {
                        if (coordinates.y > 0 && coordinates.y <= 1000)
                        {
                            coordinates.y -= 1;
                        }
                        else
                        {
                            throw new ArgumentException(String.Format($"coordinates corect is:{coordinates.x},{coordinates.y}: Can't get out of the matrix;"));
                        }
                        break;
                    }
                }
                return coordinates;
                break;

            case Direction.s:
                {
                    for (int i = 1; i <= absX; i++)
                    {
                        if (coordinates.x >= 0 && coordinates.x < 1000)
                        {
                            coordinates.x += 1;
                        }
                        else
                        {
                            throw new ArgumentException(String.Format($"coordinates corect is:{coordinates.x},{coordinates.y}: Can't get out of the matrix;"));
                        }
                        break;
                    }

                }
                return coordinates;
                break;

            case Direction.n:
                {
                    for (int i = 1; i <= absX; i++)
                    {
                        if (coordinates.x > 0 && coordinates.x <= 1000)
                        {
                            coordinates.x -= 1;
                        }
                        else
                        {
                            throw new ArgumentException(String.Format($"coordinates corect is:{coordinates.x},{coordinates.y}: Can't get out of the matrix;"));
                        }
                        break;
                    }
                }
                return coordinates;
                break;
            case Direction.sw:
                {
                    for (int i = 1; i <= absX; i++)
                    {
                        if (coordinates.x >= 0 && coordinates.x < 1000)
                        {
                            coordinates.x += 1;
                        }
                        else
                        {
                            throw new ArgumentException(String.Format($"coordinates corect is:{coordinates.x},{coordinates.y}: Can't get out of the matrix;"));
                        }
                        break;
                    }
                    for (int j = 1; j <= absY; j++)
                    {
                        if (coordinates.y >= 0 && coordinates.y < 1000)
                        {
                            coordinates.y += 1;
                        }
                        else
                        {
                            throw new ArgumentException(String.Format($"coordinates corect is:{coordinates.x},{coordinates.y}: Can't get out of the matrix;"));
                        }
                        break;
                    }
                }
                return coordinates;
                break;
            case Direction.ne:
                {
                    for (int i = 1; i <= absX; i++)
                    {
                        if (coordinates.x > 0 && coordinates.x <= 1000)
                        {
                            coordinates.x -= 1;
                        }
                        else
                        {
                            throw new ArgumentException(String.Format($"coordinates corect is:{coordinates.x},{coordinates.y}: Can't get out of the matrix;"));
                        }
                        break;

                    }
                    for (int j = 1; j <= absY; j++)
                    {
                        if (coordinates.y > 0 && coordinates.y <= 1000)
                        {
                            coordinates.y -= 1;
                        }
                        else
                        {
                            throw new ArgumentException(String.Format($"coordinates corect is:{coordinates.x},{coordinates.y}: Can't get out of the matrix;"));
                        }
                        break;
                    }

                }
                return coordinates;
                break;
            case Direction.nw:
                {
                    for (int i = 1; i <= absX; i++)
                    {
                        if (coordinates.x > 0 && coordinates.x <= 1000)
                        {
                            coordinates.x -= 1;
                        }
                        else
                        {
                            throw new ArgumentException(String.Format($"coordinates corect is:{coordinates.x},{coordinates.y}: Can't get out of the matrix;"));
                        }
                        break;

                    }
                    for (int j = 1; j <= absY; j++)
                    {
                        if (coordinates.y >= 0 && coordinates.y < 1000)
                        {
                            coordinates.y += 1;
                        }
                        else
                        {
                            throw new ArgumentException(String.Format($"coordinates corect is:{coordinates.x},{coordinates.y}: Can't get out of the matrix;"));
                        }
                        break;

                    }

                }
                return coordinates;
                break;
            case Direction.se:
                {
                    for (int i = 1; i <= absX; i++)
                    {
                        if (coordinates.x >= 0 && coordinates.x < 1000)
                        {
                            coordinates.x += 1;
                        }
                        else
                        {
                            throw new ArgumentException(String.Format($"coordinates corect is:{coordinates.x},{coordinates.y}: Can't get out of the matrix;"));
                        }
                        
                        break;
                    }
                    for (int j = 1; j <= absY; j++)
                    {
                        if (coordinates.y > 0 && coordinates.y <= 1000)
                        {
                            coordinates.y -= 1;
                        }
                        else
                        {
                            throw new ArgumentException(String.Format($"coordinates corect is:{coordinates.x},{coordinates.y}: Can't get out of the matrix;"));
                        }
                        break;
                        
                    }
                }
            
                return coordinates;
                break;

            default:
                return coordinates;
                break;
        }
    }
    
    public static async Task<double> getdistanceasync(Coordinates coordinates1, Coordinates coordinates2)
    {
        double distance = Math.Sqrt(Math.Pow(coordinates2.x - coordinates1.x, 2) +
            Math.Pow(coordinates2.y - coordinates1.y, 2));
        return distance;
    }
}
