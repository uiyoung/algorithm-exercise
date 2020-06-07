using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Algorithm
{
    class Position
    {
        public int Y;
        public int X;

        public Position(int y, int x)
        {
            Y = y;
            X = x;
        }
    }

    class Player
    {
        public int PosY { get; private set; }
        public int PosX { get; private set; }
        private Board _board;

        private enum Direction // 반시계 방향
        {
            Up,
            Left,
            Down,
            Right
        }

        private int _dir = (int)Direction.Up;

        // 이동한 경로를 저장할 리스트
        private List<Position> _points = new List<Position>();

        public void Initialize(int posY, int posX, Board board)
        {
            PosY = posY;
            PosX = posX;
            _board = board;

            //RightHand();  // 오른손법칙 길찾기
            //BFS();        // BFS를 이용한 길찾기
            AStar();        // A* 길찾기
        }

        struct PQNode : IComparable<PQNode>
        {
            public int F;
            public int G;
            public int X;
            public int Y;

            public int CompareTo(PQNode other)
            {
                if (F == other.F)
                    return 0;

                return F < other.F ? 1 : -1;    // F가 작을수록 좋기 때문
            }
        }

        private void AStar()
        {
            // U L D R 4방향 이동만 할 때
            int[] deltaY = new int[] { -1, 0, 1, 0 };
            int[] deltaX = new int[] { 0, -1, 0, 1 };
            int[] cost = new int[] { 10, 10, 10, 10 };  // 방향에따른 이동 비용

            // U L D R UL DL DR UR 대각 선이동 허용시
            //int[] deltaY = new int[] { -1, 0, 1, 0, -1, 1, 1, -1 };
            //int[] deltaX = new int[] { 0, -1, 0, 1, -1, -1, 1, 1 };
            //int[] cost = new int[] { 10, 10, 10, 10, 14, 14, 14, 14 };  // 방향에따른 이동 비용

            // 점수 매기기
            // F = G + H
            // F : 최종점수(작을수록 좋음, 경로에 따라 달라짐)
            // G : 시작점에서 해당 좌표까지 이동하는데 드는 비용(경로에따라 달라짐)
            // H : 목적지에서 얼마나 가까운지 (고정)

            // (y,x) 이미 방문했는지 여부 (방문 = closed 상태)
            bool[,] closed = new bool[_board.Size, _board.Size];

            // (y,x) 가는길을 한번이라도 발견 했는지
            // 발견 X => MaxValue 
            // 발견 O => F값(G+H)
            int[,] open = new int[_board.Size, _board.Size];    // openList
            for (int y = 0; y < _board.Size; y++)
                for (int x = 0; x < _board.Size; x++)
                    open[y, x] = Int32.MaxValue;    // 초기값을 MaxValue로 초기화

            // 부모 노드 기록할 배열
            Position[,] parent = new Position[_board.Size, _board.Size];

            // 오픈리스트에 있는 정보들 중에서 가장 좋은 후보를 빠르게 뽑아오기 위한 도구
            PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();

            // 시작점 발견(예약 진행)
            // F = G+H = 시작점이므로 0 + 시작점에서 목적지까지 거리(절대값 (y2,x2) - (y1,x1)) * 비용 10
            open[PosY, PosX] = 10 * (Math.Abs(_board.DestY - PosY) + Math.Abs(_board.DestX - PosX));
            // 우선순위큐에도 저장
            pq.Push(new PQNode() { F = 10 * (Math.Abs(_board.DestY - PosY) + Math.Abs(_board.DestX - PosX)), G = 0, Y = PosY, X = PosX });
            // 시작점의 부모는 자기자신으로
            parent[PosY, PosX] = new Position(PosY, PosX);

            while (pq.Count > 0)
            {
                // 제일 좋은 후보를 찾는다(Priority Queue사용)
                PQNode node = pq.Pop();

                // 동일한 좌표를 여러 경로로 찾아서, 더 빠른경로로 인해 이미 방문(closed)된 경우 skip
                if (closed[node.Y, node.X])
                    continue;

                // 방문한다
                closed[node.Y, node.X] = true;

                // 목적지 도착했으면 바로 종료
                if (node.Y == _board.DestY && node.X == _board.DestX)
                    break;

                // 상하좌우 등 이동할 수 있는 좌표인지 확인해서 예약(open)한다
                for (int i = 0; i < deltaY.Length; i++)
                {
                    int nextY = node.Y + deltaY[i];
                    int nextX = node.X + deltaX[i];

                    // 유효 범위를 벗어났으면 스킵
                    if (nextY < 0 || nextY >= _board.Size || nextX < 0 || nextX >= _board.Size)
                        continue;
                    // 벽으로 막혀 갈수없으면 스킵
                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;
                    // 이미 방문한곳이면 스킵
                    if (closed[nextY, nextX])
                        continue;

                    // 비용계산
                    int g = node.G + cost[i];
                    int h = 10 * (Math.Abs(_board.DestY - nextY) + Math.Abs(_board.DestX - nextX));

                    //다른경로에서 더 빠른길을 이미 찾았으면 스킵
                    if (open[nextY, nextX] < g + h)
                        continue;

                    // 여기까지 넘어왔다면 지금까지 찾은 길 중 가장좋은 길이므로 예약 진행
                    open[nextY, nextX] = g + h;
                    // 우선순위큐에도 저장
                    pq.Push(new PQNode() { F = g + h, G = g, Y = nextY, X = nextX });
                    // 부모노드 기록
                    parent[nextY, nextX] = new Position(node.Y, node.X);
                }
            }

            CalcPathFromParent(parent);
        }

        private void BFS()
        {
            int[] deltaY = new int[] { -1, 0, 1, 0 };
            int[] deltaX = new int[] { 0, -1, 0, 1 };

            bool[,] found = new bool[_board.Size, _board.Size];
            Position[,] parent = new Position[_board.Size, _board.Size];

            Queue<Position> q = new Queue<Position>();
            q.Enqueue(new Position(PosY, PosX));
            found[PosY, PosX] = true;
            parent[PosY, PosX] = new Position(PosY, PosX);

            while (q.Count > 0)
            {
                Position pos = q.Dequeue();
                int nowY = pos.Y;
                int nowX = pos.X;

                // 상하좌우 4방향을 체크 
                for (int next = 0; next < 4; next++)
                {
                    int nextY = nowY + deltaY[next];
                    int nextX = nowX + deltaX[next];

                    // 유효범위를 벗어났으면 스킵
                    if (nextY < 0 || nextY >= _board.Size || nextX < 0 || nextX >= _board.Size)
                        continue;
                    // 벽으로 막혀있으면 스킵
                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;
                    // 이미 방문한곳이며 스킵
                    if (found[nextY, nextX])
                        continue;

                    q.Enqueue(new Position(nextY, nextX));
                    found[nextY, nextX] = true;
                    parent[nextY, nextX] = new Position(nowY, nowX);
                }
            }

            CalcPathFromParent(parent);
        }

        // 마지막 목적지 좌표부터 거슬러 올라가기
        private void CalcPathFromParent(Position[,] parent)
        {
            int y = _board.DestY;
            int x = _board.DestX;

            // 부모의 좌표값이 자신의 좌표값과 같으면 시작점이므로 그때까지 반복
            while (parent[y, x].Y != y || parent[y, x].X != x)
            {
                _points.Add(new Position(y, x));    // 값 저장
                Position pos = parent[y, x];    // 부모좌표값으로 이동
                y = pos.Y;
                x = pos.X;
            }

            _points.Add(new Position(y, x));    // 시작점 추가
            _points.Reverse();  // 역순
        }

        private void RightHand()
        {
            // 현재 바라보고 있는 방향 기준으로 좌표변화를 나타낸다
            int[] frontY = new int[] { -1, 0, 1, 0 };
            int[] frontX = new int[] { 0, -1, 0, 1 };
            int[] rightY = new int[] { 0, -1, 0, 1 };
            int[] rightX = new int[] { 1, 0, -1, 0 };

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

        private const int MOVE_TICK = 50;  // 0.05초 마다 이동
        private int _sumTick = 0;
        private int _lastIndex = 0;
        public void Update(int deltaTick)
        {
            // 도착했다면
            if (_lastIndex >= _points.Count)
            {
                // 초기화
                _lastIndex = 0;
                _points.Clear();
                _board.Initialize(_board.Size, this);
                Initialize(1, 1, _board);
            }

            _sumTick += deltaTick;
            if (_sumTick >= MOVE_TICK)
            {
                _sumTick = 0; // 초기화

                // 0.1초마다 실행될 로직
                PosY = _points[_lastIndex].Y;
                PosX = _points[_lastIndex].X;
                _lastIndex++;
            }
        }
    }
}

