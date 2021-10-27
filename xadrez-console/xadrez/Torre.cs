using tabuleiro;

namespace xadrez
{
    class Torre : Peca
    {
        public Torre(Tabuleiro tab, Cor cor) : base(cor, tab)
        {
        }
        public override string ToString()
        {
            return "T";
        }
        private bool podeMover(Posicao pos)//verifica se a Torre pode se mover para a Posição pos
        {
            Peca p = Tab.Peca(pos);
            return p == null || p.Cor != Cor; // se estiver vazio ou for uma peça adversaria
        }
        public override bool[,] movimentosPossiveis() //sobreposição do metodo 
                                                      //retorna uma matriz com movimentos possiveis
        {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];

            Posicao pos = new Posicao(0, 0);

            //acima
            pos.definirValores(Posicao.Linha - 1, Posicao.Coluna);
            while(Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if(Tab.Peca(pos) != null && Tab.Peca(pos).Cor != Cor)// verifica se tem uma peça adversaria na posição
                {
                    break;//caso seja verdade encerra o while
                }
                pos.Linha = pos.Linha - 1; // verifica a proxima linha acima, toda vez que roda o while diminui 1
            }
            //abaixo
            pos.definirValores(Posicao.Linha +1, Posicao.Coluna);
            while (Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.Peca(pos) != null && Tab.Peca(pos).Cor != Cor)// verifica se tem uma peça adversaria na posição
                {
                    break;//caso seja verdade encerra o while
                }
                pos.Linha = pos.Linha + 1; // verifica a proxima linha abaixo
            }
            //direita
            pos.definirValores(Posicao.Linha , Posicao.Coluna + 1);
            while (Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.Peca(pos) != null && Tab.Peca(pos).Cor != Cor)// verifica se tem uma peça adversaria na posição
                {
                    break;//caso seja verdade encerra o while
                }
                pos.Coluna = pos.Coluna +1; // verifica a proxima coluna a direita
            }
            //esquerda
            pos.definirValores(Posicao.Linha , Posicao.Coluna - 1);
            while (Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.Peca(pos) != null && Tab.Peca(pos).Cor != Cor)// verifica se tem uma peça adversaria na posição
                {
                    break;//caso seja verdade encerra o while
                }
                pos.Coluna = pos.Coluna -1; // verifica a proxima a esquerda
            }
            return mat; //retorna matriz como resposta
        }
    }
}
