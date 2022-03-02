using System;
// Validar nome com pelo menos 6 caracteres
// Validar se existe chamado em aberto com o equipamento a ser excluído
//
namespace GestaoEquipamentosConsoleApp
{
    internal class Program
    {
        static bool sairSistema;

        // base cadastro equipamentos
        static string[] nomes = new string[100], seriais = new string[100], fabricacaoDatas = new string[100], fabricantes = new string[100];
        static decimal[] precoAquisicoes = new decimal[100];
        
        // base controle de chamados
        static string[] titulos = new string[100], descricoes = new string[100], dataAberturas = new string[100];
        static int[] equipamentoPosicaoReferencias = new int[100];
        static DateTime[] dataAberturaChamados = new DateTime[100];
        static void Main(string[] args)
        {
            char opcaoMenu;
            do
            {
                MenuOpcoes(out opcaoMenu);
                ChamaMetodos(opcaoMenu);
            } while (sairSistema != true);
        }

        #region Metodos equipamentos
        public static void ListarEquipamentos()
        {
            LimparConsole();
            MensagemTitulo("Listar equipamentos", true, true);
            if (nomes[0] != "" && nomes[0] != null)
                ImprimeListaEquipamentos();
            else
                MensagemAviso("Nenhum produto cadastrado", false, true);
            
            Mensagem("Pressione enter para voltar para o menu", false, true);
            Console.ReadKey();
            LimparConsole();
        }

        public static void CadastrarEquipamentos()
        {
            string nome, serie, dataFabricacao, fabricante;
            decimal precoAquisicao;

            MensagemTitulo("Cadastrar Equipamentos", true, true);
            Mensagem("Informe o nome com mais no mínimo 6 caracteres ou enter para sair: ", false, false);
            nome = Console.ReadLine();
            if(nome.Length > 0 && nome.Length < 6)
            {
                string nomeValidacao;
                do
                {
                    MensagemAviso("", false, true);
                    MensagemAviso("Caracter deve conter pelo menos 6 caracteres: ", false, true);
                    Mensagem("Informe o nome com no mínimo 6 caracteres ou enter para sair: ", false, true);
                    nome = Console.ReadLine();
                    if (nome == "")
                    {
                        LimparConsole();
                        return;
                    }
                    nomeValidacao = nome;
                } while (nomeValidacao.Trim().Length <= 5);
            }
            if (nome == "")
            {
                LimparConsole();
                return;
            }
            Console.Write("Informe o numero de serie ou enter para sair: ");
            serie = Console.ReadLine();
            if (serie == "")
            {
                LimparConsole();
                return;
            }
            Console.Write("Informe o fabricante ou enter para sair: ");
            fabricante = Console.ReadLine();
            if (fabricante == "")
            {
                LimparConsole();
                return;
            }
            Console.Write("Informe o ano de fabricação ou enter para sair: ");
            dataFabricacao = Console.ReadLine();
            if (dataFabricacao == "")
            {
                LimparConsole();
                return;
            }
            Console.Write("Informe o preço de aquisição ou enter para sair: ");
            string lerTela = Console.ReadLine();
            if (fabricante == "")
            {
                LimparConsole();
                return;
            }
            precoAquisicao = Convert.ToDecimal(lerTela);
            
            int posicaoInsert = SelectIdToInsert();
            if (posicaoInsert != -1)
            {
                nomes[posicaoInsert] = nome;
                seriais[posicaoInsert] = serie;
                fabricacaoDatas[posicaoInsert] = dataFabricacao;
                fabricantes[posicaoInsert] = fabricante;
                precoAquisicoes[posicaoInsert] = precoAquisicao;
                LimparConsole();
                MensagemSucesso("Equipamento cadastrado com sucesso!", false, true);
            }
            else 
            { 
                MensagemAviso("Base de dados está cheia, contate o administrador.", false, true);
                Console.ReadKey();
                LimparConsole();
            }
        }

