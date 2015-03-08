using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CounterNinja.Unit
{
    public class HealthCounter
    {
        private int curHealth;
        private int maxHealth;
        private int lives;
        private bool invincible;
        private bool justHit;

        /// <summary>
        /// Creates a basic health counter.
        /// Using -1 for both health and lives will put 
        /// the counter into an invincible state, which does not take heal or damage 
        /// amount.
        /// </summary>
        /// <param name="health"></param>
        /// <param name="lives"></param>
        public HealthCounter(int lives, int health)
        {
            curHealth = health;
            maxHealth = health;
            this.lives = lives;
            invincible = health == -1 && lives == -1;
        }

        #region Accessor methods

        public bool JustHit
        {
            get { return justHit; }
            set { justHit = value; }
        }

        public bool IsDead
        {
            get { return lives == 0 && curHealth == 0; }
        }

        public bool Invincible
        {
            get { return invincible; }
            set { invincible = value; }
        }

        public int Amount
        {
            get { return curHealth; }
            set { curHealth = value; }
        }

        public int Max
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        #endregion

        /// <summary>
        /// Takes damage equal to amount. If overdamage is done,
        /// it will calculate into the lives and take lives until 
        /// the damage is paid for.
        /// </summary>
        /// <param name="amount">Amount to damage. Default is 1.</param>
        public void TakeDamage(int amount = 1)
        {
            if (invincible)
                return;
            justHit = true;
            curHealth -= amount;
            while (curHealth < 0 && lives > 0)
            {
                lives--;
                curHealth += maxHealth;
            }
            if (curHealth < 0)
                curHealth = 0;
        }

        /// <summary>
        /// Takes heal amount. If overhealing is done,
        /// the extra health turns into lives depending
        /// on how much health was added.
        /// </summary>
        /// <param name="amount">Amount to heal. Default is 1.</param>
        public void TakeHeal(int amount = 1)
        {
            if (invincible)
                return;
            curHealth += amount;
            while (curHealth > maxHealth)
            {
                lives++;
                curHealth -= maxHealth;
            }
        }
    }
}
