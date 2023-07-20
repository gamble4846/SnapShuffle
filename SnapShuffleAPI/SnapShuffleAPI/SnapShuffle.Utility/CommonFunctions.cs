using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SnapShuffle.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SnapShuffle.Utility
{
    public class CommonFunctions
    {
        public string ContentRootPath { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public CommonFunctions(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public string GetConntectionString()
        {
            //return "Server=tcp:chinese-jimmy.at.ply.gg,52648;Database=SnapShuffle;User Id=rohan;Password=18158114;";
            return "Server=localhost;Database=SnapShuffle;User Id=rohan;Password=18158114;";
        }

        public string GetDefaultPrivateKey()
        {
            var privateKey = @"-----BEGIN RSA PRIVATE KEY-----
MIICXQIBAAKBgQC0BRJHY8h6fzejVXzrFFjR9URzVRW9W/GvUx36IA3M+wEym58B
g/z2/ZRTXOFbL6b0Qx60nkeTSPH+kce6gNxkBkBRep8Q6x1/uOOruaKJLTEbiGP9
mQHuOFgQmmzJsrHmmol150T8ClUewRGXTzJq+xq/1aX6FUedICLuyBT6yQIDAQAB
AoGBAJqogLu0jlgvU62PlHKiWGyJOvWa88Jra4gk8GwjfbfNLYo9CK5Ups/Lq9Ky
ySl+FwFjaM3j7borwTvkIqOoca5MX0DqjNt1X3Da0YPAC52ZAON960t/EVGeecLU
xT7tTGuW889CEW5XyIkXIL6EBzh6DcfGWqsKlUTWU/kpZOfBAkEA3eEWkpyuZoTX
0uuLiOv66OK/Nz+t/BXj0dmmp5eqiwxeIejdwgT71t6GVhEcvoCEuAUw/Qz6ugmf
ir5H4CaK4wJBAM+0EQgZ7Yd06IZLgFsgx0e7bPtOBobk1eCV+m7CHh/03FWQx5+J
QY89aVoBwzf9NZ/PM9HSK8gSWCy91+CXt2MCQQCXzTYKXNQdI+odTjYLGZhy2R+G
BzVb8QYLPuP8aZGuzGlivdVqtsouedRi8hY+Z+Nlm3emyciIm6jh3cmDHBuNAkA1
8aFexkUfoihmAKP0hv3azn3OgwRE8pftvWYOcBIc4J59Bp4h/CbvydZbzwthtjzA
JbNiskA2tTT7Yc74gM7bAkB3Gvc4IADjOygPSjVXr8jk7s307ZinibFSF+alm5RI
bg1MdwGLNyat5b3X8OpWtg3aJEGA0C+yCZoTOBi6a1Kl
-----END RSA PRIVATE KEY-----";

            var publicKey = @"-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC0BRJHY8h6fzejVXzrFFjR9URz
VRW9W/GvUx36IA3M+wEym58Bg/z2/ZRTXOFbL6b0Qx60nkeTSPH+kce6gNxkBkBR
ep8Q6x1/uOOruaKJLTEbiGP9mQHuOFgQmmzJsrHmmol150T8ClUewRGXTzJq+xq/
1aX6FUedICLuyBT6yQIDAQAB
-----END PUBLIC KEY-----";

            return "<RSAKeyValue><Modulus>tAUSR2PIen83o1V86xRY0fVEc1UVvVvxr1Md+iANzPsBMpufAYP89v2UU1zhWy+m9EMetJ5Hk0jx/pHHuoDcZAZAUXqfEOsdf7jjq7miiS0xG4hj/ZkB7jhYEJpsybKx5pqJdedE/ApVHsERl08yavsav9Wl+hVHnSAi7sgU+sk=</Modulus><Exponent>AQAB</Exponent><P>3eEWkpyuZoTX0uuLiOv66OK/Nz+t/BXj0dmmp5eqiwxeIejdwgT71t6GVhEcvoCEuAUw/Qz6ugmfir5H4CaK4w==</P><Q>z7QRCBnth3TohkuAWyDHR7ts+04GhuTV4JX6bsIeH/TcVZDHn4lBjz1pWgHDN/01n88z0dIryBJYLL3X4Je3Yw==</Q><DP>l802ClzUHSPqHU42CxmYctkfhgc1W/EGCz7j/GmRrsxpYr3VarbKLnnUYvIWPmfjZZt3psnIiJuo4d3JgxwbjQ==</DP><DQ>NfGhXsZFH6IoZgCj9Ib92s59zoMERPKX7b1mDnASHOCefQaeIfwm78nWW88LYbY8wCWzYrJANrU0+2HO+IDO2w==</DQ><InverseQ>dxr3OCAA4zsoD0o1V6/I5O7N9O2Yp4mxUhfmpZuUSG4NTHcBizcmreW91/DqVrYN2iRBgNAvsgmaEzgYumtSpQ==</InverseQ><D>mqiAu7SOWC9TrY+UcqJYbIk69ZrzwmtriCTwbCN9t80tij0IrlSmz8ur0rLJKX4XAWNozePtuivBO+Qio6hxrkxfQOqM23VfcNrRg8ALnZkA433rS38RUZ55wtTFPu1Ma5bzz0IRblfIiRcgvoQHOHoNx8ZaqwqVRNZT+Slk58E=</D></RSAKeyValue>";
        }

        public string GetJWTSecret()
        {
            return "401d1989-bc6b-4f0a-990a-83535b25da7f-a5a8e748-0b91-4d06-bc4b-06e2b851db33";
        }

        public string DecryptRSAEncryptedString(string cipherText, string privateKey = null)
        {
            if (String.IsNullOrEmpty(privateKey))
            {
                privateKey = GetDefaultPrivateKey();
            }
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);
            var encryptedBytes = Convert.FromBase64String(cipherText);
            var decryptedBytes = rsa.Decrypt(encryptedBytes, false);
            var decryptedData = Encoding.UTF8.GetString(decryptedBytes);

            return decryptedData;
        }

        public string Encrypt(string clearText, string EncryptionKey = null)
        {
            if (String.IsNullOrEmpty(EncryptionKey))
            {
                EncryptionKey = GetJWTSecret();
            }
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public string CreateJWTToken(List<ClaimModel> Claims)
        {
            var Secret = GetJWTSecret();
            var ValidIssuer = "https://localhost:44376/";
            var ValidAudience = "https://localhost:44376/";

            var claims = new List<Claim>();

            foreach (var claim in Claims)
            {
                claims.Add(new Claim(claim.ClaimName, JsonConvert.SerializeObject(claim.Data)));
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(issuer: ValidIssuer, audience: ValidAudience, claims: claims, expires: DateTime.Now.AddYears(1), signingCredentials: signinCredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }

        public string GetRandomPrintScreenId()
        {
            string[] characters = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            Random random = new Random();
            string Id = characters[random.Next(0, characters.Length)] + characters[random.Next(0, characters.Length)] + characters[random.Next(0, characters.Length)] + characters[random.Next(0, characters.Length)] + characters[random.Next(0, characters.Length)] + characters[random.Next(0, characters.Length)];
            return Id;
        }

        public string GetSrcOfTagFromHTMLById(string htmlString, string id)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlString);
                HtmlNode element = doc.GetElementbyId(id);
                if (element != null)
                {
                    var currentSRC = element.GetAttributeValue("src", "");
                    if (String.IsNullOrEmpty(currentSRC))
                    {
                        return null;
                    }
                    else
                    {
                        return currentSRC;
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public byte[] ConvertUrlToByteArray(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    byte[] imageBytes = webClient.DownloadData(url);
                    return imageBytes;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public byte[] EncryptImageBytes(byte[] imageBytes)
        {
            Random random = new Random();
            for (int i = 0; i < imageBytes.Length - 2; i += 3)
            {
                imageBytes[i] = (byte)random.Next(256); // Red
                imageBytes[i + 1] = (byte)random.Next(256); // Green
                imageBytes[i + 2] = (byte)random.Next(256); // Blue
            }
            return imageBytes;
        }

        public async Task<ImgurResponseModel> UploadImageToImgur(byte[] bytes)
        {
            var httpclient = new HttpClient();
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", "263bbc738ab2de2");
            httpclient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "text/plain");
            string temp_inBase64 = Convert.ToBase64String(bytes);
            var response = await httpclient.PostAsync("https://api.imgur.com/3/Image", new StringContent(temp_inBase64));
            var stringcontent = await response.Content.ReadAsStringAsync();
            var ImgurResponseModel = JsonConvert.DeserializeObject<ImgurResponseModel>(stringcontent);

            if (ImgurResponseModel.success)
            {
                return ImgurResponseModel;
            }
            else
            {
                return null;
            }
        }

        public byte[] MovePixelsImage(byte[] inputBytes, int MoveCount, bool IsEncrypt)
        {
            Bitmap bmp;
            using (var ms = new MemoryStream(inputBytes))
            {
                bmp = new Bitmap(ms);
            }

            List<List<ColorWapper>> PixalDoubleArray = new List<List<ColorWapper>>();

            Bitmap NewBmp = bmp;


            for (int i = 0; i < bmp.Width; i++)
            {
                List <ColorWapper> JUSTHights = new List<ColorWapper>();
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color pixel = bmp.GetPixel(i, j);
                    ColorWapper wrap = new ColorWapper()
                    {
                        Color = pixel,
                    };
                    JUSTHights.Add(wrap);
                }
                PixalDoubleArray.Add(JUSTHights);
            }

            if (IsEncrypt)
            {
                PixalDoubleArray = EncryptImage(PixalDoubleArray, MoveCount);
            }
            else
            {
                PixalDoubleArray = DecryptImage(PixalDoubleArray, MoveCount);
            }
            


            for (int i = 0; i < PixalDoubleArray.Count; i++)
            {
                for (int j = 0; j < PixalDoubleArray[i].Count; j++)
                {
                    NewBmp.SetPixel(i, j, PixalDoubleArray[i][j].Color);
                }
            }

            using (var stream = new MemoryStream())
            {
                NewBmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public List<List<ColorWapper>> EncryptImage(List<List<ColorWapper>> PixalDoubleArray, int MoveCount)
        {
            for (int i = 0; i < PixalDoubleArray.Count; i++)
            {
                PixalDoubleArray[i] = ShuffleExtensions.Shuffle(PixalDoubleArray[i], MoveCount);
            }

            PixalDoubleArray = ShuffleExtensions.Shuffle(PixalDoubleArray, MoveCount);

            return PixalDoubleArray;
        }

        public List<List<ColorWapper>> DecryptImage(List<List<ColorWapper>> PixalDoubleArray, int MoveCount)
        {
            PixalDoubleArray = ShuffleExtensions.DeShuffle(PixalDoubleArray, MoveCount);

            for (int i = 0; i < PixalDoubleArray.Count; i++)
            {
                PixalDoubleArray[i] = ShuffleExtensions.DeShuffle(PixalDoubleArray[i], MoveCount);
            }

            return PixalDoubleArray;
        }
    }
}
