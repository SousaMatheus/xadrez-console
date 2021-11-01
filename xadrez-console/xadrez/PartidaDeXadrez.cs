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
            turno++;
            mudaJogador();
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
            if (!tab.Peca(origem).podeMoverPara(destino))//se não pode mover
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
            colocarNovaPeca('c', 1, new Torre(tab, Cor.Branco));
            colocarNovaPeca('c', 2, new Torre(tab, Cor.Branco));
            colocarNovaPeca('d', 1, new Rei(tab, Cor.Branco));
            colocarNovaPeca('e', 1, new Torre(tab, Cor.Branco));            
            colocarNovaPeca('d', 2, new Torre(tab, Cor.Branco));
            colocarNovaPeca('e', 2, new Torre(tab, Cor.Branco));

            colocarNovaPeca('c', 8, new Torre(tab, Cor.Preto));
            colocarNovaPeca('d', 8, new Rei(tab, Cor.Preto));
            colocarNovaPeca('e', 8, new Torre(tab, Cor.Preto));
            colocarNovaPeca('c', 7, new Torre(tab, Cor.Preto));
            colocarNovaPeca('d', 7, new Torre(tab, Cor.Preto));
            colocarNovaPeca('e', 7, new Torre(tab, Cor.Preto)); ;
            
            //metodo antigo antes de refatorar
            //tab.colocarPeca(new Torre(tab, Cor.Preto), new PosicaoXadrez('e', 7).toPosicao());
            //atributo tab do tipo Tabuleiro chama Metodo colocarPeca, que tem como construtor basico
            //Peca e Posição. Instancia uma nova classe tipo Rei no tab com cor. Instancia  uma PosicaoXadrex
            //que converte caractere e numero para posição na matriz com metodo toPosicao()
        }
    }
}
