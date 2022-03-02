using System;

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

            Console.Write("Informe o nome ou enter para sair: ");
            nome = Console.ReadLine();
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
            Console.Write("Informe a data de fabricação ou enter para sair: ");
            dataFabricacao = Console.ReadLine();
            if (dataFabricacao == "")
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

            MensagemTitulo("Editar Equipamentos", true, true);
            if (nomes[0] == null)
                MensagemAviso("Nenhum equipamento cadastrado", false, true);
            else
            {
                ImprimeListaEquipamentos();
                Mensagem("Informe o id que deseja alterar ou enter para voltar:", false, false);
                string lerTela = Console.ReadLine();
                if (lerTela == "")
                {
                    LimparConsole();
                    return;
                }
                int id = Convert.ToInt32(lerTela) - 1;
                Console.Write("Informe o nome ou aperte enter para manter a informação anterior: ");
                nome = Console.ReadLine();
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

        public static void ExcluirEquipamentos()
        {
            MensagemTitulo("Excluir Equipamentos", true, true);
            if (nomes[0] == null) { 
                MensagemAviso("Nenhum equipamento cadastrado, pressione enter para voltar", false, true);
                Console.ReadKey();
                LimparConsole();
            }
            else
            {
                ImprimeListaEquipamentos();

                Mensagem("Informe o id que deseja excluir ou enter para sair:", false, false);
                string lerTela = Console.ReadLine();
                if (lerTela == "")
                {
                    LimparConsole();
                    return;
                }
                int id = Convert.ToInt32(lerTela) - 1;
                if (ExisteNoArray(id))
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
            
            if (ExisteChamados() == false)
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
            if (nomes[0] == "" || nomes[0] == null)
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
            MensagemAviso("", false, true);
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
            Console.Write("Informe a data de abertura do chamado ou enter para sair: ");
            dataAbertura = Console.ReadLine();
            if (dataAbertura == "")
            {
                LimparConsole();
                return;
            }
            
            int posicaoInsert = SelectPosicaoInsercao();
            if (posicaoInsert != -1)
            {
                titulos[posicaoInsert] = titulo;
                descricoes[posicaoInsert] = descricao;
                equipamentoPosicaoReferencias[posicaoInsert] = equipamentoReferencia;
                dataAberturas[posicaoInsert] = dataAbertura;
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
            if (titulos[0] == null)
                MensagemAviso("Nenhum chamado cadastrado", false, true);
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
                Console.Write("Informe a nova data de abertura ou aperte enter para manter a informação anterior: ");
                dataAbertura = Console.ReadLine();
                
                if (titulo != null && titulo != "")
                    titulos[id] = titulo;
                if (descricao != null && descricao != "")
                    descricoes[id] = descricao;
                if (referenciaProduto != null && referenciaProduto != "")
                    equipamentoPosicaoReferencias[id] = Convert.ToInt32(referenciaProduto);
                if (dataAbertura != null && dataAbertura != "")
                    dataAberturas[id] = dataAbertura;
                MensagemSucesso("Equipamento editado com sucesso!", true, true);
            }
        }
        public static void ExcluirChamados()
        {
            MensagemTitulo("Excluir Equipamentos", true, true);
            if (titulos[0] == null)
            {
                MensagemAviso("Nenhum chamado cadastrado, pressione enter para voltar", false, true);
                Console.ReadKey();
                LimparConsole();
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
                if (ExisteNoArray(id))
                {
                    titulos[id] = "";
                    descricoes[id] = "";
                    equipamentoPosicaoReferencias[id] = default;
                    dataAberturas[id] = "";
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
        private static bool ExisteNoArray(int id)
        {
            if (nomes[id] != null && nomes[id] !="")
                return true;
            else
                return false;
        }
        #endregion
        #region Chamados
        private static void ImprimeListaChamados()
        {
            for (int i = 0; i < titulos.Length; i++)
            {
                if (titulos[i] != "" && titulos[i] != null)
                    Console.Write("ID: {0} | Título: {1} | Equipamento: {2} | Data de abertura: {3} | Dias em aberto: {4}\n", (i+1).ToString(), titulos[i], nomes[equipamentoPosicaoReferencias[i]], dataAberturas[i], dataAberturas[i]);
            }
            Console.WriteLine();
        }
        private static bool ExisteChamados()
        {
            for (int i = 0; i < titulos.Length; i++)
            {
                if (titulos[i] == "" || titulos[i] == null)
                    return false;
                else
                    return true;
            }
            return false;
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
        #endregion

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
                if (pularLinha == true)
                    Console.WriteLine(mensagem);
                else
                    Console.Write(mensagem);
                Console.ResetColor();
                Console.WriteLine("");
            }
            private static void MensagemErro(string mensagem, bool limparConsole, bool pularLinha)
            {
                if (limparConsole == true)
                    Console.Clear();
                Console.BackgroundColor = ConsoleColor.Red;
                if (pularLinha == true)
                    Console.WriteLine(mensagem);
                else
                    Console.Write(mensagem);
                Console.ResetColor();
                Console.WriteLine("");
            }
            private static void MensagemAviso(string mensagem, bool limparConsole, bool pularLinha)
            {
                if (limparConsole == true)
                    Console.Clear();
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                if (pularLinha == true)
                    Console.WriteLine(mensagem);
                else
                    Console.Write(mensagem);
                Console.ResetColor();
                Console.WriteLine("");
            }
            private static void MensagemTitulo(string mensagem, bool limparConsole, bool pularLinha)
        {
            if (limparConsole == true)
                Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            if (pularLinha == true)
                Console.WriteLine(mensagem);
            else
                Console.Write(mensagem);
            Console.ResetColor();
            Console.WriteLine("");
        }
            #endregion

        #endregion
    }
}