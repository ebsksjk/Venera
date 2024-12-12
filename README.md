<img src="https://th.bing.com/th/id/OIG2.NKZTAfcTfxiSzuUPiUPP?pid=ImgGn" width="175">

*(DALL-E generated)*

> Looking for our Sputnik Proxy? [Here is the source code](https://github.com/Mondei1/sputnik).

# Venera

A take on operating systems, using CosmOS. 

For instructions on how to compile executables for Venera, see VoPo/CompileGoodies/Readme.md.


## Processes
Processes are de facto ELF executables. They are loaded into memory und executed. 
Processes are registered in a process table, which is a list of processes internally.
To access the list of runned processes, you can either use 'faketop', or read 0:\Venera\Sys\PT or each individual process file in 0:\Venera\Sys\proc\.

PT and the files in 0:\Venera\Sys\proc\ are representative of every elf file run since booting, as there is no multitasking yet.
The files have the following format:
```
<PID> <name> <entrypoint> <path of executable> <start of process>
```

All entries are seperated by a space. Each Line is a new process.

## Dependencies
Venera requires three dependencies which are copied into this repository.

### [CosmosTTF](https://github.com/GoldenretriverYT/CosmosTTF) by GoldenretriverYT
Mostly left as-is.

<details>

<summary>License (MIT)</summary>

```
MIT License

Copyright (c) 2022 GoldenretriverYT

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
</details>

### [CosmosELF](https://github.com/Guillermo-Santos/CosmosELF/tree/master) by Guillermo-Santos
Heavily adapted and modified.

<details>

<summary>License (MIT)</summary>

```
MIT License

Copyright (c) 2018 Emile Badenhorst

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
</details>

### [CosmosLanguageIntegratedQuery](https://github.com/Samma2009/CosmosLanguageIntegratedQuery/tree/main) by Samma2009
Unchanged.

<details>

<summary>License (MIT)</summary>

```
MIT License

Copyright (c) 2024 Samma - PixelStudio

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

</details>