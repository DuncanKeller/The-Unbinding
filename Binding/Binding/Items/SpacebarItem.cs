using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Binding
{
    abstract class SpacebarItem
    {
        string name;
        protected Player player;
        protected GameManager manager;
        protected int charge;
        protected int chargeTime = 4;
        const int maxCharge = 8;

        public bool IsCharged
        {
            get
            {
                return charge == maxCharge;
            }
        }

        public SpacebarItem(Player p, GameManager m)
        {
            player = p;
            manager = m;
            charge = maxCharge;
        }

        public void DrainCharge()
        {
            charge = 0;
        }

        public void Charge()
        {
            if (charge < maxCharge)
            {
                charge += chargeTime;
            }
            if (charge > maxCharge)
            {
                charge = maxCharge;
            }
        }

        public virtual void Activate()
        {
            charge = 0;
        }
    }
}
