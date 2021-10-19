using System;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using NUnit.Framework;
using Libraries.DapperBuilder;

namespace Test.DapperBuilder
{
    public class DapperBuilderUsageTest
    {
        IDbConnection cn;

        [SetUp]
        public void Setup()
        {
            string SQLConnection = @"";
            cn = new SqlConnection(SQLConnection);
        }

        /// <summary>
        /// ［流暢式］
        /// 注意@AND會自己組
        /// ***************************************
        /// SELECT nbr, item_nbr, qty, amt
        /// FROM ord_bat
        /// WHERE [nbr] = @p0 AND [qty] >= @p1
        /// ORDER BY item_nbr
        /// </summary>
        [Test]
        public void Test_Fluent_Func()
        {
            var q = cn.QueryBuilder()
                .Select($"nbr")
                .Select($"item_nbr")
                .Select($"qty")
                .Select($"amt")
                .From($"ord_bat")
                .Where($"[nbr] = {1031106013}");
            var result = q.Query();
            Assert.That(result.Any());
            // if (true) q.Where($"[qty] >= {0}");
        }

        /// <summary>
        /// ［流暢式］
        /// 帶Join、數組複雜測試
        /// ***************************************
        /// SELECT a.nbr , a.cus_nbr ,c.cus_alias
        /// FROM ord_bah a
        /// LEFT JOIN cus_cus c on c.cus_nbr=a.cus_nbr
        /// WHERE a.nbr in @p0
        /// </summary>
        [Test]
        public void Test_Fluent_Completed_Joins_Func()
        {
            var nbrs = new string[] { "1030701001", "1030701002", "1030701004" };

            var q = cn.QueryBuilder()
                .Select($"a.nbr , a.cus_nbr ,c.cus_alias")
                .From($"ord_bah a")
                .From($"LEFT JOIN cus_cus c on c.cus_nbr=a.cus_nbr ")
                .Where($"a.nbr in {nbrs}");


            var qSql = q.Sql;
            Console.WriteLine(qSql);

            var result = q.Query();
            Assert.That(result.Any());
        }


        /// <summary>
        /// ［整貼式］
        /// SELECT INV_BAL.WARE_NBR, INV_BAL.ITEM_NBR, INV_BAL.PRO_NBR, INV_BAL.UNIT, INV_ITM.ITEM_DESC
        /// FROM INV_BAL,INV_ITM
        /// WHERE INV_BAL.ITEM_NBR = INV_ITM.ITEM_NBR
        /// AND INV_ITM.UD_OH_CTL = 'Y'
        /// AND INV_BAL.ITEM_NBR >= @p0
        /// AND INV_BAL.ITEM_NBR<=@p1 AND INV_BAL.WARE_NBR>=@p2 AND INV_BAL.WARE_NBR<=@p3
        /// AND INV_ITM.M_TYPE>=@p4 AND INV_ITM.M_TYPE<=@p5
        /// </summary>
        [Test]
        public void Test_EntireSQL_Func()
        {
            var mITEM_NBR1 = "";
            var mITEM_NBR2 = "ZZZZ";
            var mWARE_NBR1 = "";
            var mWARE_NBR2 = "ZZZZ";
            var mM_TYPE1 = "";
            var mM_TYPE2 = "ZZZZ";
            var mACR_MON = "10908";

            var q = cn // SQL
                .QueryBuilder($@"SELECT INV_BAL.WARE_NBR, INV_BAL.ITEM_NBR, INV_BAL.PRO_NBR, INV_BAL.UNIT, INV_ITM.ITEM_DESC
                                      FROM INV_BAL,
                                           INV_ITM
                                      WHERE INV_BAL.ITEM_NBR = INV_ITM.ITEM_NBR
                                        AND INV_ITM.UD_OH_CTL = 'Y'
                                      AND INV_BAL.ITEM_NBR >= {mITEM_NBR1}
                                      AND INV_BAL.ITEM_NBR<={mITEM_NBR2} AND INV_BAL.WARE_NBR>={mWARE_NBR1} AND INV_BAL.WARE_NBR<={mWARE_NBR2}
                                      AND INV_ITM.M_TYPE>={mM_TYPE1} AND INV_ITM.M_TYPE<={mM_TYPE2}");

            Console.WriteLine(q.Sql);
            var result = q.Query();
            Assert.That(result.Any());
        }

        /// <summary>
        /// ［整貼式］
        /// 　WHERE，抽出來另外組，ORDERBY放最後
        /// ********************
        /// SELECT *
        /// FROM INV_ITM
        /// WHERE INV_ITM.ITEM_NBR >= @p0 AND INV_ITM.ITEM_NBR<=@p1
        /// ORDER BY ITEM_NBR
        /// </summary>
        [Test]
        public void Test_EntireSQL_INSIDE_where_Func()
        {
            var mITEM_NBR1 = "";
            var mITEM_NBR2 = "ZZZZ";
            var mWARE_NBR1 = "";
            var mWARE_NBR2 = "0002-DA074               ";

            var q = cn // SQL
                .QueryBuilder($@"SELECT *
                                 FROM INV_ITM
                                /**where**/
                                ORDER BY ITEM_NBR");


            q.Where($"INV_ITM.ITEM_NBR >= {mITEM_NBR1}");
            q.Where($"INV_ITM.ITEM_NBR<={mITEM_NBR2}");

            var qSql = q.Sql;
            Console.WriteLine(qSql);

            // var result = q.Query();
            Assert.That(true);
        }


        /// <summary>
        /// ［命令式］CommandBuilder
        /// SELECT * FROM BAS_WAR WHERE WARE_DESC is not null
        /// </summary>
        [Test]
        public void Command_Func()
        {
            var q = cn.CommandBuilder($"SELECT * FROM BAS_WAR");
            q.Append($"WHERE WARE_DESC is not null");


            Console.WriteLine(q.Sql);
            var result = q.Query();

            Assert.That(result.Any());
        }
    }

    public class Cat
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}