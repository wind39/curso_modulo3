using System;

namespace Partidas
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Spartacus.Database.Generic v_db;
            Spartacus.Database.Command v_insert, v_update;
            int v_num_servidores, v_num_jogadores;
            int v_servidor, v_jogador, v_resultado, v_duracao;
            System.DateTime v_datahora;

            System.Random v_random = new System.Random();
            int[] v_jogadores = new int[4];
            int[,] v_resultados = {
                {0, 1, 2, 3},
                {0, 1, 3, 2},
                {0, 2, 1, 3},
                {0, 2, 3, 1},
                {0, 3, 1, 2},
                {0, 3, 2, 1},
                {1, 0, 2, 3},
                {1, 0, 3, 2},
                {2, 0, 1, 3},
                {2, 0, 3, 1},
                {3, 0, 1, 2},
                {3, 0, 2, 1},
                {1, 2, 0, 3},
                {1, 3, 0, 2},
                {2, 1, 0, 3},
                {2, 3, 0, 1},
                {3, 1, 0, 2},
                {3, 2, 0, 1},
                {1, 2, 3, 0},
                {1, 3, 2, 0},
                {2, 1, 3, 0},
                {2, 3, 1, 0},
                {3, 1, 2, 0},
                {3, 2, 1, 0}
            };
            System.DateTime v_inicio = System.DateTime.Parse("2016-01-01 00:00:00");
            System.TimeSpan v_intervalo = System.DateTime.Now - v_inicio;

            v_insert = new Spartacus.Database.Command();
            v_insert.v_text = "insert into partidas (id_servidor, situacao, jogador1, jogador2, jogador3, jogador4, inicio, duracao, primeiro, segundo, terceiro, quarto) values (#id_servidor#, 'F', #jogador1#, #jogador2#, #jogador3#, #jogador4#, #inicio#, #duracao#, #primeiro#, #segundo#, #terceiro#, #quarto#)";
            v_insert.AddParameter("id_servidor", Spartacus.Database.Type.INTEGER);
            v_insert.AddParameter("jogador1", Spartacus.Database.Type.INTEGER);
            v_insert.AddParameter("jogador2", Spartacus.Database.Type.INTEGER);
            v_insert.AddParameter("jogador3", Spartacus.Database.Type.INTEGER);
            v_insert.AddParameter("jogador4", Spartacus.Database.Type.INTEGER);
            v_insert.AddParameter("inicio", Spartacus.Database.Type.STRING);
            v_insert.AddParameter("duracao", Spartacus.Database.Type.INTEGER);
            v_insert.AddParameter("primeiro", Spartacus.Database.Type.INTEGER);
            v_insert.AddParameter("segundo", Spartacus.Database.Type.INTEGER);
            v_insert.AddParameter("terceiro", Spartacus.Database.Type.INTEGER);
            v_insert.AddParameter("quarto", Spartacus.Database.Type.INTEGER);

            v_update = new Spartacus.Database.Command();
            v_update.v_text = "update jogadores set pontuacao = pontuacao + #pontos# where id = #id#";
            v_update.AddParameter("pontos", Spartacus.Database.Type.INTEGER);
            v_update.AddParameter("id", Spartacus.Database.Type.INTEGER);

            v_db = new Spartacus.Database.Postgresql(
                "192.168.56.2",
                "avaliacao",
                "postgres",
                "curso"
            );
            v_db.Open();

            v_num_servidores = int.Parse(v_db.ExecuteScalar("select count(*) from servidores"));
            v_num_jogadores = int.Parse(v_db.ExecuteScalar("select count(*) from jogadores"));

            for (int k = 2; k <= 500000; k++)
            {
                v_servidor = v_random.Next(1, v_num_servidores + 1);

                v_jogadores[0] = 0;
                v_jogadores[1] = 0;
                v_jogadores[2] = 0;
                v_jogadores[3] = 0;
                for (int j = 0; j < 4; j++)
                {
                    do
                    {
                        v_jogador = v_random.Next(1, v_num_jogadores + 1);
                    }
                    while (v_jogador == v_jogadores[0] ||
                           v_jogador == v_jogadores[1] ||
                           v_jogador == v_jogadores[2] ||
                           v_jogador == v_jogadores[3]);

                    v_jogadores[j] = v_jogador;
                }

                v_datahora = v_inicio.AddSeconds(v_random.Next((int) v_intervalo.TotalSeconds));

                v_duracao = v_random.Next(10, 3601);

                v_resultado = v_random.Next(24);

                v_insert.SetValue("id_servidor", v_servidor.ToString());
                v_insert.SetValue("jogador1", v_jogadores[0].ToString());
                v_insert.SetValue("jogador2", v_jogadores[1].ToString());
                v_insert.SetValue("jogador3", v_jogadores[2].ToString());
                v_insert.SetValue("jogador4", v_jogadores[3].ToString());
                v_insert.SetValue("inicio", v_datahora.ToString());
                v_insert.SetValue("duracao", v_duracao.ToString());
                v_insert.SetValue("primeiro", v_jogadores[v_resultados[v_resultado, 0]].ToString());
                v_insert.SetValue("segundo", v_jogadores[v_resultados[v_resultado, 1]].ToString());
                v_insert.SetValue("terceiro", v_jogadores[v_resultados[v_resultado, 2]].ToString());
                v_insert.SetValue("quarto", v_jogadores[v_resultados[v_resultado, 3]].ToString());
                v_db.Execute(v_insert.GetUpdatedText());

                v_update.SetValue("pontos", "5");
                v_update.SetValue("id", v_jogadores[v_resultados[v_resultado, 0]].ToString());
                v_db.Execute(v_update.GetUpdatedText());

                v_update.SetValue("pontos", "2");
                v_update.SetValue("id", v_jogadores[v_resultados[v_resultado, 1]].ToString());
                v_db.Execute(v_update.GetUpdatedText());

                v_update.SetValue("pontos", "1");
                v_update.SetValue("id", v_jogadores[v_resultados[v_resultado, 2]].ToString());
                v_db.Execute(v_update.GetUpdatedText());

                Console.WriteLine("Gerando partida {0}", k);
            }

            v_db.Close();
        }
    }
}
