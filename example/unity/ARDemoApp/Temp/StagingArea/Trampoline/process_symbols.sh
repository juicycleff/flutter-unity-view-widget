#!/bin/sh

#  process_symbols
#
#  Copyright (c) 2015 Unity Technologies. All rights reserved.

if [ "${SYNCHRONOUS_SYMBOL_PROCESSING}" = "TRUE" ]; then
    "$PROJECT_DIR/usymtool" -symbolPath "$DWARF_DSYM_FOLDER_PATH/$DWARF_DSYM_FILE_NAME"
else
    nohup "$PROJECT_DIR/usymtool" -symbolPath "$DWARF_DSYM_FOLDER_PATH/$DWARF_DSYM_FILE_NAME" > /dev/null 2>&1 &
    disown
fi
