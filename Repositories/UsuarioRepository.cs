using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funtrip.Models;
using Funtrip.Repositories.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories
{
    public class UsuarioRepository 
    {
        private readonly DbContextOptions<DataBaseContext> _options;
        private readonly UserManager<Usuario> _userManager;
        public UsuarioRepository(DbContextOptions<DataBaseContext> options, UserManager<Usuario> userManager)
        {
            this._options = options;
            this._userManager = userManager;
        }

        public DbContext DbContext{ get { return Context as DbContext; } }

        public UserManager<Usuario> UserManager
        {
            get { return this._userManager; }
        }

        public DataBaseContext Context
        {
            get { return new DataBaseContext(this._options); }
        }

        public async Task<Usuario> GetUserById(string id)
        {
            return await UserManager.FindByIdAsync(id);
        }

        public List<Usuario> GetAllUsers()
        {
            return UserManager.Users.ToList();
        }
        
        public List<SelectListItem> GetUsersListItem()
        {
            return (from user in UserManager.Users.OrderBy(u => u.Email) select new SelectListItem { Text = user.Email, Value = user.Id }).ToList();
        }
    }
}