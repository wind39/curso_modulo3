using System;

namespace Financas
{
    public class JanelaDebito : Spartacus.Forms.Window
    {
        Spartacus.Database.Generic v_database;

        public Spartacus.Forms.Lookup v_agente;
        Spartacus.Forms.Textbox v_valor;
        Spartacus.Forms.Memobox v_descricao;
        Spartacus.Forms.Buttons v_buttons;

        Spartacus.Database.Command v_cmd;

        public JanelaDebito(Spartacus.Database.Generic p_database, Spartacus.Forms.Window p_pai)
            : base("Novo Débito", 600, 300, p_pai)
        {
            this.v_database = p_database;

            this.v_agente = new Spartacus.Forms.Lookup(this, "Agente", 40, 60);
            this.v_agente.Populate(
                this.v_database,
                "select a.id,   " +
                "       a.nome  " +
                "from agentes a " +
                "order by a.id  "
            );
            this.Add(this.v_agente);

            this.v_valor = new Spartacus.Forms.Textbox(this, "Valor");
            this.Add(this.v_valor);

            this.v_descricao = new Spartacus.Forms.Memobox(this, "Descrição", 150);
            this.Add(this.v_descricao);

            this.v_buttons = new Spartacus.Forms.Buttons(this);
            this.v_buttons.AddButton("Salvar", this.ClickSalvar);
            this.v_buttons.AddButton("Cancelar", this.ClickCancelar);
            this.Add(this.v_buttons);

            this.v_cmd = new Spartacus.Database.Command();
            this.v_cmd.v_text = "insert into movimentos (idagente, data, debito, credito, saldo, descricao) values (#IDAGENTE#, #DATA#, #DEBITO#, #CREDITO#, #SALDO#, #DESCRICAO#)";
            this.v_cmd.AddParameter("IDAGENTE", Spartacus.Database.Type.INTEGER);
            this.v_cmd.AddParameter("DATA", Spartacus.Database.Type.INTEGER);
            this.v_cmd.AddParameter("DEBITO", Spartacus.Database.Type.REAL);
            this.v_cmd.SetLocale("DEBITO", Spartacus.Database.Locale.EUROPEAN);
            this.v_cmd.AddParameter("CREDITO", Spartacus.Database.Type.REAL);
            this.v_cmd.SetLocale("CREDITO", Spartacus.Database.Locale.EUROPEAN);
            this.v_cmd.AddParameter("SALDO", Spartacus.Database.Type.REAL);
            this.v_cmd.SetLocale("SALDO", Spartacus.Database.Locale.EUROPEAN);
            this.v_cmd.AddParameter("DESCRICAO", Spartacus.Database.Type.STRING);
        }

        public void ClickSalvar(object sender, EventArgs e)
        {
            double v_saldo, v_debito, v_novosaldo;
            string v_tmp;

            v_tmp = this.v_database.ExecuteScalar(
                "select m.saldo        " +
                "from movimentos m     " +
                "where m.id = (        " +
                "    select max(n.id)  " +
                "    from movimentos n " +
                ")                     "
            );

            if (!double.TryParse(v_tmp, out v_saldo))
                v_saldo = 0.0;

            if (!double.TryParse(this.v_valor.GetValue(), out v_debito))
                Spartacus.Forms.Messagebox.Show("Você deve digitar um valor válido.", "Erro!", Spartacus.Forms.Messagebox.Icon.ERROR);
            else
            {
                v_novosaldo = v_saldo - v_debito;

                this.v_cmd.SetValue("IDAGENTE", this.v_agente.GetValue());
                this.v_cmd.SetValue("DATA", DateTime.Now.ToString("yyyyMMdd"));
                this.v_cmd.SetValue("DEBITO", v_debito.ToString());
                this.v_cmd.SetValue("CREDITO", "0");
                this.v_cmd.SetValue("SALDO", v_novosaldo.ToString());
                this.v_cmd.SetValue("DESCRICAO", this.v_descricao.GetValue(), false);

                this.v_database.Execute(this.v_cmd.GetUpdatedText());

                this.Hide();
            }
        }

        public void ClickCancelar(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
