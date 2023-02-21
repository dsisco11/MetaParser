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
   
Finally; define all of your tokens and patterns inside the metaparser file you just made.
You will find an example parser definition file below.

## Parser Definition Format
// TODO: Documentation explaining schema

### Tokens
// TODO: Describe token types

## Example Parser File
```json
{
  "namespace": "ExampleParser",
  "tokens": {
    "char_open_bracket": {
      "$type": "constant",
      "value": "{"
    },
    "char_close_bracket": {
      "$type": "constant",
      "value": "}"
    },
    "char_asterisk": {
      "$type": "constant",
      "value": "*"
    },
    "char_solidus": {
      "$type": "constant",
      "value": "/"
    },
    "char_reverse_solidus": {
      "$type": "constant",
      "value": "\\"
    },
    "whitespace": {
      "$type": "compound",
      "consume": [ " ", "\t", "\f" ]
    },
    "digits": {
      "$type": "compound",
      "consume": { "range": [ "0", "9" ] }
    },
    "letters": {
      "$type": "compound",
      "consume": [
        { "range": [ "a", "z" ] },
        { "range": [ "A", "Z" ] }
      ]
    },
    "newline": {
      "$type": "compound",
      "consume": "\n"
    },
    "identifier": {
      "$type": "complex",
      "start": [ "letters" ],
      "consume": [ "letters", "digits" ]
    },
    "comment": {
      "$type": "complex",
      "start": [ "char_solidus", "char_asterisk" ],
      "end": [ "char_asterisk", "char_solidus" ],
      "escape": [ "char_reverse_solidus" ]
    },
    "codeblock": {
      "$type": "complex",
      "start": [ "char_open_bracket" ],
      "end": [ "char_close_bracket" ]
    }
  }
}
```
