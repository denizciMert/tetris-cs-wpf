using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png",UriKind.Relative))
        };

        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png",UriKind.Relative))
        };

        private readonly Image[,] imageControls;
        private  int maxDelay = 999999;
        private readonly int minDelay = 50;
        private readonly int delayDecrease = 25;

        private GameState gameState=new GameState();
        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid);
        }

        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellsize = 25;

            for (int r=0; r<grid.Rows;r++)
            {
                for (int c=0;c<grid.Columns;c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellsize,
                        Height = cellsize
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellsize+10);
                    Canvas.SetLeft(imageControl, c * cellsize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }
            return imageControls;
        }

        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r,c];
                    imageControls[r,c].Opacity = 1;
                    imageControls[r,c].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(GameBlocks gameBlocks)
        {
            foreach (BlockPosition p in gameBlocks.TilesPositions())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[gameBlocks.Id];
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            GameBlocks next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        private void DrawHeldBlock(GameBlocks heldBlock)
        {
            if (heldBlock == null)
            {
                HoldImage.Source = blockImages[0];
            }
            else
            {
                HoldImage.Source = blockImages[heldBlock.Id];
            }
        }

        private void DrawGhostBlock(GameBlocks block)
        {
            int dropDistance = gameState.BlockDropDistance();

            foreach (BlockPosition p in block.TilesPositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
            }
        }

        private void Draw(GameState gameState)
        {
             DrawGrid(gameState.GameGrid);
             DrawGhostBlock(gameState.CurrentBlock);
             DrawBlock(gameState.CurrentBlock);
             DrawNextBlock(gameState.BlockQueue);
             DrawHeldBlock(gameState.HeldBlock);
             ScoreText.Text = $"Score: {(gameState.Score) * 100}";  
        }

        private async Task GameLoop()
        {
         
            Draw(gameState);  

            while (!gameState.GameOver)
            {
                int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrease));
                await Task.Delay(delay);
                gameState.MoveBlockDowm();
                Draw(gameState);
            }

            GameOverMenu.Visibility= Visibility.Visible;
            FinalScoreText.Text = $"Score: {(gameState.Score) * 100}";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }

            switch(e.Key)
            {
                case Key.Up:
                    gameState.HoldBlock();
                    break;
                case Key.Down:
                    gameState.MoveBlockDowm();
                    break;
                case Key.Left:
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;

                case Key.Z:
                    gameState.RotateBlockCCW();
                    break;
                case Key.X:
                    gameState.RotateBlockCW();
                    break;

                case Key.W:
                    gameState.HoldBlock();
                    break;
                case Key.S:
                    gameState.MoveBlockDowm();
                    break;
                case Key.A:
                    gameState.MoveBlockLeft();
                    break;
                case Key.D:
                    gameState.MoveBlockRight();
                    break;

                case Key.Q:
                    gameState.RotateBlockCCW();
                    break;
                case Key.E:
                    gameState.RotateBlockCW();
                    break;

                case Key.Space:
                    gameState.DropBlock();
                    break;

                default:
                    return;
            }

            Draw(gameState);
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            StartMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }


        private async void StartGame_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            StartMenu.Visibility = Visibility.Hidden;
            maxDelay = 1000;
            await GameLoop();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void HelpBack_Click(object sender, RoutedEventArgs e)
        {
            StartMenu.Visibility= Visibility.Visible;
            HelpMenu.Visibility= Visibility.Hidden;
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            StartMenu.Visibility = Visibility.Hidden;
            HelpMenu.Visibility = Visibility.Visible;
        }
    }
}
