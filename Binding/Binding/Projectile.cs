using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Binding
{
    class Projectile : Entity
    {
        int baseDamage;
        float speed;
        Vector2 velocity;
        int range;
        int rangeTimer;
        int damage;
        bool damageEnemies = true;

        public int Damage
        {
            get { return damage; }
        }

        public Rectangle GetHitRect()
        {
            return new Rectangle((int)position.X - (width / 2), (int)position.Y - height - 20,
                     width, height);
        }

        public Rectangle GetRect()
        {
            return new Rectangle((int)position.X - (width / 2), (int)position.Y - height,
                     width, height);
        }

        public Projectile(Vector2 pos, Vector2 spd, Vector2 dir, GameManager m, int r, int d)
            : base(pos, m)
        {
            width = 15;
            height = 15;
            speed = 6;
            baseDamage = 1;
            range = r;
            damage = d;

            if (dir.X > 0)
            {
                spd.X = Math.Max(spd.X, 0);
                position.Y += (float)((GameConst.random.NextDouble() * 12) - 6);
            }
            else if (dir.X < 0)
            {
                spd.X = Math.Min(spd.X, 0);
                position.Y += (float)((GameConst.random.NextDouble() * 12) - 6);
            }
            else if (dir.Y < 0)
            {
                spd.Y = Math.Min(spd.Y, 0);
                position.X += (float)((GameConst.random.NextDouble() * 12) - 6);
            }
            else
            {
                spd.Y = Math.Max(spd.Y, 0);
                position.X += (float)((GameConst.random.NextDouble() * 12) - 6);
            }

            velocity = new Vector2((dir.X * speed) + (spd.X / 1.75f), (dir.Y * speed) + (spd.Y / 1.75f));
        }

        public Projectile(Vector2 pos, Vector2 spd, GameManager m, int r, int d)
            : base(pos, m)
        {
            width = 15;
            height = 15;
            speed = 6;
            baseDamage = 1;
            range = r;
            damage = d;
            damageEnemies = false;

            velocity = new Vector2( (spd.X / 1.75f), (spd.Y / 1.75f));
        }

        #region ROZ!!
        //Hi duncan. It's your favorite  African American again!
        //I'm just here to say that I <3 you! And that I love writting
        //in your code.
        //Because it's fun!
        //And you are a fun dude.
        //So thats like, double the fun!
        //And I'm fun too!
        //So thats like, triple the fun!
        //ALL the fun!
        //I'm hungry...
        //I should probably get food...
        //I really want wings but I can't find cohoe.
        //Jew is giving me a message!
        //It is nice!
        //But not as nice as you!
        //<3
        //I shoould probably let you get back to your code now.
        //Okay Ima stop now!
        //Bai Duncan!
        //<3
        //Roz
        #endregion

        public void Destroy()
        {
            manager.BulletManager.Remove(this);
        }

        public override void Update()
        {

            if (rangeTimer < range)
            {
                rangeTimer++;
            }

            if (range == rangeTimer)
            {
                Destroy();
            }

            position.X += velocity.X;
            position.Y += velocity.Y;
        }

        public override void UpdateCollisions(List<Tile> tiles, List<Actor> actors, List<Player> players)
        {
            
            foreach (Tile tile in tiles)
            {

                if (GetRect().Intersects(tile.Rect))
                {
                    Destroy();
                }
               
            }

            foreach (Actor actor in actors)
            {
                if (damageEnemies)
                {
                    if (GetHitRect().Intersects(actor.GetHitRect()))
                    {
                        Destroy();

                        actor.SpeedX += velocity.X / 2;
                        actor.SpeedY += velocity.Y / 2;
                        if (actor is Enemy)
                        {
                            Enemy enemy = (Enemy)actor;

                            enemy.Hit(this);
                        }
                    }
                }
            }

            foreach (Player player in players)
            {
                if (!damageEnemies)
                {
                    if (GetHitRect().Intersects(player.GetHitRect()))
                    {
                        Destroy();

                        player.SpeedX += velocity.X / 2;
                        player.SpeedY += velocity.Y / 2;
                        
                    }
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            sb.Draw(GameConst.blank, GetHitRect(), Color.Black);
        }
    }
}
