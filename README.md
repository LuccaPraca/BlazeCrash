<div id="top"></div>


<br />
<div align="center">
  <a style="text-decoration: none;" href="https://blaze.com/r/KOGDR9">
    <img src="https://imgur.com/tnBL4BP.png" alt="Logo" width="auto" height="80">
  </a>

  <h2 align="center">Blaze Crash</h2>
</div>

## 🤖 Bot Blaze Crash Com Sala de Sinais

#descricao#
Esse programa e uma ferramenta para automatizar suas análises e envio de mensagem em grupos/canal do telegram , com ele você adicionará os padrões que você irá seguir e a forma no qual o robô irá analisar e de forma automática pela API da blaze e enviará de forma automatica mensagens para seu grupo/canal os sinais escolhidos.

# requisitos #
-visual Studio 2022 community
-.net 6.0
-conhecimento básico em c# para alterar as estratégias

## instruções de uso ##
 # uso com as estratégias já programadas #
Para executar o programa basta baixar o projeto e subistituir as seguintes variaveis: 
botClient = new TelegramBotClient("TELEGRAM_BOT_TOKEN_AQUI"); , e subistituir "TELEGRAM_BOT_TOKEN_AQUI" pelo token do bot do telegram criado no BotFather.
long chatId = "ID_SALA_TELEGRAM"; , e subistituir "ID_SALA_TELEGRAM" pelo id do grupo/canal do telegram obitido encaminhando uma mensagem do grupo para o bot JsonDumpBot no privado.

 # uso com estrategias personalizadas #
Para personalizar suas estrategias, alem de seguir os passos acima, voce deve ter um conhecimento basico de c#...
 basta criar uma estrategia dentro da classe EstrategiaManager seguindo o exemplo:

        Estrategias estrategiaExemplo = new Estrategias() { UtlimoResultado = 10.55f, PenultimoResultado = 1.50f, AntepenultimoResultado = 1.50f, AnteantepenultimoResultado = 1.05f, ValorParaSairNaProx = 3.25f };

E adiciononalo na lista de estrategia para ser exportado, segunido o exemplo:

        Listadeestrategias.Add(estrategiaExemplo);

Apos seguir esse passo , e so ir na funcao CheckEstrategias(List<Estrategias> ListaDeEstrategias) e adicionar a logica deseja para conferencia, sendo que quando a a logica bater chamar a funcao CheckGreenEntrada() passando os parametros ValorParaSairNaProx e id_ultima_jogada_catalogada, exemplo:

        if (ListaDeEstrategias[i].UtlimoResultado <= dataList.records[0].ultimoCrash && ListaDeEstrategias[i].PenultimoResultado <= dataList.records[1].ultimoCrash && ListaDeEstrategias[i].AntepenultimoResultado <= dataList.records[2].ultimoCrash & ListaDeEstrategias[i].AnteantepenultimoResultado <= dataList.records[3].ultimoCrash)
                {
                    await CheckGreenEntrada(ListaDeEstrategias[i].ValorParaSairNaProx , dataList.records[0].id);
                }
# contato # 

telegram: @Luccapraca
