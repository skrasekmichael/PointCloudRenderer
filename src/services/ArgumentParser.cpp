#include <Services/ArgumentParser.h>

using namespace PCR;

ArgumentParser::ArgumentParser()
{
	std::string ver = POINT_CLOUD_RENDERER_VERSION;
	this->app.set_version_flag("-v,--version", ver);

	this->app.add_option("-f,--file", this->args.file, "Path to '.pcd' file.")
		->required()
		->each([this](auto _) { this->args.type = RenderType::Point; });

	std::map<std::string, RenderType> string2RenderType {
		{ "point", RenderType::Point }
	};

	this->app.add_option("-t,--type", this->args.type, "Render type.")
		->transform(CLI::CheckedTransformer(string2RenderType, CLI::ignore_case));
}

int ArgumentParser::parse(int argc, char *argv[])
{
	try
	{
		this->app.parse(argc, argv);
		return 0;
	}
	catch (const CLI::ParseError& e)
	{
		return this->app.exit(e);
	}
}
