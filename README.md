# proxy-net

[![Coverage](https://github.com/hawks-atlanta/proxy-net/actions/workflows/coverage.yaml/badge.svg)](https://github.com/hawks-atlanta/proxy-net/actions/workflows/coverage.yaml) [![Release](https://github.com/hawks-atlanta/proxy-net/actions/workflows/release.yaml/badge.svg)](https://github.com/hawks-atlanta/proxy-net/actions/workflows/release.yaml) [![Tagging](https://github.com/hawks-atlanta/proxy-net/actions/workflows/tagging.yaml/badge.svg)](https://github.com/hawks-atlanta/proxy-net/actions/workflows/tagging.yaml) [![Test](https://github.com/hawks-atlanta/proxy-net/actions/workflows/testing.yaml/badge.svg)](https://github.com/hawks-atlanta/proxy-net/actions/workflows/testing.yaml) 

## Description

Proxy service intended to forward traffic between clients and `gateway-java` so they don't need to suffer `SOAP`

## Features

_Coming Soon added OpenAPI update_ #14 & #20

- [ ] Each API functionality could be access as listed in the table below:

<table>
  <tbody>
    <tr>
      <th>Verb</th>
      <th>URI</th>
      <th>Auth Needed?</th>
      <th>Method</th>
      <th>Description</th>
    </tr>
    <tr>
      <td>POST</td>
      <td>/login</td>
      <td class='text-align:center'>No</td>
      <td>Authenticate/Login User</td>
      <td>Authenticate an user and return a JWT.</td>
    </tr>
    <tr>
      <td>POST</td>
      <td>/register</td>
      <td class='text-align:center'>No</td>
      <td>Register user</td>
      <td>Register user and return a JWT.</td>
    </tr>    
    <tr>
      <td>POST</td>
      <td>/refreshtoken</td>
      <td class='text-align:center'>No</td>
      <td>Validate Token User (send "token": token)</td>
      <td>Validate token/user and return a JWT.</td>
    </tr>
  </tbody>
</table>


## Step for running the PROXY-NET

Clone the repo:

``````bash
git clone https://github.com/hawks-atlanta/proxy-net.git
``````

Enter the new folder:

``````
cd /proxy-net
``````

You can setup the necessary services by running:

``````bash
docker compose up -d
``````

Services port & route:

`http://localhost:8084/ROUTES`

Note: In the [Docker-Compose FILE](https://github.com/hawks-atlanta/proxy-net/blob/main/docker-compose.yaml) edit the service you want to test with, for development use the same service that the gateway container provides and for production the URL of your ONLINE service.

### **Example:**

In Docker-Compose:

``````yaml
  # Proxy-net
  # Local/Docker http://gateway:8080/service (Gateway Container)
  # Remote       http://URL-YOUR-SOAP-PR/service
....
    environment:
      - SERVICE_URL=http://gateway:8080/service 
``````

`Note: It is not necessary to add ?WSDL | connect to Localhost with using name "gateway" container`

With this config of Docker-Compose is auto impl in DockerFile:
``````yaml
...more lines
ARG ASPNETCORE_URLS
ARG SERVICE_URL
ENV ASPNETCORE_URLS=$ASPNETCORE_URLS
ENV SERVICE_URL=$SERVICE_URL
...more lines
``````

## For Development

---

#### Opening the project

1. Double click on the solution file. It will open the solution in your current version of the Visual Studio 2022.
1. (Click in button Docker)
1. Install Nuget Packages


### Restoring project's dependencies

1. In the Visual Studio, open the Package Manager Nuget and run the command to restore all packages used by the API. You can also click-right on the solution's name in the Solution Explorer and select "Restore NuGet Packages.

```sh
dotnet restore
```

### Building & Running the API

1. To build the project you have two options: The first one is click-right on the solution's name and then select "Build Solution". The second option is by running the command below through the Package Manager Console terminal;

```sh
dotnet build
```

2. To Run the API, there are also two options: Pressing F5 or executing the command through the terminal. 

```sh
dotnet run
```

## ⚠️WARNING! for updates in [Gateway-SOAP](https://github.com/hawks-atlanta/gateway-java):

To add the service either localhost or remote from the SOAP GATEWAY, the ENV `SERVICE_URL` reading is created, in the file [References.cs](https://github.com/hawks-atlanta/proxy-net/blob/main/ServiceReference/Reference.cs) implement this code:

``````c#
private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
{
    if ((endpointConfiguration == EndpointConfiguration.ServiceImpPort))
    {
        string serviceUrl = Environment.GetEnvironmentVariable("SERVICE_URL");
        if (string.IsNullOrEmpty(serviceUrl))
        {
            throw new ApplicationException("SERVICE_URL env not found!");
        }
        return new System.ServiceModel.EndpointAddress(serviceUrl);
    }
    throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
}
``````

🚨**NOTE!: **When you want to make a new update to the interface provided in the References file you need to add this line of code, be careful not to touch other lines of code!

