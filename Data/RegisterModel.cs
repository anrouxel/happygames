using System;
using System.ComponentModel.DataAnnotations;

namespace happygames.Data;

public class RegisterModel
{
    [Required]
    public string? UserName { get; set; }

    [Required]
    public string? UserFirstName { get; set; }

    [Required]
    public DateTime UserDate = DateTime.Now;
};