        public static void EditarEquipamentos()
        {
            string nome, serie, dataFabricacao, fabricante, preco;
            decimal precoAquisicao;
            bool encerrarMetodo = false;
            int id = default;

            MensagemTitulo("Editar Equipamentos", true, true);
            if (ExistePosicaoPreenchidaArrayEquipamentos() == false) { 
                MensagemAviso("Nenhum equipamento cadastrado, pressione enter para voltar ao menu.", false, true);
                Console.ReadKey();
                LimparConsole();
            }
            else
            {
                ImprimeListaEquipamentos();

                bool existeIdEquipamento = false;
                string lerTela;
                while (existeIdEquipamento == false)
                {
                    Mensagem("Informe o id que deseja alterar ou enter para voltar: ", false, false);
                    lerTela = Console.ReadLine();
                    if (lerTela == "")
                    {
                        LimparConsole();
                        encerrarMetodo = true;
                        return;
                    }
                    id = Convert.ToInt32(lerTela) - 1;
                    if (ExisteNoArrayEquipamentos(id) == true)
                    {
                        existeIdEquipamento = true;
                    }
                    else
                    {
                        MensagemAviso("\nID não encontrado, escolha o ID novamente", false, true);
                    }
                        
                }
                
                if(encerrarMetodo == false)
                {
                    string nomeValidacao;
                    do
                    {
                        Mensagem("\nInforme o nome com mais de 6 caracteres ou aperte enter para manter a informação anterior: ", false, false);
                        nome = Console.ReadLine();
                        nomeValidacao = nome;
                        if (nomeValidacao.Trim().Length > 0 && nomeValidacao.Trim().Length <= 5)
                            MensagemAviso("\nNome informado contem menos de 6 caracteres", false, true);
                    } while (nomeValidacao.Trim().Length > 0 && nomeValidacao.Trim().Length <= 5);

                    Console.Write("Informe o serie ou aperte enter para manter a informação anterior: ");
                    serie = Console.ReadLine();
                    Console.Write("Informe o sabricação ou aperte enter para manter a informação anterior: ");
                    dataFabricacao = Console.ReadLine();
                    Console.Write("Informe o fabricante ou aperte enter para manter a informação anterior: ");
                    fabricante = Console.ReadLine();
                    Console.Write("Informe o preço de aquisição ou aperte enter para manter a informação anterior: ");
                    preco = Console.ReadLine();

                    if (nome != null && nome != "")
                        nomes[id] = nome;
                    if (serie != null && serie != "")
                        seriais[id] = serie;
                    if (dataFabricacao != null && dataFabricacao != "")
                        fabricacaoDatas[id] = dataFabricacao;
                    if (fabricante != null && fabricante != "")
                        fabricantes[id] = fabricante;
                    if (preco != null && preco != "")
                        precoAquisicoes[id] = Convert.ToDecimal(preco);

                    MensagemSucesso("Equipamento editado com sucesso!", true, true);
                }
            }
        }

        public static void ExcluirEquipamentos()
        {
            MensagemTitulo("Excluir Equipamentos", true, true);
            
            if(ExistePosicaoPreenchidaArrayEquipamentos() == false)
            {
                Mensagem("Nenhum item cadastrado, pressione enter e volte ao menu.", false, true);
                Console.ReadKey();
                LimparConsole();
            }
            else
            {
                Mensagem("Segue lista de equipamentos cadastrados.", false, true);
                ImprimeListaEquipamentos();

                Mensagem("Informe o id que deseja excluir ou enter para sair:", false, false);
                string lerTela = Console.ReadLine();
                if (lerTela == "")
                {
                    LimparConsole();
                    return;
                }
                int id = Convert.ToInt32(lerTela) - 1;
                if (ExisteNoArrayEquipamentos(id))
                {
                    if (ExisteChamadoParaEsseEquipamento(id) == false)
                    {
                        nomes[id] = "";
                        seriais[id] = "";
                        fabricacaoDatas[id] = "";
                        fabricantes[id] = "";
                        precoAquisicoes[id] = default;
                        MensagemSucesso("Equipamento excluído com sucesso!", true, true);
                    }
                    else
                    {
                        MensagemAviso("Equipamento não excluído, pois o mesmo encontra-se vínculado a um chamado.", true, true);
                    }
                }
                else
                {
                    MensagemErro("Item não encontrado.", true, true);
                }
            }
        }

        #endregion

