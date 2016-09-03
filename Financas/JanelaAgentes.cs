using System;

namespace Financas
{
    public class JanelaAgentes : Spartacus.Forms.Window
    {
        Spartacus.Database.Generic v_database;

        Spartacus.Forms.Grid v_grid;
        Spartacus.Forms.Buttons v_buttons;

        JanelaAgente v_janelaagente;

        Spartacus.Database.Command v_delete;


        public JanelaAgentes(Spartacus.Database.Generic p_database, Spartacus.Forms.Window p_pai)
            : base("Cadastro de Agentes", 600, 400, p_pai)
        {
            this.v_database = p_database;

            this.v_grid = new Spartacus.Forms.Grid(this, 330);
            this.v_grid.Populate(
                this.v_database,
                "select a.id,      " +
                "       a.nome,    " +
                "       a.telefone " +
                "from agentes a    " +
                "order by a.id     "
            );
            this.Add(this.v_grid);

            this.v_buttons = new Spartacus.Forms.Buttons(this);
            this.v_buttons.AddButton("Novo", this.ClickNovo);
            this.v_buttons.AddButton("Editar", this.ClickEditar);
            this.v_buttons.AddButton("Remover", this.ClickRemover);
            this.Add(this.v_buttons);

            this.v_janelaagente = new JanelaAgente(this.v_database, this);

            this.v_delete = new Spartacus.Database.Command();
            this.v_delete.v_text = "delete from agentes where id = #ID#";
            this.v_delete.AddParameter("ID", Spartacus.Database.Type.INTEGER);
        }

        public void ClickNovo(object sender, EventArgs e)
        {
            this.v_janelaagente.v_modo_update = false;
            this.v_janelaagente.Show();
        }

        public void ClickEditar(object sender, EventArgs e)
        {
            System.Data.DataRow v_row;

            v_row = this.v_grid.CurrentRow();

            this.v_janelaagente.v_id.SetValue(v_row["id"].ToString());
            this.v_janelaagente.v_nome.SetValue(v_row["nome"].ToString());
            this.v_janelaagente.v_telefone.SetValue(v_row["telefone"].ToString());

            this.v_janelaagente.v_modo_update = true;
            this.v_janelaagente.Show();
        }

        public void ClickRemover(object sender, EventArgs e)
        {
            System.Data.DataRow v_row;

            v_row = this.v_grid.CurrentRow();

            this.v_delete.SetValue("ID", v_row["id"].ToString());
            this.v_database.Execute(this.v_delete.GetUpdatedText());

            this.v_grid.Refresh();
        }
    }
}
