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

        Spartacus.Database.Command v_cmd;

        public JanelaPrincipal(Spartacus.Database.Generic p_database) : base("Finanças Pessoais", 800, 600)
        {
            this.v_database = p_database;

            Spartacus.Forms.Menugroup v_group, v_group2;
            this.v_menu = new Spartacus.Forms.Menu(this);
            v_group = this.v_menu.AddGroup("Cadastro");
            this.v_menu.AddItem(v_group, "Agentes", this.MenuAgentes);
            this.Add(this.v_menu);
            v_group = this.v_menu.AddGroup("Relatórios");
            v_group2 = this.v_menu.AddGroup(v_group, "Listagem de Agentes");
            this.v_menu.AddItem(v_group2, "Excel", this.MenuAgentesExcel);
            this.v_menu.AddItem(v_group2, "PDF", this.MenuAgentesPDF);
            v_group2 = this.v_menu.AddGroup(v_group, "Listagem de Movimentação");
            this.v_menu.AddItem(v_group2, "Excel", this.MenuMovimentacaoExcel);
            this.v_menu.AddItem(v_group2, "PDF", this.MenuMovimentacaoPDF);

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
            this.v_janelacredito.v_agente.Refresh();
            this.v_janelacredito.Show();
        }

        public void ClickEstorno(object sender, EventArgs e)
        {
            System.Data.DataRow v_row;
            double v_saldo, v_debito, v_credito, v_novosaldo;
            string v_tmp;
            int v_idagente;

            try
            {
                v_row = this.v_grid.CurrentRow();

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

                v_debito = double.Parse(v_row["credito"].ToString());
                v_credito = double.Parse(v_row["debito"].ToString());
                v_novosaldo = v_saldo - v_debito + v_credito;

                v_idagente = int.Parse(this.v_database.ExecuteScalar("select idagente from movimentos where id = " + v_row["id"].ToString()));

                this.v_cmd.SetValue("IDAGENTE", v_idagente.ToString());
                this.v_cmd.SetValue("DATA", DateTime.Now.ToString("yyyyMMdd"));
                this.v_cmd.SetValue("DEBITO", v_debito.ToString());
                this.v_cmd.SetValue("CREDITO", v_credito.ToString());
                this.v_cmd.SetValue("SALDO", v_novosaldo.ToString());
                this.v_cmd.SetValue("DESCRICAO", "[ESTORNO] " + v_row["descricao"].ToString(), false);

                this.v_database.Execute(this.v_cmd.GetUpdatedText());

                this.v_grid.Refresh();
            }
            catch (Exception exc)
            {
                Spartacus.Forms.Messagebox.Show(exc.Message, "Exception", Spartacus.Forms.Messagebox.Icon.ASTERISK);
            }
        }

        public void ClickAtualizar(object sender, EventArgs e)
        {
            this.v_grid.Refresh();
        }

        public void MenuAgentesExcel(object sender, EventArgs e)
        {
            Spartacus.Utils.Excel v_excel;
            System.Data.DataTable v_table;

            v_table = this.v_database.Query(
                "select a.id,      " +
                "       a.nome,    " +
                "       a.telefone " +
                "from agentes a    " +
                "order by a.id     ", "AGENTES"
            );

            v_excel = new Spartacus.Utils.Excel();
            v_excel.v_set.Tables.Add(v_table);
            v_excel.Export("agentes.xlsx");

            Spartacus.Forms.Messagebox.Show("Relatório salvo com sucesso.", "OK", Spartacus.Forms.Messagebox.Icon.INFORMATION);
        }

        public void MenuAgentesPDF(object sender, EventArgs e)
        {
            Spartacus.Reporting.Report v_report;
            System.Data.DataTable v_table;

            v_table = this.v_database.Query(
                "select a.id,      " +
                "       a.nome,    " +
                "       a.telefone " +
                "from agentes a    " +
                "order by a.id     ", "AGENTES"
            );

            v_report = new Spartacus.Reporting.Report(v_table);
            v_report.Execute();
            v_report.Save("agentes.pdf");

            Spartacus.Forms.Messagebox.Show("Relatório salvo com sucesso.", "OK", Spartacus.Forms.Messagebox.Icon.INFORMATION);
        }

        public void MenuMovimentacaoExcel(object sender, EventArgs e)
        {
            Spartacus.Utils.Excel v_excel;
            System.Data.DataTable v_table;

            v_table = this.v_database.Query(
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
                "order by m.id desc   ", "MOVIMENTACAO"
            );

            v_excel = new Spartacus.Utils.Excel();
            v_excel.v_set.Tables.Add(v_table);
            v_excel.Export("movimentacao.xlsx");

            Spartacus.Forms.Messagebox.Show("Relatório salvo com sucesso.", "OK", Spartacus.Forms.Messagebox.Icon.INFORMATION);
        }

        public void MenuMovimentacaoPDF(object sender, EventArgs e)
        {
            Spartacus.Reporting.Report v_report;
            System.Data.DataTable v_table;

            v_table = this.v_database.Query(
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
                "order by m.id desc   ", "MOVIMENTACAO"
            );

            v_report = new Spartacus.Reporting.Report(v_table);
            v_report.Execute();
            v_report.Save("movimentacao.pdf");

            Spartacus.Forms.Messagebox.Show("Relatório salvo com sucesso.", "OK", Spartacus.Forms.Messagebox.Icon.INFORMATION);
        }
    }
}
