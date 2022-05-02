using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Maze
{
    public class Game1 : Game
    {
        ContentManager content;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        BuildMaze maze;

        Texture2D _mouseTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            maze.Draw(gameTime, _spriteBatch);
            maze.collide();
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
