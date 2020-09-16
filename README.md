# DependencyResolver

DependencyResolver is a small tool that analyzes software package dependencies. A system with package
dependencies is described like this:

2\
A,1\
B,1\
3\
A,1,B,1\
A,2,B,2\
C,1,B,1\

The first line is the number (N) of packages to install.\
● The next N lines are packages to install. These are in the form p,v where p is a
package and v is a version that needs to be installed.\
● The next line is the number (M) of dependencies\
● The following M lines are of the form p1,v1,p2,v2 indicating that package p1 in
version v1 depends on p2 in version v2.\
● Packages and version are guaranteed to not contain ',' characters\

## Solution

The solution is composed of three projects:

### DependencyResolver.CLI
Provides a command that users can run from command line.

### DependencyResolver.Application
Application logic.

### DependencyResolver.Infrastructure
Provides access to external dependencies such us the file system. 


## Additionally...

A test project has been included to verify correct behavior. The implementation
has been done following TDD. 

## Author

* **Cristian Perez Matturro** 

Check out my web site: https://crperez.dev
