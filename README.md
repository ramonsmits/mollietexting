mollietexting
=============
MollieTexting library makes it possible to use the Mollie.nl http api more easily
from a .net client. It makes use of .net configuration to configure the username,
password and gateway so that these do not have to be stored in code but in the
app.config/web.config of the application. It also only communicates the MD5 password
hash and only allows storing the hash in the configuration file instead of the plain
text password.

1. Makes of .net configuration
2. Has interfaces usefull for dependancy injection
3. Only allows md5 password hash in configuration
