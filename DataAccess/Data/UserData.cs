using DataAccess.DbAccess;
using DataAccess.Models;

namespace DataAccess.Data
{
    /// <summary>
    /// Accessing data for User table.
    /// </summary>
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _db;

        //accessing the interface methods to work with db
        public UserData(ISqlDataAccess db)
        {
            _db = db;
        }

        //Loading the data from stored procedure,putting it in IEnumerable of this model
        //dynamic used for adding parameters dynamicaly like new{} anonymous object later
        public Task<IEnumerable<UserModel>> GetUsers() =>
            _db.LoadData<UserModel, dynamic>(storedProcedure: "dbo.sp.User_GetAll", new { });

        //Getting async one user by id with stored procedure
        public async Task<UserModel?> GetUser(int id)
        {
            var user = await _db.LoadData<UserModel, dynamic>(
                storedProcedure: "dbo.sp.User_Get",
                new { Id = id });
            //will either return first record in IEnumerable or default value for UserModel,which is null
            return user.FirstOrDefault();
        }

        //Adding new users 
        public Task InsertUser(UserModel user) =>
            _db.SaveData(storedProcedure: "dbo.spUser_Insert", new { user.FirstName, user.LastName });

        //Updating users
        public Task UpdateUser(UserModel user) =>
            _db.SaveData(storedProcedure: "dbo.spUser_Update", user);

        //Deleting users by id
        public Task DeleteUser(int id) =>
            _db.SaveData(storedProcedure: "dbo.spUser_Delete", new { Id = id });
    }
}
