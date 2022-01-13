using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Animations;
using Microsoft.Xna.Framework.Content;

using System;

namespace SAE
{
    public class Game1 : Game
    {   

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Vector2 _persoPosition;
        private int _vitessePerso;
        private AnimatedSprite _perso;
        private Song song;
        private int _VitesseFantomeRouge;
        private int _VitesseFantomeBleu;
        private int _VitesseFantomeJaune;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _persoPosition = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - 100);
            _vitessePerso = 100;
            _VitesseFantomeJaune = 100;
            _VitesseFantomeRouge = 85;
            _VitesseFantomeBleu = 70;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // spritesheet
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("Sprites.sf", new JsonContentLoader());
            _perso = new AnimatedSprite(spriteSheet);
            this.song = Content.Load<Song>("hauntedcastle");
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

            // TODO: use this.Content to load your game content here
        }
        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(song);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float walkSpeed = deltaSeconds * _vitessePerso;
            
            _perso.Update(deltaSeconds);
            // TODO: Add your update logic here
            KeyboardState keyboardState = Keyboard.GetState();
            string animation = "idle droite";
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                animation = "marche gauche";
                _persoPosition.X -= walkSpeed;
            }
            if(keyboardState.IsKeyDown(Keys.Up))
            {
                _persoPosition.Y -= walkSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                _persoPosition.Y += walkSpeed;
            }
            if(keyboardState.IsKeyDown(Keys.Right))
            {
                animation = "marche droite";
                _persoPosition.X += walkSpeed;
            }

            _perso.Play(animation);
            _perso.Update(deltaSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(_perso, _persoPosition);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
