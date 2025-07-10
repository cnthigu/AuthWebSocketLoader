using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ServerWebSocket.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

public class AuthBehavior : WebSocketBehavior
{
    private static readonly string jwtSecret = "minha_chave_super_secreta_1234567890";

    protected override void OnMessage(MessageEventArgs e)
    {
        try
        {
            var loginRequest = JsonConvert.DeserializeObject<LoginRequest>(e.Data);

            if (loginRequest == null)
            {
                SendError("Dados inválidos");
                return;
            }

            using var db = new AppDbContext();

            var user = db.Users.FirstOrDefault(u => u.Username == loginRequest.Username);

            if (user == null)
            {
                SendError("Usuário não encontrado");
                return;
            }

            if (user.PasswordHash != loginRequest.Password)
            {
                SendError("Senha incorreta");
                return;
            }

            if (user.DataExpiracao < DateTime.Now)
            {
                SendError("Conta expirada. Renove sua assinatura.");
                return;
            }

            if (string.IsNullOrEmpty(user.HWID))
            {
                user.HWID = loginRequest.HWID;
                db.SaveChanges();
            }
            else if (user.HWID != loginRequest.HWID)
            {
                SendError("HWID não corresponde. Login negado.");
                return;
            }

            string token = GenerateJwtToken(user.Username);

            var response = new
            {
                success = true,
                token = token
            };

            Send(JsonConvert.SerializeObject(response));
        }
        catch (Exception ex)
        {
            SendError("Erro interno no servidor: " + ex.Message);
        }
    }

    private void SendError(string message)
    {
        var response = new
        {
            success = false,
            error = message
        };
        Send(JsonConvert.SerializeObject(response));
    }

    private string GenerateJwtToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtSecret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, username)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
