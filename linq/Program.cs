using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace linq
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var csvText = File.ReadAllLines("C:\\Users\\user\\source\\repos\\linq\\product.csv").Skip(1).ToList();
            List<Product> list = new List<Product>();
            foreach (var text in csvText)
            {
                list.AddRange(CreateList(text));
            }
            var totalprice = list.Sum(p => p.price);
            Console.WriteLine($"價錢的總和為: {totalprice}");
            Console.WriteLine("--------");
            var aveprice = list.Average(p => p.price);
            Console.WriteLine($"價錢的平均為: {aveprice}");
            Console.WriteLine("--------");
            var count = list.Sum((p) => p.quantity);
            Console.WriteLine($"商品的總數量 : {count}");
            Console.WriteLine("--------");
            var avecount = list.Average((p) => p.quantity);
            Console.WriteLine($"商品的總平均 : {avecount}");
            Console.WriteLine("--------");
            var maxprice = list.OrderByDescending(record => record.price).FirstOrDefault();
            Console.WriteLine("最貴的商品是: {0}, 價格是: {1}", maxprice.name, maxprice.price);
            Console.WriteLine("--------");
            var minprice = list.OrderBy(record => record.price).FirstOrDefault();
            Console.WriteLine("最便宜的商品是: {0}, 價格是: {1}", minprice.name, minprice.price);
            Console.WriteLine("--------");
            var totalCost = list.Where(p => p.type == "3C").Sum(p => p.price);
            Console.WriteLine(" 3C 的商品總價為: {0}", totalCost);
            Console.WriteLine("--------");
            var twototalCost = list.Where(p => p.type == "飲料" || p.type == "食品").Sum(p => p.price);
            Console.WriteLine("飲料及食品的商品總價為: {0}", twototalCost);
            Console.WriteLine("--------");
            var food = list.Where(p => p.type == "食品" && p.quantity > 100);
            Console.WriteLine("商品類別為食品，而且商品數量大於 100 的商品為以下幾種:");
            foreach (var item in food)
            {
                Console.WriteLine("商品名稱: {0}, 數量: {1}", item.name, item.quantity);
            }
            Console.WriteLine("--------");

            var result = list.Where(p => p.price > 1000).GroupBy(p => p.type);
            Console.WriteLine("商品的價格是大於 1000 的商品為以下幾種:");
            foreach (var group in result)
            {
                Console.WriteLine("種類: {0}", group.Key);
                foreach (var item in group)
                {
                    Console.WriteLine("商品名稱: {0}, 價格: {1}", item.name, item.price);
                }
            }
            Console.WriteLine("--------");
            var typeavgPrices = result.ToDictionary(g => g.Key, g => g.Average(p => p.price));
            foreach (var avgPrice in typeavgPrices)
            {
                Console.WriteLine("種類: {0},平均價格為: {1}", avgPrice.Key, avgPrice.Value);
            }

            Console.WriteLine("--------");
            var hightolow = list.OrderByDescending(p => p.price);
            Console.WriteLine("依照商品價格由高到低排序");
            foreach (var product in hightolow)
            {
                Console.WriteLine("{0}: {1}", product.name, product.price);
            }
            Console.WriteLine("--------");
            var lowtohigh = list.OrderBy(p => p.price);
            Console.WriteLine("依照商品價格由低到高排序");
            foreach (var product in lowtohigh)
            {
                Console.WriteLine("{0}: {1}", product.name, product.price);
            }
            Console.WriteLine("--------");
            var maxPriceByType = list.GroupBy(p => p.type) .Select(g => new { type = g.Key,MaxPrice = g.Max(p => p.price),ProductName = g.OrderBy(p => p.price).FirstOrDefault()?.name });
            Console.WriteLine("各商品類別底下，最貴的商品為以下:");
            foreach (var item in maxPriceByType)
            {
                Console.WriteLine("商品種類: {0} , 商品名稱: {1} , 商品價格: {2}", item.type, item.ProductName, item.MaxPrice);
            }
            Console.WriteLine("--------");
            var minPriceByType = list.GroupBy(p => p.type).Select(g => new { type = g.Key, MinPrice = g.Min(p => p.price), ProductName = g.OrderBy(p => p.price).FirstOrDefault()?.name });
            Console.WriteLine("各商品類別底下，最便宜的商品為以下:");
            foreach (var item in minPriceByType)
            {
                Console.WriteLine("商品種類: {0} , 商品名稱: {1} , 商品價格: {2}", item.type, item.ProductName, item.MinPrice);
            }
            Console.WriteLine("--------");
            var cheapProducts = list.Where(p => p.price <= 10000);
            Console.WriteLine("價格小於等於 10000 的商品為以下:");
            foreach (var item in cheapProducts)
            {
                Console.WriteLine("商品名稱: {0} , 商品價格: {1}", item.name, item.price);
            }
            Console.WriteLine("--------");

            int pageIndex = 0; // 頁數從 0 開始
            int pageSize = 4; // 每頁 4 筆
            int totalPage = (int)Math.Ceiling((double)list.Count / pageSize); // 總頁數

            // 取得目前頁數的資料
            var pageData = list.Skip(pageIndex * pageSize).Take(pageSize);

            // 顯示目前頁數的資料
            foreach (var product in pageData)
            {
                Console.WriteLine($"{product.sid} {product.name} {product.price}");
            }

            // 顯示分頁選擇器
            Console.WriteLine($"第 {pageIndex + 1} 頁，共 {totalPage} 頁。");
            Console.Write("請輸入指令 (n:下一頁，p:上一頁，e:離開)：");

            // 分頁選擇器
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "n")
                {
                    pageIndex++;
                    if (pageIndex >= totalPage)
                    {
                        Console.WriteLine("已經到最後一頁！");
                        pageIndex = totalPage - 1;
                    }
                    else
                    {
                        pageData = list.Skip(pageIndex * pageSize).Take(pageSize);
                        foreach (var product in pageData)
                        {
                            Console.WriteLine($"{product.sid} {product.name} {product.price}");
                        }
                        Console.WriteLine($"第 {pageIndex + 1} 頁，共 {totalPage} 頁。");
                        Console.Write("請輸入指令 (n:下一頁，p:上一頁，e:離開)：");
                    }
                }
                else if (input == "p")
                {
                    pageIndex--;
                    if (pageIndex < 0)
                    {
                        Console.WriteLine("已經到第一頁！");
                        pageIndex = 0;
                    }
                    else
                    {
                        pageData = list.Skip(pageIndex * pageSize).Take(pageSize);
                        foreach (var product in pageData)
                        {
                            Console.WriteLine($"{product.sid}   {product.name}   {product.price}");
                        }
                        Console.WriteLine($"第 {pageIndex + 1} 頁，共 {totalPage} 頁。");
                        Console.Write("請輸入指令 (n:下一頁，p:上一頁，e:離開)：");
                    }
                }
                else if (input == "e")
                {
                    break;
                }
                else
                {
                    Console.Write("輸入錯誤，請重新輸入：");
                }
            }


            Console.ReadLine();
        }

        static List<Product> CreateList(string csv) 
        {
            string[] values= csv.Split(',');
            return new List<Product>()
            {
                new Product()
                {
                    sid = Convert.ToString(values[0]),
                    name = Convert.ToString(values[1]),
                    quantity = Convert.ToInt32(values[2]),
                    price = Convert.ToInt32(values[3]),
                    type = Convert.ToString(values[4]),
                }
            };
        }
    }
}
