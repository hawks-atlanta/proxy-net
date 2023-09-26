# proxy-net

[![Coverage](https://github.com/hawks-atlanta/proxy-net/actions/workflows/coverage.yaml/badge.svg)](https://github.com/hawks-atlanta/proxy-net/actions/workflows/coverage.yaml) [![Release](https://github.com/hawks-atlanta/proxy-net/actions/workflows/release.yaml/badge.svg)](https://github.com/hawks-atlanta/proxy-net/actions/workflows/release.yaml) [![Tagging](https://github.com/hawks-atlanta/proxy-net/actions/workflows/tagging.yaml/badge.svg)](https://github.com/hawks-atlanta/proxy-net/actions/workflows/tagging.yaml) [![Test](https://github.com/hawks-atlanta/proxy-net/actions/workflows/testing.yaml/badge.svg)](https://github.com/hawks-atlanta/proxy-net/actions/workflows/testing.yaml) 

## Description

Proxy service intended to forward traffic between clients and `gateway-java` so they don't need to suffer `SOAP`

## Features

- [ ] LoginController

- [ ] RegisterController

- [ ] ChallengeController

  Each API functionality could be access as listed in the table below:

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
      <td>/challenge</td>
      <td class='text-align:center'>No</td>
      <td>Validate Token User (Bearer Authentication)</td>
      <td>Validate token/user and return a JWT.</td>
    </tr>
  </tbody>
</table>

## Running the API

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
