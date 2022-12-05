using System;
using System.Collections.Generic;
using System.Text;

namespace 回合制对战_下1
{
    // 技能类型
    public enum SkillType
    {
        NormalAttack,  // 普攻
        SuckBlood,     // 吸血
        Heal,          // 治疗
        HealOverTime,
        DamageOverTime,
    }

    // 状态类型
    public enum StateType
    {
        Heal,
        Damage,
    }
    class Utils
    {
        public static Random random = new Random();
    }
    class Character
    {
        public string name;
        public int hp;
        public int maxHp;
        public int attack;
        public int def;
        public int critChance;
        public int critRate = 20000;
        public List<Skill> skills { get; private set; }
        public List<State> states { get; private set; }
        public Character(string name, int hp, int attack, int def, int critChance)
        {
            this.name = name;
            this.maxHp = hp;
            this.hp = maxHp;
            this.def = def;
            this.attack = attack;
            this.critChance = critChance;
            skills = new List<Skill>();
            states = new List<State>();
        }
        public void CostHp(int cost)
        {
            this.hp -= cost;
            if (this.hp <= 0)
            { this.hp = 0; }
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

                        Console.WriteLine($"{this.name}使用{skill.name}对{other.name}造成{damage}点伤害。");
                        Console.WriteLine($"{other.name} 体力：{other.hp}/{other.maxHp}");
                    }
                    break;
                case SkillType.SuckBlood:
                    {
                        float a = attack * (skill.data1 / 10000f);
                        int damage = (int)(a * (1 - (float)other.def / (300 - other.def)));

                        int suck = (int)(damage * (skill.data2 / 10000f));
                        other.CostHp(damage);
                        this.AddHp(suck);
                        Console.WriteLine($"{this.name}使用{skill.name}对{other.name}造成{damage}点伤害。");
                        Console.WriteLine($"{other.name} 体力：{other.hp}/{other.maxHp}");
                        Console.WriteLine($"{name} 吸取了{suck}点体力。 体力：{hp}/{maxHp}");
                    }
                    break;
                case SkillType.Heal:
                    {
                        int heal = (int)(attack * (skill.data1 / 10000f));
                        other.AddHp(heal);

                        Console.WriteLine($"{name}使用了{skill.name}技能，治疗{heal}点体力~~");
                        Console.WriteLine($"{name} 体力：{hp}/{maxHp}");
                    }
                    break;
                case SkillType.HealOverTime:
                    {
                        int heal = (int)(attack * (skill.data1 / 10000f));
                        other.AddHp(heal);

                        // 添加状态
                        State state = new State(StateType.Heal, skill.time, skill.data2);
                        state.name = skill.name;
                        states.Add(state);


                        Console.WriteLine($"{name}使用了治疗技能，治疗{heal}点体力,并加上了{state.name}状态");
                        Console.WriteLine($"{name} 体力：{hp}/{maxHp}");
                    }
                    break;
            }
        }

        public void AddHp(int add)
        {
            this.hp += add;
            if (hp > maxHp) { this.hp = maxHp; }
        }

        // 状态生效
        public void StatesEffect()
        {
            for(int i = 0;i<states.Count;i++)
            {
                State state = states[i];

                switch (state.type)
                {
                    case StateType.Damage:
                        {
                            float a = attack * (state.data1 / 10000f);
                            int damage = (int)(a * (1 - (float)def / (300 - def)));
                            this.CostHp(damage);
                            Console.WriteLine($"{state.name}生效，损失体力{damage},{hp}/{maxHp}");
                        }
                        break;
                    case StateType.Heal:
                        {
                            int heal = (int)(attack * (state.data1 / 10000f)); 
                            this.AddHp(heal);
                            Console.WriteLine($"{state.name}生效，回复体力{heal},{hp}/{maxHp}");
                        }
                        break;
                }
            }
            // 状态时间-1，为0的要删除
            for(int i = states.Count-1;i>=0; i--)
            {
                State state = states[i];
                state.time--;
                if(state.time<=0)
                { states.RemoveAt(i); }
            }
        }
        public bool IsDead()
        {
            return this.hp <= 0;
        }
    }


    class Skill
    {
        public string name;
        public SkillType type;
        public bool self = false;
        // 普攻：攻击比例，万分比
        // 治疗：治疗比例，万分比
        // HOT: 直接治疗的万分比
        public int data1;

        // 吸血：吸取对方生命力，万分比
        // HOT:持续治疗的万分比
        public int data2;

        // 状态技能，状态的持续时间
        public int time;   // 技能上记录一个时间，传给state状态

        public Skill(string name, SkillType type, int data1,int data2 = 0,int time=0)
        {
            this.name = name;
            this.type = type;
            this.data1 = data1;
            this.data2 = data2;
            this.time = time;
        }
        public Skill(string name, SkillType type, bool self,int data1, int data2=0,int time=0)
        {
            this.name = name;
            this.type = type;
            this.self = self;
            this.data1 = data1;
            this.data2 = data2;
            this.time = time;
        }
    }

    // 状态类
    class State
    {
        public StateType type;
        public int time;       // 状态持续时间
        public int data1;      // 治疗量 万分比（这里是对应的22%）
        public string name;    // 状态名称
        public State(StateType type,int time,int data1)
        {
            this.type = type;
            this.time = time;
            this.data1 = data1;
        }

        public override string ToString()
        { return $"{name}({time})"; }
    }
}
