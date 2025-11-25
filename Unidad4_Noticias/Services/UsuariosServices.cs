using Unidad4_Noticias.Models.Entities;
using Unidad4_Noticias.Areas.Admin.Models;
using Unidad4_Noticias.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Unidad4_Noticias.Services
{
    [Authorize]
    public class UsuariosService
    {
        private readonly Repository<Usuarios> _usuariosRepo;

        public UsuariosService(
            Repository<Usuarios> usuariosRepo)
        {
            _usuariosRepo = usuariosRepo;
        }

        public Usuarios? Autenticar(string username, string password)
        {
            var passwordHash = Sha256Helper.ComputeHash(password);

            var usuario = _usuariosRepo.GetAll()
                .AsQueryable().Include(x=>x.Rol)
                .FirstOrDefault(u => u.Username == username &&
                                   u.PasswordHash == passwordHash &&
                                   u.Activo == true);

            return usuario;
        }

        public AdminUsuariosIndexViewModel GetUsuariosAdmin()
        {
            var usuarios = _usuariosRepo.GetAll()
                .Where(u => u.Activo == true)
                .OrderBy(u => u.Username)
                .Select(u => new UsuarioModel
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    FechaRegistro = u.FechaRegistro ?? DateTime.Now
                });

            return new AdminUsuariosIndexViewModel
            {
                Usuarios = usuarios
            };
        }

        public bool CrearUsuario(UsuarioCreateModel model)
        {
            try
            {
                var usuarioExistente = _usuariosRepo.GetAll()
                    .FirstOrDefault(u => u.Username == model.Username || u.Email == model.Email);

                if (usuarioExistente != null) return false;

               
                var usuario = new Usuarios
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = Sha256Helper.ComputeHash(model.Password),
                    RolId = 2, //Reportero
                    FechaRegistro = DateTime.Now,
                    Activo = true
                };

                _usuariosRepo.Insert(usuario);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public UsuarioEditModel? GetUsuarioParaEditar(int userId)
        {
            var usuario = _usuariosRepo.Get(userId);
            if (usuario == null || usuario.Activo == false) return null;

            return new UsuarioEditModel
            {
                UserId = usuario.Id,
                Username = usuario.Username,
                Email = usuario.Email
            };
        }

        public bool EditarUsuario(UsuarioEditModel model)
        {
            try
            {
                var usuario = _usuariosRepo.Get(model.UserId);
                if (usuario == null) return false;

                var usuarioExistente = _usuariosRepo.GetAll()
                    .FirstOrDefault(u => u.Id != model.UserId &&
                                       (u.Username == model.Username || u.Email == model.Email));

                if (usuarioExistente != null) return false;

                usuario.Username = model.Username;
                usuario.Email = model.Email;

                _usuariosRepo.Update(usuario);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CambiarPassword(CambioPasswordModel model)
        {
            try
            {
                var usuario = _usuariosRepo.Get(model.UserId);
                if (usuario == null) return false;

                usuario.PasswordHash = Sha256Helper.ComputeHash(model.NewPassword);
                _usuariosRepo.Update(usuario);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public UsuarioDeleteModel? GetUsuarioParaEliminar(int userId)
        {
            var usuario = _usuariosRepo.Get(userId);
            if (usuario == null || usuario.Activo == false) return null;

            return new UsuarioDeleteModel
            {
                UserId = usuario.Id,
                UserName = usuario.Username
            };
        }

        public bool EliminarUsuario(int userId)
        {
            try
            {
                var usuario = _usuariosRepo.Get(userId);
                if (usuario == null) return false;

                usuario.Activo = false;
                _usuariosRepo.Update(usuario);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Usuarios? GetUsuarioById(int id)
        {
            return _usuariosRepo.Get(id);
        }
    }
}