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

                /*Spartacus.Database.Command v_cmd;
                string v_estado = "PR";
                int x = 34;

                // 1) informar qual SGBD e dados de conexão
                v_db = new Spartacus.Database.Sqlite("../../../databases/lugares.db");

                // construindo o cmd
                v_cmd = new Spartacus.Database.Command();
                v_cmd.v_text = "select * from estados where sigla = #SIGLA# and campo = #CAMPO#";

                v_cmd.AddParameter("SIGLA", Spartacus.Database.Type.STRING);
                v_cmd.AddParameter("CAMPO", Spartacus.Database.Type.INTEGER);

                // usando o cmd
                v_cmd.SetValue("SIGLA", "PR");
                v_cmd.SetValue("CAMPO", "34");

                v_cmd.SetValue("SIGLA", v_estado);
                v_cmd.SetValue("CAMPO", x.ToString());

                // 2) fazer a consulta e jogar os resultados no DataTable
                v_table = v_db.Query(string.Format("select * from estados where sigla = '{0}' and campo = {1}", v_estado, x), "ESTADOS");

                v_table = v_db.Query(v_cmd.GetUpdatedText(), "ESTADOS");

                // 3) usar o DataTable (imprimir na tela)
                foreach (System.Data.DataColumn c in v_table.Columns)
                    Console.Write("{0}  ", c.ColumnName);
                Console.WriteLine();
                foreach (System.Data.DataRow r in v_table.Rows)
                {
                    foreach (System.Data.DataColumn c in v_table.Columns)
                        Console.Write("{0}      ", r[c].ToString());
                    Console.WriteLine();
                }*/
            }
            catch (Spartacus.Database.Exception exc)
            {
                Console.WriteLine(exc.v_message);
            }
        }
    }
}
