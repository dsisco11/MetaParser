# MetaParser v2 - JSON Schema Specification

> **Document Created:** December 30, 2025  
> **Schema Version:** 2.0  
> **Status:** Draft

---

## Table of Contents

1. [Overview](#overview)
2. [Root Properties](#root-properties)
3. [Token Definitions](#token-definitions)
4. [Pattern Types](#pattern-types)
5. [Token References](#token-references)
6. [Trivia Tokens](#trivia-tokens)
7. [Validation Rules](#validation-rules)
8. [Complete Examples](#complete-examples)
9. [JSON Schema Definition](#json-schema-definition)

---

## Overview

The MetaParser v2 schema provides a simplified, declarative way to define text parsers. Users define tokens and their patterns; the generator automatically determines parsing stages based on dependency analysis.

### Design Principles

1. **Simplicity** - No manual stage assignment required
2. **Clarity** - Explicit distinction between literal text and token references
3. **Flexibility** - Support for characters, ranges, sequences, and token composition
4. **Automatic Optimization** - Generator determines optimal consumer levels
5. **Deterministic Priority** - Token definition order determines consumption priority

### File Convention

- **Extension:** `.metaparser.json`
- **Build Action:** C# analyzer additional file

### Token Priority

When multiple tokens could match at the same position, **definition order determines priority**. Tokens defined earlier in the JSON file have higher priority and are attempted first.

Each token receives a virtual priority value equal to its index (0-based) in the `tokens` object:

```json
{
  "tokens": {
    "keyword_if": { "consume": "if" },
    "identifier": { "consume": { "$token": "letter" } }
  }
}
```

| Token | Priority | Notes |
|-------|----------|-------|
| `keyword_if` | 0 | Tried first - matches "if" exactly |
| `identifier` | 1 | Tried second - would also match "if" as letters |

**Best Practice:** Define more specific tokens (keywords, operators) before general tokens (identifiers, numbers).

```json
{
  "tokens": {
    "keyword_if": { "consume": "if" },
    "keyword_else": { "consume": "else" },
    "keyword_while": { "consume": "while" },
    "identifier": { "consume": { "$token": "identifier_char" } }
  }
}
```

This ensures `if` is recognized as `keyword_if` rather than as an `identifier`.

---

## Root Properties

```json
{
  "$schema": "https://raw.githubusercontent.com/dsisco11/MetaParser/v2/Schemas/schema-v2.json",
  "namespace": "MyParser",
  "classname": "Parser",
  "tokens": { ... },
  "trivia": [ ... ]
}
```

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `$schema` | string | No | Schema URL for IDE validation |
| `namespace` | string | **Yes** | Namespace for generated code |
| `classname` | string | No | Parser class name (default: `"Parser"`) |
| `tokens` | object | **Yes** | Token definitions (see below) |
| `trivia` | array | No | Token names to treat as trivia |

---

## Token Definitions

Tokens are defined as named properties within the `tokens` object. Each token specifies patterns for matching input.

### Basic Structure

```json
{
  "tokens": {
    "token_name": {
      "start": <pattern>,
      "consume": <pattern>,
      "stop": <pattern>,
      "escape": <pattern>
    }
  }
}
```

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `start` | pattern | No | Pattern that must match to begin consuming |
| `consume` | pattern | **Yes** | Pattern(s) to consume repeatedly |
| `stop` | pattern | No | Pattern that ends consumption (not consumed) |
| `escape` | pattern | No | Pattern that escapes the next character/token |

### Consumption Behavior

1. If `start` is defined, it must match first (consumed)
2. `consume` patterns are matched repeatedly
3. If `stop` is defined, consumption ends when it matches (not consumed)
4. If `escape` matches, the next item is consumed literally

---

## Pattern Types

Patterns define what characters or tokens to match. There are several pattern types:

### 1. Literal String

Matches an exact sequence of characters.

```json
{
  "keyword_if": {
    "consume": "if"
  },
  "operator_plus": {
    "consume": "+"
  }
}
```

### 2. Literal Array

Matches any one of the specified strings.

```json
{
  "whitespace": {
    "consume": [" ", "\t", "\r", "\n"]
  },
  "operator": {
    "consume": ["+", "-", "*", "/"]
  }
}
```

### 3. Character Range

Matches any character within an inclusive range.

```json
{
  "digit": {
    "consume": { "range": ["0", "9"] }
  },
  "lowercase": {
    "consume": { "range": ["a", "z"] }
  }
}
```

### 4. Multiple Ranges

Combine multiple ranges in an array.

```json
{
  "letter": {
    "consume": [
      { "range": ["a", "z"] },
      { "range": ["A", "Z"] }
    ]
  },
  "alphanumeric": {
    "consume": [
      { "range": ["a", "z"] },
      { "range": ["A", "Z"] },
      { "range": ["0", "9"] }
    ]
  }
}
```

### 5. Mixed Patterns

Combine literals and ranges.

```json
{
  "identifier_char": {
    "consume": [
      { "range": ["a", "z"] },
      { "range": ["A", "Z"] },
      { "range": ["0", "9"] },
      "_"
    ]
  }
}
```

---

## Token References

To consume other tokens (rather than raw characters), use the `$token` syntax. This is how higher-level tokens are composed from lower-level ones.

### Syntax

```json
{ "$token": "token_name" }
```

### Single Token Reference

```json
{
  "digit": {
    "consume": { "range": ["0", "9"] }
  },
  "number": {
    "consume": { "$token": "digit" }
  }
}
```

In this example:
- `digit` consumes raw characters (Level 0)
- `number` consumes `digit` tokens (Level 1)

### Multiple Token References

```json
{
  "number": {
    "consume": { "$token": "digit" }
  },
  "identifier": {
    "consume": { "$token": "identifier_char" }
  },
  "value": {
    "consume": [
      { "$token": "number" },
      { "$token": "identifier" }
    ]
  }
}
```

### Literal vs Token Reference

| Pattern | Meaning |
|---------|---------|
| `"digit"` | Literal string "digit" |
| `{ "$token": "digit" }` | Reference to token named `digit` |

```json
{
  "tokens": {
    "digit": {
      "consume": { "range": ["0", "9"] }
    },
    "number": {
      "consume": { "$token": "digit" }
    },
    "word_digit": {
      "consume": "digit"
    }
  }
}
```

- `number` → Consumes tokens of type `digit` → **Level 1**
- `word_digit` → Consumes literal text "digit" → **Level 0**

---

## Trivia Tokens

Trivia tokens represent non-semantic content (whitespace, comments). They are attached to regular tokens as leading or trailing trivia.

### Declaration

```json
{
  "tokens": {
    "whitespace": {
      "consume": [" ", "\t"]
    },
    "newline": {
      "consume": ["\r\n", "\n"]
    },
    "line_comment": {
      "start": "//",
      "consume": { "range": [" ", "~"] },
      "stop": "\n"
    }
  },
  "trivia": ["whitespace", "newline", "line_comment"]
}
```

### Trivia Attachment Rules

1. **Leading-preferred:** Trivia attaches as leading to the next token
2. **Trailing fallback:** At EOF or end-of-block, trivia attaches as trailing

---

## Validation Rules

The generator enforces these validation rules:

### 1. Unique Token Names

All token names must be unique within the schema.

```json
// ERROR: Duplicate token name
{
  "tokens": {
    "digit": { "consume": { "range": ["0", "9"] } },
    "digit": { "consume": "0" }
  }
}
```

### 2. Valid Token References

All `$token` references must refer to defined tokens.

```json
// ERROR: 'letter' is not defined
{
  "tokens": {
    "identifier": {
      "consume": { "$token": "letter" }
    }
  }
}
```

### 3. No Circular Dependencies

Token dependencies must not form cycles.

```json
// ERROR: Circular dependency: a → b → a
{
  "tokens": {
    "a": { "consume": { "$token": "b" } },
    "b": { "consume": { "$token": "a" } }
  }
}
```

### 4. Valid Character Ranges

Range start must be less than or equal to range end.

```json
// ERROR: Invalid range
{
  "tokens": {
    "invalid": {
      "consume": { "range": ["z", "a"] }
    }
  }
}
```

### 5. Non-Empty Consume

Every token must have a `consume` pattern.

```json
// ERROR: Missing consume
{
  "tokens": {
    "empty": {
      "start": "["
    }
  }
}
```

### 6. Valid Trivia References

All items in `trivia` array must reference defined tokens.

```json
// ERROR: 'comment' is not defined
{
  "tokens": {
    "whitespace": { "consume": " " }
  },
  "trivia": ["whitespace", "comment"]
}
```

---

## Complete Examples

### Example 1: Simple Calculator Tokens

```json
{
  "$schema": "https://raw.githubusercontent.com/dsisco11/MetaParser/v2/Schemas/schema-v2.json",
  "namespace": "Calculator",
  "tokens": {
    "whitespace": {
      "consume": [" ", "\t"]
    },
    "newline": {
      "consume": ["\r\n", "\n"]
    },
    "digit": {
      "consume": { "range": ["0", "9"] }
    },
    "number": {
      "consume": { "$token": "digit" }
    },
    "plus": {
      "consume": "+"
    },
    "minus": {
      "consume": "-"
    },
    "multiply": {
      "consume": "*"
    },
    "divide": {
      "consume": "/"
    },
    "lparen": {
      "consume": "("
    },
    "rparen": {
      "consume": ")"
    }
  },
  "trivia": ["whitespace", "newline"]
}
```

**Dependency Analysis:**
```
Level 0: whitespace, newline, digit, plus, minus, multiply, divide, lparen, rparen
Level 1: number (depends on digit)
```

### Example 2: Programming Language Basics

```json
{
  "$schema": "https://raw.githubusercontent.com/dsisco11/MetaParser/v2/Schemas/schema-v2.json",
  "namespace": "MiniLang",
  "classname": "Lexer",
  "tokens": {
    "whitespace": {
      "consume": [" ", "\t"]
    },
    "newline": {
      "consume": ["\r\n", "\n"]
    },
    "line_comment": {
      "start": "//",
      "consume": [
        { "range": [" ", "~"] },
        "\t"
      ],
      "stop": "\n"
    },
    "digit": {
      "consume": { "range": ["0", "9"] }
    },
    "letter": {
      "consume": [
        { "range": ["a", "z"] },
        { "range": ["A", "Z"] }
      ]
    },
    "identifier_start": {
      "consume": [
        { "range": ["a", "z"] },
        { "range": ["A", "Z"] },
        "_"
      ]
    },
    "identifier_continue": {
      "consume": [
        { "range": ["a", "z"] },
        { "range": ["A", "Z"] },
        { "range": ["0", "9"] },
        "_"
      ]
    },
    "identifier": {
      "start": { "$token": "identifier_start" },
      "consume": { "$token": "identifier_continue" }
    },
    "integer": {
      "consume": { "$token": "digit" }
    },
    "string_literal": {
      "start": "\"",
      "consume": [
        { "range": [" ", "!"] },
        { "range": ["#", "~"] }
      ],
      "stop": "\"",
      "escape": "\\"
    },
    "keyword_if": { "consume": "if" },
    "keyword_else": { "consume": "else" },
    "keyword_while": { "consume": "while" },
    "keyword_return": { "consume": "return" },
    "op_assign": { "consume": "=" },
    "op_eq": { "consume": "==" },
    "op_ne": { "consume": "!=" },
    "op_lt": { "consume": "<" },
    "op_gt": { "consume": ">" },
    "op_le": { "consume": "<=" },
    "op_ge": { "consume": ">=" },
    "semicolon": { "consume": ";" },
    "lbrace": { "consume": "{" },
    "rbrace": { "consume": "}" },
    "lparen": { "consume": "(" },
    "rparen": { "consume": ")" }
  },
  "trivia": ["whitespace", "newline", "line_comment"]
}
```

**Dependency Analysis:**
```
Level 0: whitespace, newline, line_comment, digit, letter, identifier_start,
         identifier_continue, string_literal, all keywords, all operators,
         semicolon, lbrace, rbrace, lparen, rparen

Level 1: identifier (depends on identifier_start, identifier_continue)
         integer (depends on digit)
```

### Example 3: JSON Tokens

```json
{
  "$schema": "https://raw.githubusercontent.com/dsisco11/MetaParser/v2/Schemas/schema-v2.json",
  "namespace": "JsonParser",
  "tokens": {
    "whitespace": {
      "consume": [" ", "\t", "\r", "\n"]
    },
    "digit": {
      "consume": { "range": ["0", "9"] }
    },
    "hex_digit": {
      "consume": [
        { "range": ["0", "9"] },
        { "range": ["a", "f"] },
        { "range": ["A", "F"] }
      ]
    },
    "integer": {
      "consume": { "$token": "digit" }
    },
    "string": {
      "start": "\"",
      "consume": [
        { "range": [" ", "!"] },
        { "range": ["#", "["] },
        { "range": ["]", "~"] }
      ],
      "stop": "\"",
      "escape": "\\"
    },
    "true": { "consume": "true" },
    "false": { "consume": "false" },
    "null": { "consume": "null" },
    "colon": { "consume": ":" },
    "comma": { "consume": "," },
    "lbracket": { "consume": "[" },
    "rbracket": { "consume": "]" },
    "lbrace": { "consume": "{" },
    "rbrace": { "consume": "}" }
  },
  "trivia": ["whitespace"]
}
```

---

## JSON Schema Definition

The formal JSON Schema for MetaParser v2 definition files:

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "$id": "https://raw.githubusercontent.com/dsisco11/MetaParser/v2/Schemas/schema-v2.json",
  "title": "MetaParser v2 Definition",
  "description": "Schema for MetaParser v2 token definitions",
  "type": "object",
  "required": ["namespace", "tokens"],
  "properties": {
    "$schema": {
      "type": "string",
      "description": "Schema URL for validation"
    },
    "namespace": {
      "type": "string",
      "description": "Namespace for generated code",
      "pattern": "^[A-Za-z_][A-Za-z0-9_.]*$"
    },
    "classname": {
      "type": "string",
      "description": "Parser class name",
      "default": "Parser",
      "pattern": "^[A-Za-z_][A-Za-z0-9_]*$"
    },
    "tokens": {
      "type": "object",
      "description": "Token definitions",
      "additionalProperties": {
        "$ref": "#/$defs/tokenDefinition"
      },
      "propertyNames": {
        "pattern": "^[a-z_][a-z0-9_]*$"
      }
    },
    "trivia": {
      "type": "array",
      "description": "Token names to treat as trivia",
      "items": {
        "type": "string"
      }
    }
  },
  "$defs": {
    "tokenDefinition": {
      "type": "object",
      "required": ["consume"],
      "properties": {
        "start": { "$ref": "#/$defs/pattern" },
        "consume": { "$ref": "#/$defs/pattern" },
        "stop": { "$ref": "#/$defs/pattern" },
        "escape": { "$ref": "#/$defs/pattern" }
      },
      "additionalProperties": false
    },
    "pattern": {
      "oneOf": [
        { "$ref": "#/$defs/literalString" },
        { "$ref": "#/$defs/literalArray" },
        { "$ref": "#/$defs/characterRange" },
        { "$ref": "#/$defs/tokenReference" }
      ]
    },
    "literalString": {
      "type": "string",
      "minLength": 1
    },
    "literalArray": {
      "type": "array",
      "items": {
        "oneOf": [
          { "type": "string", "minLength": 1 },
          { "$ref": "#/$defs/characterRange" },
          { "$ref": "#/$defs/tokenReference" }
        ]
      },
      "minItems": 1
    },
    "characterRange": {
      "type": "object",
      "required": ["range"],
      "properties": {
        "range": {
          "type": "array",
          "items": { "type": "string", "minLength": 1, "maxLength": 1 },
          "minItems": 2,
          "maxItems": 2
        }
      },
      "additionalProperties": false
    },
    "tokenReference": {
      "type": "object",
      "required": ["$token"],
      "properties": {
        "$token": {
          "type": "string",
          "pattern": "^[a-z_][a-z0-9_]*$"
        }
      },
      "additionalProperties": false
    }
  }
}
```

---

## Appendix: Pattern Quick Reference

| Pattern Type | Syntax | Example | Matches |
|--------------|--------|---------|---------|
| Literal string | `"text"` | `"if"` | Exact text "if" |
| Literal array | `["a", "b"]` | `["+", "-"]` | "+" or "-" |
| Character range | `{ "range": ["x", "y"] }` | `{ "range": ["0", "9"] }` | Any digit |
| Token reference | `{ "$token": "name" }` | `{ "$token": "digit" }` | Token of type `digit` |
| Mixed array | `[..., ...]` | `[{ "range": ["a","z"] }, "_"]` | Lowercase letter or underscore |

---

*This specification defines the v2 JSON schema for MetaParser definition files. The schema prioritizes simplicity while providing full expressiveness for token definitions.*
