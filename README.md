# DCS TCP Export and Live Map

This project aims to create extensible and easy to configure TCP export for DCS World. 
Also included is a Live Map implemented in C# using ASP<span>.NET</span> Core that utilizes the TCP export to create an automatically updating map of the current mission on a website.

## Installation

### TCP Export

1. Install the latest version of [injector](https://github.com/Serious-Uglies/Injector).
2. Unsanitize the DCS Mission Scripting environment.
    * Comment out the following lines to the end of the file `<DCS-ROOT>\Scripts\MissionScripting.lua`.
    ```lua
    ...
    do
        -- These lines should not be commented out in your DCS installation. 
        -- Comment them out to make the TCP export work. If they are already
        -- commented out, no action is required.

        -- sanitizeModule('os')
        -- sanitizeModule('io')
        -- sanitizeModule('lfs')
        -- require = nil
        loadlib = nil
    end
    ```
3. Download the latest release from the releases section.
4. Drop the contents of the release ZIP archive into your DCS Saved Games folder. This folder should be found here: `C:\Users\<YOUR-NAME>\Saved Games\DCS` for the stable version or `C:\Users\<YOUR-NAME>\Saved Games\DCS.openbeta` for the Open Beta.
5. Launch DCS and start a mission. If everything worked, the DCS log file should contain something like the following lines:
    ```
    2021-06-02 22:31:29.914 INFO    INJECTOR: Loading module TcpExport
    2021-06-02 22:31:29.914 INFO    INJECTOR: Loading C:/Users/<NAME>/Saved Games/DCS.server/Scripts/Inject/TcpExport/hook.lua into hook environment
    2021-06-02 22:31:29.916 INFO    INJECTOR: File was loaded successfully
    2021-06-02 22:31:29.916 INFO    INJECTOR: Loading C:/Users/<NAME>/Saved Games/DCS.server/Scripts/Inject/TcpExport/init.lua into mission environment
    2021-06-02 22:31:29.926 INFO    SCRIPTING: [TcpExport] Starting up TCP export. Connecting and sending initial information
    ```

### Live Map

As mentioned above, the Live Map is an ASP<span>.NET</span> Core app. Microsoft recommends to use a reverse proxy to host such apps. On Windows the IIS Server is recommended. The following steps outline how to setup hosting the Live Map with IIS.

1. Install the latest .NET 6 ASP<span>.NET</span> Core Hosting Bundle (includes the runtime along with IIS support).
2. Make sure that the required Windows Features for IIS are activated.
3. Download and extract the latest release from the releases section.
4. Create an application in IIS-Manager and point it to the extracted archive.
5. Open the application. If everything worked a website should open displaying the Live Map.

## Configuration

### TCP Export

The TCP Export is configured through the file `config.lua` in the `Scripts/Inject/TcpExport` directory. This configuration is global and will be applied to any mission. To prevent updates of the application to overwrite any changes to the configuration a second file `config-overrides.lua` is provided. This file should be used to change any configuration values.

To configure the TCP Export for a single mission specifically, define a mission trigger `4 MISSION START` with a `DO SCRIPT` or `DO SCRIPT FILE` action. In this script a declare a global table `TcpExportConfig` which contains the changes to the configuration. Note that any subsequent changes to the table will not be applied.

The configuriation is applied as follows:
1. `config.lua`
2. `config-overrides.lua`
3. `TcpExportConfig` global table

E.g. to disable the export for the given mission:

```lua
TcpExportConfig = {
    enabled = false
}
```

Or change the update interval:

```lua
TcpExportConfig = {
    interval = 5
}
```

The TCP Export provides the following configuration options:

```lua
local config = {
    -- Determines whether the TCP Export is enabled.
    -- Default value: true
    enabled = true,

    -- Determines the address of the TCP endpoint to export the data to.
    -- Default value: "localhost"
    address = "localhost",

    -- Determines the port of the TCP endpoint to export the data to.
    -- Default value: 31090
    port = 31090,

    -- Determines the interval in seconds, after which updates to moving objects are exported.
    -- Default value: 1
    interval = 1,

    -- Defines the types of objects to export per coalition.
    -- Set any value to false to not export the given type of object
    -- Available object types:
    --  * airplane         Any fixed wing aircraft
    --  * helicopter       Any rotary wing aircraft
    --  * ground           Any ground unit
    --  * ship             Any water based unit
    --  * structure        Any structure like buildings, fortifications, static objects
    --  * airdromes        Any fortified airfield, e.g. Senaki Kolkhi
    --  * farp             Any helipad placed by the mission designer
    export = {

        -- Determines the types of objects to export for the blue coalition.
        -- Default values: all true
        blue = {
            airplane = true,
            helicopter = true,
            ground = true,
            ship = true,
            structure = true,
            airdrome = true,
            farp = true
        },

        -- Determines the types of objects to export for the red coalition.
        -- Default values: all true
        red = {
            airplane = true,
            helicopter = true,
            ground = true,
            ship = true,
            structure = true,
            airdrome = true,
            farp = true
        },

        -- Determines the types of objects to export for the neutral coalition.
        -- Default values: all false, except "airdrome"
        neutral = {
            airplane = false,
            helicopter = false,
            ground = false,
            ship = false,
            structure = false,
            airdrome = true,
            farp = false
        }
    }
}
```

### Live Map

The Live Map uses the default `appsettings.json` configuration known from .NET. The possible configuration options are outlined below:

```jsonc
{
  // The token used to authenticate against mapbox.com
  // Default value: pk.eyJ1IjoiZGFzY2xldmVybGUiLCJhIjoiY2tvMzRsZzNnMDZ2ajJwbzBva3l3am54dCJ9.zCFL46P4HkpX4AT7idTU2w
  "MapboxToken": "pk.eyJ1IjoiZGFzY2xldmVybGUiLCJhIjoiY2tvMzRsZzNnMDZ2ajJwbzBva3l3am54dCJ9.zCFL46P4HkpX4AT7idTU2w",

  // The mapbox style to use for the map
  // Default value: mapbox://styles/dascleverle/cko5q98k62fvv18lj5jln6inl
  "MapboxStyle": "mapbox://styles/dascleverle/cko5q98k62fvv18lj5jln6inl",

  // Configures the TCP export listener
  "ExportListener": {
    // The IP address to listen on
    // Default value: 127.0.0.1
    "Address": "127.0.0.1",

    // The port to listen on
    // Default value: 31090
    "Port": 31090
  },

  // Configures the logger of the app
  // See https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-5.0#configure-logging to find out how to setup logging.
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },

  "AllowedHosts": "*"
}
```

#### Localization

The Frontend App uses [react-i18next](https://react.i18next.com/) for localization. The localization resources are provided by a backend API endpoint. 
This endpoint sources its data from JSON files the `wwwroot/lang` directory. The base files for each supported locale are named in a format defined by RFC 3066 (e.g. `en-GB`, `de-DE`, `de`, `fr`). 
The JSON contained in these files is stuctured as follows:
```json
{
    "label": "A human readable label of the language, preferrably in the language the file describes.",
    "flag": "A lower case ISO 3166 2-letter country code which shall be used to display a flag icon representing the language.",
    "resources": {
        // An object containig the various resources defined in the master file (en.json) that shall be overriden with translated values.
    }
}
```
The file `en.json` serves as the master file where all used resource keys can be found. The application will automatically fall back to the base language without a second subtag if one with a subtag can not be found. When no base language file can be found, it will then fall back to `en`.

It is possible to overwrite specific resource keys without changing the actual language files. This feature is there to avoid loosing custom translations due to an update to the application. 
To overwrite resource keys, a JSON file named `<language-code>.overrides.json` must be created. It follows the same structure as defined above. All values - even the label or flag - can be overriden at will.

## Development

For development it is very convenient to setup a symbolic link for the TCP Export:

1. Clone the repository
2. In a administrative PowerShell prompt navigate to the repository directory
3. Invoke the following commands to create an symbolic link into your DCS Saved Games directory
    ```powershell
    # Stable 
    New-Item -Type Directory -Path "$env:USERPROFILE\Saved Games\DCS\Scripts\Inject"
    New-Item -Type SymbolicLink -Path "$env:USERPROFILE\Saved Games\DCS\Scripts\Inject\TcpExport" -Target ".\dcs\Scripts\Inject\TcpExport"

    # Open Beta
    New-Item -Type Directory -Path "$env:USERPROFILE\Saved Games\DCS.openbeta\Scripts\Inject"
    New-Item -Type SymbolicLink -Path "$env:USERPROFILE\Saved Games\DCS.openbeta\Scripts\Inject\TcpExport" -Target ".\dcs\Scripts\Inject\TcpExport"
    ``` 
4. Now you can edit the files in the git repository and the changes will be mirrored to your DCS Saved Games directory

TIP: It is not required to restart DCS after changes to the lua files because the files are loaded at mission init. Only restart of the mission is required.

To make changes to the Live Map, you need to setup a .NET 6 development environment. This can either be achieved by installing [Visual Studio](https://visualstudio.microsoft.com/) or by installing the latest [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) manually. Since the Frontend is a React app you also need to install [Node.js](https://nodejs.org/en/) (LTS is fine) to be able to launch the development server and create builds. After that, the project can be launched via Visual Studio or over the command line with `dotnet run`.
