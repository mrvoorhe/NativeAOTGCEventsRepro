To Reproduce

1) `dotnet publish`

2) `GCEventsTest\bin\Debug\net7.0\win-x64\publish\GCEventsTest.exe`

3) Output will be
```
Hello, World!
Done
```

This is not expected.

The expected output is
```
Got Event Source
Got Event Source
Hello, World!
Got Event Source
GC - Gen 0
GC End - Gen - 0
GC - Gen 1
GC End - Gen - 1
GC - Gen 0
GC End - Gen - 0
GC - Gen 0
GC End - Gen - 0
GC - Gen 0
GC End - Gen - 0
GC - Gen 2
GC End - Gen - 2
Done
Got Event Source
```

You can see the expected output by running a build that does not use NativeAOT.

```
dotnet run --project GCEventsTest\GCEventsTest.csproj
```

or
```
dotnet publish -p:ShowExpectedBehavior=true --runtime win-x64 --self-contained
GCEventsTest\bin\Debug\net7.0\win-x64\publish\GCEventsTest.exe
```


