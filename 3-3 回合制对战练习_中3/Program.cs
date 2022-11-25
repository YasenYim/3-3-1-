using System;
using System.Collections.Generic;

namespace _3_3_回合制对战练习_中3
{
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
            Skill eskill1 = new Skill("泪珠", SkillType.NormalAttack, 10000);
            Skill eskill2 = new Skill("治疗", SkillType.Heal,true, 7000);
            enemy.AddSkill(eskill1);
            enemy.AddSkill(eskill2);

            int round = 1;
            while(true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"---------第{round}局---------");
                // 选择技能
                Console.ForegroundColor = ConsoleColor.Green;
                Skill playerSkill = ChooseSkill(player);
                // 玩家攻击敌人
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
                    Console.WriteLine($"{player}战败了~");
                    break;
                }


                // 选择技能
                Console.ForegroundColor = ConsoleColor.Red;
                Skill enemySkill = RandomSkill(enemy);
                // 敌人攻击玩家
                if (enemySkill.self)
                {
                    enemy.Attack(enemySkill, enemy);
                }
                else
                {
                    enemy.Attack(enemySkill, player);
                }
                // 判断死亡
                if (enemy.IsDead())
                {
                    Console.WriteLine($"{enemy}战败了~");
                    break;
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
                Console.WriteLine($"恭喜{player}获得了胜利");
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
            // 打印所有的技能
            for (int i = 0; i < cha.skills.Count; i++)
            {
                Skill skill = cha.skills[i];
                Console.WriteLine($"{i + 1}.{skill.name}");
            }
            // 输入
            int index = -1;
            while (index < 0 || index >= cha.skills.Count)
            {
                index = InputNum() - 1;
            }
            // 选择技能并返回
            Skill choose = cha.skills[index];
            return choose;
        }
    }
}
