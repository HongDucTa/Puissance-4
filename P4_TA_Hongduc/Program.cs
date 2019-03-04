using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Puissance4_GUI;

namespace P4_TA_Hongduc
{
    class Program
    {
        

        static int[,] InitialisationPlateau(int nbLigne, int nbColonne, int idVide)
            // Crée une matrice et la remplit de vide puis la renvoie.
        {
            int[,] matrice = new int[nbLigne, nbColonne];
            for (int indexLigne = 0; indexLigne < nbLigne; indexLigne++)
            {
                for (int indexColonne = 0; indexColonne < nbColonne; indexColonne++)
                {
                    matrice[indexLigne, indexColonne] = idVide;
                }
            }
            return matrice;
        }

        static void AfficherPlateau(int[,] mat)
            // Affiche la matrice sur la console.
        {
            int dimensionLigne = mat.GetLength(0);
            int dimensionColonne = mat.GetLength(1);

            // Facilite le repérage des colonnes pour l'utilisateur
            for (int index = 0; index < dimensionColonne; index++)
            {
                Console.Write("|" + (index + 1));
            }
            Console.Write("|\n");
            Console.WriteLine();


            for (int indexLigne = 0; indexLigne < dimensionLigne; indexLigne++)
            {
                Console.Write("|");
                for (int indexColonne = 0; indexColonne < dimensionColonne; indexColonne++)
                {
                    Console.Write(mat[indexLigne, indexColonne] + "|");
                }
                Console.WriteLine();
            }
            for (int index = 0; index < dimensionColonne; index++)
            {
                Console.Write("--");
            }
            Console.Write("-\n");
        }

        static int ChoixEmplacement(int[,] mat)
            // Demande à l'utilisateur de choisir une colonne valide ( compris entre 1 et 7 )
        {
            int choix = -1;
            int dimensionColonne = mat.GetLength(1);
            Console.Write("Choix de la colonne (entre 1 et " + dimensionColonne + ") : ");
            while ((choix < 1) || (choix > dimensionColonne))
            {
                choix = Convert.ToInt32(Console.ReadLine());
                if ((choix < 1) || (choix > dimensionColonne))
                {
                    Console.Write("Colonne invalide. Réessayez. ");
                }
            }
            return choix - 1;
        }

        static bool PlacementPossible(int[,] mat, int idVide, int choixColonne)
            // Vérifie que la colonne désignée par le joueur possède un emplacement vide
        {
            bool possible = false;
            int dimensionLigne = mat.GetLength(0);
            int indexLigne = dimensionLigne - 1;
            while ((indexLigne >= 0) && (possible == false))
            {
                if (mat[indexLigne,choixColonne] == idVide)
                {
                    possible = true;
                }
                
                indexLigne--;
            }
            return possible;
        }

        static void ChuteJeton(int[,] mat, int idJoueur, int idVide, int choixColonne)
            /* Place le jeton dans la colonne choisie par le joueur dans la ligne la plus basse possible
             La colonne choisie a déja été validée au préalable. */ 
        {
            int dimensionLigne = mat.GetLength(0);
            int indexLigne = dimensionLigne - 1;
            bool jetonPlace = false;
            while (jetonPlace == false)
            {
                if (mat[indexLigne, choixColonne] == idVide)
                {
                    mat[indexLigne, choixColonne] = idJoueur;
                    jetonPlace = true;
                }
                indexLigne--;
            }
        }

        static void PhaseJoueur(int[,] mat,int idVide, int idJoueur)
            /* Gère le déroulement de la phase de jeu du joueur allant du choix de la colonne
             jusqu'au placement du jeton dans le plateau. */
        {
            bool choixValide = false;
            bool placementValide = false;
            int choixColonne = -1;
            while (choixValide == false)
            {
                choixColonne = ChoixEmplacement(mat);
                placementValide = PlacementPossible(mat, idVide, choixColonne);
                if (placementValide == false)
                {
                    Console.WriteLine("Colonne pleine.");
                }
                else
                {
                    ChuteJeton(mat, idJoueur, idVide, choixColonne);
                    choixValide = true;
                }
            }
        }

        static bool CombinaisonLigneGagnante(int[,] mat,int idJoueur)
        /* Part à la recherche d'un éventuel alignement de 4 jetons du joueur entré en paramètre sur une ligne
         Cette méthode parcourt d'abord chaque élément de la matrice susceptible de commencer un alignement
         puis elle la compare avec les 3 emplacements suivants de la ligne. */ 
        {
            bool combinaisonTrouvee = false;
            int ligne = 0;
            while ((ligne < mat.GetLength(0)) && (combinaisonTrouvee == false))
            {
                int colonne = 0;
                while ((colonne < mat.GetLength(1) - 4) && (combinaisonTrouvee == false))
                {
                    if ((mat[ligne,colonne] == idJoueur) && (mat[ligne,colonne+1] == idJoueur) && (mat[ligne,colonne+2] == idJoueur) && (mat[ligne,colonne+3] == idJoueur))
                    {
                        combinaisonTrouvee = true;
                    }
                    colonne++;
                }
                ligne++;
            }
            return combinaisonTrouvee;
        }

        static bool CombinaisonColonneGagnante(int[,] mat,int idJoueur)
        /* Part à la recherche d'un éventuel alignement de 4 jetons du joueur entré en paramètre sur une colonne
         Même principe que dans la méthode précédente. */ 
        {
            bool combinaisonTrouvee = false;
            int colonne = 0;
            while ((colonne < mat.GetLength(1)) && (combinaisonTrouvee == false))
            {
                int ligne = 0;
                while ((ligne <= mat.GetLength(0) - 4) && (combinaisonTrouvee == false))
                {
                    if ((mat[ligne,colonne] == idJoueur) && (mat[ligne+1,colonne] == idJoueur) && (mat[ligne+2,colonne] == idJoueur) && (mat[ligne+3,colonne] == idJoueur))
                    {
                        combinaisonTrouvee = true;
                    }
                    ligne++;
                }
                colonne++;
            }
            return combinaisonTrouvee;
        }

