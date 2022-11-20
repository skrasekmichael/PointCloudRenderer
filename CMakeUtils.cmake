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
