# To-Text-API

Unofficial API for [ToText](http://www.to-text.net) in .NET

# Website Description
To-Text Converter is a solution, which allows you to convert images and PDFs containing written characters to text documents with no need for any software installation.<br /><br />
**Supported file formats: .tif, .jpg, .bmp, .png, .pdf**

# Usage

```csharp
var totext = new ToText.ToText();

// using a file in your pc (image or pdf file)
var text1 = totext.Convert(@"{filePath}", ToText.Languages.English);

// using Image object
var text2 = totext.Convert(image, ToText.Languages.English);

// using a byte array
var text3 = totext.Convert(byteArray, ToText.Languages.English);

```
# Dependencies
* [HtmlAgilityPack](https://www.nuget.org/packages/HtmlAgilityPack/)
