# - Find CURL
# Find the native CURL includes and library
#
#  curl - where to find mysql.h, etc.
#  curl   - List of libraries when using .
#  curl       - True if CURL found.
CMAKE_MINIMUM_REQUIRED(VERSION 2.8)
IF (CURL_INCLUDE_DIR)
  # Already in cache, be silent
  SET(CURL_FIND_QUIETLY TRUE)
ENDIF (CURL_INCLUDE_DIR)

FIND_PATH(CURL_INCLUDE_DIR curl.h
  /usr/local/include/curl
  /usr/include/curl
  /usr/local/curl
)

SET(CURL_NAMES curl)
FIND_LIBRARY(CURL_LIBRARY
  NAMES ${CURL_NAMES}
  PATHS /usr/lib /usr/lib/curl /usr/local/lib /usr/local/curl/lib /usr/local/lib/curl
)

IF (CURL_INCLUDE_DIR AND CURL_LIBRARY)
  SET(CURL_FOUND TRUE)
  SET( CURL_LIBRARIES ${CURL_LIBRARIES} )
ELSE (CURL_INCLUDE_DIR AND CURL_LIBRARY)
  SET(CURL_FOUND FALSE)
  SET( CURL_LIBRARIES )
ENDIF (CURL_INCLUDE_DIR AND CURL_LIBRARY)

IF (CURL_FOUND)
  IF (NOT CURL_FIND_QUIETLY)
    MESSAGE(STATUS "Found CURL: ${CURL_LIBRARY} Path:${CURL_INCLUDE_DIR}")
  ENDIF (NOT CURL_FIND_QUIETLY)
ELSE (CURL_FOUND)
  IF (CURL_REQUIRED)
    MESSAGE(STATUS "Looked for CURL libraries named ${CURL_NAMES}.")
    MESSAGE(FATAL_ERROR "Could NOT find CURL library")
  ENDIF (CURL_REQUIRED)
ENDIF (CURL_FOUND)

MARK_AS_ADVANCED(
  CURL_LIBRARY
  CURL_INCLUDE_DIR
  )

