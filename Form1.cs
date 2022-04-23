using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
	public partial class Form1 : Form
	{
		private List<Circle> snake = new List<Circle>();
		private Circle food = new Circle();
		public static bool GameOver { get; set; }
		public Form1()
		{
			InitializeComponent();

			Settings.ReadSettings();
			//Set game speed and start timer
			gameTimer.Interval = 1000 / Settings.Speed;
			gameTimer.Tick += gameTimer_Tick;
			gameTimer.Start();

			//Start new game
			StartGame();
		}

		private void StartGame()
		{
			GameOver = false;
			Settings.Score = 0;
			label1.Hide();

			//Create new snake object
			snake.Clear();
			Circle head = new Circle();
			head.X = 10;
			head.Y = 5;
			snake.Add(head);

			//Add some circles to make our snake quite long
			for(int i = 0; i < 3; i++)
				snake.Add(new Circle(i + Settings.PitchX, 5));

			label2.Text = Settings.Score.ToString();
			GenerateFood();
		}

		private void GenerateFood()
		{
			int maxXPos = pbCanvas.ClientSize.Width / Settings.PitchX;
			int maxYPos = pbCanvas.ClientSize.Height / Settings.PitchY;

			Random rand = new Random();
			food = new Circle();
			food.X = rand.Next(0, maxXPos);
			food.Y = rand.Next(0, maxYPos);
		}

		private void MoveSnake()
		{
			for(int i = snake.Count - 1; i >= 0; i--)
			{
				//Move head
				if(i == 0)
				{
					switch(Settings.Direction)
					{
						case Direction.Right:
							snake[i].X++;
							break;
						case Direction.Left:
							snake[i].X--;
							break;
						case Direction.Up:
							snake[i].Y--;
							break;
						case Direction.Down:
							snake[i].Y++;
							break;
					}

					int maxXPos = pbCanvas.ClientSize.Width / Settings.PitchX;
					int maxYPos = pbCanvas.ClientSize.Height / Settings.PitchY;

					//Detect collision with game area borders
					if(snake[i].X < 0 || snake[i].X > maxXPos || snake[i].Y < 0 || snake[i].Y > maxYPos)
						Die();

					//Detect collision with body
					for(int j = 1; j < snake.Count; j++)
					{
						if(snake[i].X == snake[j].X && snake[i].Y == snake[j].Y)
							Die();
					}

					//Detect collision with food
					if(snake[0].X == food.X && snake[0].Y == food.Y)
					{
						Grow();
					}
				}
				//Move body
				else
				{
					snake[i].X = snake[i - 1].X;
					snake[i].Y = snake[i - 1].Y;
				}
			}
		}

		private void Grow()
		{
			Circle food = new Circle();
			food.X = snake[snake.Count - 1].X;
			food.Y = snake[snake.Count - 1].Y;
			snake.Add(food);

			//Update score
			Settings.Score += Settings.Points;
			label2.Text = Settings.Score.ToString();

			GenerateFood();
		}

		private void Die()
		{
			GameOver = true;
		}

		private void gameTimer_Tick(object sender, EventArgs e)
		{
			if(GameOver)
			{
				if(Input.KeyPressed(Keys.Enter))
				{
					StartGame();
				}
			}
			else
			{
				if(Input.KeyPressed(Keys.Right) && Settings.Direction != Direction.Left)
					Settings.Direction = Direction.Right;
				else if(Input.KeyPressed(Keys.Left) && Settings.Direction != Direction.Right)
					Settings.Direction = Direction.Left;
				else if(Input.KeyPressed(Keys.Up) && Settings.Direction != Direction.Down)
					Settings.Direction = Direction.Up;
				else if(Input.KeyPressed(Keys.Down) && Settings.Direction != Direction.Up)
					Settings.Direction = Direction.Down;
				MoveSnake();
			}
			pbCanvas.Invalidate();
		}

		private void DrawSnakesHead(ref Graphics g, Direction d)
		{
			Brush snakeBrush = new SolidBrush(Settings.SnakeColor);

			//know the head's coordinates
			int headX, headY;
			headX = snake[0].X * Settings.PitchX;
			headY = snake[0].Y * Settings.PitchY;
			
			switch(d)
			{
				case Direction.Down:
					//draw head base
					g.FillEllipse(snakeBrush, new RectangleF(headX, headY, Settings.PitchX, Settings.PitchY * 1.5f));
					//draw eyes
					g.FillEllipse(Brushes.Yellow, new Rectangle(headX + 3, headY + 14, 4, 4));
					g.FillEllipse(Brushes.Black, new Rectangle(headX + 4, headY + 16, 2, 2));
					g.FillEllipse(Brushes.Yellow, new Rectangle(headX + 9, headY + 14, 4, 4));
					g.FillEllipse(Brushes.Black, new Rectangle(headX + 10, headY + 16, 2, 2));
					break;
				case Direction.Up:
					//draw head base
					g.FillEllipse(snakeBrush, new RectangleF(headX, headY - Settings.PitchY * 0.5f, Settings.PitchX, Settings.PitchY * 1.5f));
					//draw eyes
					g.FillEllipse(Brushes.Yellow, new Rectangle(headX + 3, headY - 2, 4, 4));
					g.FillEllipse(Brushes.Black, new Rectangle(headX + 4, headY - 2, 2, 2));
					g.FillEllipse(Brushes.Yellow, new Rectangle(headX + 9, headY - 2, 4, 4));
					g.FillEllipse(Brushes.Black, new Rectangle(headX + 10, headY - 2, 2, 2));
					break;
				case Direction.Left:
					//draw head base
					g.FillEllipse(snakeBrush, new RectangleF(headX - Settings.PitchX * 0.5f, headY, Settings.PitchX * 1.5f, Settings.PitchY));
					//draw eyes
					g.FillEllipse(Brushes.Yellow, new Rectangle(headX - 2, headY + 3, 4, 4));
					g.FillEllipse(Brushes.Black, new Rectangle(headX - 2, headY + 4, 2, 2));
					g.FillEllipse(Brushes.Yellow, new Rectangle(headX - 2, headY + 9, 4, 4));
					g.FillEllipse(Brushes.Black, new Rectangle(headX - 2, headY + 10, 2, 2));
					break;
				case Direction.Right:
					//draw head base
					g.FillEllipse(snakeBrush, new RectangleF(headX, headY, Settings.PitchX * 1.5f, Settings.PitchY));
					//draw eyes
					g.FillEllipse(Brushes.Yellow, new Rectangle(headX + 14, headY + 3, 4, 4));
					g.FillEllipse(Brushes.Black, new Rectangle(headX + 16, headY + 4, 2, 2));
					g.FillEllipse(Brushes.Yellow, new Rectangle(headX + 14, headY + 9, 4, 4));
					g.FillEllipse(Brushes.Black, new Rectangle(headX + 16, headY + 10, 2, 2));
					break;
			}
		}

		private void DrawSnake(Graphics g)
		{
			//Set color of the snake
			Random rand = new Random();
			Color snakeColor;
			if(Settings.MultiColor)
				snakeColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
			else
				snakeColor = Settings.SnakeColor;

			Brush snakeBrush = new SolidBrush(snakeColor);

			//Draw the snake and its food
			for(int i = 0; i < snake.Count; i++)
			{
				if(i == 0)
				{
					DrawSnakesHead(ref g, Settings.Direction);
				}
				//Drawing the end of snake's tail
				else if(i == snake.Count - 1)
				{
					g.FillEllipse(snakeBrush, new Rectangle(snake[i].X * Settings.PitchX + 3, snake[i].Y * Settings.PitchY + 3, Settings.PitchX - 4, Settings.PitchY - 4));
				}
				else if(i == snake.Count - 2)
				{
					g.FillEllipse(snakeBrush, new Rectangle(snake[i].X * Settings.PitchX + 2, snake[i].Y * Settings.PitchY + 2, Settings.PitchX - 3, Settings.PitchY - 3));
				}
				else if(i == snake.Count - 3)
				{
					g.FillEllipse(snakeBrush, new Rectangle(snake[i].X * Settings.PitchX + 1, snake[i].Y * Settings.PitchY + 1, Settings.PitchX - 2, Settings.PitchY - 2));
				}
				//Draw snake's body
				else
				{
					g.FillEllipse(snakeBrush, new Rectangle(snake[i].X * Settings.PitchX, snake[i].Y * Settings.PitchY, Settings.PitchX, Settings.PitchY));
				}

				//Drawing the food
				g.FillEllipse(new SolidBrush(Settings.FoodColor), new Rectangle(food.X * Settings.PitchX, food.Y * Settings.PitchY, Settings.PitchX + 4, Settings.PitchY + 4));
				g.FillEllipse(Brushes.White, new Rectangle(food.X * Settings.PitchX + 4, food.Y * Settings.PitchY + 4, 4, 4));
			}
		}

		private void pbCanvas_Paint(object sender, PaintEventArgs e)
		{
			if(!GameOver)
			{
				DrawSnake(e.Graphics);
			}
			else
			{
				string gameOver = "Игра окончена\nВаш финальный счет: " + Settings.Score + "\nНажмите Enter, чтобы попробовать снова";
				label1.Text = gameOver;
				label1.Show();
			}
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			Input.ChangeState(e.KeyCode, true);
		}

		private void Form1_KeyUp(object sender, KeyEventArgs e)
		{
			Input.ChangeState(e.KeyCode, false);
		}

		private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
		{
			gameTimer.Stop();
			Form2 settingsForm = new Form2();
			settingsForm.parent = this;
			settingsForm.ShowDialog();
		}

		private void обИгреToolStripMenuItem_Click(object sender, EventArgs e)
		{
			gameTimer.Stop();
			Form3 form = new Form3();
			form.parent = this;
			form.ShowDialog();
		}
	}
}
