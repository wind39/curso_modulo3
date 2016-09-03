using System;

namespace Financas
{
    public class JanelaDebito : Spartacus.Forms.Window
    {
        Spartacus.Database.Generic v_database;

        Spartacus.Forms.Textbox v_id;
        public Spartacus.Forms.Lookup v_agente;
        Spartacus.Forms.Textbox v_valor;
        Spartacus.Forms.Memobox v_descricao;
        Spartacus.Forms.Buttons v_buttons;

        public JanelaDebito(Spartacus.Database.Generic p_database, Spartacus.Forms.Window p_pai)
            : base("Novo Débito", 400, 500, p_pai)
        {
            this.v_database = p_database;

            this.v_id = new Spartacus.Forms.Textbox(this, "Código");
            this.Add(this.v_id);

            this.v_agente = new Spartacus.Forms.Lookup(this, "Agente");
            this.v_agente.Populate(
                this.v_database,
                "select a.id,   " +
                "       a.nome  " +
                "from agentes a " +
                "order by a.id  "
            );
            this.Add(this.v_agente);
        }
    }
}
