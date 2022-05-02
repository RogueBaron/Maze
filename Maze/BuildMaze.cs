using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace Maze
{
    public class BuildMaze
    {

        /// <summary>The map filename</summary>
        private string _mapFilename = "maze.txt";

        /// <summary>The tileset texture</summary>
        private Texture2D _tilesetTexture;

        /// <summary>The map and tile dimensions</summary>
        private int _tileWidth, _tileHeight, _mapWidth, _mapHeight;

        /// <summary>The tileset data</summary>
        private Rectangle[] _tiles;

        /// <summary>The map data</summary>
        private int[] _map;

        private Color c;

        public int collide()
        {
            Point p = Mouse.GetState().Position;
            var end = new Rectangle(720, 400, 40, 40);

            var loc = (Mouse.GetState().Y / 40) * 20 + (Mouse.GetState().X / 40);

            if (loc < 0 || loc > 239)
            {
                return -1;
            }


            if (_map[loc] == 1)
            {
                c = Color.Red;
                return 1;
            }
            else if (end.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                c = Color.GreenYellow;
                return 2;
            }
            else
            {
                c = Color.White;
                return 0;
            }

        }

        public void LoadContent(ContentManager content)
        {
            // Read in the map file
            string data = File.ReadAllText(Path.Join(content.RootDirectory, _mapFilename));
            var lines = data.Split('\n');

            // First line is tileset image file name 
            var tilesetFileName = lines[0].Trim();
            _tilesetTexture = content.Load<Texture2D>(tilesetFileName);

            // Second line is tile size
            var secondLine = lines[1].Split(',');
            _tileWidth = int.Parse(secondLine[0]);
            _tileHeight = int.Parse(secondLine[1]);

            // Now that we know the tile size and tileset
            // image, we can determine tile bounds
            int tilesetColumns = _tilesetTexture.Width / _tileWidth;
            int tilesetRows = _tilesetTexture.Height / _tileWidth;
            _tiles = new Rectangle[tilesetColumns * tilesetRows];
            for (int y = 0; y < tilesetRows; y++)
            {
                for (int x = 0; x < tilesetColumns; x++)
                {
                    _tiles[y * tilesetColumns + x] = new Rectangle(
                        x * _tileWidth, // upper left-hand x cordinate
                        y * _tileHeight, // upper left-hand y coordinate
                        _tileWidth, // width 
                        _tileHeight // height
                        );
                }
            }

            // Third line is map size (in tiles)
            var thirdLine = lines[2].Split(',');
            _mapWidth = int.Parse(thirdLine[0]);
            _mapHeight = int.Parse(thirdLine[1]);

            // Fourth line is map data
            _map = new int[_mapWidth * _mapHeight];
            var fourthLine = lines[3].Split(',');
            for (int i = 0; i < _mapWidth * _mapHeight; i++)
            {
                _map[i] = int.Parse(fourthLine[i]);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(_tilesetTexture,);
            for (int y = 0; y < _mapHeight; y++)
            {
                for (int x = 0; x < _mapWidth; x++)
                {
                    // Indexes start at 1, so shift for array coordinates
                    int index = _map[y * _mapWidth + x] - 1;
                    // Index of -1 (shifted from 0) should not be drawn
                    if (index == -1) continue;
                    spriteBatch.Draw(
                        _tilesetTexture,
                        new Vector2(
                            x * _tileWidth,
                            y * _tileHeight
                        ),
                        _tiles[index],
                        c
                    );
                }
            }
            spriteBatch.Draw(_tilesetTexture, new Vector2(40, 40), Color.BlueViolet);
            spriteBatch.Draw(_tilesetTexture, new Vector2(720, 400), Color.Green);
        }
    }
}
