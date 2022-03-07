using System;
using System.Text.RegularExpressions;

namespace GestaoEquipamentosConsoleApp
{
    internal class Program
    {
        static bool sairSistema = false;

        // base cadastro equipamentos
        static string[] equipamentoNomes = new string[100], equipamentoSeriais = new string[100], equipamentoFabricacaoDatas = new string[100], equipamentoFabricantes = new string[100];
        static decimal[] equipamentoPrecoAquisicoes = new decimal[100];
        
        // base controle de chamados
        static string[] chamadoTitulos = new string[100], chamadoDescricoes = new string[100], chamadoDataAberturas = new string[100];
        static int[] chamadoEquipamentoChamados = new int[100], ChamadoSolicitantesChamados = new int[100];
        static int[] chamadoStatus = new int[100];//1 aberto | 2 fechado
        static DateTime[] chamadoDataAberturaChamados = new DateTime[100];
        
        // base cadastro Solicitantes
        static string[] solicitanteNomes = new string[100], solicitanteEmails = new string[100], solicitanteTelefones = new string[100];
        
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
            if (ExistePosicaoPreenchidaArrayEquipamentos() == true)
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
            string nome, serie, dataFabricacao, fabricante, nomeValidacao, padraoApenasNumeros = @"^\d+$";
            decimal precoAquisicao = default;
             bool apenasNumeros = false;

            TituloTelas("Cadastrar Equipamentos", true, true);
           
            do
            {
                Mensagem("-Informe o nome com no mínimo 6 caracteres ou enter para sair: ", false, false);
                nome = Console.ReadLine();
                if (nome == "")
                {
                    LimparConsole();
                    return;
                }
                nomeValidacao = nome;
                
                if (nomeValidacao.Trim().Length > 0 && nomeValidacao.Trim().Length < 6)
                    MensagemAviso("\nCaracter deve conter pelo menos 6 caracteres: \n", false, false);
                
            } while (nomeValidacao.Trim().Length <= 5);
            if (nome == "")
            {
                LimparConsole();
                return;
            }
            
            do
            {
                Mensagem("-Informe o numero de serie (deve ser único) ou enter para sair: ", false, false);
                serie = Console.ReadLine();
                if (serie == "")
                {
                    LimparConsole();
                    return;
                }
                nomeValidacao = serie;
                if (ExisteNumeroSerieRegistrado(nomeValidacao) == true)
                    MensagemAviso("Número de série ja registrado\n", false, false);
                else
                    break;

            } while (serie != "");
            if (serie == "")
            {
                LimparConsole();
                return;
            }

            Console.Write("-Informe o fabricante ou enter para sair: ");
            fabricante = Console.ReadLine();
            if (fabricante == "")
            {
                LimparConsole();
                return;
            }

            while (true) 
            {
                bool valida = false;
                Mensagem("-Informe a data de fabricação EX:(00/00/0000) ou enter para sair: ", false, false);
                dataFabricacao = Console.ReadLine();
                if(dataFabricacao != "")
                    valida = ValidaData(dataFabricacao);
                if (dataFabricacao == "" || valida == true)
                    break;
            }
            if (dataFabricacao == "")
            {
                LimparConsole();
                return;
            }

            string lerTela;
            do
            {
                MensagemAviso("O preço não poderá ser negativo ou zero", false, false);
                Mensagem("\n-Informe o preço de aquisição ou enter para sair: ", false, false);
                lerTela = Console.ReadLine();
                if (lerTela == "")
                    break;
                
                precoAquisicao = Convert.ToDecimal(lerTela);
            } while (precoAquisicao <= 0 && lerTela != "");
            if (lerTela == "")
            {
                LimparConsole();
                return;
            }

