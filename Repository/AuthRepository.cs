using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Movies_Api.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Movies_Api.Repository
{
    public class AuthRepository : IAuth
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> userRole;
        IMapper _mapper;
        
        Jwt _jwt;
        public AuthRepository(UserManager<ApplicationUser> _userManager, IMapper mapper, IOptions<Jwt>  jwt , RoleManager<IdentityRole> _userRole)
        {
            userManager = _userManager;
            _mapper = mapper;
            _jwt = jwt.Value;
            userRole = _userRole;
        }

        public async Task<string> AddRole(Rolemodel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user is null|| !await userRole.RoleExistsAsync(model.Role))
            {
                return "Invalid UserID Or Role";
            }
            if (await userManager.IsInRoleAsync(user,model.Role))
            {
                return "User already assigned to this role";
            }
            var result = await userManager.AddToRoleAsync(user,model.Role);
            return result.Succeeded ? string.Empty : "Something Wrong";
        }

        public async Task<Authmodel> Login(Loginmodel model)
        {
            var authmodel = new Authmodel();
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null|| !await userManager.CheckPasswordAsync(user,model.Password))
            {
                authmodel.Message = "Email Or Password is Not found";
                return authmodel;
            }
            var jwtSecurityToken = await CreateJwtToken(user);
            var roles = await userManager.GetRolesAsync(user);

            authmodel.Email = model.Email;
            authmodel.Username = user.UserName;
            authmodel.IsAuthenticated = true;
            authmodel.Expiration = jwtSecurityToken.ValidTo;
            authmodel.Roles = roles.ToList();
            authmodel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);



            return authmodel;
        }

        public async Task <Authmodel> Register(Register model)
        {
            if (await userManager.FindByEmailAsync(model.Email)!=null)
            {
                return new Authmodel() {Message="Email is Already Registered" };
            }
            if (await userManager.FindByNameAsync(model.UserName)!=null)
            {
                return new Authmodel() { Message = "UserName is Already Registered" };
            }
            //var user = new ApplicationUser
            //{
            //    UserName = model.UserName,
            //    Email = model.Email,
            //    FirstName = model.FirstName,
            //    LastName = model.LastName
            //};
            var user=_mapper.Map<ApplicationUser>(model);

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new Authmodel { Message = errors };
            }

            await userManager.AddToRoleAsync(user, Roles.User_Role);

            var jwtSecurityToken = await CreateJwtToken(user);

            return new Authmodel
            {
                Email = user.Email,
                Expiration = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName
            };
        }



        // this method to generate JWT Token
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
