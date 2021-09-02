using System;
using System.Collections.Generic;
using JWT;
using JWT.Builder;
using JWT.Algorithms;
using JWT.Serializers;

namespace GFDF.Infrastruct.Core
{
    public class JWTHelper
    {
        private string secret = "eb2298f46b93fbda7c78c904b8cd1337"; 

        public JWTHelper(string strSecret)
        {
            secret = strSecret;
        }

        /// <summary>
        /// 生成JWT
        /// </summary>
        /// <param name="data">附加对象</param>
        /// <param name="timeout">过期时长(s)</param>
        /// <returns></returns>
        public string Encode(object data,string aud="USER",string sub="", double timeout=7200,long nbf=0,long jti=0)
        {
            var payload = new Dictionary<string, object> {
                {"iss","6b93fbda7c78c904" },
                {"exp",DateTimeOffset.UtcNow.AddSeconds(timeout).ToUnixTimeSeconds() },
                {"sub",sub },
                {"aud",aud },
                {"nbf",nbf },
                {"iat",DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
                {"jti",jti },
                {"data",data }
            };
             return new JwtBuilder().WithAlgorithm(new HMACSHA256Algorithm()).WithSecret(secret)
                .AddClaims(payload).Encode();
        }

        public dynamic Decode(string token)
        {
           return new JwtBuilder().WithAlgorithm(new HMACSHA256Algorithm())
           .WithSecret(secret)
           .WithSerializer(new JsonNetSerializer())
           .WithUrlEncoder(new JwtBase64UrlEncoder())
           .WithValidator(new JwtValidator(new JsonNetSerializer(), new UtcDateTimeProvider()))
           .MustVerifySignature()
           .Decode<Dictionary<string, object>>(token);
        }
    }
}
