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
        private Vector2 _gunPosition;
        private int _vitessePerso;
        private Vector2 _positionFantome;
        private AnimatedSprite _perso;
        private AnimatedSprite _FantomeGros;
        private AnimatedSprite _FantomeBase;
        private AnimatedSprite _FantomePetit;
        private AnimatedSprite _gun;
        private Song song;
        private int _VitesseFantomePetit;
        private int _VitesseFantomeBase;
        private int _VitesseFantomeGros;
        private Texture2D _backgroundTexture;
        private Texture2D _backgroundBonesTexture;
        private Vector2 _distance;
        private float rotation;
        private int gunRotationPosition;

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
            _gunPosition = new Vector2(GraphicsDevice.Viewport.Width / 2 + 15, GraphicsDevice.Viewport.Height - 97);
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
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("animations.sf", new JsonContentLoader());
            _backgroundTexture = Content.Load<Texture2D>("Battleground4");
            _backgroundBonesTexture = Content.Load<Texture2D>("bones");
            SpriteSheet spriteGun = Content.Load<SpriteSheet>("Gun.sf", new JsonContentLoader());
            _perso = new AnimatedSprite(spriteSheet);
            _gun = new AnimatedSprite(spriteGun);
            _FantomePetit = new AnimatedSprite(spriteSheet, "fantôme petit");

            System.Random fantomey = new Random();
            int positionFantomeY = fantomey.Next(0, GraphicsDevice.Viewport.Width);

            monsters = new AnimatedSprite[3];
            monsters[0] = new AnimatedSprite(spriteSheet, "fantôme petit");
            monsters[1] = new AnimatedSprite(spriteSheet, "fantôme base");
            monsters[2] = new AnimatedSprite(spriteSheet, "fantôme gros");

            monsterPositions = new Vector2[3];
            monsterPositions[0] = new Vector2(10, positionFantomeY);
            monsterPositions[1] = new Vector2(850, positionFantomeY);
            monsterPositions[2] = new Vector2(10, positionFantomeY);


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


            IsMouseVisible = true;
            Vector2 mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            _distance.X = Mouse.GetState().X - _gunPosition.X;
            _distance.Y = Mouse.GetState().Y - _gunPosition.Y;
            rotation = (float)Math.Atan2(_distance.Y, _distance.X);

            _perso.Update(deltaSeconds);
            _gun.Update(deltaSeconds);
            _FantomePetit.Update(deltaSeconds);
            foreach (AnimatedSprite monster in monsters)
            {
                monster.Update(deltaSeconds);
            }
            // TODO: Add your update logic here
            KeyboardState keyboardState = Keyboard.GetState();
            string animation = "idle droite";
            string animationGun = "gun droite";
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                /*animation = "marche gauche";
                animationGun = "gun gauche";
                gunRotationPosition = - 15;*/
                animation = "marche droite";
                _persoPosition.X -= walkSpeed;
                _gunPosition.X -= walkSpeed;
            }
            if(keyboardState.IsKeyDown(Keys.Up))
            {
                _persoPosition.Y -= walkSpeed;
                _gunPosition.Y -= walkSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                _persoPosition.Y += walkSpeed;
                _gunPosition.Y += walkSpeed;
            }
            if(keyboardState.IsKeyDown(Keys.Right))
            {
                animation = "marche droite";
                //gunRotationPosition = 15;
                _persoPosition.X += walkSpeed;
                _gunPosition.X += walkSpeed;
            }
            

            for (int i = 0; i < monsters.Length; i++)//pour tout les mobs
            {
                //monsters[i] le mob
                //monsterPositions[i] la position du mob


               System.Random apparition = new Random();
                int spawn = apparition.Next(0,3);
                


                //faire l'animation du mob (son déplacement)
            }

            _perso.Play(animation);
            _gun.Play(animationGun);
            _perso.Update(deltaSeconds);
            _FantomePetit.Play("fantôme petit");
            _FantomePetit.Update(deltaSeconds);

            foreach (AnimatedSprite monster in monsters)
            {
                monster.Update(deltaSeconds);
            }
            // Rectangle rectanglePerso = new Rectangle((int)_persoPosition.X, (int)_persoPosition.Y, );

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(_perso, _persoPosition);
            _spriteBatch.Draw(_gun, _gunPosition, rotation);
            _spriteBatch.Draw(_backgroundBonesTexture, new Vector2(0, 0), Color.White);
            for (int i = 0; i < monsters.Length; i++)
            {
                _spriteBatch.Draw(monsters[i], monsterPositions[i]);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
