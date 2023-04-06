# Members1stDemo

This is a demonstration of a simple REST API that implements several different GET queries against data imported into an 
Entity Framework Core in-memory database.  The source data is contained in the transactions.json file, and the user interface
is provided by Swagger.

## Steps to run:

 1. From your favorite shell, execute:
    `git clone https://github.com/ggariepy/Members1stDemo.git`
 1. Open the Members1stRest solution using Visual Studio 2022
 1. Run the solution in debug mode using the https configuration
 
You should see a Swagger UI appear on your Windows machine's default browser.  Test the various endpoints to see what they do.

Alternatively, you can run this API directly from the URL field in your browser.  

Try entering:
`https://localhost:7294/transaction/{id}?transId=1` 

Finally, if you wish to use curl or some other command-line approach to issue GET requests, it's as simple as:


`curl -X 'GET' \
  'https://localhost:7294/transaction/{id}?transId=1' \
  -H 'accept: */*'`
