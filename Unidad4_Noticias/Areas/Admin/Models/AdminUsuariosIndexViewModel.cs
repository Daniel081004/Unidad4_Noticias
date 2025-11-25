using System.ComponentModel.DataAnnotations;

namespace Unidad4_Noticias.Areas.Admin.Models
{
    public class AdminUsuariosIndexViewModel
    {
        public IEnumerable<UsuarioModel>? Usuarios { get; set; }
        public UsuarioCreateModel? NuevoUsuario { get; set; }
        public UsuarioEditModel? UsuarioEditar { get; set; }
        public CambioPasswordModel? CambioPassword { get; set; }
        public UsuarioDeleteModel? UsuarioEliminar { get; set; }
    }

    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
    }

    public class UsuarioCreateModel
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [StringLength(50, ErrorMessage = "El usuario no puede exceder 50 caracteres")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirmar contraseña es requerido")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; } = null!;
    }

    public class UsuarioEditModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [StringLength(50, ErrorMessage = "El usuario no puede exceder 50 caracteres")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string Email { get; set; } = null!;
    }

    public class CambioPasswordModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es requerida")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "Confirmar contraseña es requerido")]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; } = null!;
    }

    public class UsuarioDeleteModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
    }
}
