using System;

namespace SqliteCommand
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Mono.Data.Sqlite.SqliteConnection v_con = null;
            Mono.Data.Sqlite.SqliteCommand v_cmd = null;

            Console.WriteLine("Exemplo SQLite usando Command");
            Console.WriteLine();

            try
            {
                // 1) instanciando Connection
                v_con = new Mono.Data.Sqlite.SqliteConnection(
                    "Data Source=../../../databases/lugares.db;Version=3;Synchronous=Full;Journal Mode=Off;"
                );

                // 2) abrindo Connection
                v_con.Open();

                // 3) instanciando Command
                v_cmd = new Mono.Data.Sqlite.SqliteCommand(
                    //"insert into estados values (60, 'WI', 'William Ivanski')",
                    "delete from estados where codigo = 60",
                    v_con
                );

                // 4) executando Command
                v_cmd.ExecuteNonQuery();

                Console.WriteLine("Ok.");
            }
            catch (Mono.Data.Sqlite.SqliteException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                // 5) liberando Command
                if (v_cmd != null)
                {
                    v_cmd.Cancel();
                    v_cmd.Dispose();
                    v_cmd = null;
                }

                // 6) fechando e liberando Connection
                if (v_con != null)
                {
                    v_con.Close();
                    v_con = null;
                }
            }

            Console.ReadKey();
        }
    }
}
