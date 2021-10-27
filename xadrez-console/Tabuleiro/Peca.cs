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
        public abstract bool[,] movimentosPossiveis(); //metodo abstract pois superClasse Peca é muito generico e não pode ser implementado nessa classe
        
        
    }
}
