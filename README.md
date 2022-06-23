- **Make sure to not push API key or accounts will be blocked**

# Deploy to Heroku with Docker

Runt the following commands in the terminal with **adnim rights** in the root folder

- **DOCKER**
- ```docker build -t [projectname] .```
- ```docker run -d -p 5000:80 --name myapp [projectname]```
- ```heroku container:login```
- ```docker build -t registry.heroku.com/[heroku-app-mane]/web .```
- ```docker push registry.heroku.com/[heroku-app-mane]/web```
- ```heroku container:release web --app [heroku-app-mane]```

I had to use this resourses to fix my environment:
- **Heroku only supports Linux Docker type!**
- error : No such host is known [C:\app\dotnetapp.sln] (**Docker for Windows uses the network adapter with the lowest Id**. When this adapter is deactivated or if your addapter for example (wi-fi) does not have the lowest id, your container cannot connect to the internet.)  https://improveandrepeat.com/2019/09/how-to-fix-network-errors-with-docker-and-windows-containers/
- Unexpected HTTP status: 500 internal server error https://stackoverflow.com/questions/54987383/docker-pushing-to-heroku-unexpected-http-status-500-internal-server-error
