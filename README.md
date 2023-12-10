appsettings.json должен быть представлен в следующем формате для запуска API

{
"Jwt": {
"Key": "your_secret_key_here",
"Issuer": "your_issuer",
"Audience": "your_audience"
},
"MongoDbSettings": {
"ConnectionString": "connection_string_to_mongo",
"DatabaseName": "database_name"
},
"EmailSettings": {
"MailServer": "smtp.example.com",
"MailPort": 587,
"SenderName": "your-email@example.com",
"Sender": "your-email@example.com",
"Password": "your-password"
},
"Logging": {
"LogLevel": {
"Default": "Information",
"Microsoft.AspNetCore": "Warning"
}
},
"AllowedHosts": "\*"
}
