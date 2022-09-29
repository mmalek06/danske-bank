# Things to note:

1. In a production app, the DB password would be injected into the application through some kind of 
secret management tool (in Azure Devops that would be the Variables subpage). Here I decided not 
to overcomplicate things and just store the password in plain text.

2. EF migrations are run on app start which may or may not be the desired behavior for a production app.

3. I didn't write tests for all the important parts, but for most of them. I went with the suggestion of
   writing "some" tests :). For example I wrote two to check if the GET endpoint is behaving correctly just to 
   demonstrate testing of a controller, but I didn't include any for the POST endpoint to save time.

# Setting up the db in Docker

As a convenience I provided a small compose file to set up SqlServer in Docker. In order to run it, 
cd to the devops folder at the repository main level, open the terminal and type in the below:

```
	docker compose -f docker-compose.sqlserver.yml up -d
```

The ""sqlserver"" in the file name is there because of the namespacing. Normally I set up my projects
so that they can be run entirely locally, which means spinning up many, many infrastructural pieces.
I put them all into their own separate docker-compose files and put into a devops folder, like the one
included.

# Connecting to the DB

Host and port: localhost,2433<br />
Login: SA<br />
Password: DanskeBank@2022<br />
