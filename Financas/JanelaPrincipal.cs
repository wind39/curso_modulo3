using System;

namespace Financas
{
    public class JanelaPrincipal : Spartacus.Forms.Window
    {
        Spartacus.Database.Generic v_database;

        Spartacus.Forms.Menu v_menu;
        Spartacus.Forms.Grid v_grid;
        Spartacus.Forms.Buttons v_buttons;

        JanelaAgentes v_janelaagentes;
        JanelaDebito v_janeladebito;
        JanelaCredito v_janelacredito;


        public JanelaPrincipal(Spartacus.Database.Generic p_database) : base("Finanças Pessoais", 800, 600)
        {
            this.v_database = p_database;

            Spartacus.Forms.Menugroup v_group;
            this.v_menu = new Spartacus.Forms.Menu(this);
            v_group = this.v_menu.AddGroup("Cadastro");
            this.v_menu.AddItem(v_group, "Agentes", this.MenuAgentes);
            this.Add(this.v_menu);

            this.v_grid = new Spartacus.Forms.Grid(this, 480);
            this.v_grid.Populate(
                this.v_database,
                "select m.id,         " +
                "       m.data,       " +
                "       a.nome,       " +
                "       m.descricao,  " +
                "       m.debito,     " +
                "       m.credito,    " +
                "       m.saldo       " +
                "from movimentos m    " +
                "inner join agentes a " +
                "on a.id = m.idagente " +
                "order by m.id desc   "
            );
            this.Add(this.v_grid);

            this.v_buttons = new Spartacus.Forms.Buttons(this);
            this.v_buttons.AddButton("Débito", this.ClickDebito);
            this.v_buttons.AddButton("Crédito", this.ClickCredito);
            this.v_buttons.AddButton("Estorno", this.ClickEstorno);
            this.v_buttons.AddButton("Atualizar", this.ClickAtualizar);
            this.Add(this.v_buttons);

            this.v_janelaagentes = new JanelaAgentes(this.v_database, this);
            this.v_janeladebito = new JanelaDebito(this.v_database, this);
            this.v_janelacredito = new JanelaCredito(this.v_database, this);
        }

        public void MenuAgentes(object sender, EventArgs e)
        {
            this.v_janelaagentes.Show();
        }

        public void ClickDebito(object sender, EventArgs e)
        {
            this.v_janeladebito.v_agente.Refresh();
            this.v_janeladebito.Show();
        }

        public void ClickCredito(object sender, EventArgs e)
        {
            //this.v_janelacredito.v_agente.Refresh();
            this.v_janelacredito.Show();
        }

        public void ClickEstorno(object sender, EventArgs e)
        {
            Spartacus.Forms.Messagebox.Show("Ainda não implementado.", "Erro!", Spartacus.Forms.Messagebox.Icon.HAND);
        }

        public void ClickAtualizar(object sender, EventArgs e)
        {
            Spartacus.Forms.Messagebox.Show("Ainda não implementado.", "Erro!", Spartacus.Forms.Messagebox.Icon.HAND);
        }
    }
}
