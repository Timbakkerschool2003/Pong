using System;
using System.Text;
using System.Threading;

class PongGame
{
    private int paddle1Position, paddle2Position;
    private int ballX, ballY;
    private int ballVelocityX, ballVelocityY;
    private const int playfieldWidth = 80;
    private const int playfieldHeight = 24;
    private const int paddleHeight = 4;
    private int player1Score, player2Score;
    private bool gameRunning = true;
    private DateTime lastBallMoveTime;

    public PongGame()
    {
        Console.CursorVisible = false;
        ResetGame();
    }

    private void ResetGame()
    {
        paddle1Position = paddle2Position = playfieldHeight / 2 - paddleHeight / 2;
        player1Score = 0;
        player2Score = 0;
        ResetBall();
        Console.Clear();
    }

    private void ResetBall()
    {
        ballX = playfieldWidth / 2;
        ballY = new Random().Next(1, playfieldHeight - 1);
        ballVelocityX = new Random().Next(0, 2) * 2 - 1;
        ballVelocityY = new Random().Next(0, 2) * 2 - 1;
        lastBallMoveTime = DateTime.Now;
    }

    public void Run()
    {
        while (gameRunning)
        {
            if ((DateTime.Now - lastBallMoveTime).TotalMilliseconds > 100)
            {
                MoveBall();
                CheckCollision();
                lastBallMoveTime = DateTime.Now;
            }

            HandleInput();
            Draw();
            Thread.Sleep(20);
        }
    }

    private void HandleInput()
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.W:
                    paddle1Position = Math.Max(1, paddle1Position - 1);
                    break;
                case ConsoleKey.S:
                    paddle1Position = Math.Min(playfieldHeight - paddleHeight - 1, paddle1Position + 1);
                    break;
                case ConsoleKey.UpArrow:
                    paddle2Position = Math.Max(1, paddle2Position - 1);
                    break;
                case ConsoleKey.DownArrow:
                    paddle2Position = Math.Min(playfieldHeight - paddleHeight - 1, paddle2Position + 1);
                    break;
                case ConsoleKey.Escape:
                    gameRunning = false;
                    break;
            }
        }
    }

    private void MoveBall()
    {
        ballX += ballVelocityX;
        ballY += ballVelocityY;
    }

    private void CheckCollision()
    {
        if (ballY <= 1 || ballY >= playfieldHeight - 2)
        {
            ballVelocityY = -ballVelocityY;
        }

        if (ballX == 3 && ballY >= paddle1Position && ballY <= paddle1Position + paddleHeight)
        {
            ballVelocityX = -ballVelocityX;
        }

        if (ballX == playfieldWidth - 4 && ballY >= paddle2Position && ballY <= paddle2Position + paddleHeight)
        {
            ballVelocityX = -ballVelocityX;
        }

        if (ballX < 1)
        {
            player2Score++;
            ResetBall();
        }
        else if (ballX > playfieldWidth - 2)
        {
            player1Score++;
            ResetBall();
        }
    }

    private void Draw()
    {
        Console.SetCursorPosition(0, 0);
        StringBuilder frame = new StringBuilder();

        for (int y = 0; y <= playfieldHeight; y++)
        {
            for (int x = 0; x <= playfieldWidth; x++)
            {
                if (x == 0 || y == 0 || x == playfieldWidth || y == playfieldHeight)
                {
                    frame.Append("*");
                }
                else if (x == 2 && y >= paddle1Position && y < paddle1Position + paddleHeight)
                {
                    frame.Append("|");
                }
                else if (x == playfieldWidth - 3 && y >= paddle2Position && y < paddle2Position + paddleHeight)
                {
                    frame.Append("|");
                }
                else if (x == ballX && y == ballY)
                {
                    frame.Append("■");
                }
                else
                {
                    frame.Append(" ");
                }
            }
            frame.AppendLine();
        }

        string scoreText = $"Player 1: {player1Score}  Player 2: {player2Score}";
        frame.AppendLine(scoreText.PadLeft(playfieldWidth / 2 + scoreText.Length / 2));

        Console.Write(frame.ToString());
    }

    static void Main()
    {
        var game = new PongGame();
        game.Run();
    }
}
