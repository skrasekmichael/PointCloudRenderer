#include <Window.h>
#include <Services/ArgumentParser.h>

#undef main

int main(int argc, char *argv[])
{
	ArgumentParser parser;
	auto isParsed = parser.parse(argc, argv);
	if (isParsed != 0)
		return isParsed;

	if (parser.args.type == RenderType::None)
		return 0;

	auto window = Window();
	return window.mainLoop();
}
