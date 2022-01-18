using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Animations;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

using System;
using MonoGame.Extended.TextureAtlases;
using System.Collections.Generic;

namespace SAE
{
    public class Game1 : Game
    {
        public const int LARGEUR_PERSO = 32;
        public const int HAUTEUR_PERSO = 52;
        private Rectangle _hitboxPerso;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Vector2 _persoPosition;
        private Vector2 _gunPosition;
        private int _vitessePerso;
        private AnimatedSprite _perso;
        private AnimatedSprite _FantomeGros;
        private AnimatedSprite _FantomeBase;
        private AnimatedSprite _FantomePetit;
        public const int LARGEUR_BASE = 72;
        public const int HAUTEUR_BASE = 60;
        public const int LARGEUR_GROS = 84;
        public const int HAUTEUR_GROS = 88;
        public const int LARGEUR_PETIT = 40;
        public const int HAUTEUR_PETIT = 44;
        public const int HAUTEUR_BULLET = 4;
        public const int LARGEUR_BULLET = 4;
        private AnimatedSprite _gun;
        private Song song;
        private Texture2D _backgroundTexture;
        private Texture2D _backgroundBonesTexture;
        private Vector2 _distance;
        private float rotation;
        private int [] VitesseFantomes;
        public int _score;
        public SpriteFont _textScore;
        public Vector2 _positionScore;
        private AnimatedSprite _vie;
        public int viePerso;
        public Vector2 _viePosition;

        private AnimatedSprite[] monsters;
        private Vector2[] monsterPositions;
        private string[] monsterName;

        List<Bullets> bullets = new List<Bullets>();
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
            _hitboxPerso = new Rectangle((int)_persoPosition.X, (int)_persoPosition.Y, LARGEUR_PERSO, HAUTEUR_PERSO);
            _gunPosition = new Vector2(GraphicsDevice.Viewport.Width / 2 + 15, GraphicsDevice.Viewport.Height - 97);
            _vitessePerso = 160;
            viePerso = 3;
            _viePosition = new Vector2(70, 25);
            VitesseFantomes = new int[3];
            VitesseFantomes[0] = 100;
            VitesseFantomes[1] = 85;
            VitesseFantomes[2] = 70;

            monsterName = new string[3];
            monsterName[0] = "fantôme petit";
            monsterName[1] = "fantôme base";
            monsterName[2] = "fantôme gros";

            _score = 0;
            _positionScore = new Vector2(660, 0);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // spritesheet
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("animations.sf", new JsonContentLoader());
           // SpriteSheet spriteSheetCoeurs = Content.Load<SpriteSheet>("coeurs.sf", new JsonContentLoader());
            _backgroundTexture = Content.Load<Texture2D>("Battleground4");
            _backgroundBonesTexture = Content.Load<Texture2D>("bones");
            SpriteSheet spriteGun = Content.Load<SpriteSheet>("Gun.sf", new JsonContentLoader());
            _perso = new AnimatedSprite(spriteSheet);
           // _vie = new AnimatedSprite(spriteSheetCoeurs);
            _gun = new AnimatedSprite(spriteGun);
            _FantomePetit = new AnimatedSprite(spriteSheet, "fantôme petit");

            _textScore = Content.Load<SpriteFont>("Font");

            monsters = new AnimatedSprite[3];
            monsterPositions = new Vector2[3];
            for (int i = 0; i < monsters.Length; i++)//pour tout les mobs
            {
                System.Random fantomey = new Random();
                int positionFantomeY = fantomey.Next(0, GraphicsDevice.Viewport.Height);

                monsters[i] = new AnimatedSprite(spriteSheet, monsterName[i]);
                monsterPositions[i] = new Vector2(0 + monsters[i].TextureRegion.Width, positionFantomeY);
            }

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

            string animationCoeurs = "1 vie";
            if(viePerso == 3)
            {
                animationCoeurs = "1 vie";
            }
            else if(viePerso == 2)
            {
                animationCoeurs = "2 vie";
            }
            else
            {
                animationCoeurs = "3 vie";
            }
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

