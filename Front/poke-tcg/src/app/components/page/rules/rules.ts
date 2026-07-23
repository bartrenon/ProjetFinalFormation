import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

interface TurnPhase {
  id: number;
  title: string;
  tag: string;
  detail: string;
  bullets: string[];
}
 
interface WinCondition {
  icon: string;
  title: string;
  detail: string;
}

@Component({
  selector: 'app-rules',
  imports: [CommonModule],
  templateUrl: './rules.html',
  styleUrl: './rules.scss',
})
export class Rules {

 readonly openPhase = signal<number>(1);
 
  togglePhase(id: number): void {
    this.openPhase.set(this.openPhase() === id ? -1 : id);
  }
 
  readonly winConditions: WinCondition[] = [
    {
      icon: '⬡',
      title: "6 Pions Prix récupérés",
      detail:
        "Chaque Pokémon mis K.O. chez l'adversaire vous fait piocher un Pion Prix. Le premier joueur à en avoir récupéré 6 gagne la partie.",
    },
    {
      icon: '🂠',
      title: "L'adversaire n'a plus de Pokémon",
      detail:
        "Si tous les Pokémon d'un joueur (actif et banc) sont mis K.O. et qu'il ne peut plus en poser, il perd immédiatement.",
    },
    {
      icon: '≡',
      title: "L'adversaire ne peut plus piocher",
      detail:
        "Si un joueur doit piocher alors que son deck est vide, il perd la partie au début de son tour suivant.",
    },
  ];
 
  readonly deckParts = [
    {
      name: "Cartes Pokémon",
      count: "Variable",
      detail:
        "Pokémon de base et leurs évolutions. Chaque carte affiche des PV, un ou plusieurs types, des attaques, une faiblesse, une résistance et un coût de retraite.",
    },
    {
      name: "Cartes Dresseur",
      count: "Variable",
      detail:
        "Objets, Supporters et Stades. Elles offrent des effets ponctuels : piocher, soigner, chercher une carte, perturber l'adversaire.",
    },
    {
      name: "Cartes Énergie",
      count: "Variable",
      detail:
        "Énergies de base ou spéciales. On les attache aux Pokémon pour payer le coût de leurs attaques et de leur retraite.",
    },
  ];
 
  readonly setupSteps = [
    "Mélangez votre deck de 60 cartes et piochez une main de 7 cartes.",
    "Si votre main ne contient aucun Pokémon de base, montrez-la à l'adversaire, mélangez et repiochez (mulligan).",
    "Placez un Pokémon de base face cachée comme Pokémon actif, et jusqu'à 5 autres sur votre banc.",
    "Retournez 6 cartes du dessus de votre deck : ce sont vos Pions Prix, qu'on ne regarde pas.",
    "Retournez vos Pokémon actif et de banc face visible : la partie peut commencer.",
  ];
 
  readonly turnPhases: TurnPhase[] = [
    {
      id: 1,
      title: "Phase de pioche",
      tag: "Obligatoire",
      detail:
        "En début de tour (sauf le tout premier joueur), piochez une carte du dessus de votre deck. C'est la seule action imposée du tour.",
      bullets: [],
    },
    {
      id: 2,
      title: "Phase d'actions",
      tag: "Dans l'ordre de votre choix",
      detail:
        "Vous pouvez enchaîner librement, autant de fois que les règles le permettent :",
      bullets: [
        "Poser un ou plusieurs Pokémon de base sur le banc",
        "Faire évoluer un Pokémon déjà en jeu (pas au premier tour, ni le tour où il vient d'arriver)",
        "Attacher une carte Énergie à un de vos Pokémon (une seule par tour)",
        "Jouer des cartes Dresseur : Objets (illimités), Supporters (un seul par tour), Stades",
        "Utiliser les capacités de vos Pokémon",
        "Faire battre en retraite votre Pokémon actif en payant son coût en énergie",
      ],
    },
    {
      id: 3,
      title: "Phase d'attaque",
      tag: "Optionnelle, termine le tour",
      detail:
        "Votre Pokémon actif peut utiliser une attaque dont vous payez le coût en énergie. Les dégâts infligés dépendent de la faiblesse et de la résistance du Pokémon visé.",
      bullets: [
        "Faiblesse : les dégâts sont généralement doublés",
        "Résistance : les dégâts sont généralement réduits de 30",
        "Attaquer met fin à votre tour",
      ],
    },
    {
      id: 4,
      title: "Résolution et K.O.",
      tag: "Automatique",
      detail:
        "Si les dégâts subis égalent ou dépassent ses PV restants, le Pokémon est mis K.O. et retiré du jeu.",
      bullets: [
        "Le joueur qui a infligé le K.O. pioche un Pion Prix",
        "Le joueur qui a perdu son Pokémon actif en remet un depuis son banc",
        "La main passe ensuite au joueur suivant",
      ],
    },
  ];
}
 
