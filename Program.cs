using System;
using System.Threading;

namespace Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            const int WAIT_TICK = 1000 / 30;
            int lastTick = 0;

            Board board = new Board();
            board.Initialize(25);

            while (true)
            {
                #region 프레임 관리
                // 만약 경과한시간이 1/30초보다 작다면
                if (System.Environment.TickCount - lastTick < WAIT_TICK)
                    continue;

                lastTick = System.Environment.TickCount;
                #endregion

                // input

                // logic

                // render
                Console.SetCursorPosition(0, 0);
                board.Render();
            }
        }
    }
}
