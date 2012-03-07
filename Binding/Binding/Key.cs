using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Binding
{
    class Key : Actor
    {

        public Key(Vector2 pos, GameManager gm)
            : base(pos, gm)
        {
            width = 25;
            height = 40;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void UpdateCollisions(List<Tile> tiles, List<Actor> actors, List<Player> players)
        {
            base.UpdateCollisions(tiles, actors, players);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(GameConst.blank, GetRect(), Color.SkyBlue);

            base.Draw(sb);
        }
    }
}
