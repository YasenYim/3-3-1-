using System;
using System.Collections.Generic;

namespace _3_3_回合制对战练习
{
    public enum SkillType
    {
        NormalAttack,
    }
    class Character
    {
        public string name { get; private set; }
        public int hp { get; private set; }
        public int maxHp { get; private set; }
        public int attack { get; private set; }
        public int def { get; private set; }
        public int critChance;          // 暴击率
        public int critRate = 20000;    // 暴击倍数
        public List<Skill> skills { get; private set; }
        public Character(string name,int hp,int attack,int def ,int critChance)
        {
            this.name = name;
            this.maxHp = hp;
            this.hp = maxHp;
            this.attack = attack;
            this.def = def;
            this.critChance = critChance;
            skills = new List<Skill>();
        }
        public void AddSkill(Skill skill)
        {
            skills.Add(skill);
        }

        public void CostHp(int cost)
        {
            this.hp -= cost;
            if (this.hp <= 0)
            { this.hp = 0; }
        }

        public void Attack(Skill skill, Character other)
        {
            float a = attack * (skill.data1 / 10000f);
            int damage = (int)(a * (1 - (float)other.def / (300 - other.def)));

            other.CostHp(damage);
            Console.WriteLine($"{this.name}使用{skill.name}对{other.name}造成了{damage}点伤害~");
            Console.WriteLine($"{other.name} 体力{other.hp}/{other.maxHp}");
           
        }
        public bool IsDead()
        {
            return hp <= 0;
        }
    }

    class Skill
    {
        public string name;
        public SkillType type;
        public int data1;
    
        public Skill(string name,SkillType type,int data1)
        {
            this.name = name;
            this.type = type;
            this.data1 = data1;
        }
    }
    
    class Program
    {
       
        static void Main(string[] args)
        {
            Character player = new Character("三尾狐" ,875, 124, 68, 1000);
            Skill skill1 = new Skill("尾袭", SkillType.NormalAttack, 10000);
            player.AddSkill(skill1);
           
            Character enemy = new Character("雨女", 1035, 97, 73, 500);
            Skill eskill1 = new Skill("泪珠", SkillType.NormalAttack, 10000);
            enemy.AddSkill(eskill1);

            int round = 1;
            while(true)
            {
                Console.WriteLine($"-------这是比赛的第{round}局--------");

                // 玩家攻击敌人
                // 选择技能
                Skill playerSkill = player.skills[0];
                player.Attack(playerSkill, enemy);
                if(enemy.IsDead())
                {
                    Console.WriteLine($"{enemy.name}战败了~");
                    break;
                }
                // 敌人攻击玩家
                // 选择技能
                Skill enemySkill = enemy.skills[0];
                enemy.Attack(enemySkill, player);
                if (player.IsDead())
                {
                    Console.WriteLine($"{player.name}战败了~");
                    break;
                }
            }
            if(player.IsDead())
            {
                Console.WriteLine($"{player.name}战败了，游戏结束。");
            }
            else
            {
                Console.WriteLine($"{enemy.name}战败了.");
                Console.WriteLine($"恭喜{player.name}获得胜利！");
            }
            Console.ReadKey();
        }
    }          
}
