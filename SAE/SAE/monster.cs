using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;


namespace SAE
{
    public class Monster
    {
        private Vector2 position;
        private AnimatedSprite sprite;
        private Rectangle hitbox;
        private int vitesse;
        private int defaultLife;
        private int life;
        private string name;

        public Monster(int vitesse, int defaultLife, string name,int height,int width)
        {
            Vitesse = vitesse;
            DefaultLife = defaultLife;
            Life = defaultLife;
            Name = name;
            Hitbox = new Rectangle(0, 0, width, height);
            Position = new Vector2(0, 0);
        }

        public Vector2 Position 
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                hitbox.X = (int)Math.Round(value.X,0);
                hitbox.Y = (int)Math.Round(value.Y, 0);
            }
        }
        public AnimatedSprite Sprite
        {
            get
            {
                return sprite;
            }
            set
            {
                sprite = value;
            }
        }
        public Rectangle Hitbox
        {
            get
            {
                return hitbox;
            }
            set
            {
                hitbox = value;
            }
        }
        public int Vitesse
        {
            get
            {
                return vitesse;
            }
            set
            {
                vitesse = value;
            }
        }
        public int DefaultLife
        {
            get
            {
                return defaultLife;
            }
            set
            {
                defaultLife = value;
            }
        }
        public int Life
        {
            get
            {
                return life;
            }
            set
            {
                life = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public void LoadSprite(SpriteSheet spriteSheet)
        {
            Sprite = new AnimatedSprite(spriteSheet, Name);
        }
        public void Move(Squelette cible,float deltaSeconds, GraphicsDevice graphicsDevice)
        {
            float moveX;
            float moveY;
            float diagonal;
            if (this.Position.X == cible.Position.X || this.Position.Y == cible.Position.Y)
            {
                diagonal = 1;
            }
            else
            {
                diagonal = (float)Math.Sqrt((this.Vitesse * this.Vitesse) / 2) / this.Vitesse;
            }

            if (this.Position.X > cible.Position.X)
            {
                moveX = -(float)this.Vitesse * diagonal * deltaSeconds;
            }
            else if (this.Position.X < cible.Position.X)
            {
                moveX = (float)this.Vitesse * diagonal * deltaSeconds;
            }
            else
            {
                //sur la même ligne pas besoin de bouger
                moveX = 0;
            }

            if (this.Position.Y > cible.Position.Y)
            {
                moveY = -(float)this.Vitesse * diagonal * deltaSeconds;
            }
            else if (this.Position.Y < cible.Position.Y)
            {
                moveY = (float)this.Vitesse * diagonal * deltaSeconds;
            }
            else
            {
                //sur la même colonne pas besoin de bouger
                moveY = 0;
            }
            this.Position = new Vector2(this.Position.X + moveX, this.Position.Y + moveY);

            if (this.Hitbox.Intersects(cible.Hitbox))
            {
                cible.Life--;
                this.Respawn(graphicsDevice);
            }

        }
        public void Respawn(GraphicsDevice graphicsDevice)
        {
            Random fantomey = new Random();
            int positionFantomeY = fantomey.Next(0, graphicsDevice.Viewport.Height);
            int cote = fantomey.Next(0, 2);
            if (cote == 0)
            {
                this.Position = new Vector2(0 - this.Sprite.TextureRegion.Width, positionFantomeY);
            }
            else
            {
                this.Position = new Vector2(graphicsDevice.Viewport.Width, positionFantomeY);
            }
        }
    }
}
