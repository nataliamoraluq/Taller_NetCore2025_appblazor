using TallerNatBlazorApp.Data.Models;
using TallerNatBlazorApp.Data.Services;
using Microsoft.AspNetCore.Components;

namespace TallerNatBlazorApp.Components.Pages.Register
{
    public partial class Register
    {
        // using the User DTO
        public UserDTO user = new();
        //
        [Inject]
        public AuthService? service { get; set; }
        //
        public string mensaje { get; set; } = string.Empty;
        public string tipoMessage { get; set; } = string.Empty;
        //
        public async void RegisterUser()
        {
            if(user.UserName.Length < 2 && user.Password.Length < 2)
            {
                //!!!*** detallito con esto ojo
                mensaje = "estos campos no pueden estar vacios";
                tipoMessage = "alert alert-danger";
                return;
            }
            //
            var response = await service.Register(user);

            if(response.Ok)
            {
                mensaje = "Usuario creado exitosamente!";
                tipoMessage = "alert alert-success";
            }
            else
            {
                mensaje = response.Message;
                tipoMessage = "alert alert-danger";
            }
            Clean();
            StateHasChanged();
        }
        //
        public void Clean()
        {
            user.Password = "";
            user.UserName = "";
        }

    }
}