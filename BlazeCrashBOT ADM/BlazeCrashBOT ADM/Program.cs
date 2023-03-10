using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

public class Program
{

    public class Record
    {
        public string id { get; set; }
        public string crash_point { get; set; }
        public string created_at { get; set; }
        public float ultimoCrash;
    }

    public class Data 
    { 
        public int total_pages { get; set; }
        public List<Record> records { get; set; }
    }

    public class IsRunning 
    {
        public string id { get; set; }
        public string status { get; set; }    

    }



    public class Estrategias
    {
        public float UtlimoResultado;
        public float PenultimoResultado;
        public float AntepenultimoResultado;
        public float AnteantepenultimoResultado;
        public float PreantepenultimoResultado;
        public float ValorParaSairNaProx;
    }

    public static class EstrategiaManager
    {
        public static List<Estrategias> GetListaDeEstrategias()
        {
            List<Estrategias> Listadeestrategias = new List<Estrategias>();
            // se tiver 3 seguidas <= 1.5 ou na quarta tirar em 1.9
            Estrategias estrategiasum = new Estrategias() { UtlimoResultado = 1.5f, PenultimoResultado = 1.50f, AntepenultimoResultado = 1.50f, ValorParaSairNaProx = 2.25f };
           

            Listadeestrategias.Add(estrategiasum);

            return Listadeestrategias;
        }
    }

    private static ITelegramBotClient botClient;

    private const string ApiUrl = "https://blaze.com/api/crash_games/history?records";

    public static float entradasTotais;
    public static float entradasLoss;
    public static float entradasWins;

    // main funcao
    public static async Task Main()
    {
        botClient = new TelegramBotClient("TELEGRAM_BOT_TOKEN_AQUI");
        EnviarMensagem("Bot iniciado com sucesso, vamos comecar o massacrar com a tia blaze!");
        await Start();

    }

    public static async Task Start()
    {
        Console.WriteLine("Iniciou o Programa!");

        //await RetornaSeEstaRolando();

        //await RetornaUltimosResultados();

        await CheckEstrategias(EstrategiaManager.GetListaDeEstrategias());


    }


    //estrutura de codigo

    public static async Task CheckEstrategias(List<Estrategias> ListaDeEstrategias)
    {
        while (true)
        {
            Console.WriteLine("Rodou A Primeira Analise");
            Thread.Sleep(1500);

            var dataList = await RetornaUltimosResultados();

            for (int i = 0; i < ListaDeEstrategias.Count; i++)
            {
                if (ListaDeEstrategias[i].UtlimoResultado <= dataList.records[0].ultimoCrash && ListaDeEstrategias[i].PenultimoResultado <= dataList.records[1].ultimoCrash && ListaDeEstrategias[i].AntepenultimoResultado <= dataList.records[2].ultimoCrash )
                {
                    await CheckGreenEntrada(ListaDeEstrategias[i].ValorParaSairNaProx , dataList.records[0].id);
                }
                
            }
        }
    }

    public static async Task CheckGreenEntrada( float ValorParaSairNaProx , string idPartida)
    {
        while (true)
        {
            var resultadoEstaRolando = await RetornaSeEstaRolando();
            Thread.Sleep(1500);

            if (resultadoEstaRolando.status == "graphing")
            {
                EnviarMensagem(@"<b>🚨ENTRADA CONFIRMADA🚨</b>" +
                            "\n" +
                            "\n" +
                            "\n📊<b>ENTRAR ATE O MULTIPLICADOR:</b>:" +
                            "\n" + ValorParaSairNaProx +
                            "\n" +
                            "\n" +
                            "\n👩‍💻<a href='https://blaze.com/pt/games/crash' rel='nofollow'>ABRIR JOGO</a>👈👈👈 " +
                            "\n" +
                            "\n➡ <a href='http://blaze.com/r/d39Nn' rel='nofollow' >️ CLIQUE AQUI</a> E ABRA SUA CONTA!");
                entradasTotais += 1;
                await CheckVitoria(resultadoEstaRolando.id , ValorParaSairNaProx);

            }
            else if(resultadoEstaRolando.status == "waiting")
            {
                EnviarMensagem(@"<b>🚨ENTRADA CONFIRMADA🚨</b>" +
                            "\n" +
                            "\n" +
                            "\n📊<b>ENTRAR ATE O MULTIPLICADOR:</b>:" +
                            "\n" + ValorParaSairNaProx +
                            "\n" +
                            "\n" +
                            "\n👩‍💻<a href='https://blaze.com/pt/games/crash' rel='nofollow'>ABRIR JOGO</a>👈👈👈 " +
                            "\n" +
                            "\n➡ <a href='http://blaze.com/r/d39Nn' rel='nofollow' >️ CLIQUE AQUI</a> E ABRA SUA CONTA!");
                entradasTotais += 1;
                await CheckVitoria(resultadoEstaRolando.id, ValorParaSairNaProx);

            }
            else if (resultadoEstaRolando.status == "complete")
            {
                EnviarMensagem(@"<b>🚨ENTRADA CONFIRMADA🚨</b>" +
                            "\n" +
                            "\n" +
                            "\n📊<b>ENTRAR ATE O MULTIPLICADOR:</b>:" +
                            "\n" + ValorParaSairNaProx +
                            "\n" +
                            "\n" +
                            "\n👩‍💻<a href='https://blaze.com/pt/games/crash' rel='nofollow'>ABRIR JOGO</a>👈👈👈 " +
                            "\n" +
                            "\n➡ <a href='http://blaze.com/r/d39Nn' rel='nofollow' >️ CLIQUE AQUI</a> E ABRA SUA CONTA!");
                entradasTotais += 1;
                await CheckVitoria(resultadoEstaRolando.id, ValorParaSairNaProx);

            }
            else
            {
                continue;
            }

        }
        
    }

