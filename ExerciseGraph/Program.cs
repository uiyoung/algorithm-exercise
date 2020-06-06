using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Exercise
{
    class Graph
    {
        int[,] adjacent1 = new int[6, 6]
        {
            {0, 1, 0, 1, 0, 0},
            {1, 0, 1, 1, 0, 0},
            {0, 1, 0, 0, 0, 0},
            {1, 1, 0, 0, 1, 0},
            {0, 0, 0, 1, 0, 1},
            {0, 0, 0, 0, 1, 0},
        };

        List<int>[] adjacent2 = new List<int>[]
        {
            new List<int>() { 1, 3 },
            new List<int>() { 0, 2, 3 },
            new List<int>() { 1 },
            new List<int>() { 0, 1, 4 },
            new List<int>() { 3, 5 },
            new List<int>() { 4 }
        };

        // 가중치 그래프
        int[,] adjacent3 = new int[6, 6]
        {
            {-1, 15, -1, 35, -1, -1},
            {15, -1, 05, 10, -1, -1},
            {-1, 05, -1, -1, -1, -1},
            {35, 10, -1, -1, 05, -1},
            {-1, -1, -1, 05, -1, 05},
            {-1, -1, -1, -1, 05, -1}
        };

        bool[] visited = new bool[6];

        // 1) 우선 now부터 방문하고
        // 2) now와 연결된 정점들을 하나씩 확인해서 아직 미발견(미방문) 상태라면 방문한다.
        // 그래프를 행렬로 표기한경우
        public void DFS(int now)
        {
            Console.WriteLine(now);
            visited[now] = true;    // 1) 우선 now부터 방문

            for (int next = 0; next < 6; next++)
            {
                if (adjacent1[now, next] == 0)  // 연결되어 있지 않으면 skip
                    continue;
                if (visited[next])  // 이미 방문했으면 skip
                    continue;

                DFS(next); //재귀
            }
        }

        // 그래프를 리스트로 표기한경우
        public void DFS2(int now)
        {
            Console.WriteLine(now);
            visited[now] = true;    // 1) 우선 now부터 방문

            foreach (var next in adjacent2[now])
            {
                if (visited[next])   // 이미 방문했으면 skip
                    continue;

                DFS2(next);
            }

        }

        public void SearchAll()
        {
            visited = new bool[6];  // visited 초기화

            // 간선이 끊어져 있는 경우 끊어져있는 부분부터 DFS 다시 시작
            for (int now = 0; now < 6; now++)
            {
                if (!visited[now])
                    DFS(now);
            }
        }

        public void BFS(int start)
        {
            bool[] found = new bool[6];
            int[] parent = new int[6];
            int[] distance = new int[6];

            Queue<int> q = new Queue<int>();    // 예약 목록(먼저 발견한 지점부터 탐색하기위해)
            q.Enqueue(start);
            found[start] = true;    // 우선 now부터 발견
            parent[start] = start;
            distance[start] = 0;

            while (q.Count > 0)
            {
                int now = q.Dequeue();
                Console.WriteLine(now);

                for (int next = 0; next < 6; next++)
                {
                    if (adjacent1[now, next] == 0)  //  인접하지 않았으면 스킵
                        continue;
                    if (found[next])    //이미 발견한 애라면 스킵
                        continue;

                    q.Enqueue(next);
                    found[next] = true;
                    parent[next] = now;
                    distance[next] = distance[now] + 1;
                }
            }
        }

        public void Dijikstra(int start)
        {
            bool[] visited = new bool[6];
            int[] distance = new int[6];   // 최단거리를 기록
            int[] parent = new int[6];  // 부모 정점을 기록

            // distance가 0으로 초기화되어있으면 초기값이라서 0인지 방문을 못해서 0인지 모르므로 맥스값으로 초기화
            Array.Fill(distance, Int32.MaxValue);

            distance[start] = 0;
            parent[start] = start;

            while (true)
            {
                // 가장 유력한 후보의 거리와 번호를 저장
                // 비현실적 값을 넣어서 후보를 찾았는지 못찾았는지 여부를 closest와 now변하지않았는지로 체크할 수 있다.
                int closest = Int32.MaxValue;
                int now = -1;

                // 제일 좋은 후보를 찾는다 (가장 가까이에 있는)
                for (int i = 0; i < 6; i++)
                {
                    // 이미 방문한 정점은 스킵
                    if (visited[i])
                        continue;
                    // 아직 발견(예약)된 적이 없거나, 기존 후보보다 멀리 있으면 스킵
                    if (distance[i] == Int32.MaxValue || distance[i] >= closest)
                        continue;

                    // 여기까지 왔다면 여태껏 발견한 가장 좋은 후보라는 의미므로 정보를 갱신
                    closest = distance[i];
                    now = i;
                }

                // 여기까지 왔는데 now가 -1이라면 다음 후보가 하나도 없다는 의미(모든점을 다 찾았거나 연결이 단절되어 후보가없다는 의미)
                if (now == -1)
                    break;


                // 제일 좋은 후보를 찾았으니까 방문한다
                visited[now] = true;

                // 방문한 정점과 인접한 정점들을 조사해서
                // 상황에따라 발견한 최단거리를 갱신한다.
                for (int next = 0; next < 6; next++)
                {
                    // 연결되지 않은 정점 스킵
                    if (adjacent3[now, next] == -1)
                        continue;
                    // 이미 방문한 정점 스킵
                    if (visited[next])
                        continue;

                    // 새로 조사된 정점의 최단거리를 계산한다
                    int nextDist = distance[now] + adjacent3[now, next];
                    // 만약 기존에 발견한 최단거리가 새로 조사된 최단거리보다 크면 정보를 갱신
                    if (nextDist < distance[next])
                    {
                        distance[next] = nextDist;
                        parent[next] = now;
                    }
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph();
            //graph.SearchAll();
            //graph.BFS(0);
            graph.Dijikstra(0);
        }
    }
}
