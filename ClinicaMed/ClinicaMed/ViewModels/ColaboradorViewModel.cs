using Microsoft.AspNetCore.Mvc;
using ClinicaMed.Models;

namespace ClinicaMed.ViewModels
{
    public class ColaboradorViewModel
    {
        public Colaborador Colaborador { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string AccountStatus { get; set; }
    }
}
