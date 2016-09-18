using System;

namespace Lista4Exercicio06
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Spartacus.Database.Generic v_database;
            Spartacus.Utils.ProgressEventClass v_progress;
            Spartacus.Utils.ErrorEventClass v_error;
            Spartacus.Database.Command v_cmd;

            v_database = new Spartacus.Database.Sqlite("../../../databases/cotacoes.db");

            v_progress = new Spartacus.Utils.ProgressEventClass();
            v_progress.ProgressEvent += OnProgress;
            v_error = new Spartacus.Utils.ErrorEventClass();
            v_error.ErrorEvent += OnError;

            // carregando arquivo XLSX para tabela que não existe
            /*v_database.TransferFromFile(
                args[0],
                "cotacoes_excel",
                v_progress,
                v_error
            );*/

            // carregando arquivo CSV para tabela que não existe
            /*v_database.TransferFromFile(
                args[0],
                ";",
                "",
                false,
                System.Text.Encoding.UTF8,
                "cotacoes_tmp", // tabela não existe
                v_progress,
                v_error
            );*/

            // carregando arquivo CSV para tabela que existe
            v_cmd = new Spartacus.Database.Command();
            v_cmd.v_text = "(#col0#,#col1#,#col2#,#col3#,#col4#,#col5#,#col6#,#col7#)";
            v_cmd.AddParameter("col0", Spartacus.Database.Type.STRING);
            v_cmd.AddParameter("col1", Spartacus.Database.Type.INTEGER);
            v_cmd.AddParameter("col2", Spartacus.Database.Type.STRING);
            v_cmd.AddParameter("col3", Spartacus.Database.Type.STRING);
            v_cmd.AddParameter("col4", Spartacus.Database.Type.REAL);
            v_cmd.SetLocale("col4", Spartacus.Database.Locale.EUROPEAN);
            v_cmd.AddParameter("col5", Spartacus.Database.Type.REAL);
            v_cmd.SetLocale("col5", Spartacus.Database.Locale.EUROPEAN);
            v_cmd.AddParameter("col6", Spartacus.Database.Type.REAL);
            v_cmd.SetLocale("col6", Spartacus.Database.Locale.EUROPEAN);
            v_cmd.AddParameter("col7", Spartacus.Database.Type.REAL);
            v_cmd.SetLocale("col7", Spartacus.Database.Locale.EUROPEAN);

            v_database.TransferFromFile(
                args[0],
                ";",
                "",
                false,
                System.Text.Encoding.UTF8,
                "cotacoes", // tabela existe
                "(dia,codmoeda,tipomoeda,siglamoeda,taxacompra,taxavenda,parcompra,parvenda)",
                v_cmd,
                v_progress,
                v_error
            );

            // salvando arquivo XLSX com os dados
            //v_database.TransferToFile("select * from cotacoes", "export_cotacoes.xlsx");
        }

        public static void OnProgress(Spartacus.Utils.ProgressEventClass sender, Spartacus.Utils.ProgressEventArgs e)
        {
            Console.WriteLine("Linhas inseridas até o momento: {0}", e.v_counter);
        }

        public static void OnError(Spartacus.Utils.ErrorEventClass sender, Spartacus.Utils.ErrorEventArgs e)
        {
            Console.WriteLine(e.v_message);
        }
    }
}
