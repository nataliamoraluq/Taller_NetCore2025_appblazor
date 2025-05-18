using Newtonsoft.Json.Linq;

namespace TallerNatBlazorApp.Components
{
    //token base prof. Guillermo 
    public class TokenContainer
    {
        //
        public string? token { get; private set; } = string.Empty;
        //
        public void AsignarToken(string newToken)
        {
            token = newToken;
            NotifyStateChanged();
        }
        //
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
        //
        public void Clear()
        {
            token = null;
            NotifyStateChanged();
        }
        //
    }
}