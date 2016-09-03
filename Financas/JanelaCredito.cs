using System;

namespace Financas
{
    public class JanelaCredito : Spartacus.Forms.Window
    {
        Spartacus.Database.Generic v_database;

        public JanelaCredito(Spartacus.Database.Generic p_database, Spartacus.Forms.Window p_pai)
            : base("Novo Crédito", 400, 500, p_pai)
        {
            this.v_database = p_database;
        }
    }
}
