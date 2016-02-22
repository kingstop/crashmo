# - Find LOG4CXX
# Find the native LOG4CXX includes and library
#
#  LOG4CXX - where to find mysql.h, etc.
#  LOG4CXX   - List of libraries when using .
#  LOG4CXX       - True if LOG4CXX found.
CMAKE_MINIMUM_REQUIRED(VERSION 2.8)
IF (LOG4CXX_INCLUDE_DIR)
  # Already in cache, be silent
  SET(LOG4CXX_FIND_QUIETLY TRUE)
ENDIF (LOG4CXX_INCLUDE_DIR)

FIND_PATH(LOG4CXX_INCLUDE_DIR logger.h
  /usr/local/include/log4cxx
  /usr/include/log4cxx
  /usr/local/log4cxx
)

SET(LOG4CXX_NAMES log4cxx)
FIND_LIBRARY(LOG4CXX_LIBRARY
  NAMES ${LOG4CXX_NAMES}
  PATHS /usr/lib /usr/lib/log4cxx /usr/local/lib /usr/local/log4cxx/lib /usr/local/lib/log4cxx
)

IF (LOG4CXX_INCLUDE_DIR AND LOG4CXX_LIBRARY)
  SET(LOG4CXX_FOUND TRUE)
  SET( LOG4CXX_LIBRARIES ${LOG4CXX_LIBRARIES} )
ELSE (LOG4CXX_INCLUDE_DIR AND LOG4CXX_LIBRARY)
  SET(LOG4CXX_FOUND FALSE)
  SET( LOG4CXX_LIBRARIES )
ENDIF (LOG4CXX_INCLUDE_DIR AND LOG4CXX_LIBRARY)

IF (LOG4CXX_FOUND)
  IF (NOT LOG4CXX_FIND_QUIETLY)
    MESSAGE(STATUS "Found LOG4CXX: ${LOG4CXX_LIBRARY} Path:${LOG4CXX_INCLUDE_DIR}")
  ENDIF (NOT LOG4CXX_FIND_QUIETLY)
ELSE (LOG4CXX_FOUND)
  IF (LOG4CXX_REQUIRED)
    MESSAGE(STATUS "Looked for LOG4CXX libraries named ${LOG4CXX_NAMES}.")
    MESSAGE(FATAL_ERROR "Could NOT find LOG4CXX library")
  ENDIF (LOG4CXX_REQUIRED)
ENDIF (LOG4CXX_FOUND)

MARK_AS_ADVANCED(
  LOG4CXX_LIBRARY
  LOG4CXX_INCLUDE_DIR
  )

