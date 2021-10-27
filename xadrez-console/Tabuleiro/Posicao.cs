namespace tabuleiro
{
    class Posicao
    {
        public int Linha { get; set; }
        public int Coluna { get; set; }
        public Posicao (int linha, int coluna)
        {
            Linha = linha;
            Coluna = coluna;
        }
        public void definirValores (int linha, int coluna) // metodo para definir posição em apenas uma linha de comando
        {
            Linha = linha;
            Coluna = coluna;
        }
        public override string ToString()
        {
            return Linha
                + ", "
                + Coluna;
        }
    }
}
