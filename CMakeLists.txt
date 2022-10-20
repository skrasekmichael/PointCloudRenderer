cmake_minimum_required(VERSION 3.19)

project(PointCloudRenderer)

add_subdirectory(libs/SDL-release-2.24.1)

add_executable(${PROJECT_NAME} src/main.cpp)

target_link_libraries(${PROJECT_NAME} PUBLIC SDL2::SDL2-static)
