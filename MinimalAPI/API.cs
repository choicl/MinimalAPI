namespace MinimalAPI
{
    public static class API
    {
        //extension method for WebApplication class
        public static void ConfigureApi(this WebApplication app)
        {
            //api endpoints mapping
            app.MapGet("/Users", GetUsers);
            app.MapGet("/Users/{id}", GetUser);
            app.MapPost("/Users", InsertUser);
            app.MapPut("/Users", UpdateUser);
            app.MapDelete("/Users/{id}", DeleteUser);
        }

        //using IResult to wrap the results in proper http 
        //makes more spesific why the things don't work
        private static async Task<IResult> GetUsers(IUserData data)
        {
            try
            {
                //wrapping the results in a http message of 200
                return Results.Ok(await data.GetUsers());
            }
            catch (Exception ex)
            {
                //can add logging here
                return Results.Problem(ex.Message);
            }
        }
        //id comes from the call to the api
        //data comes from di
        private static async Task<IResult> GetUser(int id, IUserData data)
        {
            try
            {
                var result = await data.GetUser(id);
                if (result == null) return Results.NotFound();
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> InsertUser(UserModel user, IUserData data)
        {
            try
            {
                await data.InsertUser(user);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
        private static async Task<IResult> UpdateUser(UserModel user, IUserData data)
        {
            try
            {
                await data.UpdateUser(user);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
        private static async Task<IResult> DeleteUser(int id, IUserData data)
        {
            try
            {
                await data.DeleteUser(id);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
