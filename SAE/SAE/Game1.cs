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
using MonoGame.Extended.TextureAtlases;
using System.Collections.Generic;

namespace SAE
{
    public class Game1 : Game
    {   

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Vector2 _persoPosition;
        private int _vitessePerso;
        private Vector2 _positionFantome;
        private AnimatedSprite _perso;
        private AnimatedSprite _FantomeGros;
        private AnimatedSprite _FantomeBase;
        private AnimatedSprite _FantomePetit;
        private Song song;
        private int _VitesseFantomePetit;
        private int _VitesseFantomeBase;
        private int _VitesseFantomeGros;

        private AnimatedSprite[] monsters;
        private Vector2[] monsterPositions;
      

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
            _VitesseFantomePetit = 100;
            _VitesseFantomeBase = 85;
            _VitesseFantomeGros = 70;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // spritesheet
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("Sprites.sf", new JsonContentLoader());
            _perso = new AnimatedSprite(spriteSheet);
            _FantomePetit = new AnimatedSprite(spriteSheet, "fantôme petit");
            
            monsters = new AnimatedSprite[3];
            monsters[0] = new AnimatedSprite(spriteSheet, "fantôme petit");
            monsters[1] = new AnimatedSprite(spriteSheet, "fantôme base");
            monsters[2] = new AnimatedSprite(spriteSheet, "fantôme gros");

            monsterPositions = new Vector2[3];
            monsterPositions[0] = new Vector2(10, 10);
            monsterPositions[1] = new Vector2(850, 10);
            monsterPositions[2] = new Vector2(10, 400);

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
            _FantomePetit.Update(deltaSeconds);
            foreach (AnimatedSprite monster in monsters)
            {
                monster.Update(deltaSeconds);
            }
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


            for (int i = 0; i < monsters.Length; i++)//pour tout les mobs
            {
                //monsters[i] le mob
                //monsterPositions[i] la position du mob

                //faire l'animation du mob (son déplacement)
            }

            _perso.Play(animation);
            _perso.Update(deltaSeconds);
            _FantomePetit.Play("fantôme petit");
            _FantomePetit.Update(deltaSeconds);

            foreach (AnimatedSprite monster in monsters)
            {
                monster.Update(deltaSeconds);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(_perso, _persoPosition);
            _spriteBatch.Draw(_FantomePetit, _persoPosition);
            for (int i = 0; i < monsters.Length; i++)
            {
                _spriteBatch.Draw(monsters[i], monsterPositions[i]);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
