using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorld.Model.User
{
    public interface IAttackStrategy
    {
        void AttackTarget(Monster monster);

        void AddHP(Role role);
    }

    public class WoodSword : IAttackStrategy
    {
        //public void AttackTarget(Monster monster)
        //{
        //    monster.Notify(20);
        //}
        public void AttackTarget(Monster monster)
        {
            monster.Notify(20);
        }

        public void AddHP(Role role)
        {
            role.AddHP(20);
        }
    }


    /// <summary>  
    /// 角色  
    /// </summary>  
    public class Role
    {

        private int HP { get; set; }

        /// <summary>  
        /// 表示角色目前所持武器  
        /// </summary>  
        public IAttackStrategy Weapon { get; set; }

        /// <summary>  
        /// 攻击怪物  
        /// </summary>  
        /// <param name="monster">被攻击的怪物</param>  
        public void Attack(Monster monster)
        {
            this.Weapon.AttackTarget(monster);
        }

        public void AddHP(int hp)
        {
            this.HP = this.HP + hp;
        }
    }


    /// <summary>  
    /// 怪物  
    /// </summary>  
    public class Monster
    {
        /// <summary>  
        /// 怪物的名字  
        /// </summary>  
        public string Name { get; set; }

        /// <summary>  
        /// 怪物的生命值  
        /// </summary>  
        private int HP { get; set; }

        public Monster(string name, int hp)
        {
            this.Name = name;
            this.HP = hp;
        }

        /// <summary>  
        /// 怪物被攻击时，被调用的方法，用来处理被攻击后的状态更改  
        /// </summary>  
        /// <param name="loss">此次攻击损失的HP</param>  
        public void Notify(int loss)
        {
            if (this.HP <= 0)
            {
                Console.WriteLine("此怪物已死");
                return;
            }

            this.HP -= loss;
            if (this.HP <= 0)
            {
                Console.WriteLine("怪物" + this.Name + "被打死");
            }
            else
            {
                Console.WriteLine("怪物" + this.Name + "损失" + loss + "HP");
            }
        }
    }

}
