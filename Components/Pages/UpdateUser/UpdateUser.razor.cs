using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TallerNatBlazorApp.Data.Models;
using Core.Entities;
using TallerNatBlazorApp.Data.Services;
using Microsoft.AspNetCore.Components;

namespace TallerNatBlazorApp.Components.Pages.UpdateUser
{
    public partial class UpdateUser
    {
        [Inject]
        NavigationManager NavigationManager { get; set; } //para nav
        [Inject]
        TokenContainer tokenContainer { get; set; } //container donde manejamos el token
        //
        public UserDTO user { get; set; } = new();
        public string mensaje { get; set; } = string.Empty;
        public string tipoMessage { get; set; } = string.Empty;    
        //
        [Inject]
        public AuthService service { get; set; } //authentication service

        public async void Update()
        {
            var response = await service.UpdateUser(user, tokenContainer.token);

            if(response.Ok)
            {
                mensaje = "Usuario actualizado!! :D";
                tipoMessage = "alert alert-success";
            }
            else
            {
                mensaje = response.Message;
                tipoMessage = "alert alert-success";
            }
            Clean();
            StateHasChanged();
        }
        // *** Clean inputs ***
        public void Clean()
        {
            user.UserName = string.Empty;
            user.Password = string.Empty;
        }
        //
        /*public void Logout()
        {
            tokenContainer.Clean();
            NavigationManager.NavigateTo("/");
        }*/
    }
}