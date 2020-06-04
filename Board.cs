using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace Algorithm
{
    class Board
    {
        private const char CIRCLE = '\u25cf';

        public TileType[,] Tile { get; private set; }
        public int Size { get; private set; }

        private Player _player;

        public enum TileType
        {
            Empty,
            Wall,
        }

        public void Initialize(int size, Player player)
        {
            // 벽으로 둘러쌓여있다는 가정을 해야되기 때문에 사이즈가 반드시 홀수여야 한다.
            if (size % 2 == 0)
                return;


            Size = size;
            _player = player;
            Tile = new TileType[size, size];

            // Generate Maze(from Mazes for Programmers)
            //GererateByBinaryTree();
            GenerateBySideWinder();
        }

        public void Render()
        {
            ConsoleColor prevColor = Console.ForegroundColor;

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (y == _player.PosY && x == _player.PosX)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else
                        Console.ForegroundColor = GetTileColor(Tile[y, x]);
                    
                    Console.Write(CIRCLE);
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = prevColor;
        }

        private ConsoleColor GetTileColor(TileType type)
        {
            switch (type)
            {
                case TileType.Empty:
                    return ConsoleColor.Green;
                case TileType.Wall:
                    return ConsoleColor.Red;
                default:
                    return ConsoleColor.Green;
            }
        }

        private void GererateByBinaryTree()
        {
            // 일단 길을 다 막아버리는 작업
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        Tile[y, x] = TileType.Wall;
                    else
                        Tile[y, x] = TileType.Empty;
                }
            }

            // 랜덤으로 우측 혹은 아래로 길을 뚫는 작업
            Random rand = new Random();
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    // 벽인 경우
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;

                    // 마지막 지점에서는 길을 뚫지 않는다.
                    if (x == Size - 2 && y == Size - 2)
                        continue;

                    // 마지막 y길은 벽으로 막혀있으므로 오른쪽으로만 뚫을 수 있다.
                    if (y == Size - 2)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue;
                    }

                    // 마지막 x길은 벽으로 막혀있으므로 아래로만 뚫을 수 있다.
                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue;
                    }

                    // 50%확률로 우측 혹은 아래로 길 뚫기
                    if (rand.Next(0, 2) == 0)
                        Tile[y, x + 1] = TileType.Empty;
                    else
                        Tile[y + 1, x] = TileType.Empty;
                }
            }
        }

        private void GenerateBySideWinder()
        {
            // 일단 길을 다 막아버리는 작업
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        Tile[y, x] = TileType.Wall;
                    else
                        Tile[y, x] = TileType.Empty;
                }
            }

            // 길을 뚫는 작업
            Random rand = new Random();
            for (int y = 0; y < Size; y++)
            {
                int count = 1;  // 연속으로 오른쪽으로 몇개의 길을 뚫었는지를 카운트
                for (int x = 0; x < Size; x++)
                {
                    // 벽인 경우
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;

                    // 마지막 지점에서는 길을 뚫지 않는다.
                    if (x == Size - 2 && y == Size - 2)
                        continue;

                    // 마지막 y길은 벽으로 막혀있으므로 오른쪽으로만 뚫을 수 있다.
                    if (y == Size - 2)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue;
                    }

                    // 마지막 x길은 벽으로 막혀있으므로 아래로만 뚫을 수 있다.
                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue;
                    }

                    // 50%확률로 우측으로 길 뚫기
                    if (rand.Next(0, 2) == 0)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        count++;
                    }
                    else
                    {
                        // 오른쪽으로 연속으로 뚫어진 길중 하나를 랜덤으로 골라 아래로 길을 뚫는다
                        int randomIndex = rand.Next(0, count);
                        Tile[y + 1, x - randomIndex *2] = TileType.Empty;  // x좌표마다 벽으로 이어져있기 때문에 * 2
                        // 2를 곱해준 이유는 원래 기본 맵이 [공간] [벽] [공간] [벽] 으로 이루어져 있었으니,특정[공간]에서 이전[공간]으로 가기 위해선 x좌표를 2씩 후진해야 하기 때문입니다.
                        count = 1;  // 카운트 초기화
                    }
                }
            }
        }
    }
}