        static bool CombinaisonDiagonaleDecroissante(int[,] mat, int idJoueur)
            /* Part à la recherche d'un alignement de 4 jetons du joueur entré en paramètre placés en diagonale croissante
             Même principe que précédemment. */ 
        {
            bool combinaisonTrouvee = false;
            int ligne = 0;
            while ((ligne <= mat.GetLength(0) - 4) && (combinaisonTrouvee == false))
            {
                int colonne = 0;
                while ((colonne <= mat.GetLength(1) - 4) & (combinaisonTrouvee == false))
                {
                    if ((mat[ligne, colonne] == idJoueur) && (mat[ligne + 1, colonne + 1] == idJoueur) && (mat[ligne+2,colonne+2] == idJoueur) && (mat[ligne+3,colonne+3] == idJoueur))
                    {
                        combinaisonTrouvee = true;
                    }
                    colonne++;
                }
                ligne++;
            }
            return combinaisonTrouvee;
        }

        static bool CombinaisonDiagonaleCroissante(int[,] mat, int idJoueur)
            /* Part à la reherche d'un alignement de 4 jetons du joueur entré en paramère placés en diagonale décroissante
             Même principe que précédemment. */ 
        {
            bool combinaisonTrouvee = false;
            int colonne = 0;
            while ((colonne <= mat.GetLength(1) - 4) && (combinaisonTrouvee == false))
            {
                int ligne = mat.GetLength(0) - 1;
                while ((ligne >= 3) && (combinaisonTrouvee == false))
                {
                    if ((mat[ligne,colonne] == idJoueur) && (mat[ligne-1,colonne+1] == idJoueur) && (mat[ligne-2,colonne+2] == idJoueur) && (mat[ligne-3,colonne+3] == idJoueur))
                    {
                        combinaisonTrouvee = true;
                    }
                    ligne--;
                }
                colonne++;
            }
            return combinaisonTrouvee;
        }

        static bool CombinaisonGagnanteTrouvee(int[,] mat,int idJoueur)
            /* Parcourt la matrice à la recherche d'un alignement de 4 jetons créé par le joueur qui vient de jouer
             et renvoie true s'il y en a un. */ 
        {
            bool combinaisonTrouvee = false;
            combinaisonTrouvee = CombinaisonLigneGagnante(mat, idJoueur);
            if (combinaisonTrouvee == false)
            {
                combinaisonTrouvee = CombinaisonColonneGagnante(mat, idJoueur);
            }
            if (combinaisonTrouvee == false)
            {
                combinaisonTrouvee = CombinaisonDiagonaleDecroissante(mat, idJoueur);
            }
            if (combinaisonTrouvee == false)
            {
                combinaisonTrouvee = CombinaisonDiagonaleCroissante(mat, idJoueur);
            }
            return combinaisonTrouvee;
        }


        [System.STAThreadAttribute()]
        static void Main(string[] args)
        {
            /* Le plateau était à l'origine une matrice de chaînes de caractères pour pouvoir "personnaliser" les jetons
             mais il a été modifié en matrice d'entiers afin de pouvoir utiliser l'interface graphique d'où la présence
             des constantes. */ 
            const int ID_VIDE = 0;
            const int ID_JOUEUR_1 = 1;
            const int ID_JOUEUR_2 = 2;
            int nbTour = 0;
            bool finDuJeu = false; // finDuJeu correspond au cas où l'un des joueurs a gagné.
            int[,] plateau = InitialisationPlateau(6,7,ID_VIDE);
            Fenetre gui = new Fenetre(plateau);
            AfficherPlateau(plateau);

            while (finDuJeu == false && nbTour < plateau.GetLength(0)*plateau.GetLength(1))
            {
                if ((nbTour%2)+1 == 1)
                {
                    Console.WriteLine("C'est au tour du joueur 1.");
                    gui.changerMessage("C'est au tour du joueur 1.");
                    PhaseJoueur(plateau, ID_VIDE, ID_JOUEUR_1);
                    finDuJeu = CombinaisonGagnanteTrouvee(plateau, ID_JOUEUR_1);
                    if (finDuJeu == true)
                    {
                        Console.WriteLine("Le joueur 1 a gagné.");
                        gui.changerMessage("Le joueur 1 a gagné.");
                    }
                }
                else
                {
                    Console.WriteLine("C'est au tour du joueur 2.");
                    gui.changerMessage("C'est au tour du joueur 2.");
                    PhaseJoueur(plateau, ID_VIDE, ID_JOUEUR_2);
                    finDuJeu = CombinaisonGagnanteTrouvee(plateau, ID_JOUEUR_2);
                    if (finDuJeu == true)
                    {
                        Console.WriteLine("Le joueur 2 a gagné.");
                        gui.changerMessage("Le joueur 2 a gagné.");
                    }
                }
                AfficherPlateau(plateau);
                gui.rafraichirGrille();
                nbTour++;
            }
            if (finDuJeu == false) // Le cas où toutes les cases ont été remplis sans qu'il n'y ait de gagnant.
            {
                Console.WriteLine("Match nul !");
            }
            Console.WriteLine("\nFin du programme.");
            gui.changerMessage("Fin du programme.");
            Console.ReadKey();
        }
    }
}
