using System.Collections.Generic;
using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas;//conjunto do tipo peca
        private HashSet<Peca> capturadas;
        public bool xeque { get; private set; }
        public Peca vulneravelEnPassant { get; private set; }

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branco;
            pecas = new HashSet<Peca>();//instanciando a coleção antes de por as pecas
            capturadas = new HashSet<Peca>();
            colocarPecas();
            terminada = false;
            xeque = false;
            vulneravelEnPassant = null;
    }
        public Peca executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQtdeMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if (pecaCapturada != null)//se houver peca capturada
            {
                capturadas.Add(pecaCapturada);//add no conjunto
            }

            // # jogadaEspecial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.retirarPeca(origemT);
                T.incrementarQtdeMovimentos();
                tab.colocarPeca(T, destinoT);
            }

            // # jogadaEspecial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna -1);
                Peca T = tab.retirarPeca(origemT);
                T.incrementarQtdeMovimentos();
                tab.colocarPeca(T, destinoT);
            }

            // # jogadaEspecical en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == null) // se o movimento é na diagonal(captura) e não houve captura(tradcional)
                {
                    Posicao posP;//posicao peao 
                    if (p.Cor == Cor.Branco)// se for branco
                    {
                        posP = new Posicao(destino.Linha + 1, destino.Coluna);//peca capturada esta uma linha abaixo
                    }
                    else//se for preto
                    {
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);//peca capturada esta uma linha acima
                    }
                    pecaCapturada = tab.retirarPeca(posP);//captura a peça na posição 
                    capturadas.Add(pecaCapturada);//adciona no conjunto das capturadas
                }
            }

            return pecaCapturada;
        }
        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.retirarPeca(destino);
            p.decrementarQtdeMovimentos();
            if (pecaCapturada != null)
            {
                tab.colocarPeca(pecaCapturada, destino); // volta peça capturada no destino
                capturadas.Remove(pecaCapturada); // remove das peças capturadas
            }
            tab.colocarPeca(p, origem);

            // # jogadaEspecial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.retirarPeca(origem);
                T.decrementarQtdeMovimentos();
                tab.colocarPeca(T, origemT);
            }

            // # jogadaEspecial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4 );
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.retirarPeca(origem);
                T.decrementarQtdeMovimentos();
                tab.colocarPeca(T, origemT);
            }

            // #jogadaEspecial en passan
            if (p is Peao)
            {
                if (origem.Coluna != origem.Coluna && pecaCapturada == vulneravelEnPassant)
                    //se foi feito movimento de captura e a pecaCapturada é vulneravel 
                {
                    Peca peao = tab.retirarPeca(destino);
                    Posicao posP;
                    if (p.Cor == Cor.Branco)
                    {
                        posP = new Posicao(3, destino.Coluna);//coloca o peão preto captturado na linha 3 
                    }
                    else //se for preto
                    {
                        posP = new Posicao(4, destino.Coluna);//coloca o peão branco capturado na linah 5
                    }
                    tab.colocarPeca(peao, posP);
                }
            }
        }
        public void realizaJogada(Posicao origem, Posicao destino)
        {
            //não pode executar movimento que te deixe em xeque
            Peca pecaCapturada =  executaMovimento(origem, destino);
            if (estaEmXeque(jogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }
            if (estaEmXeque(adversaria(jogadorAtual)))  
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }
            if (testeXequemate(adversaria(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                turno++;
                mudaJogador();
            }

            Peca p = tab.Peca(destino);//qual peca foi movida
            // #jogadaEspecial En Passant
            if (p is Peao && (destino.Linha == origem.Linha -2 || destino.Linha == origem.Linha + 2))
            {
                vulneravelEnPassant = p;
            }
            else
            {
                vulneravelEnPassant = null;
            }
        }

        public void validarPosicaoDeOrigem(Posicao pos)
        {
            if(tab.Peca(pos) == null)//se não existe peça na posição
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida");
            }
            if(jogadorAtual != tab.Peca(pos).Cor)//se a peça escolhida não é sua
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!tab.Peca(pos).existeMovimentosPossiveis())//se não existe movimentos possiveis
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem");
            }
        }
        public void validarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!tab.Peca(origem).movimentoPossivel(destino))//se não pode mover
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }
        private void mudaJogador()
        {
            if (jogadorAtual == Cor.Branco)
            {
                jogadorAtual = Cor.Preto;
            }
            else
            {
                jogadorAtual = Cor.Branco;
            }
        }
        public HashSet<Peca> pecasCapturadas(Cor cor)//quem são as pecas de acordo com a cor
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> pecasEmJogo(Cor cor)//quem são as pecas em jogo de acordo com cor
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            } 
            aux.ExceptWith(pecasCapturadas(cor));// exceto pecas capturas
            return aux;
        }

        private Cor adversaria(Cor cor)
        {
            if (cor == Cor.Branco)
            {
                return Cor.Preto;
            }
            else
            {
                return Cor.Branco;
            }
        }

        private Peca rei(Cor cor)//quem é o rei de uma dada cor
        {
            foreach (Peca x in pecasEmJogo(cor))//Peca é superclasse e Rei é subclasse
            {
                //testando se uma variavel x do tipo Peca(superclasse) é uma instância de Rei(subclasse) 
                if (x is Rei)// se x é uma instancia da classe Rei
                {
                    return x;
                }                
            }
            return null;
        }

        public bool estaEmXeque(Cor cor)//verifica se o Rei esta em xeque
        {
            Peca R = rei(cor);//variavel tipo peca recebe um objeto tipo rei (upcasting)
            if (R == null) //não deve acontecer
            {
                throw new TabuleiroException("Não tem rei da cor " + cor + " no tabuleiro!");
            }
            foreach (Peca x in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = x.movimentosPossiveis();
                if(mat[R.Posicao.Linha, R.Posicao.Coluna])
                {
                    return true;
                }                
            }
            return false;
        }

        public bool testeXequemate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }
            foreach (Peca x in pecasEmJogo(cor))
            {
                bool[,] mat = x.movimentosPossiveis();
                for (int i = 0; i < tab.Linhas; i++)
                {
                    for (int j = 0; j < tab.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem, destino);//faz o movimento
                            bool testeXeque = estaEmXeque(cor);//testa se esta em xeque
                            desfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        public void colocarNovaPeca(char coluna, int linha, Peca peca)//dado uma coluna e linha e peca
        {
            //vai no tabuleiro da partida e coloca a peca
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            //adiciona a peca no conjunto
            pecas.Add(peca);
        }
        private void colocarPecas()
        {
            //alem de instanciar as pecas guarda no conjunto
            colocarNovaPeca('a', 1, new Torre(tab, Cor.Branco));
            colocarNovaPeca('b', 1, new Cavalo(tab, Cor.Branco));
            colocarNovaPeca('c', 1, new Bispo(tab, Cor.Branco));
            colocarNovaPeca('d', 1, new Dama(tab, Cor.Branco));            
            colocarNovaPeca('e', 1, new Rei(tab, Cor.Branco, this));//referência necessaria para indicar qual partida (auto referencia ao objeto)
            colocarNovaPeca('f', 1, new Bispo(tab, Cor.Branco));
            colocarNovaPeca('g', 1, new Cavalo(tab, Cor.Branco));
            colocarNovaPeca('h', 1, new Torre(tab, Cor.Branco));
            colocarNovaPeca('a', 2, new Peao(tab, Cor.Branco, this));//toda vez que instanciar vai precisar receber a partida como argumento no construtor
            colocarNovaPeca('b', 2, new Peao(tab, Cor.Branco, this));
            colocarNovaPeca('c', 2, new Peao(tab, Cor.Branco, this));            
            colocarNovaPeca('d', 2, new Peao(tab, Cor.Branco, this));
            colocarNovaPeca('e', 2, new Peao(tab, Cor.Branco, this));
            colocarNovaPeca('f', 2, new Peao(tab, Cor.Branco, this));
            colocarNovaPeca('g', 2, new Peao(tab, Cor.Branco, this));
            colocarNovaPeca('h', 2, new Peao(tab, Cor.Branco, this));

            colocarNovaPeca('a', 8, new Torre(tab, Cor.Preto));
            colocarNovaPeca('b', 8, new Cavalo(tab, Cor.Preto));
            colocarNovaPeca('c', 8, new Bispo(tab, Cor.Preto));
            colocarNovaPeca('d', 8, new Dama(tab, Cor.Preto));
            colocarNovaPeca('e', 8, new Rei(tab, Cor.Preto, this));//referência necessaria para indicar qual partida
            colocarNovaPeca('f', 8, new Bispo(tab, Cor.Preto));
            colocarNovaPeca('g', 8, new Cavalo(tab, Cor.Preto));
            colocarNovaPeca('h', 8, new Torre(tab, Cor.Preto));
            colocarNovaPeca('a', 7, new Peao(tab, Cor.Preto, this));
            colocarNovaPeca('b', 7, new Peao(tab, Cor.Preto, this));            
            colocarNovaPeca('c', 7, new Peao(tab, Cor.Preto, this));
            colocarNovaPeca('d', 7, new Peao(tab, Cor.Preto, this));
            colocarNovaPeca('e', 7, new Peao(tab, Cor.Preto, this));
            colocarNovaPeca('f', 7, new Peao(tab, Cor.Preto, this));
            colocarNovaPeca('g', 7, new Peao(tab, Cor.Preto, this));
            colocarNovaPeca('h', 7, new Peao(tab, Cor.Preto, this));

            //metodo antigo antes de refatorar
            //tab.colocarPeca(new Torre(tab, Cor.Preto), new PosicaoXadrez('e', 7).toPosicao());
            //atributo tab do tipo Tabuleiro chama Metodo colocarPeca, que tem como construtor basico
            //Peca e Posição. Instancia uma nova classe tipo Rei no tab com cor. Instancia  uma PosicaoXadrex
            //que converte caractere e numero para posição na matriz com metodo toPosicao()
        }
    }
}
