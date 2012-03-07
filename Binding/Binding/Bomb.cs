using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Binding
{
    class Bomb : Actor
    {
        Player spawningPlayer;
        int timer;
        int maxTimer = 125;
        bool exploding;
        int radius;

        public bool Exlpoding
        {
            get { return exploding; }
        }

        public Bomb(Vector2 pos, GameManager gm, Player sp)
            : base(pos, gm)
        {
            spawningPlayer = sp;
            exploding = false;
            radius = 75;
            width = 40;
            height = 40;
            ignorePlayerMovement = true;
        }

        public override void Update()
        {
            base.Update();
            if (timer < maxTimer)
            {
                timer++;
            }
            if (timer >= maxTimer - (maxTimer / 15))
            {
                exploding = true;
            }
            if (timer == maxTimer)
            {
                manager.ActorManager.Remove(this);
            }
        }

        public override void UpdateCollisions(List<Tile> tiles, List<Actor> actors, List<Player> players)
        {
            base.UpdateCollisions(tiles, actors, players);
            if (this.exploding)
            {
                foreach (Tile tile in tiles)
                {
                    if (tile.Type == TileType.rock
                        && InExplosionRadius(tile.Rect))
                    {
                        tile.Destroy();
                    }
                }
            }

            if (!spawningPlayer.GetRect().Intersects(GetHitRect()))
            {
                ignorePlayerMovement = false;
            }
        }

        public bool InExplosionRadius(Rectangle rect)
        {
            double circleX = Math.Abs(position.X - rect.X - (rect.Width / 2));
            double circleY = Math.Abs(position.Y - rect.Y - (rect.Height / 2));

            if (circleX > (GetHitRect().Width / 2 + radius)) { return false; }
            if (circleY > (GetHitRect().Height / 2 + radius)) { return false; }

            if (circleX <= (GetHitRect().Width / 2)) { return true; }
            if (circleY <= (GetHitRect().Height / 2)) { return true; }

            double cornerDistance = Math.Pow((circleX - GetHitRect().Width / 2), 2) +
                         Math.Pow((circleX - GetHitRect().Height / 2), 2);

            return (cornerDistance <= (Math.Pow(radius, 2)));
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(GameConst.blank, GetHitRect(), Color.Blue);
            
            base.Draw(sb);
        }
    }
}
