using System;
using System.Text;
using System.Threading;

namespace Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;

            Board board = new Board();
            Player player = new Player();
            board.Initialize(25, player);
            player.Initialize(1, 1, board);

            const int WAIT_TICK = 1000 / 30;
            int lastTick = 0;

            while (true)
            {
                #region 프레임 관리
                // 만약 경과한시간이 1/30초보다 작다면
                int currentTick = System.Environment.TickCount;
                if (currentTick - lastTick < WAIT_TICK)
                    continue;

                int deltaTick = currentTick - lastTick;
                lastTick = currentTick;
                #endregion

                // input

                // logic
                player.Update(deltaTick);

                // render
                Console.SetCursorPosition(0, 0);
                board.Render();
            }
        }
    }
}
