using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Diagnostics;
using SeleniumTests;

namespace EntityTest
{
    class Program
    {
        static void Main(string[] args)
        {

            string sql = "";
            Stopwatch sw = new Stopwatch();
            TimeSpan e1;
            long ms;

            using (var context = new TUMISDBEntities())
            {
                sw.Reset();
                sw = Stopwatch.StartNew();

                // Lazy Loading
                foreach (var parent in context.DemoBook)
                {
                    Console.Write("Book :{0}, author:", parent.title);
                    
                    foreach (var item in parent.DemoBookAuth)
                    {
                        Console.Write("{0}, ",item.psn_nam );

                    }
                    Console.WriteLine();
                }

                sw.Stop();
                e1 = sw.Elapsed;
                ms = sw.ElapsedMilliseconds;
                Console.WriteLine("Lazy Loading: " + e1 + "秒 " + ms + "毫秒");
                Console.WriteLine("----------");
                var booklist = from tmp in context.DemoBook select new { tmp.title, tmp.DemoBookAuth };
                
                sql = (booklist as ObjectQuery).ToTraceString(); // check sql
                //Console.WriteLine(sql);

                sw.Reset();
                sw = Stopwatch.StartNew();

                //Eager Loading 機制: Include Method
                foreach (var parent in context.DemoBook.Include("DemoBookAuth"))
                {
                    Console.Write("Book :{0}, author:", parent.title);

                    foreach (var item in parent.DemoBookAuth)
                    {
                        Console.Write("{0}, ", item.psn_nam);
                    }
                    Console.WriteLine();
                }

                sw.Stop();
                e1 = sw.Elapsed;
                ms = sw.ElapsedMilliseconds;
                Console.WriteLine("Eager Loading: " + e1 + "秒 " + ms + "毫秒");


            }

        }
    }


}
