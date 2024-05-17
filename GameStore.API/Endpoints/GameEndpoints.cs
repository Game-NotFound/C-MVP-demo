using GameStore.API.DTOs;

namespace GameStore.API.Endpoints;

public static class GameEndpoints
{
    const string getGameEndPointName = "GetGame";

    private static readonly List<GameDTO> games = [
        new (1, "Street Fighter II", "Fighting", 19.99M, new DateOnly(1992,7,15)),
        new (2, "Final Fantasy XIV", "Role Playing", 59.99M, new DateOnly(2010, 9,30)),
        new (3, "FIFA 23", "Sports", 69.99M, new DateOnly(2022, 9,27))
    ];


    public static RouteGroupBuilder MapGamesEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("games");
        // GET /games
        group.MapGet("/", () => games);

        //GET /games/1
        group.MapGet("/{id}", (int id) =>
        {
            GameDTO? game = games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        }).WithName(getGameEndPointName);

        //POST /games
        group.MapPost("/", (CreateGameDTO newGame) =>
        {
            GameDTO game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );
            games.Add(game);

            return Results.CreatedAtRoute(getGameEndPointName, new { id = game.Id }, game);
        });


        // PUT
        group.MapPut("/{id}", (int id, UpdateGameDTO updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NoContent();
            }

            games[index] = new GameDTO(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });


        // DELETE
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        return group;
    }
}
