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
    }

    // Контролер координат
    class CoordinateHelper
    {
        // Перевірити чи це стіна
        public static bool IsWall(Coordinate pos)
        {
            return false;
        }

        // Перевірити чи це змія натрапила на себе
        public static bool IsSnake(Coordinate pos, ref List<Coordinate> coordinates)
        {
            return false;
        }

        // Перевірити чи це їжа
        public static bool IsFood(Coordinate pos, Coordinate food)
        {
            return false;
        }
    }

    // Контролер відображення
    class ViewController
    {
        private int screenHeight { get; set; }
        private int screenWidth { get; set; }
        public char bodySnake { get; set; }     // Як відображається тіло змійки
        public char wall { get; set; }          // Як відображається стіна
        public char food { get; set; }          // Як відображається їжа
        public char nothing { get; set; }       // Як відображається пусте місце
     
        public ViewController()
        {
            //Встановлюємо розміри екрану
            screenHeight = 20;
            screenWidth = 50;

            Console.WindowHeight = screenHeight;
            Console.WindowWidth = screenWidth;
            Console.SetWindowSize(screenWidth, screenHeight);
            Console.SetBufferSize(screenWidth, screenHeight);
            Console.CursorVisible = false;

            bodySnake = '*';
            wall = '#';
            food = '@';
            nothing = ' ';
           
        }

        public void PlayingField()
        {
            Console.WriteLine("Reload map");
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
        List<Coordinate> coordinates;

        public SnakeController()
        {
            coordinates = new List<Coordinate>();
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

        public bool Up()
        {
            return true;
        }

        public bool Down()
        {
            return true;
        }

        public bool Left()
        {
            return true;
        }

        public bool Right()
        {
            return true;
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
        }

        // Паралельно запускаємо паралельні цикли на переміщення і зчитування клавіш
        public void Play()
        {
            play = true;
            timeout = 1000;
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
