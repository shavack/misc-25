# C# 8
- readonly instance members - members which do not modify the state of a struct
    `` 
    public readonly double Sum()
    {
        return X + Y;
    }
    ``
- default interface methods - it's possible to declare bodies of methods in interfaces
- pattern matching **is**, **switch**, **and**, **or**, **not** - new keywords for expressions and new switch statement/expression 
    - decoration and type patterns
    ```
        int? xNullable = 7;
        int y = 23;
        int? c = 4;
        object yBoxed = y;
        if (xNullable is int a && yBoxed is int b && c is not null)
        {
            Console.WriteLine(a + b);  // output: 30
        }
    ```
    - constant, relational & logical pattern 
    ```
        static string Classify(double measurement) => measurement switch
        {

            -40.0 => "Too low",
            >= -40.0 and < 0 => "Low",
            0 => "Zero"
            > 0 and < 10.0 => "Acceptable",
            >= 10.0 and < 20.0 => "High",
            >= 20.0 => "Too high",
            double.NaN => "Unknown",
        };
    ```
    - precedence and order checking
    ```
        static bool IsLetter(char c) => c is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z');
    ```
    - property pattern
    ```
        static bool IsConferenceDay(DateTime date) => date is { Year: 2020, Month: 5, Day: 19 or 20 or 21 };
    ```
    - parenthesized pattern - can use parenthesis with logical patterns
    ```
        if (input is not (float or double))
    ```
    - list pattern - can compare lists like this:
    ```
        int[] numbers = { 1, 2, 3 };
        Console.WriteLine(numbers is [1, 2, 3]); 
        Console.WriteLine(numbers is [0 or 1, <= 2, >= 3])   
    ```

- new using without block - no need to add {} to usings anymore, dispose is triggered at the end of the range (method/block etc.)
    - before   
    ```
        using (var stream = new FileStream("plik.txt", FileMode.Open))
            {
            } 
    ```
    - after
    ```
        using var stream = new FileStream("plik.txt", FileMode.Open);
    ```
- static local functions - a static function within function can be added
    ```
    public static int LocalFunctionFactorial(int n)
    {
        return nthFactorial(n);

        int nthFactorial(int number) => number < 2 
            ? 1 
            : number * nthFactorial(number - 1);
    }
    ```
- nullable reference types - can do string? to indicate nullable and it will be fine for compiler
- asynchronous streams - _await foreach_ can be used to consume asynchronous stream of data
    ```
    await foreach (var item in GenerateSequenceAsync())
    {
        Console.WriteLine(item);
    }
    ```
- Range operator _.._ - an operator which can be used with arrays to indicate a range of elements
    ```
        int[] oneThroughTen =[ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        Write(oneThroughTen, ..); //      0..^0:      1, 2, 3, 4, 5, 6, 7, 8, 9, 10
        Write(oneThroughTen, ..3); //      0..3:       1, 2, 3
        Write(oneThroughTen, 2..); //      2..^0:      3, 4, 5, 6, 7, 8, 9, 10
        Write(oneThroughTen, 3..5); //      3..5:       4, 5
        Write(oneThroughTen, ^2..); //      ^2..^0:     9, 10
        Write(oneThroughTen, ..^3); //      0..^3:      1, 2, 3, 4, 5, 6, 7
        Write(oneThroughTen, 3..^4); //      3..^4:      4, 5, 6
        Write(oneThroughTen, ^4..^2); //      ^4..^2:     7, 8
    ```
- null-coalescing assignlem - new operator _??=_ assigning value from right to left only if left is null
- string interpolation using _$_ - just like in javascript
    ```
    Console.WriteLine($"Hello, {name}! Today is {date.DayOfWeek}, it's {date:HH:mm} now.");
    ```

# C# 9

# C# 10

# C# 11

# C# 12
