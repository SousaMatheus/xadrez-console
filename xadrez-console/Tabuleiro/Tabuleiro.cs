namespace tabuleiro
{
    class Tabuleiro
    {
        public int Linhas { get; set; }
        public int Colunas { get; set; }
        private Peca[,] Pecas;

        public Tabuleiro(int linhas, int colunas)
        {
            Linhas = linhas;
            Colunas = colunas;
            Pecas = new Peca[linhas, colunas];
        }
        public Peca Peca(Posicao pos)//sobrecarga no metodo que recebe uma Posicao pos
        {
            return Pecas[pos.Linha, pos.Coluna];//retorna matriz Pecas na pos.Linha,  pos.Coluna
        }
        public Peca Peca(int linha, int coluna)
        {
            return Pecas[linha, coluna];
        }
        public bool existePeca (Posicao pos)// metodo para verificar se existe peça dado uma Posicao pos
        {
            validarPosicao(pos);// primeiro verifica se é valido, caso não lança exceção
            return Peca(pos) != null; // se for diferente de nulo retorna posição
        }
        public void colocarPeca(Peca p, Posicao pos)
        {
            if (existePeca(pos))//verifica se ja existe  uma peca
            {
                throw new TabuleiroException("Já existe uma peça nessa posição!");
            }
            Pecas[pos.Linha, pos.Coluna] = p;
            p.Posicao = pos;
        }
        public bool posicaoValida(Posicao pos)//verifica se a Posicao pos é valida ou não
        {
            if (pos.Linha < 0 || pos.Linha >= Linhas || pos.Coluna < 0 || pos.Coluna >= Colunas)
            {
                return false;
            }
            return true;
        }

        public void validarPosicao(Posicao pos)// método que não  retorna nada
        {
            if (!posicaoValida(pos))//se posicaoValida não for válido
            {
                throw new TabuleiroException("Posição invalida!"); // lança uma exceção e corta a execução 
            }
        }
    }
}
