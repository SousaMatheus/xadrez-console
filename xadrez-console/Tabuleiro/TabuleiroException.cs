using System;

namespace tabuleiro
{
    class TabuleiroException : Exception
    {
        public TabuleiroException(string msg ) : base(msg)
        {//contrutor da clase que recebe um msg string e repassa para a msg para a classe Exception
        }
    }
}
