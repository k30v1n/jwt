# JWTs
Json Web Tokens - Examples about custom JWT signatures using manual signatures with asymetric generated RSA keys. The generated JWT can be validated/checked on https://jwt.io

## Projects
- Jwt-ConsoleApp: Here is shown how to manually generate a JWT token and validate it with Private and Public key
- Jwt-WebAPplication: Here is shown how to configure the JWT validation on APIs using the Authorization header with a Public key

## Creating RSA SHA256 signing keys (testing)

1. Create a RSA key
   - `openssl genrsa -out rsakey.key 2048`

1. Create a x509 certificate and a public key
   `openssl req -new -x509 -sha256 -key rsakey.key -out public-certificate.cer -days 10000`

1. Export the public and private key to a encripted pfx certificate
   `openssl pkcs12 -export -out private-certificate.pfx -inkey rsakey.key -in public-certificate.cer`