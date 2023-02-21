# MetaParser

## Description
MetaParser is a C# source code generator which produces a text parser/tokenizer based on a definition file.   
The definition file is a simple json file with the extension '.metaparser.json' which adheres to the metaparser schema.
Schemas can be found under the "Schemas" folder at this repositories root.

## Usage
Get started by installing the nuget package 'MetaParser'.   
Then; in your project create a new file with the extension `metaparser.json` eg; `CssParser.metaparser.json`  
Set the newly created files type to an "analyzer additional file" in your project settings 
or in visual studio select "C# analyzer additional file" from the "Build Action" dropdown list within the files properties.

## Tokens
