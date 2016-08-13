using System;

namespace SpartacusExemplo01
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Spartacus.Database.Generic v_db;
            System.Data.DataTable v_table;

            Console.WriteLine("Exemplo SQLite usando Spartacus");
            Console.WriteLine();

            try
            {
                // 1) informar qual SGBD e dados de conexão
                v_db = new Spartacus.Database.Sqlite("../../../databases/lugares.db");

                // 2) fazer a consulta e jogar os resultados no DataTable
                v_table = v_db.Query("select * from estados", "ESTADOS");

                // 3) usar o DataTable (imprimir na tela)
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
            catch (Spartacus.Database.Exception exc)
            {
                Console.WriteLine(exc.v_message);
            }
        }
    }
}
