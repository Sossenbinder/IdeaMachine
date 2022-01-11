openssl req -newkey rsa:2048 -x509 -nodes -keyout ../../AzuriteCert/azuriteKey.pem -new -out ../../AzuriteCert/azuriteCert.crt -sha256 -days 365 -addext "subjectAltName=IP:127.0.0.1,DNS:ideamachine.azurite,DNS:localhost" -subj "/C=CO/ST=ST/L=LO/O=OR/OU=OU/CN=CN"

Import-Certificate -FilePath ../../AzuriteCert/azuriteCert.crt -CertStoreLocation Cert:\LocalMachine\TrustedPublisher