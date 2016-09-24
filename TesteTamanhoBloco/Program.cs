using System;

namespace TesteTamanhoBloco
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Spartacus.Database.Generic v_database;
            Spartacus.Utils.ProgressEventClass v_progress;
            Spartacus.Utils.ErrorEventClass v_error;
            Spartacus.Database.Command v_cmd;

            v_database = new Spartacus.Database.Sqlite("../../../databases/cotacoes_tamanhobloco.db");
            v_database.v_blocksize = 10000;

            v_progress = new Spartacus.Utils.ProgressEventClass();
            v_progress.ProgressEvent += OnProgress;
            v_error = new Spartacus.Utils.ErrorEventClass();
            v_error.ErrorEvent += OnError;

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

            v_database.Open();

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

            v_database.Close();

            // como fazer o experimento de forma automática
            /*
            int[] v_exp = { 1, 5, 10, 50, 100, 250, 500, 750, 1000, 2000, 3000 };
            System.Diagnostics.Stopwatch v_watch = new System.Diagnostics.Stopwatch();
            foreach (int v_bloco in v_exp)
            {
                v_database.v_blocksize = v_bloco;
                v_watch.Start;

                v_database.Open();
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
                v_database.Close();

                v_watch.Stop;
                Console.WriteLine("Tamanho de bloco = {0}, tempo total = {1}", v_bloco, v_watch.Elapsed);
            }
            */
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
