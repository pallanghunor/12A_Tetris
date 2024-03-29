﻿using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _12A_Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative))
        };

        private ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative))
        };
        
        private Game game = new Game();

        private DateTime lastMoveTime = DateTime.MinValue;
        private TimeSpan moveInterval = TimeSpan.FromMilliseconds(75);
        public MainWindow()
        {
            InitializeComponent();
            game.Level = 1;
        }

        //private void FillLevelcbx()
        //{
        //    for (int i = 1; i <= 5; i++)
        //    {
        //        levelCbx.Items.Add(i);
        //    }
        //}
        private void GenerateGameGrid()
        {
            for (int row = 0; row < game.Grid.Rows - 2; row++)
            {
                gameGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) });
            }
            for (int column = 0; column < game.Grid.Columns; column++)
            {
                gameGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50) });
            }
        }

        private void DrawGameGrid(GameGrid grid)
        {
            gameGrid.Children.Clear();
            for (int row = 0; row < grid.Rows - 2; row++)
            {
                for (int column = 0; column < grid.Columns; column++)
                {
                    Image tile = new Image();
                    tile.Source = tileImages[grid[row + 2, column]];
                    Grid.SetRow(tile, row);
                    Grid.SetColumn(tile, column);
                    gameGrid.Children.Add(tile);
                }
            }
        }

        private void DrawCurrentBlock(Block block)
        {
            foreach (var p in block.GetTilePositions())
            {
                Image tile = new Image();
                tile.Source = tileImages[block.Id];
                if (p.Row >= 2)
                {
                    Grid.SetRow(tile, p.Row - 2);
                    Grid.SetColumn(tile, p.Column);
                    gameGrid.Children.Add(tile);
                }
            }
        }

        private void DrawBlockQueue(BlockQueue blockQueue)
        {
            for ( int i = 0; i < blockQueue.blocksQueue.Length; i++)
            {
                Image block = new Image();
                block.Source = blockImages[blockQueue.blocksQueue[i].Id];
                Grid.SetRow(block, i);
                block.Margin = new Thickness(5);
                nextBlocksGrid.Children.Add(block);
            }
        }

        private void DrawHeldBlock(Block block)
        {
            heldBlockGrid.Children.Clear();
            if (block != null)
            {
                Image blockImage = new Image();
                blockImage.Source = blockImages[block.Id];
                heldBlockGrid.Children.Add(blockImage);
            }
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = game.BlockDropDistance();

            foreach (var p in block.GetTilePositions())
            {
                Image tile = new Image();
                tile.Source = tileImages[block.Id];
                tile.Opacity = 0.25;
                if (p.Row + dropDistance >= 2)
                {
                    Grid.SetRow(tile, p.Row + dropDistance - 2);
                    Grid.SetColumn(tile, p.Column);
                    gameGrid.Children.Add(tile);
                }
            }
        }

        private void Draw()
        {
            gameGrid.Children.Clear();
            nextBlocksGrid.Children.Clear();
            GenerateGameGrid();
            DrawGameGrid(game.Grid);
            DrawGhostBlock(game.CurrentBlock);
            DrawCurrentBlock(game.CurrentBlock);
            DrawBlockQueue(game.BlockQueue);
            DrawHeldBlock(game.HeldBlock);
            scoreTxtBlck.Text = $"Score: {game.Score}";

        }

        private async Task GameLoop()
        {
            Draw();
            while (!game.GameOver)
            {
                if (!game.Paused)
                {
                    await Task.Delay(game.GameSpeed);
                    game.MoveBlockDown();
                    Draw();
                } 
                else
                {
                    await Task.Delay(1);
                }
            }

            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreTxtBlck.Text = $"Final Score: {game.Score}";
            game.WriteScoreHistory();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (game.GameOver || game.Paused)
            {
                return;
            }

            DateTime now = DateTime.Now;
            TimeSpan elapsed = now - lastMoveTime;

            if (elapsed < moveInterval)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    game.MoveBlockLeft();
                    break;
                case Key.Right:
                    game.MoveBlockRight();
                    break;
                case Key.Up:
                    game.RotateBlockClockwise();
                    break;
                case Key.Down:
                    game.MoveBlockDown();
                    break;
                case Key.Z:
                    game.RotateBlockCounterClockwise();
                    break;
                case Key.Space:
                    game.DropBlock();
                    break;
                case Key.C:
                    game.HoldBlock();
                    break;
                case Key.P:
                    pauseGame();
                    break;
                case Key.Escape:
                    pauseGame();
                    break;
                default:
                    return;
            }

            lastMoveTime = now;
            Draw();
        }

        private void pauseGame()
        {
            game.Paused = true;
            PauseMenu.Visibility = Visibility.Visible;
        }

        private async void Restart_Click(object sender, RoutedEventArgs e)
        {
            game = new Game();
            GameOverMenu.Visibility= Visibility.Hidden;
            startGameMenu.Visibility = Visibility.Visible;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ResumeBtn_Click(object sender, RoutedEventArgs e)
        {
            game.Paused = false;
            PauseMenu.Visibility = Visibility.Hidden;
        }

        private async void StartGameBtn_Click(object sender, RoutedEventArgs e)
        {
            startGameMenu.Visibility = Visibility.Hidden;
            game.Name = NameTBx.Text;
            levelTxtBlck.Text = $"Level: {game.Level}";
            if(game.ScoreHistory.Count > 0)
            {
                bestPlayerTxtBlck.Text = $"{game.ScoreHistory.Max(x => x.Name)}";
                bestPlayerScoreTxtBlck.Text = $"{game.ScoreHistory.Max(x => x.Point)}";
            }
            game.SetGameSpeed();
            game.Paused = false;
            await GameLoop();
        }

        private void pauseBtn_Click(object sender, RoutedEventArgs e)
        {
            pauseGame();
        }

        private void NameTBx_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(NameTBx.Text != "")
            {
                nameTbxPlaceholder.Visibility = Visibility.Hidden;
            }
            else
            {
                nameTbxPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void levelBtn_Click(object sender, RoutedEventArgs e)
        {
            game.Level++;
            if(game.Level > 5)
            {
                game.Level = 1;
            }
            levelBtn.Content = $"Level: {game.Level}";
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            HelpMenu.Visibility = Visibility.Hidden;
            startGameMenu.Visibility = Visibility.Visible;
        }

        private void helpBtn_Click(object sender, RoutedEventArgs e)
        {
            startGameMenu.Visibility = Visibility.Hidden;
            HelpMenu.Visibility = Visibility.Visible;
        }
    }
}