# MetaParser

## Overview
MetaParser is a C# source code generator which produces a text parser/tokenizer based on a definition file.   
The definition file is a simple json file with the extension '.metaparser.json' which adheres to the metaparser schema.
Schemas can be found under the "Schemas" folder at this repositories root.

## Usage
Get started by installing the nuget package 'MetaParser'.   
Then; in your project create a new file with the extension `metaparser.json` eg; `CssParser.metaparser.json`  
Set the newly created files type to an "analyzer additional file" in your project settings 
or in visual studio select "C# analyzer additional file" from the "Build Action" dropdown list within the file properties.
   
Finally; define all of your tokens inside the file you just made.
An example definition file can be found at the bottom of this readme.

### Output
Each definition file will result in the generation of a new `Parser` class under the namespace specified in the parser definition file.
The generated parser class contains a method named `Parse(ReadOnlyMemory input)` which outputs a list of `Token` objects.
Below is a diagram illustrating the generated parser class structure.
```mermaid
---
title: Parsing Process
---
classDiagram
Parser --> Token : Parse("hello world")
Token --* ValueToken : ["hello"]
Token --* ValueToken : [" "]
Token --* ValueToken : ["world"]

class Parser{
   Token[] Parse(ReadOnlyMemory)
}
class Token{
   ETokenType Id
   ValueToken[] Items
}
class ValueToken{
   ETokenType Id
   ReadOnlyMemory Data
}
```


## Definition Files
A MetaParser definition file is a simple JSON object structure with only a handful of primary fields;
- $schema: _which version of the metaparser schema the file adheres to_
- namespace: _namespace which the generated parsing code will reside in_
- definitions: _collection of token pattern definitions for the parser to detect and consume_

### Tokens
The _definitions_ field is an JSON object, and each named property within it is a token whose name corrosponds to the property name.
There are 3 types of tokens.

// TODO: Finish explaining token types

## Example Parser File
```json
{
  "$schema": "https://raw.githubusercontent.com/dsisco11/MetaParser/master/Schemas/schema-01.json",
  "namespace": "ExampleParser",
  "definitions": {
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