        #region Metodos Chamados
        public static void ListarChamados()
        {
            LimparConsole();
            MensagemTitulo("Listar Chamados", true, true);
            
            if (ExistePosicaoPreenchidaArrayChamados() == false)
            {
                MensagemAviso("Não existe chamados cadastrados, pressione enter para voltar para o menu", false, true);
                Console.ReadKey();
                LimparConsole();
                return;
            }

            ImprimeListaChamados();
            Mensagem("Pressione enter para voltar ao menu principal", false, true);
            Console.ReadKey();
            LimparConsole();
        }
        public static void CadastrarChamados()
        {
            string titulo, descricao, dataAbertura, equipamentoReferenciaLerTela;
            int equipamentoReferencia;

            MensagemTitulo("Cadastrar Chamados", true, true);
            if (ExistePosicaoPreenchidaArrayEquipamentos() == false)
            {
                MensagemAviso("Nenhum equipamento cadastrado, pressione enter para voltar ao menu", false, true);
                Console.ReadKey();
                LimparConsole();
                return;
            }
                
            Console.Write("Informe o título ou enter para sair: ");
            titulo = Console.ReadLine();
            if (titulo == "")
            {
                LimparConsole();
                return;
            }
            Console.Write("Informe a descrição ou enter para sair: ");
            descricao = Console.ReadLine();
            if (descricao == "")
            {
                LimparConsole();
                return;
            }
            MensagemAviso("Equipamentos cadastrados", false, true);
            ImprimeListaEquipamentos();
            Console.Write("Informe a referência do equipamento (EX: 1) ou enter para sair: ");
            equipamentoReferenciaLerTela = Console.ReadLine();
            if (equipamentoReferenciaLerTela == "")
            {
                LimparConsole();
                return;
            }
            equipamentoReferencia = Convert.ToInt32(equipamentoReferenciaLerTela) - 1;
            if (nomes[equipamentoReferencia] == null)
            {
                MensagemAviso("Equipamento não encontrado", false, true);
                Console.ReadKey();
                LimparConsole();
                return;
            }
            Console.Write("Informe a data de abertura do chamado (EX: 00/00/0000) ou enter para sair: ");
            dataAbertura = Console.ReadLine();
            if (equipamentoReferenciaLerTela == "")
            {
                LimparConsole();
                return;
            }

            int posicaoInsert = SelectPosicaoInsercao();
            if (posicaoInsert != -1)
            {
                string[] dataSeparada = dataAbertura.Split("/");
                int dia = Convert.ToInt32(dataSeparada[0]);
                int mes = Convert.ToInt32(dataSeparada[1]);
                int ano = Convert.ToInt32(dataSeparada[2]);

                DateTime dataCriacaoChamado = new DateTime(ano, mes, dia);

                titulos[posicaoInsert] = titulo;
                descricoes[posicaoInsert] = descricao;
                equipamentoPosicaoReferencias[posicaoInsert] = equipamentoReferencia;
                dataAberturas[posicaoInsert] = dataAbertura;
                dataAberturaChamados[posicaoInsert] = dataCriacaoChamado;
                LimparConsole();
                MensagemSucesso("Equipamento cadastrado com sucesso!", false, true);
            }
            else
            {
                MensagemAviso("Base de dados está cheia, contate o administrador.", false, true);
                Console.ReadKey();
                LimparConsole();
            }
        }
        public static void EditarChamados()
        {
            string titulo, descricao, referenciaProduto, dataAbertura;
            int idReferenciaProduto;

            LimparConsole();

            MensagemTitulo("Editar Chamados", true, true);
            if (ExistePosicaoPreenchidaArrayChamados() == false) 
            { 
                MensagemAviso("Nenhum chamado cadastrado, pressione enter para voltar ao menu.", false, true);
                Console.ReadKey();
                LimparConsole();
                return;
            }
            else
            {
                ImprimeListaChamados();
                Mensagem("Informe o id que deseja alterar ou enter para voltar:", false, false);
                string lerTela = Console.ReadLine();
                if (lerTela == "")
                {
                    LimparConsole();
                    return;
                }

                MensagemTitulo("Editar Chamado", true, true);
                ImprimeListaEquipamentos();

                int id = Convert.ToInt32(lerTela) - 1;
                Console.Write("Informe o título ou aperte enter para manter a informação anterior: ");
                titulo = Console.ReadLine();
                Console.Write("Informe a descrição ou aperte enter para manter a informação anterior: ");
                descricao = Console.ReadLine();
                Console.Write("Informe a nova referência do produto ou aperte enter para manter a informação anterior: ");
                referenciaProduto = Console.ReadLine();
                Console.Write("Informe a nova data de abertura (EX: 00/00/0000) ou aperte enter para manter a informação anterior: ");
                dataAbertura = Console.ReadLine();

                if (titulo != null && titulo != "")
                    titulos[id] = titulo;
                if (descricao != null && descricao != "")
                    descricoes[id] = descricao;
                if (referenciaProduto != null && referenciaProduto != "")
                    equipamentoPosicaoReferencias[id] = Convert.ToInt32(referenciaProduto);
                if (dataAbertura != null && dataAbertura != "")
                {
                    string[] dataSeparada = dataAbertura.Split("/");
                    int dia = Convert.ToInt32(dataSeparada[0]);
                    int mes = Convert.ToInt32(dataSeparada[1]);
                    int ano = Convert.ToInt32(dataSeparada[2]);

                    DateTime dataCriacaoChamado = new DateTime(ano, mes, dia);

                    dataAberturas[id] = dataAbertura;
                    dataAberturaChamados[id] = dataCriacaoChamado;
                }
                    
                MensagemSucesso("Equipamento editado com sucesso!", true, true);
            }
        }
        public static void ExcluirChamados()
        {
            MensagemTitulo("Excluir Equipamentos", true, true);
            if (ExistePosicaoPreenchidaArrayChamados() == false)
            {
                MensagemAviso("Nenhum chamado cadastrado, pressione enter para voltar.", false, true);
                Console.ReadKey();
                LimparConsole();
                return;
            }
            else
            {
                ImprimeListaChamados();

                Mensagem("Informe o id que deseja excluir ou enter para sair:", false, false);
                string lerTela = Console.ReadLine();
                if (lerTela == "")
                {
                    LimparConsole();
                    return;
                }
                int id = Convert.ToInt32(lerTela) - 1;
                if (ExisteNoArrayChamados(id))
                {
                    titulos[id] = "";
                    descricoes[id] = "";
                    equipamentoPosicaoReferencias[id] = default;
                    dataAberturas[id] = "";
                    dataAberturaChamados[id] = default;
                    MensagemSucesso("Equipamento excluído com sucesso!", true, true);
                }
                else
                {
                    MensagemErro("Item não encontrado.", true, true);
                }
            }
        }
       
