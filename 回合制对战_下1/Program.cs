using System;
using System.Collections.Generic;
/* 
* 玩家与敌人对战。玩家可选择行动，敌人自己选择每回合的行动
* 每回合双方各行动一次，生命值先达到0的一方为失败
* 伤害公式：攻击力*（1-防御力/（300+防御力））
*/

/* 遇到大型的复杂的问题，先考虑将项目分阶段拆解
 * 1、先制作一个简化版的对战程序（之前的练习中已经做过）
 * 2、根据新的需求，给类型加入必要的字段，通过字段理清思路
 * 3、一个接着一个实现更多功能。在必要时，调整游戏的整体逻辑
 * 其次是在每一个阶段中，将功能分块拆解
 */
namespace 回合制对战_下1
{
   
    class Program
    {

        // 敌人选择技能的函数
        public static Skill ChooseSkill(Character cha)
        {
            // 打印角色的技能列表
            for (int i = 0; i < cha.skills.Count; i++)
            {
                Skill skill = cha.skills[i];
                Console.WriteLine($"{i + 1}.{skill.name}");
            }

            // 让用户输入技能序号
            int index = -1;
            while (index < 0 || index >= cha.skills.Count)
            { index = InputNum() - 1; }

            // 选中技能并返回技能
            Skill choose = cha.skills[index];
            return choose;
        }

        // 输入数字，如果错误重新输入
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
                    continue;
                }

                return num;
            }
        }
        static Skill RandomSkill(Character cha)
        {
            int index = Utils.random.Next(0,cha.skills.Count);
            return cha.skills[index];
        }

        static void PrintState(Character cha)
        {
            for (int i = 0; i < cha.states.Count; i++)
            {
                State state = cha.states[i];
                Console.Write(state + " ");
            }
            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            Character player = new Character("三尾狐", 875, 124, 68, 1000);
            Skill skill1 = new Skill("尾袭",SkillType.NormalAttack,10000);
            Skill skill2 = new Skill("诱惑", SkillType.SuckBlood, 8000,2000);
            Skill skill3 = new Skill("治愈之光", SkillType.HealOverTime, 5000, 2200,3);
            player.AddSkill(skill1);
            player.AddSkill(skill2);
            player.AddSkill(skill3);

            Character enemy = new Character("雨女", 1035, 97, 73, 500);
            Skill eskill1 = new Skill("泪珠", SkillType.NormalAttack,10000);
            Skill eskill2 = new Skill("治疗", SkillType.Heal, true,7000);
            enemy.AddSkill(eskill1);
            enemy.AddSkill(eskill2);

            int round = 1;

            //战斗循环
            while(true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"--------第{round}回合--------");

                // 显示角色身上的状态
                Console.ForegroundColor = ConsoleColor.Gray;
                PrintState(player);

                // 玩家攻击敌人
                Console.ForegroundColor = ConsoleColor.Green;

                // 行动之前，状态生效
                player.StatesEffect();

                // 选择技能
                //Skill playerSkill = player.skills[0];
                Skill playerSkill = ChooseSkill(player);
                if(playerSkill.self)
                { player.Attack(playerSkill, player); }
                else
                { player.Attack(playerSkill, enemy); }

                // 判断敌人是否死亡
                if(enemy.IsDead())
                { Console.WriteLine($"{enemy.name}战败了！");break; }

                // 显示角色身上的状态
                Console.ForegroundColor = ConsoleColor.Gray;
                PrintState(enemy);

                // 敌人攻击玩家
                Console.ForegroundColor = ConsoleColor.Red;

                // 行动之前，状态生效
                enemy.StatesEffect();

                // 选择技能
                //Skill enemySkill = enemy.skills[0];
                Skill enemySkill = RandomSkill(enemy);
                if (enemySkill.self)
                { enemy.Attack(enemySkill, enemy); }
                else
                { enemy.Attack(enemySkill, player); }

                // 判断敌人是否死亡
                if(player.IsDead())
                { Console.WriteLine($"{player.name}战败了！");break; }
            }

            Console.ForegroundColor = ConsoleColor.Gray;

            if(player.IsDead())
            { Console.WriteLine($"{player.name}战败了，游戏结束。"); }
            else
            { Console.WriteLine($"恭喜{player.name}获得了胜利！"); }

            Console.ReadKey();
        }
    }
}
