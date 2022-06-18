# UrlShortener

Frontend located at https://github.com/datur/UrlShortenerUi

C# api using in memory database

## Requirements
An endpoint that will shorten a URL
- [x] Validate url 
- [x] Check if shortened url already exists
- [x] Hash the url in some way
- [x] store the url
- [x] Return the shortened url

An endpoint that retreives the URL from the shortened url
- [x] url must be valid
- [x] try and find the urls hash
- [x] redirect to the url if found - Handled in frontend
