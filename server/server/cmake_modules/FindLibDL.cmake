#                                               -*- cmake -*-
#
#  FindLibdl.cmake: Try to find Libdl
#
#  (C) Copyright 2005-2011 EDF-EADS-Phimeca
#
#  This library is free software; you can redistribute it and/or
#  modify it under the terms of the GNU Lesser General Public
#  License as published by the Free Software Foundation; either
#  version 2.1 of the License.
#
#  This library is distributed in the hope that it will be useful
#  but WITHOUT ANY WARRANTY; without even the implied warranty of
#  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
#  Lesser General Public License for more details.
#
#  You should have received a copy of the GNU Lesser General Public
#  License along with this library; if not, write to the Free Software
#  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307 USA
#
#  @author: $LastChangedBy: dutka $
#  @date:   $LastChangedDate: 2010-02-04 16:44:49 +0100 (Thu, 04 Feb 2010) $
#  Id:      $Id: Makefile.am 1473 2010-02-04 15:44:49Z dutka $
#
#
# - Try to find Libdl
# Once done this will define
#
#  LIBDL_FOUND - System has Libdl
#  LIBDL_INCLUDE_DIR - The Libdl include directory
#  LIBDL_LIBRARIES - The libraries needed to use Libdl
#  LIBDL_DEFINITIONS - Compiler switches required for using Libdl
CMAKE_MINIMUM_REQUIRED(VERSION 2.8)
IF (LIBDL_INCLUDE_DIR AND LIBDL_LIBRARIES)
   # in cache already
   SET(Libdl_FIND_QUIETLY TRUE)
ENDIF (LIBDL_INCLUDE_DIR AND LIBDL_LIBRARIES)

#IF (NOT WIN32)
#   # use pkg-config to get the directories and then use these values
#   # in the FIND_PATH() and FIND_LIBRARY() calls
#   FIND_PACKAGE(PkgConfig)
#   PKG_CHECK_MODULES(PC_LIBDL libdl)
#   SET(LIBDL_DEFINITIONS ${PC_LIBDL_CFLAGS_OTHER})
#ENDIF (NOT WIN32)

FIND_PATH(LIBDL_INCLUDE_DIR dlfcn.h
   HINTS
   ${LIBDL_INCLUDEDIR}
   ${PC_LIBXML_INCLUDE_DIRS}
   PATH_SUFFIXES libdl
   )

FIND_LIBRARY(LIBDL_LIBRARIES NAMES dl libdl
   HINTS
   ${PC_LIBDL_LIBDIR}
   ${PC_LIBDL_LIBRARY_DIRS}
   )

INCLUDE(FindPackageHandleStandardArgs)

# handle the QUIETLY and REQUIRED arguments and set LIBDL_FOUND to TRUE if 
# all listed variables are TRUE
FIND_PACKAGE_HANDLE_STANDARD_ARGS(Libdl DEFAULT_MSG LIBDL_LIBRARIES LIBDL_INCLUDE_DIR)

MARK_AS_ADVANCED(LIBDL_INCLUDE_DIR LIBDL_LIBRARIES)


