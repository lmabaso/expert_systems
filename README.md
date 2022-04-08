# Expert-System

About
-----
>This project requires us to create an expert system for calculating proposals, in other words a program that can reason about a set of rules and initial facts in order to deduce certain other facts.

Installation
------------
Run `Makefile`

Usage
-----
`mono expert_system.cs [-h] [-q] [-i] file`
* -h: Show help message and exit
* -q: Remove all output except result
* -i: Enable interative mode

### Example
```
A | B + C => E
(F | G) + H => E

?E

#=A #, E should be true.
#=B #, E should be false.
#=C #, E should be false.
#=AC #, E should be true.
#=BC #, E should be true.

#=F #, E should be false.
#=G #, E should be false.
#=H #, E should be false.
#=FH #, E should be true.
=GH #, E should be true.
```

##### Project done in 2018