    public static async Task CheckVitoria(string id , float ValorParaSairNaProx )
    {
        while (true)
        {
            Thread.Sleep(1500);
            var dataList = await RetornaUltimosResultados();

            if(id == dataList.records[1].id)
            {
                await DecobreOResultado(dataList.records[0].id, ValorParaSairNaProx);
            }
            else
            {
                continue;
            }



        }
    }

    public static async Task DecobreOResultado(string id , float ValorParaSairNaProx)
    {
        while (true)
        {
            Thread.Sleep(1500);
            var dataList = await RetornaUltimosResultados();

            if(id == dataList.records[0].id)
            {
                if(dataList.records[0].ultimoCrash >= ValorParaSairNaProx)
                {
                    EnviarMensagem("✅✅✅");
                    entradasWins++;
                    EnviarMensagem("📊 RELATÓRIO 📊" +
                           "\nTotal de entradas: " + entradasTotais +
                           "\nTotal de win: " + entradasWins +
                           "\nTotal de loss: " + entradasLoss +
                           "\nTaxa de acertividade: " + RetornaPorcetagem() + "%"
                           );
                    await Start();
                }
                else
                {
                    EnviarMensagem("❌❌❌");
                    entradasLoss++;
                    EnviarMensagem("📊 RELATÓRIO 📊" +
                                    "\nTotal de entradas: " + entradasTotais +
                                    "\nTotal de win: " + entradasWins +
                                    "\nTotal de loss: " + entradasLoss +
                                    "\nTaxa de acertividade: " + RetornaPorcetagem() + "%"
                                    );
                    await Start();
                }
            }else if (id == dataList.records[1].id)
            {
                if (dataList.records[1].ultimoCrash >= ValorParaSairNaProx)
                {
                    EnviarMensagem("✅✅✅");
                    entradasWins++;
                    EnviarMensagem("📊 RELATÓRIO 📊" +
                                    "\nTotal de entradas: " + entradasTotais +
                                    "\nTotal de win: " + entradasWins +
                                     "\nTotal de loss: " + entradasLoss +
                                     "\nTaxa de acertividade: " + RetornaPorcetagem() + "%"
       );
                    await Start();
                }
                else
                {
                    EnviarMensagem("❌❌❌");
                    entradasLoss++;
                    EnviarMensagem("📊 RELATÓRIO 📊" +
                                   "\nTotal de entradas: " + entradasTotais +
                                   "\nTotal de win: " + entradasWins +
                                   "\nTotal de loss: " + entradasLoss +
                                   "\nTaxa de acertividade: " + RetornaPorcetagem() + "%"
       );
                    await Start();
                }
            }

        }
    }




    //funcoes


    public static string RetornaPorcetagem()
    {
        float resultado = entradasWins / entradasTotais * 100;
        Console.WriteLine("o resultado da porcentagem e" + resultado.ToString() + " e fomatado e : " + resultado.ToString("F2"));
        return resultado.ToString("F2");
    }

    public static async Task <Data> RetornaUltimosResultados()
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(ApiUrl);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var dataList = System.Text.Json.JsonSerializer.Deserialize<Data>(responseBody);

        var cultura = new CultureInfo("pt-BR");
        for (int i = 0; i < dataList.records.Count; i++)
        { 
                var numeroComVirgula = dataList.records[i].crash_point.Replace(".", ",");
                dataList.records[i].ultimoCrash = float.Parse(numeroComVirgula, cultura);
        }

        return dataList;
    }

    public static async Task<IsRunning> RetornaSeEstaRolando()
    {
        var client = new HttpClient();
        var response = await client.GetAsync("https://blaze.com/api/crash_games/current");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var dataList = System.Text.Json.JsonSerializer.Deserialize<IsRunning>(responseBody);


        Console.WriteLine($"o id e: {dataList.id} e o status e {dataList.status}");


        return dataList;

    }




    public static async void EnviarMensagem(string mensagem, string caminhoFoto = null)
    {
        long chatId = "ID_SALA_TELEGRAM";  // Substitua pelo ID do grupo em que deseja enviar a mensagem


        if (caminhoFoto == null)
        {
            await botClient.SendTextMessageAsync(chatId, mensagem, ParseMode.Html, disableWebPagePreview: true);
        }
        else
        {
            InputOnlineFile foto = new InputOnlineFile(caminhoFoto);
            await botClient.SendPhotoAsync(chatId, foto, caption: mensagem, ParseMode.Html);
        }
    }





}