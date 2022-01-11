openssl req -newkey rsa:2048 -x509 -nodes -keyout cert/nginx_key.pem -new -out cert/nginx_cert.crt -sha256 -days 365 -addext "subjectAltName=IP:127.0.0.1,DNS:localhost" -subj "/C=CO/ST=ST/L=LO/O=OR/OU=OU/CN=CN"

Import-Certificate -FilePath nginx_cert.crt -CertStoreLocation Cert:\LocalMachine\TrustedPublisher