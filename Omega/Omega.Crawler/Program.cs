using System;
using System.Threading;

namespace Omega.Crawler
{
    class Program
    {
        static void Main( string[] args )
        {
            Analyser a = new Analyser();
            CredentialAuth c = new CredentialAuth();

            for (;;)
            {
                // Les ranger dans une liste
                // envoyer cette liste dans l'analyser si la liste n'est pas vide
                // Reprendre l'analyse des track déja connu
                Thread.Sleep(1000);
            }
        }
    }
}
