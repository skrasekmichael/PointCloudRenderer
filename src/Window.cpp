#include <Window.h>

int Window::mainLoop()
{
	if (SDL_Init(SDL_INIT_VIDEO) != 0)
	{
		std::cout << "SDL_Init Error: " << SDL_GetError() << std::endl;
		return 1;
	}

	auto window_ptr = SDL_CreateWindow("Point Cloud Renderer", 100, 100, 800, 600, SDL_WINDOW_OPENGL);

	if (window_ptr == nullptr)
	{
		std::cout << "SDL_CreateWindow Error: " << SDL_GetError() << std::endl;
		SDL_Quit();
		return 1;
	}

	SDL_GL_SetAttribute(SDL_GL_CONTEXT_MAJOR_VERSION, 4);
	SDL_GL_SetAttribute(SDL_GL_CONTEXT_MINOR_VERSION, 6);
	SDL_GL_SetAttribute(SDL_GL_CONTEXT_PROFILE_MASK, SDL_GL_CONTEXT_PROFILE_CORE);

	auto context = SDL_GL_CreateContext(window_ptr);

	auto running = true;

	while (running)
	{
		SDL_Event event;
		while (SDL_PollEvent(&event))
		{
			if (event.type == SDL_QUIT)
				running = false;
		}

		SDL_GL_SwapWindow(window_ptr);
	}

	SDL_GL_DeleteContext(context);
	SDL_DestroyWindow(window_ptr);

	return 0;
}
