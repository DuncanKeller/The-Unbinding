using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Binding
{
    abstract class Entity
    {
        protected GameManager manager;
        protected Vector2 position;
        protected int width;
        protected int height;


        public Entity(Vector2 pos, GameManager m)
        {
            position = pos;
            manager = m;
        }

        public abstract void Update();

        public abstract void UpdateCollisions(List<Tile> tiles, List<Actor> actors, List<Player> players);

        public virtual void Draw(SpriteBatch sb)
        {
            
        }
    }
}
