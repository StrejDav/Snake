using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Snake
{
    class Snake
    {
        List<Vector2> snakeParts = new List<Vector2>();
        List<Vector2> bombs = new List<Vector2>();
        Vector2 fruit;
        int difficulty = 1;

        public int GetLength()
        {
            return snakeParts.Count;
        }

        public Snake(int diff)
        {
            snakeParts.Add(new Vector2(Console.WindowWidth / 2, Console.WindowHeight / 2));
            snakeParts.Add(new Vector2(Console.WindowWidth / 2 + 1, Console.WindowHeight / 2));
            snakeParts.Add(new Vector2(Console.WindowWidth / 2 + 2, Console.WindowHeight / 2));


            Console.ForegroundColor = ConsoleColor.Green;
            foreach (Vector2 snakePart in snakeParts) // initial draw of the snake parts
            {
                Console.SetCursorPosition((int)snakePart.X, (int)snakePart.Y);
                Console.Write('═');
            }

            Console.SetCursorPosition((int)snakeParts[^1].X, (int)snakeParts[^1].Y);
            Console.Write(' ');

            difficulty = diff;


            GenerateFood(); // initial generation of food
        }
        
        public void Render()
        {
            AteFood(); // checks if snake ate food; generates new food and bomb if it did
            Console.ForegroundColor = ConsoleColor.Green; 
            Console.SetCursorPosition((int)snakeParts[^1].X, (int)snakeParts[^1].Y); // deletes last part of snake
            Console.Write(' ');
            Console.SetCursorPosition((int)snakeParts[0].X, (int)snakeParts[0].Y); // draw first part of snake
            if (snakeParts[0].Y - snakeParts[1].Y == -1 & snakeParts[2].X - snakeParts[1].X == 1)
            {
                Console.Write("║");
                Console.SetCursorPosition((int)snakeParts[1].X, (int)snakeParts[1].Y);
                Console.Write("╚");
            }
            else if (snakeParts[0].Y - snakeParts[1].Y == -1 & snakeParts[2].X - snakeParts[1].X == -1)
            {
                Console.Write("║");
                Console.SetCursorPosition((int)snakeParts[1].X, (int)snakeParts[1].Y);
                Console.Write("╝");
            }
            else if (snakeParts[0].Y - snakeParts[1].Y == 1 & snakeParts[2].X - snakeParts[1].X == 1)
            {
                Console.Write("║");
                Console.SetCursorPosition((int)snakeParts[1].X, (int)snakeParts[1].Y);
                Console.Write("╔");
            }
            else if (snakeParts[0].Y - snakeParts[1].Y == 1 & snakeParts[2].X - snakeParts[1].X == -1)
            {
                Console.Write("║");
                Console.SetCursorPosition((int)snakeParts[1].X, (int)snakeParts[1].Y);
                Console.Write("╗");
            }
            else if (snakeParts[0].X - snakeParts[1].X == -1 & snakeParts[2].Y - snakeParts[1].Y == 1)
            {
                Console.Write("═");
                Console.SetCursorPosition((int)snakeParts[1].X, (int)snakeParts[1].Y);
                Console.Write("╗");
            }
            else if (snakeParts[0].X - snakeParts[1].X == 1 & snakeParts[2].Y - snakeParts[1].Y == 1)
            {
                Console.Write("═");
                Console.SetCursorPosition((int)snakeParts[1].X, (int)snakeParts[1].Y);
                Console.Write("╔");
            }
            else if (snakeParts[0].X - snakeParts[1].X == -1 & snakeParts[2].Y - snakeParts[1].Y == -1)
            {
                Console.Write("═");
                Console.SetCursorPosition((int)snakeParts[1].X, (int)snakeParts[1].Y);
                Console.Write("╝");
            }
            else if (snakeParts[0].X - snakeParts[1].X == 1 & snakeParts[2].Y - snakeParts[1].Y == -1)
            {
                Console.Write("═");
                Console.SetCursorPosition((int)snakeParts[1].X, (int)snakeParts[1].Y);
                Console.Write("╚");
            }
            else if (snakeParts[0].X - snakeParts[1].X == 0 & snakeParts[0].Y - snakeParts[1].Y != 0)
            {
                Console.Write("║");
            }
            else if (snakeParts[0].X - snakeParts[1].X != 0 & snakeParts[0].Y - snakeParts[1].Y == 0)
            {
                Console.Write("═");
            }
        }

        public void Move()
        {
            snakeParts.Insert(0, snakeParts[0] + (snakeParts[0] - snakeParts[1]));
            snakeParts.RemoveAt(snakeParts.Count - 1);
        }

        public void Move(Vector2 dir)
        {
            Vector2 firstPart = snakeParts[0] + dir;
            if ((firstPart != snakeParts[1]) & (firstPart != snakeParts[0] + (snakeParts[0] - snakeParts[1])))
            {
                snakeParts.Insert(0, firstPart);
            }
            else
            {
                snakeParts.Insert(0, snakeParts[0] + (snakeParts[0] - snakeParts[1]));
            }
            snakeParts.RemoveAt(snakeParts.Count - 1);
        }

        public void AddPart()
        {
            Vector2 diff = snakeParts[^1] - snakeParts[^2];
            Vector2 lastPart = snakeParts[^1];
            snakeParts.Add(lastPart + diff);
        }

        public bool Collision()
        {
            if (snakeParts[0].X == -1 | snakeParts[0].Y == -1 | snakeParts[0].X == Console.WindowWidth | snakeParts[0].Y == Console.WindowHeight | bombs.Contains(snakeParts[0]))
                return true;
            for(int i = 1; i < snakeParts.Count; i++)
                if (snakeParts[0] == snakeParts[i])
                    return true;
            return false;
        }

        private void AteFood()
        {
            if (snakeParts[0] == fruit)
            {
                AddPart();
                GenerateBomb();
                GenerateFood();
            }
        }

        private void GenerateFood()
        {
            Random rnd = new Random();
            fruit = new Vector2(rnd.Next(0, Console.WindowWidth), rnd.Next(0, Console.WindowHeight));
            while (snakeParts.Contains(fruit) | bombs.Contains(fruit))
                fruit = new Vector2(rnd.Next(0, Console.WindowWidth), rnd.Next(0, Console.WindowHeight));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition((int)fruit.X, (int)fruit.Y);
            Console.Write("♂");
        }
       
        
        private void GenerateBomb()
        {
            Random rnd = new Random();
            for (int i = 1; i <= difficulty; i++)
            {
                Vector2 bomb = new Vector2(rnd.Next(0, Console.WindowWidth), rnd.Next(0, Console.WindowHeight));
                while (snakeParts.Contains(bomb) | VectorDifferenceLength(snakeParts[0], bomb)! < 2)
                    bomb = new Vector2(rnd.Next(0, Console.WindowWidth), rnd.Next(0, Console.WindowHeight));
                bombs.Add(bomb);

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition((int)bomb.X, (int)bomb.Y);
                Console.Write("♀");
            }
        }

        public double GetDT()
        {
            double x = 1 / (double)snakeParts.Count;
            if (snakeParts[0] - snakeParts[1] == new Vector2(0, 1) | snakeParts[0] - snakeParts[1] == new Vector2(0, -1))
                return 1.5 * x;
            return x;
        }

        private double VectorDifferenceLength(Vector2 vector1, Vector2 vector2)
        {
            return Math.Sqrt((vector1.X - vector2.X) * (vector1.X - vector2.X) + (vector1.Y - vector2.Y) * (vector1.Y - vector2.Y));

        }
    }
}