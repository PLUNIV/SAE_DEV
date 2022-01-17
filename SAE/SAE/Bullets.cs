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
    class Bullets
    {

        
        public Texture2D _bullet;
        public Vector2 _bulletPosition;
        public Vector2 Vélocité;
        public Vector2 origine;

        public bool isVisible;

        public Bullets(Texture2D _bulletTexture)
        {
            //_bullet = Content.Load<Texture2D>("Battleground4");
            _bullet = _bulletTexture;
            isVisible = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_bullet, _bulletPosition, null, Color.White, 0f, origine, 1f, SpriteEffects.None, 0);
        }

    }
}
