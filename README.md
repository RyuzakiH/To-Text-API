To-Text-API
[![AppVeyor](https://img.shields.io/appveyor/ci/RyuzakiH/To-Text-API/master.svg?maxAge=60)](https://ci.appveyor.com/project/RyuzakiH/To-Text-API)
[![NuGet](https://img.shields.io/nuget/v/TempMail.API.svg?maxAge=60)](https://www.nuget.org/packages/ToText.API)
=============

Unofficial API for [ToText](http://www.to-text.net) in .NET

**NuGet**: https://www.nuget.org/packages/ToText.API

# Website Description
To-Text Converter is a solution, which allows you to convert images and PDFs containing written characters to text documents with no need for any software installation.<br /><br />
**Supported file formats: .tif, .jpg, .bmp, .png, .pdf**

# Usage

```csharp
var client = new ToTextClient();

// using a file in your pc (image or pdf file)
var text1 = client.Convert($"{filePath}", Languages.English);

// using Image object
var text2 = client.Convert(image, Languages.English);

// using a byte array
var text3 = client.Convert(byteArray, Languages.English);

```

Synchronous Example

```csharp
var client = new ToTextClient();

var result = client.Convert(@"..\..\..\assets\sample0.png", Languages.English);
```

Asynchronous Example

```csharp
var client = new ToTextClient();

var result = await client.ConvertAsync(@"..\..\..\assets\sample1.png", Languages.English);
```

Full Test Example [Here](https://github.com/RyuzakiH/To-Text-API/blob/master/src/ToText.Example/Program.cs)

# Supported Platforms
[.NET Standard 2.0](https://github.com/dotnet/standard/blob/master/docs/versions.md)

# Dependencies
* [HtmlAgilityPack](https://www.nuget.org/packages/HtmlAgilityPack/)
* [System.Drawing.Common](https://www.nuget.org/packages/System.Drawing.Common)
