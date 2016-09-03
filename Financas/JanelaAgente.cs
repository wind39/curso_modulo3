using System;

namespace Financas
{
    public class JanelaAgente : Spartacus.Forms.Window
    {
        Spartacus.Database.Generic v_database;

        public Spartacus.Forms.Textbox v_id;
        public Spartacus.Forms.Textbox v_nome;
        public Spartacus.Forms.Textbox v_telefone;
        Spartacus.Forms.Buttons v_buttons;

        Spartacus.Database.Command v_insert;
        Spartacus.Database.Command v_update;

        public bool v_modo_update;


        public JanelaAgente(Spartacus.Database.Generic p_database, Spartacus.Forms.Window p_pai)
            : base("Edição de Agente", 300, 200, p_pai)
        {
            this.v_database = p_database;

            this.v_id = new Spartacus.Forms.Textbox(this, "Código");
            this.v_id.Disable();
            this.Add(this.v_id);

            this.v_nome = new Spartacus.Forms.Textbox(this, "Nome");
            this.Add(this.v_nome);

            this.v_telefone = new Spartacus.Forms.Textbox(this, "Telefone");
            this.Add(this.v_telefone);

            this.v_buttons = new Spartacus.Forms.Buttons(this);
            this.v_buttons.AddButton("Salvar", this.ClickSalvar);
            this.v_buttons.AddButton("Cancelar", this.ClickCancelar);
            this.Add(this.v_buttons);

            this.v_insert = new Spartacus.Database.Command();
            this.v_insert.v_text = "insert into agentes (nome, telefone) values (#NOME#, #TELEFONE#)";
            this.v_insert.AddParameter("NOME", Spartacus.Database.Type.STRING);
            this.v_insert.AddParameter("TELEFONE", Spartacus.Database.Type.STRING);

            this.v_update = new Spartacus.Database.Command();
            this.v_update.v_text = "update agentes set nome = #NOME#, telefone = #TELEFONE# where id = #ID#";
            this.v_update.AddParameter("NOME", Spartacus.Database.Type.STRING);
            this.v_update.AddParameter("TELEFONE", Spartacus.Database.Type.STRING);
            this.v_update.AddParameter("ID", Spartacus.Database.Type.INTEGER);
        }

        public void ClickSalvar(object sender, EventArgs e)
        {
            if (this.v_modo_update)
            {
                this.v_update.SetValue("NOME", this.v_nome.GetValue());
                this.v_update.SetValue("TELEFONE", this.v_telefone.GetValue());
                this.v_update.SetValue("ID", this.v_id.GetValue());
                this.v_database.Execute(this.v_update.GetUpdatedText());
            }
            else
            {
                this.v_insert.SetValue("NOME", this.v_nome.GetValue());
                this.v_insert.SetValue("TELEFONE", this.v_telefone.GetValue());
                this.v_database.Execute(this.v_insert.GetUpdatedText());
            }

            this.Clear();
            this.Hide();
        }

        public void ClickCancelar(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
