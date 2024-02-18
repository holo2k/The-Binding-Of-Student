using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace The_Binding_Of_Student.Code
{
    public static class LevelGeneration
    {
        public static Room[,] rooms = new Room[9, 9];   
        public static int roomsCount = 10;
        static int doorsCount;
        static Random rnd = new Random();
        static Direction dir;
        static int numberOfDoors = rnd.Next(1, 5);
        static int mapHeight = rooms.GetLength(0);
        static int mapWidth = rooms.GetLength(1);

        static public void Start()
        {
            doorsCount = 15;
            rooms = new Room[9, 9];
            Player.PersonPosition = new Vector2(800, 400);
            Player.healthPoints = 99;
            Player.bullets.Clear();
            Game1.gameFinished = false;

            rooms[4, 4] = new Room(new Vector2(4, 4), 1, 0);
            for (int i = 0; i < 4; i++)
            {
                dir = (Direction)rnd.Next(0, 4);
                rooms[4, 4].doors.Add(new Door(dir));
            }
            rooms[4, 4].UpdateState();

            Game1.currentRoom = rooms[4, 4];

            while (doorsCount > 0)
            {
                int directionCount = 4;

                int ringCount = (mapHeight - 1) / 2;
                int[] dy = { 1, 0, -1, 0 };
                int[] dx = { 0, -1, 0, 1 };
                int y = ringCount;
                int x = ringCount - 1;
                int repeatCount = 0;
                for (int ring = 0; ring < ringCount; ring++)
                {
                    y--;
                    x++;
                    repeatCount += 2;
                    for (int direction = 0; direction < directionCount; direction++)
                        for (int repeat = 0; repeat < repeatCount; repeat++)
                        {
                            y += dy[direction];
                            x += dx[direction];
                            if (y < mapHeight && x < mapHeight && x > -1 && y > -1)
                            {
                                if (rooms[y, x] != null && rooms[y, x].doorTop)
                                {
                                    SetRoomDoors(y - 1, x, 0, 1, 2, Direction.Bottom);
                                }
                                if (rooms[y, x] != null && rooms[y, x].doorLeft)
                                {
                                    SetRoomDoors(y, x - 1, 0, 1, 3, Direction.Right);
                                }
                                if (rooms[y, x] != null && rooms[y, x].doorRight)
                                {
                                    SetRoomDoors(y, x + 1, 1, 2, 3, Direction.Left);
                                }
                                if (rooms[y, x] != null && rooms[y, x].doorBot)
                                {
                                    SetRoomDoors(y + 1, x, 0, 2, 3, Direction.Top);
                                }
                            } 
                        }
                }
            }

            for (int i = 0; i < mapWidth; i++) 
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    if (rooms[i,j]!= null && rooms[i, j].doors.Count > 0)
                    {
                        if (rooms[i, j].doors.Any(x => x.Dir == Direction.Top) && rooms[i-1, j] == null)
                        {
                            rooms[i, j].doors.RemoveAll(x => x.Dir == Direction.Top);
                        }
                        if (rooms[i, j].doors.Any(x => x.Dir == Direction.Bottom) && i+1 != mapWidth && rooms[i+1,j] == null)
                        {
                            rooms[i, j].doors.RemoveAll(x => x.Dir == Direction.Bottom);
                        }
                        if (rooms[i, j].doors.Any(x => x.Dir == Direction.Left) && rooms[i, j-1] == null)
                        {
                            rooms[i, j].doors.RemoveAll(x => x.Dir == Direction.Left);
                        }
                        if (rooms[i, j].doors.Any(x => x.Dir == Direction.Right) && j+1 != mapHeight && rooms[i, j+1] == null)
                        {
                            rooms[i, j].doors.RemoveAll(x => x.Dir == Direction.Right);
                        }
                        rooms[i, j].UpdateState();
                        rooms[i, j].doors = rooms[i, j].doors.Distinct(new DoorTypeComparer()).ToList();
                    }
            
                }
            }
        }

        private static void SetRoomDoors(int i, int j, int firstDir, int secondDir, int thirdDir, Direction direction) 
        {
            numberOfDoors = rnd.Next(1, 3); //3
            doorsCount -= numberOfDoors;
            int currentRooms = rooms.Cast<Room>().Where(x => x!= null && x.doors.Count!=0).Count();

            if (j <= 0 || i <= 0 || j >= mapWidth - 1 || i >= mapHeight) return;

            if (mapWidth > j && mapHeight > i 
                && 0 < i && 0 < j)
            {
                if (rooms[i, j] == null)
                {
                    rooms[i, j] = new Room(new Vector2(i, j), 2, rnd.Next(1, 4));
                }
                rooms[i, j].doors.Add(new Door(direction));
                if(currentRooms < 10)
                {
                    for (int t = 0; t < numberOfDoors; t++)
                    {
                        dir = (Direction)new int[3] { firstDir, secondDir, thirdDir }[rnd.Next(0, 3)]; 
                        if (!rooms[i, j].doors.Contains(new Door(dir)))
                            rooms[i, j].doors.Add(new Door(dir));  
                    }
                }               
                rooms[i, j].UpdateState();
            }
        }
    }
}
