# UrlShortenerMinimalAPI
Url shortener service using .Net core minimal API.
minimal api suits best for solutions with simpler logic
Controllers are not used in this architecture as the http actions are performed in program.cs
The encoder works using Base62 conversion logic.
The counter logic can be improved by limiting a server to use a particular range.
Multiple servers can be used to leverage the microservices architecture in a distributed environment
