using System;
using System.Text.RegularExpressions;
// Validar nome com pelo menos 6 caracteres
// Validar se existe chamado em aberto com o equipamento a ser excluído
//
namespace GestaoEquipamentosConsoleApp
{
    internal class Program
    {
        static bool sairSistema = false;

        // base cadastro equipamentos
        static string[] equipamentoNomes = new string[100], equipamentoSeriais = new string[100], equipamentoFabricacaoDatas = new string[100], equipamentoFabricantes = new string[100];
        static decimal[] equipamentoPrecoAquisicoes = new decimal[100];
        
        // base controle de chamados
        static string[] titulos = new string[100], descricoes = new string[100], dataAberturas = new string[100];
        static int[] equipamentoPosicaoReferencias = new int[100];
        static DateTime[] dataAberturaChamados = new DateTime[100];

        // base cadastro Solicitantes
        static string[] solicitanteNomes = new string[100], solicitanteEmails = new string[100], solicitanteTelefones= new string[100];
        
        static void Main(string[] args)
        {
            char telaSelecionada;
            char menuSelecionado;
            char opcaoMenu;
            do
            {
                MenuTelasPrincipais(out telaSelecionada);
                if (sairSistema != true && telaSelecionada != '0')
                {
                    MenuCrud(out menuSelecionado, telaSelecionada);
                    ChamaMetodos(telaSelecionada, menuSelecionado);
                }
            } while (sairSistema != true);
        }

        #region Metodos equipamentos
        public static void ListarEquipamentos()
        {
            LimparConsole();
            TituloTelas("Listar equipamentos", true, true);
            if (equipamentoNomes[0] != "" && equipamentoNomes[0] != null)
            {
                ImprimeListaEquipamentos();
                Mensagem("Pressione enter para voltar para o menu", false, true);
            }
            else
                MensagemAviso("- Nenhum produto cadastrado, pressione enter para voltar para o menu.", false, true);
            
            Console.ReadKey();
            LimparConsole();
        }

