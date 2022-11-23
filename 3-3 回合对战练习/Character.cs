using System;
using System.Collections.Generic;
using System.Text;

namespace _3_3_回合对战练习
{
    public enum SkillType
    {
        NormalAttack,         
        SuckBlood,
        Heal,           
    }

    class Utils
    {
        public static Random random = new Random();
    }

    class Character
    {
        public string name { get; private set; }
        public int hp { get; private set; }
        public int maxHp { get; private set; }
        public int attack { get; private set; }
        public int def { get; private set; }

        public int critChance;
        public int critRate = 20000;
        public List<Skill> skills { get; private set; }
        public Character(string name, int hp, int attack, int def, int critChance)
        {
            this.name = name;
            this.maxHp = hp;
            this.hp = maxHp;
            this.attack = attack;
            this.def = def;
            this.critChance = critChance;
            skills = new List<Skill>();
        }
        public void CostHp(int cost)
        {
            this.hp -= cost;
            if (this.hp <= 0)
            { this.hp = 0; }
        }
        public void AddHp(int add)
        {
            hp += add;
            if(this.hp>maxHp){this.hp = maxHp;}
            //hp = Math.Min(hp, maxHp);
        }

        public void AddSkill(Skill skill)
        {
            skills.Add(skill);
        }

        public void Attack(Skill skill, Character other)
        {
            switch (skill.type)
            {
                case SkillType.NormalAttack:
                    {
                        float a = attack * (skill.data1 / 10000f);
                        int damage = (int)(a * (1 - (float)other.def / (300 - other.def)));
                        other.CostHp(damage);

                        Console.WriteLine($"{this.name}使用了{skill.name}对{other.name}造成了{damage}点伤害~");
                        Console.WriteLine($"{other.name} 体力{other.hp}/{other.maxHp}");
                    }
                    break;
                case SkillType.SuckBlood:
                    {
                        float a = attack * (skill.data1 / 10000f);
                        int damage = (int)(a * (1 - (float)other.def / (300 - other.def)));

                        int suck = (int)(damage * (skill.data2 / 10000f));
                        other.CostHp(damage);
                        this.AddHp(suck);
                        Console.WriteLine($"{this.name}使用了{skill.name}对{other.name}造成了{damage}点伤害");
                        Console.WriteLine($"{other.name} 体力{other.hp}/{other.maxHp}");
                        Console.WriteLine($"{name}吸取了{suck}点生命。 体力：{hp}/{maxHp}");
                    }
                    break;
                case SkillType.Heal:
                    {
                        int heal = (int)(attack * (skill.data1 / 10000f));
                        other.AddHp(heal);

                        Console.WriteLine($"{this.name}使用了治疗技能，治疗{heal}点生命力~~");
                        Console.WriteLine($"{this.name} 体力：{hp}/{maxHp}");
                    }
                    break;
            }
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

        public bool self = false;

        // 普攻,攻击比例,万分比
        // 治疗,吸取对方生命力的百分比
        public int data1;
        // 吸血,吸取对方生命力的万分比
        public int data2;   


        public Skill(string name,SkillType type,int data1, int data2=0)
        {
            this.name = name;
            this.type = type;
            this.data1 = data1;
            this.data2 = data2;
        }

        public Skill(string name, SkillType type,bool self, int data1, int data2 = 0)
        {
            this.name = name;
            this.type = type;
            this.self = self;
            this.data1 = data1;
            this.data2 = data2;
        }
    }

}
