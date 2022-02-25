using System;

namespace GestaoEquipamentosConsoleApp
{
    internal class Program
    {
        static bool sairSistema;

        // base cadastro equipamentos
        static string[] nomes, seriais, fabricacaoDatas, fabricantes;
        static decimal[] precoAquisicoes;

        // base controle de chamados
        static string[] titulo, descricao, equipamentoPosicaoReferencia, dataAbertura;
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
        #region visualizar/listar equipamentos
        public static void VisualizarEquipamentos()
        {
            LimpaMenu();
            if(nomes != null)
            {
                ListarEquipamentos();
            }
            else
            {
                PrintarMensagemColorida("Nenhum produto cadastrado", "amarelo", true, true);
            }
            PrintarMensagemColorida("Pressione enter para voltar para o menu", "preto", false, true);
            Console.ReadKey();
            PrintarMensagemColorida("", "preto", true, true);
        }

        private static void ListarEquipamentos()
        {
            PrintarMensagemColorida("Lista equipamentos", "amarelo", true, true);
            for (int i = 0; i < nomes.Length; i++)
                Console.Write("ID: {5} | Produto: {0} | Numero de série: {1} | Data de fabricação: {2} | fabricante: {3} | preço: {4} \n", nomes[i], seriais[i], fabricacaoDatas[i], fabricantes[i], precoAquisicoes[i].ToString(),(i + 1).ToString());
        }
        #endregion
        #region cadastrar equipamentos
        public static void CadastrarEquipamentos()
        {
            string nome, serie, dataFabricacao, fabricante;
            decimal precoAquisicao; 
            PrintarMensagemColorida("Cadastrar equipamento", "preto", true, true);
            Console.Write("Informe o nome: ");
            nome = Console.ReadLine();
            Console.Write("Informe o numero de serie: ");
            serie = Console.ReadLine();
            Console.Write("Informe a data de fabricação: ");
            dataFabricacao = Console.ReadLine();
            Console.Write("Informe o fabricante: ");
            fabricante = Console.ReadLine();
            Console.Write("Informe o preço de aquisição: ");
            precoAquisicao = Convert.ToDecimal(Console.ReadLine());
            if (nomes != null)
                AdicionaItemArray(nome, serie, dataFabricacao, fabricante, precoAquisicao);
            else
                CriaArrayEquipamentos(nome, serie, dataFabricacao, fabricante, precoAquisicao);
            

            LimpaMenu();
            PrintarMensagemColorida("Cadastro realizado com sucesso", "azul", true, true);

        }

        private static void CriaArrayEquipamentos(string nome, string serie, string dataFabricacao, string fabricante, decimal precoAquisicao)
        {
            nomes =  new string[1] { nome };
            seriais =  new string[1] { serie };
            fabricacaoDatas = new string[1] { dataFabricacao };
            fabricantes = new string[1] { fabricante };
            precoAquisicoes = new decimal[1] { precoAquisicao };
        }

        private static void AdicionaItemArray(string nome, string serie, string dataFabricacao, string fabricante, decimal precoAquisicao)
        {
            string[] nomesAux = new string[nomes.Length + 1];
            string[] seriaisAux = new string[seriais.Length + 1];
            string[] fabricacaoDatasAux = new string[fabricacaoDatas.Length + 1];
            string[] fabricantesAux = new string[fabricantes.Length + 1];
            decimal[] precoAquisicoesAux = new decimal[precoAquisicoes.Length + 1];
            for (int z = 0; z < nomes.Length; z++)
            {
                nomesAux[z] = nomes[z];
                seriaisAux[z] = seriais[z];
                fabricacaoDatasAux[z] = fabricacaoDatas[z];
                fabricantesAux[z] = fabricantes[z];
                precoAquisicoesAux[z] = precoAquisicoes[z];
            }
            for (int y = (nomesAux.Length - 1); y < nomesAux.Length; y++)
            {
                nomesAux[y] = nome;
                seriaisAux[y] = serie;
                fabricacaoDatasAux[y] = dataFabricacao;
                fabricantesAux[y] = fabricante;
                precoAquisicoesAux[y] = precoAquisicao;
            }
            Array.Resize(ref nomes, nomesAux.Length);
            Array.Resize(ref seriais, nomesAux.Length);
            Array.Resize(ref fabricacaoDatas, nomesAux.Length);
            Array.Resize(ref fabricantes, nomesAux.Length);
            Array.Resize(ref precoAquisicoes, nomesAux.Length);
            for (int y = 0; y < nomesAux.Length; y++)
            {
                nomes[y] = nomesAux[y];
                seriais[y] = seriaisAux[y];
                fabricacaoDatas[y] = fabricacaoDatasAux[y];
                fabricantes[y] = fabricantesAux[y];
                precoAquisicoes[y] = precoAquisicoesAux[y];
            }
        }
        #endregion
        #region Editar equipamentos
        public static void EditarEquipamentos()
        {
            PrintarMensagemColorida("Editar equipamentos", "verde", false, true);
            if(nomes != null) {
                ListarEquipamentos();
                PrintarMensagemColorida("De acordo com o ID da lista, informe equipamento deseja editar (EX: 1):", "amarela", false, false);
                int opcaoEditar = Convert.ToInt32(Console.ReadLine()) - 1;
                if(opcaoEditar == 0)
                {
                    
                }
            }
            else
            {
                PrintarMensagemColorida("Nenhum item cadastrado, precione enter para voltas ao menu", "vermelho", true, true);
                Console.ReadKey();
            }
        }
        #endregion
        #region Excluir equipamentos
        public static void ExcluirEquipamentos()
        {
            LimpaMenu();
            PrintarMensagemColorida("Excluir equipamentos", "vermelho", false, true);
        }
        #endregion
        #endregion

        #region Metodos Chamados
        public static void CadastrarChamados()
        {
            LimpaMenu();
            Console.WriteLine("Cadastrar Chamados");
        }
        public static void VisualizarChamados()
        {
            LimpaMenu();
            Console.WriteLine("Visualizar Chamados");
        }
        public static void EditarChamados()
        {
            LimpaMenu();
            Console.WriteLine("Editar Chamados");
        }
        public static void ExcluirChamados()
        {
            LimpaMenu();
            Console.WriteLine("Excluir Chamados");
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
        private static void LimpaMenu()
        {
            Console.Clear();
        }
        #endregion

        #region Metodos auxiliares
        public static void ChamaMetodos(char opcaoMenu)
        {
            switch (opcaoMenu)
            {
                case '0':
                    SairSistema();
                    break;
                case '1':
                    VisualizarEquipamentos();
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
                    VisualizarEquipamentos();
                    break;
                case '6':
                    CadastrarEquipamentos();
                    break;
                case '7':
                    EditarEquipamentos();
                    break;
                case '8':
                    ExcluirEquipamentos();
                    break;
                default:
                    OpcaoInvalida();
                    break;

            }
        }
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
        #endregion
    }
}