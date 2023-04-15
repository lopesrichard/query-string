# QueryString

Parse a html query component into a [JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject)

## Usage

```csharp
JsonObject parsed = QueryString.Parse(query);
```

### Examples

#### 1. Single parameter

```csharp
JsonObject parsed = QueryString.Parse("lorem=ipsum");
Console.WriteLine(JsonSerializer.Serialize(parsed));
```

#### Output

```json
{
  "lorem": "ipsum"
}
```

#### 2. Beginning with question mark

```csharp
JsonObject parsed = QueryString.Parse("?lorem=ipsum");
Console.WriteLine(JsonSerializer.Serialize(parsed));
```

#### Output

```json
{
  "lorem": "ipsum"
}
```

#### 3. Multiple parameters

```csharp
JsonObject parsed = QueryString.Parse("lorem=ipsum&dolor=sit");
Console.WriteLine(JsonSerializer.Serialize(parsed));
```

#### Output

```json
{
  "lorem": "ipsum",
  "dolor": "sit"
}
```

#### 4. Array parameter

```csharp
JsonObject parsed = QueryString.Parse("lorem[]=ipsum&lorem[]=dolor");
Console.WriteLine(JsonSerializer.Serialize(parsed));
```

#### Output

```json
{
  "lorem": ["ipsum", "dolor"]
}
```

#### 5. Array parameter with numeric index

```csharp
JsonObject parsed = QueryString.Parse("lorem[4]=ipsum&lorem[6]=dolor");
Console.WriteLine(JsonSerializer.Serialize(parsed));
```

#### Output

```json
{
  "lorem": [null, null, null, null, "ipsum", null, "dolor"]
}
```

#### 6. Object parameter

```csharp
JsonObject parsed = QueryString.Parse("lorem[ipsum]=dolor&lorem[amet]=consectetur");
Console.WriteLine(JsonSerializer.Serialize(parsed));
```

#### Output

```json
{
  "lorem": {
    "ipsum": "dolor",
    "amet": "consectetur"
  }
}
```

#### 7. A more complex example

```csharp
JsonObject parsed = QueryString.Parse("lorem[ipsum][0][dolor][0]=sit&lorem[ipsum][0][elit][]=adipiscing");
Console.WriteLine(JsonSerializer.Serialize(parsed));
```

#### Output

```json
{
  "lorem": {
    "ipsum": [
      {
        "dolor": ["sit"],
        "elit": ["adipiscing"]
      }
    ]
  }
}
```
