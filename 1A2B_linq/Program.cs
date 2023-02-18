using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1A2B_linq
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("歡迎來到 1A2B 猜數字的遊戲～");
            Random random = new Random();
            int[] answer = Enumerable.Range(0, 10).OrderBy(_ => random.Next()).Take(4).ToArray();


            while (true)
            {
                Console.Write("請輸入 4 個數字：");
                string input = Console.ReadLine();
                if (input.Length != 4)
                {
                    Console.WriteLine("輸入格式錯誤，請重新輸入！");
                    continue;
                }

                int A = 0;
                int B = 0;
                for (int i = 0; i < 4; i++)
                {
                    int number = int.Parse(input[i].ToString());
                    if (answer[i] == number)
                    {
                        A++;
                    }
                    else if (answer.Contains(number))
                    {
                        B++;
                    }
                }

                Console.WriteLine("判定結果是 " + A + "A" + B + "B");
                if (A == 4)
                {
                    Console.WriteLine("恭喜你！猜對了！！");
                    break;
                }
            }

            Console.Write("你要繼續玩嗎？(y/n): ");
            string continueGame = Console.ReadLine();
            if (continueGame != "y")
            {
                Console.WriteLine("遊戲結束，下次再來玩喔～");
            }
            else
            {
                Main(null);
            }
        }
    }
}