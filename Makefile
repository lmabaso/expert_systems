exec:
	mono expert_systems.exe

compile: \
expert_systems.cs
	@mcs expert_systems.cs
	@echo "compiling"
	@touch compile

clean:
	echo "cleaning"
	@rm *.exe
	@rm compile

re: clean compile
