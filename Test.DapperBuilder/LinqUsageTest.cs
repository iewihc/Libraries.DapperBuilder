using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using NUnit.Framework;

namespace Test.DapperBuilder
{
    public class LinqUsageTest
    {
        IDbConnection cn;

        [SetUp]
        public void Setup()
        {
            string SQLConnection = @"";
            cn = new SqlConnection(SQLConnection);
        }

        /// <summary>
        /// 篩選: 數組取偶數
        /// </summary>
        [Test]
        public void Test_Where_Mod_Func()
        {
            var arr = new[] {1, 2, 3, 4, 5};
            var query = arr.Where(item => item % 2 == 0);
            foreach (var el in query)
            {
                Console.WriteLine(el);  
            }
            Assert.That(true);
        }

        /// <summary>
        /// x index
        /// a     0
        /// b     1
        /// c     2
        /// </summary>
        [Test]
        public void Test_String_Override_Func()
        {
            var helloStr = "abcde";
            var query = helloStr.Select((x, index) => new {x, index});
            foreach (var item in query)
            {
                Console.WriteLine(item);
            }
            Assert.That(true);
        }

        /// <summary>
        /// GroupBy 以10為一組
        /// group key = 5 values = 55 57
        /// group key = 6 values = 63 66 69
        /// group key = 8 values = 80
        /// group key = 9 values = 96
        /// </summary>
        [Test]
        public void Test_Group_Func()
        {
            var mathScoreArray = new[] {96, 55, 63, 57, 69, 66,80};
            var query = mathScoreArray.OrderBy(x => x).GroupBy(x => x / 10);
            foreach (var group in query)
            {
                Console.WriteLine(value: group.Key);
                
                foreach (var val in group)
                {
                    Console.WriteLine(val);
                }
                
            }
        }

        /// <summary>
        /// 一個集合的每一項和第二個集合的每一項匹配
        /// (人配對水果)
        /// SelectMany <--> Groupby
        /// </summary>
        [Test]
        public void Test_SelectMany_Func()
        {
            string[] people = { "張三", "李四", "王二麻" };
            string[] fruits = { "橘子", "香蕉", "西瓜", "蘋果" };
            var query = people.SelectMany(x => fruits.Select(y=>x+"love"+y));
            var query2 = people.SelectMany(x => fruits, 
                (x, y) => x +"喜歡吃" +y);
            foreach (var item in query2)
            {
                Console.WriteLine(item);
            }            

            Assert.That(true);
        }

        /// <summary>
        /// Foreach Index...
        /// </summary>
        [Test]
        public void Foreach_Index_Inside_func()
        {
            var students = new List<Inv_itm>();
            students.Add(new Inv_itm { item_nbr = "1",item_name = "LIN"});
            students.Add(new Inv_itm { item_nbr = "2",item_name = "LEE"});
            students.Add(new Inv_itm { item_nbr = "3",item_name = "WANG"});

            foreach (var item in students)
            {
                var idx =students.IndexOf(item);
                var id = students[idx].item_nbr;
                Console.WriteLine("iddd",id);
            }
            
            Assert.That(true);
        }

        /// <summary>
        /// VFP SEEK
        /// ************
        /// 實際範例在 VFP 程式中    
        /// SELECT INV_ITM               &&INV_ITM是資料表別名稱或是別名
        /// INDEX ON item_nbr TAG T1     &&INV_ITM要先做索引
        /// =SEEK(mITEM_NBR)
        /// 1.從資料表區中選擇INV_ITM
        /// 2.從INV_ITM的索引中搜尋 變數mITEM_NBR (m.品號)
        /// 3.有找到就會返回.T. 
        /// </summary>
        [Test]
        public void VFP_Seek_Func()
        {
            var products = new List<Inv_itm>();
            
            Inv_itm itm = new Inv_itm();
            
            
            products.Add(new Inv_itm { item_nbr = "A01",item_name = "LIN Product"});
            products.Add(new Inv_itm { item_nbr = "B01",item_name = "LEE Product"});
            products.Add(new Inv_itm { item_nbr = "C01",item_name = "WANG Product"});

            var mITEM_NBR = "A01";
            var dic = products.ToDictionary(item => item.item_nbr);
            bool isExsit = dic.ContainsKey(mITEM_NBR);
            
            Assert.That(isExsit);
        }
        
  

        
        
        
    }

    public class Inv_itm
    {
        public string item_nbr { get; set; }
        public string item_name { get; set; }
    }
}