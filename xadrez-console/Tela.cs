using System;
using tabuleiro;


namespace xadrez_console
{
    class Tela
    {
        public static void ImprimirTabuleiro (Tabuleiro tab)
        {
            for (int i = 0; i < tab.Linhas; i++)
            {
                Console.Write(8 - i +" ");
                for (int j = 0; j < tab.Colunas; j++)
                {
                    if (tab.Peca(i,j) == null)
                    {
                        Console.Write("- ");
                    }
                    else
                    {
                        imprimirPeca(tab.Peca(i,j));
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");            
        }

        public static void imprimirPeca(Peca peca)
        {
            if (peca.Cor == Cor.Branco)
            {
                Console.Write(peca);
            }
            else
            {
                ConsoleColor aux = Console.ForegroundColor;// tipo consolecolor recebe a cor do caractere exibido, por padrão é cinza
                Console.ForegroundColor = ConsoleColor.Yellow;//se for preto caractere recebe cor amarela
                Console.Write(peca);
                Console.ForegroundColor = aux;
            }
        }
    }
}
