using System;

namespace Financas
{
    class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Spartacus.Database.Generic v_database;
            JanelaPrincipal v_janelaprincipal;

            v_database = new Spartacus.Database.Sqlite("../../../databases/financas.db");

            v_janelaprincipal = new JanelaPrincipal(v_database);

            v_janelaprincipal.Run();
        }
    }
}