            int posicaoInsert = PosicaoParaInsercaoquipamentos();
            if (posicaoInsert != -1)
            {
                equipamentoNomes[posicaoInsert] = nome;
                equipamentoSeriais[posicaoInsert] = serie;
                equipamentoFabricacaoDatas[posicaoInsert] = dataFabricacao;
                equipamentoFabricantes[posicaoInsert] = fabricante;
                equipamentoPrecoAquisicoes[posicaoInsert] = precoAquisicao == default ? default : precoAquisicao;
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
            string nome, serie, dataFabricacao, fabricante, preco, lerTela, padraoApenasNumeros = @"^\d+$", nomeValidacao;
            decimal precoAquisicao;
            bool encerrarMetodo = false, existeIdEquipamento = false, apenasNumeros = false;
            int ID = default;

            TituloTelas("Editar Equipamentos", true, true);
            if (ExistePosicaoPreenchidaArrayEquipamentos() == false) { 
                MensagemAviso("- Nenhum equipamento cadastrado, pressione enter para voltar ao menu.", false, true);
                Console.ReadKey();
                LimparConsole();
            }
            else
            {
                ImprimeListaEquipamentos();
                do
                {
                    Mensagem("Informe o id que deseja alterar ou enter para voltar: ", false, false);
                    lerTela = Console.ReadLine();
                    apenasNumeros = Regex.IsMatch(lerTela, padraoApenasNumeros);
                }
                while (lerTela != "" && apenasNumeros == false);
                    
                if (lerTela == "")
                {
                    LimparConsole();
                    return;
                }
                ID = Convert.ToInt32(lerTela) - 1;
                
                if(ExisteNoArrayEquipamentos(ID) == false)
                {
                    MensagemAviso("\nID não encontrado, pressione enter e volte ao menu.", false, true);
                    Console.ReadLine();
                    LimparConsole();
                }
                else
                {
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
                    
                    equipamentoNomes[ID] = nome != "" ? nome : equipamentoNomes[ID];
                    equipamentoSeriais[ID] = serie != "" ? serie : equipamentoSeriais[ID];
                    equipamentoFabricacaoDatas[ID] = dataFabricacao != "" ? dataFabricacao : equipamentoFabricacaoDatas[ID];
                    equipamentoFabricantes[ID] = fabricante != "" ? fabricante : equipamentoFabricantes[ID];
                    equipamentoPrecoAquisicoes[ID] = preco != "" ? Convert.ToDecimal(preco) : equipamentoPrecoAquisicoes[ID];
                    
                    MensagemSucesso("Equipamento editado com sucesso!", true, true);
                }                
            }
        }
        public static void ExcluirEquipamentos()
        {
            string lerTela, padraoApenasNumeros = @"^\d+$";
            bool apenasNumeros = false;
            int ID = default;
            TituloTelas("Excluir Equipamentos", true, true);
            
            if(ExistePosicaoPreenchidaArrayEquipamentos() == false)
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
                    Mensagem("Informe o id que deseja excluir ou enter para voltar: ", false, false);
                    lerTela = Console.ReadLine();
                    apenasNumeros = Regex.IsMatch(lerTela, padraoApenasNumeros);
                }
                while (lerTela != "" && apenasNumeros == false);
                
