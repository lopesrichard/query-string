# QueryString

## Usage

```csharp
JObject parsed = QueryString.Parse(query);
```

### Examples

#### 1. Single parameter

```csharp
JObject parsed = QueryString.Parse("lorem=ipsum");
Console.WriteLine(JsonConvert.SerializeObject(parsed));
```

#### Output

```json
{
  "lorem": "ipsum"
}
```

#### 2. Beginning with question mark

```csharp
JObject parsed = QueryString.Parse("?lorem=ipsum");
Console.WriteLine(JsonConvert.SerializeObject(parsed));
```

#### Output

```json
{
  "lorem": "ipsum"
}
```

#### 3. Multiple parameters

```csharp
JObject parsed = QueryString.Parse("lorem=ipsum&dolor=sit");
Console.WriteLine(JsonConvert.SerializeObject(parsed));
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
JObject parsed = QueryString.Parse("lorem[]=ipsum&lorem[]=dolor");
Console.WriteLine(JsonConvert.SerializeObject(parsed));
```

#### Output

```json
{
  "lorem": ["ipsum", "dolor"]
}
```

#### 5. Array parameter with numeric index

```csharp
JObject parsed = QueryString.Parse("lorem[4]=ipsum&lorem[6]=dolor");
Console.WriteLine(JsonConvert.SerializeObject(parsed));
```

#### Output

```json
{
  "lorem": [null, null, null, null, "ipsum", null, "dolor"]
}
```

#### 6. Object parameter

```csharp
JObject parsed = QueryString.Parse("lorem[ipsum]=dolor&lorem[amet]=consectetur");
Console.WriteLine(JsonConvert.SerializeObject(parsed));
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
JObject parsed = QueryString.Parse("lorem[ipsum][0][dolor][0]=sit&lorem[ipsum][0][elit][]=adipiscing");
Console.WriteLine(JsonConvert.SerializeObject(parsed));
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
