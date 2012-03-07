using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Binding
{
    class ActorManager<T> where T : Entity
    {
        Queue<T> toAdd = new Queue<T>();
        List<T> items = new List<T>();
        Queue<T> toDispose = new Queue<T>();
        GameManager manager;

        public ActorManager(GameManager m)
        {
            manager = m;
        }

        public List<T> Items
        {
            get
            {
                return items;
            }
        }

        public void DisposeEntities()
        {
            foreach (T e in toDispose)
            {
                if (items.Contains(e))
                { items.Remove(e); }
            }
            toDispose.Clear();
        }

        public void AddEntities()
        {
            foreach (T e in toAdd)
            {
                items.Add(e);
            }
            toAdd.Clear();
        }

        public void Add(T e)
        {
            toAdd.Enqueue(e);
        }

        public void AddMultiple(List<T> es)
        {
            foreach (T e in es)
            {
                toAdd.Enqueue(e);
            }
        }

        public void Remove(T e)
        {
            toDispose.Enqueue(e);
        }

        public void Update(List<Tile> tiles, List<Actor> actors, List<Player> players)
        {
            DisposeEntities();
            AddEntities();

            foreach (T e in items)
            {
                if (e is Actor)
                {
                    if ((e as Actor).CurrentRoom == manager.Map.CurrentRoom)
                    {
                        e.Update();
                        e.UpdateCollisions(tiles, actors, players);
                    }
                }
                else
                {
                    e.Update();
                    e.UpdateCollisions(tiles, actors, players);
                }
            }
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            foreach (T a in items)
            {
                a.Draw(sb);
            }
        }

    }
}
