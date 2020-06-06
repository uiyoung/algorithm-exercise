using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithm
{
    class Position
    {
        public int PosY;
        public int PosX;

        public Position(int posY, int posX)
        {
            PosY = posY;
            PosX = posX;
        }
    }

    class Player
    {
        public int PosY { get; private set; }
        public int PosX { get; private set; }
        private Board _board;
        private Random _random = new Random();

        private enum Direction // 반시계 방향
        {
            Up,
            Left,
            Down,
            Right
        }

        private int _dir = (int)Direction.Up;

        // 현재 바라보고 있는 방향 기준으로 좌표변화를 나타낸다
        int[] frontY = new int[] { -1, 0, 1, 0 };
        int[] frontX = new int[] { 0, -1, 0, 1 };
        int[] rightY = new int[] { 0, -1, 0, 1 };
        int[] rightX = new int[] { 1, 0, -1, 0 };

        // 이동한 경로를 저장할 리스트
        private List<Position> _points = new List<Position>();

        public void Initialize(int posY, int posX, Board board)
        {
            PosY = posY;
            PosX = posX;
            _board = board;

            // 초기위치 저장
            _points.Add(new Position(PosY, PosX));

            // 목적지에 도착하기 전까지 계속 실행
            while (PosY != _board.DestY || PosX != _board.DestX)
            {
                // 1. 현재 바라보는 방향 기준으로 오른쪽방향으로 갈 수 있는지 확인 
                if (_board.Tile[PosY + rightY[_dir], PosX + rightX[_dir]] == Board.TileType.Empty)
                {
                    // 오른쪽방향으로 90도 회전
                    _dir = (_dir - 1 + 4) % 4;

                    // 앞으로 한칸 전진
                    PosY = PosY + frontY[_dir];
                    PosX = PosX + frontX[_dir];

                    // 전진했으면 저장
                    _points.Add(new Position(PosY, PosX));
                }
                // 2. 현재 바라보는 방향을 기준으로 전진 할 수 있는지 확인
                else if (_board.Tile[PosY + frontY[_dir], PosX + frontX[_dir]] == Board.TileType.Empty)
                {
                    // 앞으로 한칸 전진
                    PosY = PosY + frontY[_dir];
                    PosX = PosX + frontX[_dir];

                    // 전진했으면 저장
                    _points.Add(new Position(PosY, PosX));
                }
                // 오른쪽, 앞 모두 막혀있어서 전진 할 수 없으면
                else
                {
                    // 왼쪽방향으로 90도 회전
                    _dir = (_dir + 1) % 4;
                }
            }
        }

        private const int MOVE_TICK = 100;  // 0.1초
        private int _sumTick = 0;
        private int _lastIndex = 0;
        public void Update(int deltaTick)
        {
            if (_lastIndex >= _points.Count)
                return;

            _sumTick += deltaTick;
            if (_sumTick >= MOVE_TICK)
            {
                _sumTick = 0; // 초기화

                // 0.1초마다 실행될 로직
                PosY = _points[_lastIndex].PosY;
                PosX = _points[_lastIndex].PosX;
                _lastIndex++;
            }
        }
    }
}

