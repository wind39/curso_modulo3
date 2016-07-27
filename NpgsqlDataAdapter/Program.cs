using System;

namespace NpgsqlDataAdapter
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Npgsql.NpgsqlConnection v_con = null;
            Npgsql.NpgsqlDataAdapter v_adapter = null;
            System.Data.DataTable v_table;

            Console.WriteLine("Exemplo PostgreSQL usando DataAdapter");
            Console.WriteLine();

            try
            {
                // 1) instanciando Connection
                v_con = new Npgsql.NpgsqlConnection(
                    "Server=127.0.0.1;Port=5432;Database=lugares;User ID=postgres;Password=knightnote"
                );

                // 2) abrindo Connection
                v_con.Open();

                // 3) instanciando DataAdapter
                v_adapter = new Npgsql.NpgsqlDataAdapter("select * from estados", v_con);

                // 4) instanciando DataTable
                v_table = new System.Data.DataTable("RESULTADO");

                // 5) alimentando DataTable
                v_adapter.Fill(v_table);

                // 6) usando DataTable (imprimindo na tela)
                foreach (System.Data.DataColumn c in v_table.Columns)
                    Console.Write("{0}  ", c.ColumnName);
                Console.WriteLine();
                foreach (System.Data.DataRow r in v_table.Rows)
                {
                    foreach (System.Data.DataColumn c in v_table.Columns)
                        Console.Write("{0}      ", r[c].ToString());
                    Console.WriteLine();
                }
            }
            catch (Npgsql.NpgsqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                // 7) fechando e liberando Connection
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
