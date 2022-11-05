cmake_minimum_required(VERSION 3.19)

project(PointCloudRenderer VERSION 1.0)

set_property(GLOBAL PROPERTY USE_FOLDERS ON)

macro(InstallLib name url)
	message("-- Installing ${name}...")
	if (WIN32)
		execute_process(
			COMMAND pwsh.exe -nop -f ${CMAKE_CURRENT_SOURCE_DIR}/scripts/install.ps1 -Name ${name} -Url ${url}
			WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/libs
		)
	endif()
	message("--")

	add_subdirectory(${CMAKE_CURRENT_SOURCE_DIR}/libs/${name} EXCLUDE_FROM_ALL)
endmacro()

InstallLib(SDL2 https://github.com/libsdl-org/SDL/archive/refs/tags/release-2.24.1.zip)
InstallLib(glm https://github.com/g-truc/glm/releases/download/0.9.9.8/glm-0.9.9.8.zip)
InstallLib(CLI11 https://github.com/CLIUtils/CLI11/archive/refs/tags/v2.3.1.zip)
InstallLib(geGL https://github.com/dormon/geGL/archive/5464a364329982d5d5b04574e16458581cbeb806.zip)

macro(GroupSources dir)
	file(GLOB children RELATIVE ${CMAKE_CURRENT_SOURCE_DIR}/${dir} ${CMAKE_CURRENT_SOURCE_DIR}/${dir}/*)
	foreach(child ${children})
			if(IS_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/${dir}/${child})
				GroupSources(${dir}/${child})
			else()
				string(REPLACE "/" "\\" group_name ${dir})
				string(REPLACE "src" "Sources" group_name ${group_name})
				source_group(${group_name} FILES ${CMAKE_CURRENT_SOURCE_DIR}/${dir}/${child})
			endif()
	endforeach()
endmacro()

configure_file(src/PointCloudRendererConfig.h.in ${CMAKE_CURRENT_SOURCE_DIR}/src/PointCloudRendererConfig.h FILE_PERMISSIONS OWNER_READ)

GroupSources(src)
file(GLOB_RECURSE SOURCES CONFIGURE_DEPENDS
	"src/*.h"
	"src/*.cpp"
)

add_executable(${PROJECT_NAME} ${SOURCES})

target_link_libraries(${PROJECT_NAME} PUBLIC SDL2::SDL2-static geGL::geGL glm CLI11::CLI11)
target_include_directories(${PROJECT_NAME} PUBLIC $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/src>)