        #endregion

        #region Metodos Menu
        static void MenuOpcoes(out char opcaoSelecionada)
        {
            Console.WriteLine("Segue abaixo as opções disponíveis no sistema");
            Console.WriteLine("1- Listas Equipamentos");
            Console.WriteLine("2- Cadastrar Equipamentos");
            Console.WriteLine("3- Editar Equipamentos");
            Console.WriteLine("4- Excluir Equipamentos");
            Console.WriteLine("5- Listas Chamados");
            Console.WriteLine("6- Cadastrar Chamados");
            Console.WriteLine("7- Editar Chamados");
            Console.WriteLine("8- Excluir Chamados");
            Console.WriteLine("0- Sair do sistema");

            Console.Write("Informa a opção desejada: ");
            opcaoSelecionada = Convert.ToChar(Console.ReadLine());
        }
        public static void ChamaMetodos(char opcaoMenu)
        {
            switch (opcaoMenu)
            {
                case '0':
                    SairSistema();
                    break;
                case '1':
                    ListarEquipamentos();
                    break;
                case '2':
                    CadastrarEquipamentos();
                    break;
                case '3':
                    EditarEquipamentos();
                    break;
                case '4':
                    ExcluirEquipamentos();
                    break;
                case '5':
                    ListarChamados();
                    break;
                case '6':
                    CadastrarChamados();
                    break;
                case '7':
                    EditarChamados();
                    break;
                case '8':
                    ExcluirChamados();
                    break;
                default:
                    OpcaoInvalida();
                    break;
            }
        }
        private static void OpcaoInvalida()
        {
            PrintarMensagemColorida("Parâmetro informado não corresponde as opções disponívels", "vermelho", true, true);
        }
        private static void SairSistema()
        {
            PrintarMensagemColorida("O sistema sera fechado", "amarelo", true, true);
            Console.ReadKey();
            sairSistema = true;
        }
        private static void LimparConsole()
        {
            Console.Clear();
        }
        #endregion

