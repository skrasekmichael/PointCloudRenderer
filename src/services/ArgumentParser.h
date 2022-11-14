#pragma once

#include <CLI/App.hpp>
#include <CLI/Formatter.hpp>
#include <CLI/Config.hpp>

#include <PointCloudRendererConfig.h>

namespace PCR
{
	enum RenderType : int { None, Point };

	typedef struct {
		RenderType type;
		std::string file;
	} Arguments;

	class ArgumentParser
	{
	private:
		CLI::App app{ "Point Cloud Renderer" };
	public:
		Arguments args{ RenderType::None };

		ArgumentParser();
		int parse(int argc, char* argv[]);
	};
}