        public static void CadastrarEquipamentos()
        {
            string nome, serie, dataFabricacao, fabricante;
            decimal precoAquisicao;

            TituloTelas("Cadastrar Equipamentos", true, true);
            Mensagem("Informe o nome com no mínimo 6 caracteres ou enter para sair: ", false, false);
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
                equipamentoNomes[posicaoInsert] = nome;
                equipamentoSeriais[posicaoInsert] = serie;
                equipamentoFabricacaoDatas[posicaoInsert] = dataFabricacao;
                equipamentoFabricantes[posicaoInsert] = fabricante;
                equipamentoPrecoAquisicoes[posicaoInsert] = precoAquisicao;
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

            TituloTelas("Editar Equipamentos", true, true);
            if (ExisteChamadosComEsseProduto() == false) { 
                MensagemAviso("- Nenhum equipamento cadastrado, pressione enter para voltar ao menu.", false, true);
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
                    do
                    {
                        Mensagem("Informe o id que deseja alterar ou enter para voltar: ", false, false);
                        lerTela = Console.ReadLine();
                    }
                    while (lerTela != "" && lerTela.Length != 1);

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
                        Mensagem("Informe o nome com mais de 6 caracteres ou aperte enter para manter a informação anterior: ", false, false);
                        nome = Console.ReadLine();
                        nomeValidacao = nome;
                        if (nomeValidacao.Trim().Length > 0 && nomeValidacao.Trim().Length <= 5)
                            MensagemAviso("Nome informado contem menos de 6 caracteres\n", false, false);
                    } while (nomeValidacao.Trim().Length > 0 && nomeValidacao.Trim().Length <= 5);

                    Console.Write("Informe o serial ou aperte enter para manter a informação anterior: ");
                    serie = Console.ReadLine();
                    Console.Write("Informe o fabricante ou aperte enter para manter a informação anterior: ");
                    fabricante = Console.ReadLine();
                    Console.Write("Informe o ano de fabricação ou aperte enter para manter a informação anterior: ");
                    dataFabricacao = Console.ReadLine();
                    Console.Write("Informe o preço de aquisição ou aperte enter para manter a informação anterior: ");
                    preco = Console.ReadLine();

                    if (nome != null && nome != "")
                        equipamentoNomes[id] = nome;
                    if (serie != null && serie != "")
                        equipamentoSeriais[id] = serie;
                    if (dataFabricacao != null && dataFabricacao != "")
                        equipamentoFabricacaoDatas[id] = dataFabricacao;
                    if (fabricante != null && fabricante != "")
                        equipamentoFabricantes[id] = fabricante;
                    if (preco != null && preco != "")
                        equipamentoPrecoAquisicoes[id] = Convert.ToDecimal(preco);

                    MensagemSucesso("Equipamento editado com sucesso!", true, true);
                }
            }
        }

        public static void ExcluirEquipamentos()
        {
            string lerTela;
            TituloTelas("Excluir Equipamentos", true, true);
            
            if(ExisteChamadosComEsseProduto() == false)
            {
                MensagemAviso("- Nenhum item cadastrado, pressione enter e volte ao menu.", false, true);
                Console.ReadKey();
                LimparConsole();
            }
            else
            {
                Mensagem("Segue lista de equipamentos cadastrados.", false, true);
                ImprimeListaEquipamentos();

                do
                {
                    Mensagem("Informe o id que deseja excluir ou enter para sair: ", false, false);
                    lerTela = Console.ReadLine();
                }
                while (lerTela != "" && lerTela.Length != 1);

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
                        equipamentoNomes[id] = "";
                        equipamentoSeriais[id] = "";
                        equipamentoFabricacaoDatas[id] = "";
                        equipamentoFabricantes[id] = "";
                        equipamentoPrecoAquisicoes[id] = default;
                        MensagemSucesso("Equipamento excluído com sucesso!", true, true);
                    }
                    else
                    {
                        MensagemAviso("- Equipamento não excluído, pois o mesmo encontra-se vínculado a um chamado.", true, true);
                    }
                }
                else
                {
                    MensagemErro("- Item não encontrado.", true, true);
                }
            }
        }

        #endregion

        #region Metodos Chamados
        public static void ListarChamados()
        {
            LimparConsole();
            TituloTelas("Listar Chamados", true, true);
            
            if (ExistePosicaoPreenchidaArrayChamados() == false)
            {
                MensagemAviso("- Não existe chamados cadastrados, pressione enter para voltar para o menu", false, true);
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

            TituloTelas("Cadastrar Chamados", true, true);
            if (ExisteChamadosComEsseProduto() == false)
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
            MensagemAviso("\nEquipamentos cadastrados\n", false, false);
            ImprimeListaEquipamentos();
            bool apenasNumeros = true;
            string padraoApenasNumeros = @"^\d+$";
            do
            {
                Console.Write("Informe a referência do equipamento (EX: 1) ou enter para sair: ");
                equipamentoReferenciaLerTela = Console.ReadLine();
                apenasNumeros = Regex.IsMatch(equipamentoReferenciaLerTela, padraoApenasNumeros);
            }
            while (equipamentoReferenciaLerTela != "" && apenasNumeros == false);
            if (equipamentoReferenciaLerTela == "")
            {
                LimparConsole();
                return;
            }
            equipamentoReferencia = Convert.ToInt32(equipamentoReferenciaLerTela) - 1;
            if (equipamentoNomes[equipamentoReferencia] == null)
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
            string lerTela;

            LimparConsole();

            TituloTelas("Editar Chamados", true, true);
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
                do
                {
                    Mensagem("Informe o id que deseja alterar ou enter para voltar: ", false, false);
                    lerTela = Console.ReadLine();
                }
                while (lerTela != "" && lerTela.Length != 1);
                
                if (lerTela == "")
                {
                    LimparConsole();
                    return;
                }

                TituloTelas("Editar Chamado", true, true);
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
            string lerTela;
            TituloTelas("Excluir Equipamentos", true, true);
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

                
                do
                {
                    Mensagem("Informe o id que deseja excluir ou enter para sair: ", false, false);
                    lerTela = Console.ReadLine();
                }
                while (lerTela != "" && lerTela.Length != 1);
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
                    MensagemSucesso("Equipamento excluído com sucesso!", false, true);
                    Mensagem("Pressione enter para voltar ao menu", false, false);
                    Console.ReadKey();
                    LimparConsole();
                }
                else
                {
                    MensagemErro("Item não encontrado.", true, true);
                }
            }
        }

        #endregion

        #region Metodos Solicitantes
        private static void ListarSolicitantes()
        {
            TituloTelas("Listar Solicitantes", true, true);
            if(ExistePosicaoPreenchidaArraySolicitantes() == false)
            {
                MensagemAviso("- Nenhum solicitante cadastrado", false, true);
                Console.ReadKey();
                LimparConsole();
            }
            else
            {
                ImprimirSolicitantes();
                Mensagem("Pressione enter para voltar ao menu.", false, true);
                Console.ReadKey();
                LimparConsole();
            }
        }
        private static void CadastrarSolicitantes()
        {
            string nome, email, telefone;
            
            TituloTelas("Cadastrar Solicitantes", true, true);

            do
            {
                Mensagem("Informe o nome com no mínimo 6 caracteres ou enter para sair: ", false, false);
                nome = Console.ReadLine();
            } while (nome != "" && nome.Length < 6);
            Console.Write("Informe o email ou enter para sair: ");
            email = Console.ReadLine();
            if (email == "")
            {
                LimparConsole();
                return;
            }
            Console.Write("Informe o telefone EX(00 00000-0000) ou enter para sair: ");
            telefone = Console.ReadLine();
            if (telefone == "")
            {
                LimparConsole();
                return;
            }

            int posicaoInsert = posicaoParaInsercaoSolicitante();
            if (posicaoInsert != -1)
            {
                solicitanteNomes[posicaoInsert] = nome;
                solicitanteEmails[posicaoInsert] = email;
                solicitanteTelefones[posicaoInsert] = telefone;
                LimparConsole();
                MensagemSucesso("Solicitante cadastrado com sucesso!", false, true);
            }
            else
            {
                MensagemAviso("Base de dados está cheia, contate o administrador.", false, true);
                Console.ReadKey();
                LimparConsole();
            }
        }
        private static void EditarSolicitantes()
        {

        }
        private static void ExcluirSolicitantes()
        {

        }
        #endregion

        #region Metodos Menu
        static void MenuTelasPrincipais(out char telaSelecionada)
        {
            string lerTela = "";
            TituloTelas("Menu seletor de telas", false, true);
            while(lerTela != "1" && lerTela != "2" && lerTela != "3" && lerTela != "0") { 
                Console.WriteLine("Segue abaixo as opções disponíveis de tela");
                Console.WriteLine("1- Equipamentos");
                Console.WriteLine("2- Chamados");
                Console.WriteLine("3- Solicitantes");
                Console.WriteLine("0- Sair sistema");

                Console.Write("\nInforma a opção desejada: ");
                lerTela = Console.ReadLine();
                LimparConsole();
            }
            if (lerTela == "0")
            {
                Mensagem("Realmente deseja sair do sistema? \n (s) Sim ou (n) Não: ", false, false);
                char desejaSairDoSistema = Convert.ToChar(Console.ReadLine());
                if(desejaSairDoSistema == 's')
                    sairSistema = true;
                LimparConsole();
            }

            telaSelecionada = Convert.ToChar(lerTela);
        }
        static void MenuCrud(out char opcaoSelecionada, char telaSelecionada)
        {
            string lerTela = "", nomeTela = "";

            switch (telaSelecionada)
            {
                case '1':
                    TituloTelas("Equipamentos", false, true);
                    nomeTela = "Equipamentos";
                    break;
                case '2':
                    TituloTelas("Chamados", false, true);
                    nomeTela = "Chamados";
                    break;
                case '3':
                    TituloTelas("Solicitantes", false, true);
                    nomeTela = "Solicitantes";
                    break;
            }

            while (lerTela != "1" && lerTela != "2" && lerTela != "3" && lerTela != "4")
            {
                Console.WriteLine("Segue abaixo as opções disponíveis para {0}", nomeTela);
                Console.WriteLine("1- Listar");
                Console.WriteLine("2- Cadastrar");
                Console.WriteLine("3- Editar");
                Console.WriteLine("4- Excluir");

                Console.Write("\nInforma a opção desejada: ");
                lerTela = Console.ReadLine();
                LimparConsole();
            }

            opcaoSelecionada = Convert.ToChar(lerTela);
        }
        public static void ChamaMetodos(char telaSelecionada, char opcaoMenu)
        {
            if (telaSelecionada == '1')
            {
                switch (opcaoMenu)
                {
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
                }
            }
            else if (telaSelecionada == '2')
            {
                switch (opcaoMenu)
                {
                    case '1':
                        ListarChamados();
                        break;
                    case '2':
                        CadastrarChamados();
                        break;
                    case '3':
                        EditarChamados();
                        break;
                    case '4':
                        ExcluirChamados();
                        break;
                }
            }
            else if (telaSelecionada == '3')
            {
                switch (opcaoMenu)
                {
                    case '1':
                        ListarSolicitantes();
                        break;
                    case '2':
                        CadastrarSolicitantes();
                        break;
                    case '3':
                        EditarSolicitantes();
                        break;
                    case '4':
                        ExcluirSolicitantes();
                        break;
                }
            }
        }
        #endregion

        #region Metodos auxiliares
        private static void LimparConsole()
        {
            Console.Clear();
        }
        #region Equipamentos
        private static void ImprimeListaEquipamentos()
        {
            for (int i = 0; i < equipamentoNomes.Length; i++)
            {
                if(equipamentoNomes[i] != "" && equipamentoNomes[i] != null)
                    Console.Write("ID: {5} | Produto: {0} | Numero de série: {1} | Data de fabricação: {2} | fabricante: {3} | preço: {4} \n", equipamentoNomes[i], equipamentoSeriais[i], equipamentoFabricacaoDatas[i], equipamentoFabricantes[i], equipamentoPrecoAquisicoes[i].ToString(), (i + 1).ToString());
            }
            Console.WriteLine();
        }
        private static int SelectIdToInsert()
        {
            for (int i = 0; i < equipamentoNomes.Length; i++)
            {
                if (equipamentoNomes[i] == "" || equipamentoNomes[i] == null)
                    return i;
            }
            return -1;
        }
        private static bool ExisteNoArrayEquipamentos(int id)
        {
            if (equipamentoNomes[id] != ""  && equipamentoNomes[id] != null && equipamentoNomes[id].Length > 5)
                return true;
            else
                return false;
        }
        private static bool ExisteChamadosComEsseProduto()
        {
            for (int i = 0; i < equipamentoNomes.Length; i++)
            {
                if(equipamentoNomes[i] != "" && equipamentoNomes[i] != null)
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
                if (equipamentoPosicaoReferencias[i] == idEquipamento && titulos[i] == "")
                    return true;
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
                    Console.Write("ID: {0} | Título: {1} | Equipamento: {2} | Data de abertura: {3} | Dias em aberto: {4}\n", (i+1).ToString(), titulos[i], equipamentoNomes[equipamentoPosicaoReferencias[i]], dataAberturas[i], diferencaData);
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
        #region Solicitante
        private static bool ExistePosicaoPreenchidaArraySolicitantes()
        {
            bool ExisteSolicitanteCadastrado = false;

            for (int i = 0; i < solicitanteNomes.Length; i++)
            {
                if (solicitanteNomes[i] != "" && solicitanteNomes[i] != null) { 
                    ExisteSolicitanteCadastrado = true;
                    break;
                }
                else
                    ExisteSolicitanteCadastrado = false;
            }

            return ExisteSolicitanteCadastrado;
        }
        private static void ImprimirSolicitantes()
        {
            for (int i = 0; i < solicitanteNomes.Length; i++)
            {
                if (solicitanteNomes[i] != "" && solicitanteNomes[i] != null)
                    Console.Write("ID: {0} | Nome: {1} | Email: {2} | Telefone: {3}\n", (i+1).ToString(), solicitanteNomes[i], solicitanteEmails[i], solicitanteTelefones[i]);
            }
            Console.WriteLine();
        }
        private static int posicaoParaInsercaoSolicitante()
        {
            int id = -1;

            for (int i = 0; i < solicitanteNomes.Length; i++)
            {
                if (solicitanteNomes[i] == "" || solicitanteNomes[i] == null)
                {
                    id = i;
                    break;
                }
            }
            
            return id;
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
            Console.BackgroundColor = ConsoleColor.DarkGreen;
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
            Console.ForegroundColor = ConsoleColor.Red;
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
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (pularLinha == true)
            {
                Console.WriteLine(mensagem);
                Console.WriteLine("");
            }
            else
                Console.Write(mensagem);
            Console.ResetColor();
            
        }
        private static void TituloTelas(string mensagem, bool limparConsole, bool pularLinha)
        {
        if (limparConsole == true)
            Console.Clear();
        Console.BackgroundColor = ConsoleColor.Gray;
        Console.ForegroundColor = ConsoleColor.Black;
        if (pularLinha == true)
        {
            Console.WriteLine("---------------------------------- "+mensagem+" ----------------------------------");
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