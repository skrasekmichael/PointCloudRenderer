cmake_minimum_required(VERSION 3.19)

project(PointCloudRenderer)

execute_process(COMMAND pwsh.exe -nop -f install.ps1 WORKING_DIRECTORY ${PROJECT_SOURCE_DIR}/libs)

add_subdirectory(libs/SDL-release-2.24.1)
add_subdirectory(libs/geGL-master)
add_subdirectory(libs/glm)

add_executable(${PROJECT_NAME} src/main.cpp)

target_link_libraries(${PROJECT_NAME} PUBLIC SDL2::SDL2-static geGL::geGL glm)
target_include_directories(${PROJECT_NAME} PUBLIC $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}>)

add_custom_target(run ./${PROJECT_NAME})
