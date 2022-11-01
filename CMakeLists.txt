cmake_minimum_required(VERSION 3.19)

project(PointCloudRenderer VERSION 1.0)

configure_file(src/PointCloudRendererConfig.h.in ${PROJECT_SOURCE_DIR}/src/PointCloudRendererConfig.h)

if (WIN32)
	execute_process(COMMAND pwsh.exe -nop -f install.ps1 WORKING_DIRECTORY ${PROJECT_SOURCE_DIR}/libs)
endif()

add_subdirectory(libs/SDL-release-2.24.1)
add_subdirectory(libs/geGL-master)
add_subdirectory(libs/glm)
add_subdirectory(libs/CLI11-2.3.1)

SET(SOURCES
	src/main.cpp
	src/Window.cpp
	src/services/ArgumentParser.cpp
)

SET(INCLUDES
	src/Window.h
	src/services/ArgumentParser.h
	src/PointCloudRendererConfig.h
)

add_executable(${PROJECT_NAME} ${SOURCES} ${INCLUDES})

target_link_libraries(${PROJECT_NAME} PUBLIC SDL2::SDL2-static geGL::geGL glm CLI11::CLI11)
target_include_directories(${PROJECT_NAME} PUBLIC $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/src>)

add_custom_target(run ./${PROJECT_NAME})
