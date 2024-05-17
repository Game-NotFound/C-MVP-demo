namespace GameStore.API;

public record class CreateGameDTO( 
    string Name, 
    string Genre, 
    decimal Price, 
    DateOnly ReleaseDate
);