                if (lerTela == "")
                {
                    LimparConsole();
                    return;
                }
                ID = Convert.ToInt32(lerTela) - 1;
                if (ExisteNoArrayEquipamentos(ID) == true)
                {
                    if (ExisteChamadoParaEsseEquipamento(ID) == false)
                    {
                        equipamentoNomes[ID] = "";
                        equipamentoSeriais[ID] = "";
                        equipamentoFabricacaoDatas[ID] = "";
                        equipamentoFabricantes[ID] = "";
                        equipamentoPrecoAquisicoes[ID] = default;
                        MensagemSucesso("Equipamento excluído com sucesso!", true, true);
                    }
                    else
                    {
                        MensagemAviso("- Equipamento não excluído, pois o mesmo encontra-se vínculado a um chamado.", true, true);
                        Console.ReadKey();
                        LimparConsole();
                    }
                }
                else
                {
                    MensagemErro("- Item não encontrado, pressione enter para voltar ao menu.", true, true);
                    Console.ReadKey();
                    LimparConsole();
                }
            }
        }

        //Auxiliares
        private static bool ExistePosicaoPreenchidaArrayEquipamentos()
        {
            bool existePosicaoPreenchida = false;

            for (int i = 0; i < equipamentoNomes.Length; i++)
            {
                if (equipamentoNomes[i] != "" && equipamentoNomes[i] != null)
                {
                    existePosicaoPreenchida = true;
                    break;
                }
            }

            return existePosicaoPreenchida;
        }
        private static void ImprimeListaEquipamentos()
        {
            for (int i = 0; i < equipamentoNomes.Length; i++)
            {
                if (equipamentoNomes[i] != "" && equipamentoNomes[i] != null)
                    Console.Write("ID: {5} | Produto: {0} | Numero de série: {1} | Data de fabricação: {2} | fabricante: {3} | preço: {4} \n", equipamentoNomes[i], equipamentoSeriais[i], equipamentoFabricacaoDatas[i], equipamentoFabricantes[i], equipamentoPrecoAquisicoes[i].ToString(), (i + 1).ToString());
            }
            Console.WriteLine();
        }
        private static int PosicaoParaInsercaoquipamentos()
        {
            int idParaInsercao = -1;// Em caso de array cheio retorna -1

            for (int i = 0; i < equipamentoNomes.Length; i++)
            {
                if (equipamentoNomes[i] == "" || equipamentoNomes[i] == null)
                {
                    idParaInsercao = i;
                    break;
                }
                    
            }

            return idParaInsercao;
        }
        private static bool ExisteNoArrayEquipamentos(int id)
        {
            int idSolicitante = id;
            bool existe = false;

            if (solicitanteNomes[idSolicitante] != "" && solicitanteNomes[idSolicitante] != null)
                existe = true;

            return existe;
        }
        private static bool ExisteChamadoParaEsseEquipamento(int id)
        {
            int idEquipamento= id;
            bool existe = false;

            for (int i = 0; i < chamadoEquipamentoChamados.Length; i++)
            {
                if (chamadoEquipamentoChamados[i] == idEquipamento && chamadoTitulos[i] == "")
                {
                    existe = true;
                    break;
                }
            }

            return existe;
        }
        private static bool ExisteNumeroSerieRegistrado(string numero)
        {
            string numeroSerie = numero;
            bool existe = false;

            for (int i = 0; i < equipamentoSeriais.Length; i++)
            {
                if (equipamentoSeriais[i] == numeroSerie)
                {
                    existe = true;
                    break;
                }
            }

            return existe;
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

            Mensagem("Lista de equipamento mais problemáticos", false, true);
            EquipamentoMaisProblematicos();

            Mensagem("Pressione enter para voltar ao menu principal", false, true);

            Console.ReadKey();
            LimparConsole();
        }
        public static void CadastrarChamados()
        {
            string titulo, descricao, dataAbertura, equipamentoReferenciaLerTela, lerTela, padraoApenasNumeros = @"^\d+$";
            int equipamentoReferenciaChamado, solicitanteReferenciaChamado = -1;
            bool apenasNumeros = false;
            
            TituloTelas("Cadastrar Chamados", true, true);
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

            MensagemAviso("\nEquipamentos cadastrados\n", false, false);
            ImprimeListaEquipamentos();
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
            equipamentoReferenciaChamado = Convert.ToInt32(equipamentoReferenciaLerTela) - 1;
            if (equipamentoNomes[equipamentoReferenciaChamado] == null)
            {
                MensagemAviso("Equipamento não encontrado", false, true);
                Console.ReadKey();
                LimparConsole();
                return;
            }

            while (true)
            {
                Console.Write("Informe a data de abertura do chamado (EX: 00/00/0000) ou enter para sair: ");
                dataAbertura = Console.ReadLine();
                if (ValidaData(dataAbertura) || dataAbertura == "")
                    break;
            }
            if (dataAbertura == "")
            {
                LimparConsole();
                return;
            }

            if(ExistePosicaoPreenchidaArraySolicitantes() == true){
                Mensagem("\nLista de solicitantes cadastrados", false, true);
                ImprimirSolicitantes();
                do
                {
                    Mensagem("Informe a referência do solicitante (EX: 1) ou enter para não informar: ", false, false);
                    lerTela = Console.ReadLine();
                    if(lerTela != "")
                    {
                        apenasNumeros = Regex.IsMatch(lerTela, padraoApenasNumeros);

                        if (apenasNumeros == false && (ExisteNoArraySolicitante(Convert.ToInt32(lerTela) -1)) == true)
                            Mensagem("\nID não encontrado \n", false, false);
                    }
                    else
                    {
                        lerTela = "0";
                        apenasNumeros = true;
                    }
                }
                while (lerTela != "" & apenasNumeros == false);
                solicitanteReferenciaChamado = Convert.ToInt32(lerTela) -1;
            }
            int posicaoInsert = PosicaoParaInsercaoChamado();
            if (posicaoInsert != -1)
            {
                string[] dataSeparada = dataAbertura.Split("/");
                int dia = Convert.ToInt32(dataSeparada[0]);
                int mes = Convert.ToInt32(dataSeparada[1]);
                int ano = Convert.ToInt32(dataSeparada[2]);

                DateTime dataCriacaoChamado = new DateTime(ano, mes, dia);

                chamadoTitulos[posicaoInsert] = titulo;
                chamadoDescricoes[posicaoInsert] = descricao;
                chamadoEquipamentoChamados[posicaoInsert] = equipamentoReferenciaChamado;
                ChamadoSolicitantesChamados[posicaoInsert] = solicitanteReferenciaChamado;
                chamadoDataAberturas[posicaoInsert] = dataAbertura;
                chamadoDataAberturaChamados[posicaoInsert] = dataCriacaoChamado;
                chamadoStatus[posicaoInsert] = 1;//status aberto
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
            string titulo, descricao, referenciaProduto, dataAbertura, padraoApenasNumeros = @"^\d+$";
            int idReferenciaProduto, idSolicitanteReferenciaChamado = - 1, status;
            string lerTela;
            bool apenasNumeros = false;

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
                if (ExistePosicaoPreenchidaArraySolicitantes() == true)
                {
                    Mensagem("\nLista de solicitantes cadastrados", false, true);
                    ImprimirSolicitantes();
                    do
                    {
                        Mensagem("Informe a referência do solicitante (EX: 1) ou enter para não informar: ", false, false);
                        lerTela = Console.ReadLine();

                        if (lerTela != "")
                        {
                            apenasNumeros = Regex.IsMatch(lerTela, padraoApenasNumeros);

                            if (apenasNumeros == false && (ExisteNoArraySolicitante(Convert.ToInt32(lerTela) -1)) == true)
                                Mensagem("\nID não encontrado \n", false, false);
                        }
                        else
                        {
                            lerTela = "0";
                            apenasNumeros = true;
                        }
                    }
                    while (lerTela != "" && apenasNumeros == false);
                    idSolicitanteReferenciaChamado = Convert.ToInt32(lerTela) -1;
                }
                do
                {
                    Mensagem("Informe o status do chamado EX: (1) aberto | (2) fechado ou enter para manter: ", false, false);
                    lerTela = Console.ReadLine();
                }
                while (lerTela != "" && lerTela.Length != 1);
                status = Convert.ToInt32(lerTela);
                chamadoTitulos[id] = titulo != "" ? titulo : chamadoTitulos[id];
                chamadoDescricoes[id] = descricao != "" ? descricao : chamadoDescricoes[id];
                chamadoEquipamentoChamados[id] = referenciaProduto != "" ? Convert.ToInt32(referenciaProduto) : chamadoEquipamentoChamados[id];
                ChamadoSolicitantesChamados[id] = idSolicitanteReferenciaChamado != -1 ? idSolicitanteReferenciaChamado : ChamadoSolicitantesChamados[id];
                chamadoStatus[id] = status != default ? status : chamadoStatus[id];
                if (dataAbertura != null && dataAbertura != "")
                {
                    string[] dataSeparada = dataAbertura.Split("/");
                    int dia = Convert.ToInt32(dataSeparada[0]);
                    int mes = Convert.ToInt32(dataSeparada[1]);
                    int ano = Convert.ToInt32(dataSeparada[2]);

                    DateTime dataCriacaoChamado = new DateTime(ano, mes, dia);

                    chamadoDataAberturas[id] = dataAbertura;
                    chamadoDataAberturaChamados[id] = dataCriacaoChamado;
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
                    chamadoTitulos[id] = "";
                    chamadoDescricoes[id] = "";
                    chamadoEquipamentoChamados[id] = default;
                    ChamadoSolicitantesChamados[id] = default;
                    chamadoDataAberturas[id] = "";
                    chamadoDataAberturaChamados[id] = default;
                    chamadoStatus[id] = default;
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

        //Auxiliares
        private static bool ExistePosicaoPreenchidaArrayChamados()
        {
            bool existePosicaoPreenchida = false;

            for (int i = 0; i < chamadoTitulos.Length; i++)
            {
                if (chamadoTitulos[i] != "" && chamadoTitulos[i] != null)
                {
                    existePosicaoPreenchida = true;
                    break;
                }
            }

            return existePosicaoPreenchida;
        }
        private static void ImprimeListaChamados()
        {
            Mensagem("Chamados abertos", false, true);
            ImprimeListaChamadoAberto();
            Mensagem("Chamados fechado", false, true);
            ImprimeListaChamadoFechado();
        }
        private static int PosicaoParaInsercaoChamado()
        {
            int idParaInsercao = - 1;//Em caso array cheio retorna -1

            for (int i = 0; i < chamadoTitulos.Length; i++)
            {
                if (chamadoTitulos[i] == "" || chamadoTitulos[i] == null) 
                { 
                    idParaInsercao = i;
                    break;
                }
            }

            return idParaInsercao;
        }
        private static bool ExisteNoArrayChamados(int id)
        {
            bool existeNoArray = false;
            
            if (chamadoTitulos[id] != ""  && chamadoTitulos[id] != null)
                existeNoArray = true;

            return existeNoArray;
        }
        private static void ImprimeListaChamadoAberto()
        {
            for (int i = 0; i < chamadoTitulos.Length; i++)
            {
                if (chamadoTitulos[i] != "" && chamadoTitulos[i] != null)
                {
                    if(chamadoStatus[i] == 1)
                    {
                        string[] dataSeparada = chamadoDataAberturas[i].Split("/");
                        int dia = Convert.ToInt32(dataSeparada[0]);
                        int mes = Convert.ToInt32(dataSeparada[1]);
                        int ano = Convert.ToInt32(dataSeparada[2]);
                        string nomeSolicitante, status;

                        DateTime dataCriacaoChamado = new DateTime(ano, mes, dia);
                        DateTime dataAtual = DateTime.Now;
                        TimeSpan periodoTempo = dataAtual - dataCriacaoChamado;
                        int diferencaData = periodoTempo.Days;
                        if (ChamadoSolicitantesChamados[i] != -1)
                            nomeSolicitante = solicitanteNomes[chamadoEquipamentoChamados[i]];
                        else
                            nomeSolicitante = "Não informado";

                        status = chamadoStatus[i] == 1 ? "Aberto" : "Fechado";
                        Console.Write("ID: {0} | Título: {1} | Solicitante: {2} | Equipamento: {3} | Data de abertura: {4} | Dias em aberto: {5} | Status: {6}\n", (i+1).ToString(), chamadoTitulos[i], nomeSolicitante, equipamentoNomes[chamadoEquipamentoChamados[i]], chamadoDataAberturas[i], diferencaData, status);
                    }
                }
            }
            Console.WriteLine();
        }
        private static void ImprimeListaChamadoFechado()
        {
            for (int i = 0; i < chamadoTitulos.Length; i++)
            {
                if (chamadoTitulos[i] != "" && chamadoTitulos[i] != null)
                {
                    if (chamadoStatus[i] == 2)
                    {
                        string[] dataSeparada = chamadoDataAberturas[i].Split("/");
                        int dia = Convert.ToInt32(dataSeparada[0]);
                        int mes = Convert.ToInt32(dataSeparada[1]);
                        int ano = Convert.ToInt32(dataSeparada[2]);
                        string nomeSolicitante;

                        DateTime dataCriacaoChamado = new DateTime(ano, mes, dia);
                        DateTime dataAtual = DateTime.Now;
                        TimeSpan periodoTempo = dataAtual - dataCriacaoChamado;
                        int diferencaData = periodoTempo.Days;
                        if (ChamadoSolicitantesChamados[i] != -1)
                            nomeSolicitante = solicitanteNomes[chamadoEquipamentoChamados[i]];
                        else
                            nomeSolicitante = "Não informado";

                        Console.Write("ID: {0} | Título: {1} | Solicitante: {2} | Equipamento: {3} | Data de abertura: {4} | Dias em aberto: {5} | Status: {6}\n", (i+1).ToString(), chamadoTitulos[i], nomeSolicitante, equipamentoNomes[chamadoEquipamentoChamados[i]], chamadoDataAberturas[i], diferencaData, chamadoStatus[i]);
                    }
                }
            }
            Console.WriteLine();
        }
        private static void EquipamentoMaisProblematicos()
        {
            int[] arrayOrdenaPosicoes = chamadoEquipamentoChamados;
            int contadorNumerosDiferentes = 1, quantidadeNumeroDiferente = 1, contadorPosicoes = 0, contadorRepeticoes = 1;

            for (int i = 0; i < arrayOrdenaPosicoes.Length; i++)
            {
                for (int z = 0; z < arrayOrdenaPosicoes.Length - 1; z++)
                {
                    if(arrayOrdenaPosicoes[z] > arrayOrdenaPosicoes[z + 1])
                    {
                        int troca = arrayOrdenaPosicoes[z];
                        arrayOrdenaPosicoes[z] = arrayOrdenaPosicoes[z + 1];
                        arrayOrdenaPosicoes[z + 1] = troca;
                    }
                }
            }

            for (int i = 0; i < arrayOrdenaPosicoes.Length; i++)
            {
                if(i != arrayOrdenaPosicoes.Length -1)
                {
                    if (arrayOrdenaPosicoes[i] != arrayOrdenaPosicoes[i + 1])
                        contadorNumerosDiferentes++;
                }
            }
            int[] numeros = new int[contadorNumerosDiferentes], numeroQuantidadeRepeticoes = new int[contadorNumerosDiferentes];
            contadorPosicoes = 0;
            for (int i = 0; i < arrayOrdenaPosicoes.Length; i++)
            {
                if (equipamentoNomes[arrayOrdenaPosicoes[i]] != "")
                {
                    if (i != arrayOrdenaPosicoes.Length -1)
                    {
                    
                        if (arrayOrdenaPosicoes[i] == arrayOrdenaPosicoes[i + 1])
                        {
                            contadorRepeticoes++;
                        }
                        else
                        {
                            numeros[contadorPosicoes] = arrayOrdenaPosicoes[i];
                            numeroQuantidadeRepeticoes[contadorPosicoes] = contadorRepeticoes;
                            contadorPosicoes++;
                            contadorRepeticoes = 1;
                        }
                    }
                }
            }
            for (int i = 0; i < numeros.Length; i++)
            {
                Console.WriteLine("Equipamento {0} teve {1} ocorrência(s) de chamado(s)", equipamentoNomes[numeros[i]], numeroQuantidadeRepeticoes[i].ToString());
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
            string nome, email, telefone, lerTela;
            string padraoApenasNumeros = @"^\d+$";
            int ID;
            bool apenasNumeros = true, existeID = false;

            TituloTelas("Editar Solicitantes", true, true);

            if(ExistePosicaoPreenchidaArraySolicitantes() == false)
            {
                MensagemAviso("- Nenhum solicitante cadastrado", false, true);
                Console.ReadKey();
                LimparConsole();
            }
            else
            {
                ImprimirSolicitantes();

                do
                {
                    Mensagem("Informe a referência do equipamento (EX: 1) ou enter para sair: ", false, false);
                    lerTela = Console.ReadLine();
                    apenasNumeros = Regex.IsMatch(lerTela, padraoApenasNumeros);
                }
                while (lerTela != "" && apenasNumeros == false);
                if(lerTela == "")
                {
                    LimparConsole();
                    return;
                }
                ID = Convert.ToInt32(lerTela) - 1;
                existeID = ExisteNoArraySolicitante(ID);
                if(existeID == false)
                {
                    MensagemAviso("\nIdentificado não encontrado, pressione enter e volte ao menu.", false, true);
                    Console.ReadKey();
                    LimparConsole();
                    return;
                }
                PularLinha();
                do
                {
                    Mensagem("Informe o nome com no mínimo 6 caracteres ou enter para manter a informação anterior: ", false, false);
                    nome = Console.ReadLine();
                } while (nome != "" && nome.Length < 6);

                Console.Write("Informe o email ou enter para manter a informação anterior: ");
                email = Console.ReadLine();

                Console.Write("Informe o telefone EX(00 00000-0000) ou enter para manter a informação anterior: ");
                telefone = Console.ReadLine();

                solicitanteNomes[ID] = nome != "" ? nome : solicitanteNomes[ID];
                solicitanteEmails[ID] = email != "" ? email : solicitanteNomes[ID];
                solicitanteTelefones[ID] = telefone != "" ? telefone : solicitanteNomes[ID];
                LimparConsole();
                MensagemSucesso("Solicitante cadastrado com sucesso!", false, true);
            }
        }
        private static void ExcluirSolicitantes()
        {
            string lerTela;
            int idExclusao;
            bool apenasNumeros, existeID = false;
            string padraoApenasNumeros = @"^\d+$";

            TituloTelas("Excluir Solicitantes", true, true);

            if (ExistePosicaoPreenchidaArraySolicitantes() == false)
            {
                MensagemAviso("- Nenhum solicitante cadastrado", false, true);
                Console.ReadKey();
                LimparConsole();
            }
            else
            {
                ImprimirSolicitantes();
                do
                {
                    Mensagem("Informe a referência do equipamento (EX: 1) ou enter para sair: ", false, false);
                    lerTela = Console.ReadLine();
                    apenasNumeros = Regex.IsMatch(lerTela, padraoApenasNumeros);
                }
                while (lerTela != "" && apenasNumeros == false);
                if (lerTela == "")
                {
                    LimparConsole();
                    return;
                }
                idExclusao = Convert.ToInt32(lerTela) - 1;
                existeID = ExisteNoArraySolicitante(idExclusao);
                if (existeID == false)
                {
                    MensagemAviso("\nIdentificado não encontrado, pressione enter e volte ao menu.", false, true);
                    Console.ReadKey();
                    LimparConsole();
                    return;
                }
                else
                {
                    solicitanteNomes[idExclusao] = "";
                    solicitanteEmails[idExclusao] = "";
                    solicitanteTelefones[idExclusao] = "";
                    LimparConsole();
                    MensagemSucesso("\nItem excluido com sucesso", false, true);
                    return;
                }
            }
        }
        //auxiliares
        private static bool ExistePosicaoPreenchidaArraySolicitantes()
        {
            bool ExisteSolicitanteCadastrado = false;

            for (int i = 0; i < solicitanteNomes.Length; i++)
            {
                if (solicitanteNomes[i] != "" && solicitanteNomes[i] != null)
                {
                    ExisteSolicitanteCadastrado = true;
                    break;
                }
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
            int id = -1;// Em caso de array cheio retorna -1

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
        private static bool ExisteNoArraySolicitante(int id)
        {
            int idSolicitante = id;
            bool existe = false;

            if (solicitanteNomes[idSolicitante] != "" && solicitanteNomes[idSolicitante] != null)
                existe = true;
            
            return existe;
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
        private static bool ValidaData(string data)
        {
            string padraoApenasNumeros = @"^\d+$", dataValidacao = data;
            string[] dataSeparada = data.Split("/");
            bool valida = false;

            if (dataSeparada.Length != 3)
                MensagemAviso("Formato de data está com erro.\n", false, false);
            else if ((dataSeparada[0].Length != 2) && (dataSeparada[1].Length != 2) && dataSeparada[2].Length != 4)
                MensagemAviso("Formato de data está com erro, deve seguir o padrão 00/00/0000.\n", false, false);
            else if ((Regex.IsMatch(dataSeparada[0], padraoApenasNumeros) == false) || (Regex.IsMatch(dataSeparada[1], padraoApenasNumeros) == false) || (Regex.IsMatch(dataSeparada[2], padraoApenasNumeros) == false))
                MensagemAviso("Somente numeros devem ser informados, apenas separados pos barra '/'.\n", false, false);
            else if (Convert.ToInt32(dataSeparada[0]) < 1 || Convert.ToInt32(dataSeparada[0]) > 31)
                MensagemAviso("Dia invalido '/'.\n", false, false);
            else if (Convert.ToInt32(dataSeparada[1]) < 1 || Convert.ToInt32(dataSeparada[1]) > 12)
                MensagemAviso("mês invalido '/'.\n", false, false);
            else if (Convert.ToInt32(dataSeparada[2]) < 1900)
                MensagemAviso("Ano menor que ano mínimo aceito. (Ano mínimo: 1900)'/'.\n", false, false);
            else
                valida = true;

            return valida;
        }
        public static void PularLinha()
        {
            Console.WriteLine("");
        }
        private static void LimparConsole()
        {
            Console.Clear();
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
    
    }
}