using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE
{
    public class Squelette
    {
        private Vector2 position;
        private AnimatedSprite sprite;
        private Rectangle hitbox;
        private int vitesse;
        private int life;
        private string animation;

        public Squelette(int vitesse, int life,int height, int width, Vector2 position)
        {
            Vitesse = vitesse;
            Life = life;
            Hitbox = new Rectangle(0, 0, width, height);
            Position = position;
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
                hitbox.X = (int)Math.Round(value.X, 0);
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
        public string Animation
        {
            get
            {
                return animation;
            }
            set
            {
                animation = value;
            }
        }

        public Vector2 Move(string animation, Vector2 _gunPosition,float deltaSeconds)
        {
            float walkSpeed = deltaSeconds * this.Vitesse;
            this.Animation = animation;
            float moveX = 0;
            float moveY = 0;
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                /*animation = "marche gauche";
                animationGun = "gun gauche";
                gunRotationPosition = - 15;*/
                Animation = "marche droite";
                moveX -= walkSpeed;
                _gunPosition.X -= walkSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                moveY -= walkSpeed;
                _gunPosition.Y -= walkSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                moveY += walkSpeed;
                _gunPosition.Y += walkSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                Animation = "marche droite";
                moveX += walkSpeed;
                _gunPosition.X += walkSpeed;
            }
            this.Position = new Vector2(this.Position.X + moveX, this.Position.Y + moveY);
            return _gunPosition;
        }
    }
}
