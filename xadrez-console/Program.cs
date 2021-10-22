using System;
using tabuleiro;
using xadrez;

namespace xadrez_console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Tabuleiro tab = new Tabuleiro(8, 8);

                tab.colocarPeca(new Torre(tab, Cor.Preto), new Posicao(0, 0));
                tab.colocarPeca(new Torre(tab, Cor.Preto), new Posicao(1, 3));
                tab.colocarPeca(new Rei(tab, Cor.Preto), new Posicao(2, 0));

                tab.colocarPeca(new Torre(tab, Cor.Branco), new Posicao(5, 5));

                Tela.ImprimirTabuleiro(tab);

                Console.ReadLine();
            }
            catch(TabuleiroException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);

            }
        }
    }
}
