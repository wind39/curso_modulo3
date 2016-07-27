using System;

namespace NpgsqlDataReader
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Npgsql.NpgsqlConnection v_con = null;
            Npgsql.NpgsqlCommand v_cmd = null;
            Npgsql.NpgsqlDataReader v_reader = null;
            System.Data.DataTable v_table;
            System.Data.DataRow v_row;

            Console.WriteLine("Exemplo PostgreSQL usando DataReader");
            Console.WriteLine();

            try
            {
                // 1) instanciando Connection
                v_con = new Npgsql.NpgsqlConnection(
                    "Server=127.0.0.1;Port=5432;Database=lugares;User ID=postgres;Password=knightnote"
                );

                // 2) abrindo Connection
                v_con.Open();

                // 3) instanciando Command
                v_cmd = new Npgsql.NpgsqlCommand("select * from estados", v_con);

                // 4) executando DataReader
                v_reader = v_cmd.ExecuteReader();

                // 5) criando DataTable
                v_table = new System.Data.DataTable("RESULTADO");
                for (int i = 0; i < v_reader.FieldCount; i++)
                    v_table.Columns.Add(v_reader.GetName(i), typeof(string));

                // 6) alimentando DataTable
                while (v_reader.Read())
                {
                    v_row = v_table.NewRow();
                    for (int i = 0; i < v_reader.FieldCount; i++)
                        v_row[i] = v_reader[i].ToString();
                    v_table.Rows.Add(v_row);
                }

                // 7) usando DataTable (imprimindo na tela)
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
                // 8) liberando Command
                if (v_cmd != null)
                {
                    v_cmd.Cancel();
                    v_cmd.Dispose();
                    v_cmd = null;
                }

                // 9) liberando DataReader
                if (v_reader != null)
                {
                    v_reader.Close();
                    v_reader = null;
                }

                // 10) fechando e liberando Connection
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
