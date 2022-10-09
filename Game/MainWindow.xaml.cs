using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Game
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Ellipse Mainellipse;
        Ellipse ellipse1;
        Ellipse ellipse2;
        Rectangle rectdead;
        const int Size = 50;
        static int points;
        static int point;
        static int extra;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DrawGameArea();
        }

        private void DrawGameArea()
        {
            points = 0;
            point = 0;
            extra = 0;
            bool doneDrawingBackground = false;
            bool doneExtra = false;
            int nextX = 0, nextY = 0;
            int rowCounter = 0;
            Random random = new Random();
            if(Mainellipse == null)
                Mainellipse = ellipse;
            else
            {
                GameArea.Children.Add(Mainellipse);
                Canvas.SetTop(Mainellipse, 10);
                Canvas.SetLeft(Mainellipse, 10);
            }

            while (doneDrawingBackground == false)
            {
                int ran = random.Next(1, 10);
                int ran1 = random.Next(1, 10);
                int ran2 = random.Next(1, 10);
                Rectangle rect = new Rectangle
                {
                    Width = Size,
                    Height = Size,
                    Stroke = Brushes.Black
                };
                ellipse1 = new Ellipse
                {
                    Width = 20,
                    Height = 20,
                    Fill = Brushes.Yellow
                };
                ellipse2 = new Ellipse
                {
                    Width = 20,
                    Height = 20,
                    Fill = Brushes.Red
                };
                rectdead = new Rectangle
                {
                    Width = Size,
                    Height = Size,
                    Fill = Brushes.Black
                };
                GameArea.Children.Add(rect);
                Canvas.SetTop(rect, nextY);
                Canvas.SetLeft(rect, nextX);
                nextX += Size;
                if (nextX >= GameArea.ActualWidth)
                {
                    nextX = 0;
                    nextY += Size;
                    rowCounter++;
                }

                if (nextY >= GameArea.ActualHeight)
                    doneDrawingBackground = true;
                if (ran > 7)
                {
                    GameArea.Children.Add(rectdead);
                    Canvas.SetTop(rectdead, nextY);
                    Canvas.SetLeft(rectdead, nextX);
                }
                if (ran1 > 8)
                {
                    if (Canvas.GetTop(rectdead) != nextY && Canvas.GetLeft(rectdead) != nextX)
                    {
                        GameArea.Children.Add(ellipse1);
                        Canvas.SetTop(ellipse1, nextY + 15);
                        Canvas.SetLeft(ellipse1, nextX + 15);
                        points++;
                    }
                }
                if (doneExtra == false)
                {
                    if (ran2 > 8)
                    {
                        if (Canvas.GetTop(rectdead) != nextY && Canvas.GetLeft(rectdead) != nextX)
                        {
                            if (Canvas.GetLeft(ellipse1) != nextX + 15 && Canvas.GetTop(ellipse1) != nextX + 15)
                            {
                                GameArea.Children.Add(ellipse2);
                                Canvas.SetTop(ellipse2, nextY + 15);
                                Canvas.SetLeft(ellipse2, nextX + 15);
                                doneExtra = true;
                            }
                        }
                    }
                }
                label.Content = $"Счет 0 из {points}";
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Canvas.GetTop(ellipse) > 10)
            {
                if (e.Key == Key.W)
                {
                    Canvas.SetTop(ellipse, Canvas.GetTop(ellipse) - 50);
                }
            }
            if (Canvas.GetTop(ellipse) < 510)
            {
                if (e.Key == Key.S)
                {
                    Canvas.SetTop(ellipse, Canvas.GetTop(ellipse) + 50);
                }
            }
            if (Canvas.GetLeft(ellipse) < 710)
            {
                if (e.Key == Key.D)
                {
                    Canvas.SetLeft(ellipse, Canvas.GetLeft(ellipse) + 50);
                }
            }
            if (Canvas.GetLeft(ellipse) > 10)
            {
                if (e.Key == Key.A)
                {
                    Canvas.SetLeft(ellipse, Canvas.GetLeft(ellipse) - 50);
                }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            for (int i = 1; i < GameArea.Children.Count; i++)
            {
                UIElement child = GameArea.Children[i];
                if (Canvas.GetLeft(ellipse) == Canvas.GetLeft(child) - 5 && Canvas.GetTop(ellipse) == Canvas.GetTop(child) - 5 && (child as Ellipse).Fill == Brushes.Yellow)
                {
                    GameArea.Children.Remove(child);
                    point++;
                }
                label.Content = $"Счет {point} из {points}";
                if(Canvas.GetLeft(ellipse) == Canvas.GetLeft(child) - 5 && Canvas.GetTop(ellipse) == Canvas.GetTop(child) - 5 && (child as Ellipse).Fill == Brushes.Red)
                {
                    GameArea.Children.Remove(child);
                    extra++;
                }
                extraText.Content = $"Жизни {extra}/1";
                if (Canvas.GetLeft(ellipse) == Canvas.GetLeft(child) + 10 && Canvas.GetTop(ellipse) == Canvas.GetTop(child) + 10 && (child as Rectangle).Fill == Brushes.Black)
                {
                    if (extra == 0)
                    {
                        GameArea.Children.Clear();
                        extraText.Content = String.Empty;
                        label.FontSize = 100;
                        label.HorizontalAlignment = HorizontalAlignment.Center;
                        label.VerticalAlignment = VerticalAlignment.Center;
                        label.Margin = new Thickness(0, 0, 0, 0);
                        label.Width = 500;
                        label.Height = 500;
                        label.Content = "You lose";
                        RestartButton.IsEnabled = true;

                    }
                    else
                    {
                        extra = 0;
                    }
                }
                if (point == points)
                {
                    GameArea.Children.Clear();
                    extraText.Content = String.Empty;
                    label.FontSize = 100;
                    label.HorizontalAlignment = HorizontalAlignment.Center;
                    label.VerticalAlignment = VerticalAlignment.Center;
                    label.Margin = new Thickness(0, 0, 0, 0);
                    label.Width = 500;
                    label.Height = 500;
                    label.Content = "You win";
                    RestartButton.IsEnabled = true;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DrawGameArea();
            label.FontSize = 12;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.VerticalAlignment = VerticalAlignment.Top;
            label.Margin = new Thickness(94, 54, 0, 0);
            RestartButton.IsEnabled = false;
            extraText.Content = $"Жизни {extra}/1";
            extra = 0;
        }
    }
}
