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
        //private Rectangle _hitboxPerso;
        private Rectangle _hitboxBullets;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //public Vector2 _persoPosition;
        private Vector2 _gunPosition;
        //private int _vitessePerso;
        //private AnimatedSprite _perso;
        //private AnimatedSprite _FantomeGros;
        //private AnimatedSprite _FantomeBase;
        //private AnimatedSprite _FantomePetit;
        public const int LARGEUR_BASE = 72;
        public const int HAUTEUR_BASE = 60;
        public const int LARGEUR_GROS = 84;
        public const int HAUTEUR_GROS = 88;
        public const int LARGEUR_PETIT = 40;
        public const int HAUTEUR_PETIT = 44;
        public const int HAUTEUR_BULLET = 4;
        public const int LARGEUR_BULLET = 4;
        public const int NB_MONSTER = 3;

        private AnimatedSprite _gun;
        private Song song;
        private Texture2D _backgroundTexture;
        private Texture2D _backgroundBonesTexture;
        private Vector2 _distance;
        private float rotation;
        
        public int _score;
        public SpriteFont _textScore;
        public Vector2 _positionScore;
        private AnimatedSprite _vie;
        //public int viePerso;
        public Vector2 _viePosition;
        //private int _vieFantomeGros;
        //private int _vieFantomeBase;
        private Random fantomey = new Random();
        private int level = 1;
        private int levelLimit = 10;

        private Monster[] _monsters;
        private Squelette _perso;

        /*private AnimatedSprite[] __monsters;
        private int [] VitesseFantomes;
        private Vector2[] monsterPositions;
        private string[] monsterName;
        private Rectangle[] hitboxMonster;
        private int[] monsterLife;
        private int[] monsterDefaultLife;*/

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
            _gunPosition = new Vector2(GraphicsDevice.Viewport.Width / 2 + 15, GraphicsDevice.Viewport.Height - 97);

            _perso = new Squelette(160, 3, HAUTEUR_PERSO, LARGEUR_PERSO, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - 100));

            _monsters = new Monster[NB_MONSTER];
            _monsters[0] = new Monster(100, 1, "fantôme petit", HAUTEUR_PETIT, LARGEUR_PETIT);
            _monsters[1] = new Monster(85, 2, "fantôme base", HAUTEUR_BASE, LARGEUR_BASE);
            _monsters[2] = new Monster(70, 3, "fantôme gros", HAUTEUR_GROS, LARGEUR_GROS);

            
            _viePosition = new Vector2(70, 25);

            _score = 0;
            _positionScore = new Vector2(600, 0);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("animations.sf", new JsonContentLoader());
            SpriteSheet spriteSheetCoeurs = Content.Load<SpriteSheet>("coeurs.sf", new JsonContentLoader());
            _backgroundTexture = Content.Load<Texture2D>("Battleground4");
            _backgroundBonesTexture = Content.Load<Texture2D>("bones");
            SpriteSheet spriteGun = Content.Load<SpriteSheet>("Gun.sf", new JsonContentLoader());
            _perso.Sprite = new AnimatedSprite(spriteSheet);
            _vie = new AnimatedSprite(spriteSheetCoeurs);
            _gun = new AnimatedSprite(spriteGun);

            _textScore = Content.Load<SpriteFont>("Font");

            for (int i = 0; i < _monsters.Length; i++)//pour tout les mobs
            {
                int positionFantomeY = fantomey.Next(0, GraphicsDevice.Viewport.Height);

                _monsters[i].LoadSprite(spriteSheet);
                _monsters[i].Position = new Vector2(0 -_monsters[i].Hitbox.Width, positionFantomeY);
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


            IsMouseVisible = true;
            Vector2 mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            _distance.X = Mouse.GetState().X - _gunPosition.X;
            _distance.Y = Mouse.GetState().Y - _gunPosition.Y;
            rotation = (float)Math.Atan2(_distance.Y, _distance.X);

            _perso.Sprite.Update(deltaSeconds);
            _gun.Update(deltaSeconds);
            _vie.Update(deltaSeconds);
            foreach (Monster _monster in _monsters)
            {
               _monster.Sprite.Update(deltaSeconds);
            }
            // TODO: Add your update logic here
            string animationCoeurs = "3 vie";
            if (_perso.Life <= 0)
            {
                //mort
            }
            else
            {
                animationCoeurs = _perso.Life.ToString() + " vie";
            }

            string animation = "idle droite";
            string animationGun = "gun droite";
            _gunPosition = _perso.Move(animation,_gunPosition,deltaSeconds);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                Shoot();



            foreach (Monster _monster in _monsters)//pour tout les mobs
            {
                _monster.Move(_perso, deltaSeconds,GraphicsDevice);
            }

            if (_perso.Position.Y < 220)
            {
                _perso.Position = new Vector2(_perso.Position.X,220);
                _gunPosition.Y = _perso.Position.Y + 3;
            }

            if (_perso.Position.X < 14)
            {
                _perso.Position = new Vector2(14,_perso.Position.Y);
                _gunPosition.X = _perso.Position.X + 15;
            }

            if (_perso.Position.Y > GraphicsDevice.Viewport.Height - 14)
            {
                _perso.Position = new Vector2(_perso.Position.X,GraphicsDevice.Viewport.Height - 14);
                _gunPosition.Y = _perso.Position.Y + 3;
            }

            if (_perso.Position.X > GraphicsDevice.Viewport.Width - 14)
            {
                _perso.Position = new Vector2(GraphicsDevice.Viewport.Width - 14, _perso.Position.Y);
                _gunPosition.X = _perso.Position.X + 15;
            }

            UpdateBullets();
            _vie.Play(animationCoeurs);
            _perso.Sprite.Play(animation);
            _gun.Play(animationGun);
            _perso.Sprite.Update(deltaSeconds);

            for (int i = 0; i < _monsters.Length; i++)//pour tout les mobs
            {
                _monsters[i].Sprite.Play(_monsters[i].Name);
                _monsters[i].Sprite.Update(deltaSeconds);
               
            }
           

            base.Update(gameTime);
        }

        public void UpdateBullets()
        {
            foreach(Bullets bullets in bullets)
            {
                bullets._bulletPosition += bullets.Vélocité;
                if (Vector2.Distance(bullets._bulletPosition, _perso.Position) > 800)
                    bullets.isVisible = false;
                _hitboxBullets = new Rectangle((int)bullets._bulletPosition.X, (int)bullets._bulletPosition.Y,HAUTEUR_BULLET, LARGEUR_BULLET);
                
                for (int i = 0; i < _monsters.Length; i++)//pour tout les mobs
                {
                    if (_monsters[i].Hitbox.Intersects(_hitboxBullets))
                    {
                        bullets.isVisible = false;
                        _monsters[i].Life--;
                        if (_monsters[i].Life == 0)
                        {
                            _score += _monsters[i].DefaultLife;

                            _monsters[i].Respawn(GraphicsDevice);
                            _monsters[i].Life = _monsters[i].DefaultLife;
                        }
                    }
                }
            }

            if(level >= levelLimit)
            {
              //fait rien
            }
            else if (_score/8  > level)
            {
                for (int i = 0; i < _monsters.Length; i++)//pour tout les mobs
                {
                    _monsters[i].Vitesse = (int)Math.Round(_monsters[i].Vitesse * 1.1, 0);
                }
                _perso.Vitesse = (int)Math.Round(_perso.Vitesse * 1.1, 0);
                level++;
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].isVisible)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
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
            _spriteBatch.Draw(_perso.Sprite, _perso.Position);
            _spriteBatch.Draw(_gun, _gunPosition, rotation);
            foreach (Bullets bullet in bullets)
                bullet.Draw(_spriteBatch);
            _spriteBatch.Draw(_backgroundBonesTexture, new Vector2(0, 0), Color.White);
            for (int i = 0; i < _monsters.Length; i++)
            {
                _spriteBatch.Draw(_monsters[i].Sprite, _monsters[i].Position);
                
            }
            
            _spriteBatch.DrawString(_textScore, $"Score : {_score}", _positionScore, Color.Black);
            _spriteBatch.Draw(_vie, _viePosition);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
