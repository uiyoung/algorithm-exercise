using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ExercisePriorityQueue
{
    class PriorityQueue<T> where T : IComparable<T>
    {
        List<T> _heap = new List<T>();

        public void Push(T data)
        {
            // 힙의 맨 끝에 데이터를 삽입한다.
            _heap.Add(data);

            int now = _heap.Count - 1;

            // 도장깨기를 시작
            while (now > 0)  // 맨위(root)를 차지할 때 까지
            {
                // 도장깨기를 시도
                int next = (now - 1) / 2;   // 부모인덱스 구하는 공식

                if (_heap[now].CompareTo(_heap[next]) < 0)
                    break;  // 실패

                // 두 값을 교체한다.
                T temp = _heap[now];
                _heap[now] = _heap[next];
                _heap[next] = temp;

                // 검사위치를 이동한다
                now = next;
            }
        }

        // 제일 큰 값을 내뱉어 준다.
        public T Pop()
        {
            // 반환할 데이터를 따로 저장
            T ret = _heap[0];

            // 마지막 데이터를 루트로 이동한다
            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex];
            _heap.RemoveAt(lastIndex);
            lastIndex--;

            // 역으로 내려가는 도장깨기를 시작
            int now = 0;
            while (true)
            {
                int left = 2 * now + 1;     // 왼쪽 자식 인덱스 구하는 공식
                int right = 2 + now + 2;    // 오른쪽 자식 인덱스 구하는 공식

                int next = now;
                // 왼쪽값이 현재값보다 크면 왼쪽으로 이동
                if (left <= lastIndex && _heap[next].CompareTo(_heap[left]) < 0)
                    next = left;
                // 오른쪽값이 현재값(위의 왼쪽이동 포함)보다 크면 오른쪽으로 이동
                if (right <= lastIndex && _heap[next].CompareTo(_heap[right]) < 0)
                    next = right;

                // 왼쪽 오른쪽 모두 현재값보다 작으면 종료
                if (next == now)    // 위 if문에 걸리지 않았기 때문
                    break;

                // 두값을 교체
                T temp = _heap[now];
                _heap[now] = _heap[next];
                _heap[next] = temp;

                // 검사 위치를 이동
                now = next;

            }
            return ret;
        }

        public int Count { get { return _heap.Count; }  }
    }

    class Knight : IComparable<Knight>
    {
        public int Id { get; set; }

        public int CompareTo(Knight other)
        {
            if (Id == other.Id)
                return 0;

            return Id > other.Id ? 1 : -1;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PriorityQueue<Knight> q = new PriorityQueue<Knight>();
            q.Push(new Knight { Id = 30 });
            q.Push(new Knight { Id = 20 });
            q.Push(new Knight { Id = 10 });
            q.Push(new Knight { Id = 32 });
            q.Push(new Knight { Id = 12 });

            while (q.Count > 0)
                Console.WriteLine(q.Pop().Id);
        }
    }
}
