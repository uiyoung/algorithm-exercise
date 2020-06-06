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
    }

    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph();
            //graph.SearchAll();
            graph.BFS(0);
        }
    }
}
