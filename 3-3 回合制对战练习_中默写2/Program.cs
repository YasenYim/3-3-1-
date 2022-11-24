using System;
using System.Collections.Generic;

namespace _3_3_回合制对战练习_中默写2
{
    public enum SkillType
    {
        NormalAttack,
        SuckBlood,
        Heal,
    }
    class Skill
    {
        public string name;
        public SkillType type;
        public int data1;
        public int data2;
        public bool self = false;
        public Skill(string name, SkillType type, bool self, int data1,int data2=0)
        {
            this.name = name;
            this.type = type;
            this.self = self;
            this.data1 = data1;
            this.data2 = data2;
        }
        public Skill(string name, SkillType type, int data1, int data2 = 0)
        {
            this.name = name;
            this.type = type;
            this.data1 = data1;
            this.data2 = data2;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Character player = new Character("三尾狐", 875, 124, 68, 1000);
            Skill skill1 = new Skill("尾袭", SkillType.NormalAttack, 10000);
            Skill skill2 = new Skill("诱惑", SkillType.SuckBlood, 8000, 2000);
            player.AddSkill(skill1);
            player.AddSkill(skill2);

            Character enemy = new Character("雨女", 1035, 97, 73, 500);
            Skill eskill1 = new Skill("泪珠",SkillType.SuckBlood,10000);
            Skill eskill2 = new Skill("治疗", SkillType.Heal,true, 7000);
            enemy.AddSkill(eskill1);
            enemy.AddSkill(eskill2);

            int round = 1;
            while(true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"-------第{round}局--------");
             
                // 玩家攻击敌人
                Console.ForegroundColor = ConsoleColor.Green;
                // 选择技能
                Skill playerSkill = ChooseSkill(player);
                if (playerSkill.self)
                {
                    player.Attack(playerSkill, player);
                }
                else
                {
                    player.Attack(playerSkill, enemy);
                }
                // 判断死亡
                if (player.IsDead())
                {
                    Console.WriteLine($"{player}战败了");
                    break;
                }

               
                // 敌人攻击玩家
                Console.ForegroundColor = ConsoleColor.Red;
                // 选择技能
                Skill enemySkill = RandomSkill(enemy);
                if (playerSkill.self)
                {
                    enemy.Attack(enemySkill, enemy);
                }
                else
                {
                    player.Attack(enemySkill, player);
                }
                // 判断死亡
                if(enemy.IsDead())
                {
                    Console.WriteLine($"{enemy}战败了");
                }

            }
            Console.ForegroundColor = ConsoleColor.Gray;

            if (player.IsDead())
            {
                Console.WriteLine($"{player}战败了");
            }
            else
            {
                Console.WriteLine($"{enemy}战败了");
                Console.WriteLine($"{player}获得了胜利");
            }

            Console.ReadKey();
        }

      
        public static Skill RandomSkill(Character cha)
        {
            int index = Utils.random.Next(0, cha.skills.Count);
            return cha.skills[index];
        }

        static int InputNum()
        {
            while (true)
            {
                Console.WriteLine("请输入一个数字：");
                string input = Console.ReadLine();

                int num;
                bool success = int.TryParse(input, out num); // out输出参数

                if (!success)
                {
                    Console.WriteLine("请重新输入");
                    continue;    // continue的意思是不再执行循环体后面的内容，直接开始下一次循环string input  = Console.ReadLine();如果成功了就会return num;
                }

                return num;
            }
        }
        public static Skill ChooseSkill(Character cha)
        {
            // 打印所有的技能名字
            for (int i = 0;i < cha.skills.Count;i++)
            {
                Skill skill = cha.skills[i];
                Console.WriteLine($"{i + 1}.{skill.name}");
            }
            // 让用户输入数字
            int index = -1;
            while(index <0 ||index >= cha.skills.Count)
            {
                index = InputNum() - 1;
            }
            // 选中技能并返回技能
            Skill choose = cha.skills[index];
            return choose;
        }
    }
}
