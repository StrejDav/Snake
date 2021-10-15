using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Snake
{
    class Program
    {
        const int MF_BYCOMMAND = 0x00000000;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;
        const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        static void Main(string[] args)
        {
            Console.WindowHeight = 10;
            Console.WindowWidth = 20;

            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);

            Console.CursorVisible = false;
            ConsoleKey playAgain;

            Console.WriteLine("Zadej obtížnost: ");

            bool cont = false;
            int difficulty = 1;

            while (!cont)
            {
                try
                {
                    difficulty = Int16.Parse(Console.ReadLine());
                    cont = true;
                }
                catch
                {
                    Console.WriteLine("Nezadali jste číslo. Zadejte číslo: ");
                    cont = false;
                }
            }
            int dt = 200;

            do
            {

                Console.Clear();
                Snake snake = new Snake(difficulty);
                while (true)
                {
                    //dt = (int)(1000 * snake.GetDT());
                    DateTime beginWait = DateTime.Now;
                    while (!Console.KeyAvailable && DateTime.Now.Subtract(beginWait).TotalMilliseconds < dt) // waits for keypress
                        Thread.Sleep(1);

                    if (Console.KeyAvailable) // if key pressed
                    {
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.UpArrow:
                                snake.Move(new System.Numerics.Vector2(0, -1));
                                break;
                            case ConsoleKey.DownArrow:
                                snake.Move(new System.Numerics.Vector2(0, 1));
                                break;
                            case ConsoleKey.LeftArrow:
                                snake.Move(new System.Numerics.Vector2(-1, 0));
                                break;
                            case ConsoleKey.RightArrow:
                                snake.Move(new System.Numerics.Vector2(1, 0));
                                break;
                        }
                    }

                    else snake.Move();

                    if (snake.Collision())
                        break;

                    while ((DateTime.Now - beginWait).TotalMilliseconds < dt) // wait until the full 500 ms expire
                        Thread.Sleep(1);

                    snake.Render();
                }

                Console.Clear();
                Console.WriteLine("Game over!");
                Console.WriteLine("Your score was: {0}", snake.GetLength() - 2);
                Console.WriteLine("Press any key to play again\nPress ESC to exit");
                Thread.Sleep(1000);
                playAgain = Console.ReadKey().Key;

            } while (playAgain != ConsoleKey.Escape);
        }
    }
}