using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeManager.Data;
using VibeManager.Pages;

namespace VibeManager.Models.Controllers
{
    /// <summary>
    /// Proporciona métodos para autenticar, gestionar y manipular usuarios en la base de datos.
    /// </summary>
    public static class UsersOrm
    {
        /// <summary>
        /// Autentica a un usuario utilizando su nombre completo o correo electrónico y contraseña.
        /// </summary>
        /// <param name="username">Nombre completo o correo electrónico del usuario.</param>
        /// <param name="password">Contraseña del usuario.</param>
        /// <returns>Una instancia de <see cref="UserSession"/> si las credenciales son válidas; de lo contrario, <c>null</c>.</returns>
        public static UserSession Login(string username, string password)
        {
            try
            {
                var user = Orm.db.USERS
                    .Where(u => (u.fullname == username || u.email == username) && u.password == password)
                    .Select(u => new UserSession
                    {
                        Id = u.id,
                        RoleId = u.id_rol,
                        FullName = u.fullname
                    })
                    .FirstOrDefault();

                return user;
            }
            catch (SqlException sqlException)
            {
                Console.WriteLine(Orm.ErrorMessage(sqlException));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Obtiene el número total de usuarios registrados en la base de datos.
        /// </summary>
        /// <returns>Un número entero que representa el total de usuarios.</returns>
        public static int getTotalUsers()
        {
            int totalUsers = 0;

            try
            {
                totalUsers = Orm.db.USERS
                    .Count();
            }
            catch (SqlException sqlException)
            {
                Console.WriteLine(Orm.ErrorMessage(sqlException));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }


            return totalUsers;
        }

        /// <summary>
        /// Recupera todos los usuarios desde la base de datos con información detallada, incluyendo rol.
        /// </summary>
        /// <returns>Una lista de objetos <see cref="User"/>.</returns>
        public static List<User> GetAllUsers()
        {
            try
            {
                var users = Orm.db.USERS
                    .Select(u => new User
                    {
                        Id = u.id,
                        Fullname = u.fullname,
                        Email = u.email,
                        IdRol = u.id_rol,
                        RolName = u.ROL.name
                    })
                    .ToList();

                return users;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(Orm.ErrorMessage(ex));
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error: " + ex.Message);
            }
            return new List<User>();
        }

        /// <summary>
        /// Obtiene una lista de roles únicos utilizados por los usuarios.
        /// </summary>
        /// <returns>Una lista de objetos <see cref="Role"/> con roles distintos.</returns>
        public static List<Role> GetDistinctRolesFromUsers()
        {
            return GetAllUsers()
                .Select(u => new Role { Id = u.IdRol, Name = u.RolName })
                .GroupBy(r => r.Id)
                .Select(g => g.First())
                .ToList();
        }

        /// <summary>
        /// Crea un nuevo usuario o actualiza uno existente en la base de datos.
        /// </summary>
        /// <param name="user">El objeto <see cref="User"/> con la información del usuario a guardar.</param>
        /// <returns><c>true</c> si la operación fue exitosa; en caso contrario, <c>false</c>.</returns>
        public static bool SaveUser(User user)
        {
            try
            {
                if (user.Id == 0)
                {
                    var newUser = new USERS
                    {
                        fullname = user.Fullname,
                        email = user.Email,
                        id_rol = user.IdRol,
                        password = "default" 
                    };

                    Orm.db.USERS.Add(newUser);
                }
                else
                {
                    var existing = Orm.db.USERS.FirstOrDefault(u => u.id == user.Id);
                    if (existing != null)
                    {
                        existing.fullname = user.Fullname;
                        existing.email = user.Email;
                        existing.id_rol = user.IdRol;
                    }
                }

                Orm.db.SaveChanges();
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(Orm.ErrorMessage(ex));
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Elimina un usuario de la base de datos, junto con sus mensajes, chats, tickets, eventos y reservas relacionados.
        /// </summary>
        /// <param name="id">ID del usuario a eliminar.</param>
        /// <returns><c>true</c> si la eliminación fue exitosa; en caso contrario, <c>false</c>.</returns>
        public static bool DeleteUser(int id)
        {
            try
            {
                var user = Orm.db.USERS.FirstOrDefault(u => u.id == id);
                if (user == null)
                {
                    return false;
                }
                    
                // 1. MESSAGES
                var userChats = Orm.db.CHAT.Where(c => c.id_user == id).ToList();
                var chatIds = userChats.Select(c => c.id).ToList();
                var messages = Orm.db.MESSAGES.Where(m => m.sender_id == id || chatIds.Contains(m.id_chat)).ToList();
                Orm.db.MESSAGES.RemoveRange(messages);

                // 2. CHAT
                Orm.db.CHAT.RemoveRange(userChats);

                // 3. TICKETS
                var tickets = Orm.db.TICKETS.Where(t => t.id_user == id).ToList();
                Orm.db.TICKETS.RemoveRange(tickets);

                // 4. EVENTS organizados por el usuario
                var events = Orm.db.EVENTS.Where(e => e.id_organizer == id).ToList();
                var eventIds = events.Select(e => e.id).ToList();

                // 5. RESERVES relacionados con eventos del usuario
                var reserves = Orm.db.RESERVES.Where(r => eventIds.Contains(r.id_event)).ToList();
                Orm.db.RESERVES.RemoveRange(reserves);

                // 6. EVENTS
                Orm.db.EVENTS.RemoveRange(events);

                // 7. USERS
                Orm.db.USERS.Remove(user);

                Orm.db.SaveChanges();
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(Orm.ErrorMessage(ex));
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar usuario: " + ex.Message);
                return false;
            }
        }

    }
}
