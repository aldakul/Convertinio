# HtmlConverter

Implementation of a web service. 
The service accepts an HTML file from a web client, convert it to PDF using Puppeteer Sharp, and return it to the client.

1. Can work wih large html files
2. Web server is **stable**. To reach this, used preload file to the server while converting from html to pdf. When work is done, the html file is deleted from server.
   - This way we don't have to worry about restarting the server. Our files in safe.
3. The web service is scalable. This service may work not only for converting html's to pdf's, but anything to anything. Uses abstract implementation of DB, classes.
   - Written by following Clean Architecture principles.

## The client is contained in the index.html file
After selecting a file, POST /convert is performed
Then, every 500ms, a status request is made GET / status background of the task
After receiving a successful status, the file is downloaded GET /download

## stack
.net 6
asp.net webapi
ef memcache
PuppeterSharp
Hangfire
