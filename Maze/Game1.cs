using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using GameArchitectureExample.Screens;
using GameArchitectureExample.StateManagement;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Maze
{
    public class Game1 : Game
    {
        ContentManager content;

        private GraphicsDeviceManager _graphics;
        private readonly ScreenManager _screenManager;
        private SpriteBatch _spriteBatch;
        bool playSound = false;

        BuildMaze maze;

        Texture2D _mouseTexture;

        SoundEffect fireworkExplosionSound;
        SoundEffect errorSound;

        Song backgroundMusic;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);

            AddInitialScreens();
        }

        private void AddInitialScreens()
        {
            _screenManager.AddScreen(new BackgroundScreen(), null);
            _screenManager.AddScreen(new MainMenuScreen(), null);

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            maze = new BuildMaze();
            Mouse.SetPosition(60, 60);
            //place player at start

            base.Initialize();
        }

        protected override void LoadContent()
        {

            if (content == null) content = new ContentManager(this.Services, "Content");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _mouseTexture = this.Content.Load<Texture2D>("ghost");
            Mouse.SetCursor(MouseCursor.FromTexture2D(_mouseTexture, _mouseTexture.Width / 2, _mouseTexture.Height / 2));
            maze.LoadContent(content);
            fireworkExplosionSound = this.Content.Load<SoundEffect>("fireworkExplosion");
            errorSound = this.Content.Load<SoundEffect>("error");
            backgroundMusic = this.Content.Load<Song>("starryNight");
            MediaPlayer.Play(backgroundMusic);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();



            if (maze.collide() == 1 && playSound)
            {
                errorSound.Play(0.3f, 0, 0);
                playSound = false;
            }
            else if (maze.collide() == 2 && playSound)
            {
                fireworkExplosionSound.Play(0.3f, 0, 0);
                playSound = false;
            }
            else if(maze.collide() == 0) 
            {
               playSound = true;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            maze.Draw(gameTime, _spriteBatch);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
