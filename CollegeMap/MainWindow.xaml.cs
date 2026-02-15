using System.IO;

using System;
using System.Collections.Generic;

using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using CollegeMap.Models;

namespace CollegeMap;

public partial class MainWindow : Window
{
    
    private List<Room> rooms = new();

    public MainWindow()
    {
        
        InitializeComponent();
        LoadRooms();
    }

    private void LoadRooms()
    {
        try
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "rooms.json");
            string json = File.ReadAllText(jsonPath);

            rooms = JsonSerializer.Deserialize<List<Room>>(json) ?? new List<Room>();

            StartPointCombo.ItemsSource = rooms;
            StartPointCombo.DisplayMemberPath = "Number";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Помилка завантаження rooms.json: " + ex.Message);
        }
    }

    private void Search_Click(object sender, RoutedEventArgs e)
    {
        var start = StartPointCombo.SelectedItem as Room;
        string roomNumber = RoomTextBox.Text.Trim();

        var target = rooms.FirstOrDefault(r => r.Number == roomNumber);

        if (start == null)
        {
            MessageBox.Show("Оберіть, де ви зараз");
            return;
        }

        if (target == null)
        {
            MessageBox.Show("Аудиторію не знайдено");
            return;
        }
        
        if (start.Floor == target.Floor)
        {
            ShowMap(start.Map);

            MapCanvas.Children.Clear();
            MapCanvas.Children.Add(FloorImage);

            DrawFlag(start.X, start.Y);
            DrawBlinkingDot(target.X, target.Y);
        }
        else
        {
            ShowMap(start.Map);

            MapCanvas.Children.Clear();
            MapCanvas.Children.Add(FloorImage);

            DrawFlag(start.X, start.Y);
            
            MessageBox.Show($"Вам потрібно пройти на {target.Floor} поверх, скористайтеся сходами");
            
            ShowMap(target.Map);

            MapCanvas.Children.Clear();
            MapCanvas.Children.Add(FloorImage);

            DrawBlinkingDot(target.X, target.Y);
        }
    }
 

    private void ShowMap(string mapPath)
    {
        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, mapPath);

        var bitmap = new BitmapImage(new Uri(fullPath));
        FloorImage.Source = bitmap;
        
        MapCanvas.Width = bitmap.PixelWidth;
        MapCanvas.Height = bitmap.PixelHeight;
    }


    private void DrawFlag(double x, double y)
    {
        var flag = new System.Windows.Shapes.Polygon
        {
            Fill = Brushes.Red,
            Points = new PointCollection
            {
                new Point(0,0),
                new Point(0,20),
                new Point(15,10)
            }
        };

        Canvas.SetLeft(flag, x);
        Canvas.SetTop(flag, y - 10);

        MapCanvas.Children.Add(flag);
    }

    private void DrawBlinkingDot(double x, double y)
    {
        var dot = new System.Windows.Shapes.Ellipse
        {
            Width = 20,
            Height = 20,
            Fill = Brushes.LimeGreen,
            Opacity = 1
        };

        Canvas.SetLeft(dot, x - 10);
        Canvas.SetTop(dot, y - 10);

        MapCanvas.Children.Add(dot);

        var animation = new DoubleAnimation
        {
            From = 1,
            To = 0.2,
            Duration = TimeSpan.FromSeconds(0.6),
            AutoReverse = true,
            RepeatBehavior = RepeatBehavior.Forever
        };

        dot.BeginAnimation(UIElement.OpacityProperty, animation);
    }
    

}