            _hitboxPerso.X = (int)_persoPosition.X;
            System.Console.WriteLine(_hitboxPerso);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                Shoot();


            
            for (int i = 0; i < monsters.Length; i++)//pour tout les mobs
            {
                //monsters[i] le mob
                //monsterPositions[i] la position du mob                

                System.Random apparition = new Random();
                int spawn = apparition.Next(0, 3);
                float diagonal;
                if (monsterPositions[i].X == _persoPosition.X || monsterPositions[i].Y == _persoPosition.Y)
                {
                    diagonal = 1;
                }
                else
                {
                    diagonal = (float)Math.Sqrt((VitesseFantomes[i] * VitesseFantomes[i]) / 2)/100;
                }

                if (monsterPositions[i].X > _persoPosition.X)
                {
                    monsterPositions[i].X -= (float)VitesseFantomes[i] * diagonal * deltaSeconds;
                }
                else if (monsterPositions[i].X < _persoPosition.X)
                {
                    monsterPositions[i].X += (float)VitesseFantomes[i] * diagonal * deltaSeconds;
                }
                else
                {
                    //sur la même ligne pas besoin de bouger
                }

                if (monsterPositions[i].Y > _persoPosition.Y)
                {
                    monsterPositions[i].Y -= (float)VitesseFantomes[i] * diagonal * deltaSeconds;
                }
                else if (monsterPositions[i].Y < _persoPosition.Y)
                {
                    monsterPositions[i].Y += (float)VitesseFantomes[i] * diagonal * deltaSeconds;
                }
                else
                {
                    //sur la même colonne pas besoin de bouger
                }

                if (_persoPosition.Y < 220)
                {
                    _persoPosition.Y = 220;
                    _gunPosition.Y = _persoPosition.Y + 3;
                }

                if (_persoPosition.X < 14)
                {
                    _persoPosition.X = 14;
                    _gunPosition.X = _persoPosition.X + 15;
                }

                if (_persoPosition.Y > GraphicsDevice.Viewport.Height - 14)
                {
                    _persoPosition.Y = GraphicsDevice.Viewport.Height - 14;
                    _gunPosition.Y = _persoPosition.Y + 3;
                }

                if (_persoPosition.X > GraphicsDevice.Viewport.Width - 14)
                {
                    _persoPosition.X = GraphicsDevice.Viewport.Width - 14;
                    _gunPosition.X = _persoPosition.X + 15;
                }

                //faire l'animation du mob (son déplacement)
            }
            

            UpdateBullets();
           // _vie.Play(animationCoeurs);
            _perso.Play(animation);
            _gun.Play(animationGun);
            _perso.Update(deltaSeconds);
            _FantomePetit.Play("fantôme petit");
            _FantomePetit.Update(deltaSeconds);

            for (int i = 0; i < monsters.Length; i++)//pour tout les mobs
            {
                monsters[i].Play(monsterName[i]);
                monsters[i].Update(deltaSeconds);
            }
        

            base.Update(gameTime);
        }

        public void UpdateBullets()
        {
            foreach(Bullets bullets in bullets)
            {
                bullets._bulletPosition += bullets.Vélocité;
                if (Vector2.Distance(bullets._bulletPosition, _persoPosition) > 800)
                    bullets.isVisible = false;
                Rectangle hitboxBullets = new Rectangle((int)bullets._bulletPosition.X, (int)bullets._bulletPosition.Y,HAUTEUR_BULLET, LARGEUR_BULLET);
            }

            for(int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].isVisible)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }

            //besoin hitbox fantôme
            /*foreach(Bullets bullets in )
            {
                _score++;
            }*/
            Rectangle hitboxlePerso = new Rectangle((int)_persoPosition.X, (int)_persoPosition.Y, LARGEUR_PERSO, HAUTEUR_PERSO);
            Rectangle hitboxFantomePetit = new Rectangle((int)monsterPositions[0].X, (int)monsterPositions[0].Y, HAUTEUR_PETIT, LARGEUR_PETIT);
            Rectangle hitboxFantomeBase = new Rectangle((int)monsterPositions[1].X, (int)monsterPositions[1].Y, HAUTEUR_BASE, LARGEUR_BASE);
            Rectangle hitboxFantomeGros = new Rectangle((int)monsterPositions[2].X, (int)monsterPositions[2].Y, HAUTEUR_GROS, LARGEUR_GROS);
            
        }


        public void Shoot()
        {
            Bullets newBullet = new Bullets(Content.Load<Texture2D>("bullet"));
            newBullet.Vélocité = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * 10f;
            newBullet._bulletPosition = _gunPosition + newBullet.Vélocité;
            newBullet.isVisible = true;

            if (bullets.Count < 1)
                bullets.Add(newBullet);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(_perso, _persoPosition);
            _spriteBatch.Draw(_gun, _gunPosition, rotation);
            foreach (Bullets bullet in bullets)
                bullet.Draw(_spriteBatch);
            _spriteBatch.Draw(_backgroundBonesTexture, new Vector2(0, 0), Color.White);
            for (int i = 0; i < monsters.Length; i++)
            {
                _spriteBatch.Draw(monsters[i], monsterPositions[i]);
            }
            _spriteBatch.DrawString(_textScore, $"Score : {_score}", _positionScore, Color.Black);
          //  _spriteBatch.Draw(_vie, _viePosition);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
