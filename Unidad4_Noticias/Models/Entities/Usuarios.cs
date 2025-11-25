using System;
using System.Collections.Generic;

namespace Unidad4_Noticias.Models.Entities;

public partial class Usuarios
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int RolId { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Comentarios> Comentarios { get; set; } = new List<Comentarios>();

    public virtual ICollection<Noticias> Noticias { get; set; } = new List<Noticias>();

    public virtual Roles Rol { get; set; } = null!;
}
