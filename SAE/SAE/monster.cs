using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;


namespace SAE
{
    public class monster
    {
        private Vector2 position;
        private AnimatedSprite sprite;
        private Rectangle hitbox;
        private int vitesse;
        private int defaultLife;
        private int life;
        private string name;

        public monster(Vector2 position, AnimatedSprite sprite, int vitesse, int defaultLife, string name,int height,int width)
        {
            Position = position;
            Sprite = sprite;
            Vitesse = vitesse;
            DefaultLife = defaultLife;
            Life = defaultLife;
            Name = name;
            Hitbox = new Rectangle((int)position.X, (int)position.Y, width, height);
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
    }
}
