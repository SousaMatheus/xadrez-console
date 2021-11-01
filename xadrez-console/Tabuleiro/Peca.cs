namespace tabuleiro
{
    abstract class Peca //quando tem um metodo abstrato a classe precisa ser abstrata também
        //a classe não pode ser instanciada    
        //garante herança total, somente subclasses não abstratas podem ser instanciadas, nunca a superclasse
    {
        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; } //só pode ser alterado por ela e subclasse
        public int QtdMovimentos { get; protected set; }
        public Tabuleiro Tab { get; protected set; }

        public Peca(Cor cor, Tabuleiro tab)
        {
            Posicao = null;
            Cor = cor;
            Tab = tab;
            QtdMovimentos = 0;
        }
        public void incrementarQtdeMovimentos()
        {
            QtdMovimentos++;
        }
        public void decrementarQtdeMovimentos()
        {
            QtdMovimentos--;
        }
        public bool existeMovimentosPossiveis()
        {
            bool[,] mat = movimentosPossiveis();
            for (int i = 0; i < Tab.Linhas; i++)
            {
                for (int j = 0; j < Tab.Colunas; j++)
                {
                    if (mat[i, j])//se for true
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool movimentoPossivel(Posicao pos)//método para melhorar legibilidade na partidaDeXadrez
        {
            return movimentosPossiveis()[pos.Linha, pos.Coluna];
        }
        public abstract bool[,] movimentosPossiveis(); //metodo abstract pois superClasse Peca é muito generico e não pode ser implementado nessa classe
        
        
    }
}