        #region Metodos auxiliares
        #region Equipamentos
        private static void ImprimeListaEquipamentos()
        {
            for (int i = 0; i < nomes.Length; i++)
            {
                if(nomes[i] != "" && nomes[i] != null)
                    Console.Write("ID: {5} | Produto: {0} | Numero de série: {1} | Data de fabricação: {2} | fabricante: {3} | preço: {4} \n", nomes[i], seriais[i], fabricacaoDatas[i], fabricantes[i], precoAquisicoes[i].ToString(), (i + 1).ToString());
            }
            Console.WriteLine();
        }
        private static int SelectIdToInsert()
        {
            for (int i = 0; i < nomes.Length; i++)
            {
                if (nomes[i] == "" || nomes[i] == null)
                    return i;
            }
            return -1;
        }
        private static bool ExisteNoArrayEquipamentos(int id)
        {
            if (nomes[id] != ""  && nomes[id] != null && nomes[id].Length > 5)
                return true;
            else
                return false;
        }
        private static bool ExistePosicaoPreenchidaArrayEquipamentos()
        {
            for (int i = 0; i < nomes.Length; i++)
            {
                if(nomes[i] != "" && nomes[i] != null)
                {
                    return true;
                }
            }
            return false;
        }
        private static bool ExisteChamadoParaEsseEquipamento(int idEquipamento)
        {
            for (int i = 0; i < equipamentoPosicaoReferencias.Length; i++)
            {
                if (equipamentoPosicaoReferencias[i] == idEquipamento)
                {
                    return true;
                }
                    
            }
            
            return false;
        }
        #endregion
        #region Chamados
        private static void ImprimeListaChamados()
        {
            for (int i = 0; i < titulos.Length; i++)
            {
                if (titulos[i] != "" && titulos[i] != null)
                {
                    string[] dataSeparada = dataAberturas[i].Split("/");
                    int dia = Convert.ToInt32(dataSeparada[0]);
                    int mes = Convert.ToInt32(dataSeparada[1]);
                    int ano = Convert.ToInt32(dataSeparada[2]);

                    DateTime dataCriacaoChamado = new DateTime(ano, mes, dia);

                    DateTime dataAtual = DateTime.Now;
                    TimeSpan periodoTempo = dataAtual - dataCriacaoChamado;
                    int diferencaData = periodoTempo.Days;
                    Console.Write("ID: {0} | Título: {1} | Equipamento: {2} | Data de abertura: {3} | Dias em aberto: {4}\n", (i+1).ToString(), titulos[i], nomes[equipamentoPosicaoReferencias[i]], dataAberturas[i], diferencaData);
                }
            }
            Console.WriteLine();
        }
        static int SelectPosicaoInsercao()
        {
            for (int i = 0; i < titulos.Length; i++)
            {
                if (titulos[i] == "" || titulos[i] == null)
                    return i;
            }
            return -1;
        }
        private static bool ExisteNoArrayChamados(int id)
        {
            if (titulos[id] != ""  && titulos[id] != null)
                return true;
            else
                return false;
        }
        private static bool ExistePosicaoPreenchidaArrayChamados()
        {
            for (int i = 0; i < titulos.Length; i++)
            {
                if (titulos[i] != "" && titulos[i] != null)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Mensagens
        private static void PrintarMensagemColorida(string mensagem, string cor, bool limparConsole, bool pularLinha)
        {
            if (limparConsole == true)
                Console.Clear();
            switch (cor)
            {
                case "vermelho":
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;
                case "amarelo":
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;
                case "azul":
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    break;
                case "verde":
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;
                case "preto":
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
            }
            Console.WriteLine("");
            if(pularLinha == true)
                Console.WriteLine(mensagem);
            else
                Console.Write(mensagem);
            Console.ResetColor();
        }
        private static void Mensagem(string mensagem, bool limparConsole, bool pularLinha)
        {
            if (limparConsole == true)
                Console.Clear();
            if (pularLinha == true) { 
                Console.WriteLine(mensagem);
                Console.WriteLine("");
            }
            else
                Console.Write(mensagem);
        }
        private static void MensagemSucesso(string mensagem, bool limparConsole, bool pularLinha)
        {
            if (limparConsole == true)
                Console.Clear();
            Console.BackgroundColor = ConsoleColor.Green;
            if (pularLinha == true) { 
                Console.WriteLine(mensagem);
                Console.WriteLine("");
            }
            else
                Console.Write(mensagem);
            Console.ResetColor();
        }
        private static void MensagemErro(string mensagem, bool limparConsole, bool pularLinha)
        {
            if (limparConsole == true)
                Console.Clear();
            Console.BackgroundColor = ConsoleColor.Red;
            if (pularLinha == true) { 
                Console.WriteLine(mensagem);
                Console.WriteLine("");
            }
            else
                Console.Write(mensagem);
            Console.ResetColor();
        }
        private static void MensagemAviso(string mensagem, bool limparConsole, bool pularLinha)
        {
            if (limparConsole == true)
                Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            if (pularLinha == true)
            {
                Console.WriteLine(mensagem);
                Console.WriteLine("");
            }
            else
                Console.Write(mensagem);
            Console.ResetColor();
            
        }
        private static void MensagemTitulo(string mensagem, bool limparConsole, bool pularLinha)
        {
        if (limparConsole == true)
            Console.Clear();
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        if (pularLinha == true)
        {
            Console.WriteLine(mensagem);
            Console.WriteLine("");
        }
        else
            Console.Write(mensagem);
        Console.ResetColor();
        }
        #endregion

        #endregion
    }
}