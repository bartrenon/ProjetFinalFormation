import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

interface CardFamily {
  name: string;
  count: string;
  detail: string;
}
 
interface Callout {
  id: number;
  x: number;
  y: number;
  label: string;
  detail: string;
}
 
interface Rarity {
  id: string;
  symbol: string;
  name: string;
  era: string;
  detail: string;
  swatch: string; 
}
 
interface EnergyType {
  name: string;
  color: string;
  textColor: string;
}

@Component({
  selector: 'app-anatomy',
  imports: [CommonModule],
  templateUrl: './anatomy.html',
  styleUrl: './anatomy.scss',
})
export class Anatomy {
 
  readonly activeCallout = signal<number>(1);
 
  readonly activeRarity = signal<string>('common');
 
  setCallout(id: number): void {
    this.activeCallout.set(id);
  }
 
  setRarity(id: string): void {
    this.activeRarity.set(id);
  }
 
  get currentRarity(): Rarity {
    return this.rarities.find((r) => r.id === this.activeRarity())!;
  }
 
  get currentCallout(): Callout {
    return this.callouts.find((c) => c.id === this.activeCallout())!;
  }
 
  readonly cardFamilies: CardFamily[] = [
    {
      name: "Cartes Pokémon",
      count: "La majorité du deck",
      detail:
        "Pokémon de base ou évolutions. Chaque carte porte des PV, un type, une ou plusieurs attaques et parfois une capacité.",
    },
    {
      name: "Cartes Dresseur",
      count: "3 sous-familles",
      detail:
        "Objet (jouable librement), Supporter (un par tour) et Stade (un stade actif à la fois, affecte les deux joueurs).",
    },
    {
      name: "Cartes Énergie",
      count: "Base ou spéciale",
      detail:
        "Les énergies de base fournissent un type d'énergie pur ; les énergies spéciales ajoutent des effets, souvent au prix d'une contrainte.",
    },
  ];
 
  readonly callouts: Callout[] = [
    { id: 1, x: 50, y: 8, label: "Nom et PV", detail: "Le nom du Pokémon à gauche, ses Points de Vie (PV) à droite : la quantité de dégâts qu'il peut encaisser avant d'être mis K.O." },
    { id: 2, x: 88, y: 8, label: "Type", detail: "Le symbole en haut à droite indique le type du Pokémon, qui détermine ses éventuelles faiblesses et résistances." },
    { id: 3, x: 50, y: 42, label: "Illustration", detail: "L'artwork de la carte. Sa mise en valeur varie fortement selon la rareté : encadré simple, effet holographique, full art..." },
    { id: 4, x: 50, y: 66, label: "Attaques", detail: "Chaque attaque affiche son coût en énergie à gauche, son nom, et les dégâts infligés à droite. Certaines ont un effet additionnel décrit en petit texte." },
    { id: 5, x: 30, y: 86, label: "Faiblesse / Résistance", detail: "En bas à gauche : le type contre lequel ce Pokémon encaisse plus de dégâts (faiblesse) ou moins de dégâts (résistance)." },
    { id: 6, x: 62, y: 86, label: "Coût de retraite", detail: "Le nombre d'énergies à défausser pour retirer ce Pokémon du poste actif et le remplacer par un Pokémon du banc." },
    { id: 7, x: 10, y: 90, label: "Rareté et numéro", detail: "En bas à droite : le symbole de rareté et le numéro de la carte dans son extension (ex. 24/102)." },
  ];
 
  readonly rarities: Rarity[] = [
    {
      id: 'common',
      symbol: '●',
      name: "Commune",
      era: "Toutes extensions",
      detail:
        "La rareté la plus fréquente dans un booster. La plupart sont des Pokémon de base sans effet spectaculaire.",
      swatch: 'ink',
    },
    {
      id: 'uncommon',
      symbol: '◆',
      name: "Peu commune",
      era: "Toutes extensions",
      detail:
        "Un cran au-dessus de la Commune : souvent des évolutions intermédiaires ou des cartes Dresseur utiles.",
      swatch: 'ink',
    },
    {
      id: 'rare',
      symbol: '★',
      name: "Rare",
      era: "Toutes extensions",
      detail:
        "Identifiée par une étoile noire. Une carte Rare classique n'est pas forcément holographique.",
      swatch: 'ink',
    },
    {
      id: 'holo',
      symbol: '★',
      name: "Rare Holo",
      era: "Toutes extensions",
      detail:
        "Même étoile que la Rare, mais l'illustration bénéficie d'un effet holographique brillant, très recherché depuis les débuts du jeu.",
      swatch: 'gold',
    },
    {
      id: 'ultra',
      symbol: '★',
      name: "Ultra Rare",
      era: "Depuis les ères EX / GX / V",
      detail:
        "Regroupe les cartes à mécanique spéciale (ex, GX, V, VMAX...), souvent en Full Art : l'illustration recouvre toute la carte.",
      swatch: 'gold',
    },
    {
      id: 'secret',
      symbol: '★',
      name: "Secrète",
      era: "Fin d'extension",
      detail:
        "Numérotée au-delà du total officiel de l'extension (ex. 196/195). Regroupe souvent les cartes Rainbow ou Gold les plus prisées.",
      swatch: 'gold',
    },
  ];
 
  readonly energyTypes: EnergyType[] = [
    { name: 'Plante', color: '#5b9c46', textColor: '#f5f2e9' },
    { name: 'Feu', color: '#e4562b', textColor: '#f5f2e9' },
    { name: 'Eau', color: '#4a8fd1', textColor: '#f5f2e9' },
    { name: 'Électrique', color: '#e8c02a', textColor: '#1b1b1b' },
    { name: 'Psy', color: '#8f6bb0', textColor: '#f5f2e9' },
    { name: 'Combat', color: '#b6602f', textColor: '#f5f2e9' },
    { name: 'Obscurité', color: '#4a4340', textColor: '#f5f2e9' },
    { name: 'Métal', color: '#98a3ab', textColor: '#1b1b1b' },
    { name: 'Dragon', color: '#a8862f', textColor: '#f5f2e9' },
    { name: 'Incolore', color: '#d9d2bd', textColor: '#1b1b1b' },
  ];
}