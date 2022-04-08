# Expert-System

About
-----
>This project requires us to create an expert system for calculating proposals, in other words a program that can reason about a set of rules and initial facts in order to deduce certain other facts.

Installation
------------
Run `Makefile`

Usage
-----
`python3 expert_system.py [-h] [-q] [-i] file`
* -h: Show help message and exit
* -q: Remove all output except result
* -i: Enable interative mode

### Example
```
> python3 expert_system.py tests/basic/basic
A + B => C

=AB
?C
C:True
```

##### Project done in 2018
