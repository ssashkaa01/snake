using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake
{
    // Напрямки руху
    enum GO { UP, DOWN, LEFT, RIGHT };

    // Структура координати
    struct Coordinate
    {
        public int x { get; set; } // X координата
        public int y { get; set; } // Y координата

        public override string ToString()
        {
            return $"X: {x}, Y: {y}";
        }
    }

    // Контролер координат
    class CoordinateHelper
    {
        public static int maxHeight = 15;
        public static int maxWidth = 15;

        // Перевірити чи це стіна
        public static bool IsNotWall(Coordinate pos)
        {
            return (pos.x < maxWidth && pos.y < maxHeight && pos.y > 0 && pos.x > 0);
        }

        // Перевірити чи це змія натрапила на себе
        public static bool IsSnake(Coordinate pos, ref List<Coordinate> coordinates)
        {
            foreach(var item in coordinates)
            {
                if (item.x == pos.x && item.y == pos.y) return true;
            }

            return false;
        }

        // Перевірити чи це змія натрапила на себе
        public static bool IsSnake(int x, int y, ref List<Coordinate> coordinates)
        {
            foreach (var item in coordinates)
            {
                if (item.x == x && item.y == y) return true;
            }

            return false;
        }

        // Перевірити чи це їжа
        public static bool IsFood(Coordinate pos, Coordinate food)
        {
            return (pos.x == food.x && pos.y == food.y);
        }

        // Перевірити чи це їжа
        public static bool IsFood(int x, int y, Coordinate food)
        {
            return (x == food.x && y == food.y);
        }
    }

    // Контролер відображення
    class ViewController
    {
        private int screenHeight { get; set; }
        private int screenWidth { get; set; }
        public string bodySnake { get; set; }     // Як відображається тіло змійки
        public string wall { get; set; }          // Як відображається стіна
        public string food { get; set; }          // Як відображається їжа
        public string nothing { get; set; }       // Як відображається пусте місце
        public Coordinate foodCoordinate;
        public List<Coordinate> snakeCoordinate;

        public ViewController()
        {
            //Встановлюємо розміри екрану
            screenHeight = 25;
            screenWidth = 60;

            Console.WindowHeight = screenHeight;
            Console.WindowWidth = screenWidth;
            Console.SetWindowSize(screenWidth, screenHeight);
            Console.SetBufferSize(screenWidth, screenHeight);
            Console.CursorVisible = false;

            bodySnake = "* ";
            wall = "# ";
            food = "@ ";
            nothing = "  ";
           
        }

        public void PlayingField()
        {

            Console.Clear();

            for(int y = 0; y <= CoordinateHelper.maxHeight; y++)
            {

                for (int x = 0; x <= CoordinateHelper.maxWidth; x++)
                {
                    if (x == 0 || y == 0 || x == CoordinateHelper.maxWidth || y == CoordinateHelper.maxHeight)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;

                        Console.Write(wall);

                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        
                        if(CoordinateHelper.IsFood(x, y, foodCoordinate))
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;

                            Console.Write(food);

                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if(CoordinateHelper.IsSnake(x, y, ref snakeCoordinate))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;

                            Console.Write(bodySnake);

                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.Write(nothing);
                        }
                    }

                    
                }
                Console.WriteLine();
            }
        }
        
        public void GameOver()
        {
            Console.Clear();
            Console.WriteLine("Game Over!");
        }
    }

    // Контролер змії
    class SnakeController
    {

        public List<Coordinate> snake;
        public Coordinate food;

        public SnakeController()
        {
            snake = new List<Coordinate>()
            {
                new Coordinate(){ x = 10, y = 10 },
                new Coordinate(){ x = 9, y = 10 },
                new Coordinate(){ x = 8, y = 10 },
            };

            food = new Coordinate()
            {
                x = 4,
                y = 4
            };
        }

        public GO move { get; set; }

        // Переміщення змійки
        public bool Moving()
        {

            if(move == GO.UP)
            {
                return Up();
            }
            else if(move == GO.DOWN)
            {
                return Down();
            }
            else if (move == GO.LEFT)
            {
                return Left();
            }
            else if (move == GO.RIGHT)
            {
                return Right();
            }

            return false;
        }

        private void GenerateFood()
        {
            Random rnd = new Random();

            do
            {
                food.x = rnd.Next(0, CoordinateHelper.maxWidth);
                food.y = rnd.Next(0, CoordinateHelper.maxHeight);

            } while (CoordinateHelper.IsSnake(food, ref snake) || !CoordinateHelper.IsNotWall(food));
        }

        private bool SetNewCoordinate(Coordinate coordinate)
        {
           
            if (!CoordinateHelper.IsNotWall(coordinate)) return false;

            if (CoordinateHelper.IsSnake(coordinate, ref snake)) return false;

            if (CoordinateHelper.IsFood(coordinate, food))
            {
                snake.Insert(0, food);
                GenerateFood();
                Moving();
            }
            else
            {
                snake.Insert(0, coordinate);
                snake.RemoveAt(snake.Count - 1);
            }

            return true;
        }

        public bool Up()
        {
            Coordinate newcoordinate = new Coordinate()
            {
                x = snake.First().x,
                y = snake.First().y - 1,
            };

            return SetNewCoordinate(newcoordinate);
        }

        public bool Down()
        {
            Coordinate newcoordinate = new Coordinate()
            {
                x = snake.First().x,
                y = snake.First().y + 1,
            };
            return SetNewCoordinate(newcoordinate);
        }

        public bool Left()
        {
            Coordinate newcoordinate = new Coordinate()
            {
                x = snake.First().x - 1,
                y = snake.First().y,
            };

            return SetNewCoordinate(newcoordinate);
        }

        public bool Right()
        {
            Coordinate newcoordinate = new Coordinate()
            {
                x = snake.First().x + 1,
                y = snake.First().y,
            };
            return SetNewCoordinate(newcoordinate);
        }
    }

    class Game
    { 
        private SnakeController snakeController;
        private ViewController viewController;
        private Stopwatch stopWatch;

        private int timeout { get; set; }
        private bool play { get; set; }

        public Game()
        {
            snakeController = new SnakeController();
            viewController = new ViewController();
            stopWatch = new Stopwatch();
            UpdatePostion();
        }

        public void UpdatePostion()
        {
            viewController.snakeCoordinate = snakeController.snake;
            viewController.foodCoordinate = snakeController.food;
        }

        // Паралельно запускаємо паралельні цикли на переміщення і зчитування клавіш
        public void Play()
        {
            play = true;
            timeout = 700;
            stopWatch.Start();

            Parallel.Invoke(ReadKey, Moving);

            viewController.GameOver();
        }

        // Зчитуємо клавіши 
        public void ReadKey()
        {
            ConsoleKeyInfo action;
            do
            {
                action = Console.ReadKey();

                try
                {
                    snakeController.move = GetMoving(Convert.ToChar(action.Key));

                    // Виконуємо переміщення
                    if (snakeController.Moving())
                    {
                        UpdatePostion();

                        // Перемальовуємо поле
                        viewController.PlayingField();
                    }
                    // Якщо переміщення не вдалося гравець програв
                    else
                    {
                        play = false;
                    }
                }
                // Перемальовуємо поле
                catch
                {
                    viewController.PlayingField();
                }
            }
            while (play);
        }

        // Отримуємо напрямок відповідно від нажатої клавіши
        private GO GetMoving(char key)
        {
            switch(key)
            {
                case 'W':
                    return GO.UP;
                case 'S':
                    return GO.DOWN;
                case 'A':
                    return GO.LEFT;
                case 'D':
                    return GO.RIGHT;
            }
            throw new Exception("Bad key, please enter next key");
        }

        // Якщо гравець пасивний ми всерівно переміщуємо змійку
        private void Moving()
        {
            while (play)
            {
                if (stopWatch.ElapsedMilliseconds > timeout)
                {
                    stopWatch.Restart();

                    // Виконуємо переміщення
                    if (snakeController.Moving())
                    {
                        UpdatePostion();

                        // Перемальовуємо поле
                        viewController.PlayingField();
                    }
                    // Якщо переміщення не вдалося гравець програв
                    else
                    {
                        play = false;
                    }
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game snake = new Game();
            snake.Play();
        }
    }
}
