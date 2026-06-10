export interface UserCreate
{
    username: string;
    email: string;
    // Le backend doit hasher le mot de passe avant de l'enregistrer.
    password: string;
}
